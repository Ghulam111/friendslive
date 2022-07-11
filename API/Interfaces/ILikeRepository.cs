using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikeRepository
    {
        Task<UserLike> GetUserLike(int SourcUserId, int LikedUserId);

        Task<AppUser> GetUsersLiked(int userId);

        Task<PagedList<LikesDTO>> GetUserLikes(LikeParams likesParams);


        
         
    }
}