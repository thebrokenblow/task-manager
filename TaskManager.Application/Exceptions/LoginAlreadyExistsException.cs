namespace TaskManager.Application.Exceptions;

/// <summary>
/// Исключение, возникающее при попытке регистрации пользователя с логином, который уже существует в системе.
/// </summary>
/// <param name="login">Логин, который уже занят.</param>
/// <remarks>
/// Это исключение наследуется от <see cref="Exception"/> и используется для обработки конфликтов
/// при регистрации новых пользователей, когда указанный логин уже используется другим сотрудником.
/// </remarks>
public sealed class LoginAlreadyExistsException(string login) : Exception($"Сотрудник с логином '{login}' уже есть в системе")
{
}