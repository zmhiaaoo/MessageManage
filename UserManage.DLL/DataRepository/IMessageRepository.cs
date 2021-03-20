using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageManage.BLL;

namespace MessageManage.DAL

{
    public interface IMessageRepository
    {
        Message GetMessageById(int id);
        IQueryable<Message> GetAllMessagesToQuery();
        IEnumerable<Message> GetMessagesOfUser(ApplicationUser user);
        IEnumerable<Message> GetAllMessages();

        Message Insert(Message message);
        Message Update(Message updateMessage);
        Message Delete(int id);
       
    }
}
