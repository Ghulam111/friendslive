using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public IUserRepository UserRepository => new UserRepository(_dbContext,_mapper);

        public IMessagesRepository MessageRepository => new MessageRepository(_dbContext,_mapper);

        public ILikeRepository LikeRepository => new LikesRepository(_dbContext);

        public async Task<bool> Complete()
        {
            return await _dbContext.SaveChangesAsync() > 0 ;
        }

        public bool SaveChanges()
        {
            return _dbContext.ChangeTracker.HasChanges();
        }
    }
}