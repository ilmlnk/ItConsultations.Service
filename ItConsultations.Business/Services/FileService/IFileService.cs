using ItConsultations.Business.Dtos.FileDtos;

namespace ItConsultations.Business.Services.FileService;

public interface IFileService
{
    Task<FileDto> CreateAsync(CreateFileDto createFileDto);
    
    Task<FileDto> UploadAsync(UploadFileDto uploadDto, Stream fileStream);

    Task<Stream> DownloadAsync(string filePath);

    Task<string> GetDownloadUrlAsync(string filePath, int expirationMinutes = 60);

    Task<bool> DeleteAsync(string filePath);

    Task<bool> ExistsAsync(string filePath);

    Task<FileDto?> GetFileInfoAsync(string filePath);

    Task<IEnumerable<FileDto>> SearchFilesAsync(FileSearchDto searchDto);

    Task<FileDto> CopyAsync(string sourcePath, string destinationPath);

    Task<FileDto> MoveAsync(string sourcePath, string destinationPath);

    Task<bool> UpdateMetadataAsync(string filePath, Dictionary<string, string> metadata);

    Task<Dictionary<string, string>> GetMetadataAsync(string filePath);

    string GenerateUniqueFileName(string originalFileName);

    bool IsAllowedContentType(string contentType);

    bool IsAllowedFileSize(long fileSize);
} 