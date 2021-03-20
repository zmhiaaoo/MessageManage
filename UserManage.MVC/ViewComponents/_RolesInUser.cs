using Manage.ViewModel;
using MessageManage.BLL;
using MessageManage.DLL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.ViewComponents
{
    public class _RolesInUser : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageRepository _messageRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        public _RolesInUser(UserManager<ApplicationUser> userManager
            , IMessageRepository messageRepository
            , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _messageRepository = messageRepository;
            _roleManager = roleManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            var roleInUser = await _userManager.GetRolesAsync(user);
            return View(roleInUser);
        }
    }
}
