using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.Services.AuthenticationServices;

namespace FileStorage.API.Filters
{
    public class UserCheckerFromRequest : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration =
                context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            string userParamName = configuration.GetValue<string>("UserKeyParameter");
            
            var authService = 
                context.HttpContext.RequestServices.GetService(typeof(IAuthService)) as IAuthService;

            var userRequested = 
                await authService.GetRequestedUser(context.HttpContext.User);

            if (userRequested == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult("Unauthorized");
                return;
            }
            else
                context.HttpContext.Items[userParamName] = userRequested;

            var result = await next();
        }
    }
}
