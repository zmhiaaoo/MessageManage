using MessageManage.BLL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MessageManage.DLL
{
    public static class DataInitializer
    {
        public static IApplicationBuilder UseDataInitializer(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                #region 添加用户和管理员角色，并将两者绑定
                if (dbContext.Users.Any())
                {
                    return builder;
                }
                var user = new ApplicationUser
                {
                    Email = "1019680180@qq.com",
                    UserName = "1019680180@qq.com",
                    Gender = GenderEnum.male,
                    RegisterTime = DateTime.Now,
                    IconPath = "image1.png",
                    ChineseName = "赵淼"
                };
                userManager.CreateAsync(user, "123456").Wait();
                dbContext.SaveChanges();
                var adminRole = "admin";
                var role = new IdentityRole { Name = adminRole };
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();

                dbContext.UserRoles.Add(new IdentityUserRole<string>
                {
                    RoleId = role.Id,
                    UserId = user.Id
                });
                dbContext.SaveChanges();
                #endregion

                #region 添加消息,用户就是站长本人
                if (dbContext.Messages.Any())
                {
                    return builder;
                }
                var messages = new[]
                {
                    new Message
                    {
                        Title = "种子数据-网站未来需要补充的功能",
                        PublishTime = DateTime.Now,
                        ApplicationUserId = user.Id
                    },
                    new Message
                    {
                        Title = "种子数据-关于.net5的一些截图",
                        PublishTime = DateTime.Now,
                        ApplicationUserId = user.Id
                    }
                };
                foreach (var item in messages)
                {
                    dbContext.Messages.Add(item);
                }
                dbContext.SaveChanges();
                #endregion

                #region 添加消息中的照片
                if (dbContext.Photos.Any())
                {
                    return builder;
                }
                var photos = new[]
                {
                    new Photo
                    {
                        PhotoPath="后续.png",
                        MessageId=messages[0].Id,
                    },
                     new Photo
                    {
                        PhotoPath="net50.png",
                        MessageId=messages[1].Id,
                    },
                      new Photo
                    {
                        PhotoPath="net51.png",
                        MessageId=messages[1].Id,
                    }
                };
                foreach (var item in photos)
                {
                    dbContext.Photos.Add(item);
                }
                dbContext.SaveChanges();
                #endregion

            }
            return builder;
        }
    }
}
