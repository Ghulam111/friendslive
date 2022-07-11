using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikeRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserLike> GetUserLike(int SourcUserId, int LikedUserId)
        {
            return await _context.Likes.FindAsync(SourcUserId,LikedUserId);
        }

        public async Task<PagedList<LikesDTO>> GetUserLikes(LikeParams likeParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();
            
            if(likeParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likeParams.UserId);
                users  = likes.Select(like => like.LikedUser);
            }
            if(likeParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.LikedUserId == likeParams.UserId);
                users  = likes.Select(like => like.SourceUser);
            }

            var likedUsers =  users.Select( user => new LikesDTO{
                UserName = user.UserName,
                knownAs = user.knownAs,
                photoUrl  = user.photos.FirstOrDefault( p => p.IsMain).Url,
                City = user.City,
                Id = user.Id,
                Age = user.DateofBirth.CalculateAge()
                

            });
            return await PagedList<LikesDTO>.CreateAsync(likedUsers,likeParams.pageNumber,likeParams.PageSize);
        }

        public async Task<AppUser> GetUsersLiked(int userId)
        {
            return await _context.Users
            .Include( u => u.LikedUsers)
            .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}