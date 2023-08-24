using Entities;
using Microsoft.AspNetCore.Http;

namespace Service.Helpers;

public static class UploadFile
{
    
    public static async Task<Upload> UploadImage(this IFormFile file, string uploadsFolder, int parentId, Parent parent, CancellationToken cancellationToken)
    {
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var format = file.ContentType.Split("/");
        var name = Guid.NewGuid() + "." + format[1];
        var filePath = Path.Combine(uploadsFolder, name);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return new Upload
        {
            Type = UploadType.Image,
            Parent = parent,
            ParentId = parentId,
            Id = 0,
            ContentType = file.ContentType,
            StoredFileName = name,
            FileName = name
        };
    }

    public static string GeneratePath(this Upload upload, string url)
    {
        return Path.Combine(url, "uploads", upload.StoredFileName);
    }
}