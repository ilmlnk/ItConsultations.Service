using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.FileDtos;
using ItConsultations.Utilities.Guards;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace ItConsultations.Business.Services.FileService;

public class FileService : IFileService
{
    private readonly IFileStorage _fileStorage;
    private readonly IConfiguration _configuration;

    private readonly HashSet<string> _allowedContentTypes = new()
    {
        "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp", "image/svg+xml",
        "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        "text/plain", "text/csv",
        "application/zip", "application/x-rar-compressed", "application/x-7z-compressed",
        "video/mp4", "video/avi", "video/mov", "video/wmv", "video/webm",
        "audio/mpeg", "audio/wav", "audio/ogg", "audio/mp4"
    };

    private const long MaxFileSize = 100 * 1024 * 1024;

    public FileService(IFileStorage fileStorage, IConfiguration configuration)
    {
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public Task<FileDto> CreateAsync(CreateFileDto createFileDto)
    {
        throw new NotImplementedException();
    }

    public async Task<FileDto> UploadAsync(UploadFileDto uploadDto, Stream fileStream)
    {
        Guard.NotNull(uploadDto);
        Guard.NotNull(fileStream);

        if (!IsAllowedContentType(uploadDto.ContentType))
        {
            throw new ArgumentException($"Invalid file type: {uploadDto.ContentType}");
        }

        if (!IsAllowedFileSize(uploadDto.FileSize))
        {
            throw new ArgumentException($"File size exceeds the allowed limit: {uploadDto.FileSize} bytes");
        }

        var uniqueFileName = GenerateUniqueFileName(uploadDto.FileName);
        var filePath = string.IsNullOrEmpty(uploadDto.FilePath) 
            ? uniqueFileName 
            : Path.Combine(uploadDto.FilePath, uniqueFileName);

        var hash = await CalculateFileHashAsync(fileStream);
        fileStream.Position = 0;

        var metadata = uploadDto.Metadata ?? new Dictionary<string, string>();
        metadata["OriginalFileName"] = uploadDto.FileName;
        metadata["FileHash"] = hash;
        metadata["UploadedAt"] = DateTime.UtcNow.ToString("O");

        var uploadedFile = await _fileStorage.UploadAsync(filePath, fileStream, uploadDto.ContentType, metadata);

        uploadedFile.Hash = hash;
        uploadedFile.IsPublic = uploadDto.IsPublic;
        uploadedFile.ExpiresAt = uploadDto.ExpiresAt;

        return uploadedFile;
    }

    public async Task<Stream> DownloadAsync(string filePath)
    {
        Guard.NotNullOrEmpty(filePath);

        if (!await _fileStorage.ExistsAsync(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        return await _fileStorage.DownloadAsync(filePath);
    }

    public async Task<string> GetDownloadUrlAsync(string filePath, int expirationMinutes = 60)
    {
        Guard.NotNullOrEmpty(filePath);

        if (!await _fileStorage.ExistsAsync(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        return await _fileStorage.GetDownloadUrlAsync(filePath, expirationMinutes);
    }

    public async Task<bool> DeleteAsync(string filePath)
    {
        Guard.NotNullOrEmpty(filePath);

        return await _fileStorage.DeleteAsync(filePath);
    }

    public async Task<bool> ExistsAsync(string filePath)
    {
        Guard.NotNullOrEmpty(filePath);

        return await _fileStorage.ExistsAsync(filePath);
    }

    public async Task<FileDto?> GetFileInfoAsync(string filePath)
    {
        Guard.NotNullOrEmpty(filePath);

        return await _fileStorage.GetFileInfoAsync(filePath);
    }

    public async Task<IEnumerable<FileDto>> SearchFilesAsync(FileSearchDto searchDto)
    {
        Guard.NotNull(searchDto);

        var files = await _fileStorage.ListFilesAsync(
            searchDto.FolderPath ?? string.Empty, 
            searchDto.SearchPattern);

        var result = files.AsEnumerable();

        if (!string.IsNullOrEmpty(searchDto.ContentType))
        {
            result = result.Where(f => f.ContentType == searchDto.ContentType);
        }

        if (searchDto.MinFileSize.HasValue)
        {
            result = result.Where(f => f.FileSize >= searchDto.MinFileSize.Value);
        }

        if (searchDto.MaxFileSize.HasValue)
        {
            result = result.Where(f => f.FileSize <= searchDto.MaxFileSize.Value);
        }

        if (searchDto.CreatedFrom.HasValue)
        {
            result = result.Where(f => f.CreatedAt >= searchDto.CreatedFrom.Value);
        }

        if (searchDto.CreatedTo.HasValue)
        {
            result = result.Where(f => f.CreatedAt <= searchDto.CreatedTo.Value);
        }

        if (searchDto.IsPublic.HasValue)
        {
            result = result.Where(f => f.IsPublic == searchDto.IsPublic.Value);
        }

        if (!string.IsNullOrEmpty(searchDto.SortBy))
        {
            result = searchDto.SortBy.ToLower() switch
            {
                "name" => searchDto.SortDirection == "asc" 
                    ? result.OrderBy(f => f.Name) 
                    : result.OrderByDescending(f => f.Name),
                "size" => searchDto.SortDirection == "asc" 
                    ? result.OrderBy(f => f.FileSize) 
                    : result.OrderByDescending(f => f.FileSize),
                "createdat" => searchDto.SortDirection == "asc" 
                    ? result.OrderBy(f => f.CreatedAt) 
                    : result.OrderByDescending(f => f.CreatedAt),
                _ => result.OrderByDescending(f => f.CreatedAt)
            };
        }
        else
        {
            result = result.OrderByDescending(f => f.CreatedAt);
        }

        var skip = (searchDto.PageNumber - 1) * searchDto.PageSize;
        return result.Skip(skip).Take(searchDto.PageSize);
    }

    public async Task<FileDto> CopyAsync(string sourcePath, string destinationPath)
    {
        Guard.NotNullOrEmpty(sourcePath);
        Guard.NotNullOrEmpty(destinationPath);

        if (!await _fileStorage.ExistsAsync(sourcePath))
        {
            throw new FileNotFoundException($"Исходный файл не найден: {sourcePath}");
        }

        return await _fileStorage.CopyAsync(sourcePath, destinationPath);
    }

    public async Task<FileDto> MoveAsync(string sourcePath, string destinationPath)
    {
        Guard.NotNullOrEmpty(sourcePath);
        Guard.NotNullOrEmpty(destinationPath);

        if (!await _fileStorage.ExistsAsync(sourcePath))
        {
            throw new FileNotFoundException($"Исходный файл не найден: {sourcePath}");
        }

        return await _fileStorage.MoveAsync(sourcePath, destinationPath);
    }

    public async Task<bool> UpdateMetadataAsync(string filePath, Dictionary<string, string> metadata)
    {
        Guard.NotNullOrEmpty(filePath);
        Guard.NotNull(metadata);

        if (!await _fileStorage.ExistsAsync(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        return await _fileStorage.UpdateMetadataAsync(filePath, metadata);
    }

    public async Task<Dictionary<string, string>> GetMetadataAsync(string filePath)
    {
        Guard.NotNullOrEmpty(filePath);

        if (!await _fileStorage.ExistsAsync(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        return await _fileStorage.GetMetadataAsync(filePath);
    }

    public string GenerateUniqueFileName(string originalFileName)
    {
        Guard.NotNullOrEmpty(originalFileName);

        var extension = Path.GetExtension(originalFileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
        var guid = Guid.NewGuid().ToString("N")[..8];
        
        return $"{nameWithoutExtension}_{timestamp}_{guid}{extension}";
    }

    public bool IsAllowedContentType(string contentType)
    {
        return !string.IsNullOrEmpty(contentType) && _allowedContentTypes.Contains(contentType.ToLower());
    }

    public bool IsAllowedFileSize(long fileSize)
    {
        return fileSize > 0 && fileSize <= MaxFileSize;
    }

    private async Task<string> CalculateFileHashAsync(Stream fileStream)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = await sha256.ComputeHashAsync(fileStream);
        return Convert.ToHexString(hashBytes).ToLower();
    }
} 