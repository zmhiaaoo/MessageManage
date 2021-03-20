using MessageManage.BLL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Manage.ViewModel
{
    public class UserDetailsViewModel
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string ChineseName { get; set; }
        public GenderEnum Gender { get; set; }
        public DateTime RegisterTime { get; set; }
        public string IconPath { get; set; }
        public IList<Message> Messages { get; set; }
    }
}
