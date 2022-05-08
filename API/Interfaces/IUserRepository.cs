using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);

         Task<bool> SaveAllAsync();

         Task<AppUser> GetUserByIdAsync(int id);

         Task<IEnumerable<AppUser>> GetAllUsersAsync();

         Task<AppUser> GetUserByUsernameAsync(string username);

         Task<IEnumerable<MemberDTO>> GetMembersAsync();

         Task<MemberDTO> getMemberAsync(string username);
         
    }
}