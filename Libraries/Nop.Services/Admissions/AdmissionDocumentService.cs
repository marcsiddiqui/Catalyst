using Nop.Core;
using Nop.Core.Domain.Admissions;
using Nop.Data;

namespace Nop.Services.Admissions;

public partial class AdmissionDocumentService : IAdmissionDocumentService
{
    #region Fields

    protected readonly IRepository<AdmissionDocument> _admissionDocumentRepository;

    #endregion

    #region Ctor

    public AdmissionDocumentService(
        IRepository<AdmissionDocument> admissionDocumentRepository
        )
    {
        _admissionDocumentRepository = admissionDocumentRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<AdmissionDocument>> GetAllAdmissionDocumentsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int admissionId = 0, IEnumerable<int> admissionIds = null,
        int admissionDocumentTypeId = 0, IEnumerable<int> admissionDocumentTypeIds = null,
        string fileName = null, IEnumerable<string> fileNames = null,
        string filePath = null, IEnumerable<string> filePaths = null,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _admissionDocumentRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (admissionId > 0)
                query = query.Where(x => admissionId == x.AdmissionId);

            if (admissionIds != null && admissionIds.Any())
                query = query.Where(x => admissionIds.Contains(x.AdmissionId));

            if (admissionDocumentTypeId > 0)
                query = query.Where(x => admissionDocumentTypeId == x.AdmissionDocumentTypeId);

            if (admissionDocumentTypeIds != null && admissionDocumentTypeIds.Any())
                query = query.Where(x => admissionDocumentTypeIds.Contains(x.AdmissionDocumentTypeId));

            if (!string.IsNullOrWhiteSpace(fileName))
                query = query.Where(x => fileName == x.FileName);

            if (fileNames != null && fileNames.Any())
                query = query.Where(x => fileNames.Contains(x.FileName));

            if (!string.IsNullOrWhiteSpace(filePath))
                query = query.Where(x => filePath == x.FilePath);

            if (filePaths != null && filePaths.Any())
                query = query.Where(x => filePaths.Contains(x.FilePath));

            query = query.WhereBoolean(x => x.Deleted, deleted);





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AdmissionDocument> GetAdmissionDocumentByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _admissionDocumentRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AdmissionDocument>> GetAdmissionDocumentsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _admissionDocumentRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAdmissionDocumentAsync(AdmissionDocument admissionDocument)
    {
        if (admissionDocument == null)
            return;

        await _admissionDocumentRepository.InsertAsync(admissionDocument);
    }
    
    public virtual async Task InsertAdmissionDocumentAsync(IEnumerable<AdmissionDocument> admissionDocuments)
    {
        if (admissionDocuments == null || !admissionDocuments.Any())
            return;

        await _admissionDocumentRepository.InsertAsync(admissionDocuments.ToList());
    }

    public virtual async Task UpdateAdmissionDocumentAsync(AdmissionDocument admissionDocument)
    {
        if (admissionDocument == null)
            return;

        await _admissionDocumentRepository.UpdateAsync(admissionDocument);
    }

    public virtual async Task UpdateAdmissionDocumentAsync(IEnumerable<AdmissionDocument> admissionDocuments)
    {
        if (admissionDocuments == null || !admissionDocuments.Any())
            return;

        await _admissionDocumentRepository.UpdateAsync(admissionDocuments.ToList());
    }

    public virtual async Task DeleteAdmissionDocumentAsync(AdmissionDocument admissionDocument)
    {
        if (admissionDocument == null)
            return;

        await _admissionDocumentRepository.DeleteAsync(admissionDocument);
    }

    public virtual async Task DeleteAdmissionDocumentAsync(IEnumerable<AdmissionDocument> admissionDocuments)
    {
        if (admissionDocuments == null || !admissionDocuments.Any())
            return;

        await _admissionDocumentRepository.DeleteAsync(admissionDocuments.ToList());
    }

    #endregion
}