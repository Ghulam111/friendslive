using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class LikesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likerepository;
        public LikesController(IUserRepository userRepository , ILikeRepository likerepository)
        {
            _likerepository = likerepository;
            _userRepository = userRepository;
            
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var logedinUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(logedinUser);
            var sourceUserID = user.Id;

            var LikedUser = await _userRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _likerepository.GetUsersLiked(sourceUserID);
            
            if(LikedUser == null) return NotFound();
            if(sourceUser.UserName == username) return BadRequest("you cannot like yourself");

            var likeUser = await _likerepository.GetUserLike(sourceUserID,LikedUser.Id);

            if( likeUser != null) return BadRequest("already liked this user");

            likeUser = new UserLike {
                SourceUserId = sourceUserID,
                LikedUserId = LikedUser.Id
            };

            sourceUser.LikedUsers.Add(likeUser);

            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("failed to like user");
           



        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LikesDTO>>> getUserLikes([FromQuery]LikeParams likesParams)
        {
            var logedinUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByUsernameAsync(logedinUser);
            likesParams.UserId = user.Id;
            
            var user_likes = await _likerepository.GetUserLikes(likesParams);

            Response.addPaginationHeaders(user_likes.CurrentPage,user_likes.TotalPages,user_likes.TotalCount,user_likes.PageSize);

            return Ok(user_likes);
        }
        
    }
}