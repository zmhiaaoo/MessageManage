
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageManage.BLL
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime RegisterTime { get; set; }
      
        public string IconPath { get; set; }
        public IList<Message> Messages { get; set; }
        public string ChineseName { get; set; }
    }
}
