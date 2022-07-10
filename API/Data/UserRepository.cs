using API.DTOs;
using API.Entities;
using API.Helpers;
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

        public  async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userparams)
        {
            var query =  _dbcontext.Users
            .AsQueryable();

            query = query.Where(u => u.UserName != userparams.currentUsername);

            var minDob = DateTime.Today.AddYears(-userparams.maxAge -1);
            var maxDob = DateTime.Today.AddYears(-userparams.minAge);

            query = query.Where( u => u.DateofBirth >= minDob && u.DateofBirth <= maxDob);
            
            if(userparams.gender != null)
            {
                query = query.Where( u => u.gender == userparams.gender);
            }

            query = userparams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Createdon),
                _ => query.OrderByDescending( u => u.LastActive) 
            };
            

            return await PagedList<MemberDTO>.CreateAsync(query.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).AsNoTracking()
            
            ,userparams.pageNumber,userparams.PageSize);
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