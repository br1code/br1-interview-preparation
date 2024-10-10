using Microsoft.Extensions.Configuration;
using Br1InterviewPreparation.Application.Interfaces;

namespace Br1InterviewPreparation.Infrastructure.Services;

// TODO: create new IVideoStorageService implementation using third-party service for file storage
public class VideoStorageService : IVideoStorageService
{
    private const string VIDEO_CONTENT_TYPE = "video/webm";
    private readonly string _videoStorageFilePath;

    public VideoStorageService(IConfiguration configuration)
    {
        _videoStorageFilePath = configuration["VideoStorage:FilePath"];

        if (string.IsNullOrEmpty(_videoStorageFilePath))
        {
            throw new ArgumentException("Video Storage File Path is not configured properly.");
        }
    }

    // TODO: get content type based on file format
    public string GetContentType(string fileName)
    {
        return VIDEO_CONTENT_TYPE;
    }

    public string GetVideoFilePath(string fileName)
    {
        var filePath = Path.Combine(_videoStorageFilePath, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        return filePath;
    }

    public void DeleteVideoFile(string fileName)
    {
        var filePath = Path.Combine(_videoStorageFilePath, fileName);

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(filePath);
        }

        File.Delete(filePath);
    }
}
