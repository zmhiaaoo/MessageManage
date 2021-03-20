using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MessageManage.BLL
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Photo> Photos { get; set; }
        public DateTime PublishTime { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        
       
    }
}
