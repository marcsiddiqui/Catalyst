using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Nop.Services.Common;

public partial class AttachmentFileService : IAttachmentFileService
{
    protected readonly IWebHostEnvironment _webHostEnvironment;

    public AttachmentFileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    protected virtual string GetSafeSegment(string value)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeValue = new string((value ?? string.Empty).Where(character => !invalidChars.Contains(character)).ToArray());

        return string.IsNullOrWhiteSpace(safeValue) ? "General" : safeValue;
    }

    public virtual async Task<(string FileName, string FilePath)> SaveAttachmentAsync(IFormFile file, string moduleName, int recordId, string fileNamePrefix = null)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (recordId <= 0)
            throw new ArgumentException("Record id is required.", nameof(recordId));

        var attachmentsDirectory = Path.Combine(_webHostEnvironment.ContentRootPath, "Attachments");
        var moduleDirectory = Path.Combine(attachmentsDirectory, GetSafeSegment(moduleName));
        var recordDirectory = Path.Combine(moduleDirectory, recordId.ToString());

        System.IO.Directory.CreateDirectory(recordDirectory);

        var originalFileName = Path.GetFileName(file.FileName);
        var extension = Path.GetExtension(originalFileName);
        var safeFileNamePrefix = GetSafeSegment(fileNamePrefix);
        var storedFileName = string.IsNullOrWhiteSpace(fileNamePrefix)
            ? $"{Guid.NewGuid():N}{extension}"
            : $"{safeFileNamePrefix}-{Guid.NewGuid():N}-{recordId}{extension}";
        var physicalPath = Path.Combine(recordDirectory, storedFileName);

        await using (var fileStream = new FileStream(physicalPath, FileMode.CreateNew))
        {
            await file.CopyToAsync(fileStream);
        }

        var relativePath = Path.Combine("Attachments", GetSafeSegment(moduleName), recordId.ToString(), storedFileName)
            .Replace('\\', '/');

        return (originalFileName, relativePath);
    }

    public virtual Task DeleteAttachmentAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Task.CompletedTask;

        var physicalPath = GetAttachmentPhysicalPath(filePath);

        if (File.Exists(physicalPath))
            File.Delete(physicalPath);

        return Task.CompletedTask;
    }

    public virtual string GetAttachmentPhysicalPath(string filePath)
    {
        var normalizedPath = (filePath ?? string.Empty)
            .Replace('/', Path.DirectorySeparatorChar)
            .TrimStart(Path.DirectorySeparatorChar);

        return Path.Combine(_webHostEnvironment.ContentRootPath, normalizedPath);
    }
}
