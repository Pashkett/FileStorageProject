using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileStorage.Domain.Services.Authentication;

namespace FileStorage.API.Filters
{
    /// <summary>
    /// Filter for check if user still exists in database
    /// </summary>
    public class UserCheckerFromRequest : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

            string userParamName = configuration.GetValue<string>("UserKeyParameter");

            var authService = context.HttpContext.RequestServices.GetService<IAuthService>();

            var userRequested = await authService.GetRequestedUser(context.HttpContext.User);

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
