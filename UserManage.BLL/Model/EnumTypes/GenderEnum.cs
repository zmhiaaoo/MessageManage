using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MessageManage.BLL
{
    public enum GenderEnum
    {
        [Display(Name ="男")]
        male,
        [Display(Name = "女")]
        female
    }
}
