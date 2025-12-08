namespace TaskManager.Application.Exceptions;

/// <summary>
/// Исключение, возникающее при попытке закрыть задачу с незаполненными выходными данными документа
/// </summary>
/// <remarks>
/// Это исключение наследуется от <see cref="InvalidOperationException"/> и используется для обозначения
/// ситуаций, когда задача не может быть закрыта из-за отсутствия необходимых выходных данных документа.
/// </remarks>
public class IncompleteOutputDocumentException() : InvalidOperationException("Не заполнены выходные данные документа")
{
}