namespace API.Interfaces
{
    public interface IUnitOfWork
    {
         IUserRepository UserRepository{get;}

         IMessagesRepository MessageRepository{get;}

         ILikeRepository LikeRepository{get;}

        Task<bool> Complete();

        bool SaveChanges();

    }
}