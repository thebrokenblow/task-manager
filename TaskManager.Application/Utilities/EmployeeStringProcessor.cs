namespace TaskManager.Application.Utilities;

public sealed class EmployeeStringProcessor
{
    private const string SpaceSeparator = " ";
    private const string UnderscoreSeparator = "_";

    public static string CleanSpaces(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var elements = input.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(SpaceSeparator, elements);
    }

    public static string ConvertSpacesToUnderscore(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        var elements = input.Split(SpaceSeparator, StringSplitOptions.RemoveEmptyEntries);

        return string.Join(UnderscoreSeparator, elements);
    }
}