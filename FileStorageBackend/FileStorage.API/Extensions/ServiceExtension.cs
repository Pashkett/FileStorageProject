using Microsoft.Extensions.DependencyInjection;
using FileStorage.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;

namespace FileStorage.API.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void ConfigureAuthenticationJWT(this IServiceCollection services,
                                                      IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        public static void ConfigureAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRoleRequired", policy => policy.RequireRole("Admin"));
                options.AddPolicy("ModerateFilesRole", policy => policy.RequireRole("Admin", "Moderator"));
                options.AddPolicy("MemberRequired", policy => policy.RequireRole("Member"));
            });
        }

        public static void ConfigureFormOptionsLimits(this IServiceCollection services)
        {
            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MemoryBufferThreshold = int.MaxValue;
            });
        }
    }
}
