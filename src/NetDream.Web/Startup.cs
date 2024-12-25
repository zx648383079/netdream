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
using System;
using System.IO;
using NetDream.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Data.Common;

namespace NetDream.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration  = configuration;
            var env = configuration.GetSection(EnvironmentConfiguration.EnvironmentKey)
            .Get<EnvironmentConfiguration>() ?? new EnvironmentConfiguration();
            var currentFolder = Directory.GetCurrentDirectory();
            env.Root = Path.Combine(currentFolder, env.Root);
            env.PublicRoot = Path.Combine(currentFolder, env.PublicRoot);
            _environment = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IEnvironment _environment;
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
            services.AddSession(options =>
            {
                options.Cookie.Name = ".zodream.session";
                options.IdleTimeout = TimeSpan.FromSeconds(2000);
                options.Cookie.HttpOnly = true;
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
                {
                    o.LoginPath = new PathString("/Auth");            //登录路径：这是当用户试图访问资源但未经过身份验证时，程序将会将请求重定向到这个相对路径。
                    o.AccessDeniedPath = new PathString("/Home/Error");     //禁止访问路径：当用户试图访问资源时，但未通过该资源的任何授权策略，请求将被重定向到这个相对路径。
                    o.SlidingExpiration = true; //Cookie可以分为永久性的和临时性的。 临时性的是指只在当前浏览器进程里有效，浏览器一旦关闭就失效（被浏览器删除）。 永久性的是指Cookie指定了一个过期时间，在这个时间到达之前，此cookie一直有效（浏览器一直记录着此cookie的存在）。 slidingExpriation的作用是，指示浏览器把cookie作为永久性cookie存储，但是会自动更改过期时间，以使用户不会在登录后并一直活动，但是一段时间后却自动注销。也就是说，你10点登录了，服务器端设置的TimeOut为30分钟，如果slidingExpriation为false,那么10: 30以后，你就必须重新登录。如果为true的话，你10: 16分时打开了一个新页面，服务器就会通知浏览器，把过期时间修改为10: 46。
                });
            #region db
            RegisterDbContext(services);
            #endregion
            // 本地化
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
                    // 循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
                    // 不使用驼峰
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    // 设置时间格式
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    // 如字段为 null值，该字段不会返回到前端
                    // options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddMemoryCache();
            services.AddLogging();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
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
                    Path.Combine(_environment.PublicRoot, _environment.AssetRoot)),
                RequestPath = "/assets"
            });
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ResponseMiddleware>();
            // 方便获取当前语言
            app.UseRequestLocalization();

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


        private void RegisterDbContext(IServiceCollection services)
        {
            var connectString = Configuration.GetConnectionString("Default") ?? string.Empty;
#if DEBUG
            services.AddTransient<DbConnection>(_ => {
                var db = new MySqlConnection(connectString);
                db.Open();
                return db;
            });
#endif
            var serverVersion = ServerVersion.AutoDetect(connectString); //new MySqlServerVersion(new Version(8, 0, 29));
            services.AddDbContext<AuthContext>(
                options => options.UseMySql(connectString, serverVersion)
#if DEBUG
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
#endif
            );
            var contextOptions = new DbContextOptionsBuilder<SEOContext>().UseMySql(connectString, serverVersion)
                .Options;
            RegisterGlobeRepositories(services, contextOptions);
            RegisterRepositories(services);
        }

        private void RegisterGlobeRepositories(IServiceCollection services, DbContextOptions<SEOContext> options)
        {
            services.AddSingleton(_environment);
            services.ProvideSEORepositories(options);
            services.AddHttpContextAccessor();
            services.AddScoped<IClientContext, ClientContext>();
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
