using TaskManager.View.Middlewares;

namespace TaskManager.View.Extensions;

/// <summary>
/// Методы расширения для конфигурации middleware в приложении.
/// Предоставляет удобные методы для добавления пользовательских middleware в конвейер обработки запросов.
/// </summary>
public static class MiddlewareExtension
{
    /// <summary>
    /// Добавляет middleware для глобальной обработки исключений в конвейер приложения.
    /// Перехватывает необработанные исключения и возвращает структурированный ответ с информацией об ошибке.
    /// </summary>
    /// <param name="app">Экземпляр IApplicationBuilder для настройки конвейера middleware.</param>
    /// <returns>Экземпляр IApplicationBuilder для цепочки вызовов.</returns>
    public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}