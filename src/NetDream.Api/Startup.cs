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
using NetDream.Shared.Http;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Securities;
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
            #region ����
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

            #region Jwt����
            //�� appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            //���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����
            //�����ð󶨵�JwtSettingsʵ����
            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            //����������֤
            services.AddAuthentication(options =>
            {
                //��֤ middleware����
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //jwt token��������
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                    //Token�䷢����
                    ValidIssuer = jwtSettings.Issuer,
                    //�䷢��˭
                    ValidAudience = jwtSettings.Audience,
                    //����� keyҪ���м���
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                    /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // ������������������Ϊ false�����Բ���֤Issuer��Audience�����ǲ�������������
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // �Ƿ�Ҫ��Token��Claims�б������Expires
                    // RequireExpirationTime = true,
                    // �����ķ�����ʱ��ƫ����
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
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
