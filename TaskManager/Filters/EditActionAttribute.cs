using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Queries.Interfaces;

namespace TaskManager.Filters;


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class EditActionAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var documentQuery = context.HttpContext.RequestServices.GetRequiredService<IDocumentQuery>();
        var lastElementRequest = context.HttpContext.Request.Path.ToString()
                                                                 .Split('/')
                                                                 .Last();

        if (int.TryParse(lastElementRequest, out int id))
        {
            var document = await documentQuery.GetDetailsByIdAsync(id);

            if (document is null)
            {
                context.Result = new NotFoundResult();
            }
            else if (!document.IsNotDeletedDocument)
            {
                context.Result = new RedirectToActionResult("Index", "Documents", null);
            }
        }
        else
        {
            context.Result = new NotFoundResult();
        }

        await next();
    }
}