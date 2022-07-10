
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoservice;

        private readonly IUserRepository _IUserrepository;
        public UsersController(IUserRepository IUserrepository, IMapper mapper, IPhotoService photoservice)
        {
            _photoservice = photoservice;
            _mapper = mapper;
            _IUserrepository = IUserrepository;


        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery] UserParams userparams)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _IUserrepository.GetUserByUsernameAsync(username);

            userparams.currentUsername = user.UserName;

            var users = await _IUserrepository.GetMembersAsync(userparams);

           Response.addPaginationHeaders(users.CurrentPage,users.TotalPages,users.TotalCount,users.PageSize);

            return Ok(users);

        }

        // [HttpGet("{id}")]

        // public async Task<ActionResult<AppUser>> GetUser(int id)
        // {

        //     return await _IUserrepository.GetUserByIdAsync(id);


        // }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<MemberDTO> getuserbyUsername(string username)
        {

            return await _IUserrepository.getMemberAsync(username);

        }


        [HttpPut]

        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberupdatemodel)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _IUserrepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberupdatemodel, user);
            _IUserrepository.Update(user);

            if (await _IUserrepository.SaveAllAsync()) return NoContent();

            return BadRequest();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> UploadImage(IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _IUserrepository.GetUserByUsernameAsync(username);

            var result = await _photoservice.UploadImage(file);

            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo {

                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if(user.photos.Count == 0){

                photo.IsMain = true;
            }

            user.photos.Add(photo);

            if(await _IUserrepository.SaveAllAsync()){

               return CreatedAtRoute("GetUser",new {username = user.UserName},_mapper.Map<PhotoDTO>(photo));
                // return _mapper.Map<PhotoDTO>(photo);
            }
            return BadRequest();

        }

        [HttpPut("set-main-photo/{photoId}")]

        public async Task<ActionResult> setmainPhoto(int photoId){

            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _IUserrepository.GetUserByUsernameAsync(username);

            var photo = user.photos.FirstOrDefault(x => x.Id == photoId);

            if(photo.IsMain ) return BadRequest("this photo is already main photo");

            var photoMain = user.photos.FirstOrDefault(u => u.IsMain);
            
            if(photoMain != null){
                photoMain.IsMain = false;
            }
            photo.IsMain = true;

            if(await _IUserrepository.SaveAllAsync()){
                return NoContent();
            }
            return BadRequest("failed to set main photo");

        }

        [HttpDelete("delete-photo/{photoId}")]

        public async Task<ActionResult> deletephoto(int photoId)
        {
              var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
              var user = await _IUserrepository.GetUserByUsernameAsync(username);
              
              var photo = user.photos.FirstOrDefault(x => x.Id == photoId);

              if(photo == null) return BadRequest();

              if(photo.IsMain) return BadRequest("cannot delete main photo");

              var result = await _photoservice.DeleteImage(photo.PublicId);
              if(result.Error != null) return BadRequest(result.Error.Message);

              user.photos.Remove(photo);

              if( await _IUserrepository.SaveAllAsync()){
                  return Ok();
              }

              return BadRequest("some error occured. deletion failed");
        }

    }
}