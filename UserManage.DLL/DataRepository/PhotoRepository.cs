using MessageManage.BLL;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MessageManage.DAL
{

    public class PhotoRepository : IPhotoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger logger;
        public PhotoRepository(AppDbContext context, ILogger<PhotoRepository> logger)
        {
            _context = context;
            this.logger = logger;
        }
        public Photo Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllPhotosOfMessage(int messageId)
        {
            IEnumerable<Photo> allPhotosOfMessage = GetAllPhotosOfMessageId(messageId);
            foreach (var photo in allPhotosOfMessage)
            {
                _context.Photos.Remove(photo);
            }
            _context.SaveChanges();
        }

        public IEnumerable<Photo> GetAllPhotosOfMessage(Message message)
        {
            IEnumerable<Photo> allPhotos = _context.Photos;
            var photos = from s in allPhotos
                         where s.MessageId == message.Id
                         select s;
            return photos;

        }

        public IEnumerable<Photo> GetAllPhotosOfMessageId(int messageId)
        {
            IEnumerable<Photo> allPhotos = _context.Photos;
            var photos = from s in allPhotos
                         where s.MessageId == messageId
                         select s;
            return photos;
        }

        public Photo GetPhotoById(int id)
        {
            throw new NotImplementedException();
        }

        public Photo Insert(Photo photo)
        {
            _context.Photos.Add(photo);
            _context.SaveChanges();
            return photo;
        }
    }
}
