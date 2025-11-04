using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Queries.Interfaces;

namespace TaskManager.Filters;


[AttributeUsage(AttributeTargets.All)]
public class AuthenticationDeleteAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var documentQuery = context.HttpContext.RequestServices.GetRequiredService<IDocumentQuery>();
        var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

        var lastElementRequest = context.HttpContext.Request.Path.ToString()
                                                                 .Split('/')
                                                                 .Last();

        if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            return;
        }

        if (int.TryParse(lastElementRequest, out int id))
        {
            var document = await documentQuery.GetDetailsByIdAsync(id);

            if (document is null)
            {
                context.Result = new NotFoundResult();
            }
            else if (document.LoginAuthor != Environment.UserName)
            {
                context.Result = new RedirectToActionResult("Login", "Accounts", new { returnUrl });
            }
        }
        else if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new RedirectToActionResult("Login", "Accounts", new { returnUrl });
        }
    }
}