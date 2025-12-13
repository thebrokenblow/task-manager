namespace TaskManager.View.Utilities;

public static class NameController
{
    public static string GetControllerName(string nameController)
    {
        return nameController.Replace("Controller", string.Empty);
    }
}