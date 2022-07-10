using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);

         Task<bool> SaveAllAsync();

         Task<AppUser> GetUserByIdAsync(int id);

         Task<IEnumerable<AppUser>> GetAllUsersAsync();

         Task<AppUser> GetUserByUsernameAsync(string username);

         Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userparams);

         Task<MemberDTO> getMemberAsync(string username);
         
    }
}