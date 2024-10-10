namespace Br1InterviewPreparation.Application.Interfaces;

public interface IVideoStorageService
{
    string GetContentType(string fileName);
    string GetVideoFilePath(string fileName);
    void DeleteVideoFile(string fileName);
}
