using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class LoginViewModel
    {
        [Display(Name ="邮箱")]
        [Required(ErrorMessage = "邮箱地址不能为空")]
        [EmailAddress(ErrorMessage = "请输入正确的邮箱地址")]
        [Remote(action:"EmailInUse",controller:"Account")]
        public string Email { get; set; }
        [Display(Name ="密码")]
        [Required(ErrorMessage = "密码不能空")]
        [DataType(DataType.Password,ErrorMessage = "密码位数至少6位")]
        public string Password { get; set; }
        [Display(Name ="记住我")]
      
        public bool RememberMe { get; set; }
    }
}
