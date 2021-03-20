using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageManage.DAL;
using MessageManage.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace StudentManage
{
    public class Startup
    {
        //若要访问Startup类中的配置信息，则需要在其中注入
        //IConfiguration服务
        //添加一个构造函数，然后将IConfiguration服务注入方法中
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region 注入服务
        public void ConfigureServices(IServiceCollection services)
        {
            //连接字符串的注入
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("ManageDBConnection"))
                );

            //视图控制服务注入
            services.AddControllersWithViews(config =>
            {
                //全局应用Authorize属性
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            //依赖注入接口实现方式
            services.AddScoped<IMessageRepository, SQLMessageRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            //添加角色认证服务
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<AppDbContext>();

            //密码验证更改
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            //拒绝访问重定向
            services.ConfigureApplicationCookie(options =>
            { options.AccessDeniedPath = new PathString("/Admin/AccessDenied"); });

        }
        #endregion

        #region 配置管道中间件
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //IWebHostEnvironment 感觉很有用
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT"))
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
            app.UseDataInitializer();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
        #endregion
    }
}
