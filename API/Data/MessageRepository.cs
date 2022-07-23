using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MessageRepository : IMessagesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MessageRepository(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
            .Include(u => u.Sender)
            .Include( u => u.Recipient)
            .SingleOrDefaultAsync( m => m.Id == id);
        }

      

        public async Task<PagedList<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
        {
            var query = _context.Messages
            .OrderByDescending(u => u.MessageSent).AsQueryable();

            query = messageParams.Container switch
            {
                "Inbox" => query.Where(u => u.RecipietUsername == messageParams.Username
                 && u.RecipientDeleted == false),
                "Outbox" => query.Where(u => u.SenderUsername == messageParams.Username
                 && u.SenderDeleted == false),
                _ => query.Where(u => u.RecipietUsername == messageParams.Username 
                && u.DateRead == null && u.RecipientDeleted == false)
            };

            var message = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);

            return await PagedList<MessageDto>.CreateAsync(message,messageParams.pageNumber,messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessagesThread(string currentUsername,string recipientUsername)
        {
            var messages = await _context.Messages
            .Include(u=> u.Sender).ThenInclude(p => p.photos)
            .Include(u=> u.Recipient).ThenInclude(p => p.photos)
            .Where( m => m.Recipient.UserName == currentUsername && m.RecipientDeleted == false
             && m.Sender.UserName == recipientUsername
            ||  m.Recipient.UserName == recipientUsername && m.Sender.UserName == currentUsername && m.SenderDeleted == false)
            .OrderBy(m => m.MessageSent).ToListAsync();
            
            var unreadMessages = messages.Where( u => u.DateRead == null && u.Recipient.UserName == currentUsername).ToList();


            if(unreadMessages.Any())
            {
                foreach(var message in unreadMessages)
                {
                    message.DateRead = DateTime.Now;
                }

                await _context.SaveChangesAsync();
            }
            return _mapper.Map<IEnumerable<MessageDto>>(messages);
        }

 
    }
}