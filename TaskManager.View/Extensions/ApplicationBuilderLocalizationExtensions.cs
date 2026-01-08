using Microsoft.AspNetCore.Localization;
using System.Globalization;
using TaskManager.Domain.Utilities;

namespace TaskManager.View.Extensions;

/// <summary>
/// Методы расширения для настройки локализации в приложении.
/// </summary>
public static class ApplicationBuilderLocalizationExtensions
{
    /// <summary>
    /// Использует русскую локаль в приложении.
    /// </summary>
    public static IApplicationBuilder UseRussianLocalization(this IApplicationBuilder app)
    {
        var supportedCultures = GetRussianCultures();

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("ru-RU"),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            RequestCultureProviders = GetDefaultRequestCultureProviders()
        });

        return app;
    }

    /// <summary>
    /// Получает русскую культуру с настройками по умолчанию.
    /// </summary>
    private static CultureInfo[] GetRussianCultures()
    {
        return
        [
            new CultureInfo("ru-RU")
            {
                DateTimeFormat = new DateTimeFormatInfo
                {
                    ShortDatePattern = DateFormatDictionary.DateFormatDdMmYyyy,
                    LongDatePattern = "dd MMMM yyyy г.",
                    DateSeparator = ".",
                    ShortTimePattern = "HH:mm",
                    LongTimePattern = "HH:mm:ss"
                },
                NumberFormat = new NumberFormatInfo
                {
                    NumberDecimalSeparator = ",",
                    NumberGroupSeparator = " "
                }
            }
        ];
    }

    /// <summary>
    /// Возвращает список провайдеров культуры по умолчанию.
    /// </summary>
    private static IRequestCultureProvider[] GetDefaultRequestCultureProviders()
    {
        return
        [
            new QueryStringRequestCultureProvider(),
            new CookieRequestCultureProvider(),
            new AcceptLanguageHeaderRequestCultureProvider()
        ];
    }
}