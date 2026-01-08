using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManager.Domain.Services;
using TaskManager.View.Controllers;
using TaskManager.View.Utilities;

namespace TaskManager.View.Filters;

/// <summary>
/// Атрибут фильтра для проверки доступа пользователя к защищенным ресурсам на основе идентификатора.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class CheckUserAccessAttribute : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Выполняет асинхронную проверку доступа пользователя перед выполнением метода действия.
    /// </summary>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var authService = context.HttpContext.RequestServices.GetService<IAuthService>() ??
            throw new InvalidOperationException("Не зарегистрирован сервис IAuthService");

        if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
        {
            if (id != authService.IdCurrentUser)
            {
                RedirectToAccessDeniedError(context);
                return;
            }
        }
        else
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }

    /// <summary>
    /// Перенаправляет пользователя на страницу ошибки "Доступ запрещен".
    /// Используется при попытке доступа к ресурсу без необходимых прав доступа.
    /// </summary>
    /// <param name="context">Контекст фильтра авторизации, содержащий информацию о текущем запросе.</param>
    private void RedirectToAccessDeniedError(ActionExecutingContext context)
    {
        var nameController = NameController.GetControllerName(nameof(ErrorsController));
        var actionController = nameof(ErrorsController.AccessDeniedError);

        context.Result = new RedirectToActionResult(actionController, nameController, null);
    }
}