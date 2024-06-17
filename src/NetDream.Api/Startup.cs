using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetDream.Api.Base.Http;
using NetDream.Api.Base.Middleware;
using NetDream.Api.Models;
using NetDream.Core.Http;
using NetDream.Core.Interfaces;
using NetDream.Core.Securities;
using NetDream.Modules.Auth.Repositories;
using NetDream.Modules.Blog.Repositories;
using NetDream.Modules.Contact.Repositories;
using NetDream.Modules.Gzo.Repositories;
using NetDream.Modules.OpenPlatform.Http;
using NetDream.Modules.OpenPlatform.Repositories;
using NetDream.Modules.SEO.Repositories;
using NPoco;
using System.Text;

namespace NetDream.Api
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

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
            services.AddAuthentication(options =>
            {
                //认证 middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
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
            RegisterAuthRepositories(services);
            services.AddTransient<ISecurity, Encryptor>();
            services.AddTransient<IJsonResponse, PlatformResponse>();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NetDream.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetDream.Api v1"));
            }

            app.UseRouting();
            app.UseMiddleware<ResponseMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void RegisterGlobeRepositories(IDatabase db, IServiceCollection services)
        {
            var option = new OptionRepository(db);
            services.AddSingleton(typeof(IGlobeOption), option.LoadOption());
        }
        private static void RegisterAuthRepositories(IServiceCollection services)
        {
            services.AddScoped(typeof(ClientEnvironment), typeof(IClientEnvironment));
            services.AddScoped(typeof(UserRepository));
            services.AddScoped(typeof(BlogRepository));
            services.AddScoped(typeof(GzoRepository));
            services.AddScoped(typeof(ContactRepository));
            services.AddScoped(typeof(OpenRepository));
            // services.AddSingleton(typeof(OptionRepository));
        }
    }
}
