﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class RoleEditViewModel
    {
        public RoleEditViewModel()
        {
            Users = new List<string>();
        }
        [Display(Name ="角色Id")]
        public string Id { get; set; }
        [Display(Name ="角色名称")]
        [Required(ErrorMessage ="角色名称是必填的")]
        public string RoleName { get; set; }
        public List<string> Users { get; set; }
    }
}
