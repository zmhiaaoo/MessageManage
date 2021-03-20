using MessageManage.BLL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        [MaxLength(10, ErrorMessage = "长度不能超过10个字符")]
        [Display(Name = "用户名")]
        public string ChineseName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public GenderEnum Gender { get; set; }
        public IFormFile Icon { get; set; }
        public string ExiteIconPath { get; set; }
    }
}
