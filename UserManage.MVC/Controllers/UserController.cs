using Manage.ViewModel;
using MessageManage.BLL;
using MessageManage.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.Controllers
{
    public class UserController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(IMessageRepository messageRepository,
            IWebHostEnvironment webHostEnvironment,
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> userManager,
            IPhotoRepository photoRepository)
        {
            _messageRepository = messageRepository;
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger;
            _userManager = userManager;
            _photoRepository = photoRepository;
        }
        #region 查看个人页面
        public async Task<IActionResult> UserDetails(string email)
        {
            logger.LogError("错误");
            logger.LogCritical("严重");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"用户不存在，请重试";
                return View("NotFound");
            }
            UserDetailsViewModel userDetailsViewModel = new UserDetailsViewModel()
            {
                Email = user.Email,
                UserId = user.Id,
                ChineseName = user.ChineseName,
                Gender = user.Gender,
                RegisterTime = user.RegisterTime,
                IconPath = user.IconPath,
                Messages = _messageRepository.GetMessagesOfUser(user).ToList(),
            };
            return View(userDetailsViewModel);
        }
        #endregion

        #region 编辑修改个人资料
        //主要是修改，头像和中文名，其他的不变
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            EditUserViewModel editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                ChineseName = user.ChineseName,
                Email = user.Email,
                Gender = user.Gender,
                ExiteIconPath = user.IconPath
            };
            return View(editUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                user.ChineseName = model.ChineseName;
                user.Gender = model.Gender;
                if (model.Icon != null)
                {
                    if (model.ExiteIconPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath,
                            "images", "Icons", model.ExiteIconPath);
                        System.IO.File.Delete(filePath);
                    }
                    user.IconPath = ProcessUploadFile(model);
                }
                await _userManager.UpdateAsync(user);
                return RedirectToAction("UserDetails", new { id = model.Email });
            }
            return View();
        }

        private string ProcessUploadFile(EditUserViewModel model)
        {
            string uniqueFileName = null;
            string uploadsfolfer = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Icons");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Icon.FileName;
            string filePath = Path.Combine(uploadsfolfer, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                model.Icon.CopyTo(fileStream);
            }
            return uniqueFileName;
        }




        #endregion

        //[HttpPost]
    
    }
}

