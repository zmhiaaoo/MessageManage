using MessageManage.DLL;
using MessageManage.BLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Manage.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Manage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(IMessageRepository messageRepository,
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
        #region 网站主页呈现（排序，搜索，筛选）
        [HttpGet]
        //先查询，再筛选，再排序，最后再分页
        public async Task<IActionResult> Index(string searchString, GenderEnum? gender
            , string sorting="Id",int crruentPage=1,int pageCount=10)
        {
            MessageIndexViewModel model = new MessageIndexViewModel();
            model.SearchString = searchString;
            model.Gender = gender;
            model.Sorting = sorting;
            model.CurrentPage = crruentPage;
            model.PageCount = pageCount;
            var query = _messageRepository.GetAllMessagesToQuery();
            //查询
            if (!string.IsNullOrEmpty(searchString))
            {
                string crruentString = searchString.Trim();
                query = query.Where(s => s.Title.Contains(searchString));
            }
            //过滤
            if (gender != null)
            {
                List<Message> result = new List<Message>();
                foreach (var item in query)
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(item.ApplicationUserId);
                    if (user.Gender == gender)
                    {
                        result.Add(item);
                    }
                }
                query = result.AsQueryable();
            }
            //排序
            switch (sorting)
            {
                case "Date":
                    query = query.OrderBy(s => s.PublishTime);
                    break;
                case "Date_Desc":
                    query = query.OrderByDescending(s => s.PublishTime);
                    break;
                case "Id":
                    query = query.OrderBy(s => s.Id);
                    break;
                default:
                    query = query.OrderByDescending(s => s.Id);
                    break;
            }
            //分页
            query = query.Skip((model.CurrentPage - 1) * pageCount)
                .Take(pageCount);
            model.Messages = query.AsNoTracking().ToList();
            return View(model);
        }
        #endregion

        #region 新建消息页面
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MessageCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                //获取当前用户的用户名
                ClaimsPrincipal currentUser = this.User;
                ApplicationUser applicationUser = await
                    _userManager.FindByNameAsync(currentUser.Identity.Name);
                //var currentUser = _userManager.GetUserAsync(HttpContext.User);
                Message newMessage = new Message();
                {
                    //ApplicationUser applicationUser = new ApplicationUser();
                    //applicationUser.UserName = currentUser.Identity.Name;
                }
                newMessage.ApplicationUserId = applicationUser.Id;
                newMessage.Title = model.Title;
                newMessage.PublishTime = DateTime.Now;
                _messageRepository.Insert(newMessage);
                foreach (var item in model.Photos)
                {
                    Photo photo = new Photo
                    {
                        MessageId = newMessage.Id,
                    };
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Users");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    item.CopyTo(new FileStream(filePath, FileMode.Create));
                    photo.PhotoPath = uniqueFileName;
                    _photoRepository.Insert(photo);
                    newMessage.Photos.Add(photo);
                }
                _messageRepository.Update(newMessage);
                applicationUser.Messages.Add(newMessage);
                await _userManager.UpdateAsync(applicationUser);
                return RedirectToAction("Single", new { Id = newMessage.Id });
            }
            return View();

        }
        #endregion

        #region 单个消息图片展示
        public IActionResult Single(int id)
        {
            Message message = _messageRepository.GetMessageById(id);
            return View(message);
        }
        #endregion

        #region 重新编辑消息
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Message message = _messageRepository.GetMessageById(id);
            IEnumerable<Photo> photos = _photoRepository.GetAllPhotosOfMessageId(id);
            if (message == null)
            {
                ViewBag.ErrorMessage = $"信息不存在，请重试";
                return View("NotFound");
            }
            MessageEditViewModel messageEditViewModel = new MessageEditViewModel
            {
                Id = message.Id,
                Title = message.Title,
                PublishTime = message.PublishTime,

            };
            messageEditViewModel.ExistingPhotoPath = new List<string>();
            foreach (var photo in photos)
            {
                messageEditViewModel.ExistingPhotoPath.Add(photo.PhotoPath);
            }
            return View(messageEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(MessageEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Message message = _messageRepository.GetMessageById(model.Id);

                message.Title = model.Title;
                message.PublishTime = DateTime.Now;
                IEnumerable<Photo> Photos = _photoRepository.GetAllPhotosOfMessageId(model.Id);

                foreach (var photo in Photos)
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath
                                           , "images", "Users", photo.PhotoPath);
                    System.IO.File.Delete(filePath);
                }
                _photoRepository.DeleteAllPhotosOfMessage(model.Id);
                string uniqueFileName = null;
                foreach (var item in model.Photos)
                {
                    Photo photo = new Photo
                    {
                        MessageId = model.Id,
                    };
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "Users");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    item.CopyTo(new FileStream(filePath, FileMode.Create));
                    photo.PhotoPath = uniqueFileName;
                    _photoRepository.Insert(photo);
                    message.Photos.Add(photo);
                }
                _messageRepository.Update(message);
                return RedirectToAction("Single", new { id = model.Id });
            }
            return View(model);
        }

        #endregion

        [HttpPost]
        public IActionResult DeteleMessage(int id)
        {
            string userId = _messageRepository.GetMessageById(id).ApplicationUserId;
            _messageRepository.Delete(id);
            return RedirectToAction("UserDetails", "User", userId);
        }
    }
}
