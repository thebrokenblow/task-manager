namespace TaskManager.View.Utilities;

/// <summary>
/// Утилита для работы с именами контроллеров.
/// </summary>
public static class NameController
{
    /// <summary>
    /// Получает имя контроллера без суффикса "Controller".
    /// </summary>
    /// <param name="nameController">Полное имя контроллера.</param>
    public static string GetControllerName(string nameController)
    {
        return nameController.Replace("Controller", string.Empty);
    }
}