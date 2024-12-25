using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using NetDream.Modules.Blog;
using NetDream.Shared.Interfaces;
using System.Data.Common;
using System;
using NetDream.Shared.Models;
using System.IO;
using NetDream.Modules.Auth;
using NetDream.Modules.SEO;
using NetDream.Razor.Base.Http;

namespace NetDream.Razor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var env = configuration.GetSection(EnvironmentConfiguration.EnvironmentKey)
            .Get<EnvironmentConfiguration>() ?? new EnvironmentConfiguration();
            var currentFolder = Directory.GetCurrentDirectory();
            env.Root = Path.Combine(currentFolder, env.Root);
            env.PublicRoot = Path.Combine(currentFolder, env.PublicRoot);
            _environment = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region db
            RegisterDbContext(services);
            #endregion
            services.AddMemoryCache();
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
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
            services.ProvideBlogRepositories();
        }
    }
}
