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
        //��Ҫ����Startup���е�������Ϣ������Ҫ������ע��
        //IConfiguration����
        //���һ�����캯����Ȼ��IConfiguration����ע�뷽����
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #region ע�����
        public void ConfigureServices(IServiceCollection services)
        {
            //�����ַ�����ע��
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("ManageDBConnection"))
                );

            //��ͼ���Ʒ���ע��
            services.AddControllersWithViews(config =>
            {
                //ȫ��Ӧ��Authorize����
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            //����ע��ӿ�ʵ�ַ�ʽ
            services.AddScoped<IMessageRepository, SQLMessageRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            //��ӽ�ɫ��֤����
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddEntityFrameworkStores<AppDbContext>();

            //������֤����
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });

            //�ܾ������ض���
            services.ConfigureApplicationCookie(options =>
            { options.AccessDeniedPath = new PathString("/Admin/AccessDenied"); });

        }
        #endregion

        #region ���ùܵ��м��
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //IWebHostEnvironment �о�������
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
