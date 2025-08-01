using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using NetDream.Api.Base.Http;
using NetDream.Api.Base.Middleware;
using NetDream.Api.Models;
using NetDream.Modules.Auth;
using NetDream.Modules.Blog;
using NetDream.Modules.Contact;
using NetDream.Modules.Gzo;
using NetDream.Modules.Note;
using NetDream.Modules.Forum;
using NetDream.Modules.Document;
using NetDream.Modules.Finance;
using NetDream.Modules.Plan;
using NetDream.Modules.OpenPlatform;
using NetDream.Modules.OpenPlatform.Http;
using NetDream.Modules.SEO;
using NetDream.Modules.UserAccount;
using NetDream.Modules.UserIdentity;
using NetDream.Modules.UserProfile;
using NetDream.Shared.Converters;
using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.Json;

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
            env.CacheRoot = Path.Combine(currentFolder, env.CacheRoot);
            _environment = env;
        }
        public IConfiguration Configuration { get; private set; }
        private readonly IEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 跨域
            var hosts = _environment.AllowedHosts.Split(',');
            services.AddCors(options =>
                options.AddDefaultPolicy(
                    builder =>
#if DEBUG
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()

#else
                    builder.WithOrigins(hosts)
                    .AllowCredentials()
                    .WithHeaders(HeaderNames.Authorization, HeaderNames.ContentType)
                    
#endif
                    .WithExposedHeaders(HeaderNames.ContentDisposition)
                    .AllowAnyMethod()
                )
            );
            #endregion


            #region Jwt配置
            var jwtConfigure = Configuration.GetSection("JwtSettings");
            //将 appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(jwtConfigure);

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = jwtConfigure.Get<JwtSettings>() ?? new JwtSettings();

            //添加身份验证
            services.AddAuthentication(options => {
                //认证 middleware配置
                options.RequireAuthenticatedSignIn = false;
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

            #region db
            RegisterDbContext(services);
            #endregion

            services.AddScoped<IJsonResponse, PlatformResponse>();
            services.AddControllers().AddJsonOptions(options => {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                options.JsonSerializerOptions.Converters.Add(new MetaConverter<MetaResponse>());
            });
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
            services.AddOpenApi();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
    
            app.UseCors();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ResponseMiddleware>();
            app.MapControllers();
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
            AddContext<AuthContext>(services, connectString, serverVersion);
            AddContext<UserContext>(services, connectString, serverVersion);
            AddContext<IdentityContext>(services, connectString, serverVersion);
            AddContext<ProfileContext>(services, connectString, serverVersion);
            AddContext<SEOContext>(services, connectString, serverVersion);
            AddContext<BlogContext>(services, connectString, serverVersion);
            AddContext<ContactContext>(services, connectString, serverVersion);
            AddContext<OpenContext>(services, connectString, serverVersion);
            AddContext<NoteContext>(services, connectString, serverVersion);
            var contextOptions = new DbContextOptionsBuilder<SEOContext>().UseMySql(connectString, serverVersion)
                .Options;
            RegisterGlobeRepositories(services, contextOptions);
            RegisterRepositories(services);
        }

        private static void AddContext<TContext>(IServiceCollection services, string connectString, ServerVersion serverVersion)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(
                            options => options.UseMySql(connectString, serverVersion, builder =>
                            {
                                // 允许主键使用 in 查询
                                builder.TranslateParameterizedCollectionsToConstants();
                            })
#if DEBUG
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors()
#endif
                        );
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
            services.ProvideUserRepositories();
            services.ProvideIdentityRepositories();
            services.ProvideProfileRepositories();
            services.ProvideOpenRepositories();
            services.ProvideBlogRepositories();
            services.ProvideGzoRepositories();
            services.ProvideContactRepositories();
            services.ProvideOpenRepositories();
            services.ProvideNoteRepositories();
            services.ProvideForumRepositories();
            services.ProvideDocumentRepositories();
            services.ProvideFinanceRepositories();
            services.ProvidePlanRepositories();
        }
    }
}
