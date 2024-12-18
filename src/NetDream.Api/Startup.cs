using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NetDream.Api.Base.Http;
using NetDream.Api.Base.Middleware;
using NetDream.Api.Models;
using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Securities;
using NetDream.Modules.Auth;
using NetDream.Modules.Blog;
using NetDream.Modules.Contact;
using NetDream.Modules.Gzo;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.SEO;
using NetDream.Modules.Note;
using NetDream.Modules.OpenPlatform.Http;
using NPoco;
using System.Text;
using NetDream.Shared.Models;
using System.IO;

namespace NetDream.Api
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
        public IConfiguration Configuration { get; private set; }
        private readonly IEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 跨域
            var hosts = Configuration["AllowedHosts"].Split(',');
            services.AddCors(options =>
                options.AddPolicy("AllowSameDomain",
                builder => builder.WithOrigins(hosts)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials())
            );
            #endregion

            #region Jwt配置
            //将 appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            //添加身份验证
            services.AddAuthentication(options => {
                //认证 middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o => {
                //jwt token参数设置
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                    //Token颁发机构
                    ValidIssuer = jwtSettings.Issuer,
                    //颁发给谁
                    ValidAudience = jwtSettings.Audience,
                    //这里的 key要进行加密
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    /***********************************TokenValidationParameters的参数默认值***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为 false，可以不验证Issuer和Audience，但是不建议这样做。
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // 是否要求Token的Claims中必须包含Expires
                    // RequireExpirationTime = true,
                    // 允许的服务器时间偏移量
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    // ValidateLifetime = true
                };
            });
            #endregion
            services.AddScoped<IDatabase>(x => {
                return new Database(Configuration.GetConnectionString("Default"), DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance);
            });
            using (var db = new Database(Configuration.GetConnectionString("Default"), DatabaseType.MySQL, MySql.Data.MySqlClient.MySqlClientFactory.Instance))
            {
                RegisterGlobeRepositories(db, services);
            }
            RegisterRepositories(services);
            services.AddTransient<ISecurity, Encryptor>();
            services.AddTransient<IJsonResponse, PlatformResponse>();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddOpenApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            // app.UseRouting();
            app.UseMiddleware<ResponseMiddleware>();
            app.MapControllers();
        }

        private void RegisterGlobeRepositories(IDatabase db, IServiceCollection services)
        {
            services.AddSingleton(_environment);
            services.ProvideSEORepositories(db);
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
