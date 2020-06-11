using NLog;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using FileStorage.API.Extensions;
using FileStorage.API.Filters;
using FileStorage.Logger;
using FileStorage.Domain.ServicesExtensions;
using FileStorage.Domain.Services.Users;
using FileStorage.Domain.Services.PrivateItems;
using FileStorage.Domain.Services.Authentication;
using FileStorage.Domain.Services.RecycledItems;
using FileStorage.Domain.Services.PublicItems;
using FileStorage.Domain.Services.ItemsModeration;

namespace FileStorage.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(
                string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureFormOptionsLimits();
            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureUnitOfWork();
            services.ConfigureAutomapper();
            services.ConfigureFileSystemAbstraction();
            services.ConfigureFileSystemManagers();

            services.AddScoped<UserCheckerFromRequest>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPrivateItemsService, PrivateItemsService>();
            services.AddScoped<IRecycledItemsService, RecycledItemsService>();
            services.AddScoped<IPublicItemsService, PublicItemsService>();
            services.AddScoped<IItemsModerationService, ItemsModerationService>();

            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureAuthenticationJWT(Configuration);
            services.ConfigureAuthorizationPolicies();

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              ILoggerManager logger)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.ConfigureExceptionHandler(logger);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
