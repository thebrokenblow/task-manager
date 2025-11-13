using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Controllers;
using TaskManager.Repositories.Interfaces;
using TaskManager.Services.Interfaces;
using TaskManager.Utils;

namespace TaskManager.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class DocumentOwnerAuthorizationAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
        if (authService.IsAdmin)
        {
            return;
        }

        var path = context.HttpContext.Request.Path;
        if (!authService.IsAuthenticated)
        {
            context.Result = RedirectToAccount(path);
            return;
        }

        var documentId = GetDocumentIdFromPath(path);
        if (documentId is null)
        {
            context.Result = new BadRequestObjectResult("Документ не найден");
            return;
        }

        var documentRepository = context.HttpContext.RequestServices.GetRequiredService<IDocumentRepository>();
        var document = await documentRepository.GetByIdAsync(documentId.Value);

        if (document == null)
        {
            context.Result = new NotFoundObjectResult("Документ не найден");
            return;
        }

        if (authService.CurrentUserId is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (document.IdAuthorCreateDocument != authService.CurrentUserId.Value)
        {
            context.Result = new ForbidResult();
            return;
        }
    }

    private static RedirectToActionResult RedirectToAccount(string returnUrl)
    {
        var nameAction = nameof(AccountsController.Login);
        var fullNameController = nameof(AccountsController);
        var nameController = NameController.GetControllerName(fullNameController);

        return new RedirectToActionResult(nameAction, nameController, new { returnUrl });
    }

    private static int? GetDocumentIdFromPath(PathString path)
    {
        if (path.Value is null)
        {
            return null;
        }

        var pathSegments = path.Value.Split('/');

        if (pathSegments.Length >= 2)
        {
            var lastSegment = pathSegments[^1];

            if (int.TryParse(lastSegment, out int documentId))
            {
                return documentId;
            }
        }

        return null;
    }
}