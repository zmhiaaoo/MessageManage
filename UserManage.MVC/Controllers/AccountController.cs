using Manage.ViewModel;
using MessageManage.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;



namespace Manage.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AccountController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
             ILogger<AccountController> logger,
             IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        #region 注册
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                string unloadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images","Icons");
                if (model.Icon == null)
                {
                    uniqueFileName = "image1";
                }
                else
                {
                    if (model.Icon.ContentType != $"image/png")
                    {
                        ModelState.AddModelError("Icon", "请上传PNG格式图片文件");
                    }
                    else
                    {
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Icon.FileName;
                        string filePath = Path.Combine(unloadsFolder, uniqueFileName);
                        model.Icon.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                };
                var user = new ApplicationUser
                {
                    //登录用啥登录就等于啥
                    UserName = model.Email,
                    ChineseName = model.ChineseName,
                    Email = model.Email,
                    Gender = model.Gender,
                    IconPath = uniqueFileName,
                    RegisterTime = DateTime.Now
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, "未注册成功");
                }
            }
            return View(model);
        }

        [AcceptVerbs("Get", "Post", Route = "Register")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"邮箱：{email}已经被注册使用了");
            }
        }
        [AcceptVerbs("Get", "Post", Route = "Register")]
        public async Task<IActionResult> IsNameInUse(string chineseName)
        {
            var user = await userManager.FindByNameAsync(chineseName);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"用户名：{chineseName}已经被使用了");
            }
        }
        #endregion

        #region 登录与登出


        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);

            if (ModelState.IsValid)
            {
                var rig = await userManager.CheckPasswordAsync(user, model.Password);

                var result = await signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, true);
                if (result.Succeeded)
                {

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "登录失败，请重试");
            }
            return View(model);
        }

        [AcceptVerbs("Get", "Post", Route = "Login")]
        public async Task<IActionResult> EmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json($"邮箱：{email}不存在");
            }
            else
            {
                return Json(true);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        #endregion



    }
}
