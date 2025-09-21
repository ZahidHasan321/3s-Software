using Microsoft.AspNetCore.Http;

namespace ProductApi.Infrastructure.Services;

public interface IFileService
{
    Task<string?> SaveImageAsync(IFormFile image);
    Task DeleteImageAsync(string imagePath);
    bool IsValidImage(IFormFile file);
}

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private const long MaxFileSize = 5 * 1024 * 1024; // 5MB
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveImageAsync(IFormFile image)
    {
        if (!IsValidImage(image))
            return null;

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await image.CopyToAsync(fileStream);

        return $"/images/{uniqueFileName}";
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return;

        var fullPath = Path.Combine(_environment.WebRootPath, imagePath.TrimStart('/'));
        
        if (File.Exists(fullPath))
        {
            await Task.Run(() => File.Delete(fullPath));
        }
    }

    public bool IsValidImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        if (file.Length > MaxFileSize)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }
}