using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MessageManage.BLL
{
   public class Photo
    {
       
        [Key]
        public string PhotoPath { get; set; }
        public int MessageId { get; set; }
        public Message message { get; set; }
    }
}
