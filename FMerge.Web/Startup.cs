using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AICore.Domain.Service;
using AICore.Utils;
using FMerge.Web.BaseAI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Weixin.MP.TenPayLibV3;

namespace FMerge.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthorization();
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseHttpContextAI();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            ConfigManager.SetAppSettings(Configuration.GetSection("AppSettings"));
            ViData.DMHelper.Instance.ExportMapping();
            RegistWxSDK();
            ThreadStart ts = new ThreadStart(startMerge);
            Thread th = new Thread(ts);
            th.Start();
        }

        void startMerge()
        {
            UserPhotoMergeService x_upService = new UserPhotoMergeService();
            while (true)
            {
                int c = x_upService.PhotoMergeJob();
                if (c == 0)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        /// <summary>
        /// 注册微信支付
        /// </summary>
        private void RegistWxSDK()
        {
            var tenPayV3_MchId = ConfigManager.AppSettings("TenPayV3_MchId");
            var tenPayV3_Key = ConfigManager.AppSettings("TenPayV3_Key");
            var tenPayV3_AppId = ConfigManager.AppSettings("TenPayV3_AppId");
            var tenPayV3_AppSecret = ConfigManager.AppSettings("TenPayV3_AppSecret");
            var tenPayV3_TenpayNotify = ConfigManager.AppSettings("TenPayV3_TenpayNotify");

            var tenPayV3Info = new TenPayV3Info(tenPayV3_AppId, tenPayV3_AppSecret, tenPayV3_MchId, tenPayV3_Key,
                                                tenPayV3_TenpayNotify);
            TenPayV3InfoCollection.Register(tenPayV3Info);
        }
    }
}
