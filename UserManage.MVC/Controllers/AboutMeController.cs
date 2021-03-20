using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Controllers
{
    [AllowAnonymous]
    //[Authorize(Roles = "站长")]
    public class AboutMeController:Controller
    {
        public IActionResult MyDegree()
        {
            return View();
        }
        public IActionResult MyIdCard()
        {
            return View();
        }
    }
}
