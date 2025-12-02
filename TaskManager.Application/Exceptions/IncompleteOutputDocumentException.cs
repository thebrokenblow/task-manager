namespace TaskManager.Application.Exceptions;

public class IncompleteOutputDocumentException() : InvalidOperationException("Не заполнены выходные данные документа")
{
}