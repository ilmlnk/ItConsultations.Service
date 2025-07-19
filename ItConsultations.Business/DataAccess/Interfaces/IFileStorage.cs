using ItConsultations.Business.Dtos.FileDtos;

namespace ItConsultations.Business.DataAccess.Interfaces;

public interface IFileStorage
{
    Task<FileDto> UploadAsync(string fileName, Stream fileStream, string contentType, Dictionary<string, string>? metadata = null);

    Task<Stream> DownloadAsync(string filePath);

    Task<string> GetDownloadUrlAsync(string filePath, int expirationMinutes = 60);

    Task<bool> DeleteAsync(string filePath);

    Task<bool> ExistsAsync(string filePath);

    Task<FileDto?> GetFileInfoAsync(string filePath);

    Task<FileDto> CopyAsync(string sourcePath, string destinationPath);

    Task<FileDto> MoveAsync(string sourcePath, string destinationPath);

    Task<IEnumerable<FileDto>> ListFilesAsync(string folderPath, string? searchPattern = null);

    Task<bool> CreateFolderAsync(string folderPath);

    Task<bool> DeleteFolderAsync(string folderPath);

    Task<long> GetFileSizeAsync(string filePath);

    Task<string> GetContentTypeAsync(string filePath);

    Task<bool> UpdateMetadataAsync(string filePath, Dictionary<string, string> metadata);

    Task<Dictionary<string, string>> GetMetadataAsync(string filePath);
} 