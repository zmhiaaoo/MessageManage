using MessageManage.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManage.DLL
{

    public interface IPhotoRepository
    {
        Photo GetPhotoById(int id);
        IEnumerable<Photo> GetAllPhotosOfMessageId(int messageId);
        IEnumerable<Photo> GetAllPhotosOfMessage(Message message);
        Photo Insert(Photo photo);
        //Message Update(Message updateMessage);
        Photo Delete(int id);
        void DeleteAllPhotosOfMessage(int messageId);
    }
}
