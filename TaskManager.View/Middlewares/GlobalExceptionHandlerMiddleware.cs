using TaskManager.View.Controllers;
using TaskManager.View.Utilities;

namespace TaskManager.View.Middlewares;

/// <summary>
/// Middleware для глобальной обработки исключений в приложении.
/// Перехватывает все необработанные исключения, логирует их и перенаправляет пользователя на соответствующие страницы ошибок.
/// </summary>
public sealed class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Основной метод обработки запроса в конвейере middleware.
    /// Обрабатывает исключения, возникающие при выполнении последующих компонентов конвейера.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <returns>Задача, представляющая асинхронную операцию обработки запроса.</returns>
    /// <remarks>
    /// Логика обработки исключений:
    /// 1. UnauthorizedAccessException - перенаправление на страницу входа
    /// 2. Любое другое исключение - логирование и перенаправление на страницу необработанной ошибки
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UnauthorizedAccessException)
        {
            RedirectToUnauthorizedError(context);

            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            RedirectToUnhandledError(context);

            return;
        }
    }

    /// <summary>
    /// Перенаправляет пользователя на страницу авторизации при возникновении исключения UnauthorizedAccessException.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    private void RedirectToUnauthorizedError(HttpContext context)
    {
        var nameAction = nameof(AccountsController.Login);
        var nameController = NameController.GetControllerName(nameof(AccountsController));

        context.Response.Redirect($"/{nameController}/{nameAction}");
    }

    /// <summary>
    /// Перенаправляет пользователя на страницу необработанной ошибки при возникновении любого другого исключения.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    private void RedirectToUnhandledError(HttpContext context)
    {
        var nameAction = nameof(ErrorsController.UnhandledError);
        var nameController = NameController.GetControllerName(nameof(ErrorsController));

        context.Response.Redirect($"/{nameController}/{nameAction}");
    }
}