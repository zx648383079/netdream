using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NetDream.Web.Areas.Auth.Repositories;
using NetDream.Web.Areas.Blog.Repositories;
using NetDream.Web.Areas.Contact.Repositories;
using NetDream.Web.Areas.Gzo.Repositories;
using NetDream.Web.Areas.Open.Repositories;
using NetDream.Web.Base.Middlewares;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<MvcOptions>(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddScoped<IDatabase>(x =>
            {
                return new Database(Configuration.GetConnectionString("Default"), DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            });
            services.AddSession(options =>
            {
                options.Cookie.Name = ".zodream.session";
                options.IdleTimeout = TimeSpan.FromSeconds(2000);
                options.Cookie.HttpOnly = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Auth");            //��¼·�������ǵ��û���ͼ������Դ��δ���������֤ʱ�����򽫻Ὣ�����ض���������·����
                    o.AccessDeniedPath = new PathString("/Home/Error");     //��ֹ����·�������û���ͼ������Դʱ����δͨ������Դ���κ���Ȩ���ԣ����󽫱��ض���������·����
                    o.SlidingExpiration = true; //Cookie���Է�Ϊ�����Եĺ���ʱ�Եġ� ��ʱ�Ե���ָֻ�ڵ�ǰ�������������Ч�������һ���رվ�ʧЧ���������ɾ������ �����Ե���ָCookieָ����һ������ʱ�䣬�����ʱ�䵽��֮ǰ����cookieһֱ��Ч�������һֱ��¼�Ŵ�cookie�Ĵ��ڣ��� slidingExpriation�������ǣ�ָʾ�������cookie��Ϊ������cookie�洢�����ǻ��Զ����Ĺ���ʱ�䣬��ʹ�û������ڵ�¼��һֱ�������һ��ʱ���ȴ�Զ�ע����Ҳ����˵����10���¼�ˣ������������õ�TimeOutΪ30���ӣ����slidingExpriationΪfalse,��ô10: 30�Ժ���ͱ������µ�¼�����Ϊtrue�Ļ�����10: 16��ʱ����һ����ҳ�棬�������ͻ�֪ͨ��������ѹ���ʱ���޸�Ϊ10: 46��
                });
            RegisterAuthRepositories(services);

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    // ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    // ��ʹ���շ�
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    // ����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // ���ֶ�Ϊnullֵ�����ֶβ��᷵�ص�ǰ��
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddLogging();
            //services.AddWebSocketManager();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                                Path.Combine(Directory.GetCurrentDirectory(), "../../../PHP-ZoDream/html/assets")),
                RequestPath = "/assets"
            });
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthorization();
            app.UseMiddleware<ResponseMiddleware>();

            app.UseAuthorization();

            #region Websocket
            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(120)
            //};
            //app.UseWebSockets(webSocketOptions);
            //app.MapWebSocketManager("/Chat/Ws", serviceProvider.GetService<ChatRoomHandler>());
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                     name: "areaRoute",
                     template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void RegisterAuthRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(UserRepository));
            services.AddScoped(typeof(BlogRepository));
            services.AddScoped(typeof(GzoRepository));
            services.AddScoped(typeof(ContactRepository));
            services.AddScoped(typeof(OpenRepository));
            // services.AddSingleton(typeof(OptionRepository));
        }
    }
}
