using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using NetDream.Shared.Interfaces;
using NetDream.Modules.Auth;
using NetDream.Modules.Blog;
using NetDream.Modules.Contact;
using NetDream.Modules.Gzo;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.SEO;
using NetDream.Modules.Note;
using NetDream.Web.Base.Http;
using NetDream.Web.Base.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NPoco;
using System;
using System.IO;

namespace NetDream.Web
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private static readonly string[] _supportedCultures = ["en-US", "zh-CN"];

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
                    o.LoginPath = new PathString("/Auth");            //��¼·�������ǵ��û���ͼ������Դ��δ����������֤ʱ�����򽫻Ὣ�����ض���������·����
                    o.AccessDeniedPath = new PathString("/Home/Error");     //��ֹ����·�������û���ͼ������Դʱ����δͨ������Դ���κ���Ȩ���ԣ����󽫱��ض���������·����
                    o.SlidingExpiration = true; //Cookie���Է�Ϊ�����Եĺ���ʱ�Եġ� ��ʱ�Ե���ָֻ�ڵ�ǰ�������������Ч�������һ���رվ�ʧЧ���������ɾ������ �����Ե���ָCookieָ����һ������ʱ�䣬�����ʱ�䵽��֮ǰ����cookieһֱ��Ч�������һֱ��¼�Ŵ�cookie�Ĵ��ڣ��� slidingExpriation�������ǣ�ָʾ�������cookie��Ϊ������cookie�洢�����ǻ��Զ����Ĺ���ʱ�䣬��ʹ�û������ڵ�¼��һֱ�������һ��ʱ���ȴ�Զ�ע����Ҳ����˵����10���¼�ˣ������������õ�TimeOutΪ30���ӣ����slidingExpriationΪfalse,��ô10: 30�Ժ���ͱ������µ�¼�����Ϊtrue�Ļ�����10: 16��ʱ����һ����ҳ�棬�������ͻ�֪ͨ��������ѹ���ʱ���޸�Ϊ10: 46��
                });
            using (var db = new Database(Configuration.GetConnectionString("Default"), DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance))
            {
                RegisterGlobeRepositories(db, services);
            } 
            RegisterRepositories(services);
            // ���ػ�
            services.Configure<RequestLocalizationOptions>(options => {
                options.SetDefaultCulture(_supportedCultures[0])
                    .AddSupportedCultures(_supportedCultures)
                    .AddSupportedUICultures(_supportedCultures)
                    .AddInitialRequestCultureProvider(new AcceptLanguageHeaderRequestCultureProvider());
            });
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            
            services.AddMvc()
                .AddViewLocalization()
                .AddNewtonsoftJson(options =>
                {
                    // ѭ������
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    // ��ʹ���շ�
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    // ����ʱ���ʽ
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // ���ֶ�Ϊ nullֵ�����ֶβ��᷵�ص�ǰ��
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddMemoryCache();
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
            // �����ȡ��ǰ����
            app.UseRequestLocalization();
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

        private static void RegisterGlobeRepositories(IDatabase db, IServiceCollection services)
        {
            services.ProvideSEORepositories(db);
            services.AddHttpContextAccessor();
            services.AddScoped<IClientEnvironment, ClientEnvironment>();
        }
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.ProvideAuthRepositories();
            services.ProvideOpenRepositories();
            services.ProvideBlogRepositories();
            services.ProvideGzoRepositories();
            services.ProvideContactRepositories();
            services.ProvideOpenRepositories();
            services.ProvideNoteRepositories();
        }
    }
}
