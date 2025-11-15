using TaskManager.Application.Common;
using TaskManager.Application.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces.Queries;
using TaskManager.Domain.Interfaces.Repositories;
using TaskManager.Domain.Interfaces.Services;
using TaskManager.Domain.QueryModels;

namespace TaskManager.Application.Services;

public class DocumentService(
    IAuthService authService, 
    IDocumentQuery documentQuery,
    IDocumentRepository documentRepository) : IDocumentService
{
    public async Task<Document?> GetDetailsByIdAsync(int id)
    {
        var document = await documentRepository.GetDetailsByIdAsync(id);

        return document;
    }

    public async Task<PagedResult<FilteredRangeDocumentModel>> GetFilteredRangeAsync(string inputSearch, int page, int pageSize)
    {
        int countDocuments;
        List<FilteredRangeDocumentModel> documents;

        int countSkip = (page - 1) * pageSize;

        if (authService.IsAdmin)
        {
            (documents, countDocuments) = await documentQuery.GetDeletedRangeAsync(inputSearch, countSkip, pageSize);
        }
        else if (!string.IsNullOrWhiteSpace(inputSearch))
        {
            (documents, countDocuments) = await documentQuery.GetFilteredRangeAsync(inputSearch, countSkip, pageSize);
        }
        else
        {
            (documents, countDocuments) = await documentQuery.GetRangeAsync(countSkip, pageSize);
        }

        var filteredRangeDocuments = new PagedResult<FilteredRangeDocumentModel>(documents, countDocuments, page, pageSize);

        return filteredRangeDocuments;
    }

    public async Task ChangeStatusDocumentAsync(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        document.IsCompleted = !document.IsCompleted;
        
        await documentRepository.UpdateAsync(document);
    }

    public async Task CreateAsync(Document document)
    {
        document = TrimDocument(document);
        await documentRepository.AddAsync(document);
    }

    public async Task EditAsync(Document document)
    {
        document = TrimDocument(document);
        await documentRepository.UpdateAsync(document);
    }

    public async Task DeleteAsync(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        if (authService.IsAdmin)
        {
            await documentRepository.RemoveAsync(document);
        }
        else if (authService.IsAuthenticated)
        {
            document.DateRemove = DateTime.Now;
            document.IdAuthorRemoveDocument = document.IdAuthorCreateDocument;
            document.IdAuthorCreateDocument = id;

            await documentRepository.UpdateAsync(document);
        }
    }

    public async Task RecoverDeletedDocumentAsync(int id)
    {
        var document = await documentRepository.GetByIdAsync(id);

        document.DateRemove = null;

        if (document.IdAuthorRemoveDocument is null)
        {
            //Некорректное поведение системы
            throw new Exception();
        }
        
        document.IdAuthorCreateDocument = document.IdAuthorRemoveDocument.Value;

        await documentRepository.UpdateAsync(document);
    }

    private static Document TrimDocument(Document document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (document.SourceOutgoingDocumentNumber is not null)
        {
            document.SourceOutgoingDocumentNumber = document.SourceOutgoingDocumentNumber.Trim();
        }

        if (document.SourceCustomer is not null)
        {
            document.SourceCustomer = document.SourceCustomer.Trim();
        }

        if (document.SourceTaskText is not null)
        {
            document.SourceTaskText = document.SourceTaskText.Trim();
        }

        if (document.SourceOutputDocumentNumber is not null)
        {
            document.SourceOutputDocumentNumber = document.SourceOutputDocumentNumber.Trim();
        }

        if (document.OutputOutgoingNumber is not null)
        {
            document.OutputOutgoingNumber = document.OutputOutgoingNumber.Trim();
        }

        if (document.OutputSentTo is not null)
        {
            document.OutputSentTo = document.OutputSentTo.Trim();
        }

        if (document.OutputTransferredInWorkOrder is not null)
        {
            document.OutputTransferredInWorkOrder = document.OutputTransferredInWorkOrder.Trim();
        }

        return document;
    }
}