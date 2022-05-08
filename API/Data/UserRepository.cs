using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dbcontext;
        private readonly IMapper _mapper;

        public UserRepository(DataContext dbcontext, IMapper mapper)
        {
            _mapper = mapper;
            _dbcontext = dbcontext;

        }
        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await _dbcontext.Users
            .Include(u => u.photos)
            .ToListAsync();
        }

        public async Task<MemberDTO> getMemberAsync(string username)
        {
            return await _dbcontext.Users
            .Where(u => u.UserName == username)
            .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();

        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _dbcontext.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _dbcontext.Users
            .Include(u => u.photos)
            .SingleOrDefaultAsync(u => u.UserName == username);
        }

        public  async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await _dbcontext.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _dbcontext.Entry(user).State = EntityState.Modified;
        }
    }
}