using Microsoft.AspNetCore.Http;

namespace Nop.Services.Common;

public partial interface IAttachmentFileService
{
    Task<(string FileName, string FilePath)> SaveAttachmentAsync(IFormFile file, string moduleName, int recordId, string fileNamePrefix = null);

    Task DeleteAttachmentAsync(string filePath);

    string GetAttachmentPhysicalPath(string filePath);
}
