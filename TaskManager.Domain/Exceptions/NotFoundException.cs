namespace TaskManager.Domain.Exceptions;

/// <summary>
/// Исключение, которое возникает при попытке доступа к сущности, которая не была найдена.
/// </summary>
/// <param name="name">Название типа сущности.</param>
/// <param name="key">Идентификатор сущности, которая не была найдена.</param>
public sealed class NotFoundException(string name, object key) : Exception($"Сущность \"{name}\" с ключом ({key}) не найден.")
{
}