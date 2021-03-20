using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using MessageManage.BLL;


namespace MessageManage.DAL
{
    public class SQLMessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly IPhotoRepository photoRepository;
        private readonly ILogger logger;
        public SQLMessageRepository(AppDbContext context,
            ILogger<SQLMessageRepository> logger,
            IPhotoRepository photoRepository)
        {
            _context = context;
            this.logger = logger;
            this.photoRepository = photoRepository;
        }
        public Message Delete(int id)
        {
            Message message = _context.Messages.Find(id);
            photoRepository.DeleteAllPhotosOfMessage(id);
            if (message != null)
            {
                _context.Messages.Remove(message);
            }
            _context.SaveChanges();
            return message;
        }

        public IEnumerable<Message> GetAllMessages()
        {

            //logger.LogTrace("跟踪");
            //logger.LogDebug("调试");
            //logger.LogInformation("信息");
            //logger.LogWarning("警告");
            logger.LogError("错误");
            logger.LogCritical("严重");
            return _context.Messages;
        }

        public IQueryable<Message> GetAllMessagesToQuery()
        {
            return _context.Messages.AsQueryable();
        }

        public Message GetMessageById(int id)
        {
            Message message = _context.Messages.Find(id);
            return message;
        }

        public IEnumerable<Message> GetMessagesOfUser(ApplicationUser user)
        {
            IEnumerable<Message> allMessages = _context.Messages;
            var userMessages = from s in allMessages
                               where s.ApplicationUserId == user.Id
                               select s;
            return userMessages;
        }

        public Message Insert(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
            return message;
        }

        public Message Update(Message updateMessage)
        {
            var user = _context.Messages.Attach(updateMessage);
            user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateMessage;
        }
    }
}
