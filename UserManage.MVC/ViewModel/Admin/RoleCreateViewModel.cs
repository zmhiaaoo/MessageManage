﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewModel
{
    public class RoleCreateViewModel
    {
        [Required]
        [Display(Name ="角色")]
        public string RoleName { get; set; }
    }
}
