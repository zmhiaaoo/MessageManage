using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MessageManage.BLL;

namespace Manage.ViewModel
{
    public class MessageCreateViewModel
    {
        [Display(Name = "标题")]
        [Required(ErrorMessage = "标题不能为空")]
        [MaxLength(50, ErrorMessage = "名字的长度不能超过50个字符")]
        public string Title { get; set; }
        
       public DateTime PublishTime { get; set; }

        [Display(Name="图片")]
        [Required(ErrorMessage ="至少上传一张图片")]
        public List<IFormFile> Photos { get; set; }
    }
}
