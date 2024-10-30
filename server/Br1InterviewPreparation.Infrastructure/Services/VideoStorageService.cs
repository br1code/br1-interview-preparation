using Microsoft.Extensions.Configuration;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Infrastructure.Services;

// TODO: create new IVideoStorageService implementation using third-party service for file storage
public class VideoStorageService : IVideoStorageService
{
    private readonly string? _videoStorageFilePath;

    public VideoStorageService(IConfiguration configuration)
    {
        _videoStorageFilePath = configuration["VideoStorage:FilePath"];

        if (string.IsNullOrEmpty(_videoStorageFilePath))
        {
            throw new ArgumentException("Video Storage File Path is not configured properly.");
        }
    }

    public string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".webm" => "video/webm",
            ".mp4" => "video/mp4",
            ".avi" => "video/x-msvideo",
            _ => "application/octet-stream", // default content type
        };
    }

    public string GetVideoFilePath(string fileName)
    {
        var filePath = Path.Combine(_videoStorageFilePath!, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        return filePath;
    }

    public void DeleteVideoFile(string fileName)
    {
        var filePath = Path.Combine(_videoStorageFilePath!, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        File.Delete(filePath);
    }

    public async Task<string> SaveVideoFileAsync(string fileName, string contentType, byte[] content, CancellationToken cancellationToken = default)
    {
        if (!contentType.StartsWith("video/"))
        {
            throw new ArgumentException("Invalid file type. Only video files are allowed.");
        }

        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var filePath = Path.Combine(_videoStorageFilePath!, uniqueFileName);

        await File.WriteAllBytesAsync(filePath, content, cancellationToken);

        return uniqueFileName;
    }
}
