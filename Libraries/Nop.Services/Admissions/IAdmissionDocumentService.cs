using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Admissions;

namespace Nop.Services.Admissions;

public partial interface IAdmissionDocumentService
{
    Task<IPagedList<AdmissionDocument>> GetAllAdmissionDocumentsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int admissionId = 0, IEnumerable<int> admissionIds = null,
        int admissionDocumentTypeId = 0, IEnumerable<int> admissionDocumentTypeIds = null,
        string fileName = null, IEnumerable<string> fileNames = null,
        string filePath = null, IEnumerable<string> filePaths = null,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AdmissionDocument> GetAdmissionDocumentByIdAsync(int id);

    Task<IList<AdmissionDocument>> GetAdmissionDocumentsByIdsAsync(IEnumerable<int> ids);

    Task InsertAdmissionDocumentAsync(AdmissionDocument admissionDocument);
    
    Task InsertAdmissionDocumentAsync(IEnumerable<AdmissionDocument> admissionDocuments);

    Task UpdateAdmissionDocumentAsync(AdmissionDocument admissionDocument);

    Task UpdateAdmissionDocumentAsync(IEnumerable<AdmissionDocument> admissionDocuments);

    Task DeleteAdmissionDocumentAsync(AdmissionDocument admissionDocument);

    Task DeleteAdmissionDocumentAsync(IEnumerable<AdmissionDocument> admissionDocuments);
}