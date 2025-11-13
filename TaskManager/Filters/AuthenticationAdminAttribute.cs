using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Services.Interfaces;

namespace TaskManager.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthenticationAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

        if (!authService.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (!authService.IsAdmin)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}