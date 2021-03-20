using MessageManage.BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "邮箱地址不能为空")]
        [EmailAddress(ErrorMessage = "请输入正确的邮箱地址")]
        //[Remote(action: "IsEmailInUse", controller: "Account")]
        [Display(Name = "邮箱地址")]
        public string Email { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        [MaxLength(10, ErrorMessage = "长度不能超过10个字符")]
        [Display(Name = "用户名")]
        //[Remote(action: "IsNameInUse", controller: "Account")]
        public string ChineseName { get; set; }
        [Display(Name = "性别")]
        public GenderEnum Gender { get; set; }

        [Required(ErrorMessage = "密码不能空")]
        [DataType(DataType.Password, ErrorMessage = "密码位数至少6位")]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码与确认密码不一致，请重新输入")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "头像")]
        [Required(ErrorMessage = "请上传头像")]
        public IFormFile Icon { get; set; }
        //[DataType(DataType.Date)]
        //public DateTime RegisterTime { get; set; }
    }
}
