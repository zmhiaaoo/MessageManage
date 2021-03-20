using MessageManage.BLL;
using MessageManage.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage
{
    [AllowAnonymous]
    public class _HotUsers : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageRepository _messageRepository;
        public _HotUsers(UserManager<ApplicationUser> userManager
            , IMessageRepository messageRepository)
        {
            _userManager = userManager;
            _messageRepository = messageRepository;
        }
        //发帖最多的热门用户
        
        public  IViewComponentResult Invoke(int topN)
        {
            List<string> allUserId = new List<string>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            var messages = _messageRepository.GetAllMessages();
            if (messages.Count()==0)
            {
                return View("NoMessage");
            }
            else
            {
                //foreach (var item in messages)
                //{
                //    allUserId.Add(item.ApplicationUserId);
                //}
                //var newAllUserID = allUserId.Distinct();
                //foreach (var item in newAllUserID)
                //{
                //    var user = await _userManager.FindByIdAsync(item);
                //    users.Add(user);
                //}
                //var hotUsers = users.OrderByDescending(a => a.Messages.Count).Take(topN);
             var hotUsers=_userManager.Users.OrderByDescending(a => a.Messages.Count).Take(topN);

                return View("default", hotUsers);
            }
            
        }

    }
}
