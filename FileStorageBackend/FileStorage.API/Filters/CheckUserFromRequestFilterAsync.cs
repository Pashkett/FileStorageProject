using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FileStorage.Domain.Services.AuthenticationServices;


namespace FileStorage.API.Filters
{
    public class CheckUserFromRequestFilterAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authService = 
                context.HttpContext.RequestServices.GetService(typeof(IAuthService)) as IAuthService;
            
            var userRequested = 
                await authService.GetRequestedUser(context.HttpContext.User);

            if (userRequested == null)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Result = new JsonResult("Unauthorized");
            }
            else
                context.HttpContext.Items["UserRequested"] = userRequested;

            await next();
        }
    }
}
