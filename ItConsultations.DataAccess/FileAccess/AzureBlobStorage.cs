using ItConsultations.Business.DataAccess.Interfaces;
using ItConsultations.Business.Dtos.FileDtos;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace ItConsultations.DataAccess.FileAccess;

public class AzureBlobStorage : IFileStorage
{
    private readonly string _connectionString;
    private readonly string _containerName;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _containerClient;

    public AzureBlobStorage(string connectionString, string containerName) 
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _containerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
        
        _blobServiceClient = new BlobServiceClient(_connectionString);
        _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
    }

    public async Task<FileDto> UploadAsync(string fileName, Stream fileStream, string contentType, Dictionary<string, string>? metadata = null)
    {
        await EnsureContainerExistsAsync();
        
        var blobClient = _containerClient.GetBlobClient(fileName);
        var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };
        
        await blobClient.UploadAsync(fileStream, blobHttpHeaders, metadata);
        
        var properties = await blobClient.GetPropertiesAsync();
        
        return new FileDto
        {
            Name = Path.GetFileName(fileName),
            FileName = fileName,
            FilePath = fileName,
            ContentType = contentType,
            FileSize = properties.Value.ContentLength,
            CreatedAt = properties.Value.CreatedOn.UtcDateTime,
            UpdatedAt = properties.Value.LastModified.UtcDateTime,
            Metadata = metadata,
            Url = blobClient.Uri.ToString()
        };
    }

    public async Task<Stream> DownloadAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        
        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
        
        var response = await blobClient.DownloadAsync();
        return response.Value.Content;
    }

    public async Task<string> GetDownloadUrlAsync(string filePath, int expirationMinutes = 60)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        
        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }
        
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _containerName,
            BlobName = filePath,
            Resource = "b", // blob
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(expirationMinutes)
        };
        
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        
        var sasToken = sasBuilder.ToSasQueryParameters(
            new Azure.Storage.StorageSharedKeyCredential(
                _blobServiceClient.AccountName, 
                ExtractAccountKey(_connectionString)
            )).ToString();
        
        return $"{blobClient.Uri}?{sasToken}";
    }

    public async Task<bool> DeleteAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        var response = await blobClient.DeleteIfExistsAsync();
        return response.Value;
    }

    public async Task<bool> ExistsAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        return await blobClient.ExistsAsync();
    }

    public async Task<FileDto?> GetFileInfoAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        
        if (!await blobClient.ExistsAsync())
        {
            return null;
        }
        
        var properties = await blobClient.GetPropertiesAsync();
        
        return new FileDto
        {
            Name = Path.GetFileName(filePath),
            FileName = filePath,
            FilePath = filePath,
            ContentType = properties.Value.ContentType,
            FileSize = properties.Value.ContentLength,
            CreatedAt = properties.Value.CreatedOn.UtcDateTime,
            UpdatedAt = properties.Value.LastModified.UtcDateTime,
            Metadata = properties.Value.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
            Url = blobClient.Uri.ToString()
        };
    }

    public async Task<FileDto> CopyAsync(string sourcePath, string destinationPath)
    {
        var sourceBlob = _containerClient.GetBlobClient(sourcePath);
        var destinationBlob = _containerClient.GetBlobClient(destinationPath);
        
        if (!await sourceBlob.ExistsAsync())
        {
            throw new FileNotFoundException($"Source file not found: {sourcePath}");
        }
        
        await destinationBlob.StartCopyFromUriAsync(sourceBlob.Uri);
        
        var properties = await destinationBlob.GetPropertiesAsync();
        while (properties.Value.CopyStatus == CopyStatus.Pending)
        {
            await Task.Delay(100);
            properties = await destinationBlob.GetPropertiesAsync();
        }
        
        if (properties.Value.CopyStatus == CopyStatus.Failed)
        {
            throw new Exception($"Error copying file: {properties.Value.CopyStatusDescription}");
        }
        
        return new FileDto
        {
            Name = Path.GetFileName(destinationPath),
            FileName = destinationPath,
            FilePath = destinationPath,
            ContentType = properties.Value.ContentType,
            FileSize = properties.Value.ContentLength,
            CreatedAt = properties.Value.CreatedOn.UtcDateTime,
            UpdatedAt = properties.Value.LastModified.UtcDateTime,
            Url = destinationBlob.Uri.ToString()
        };
    }

    public async Task<FileDto> MoveAsync(string sourcePath, string destinationPath)
    {
        var copiedFile = await CopyAsync(sourcePath, destinationPath);
        await DeleteAsync(sourcePath);
        return copiedFile;
    }

    public async Task<IEnumerable<FileDto>> ListFilesAsync(string folderPath, string? searchPattern = null)
    {
        var files = new List<FileDto>();
        var prefix = string.IsNullOrEmpty(folderPath) ? null : folderPath.TrimEnd('/') + "/";
        
        await foreach (var blobItem in _containerClient.GetBlobsAsync(prefix: prefix))
        {
            if (blobItem.Properties.ContentLength == null) 
            { 
                continue; 
            }
            
            var fileName = blobItem.Name;
            
            if (!string.IsNullOrEmpty(searchPattern))
            {
                var pattern = searchPattern.Replace("*", ".*").Replace("?", ".");
                if (!System.Text.RegularExpressions.Regex.IsMatch(Path.GetFileName(fileName), pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    continue;
                }
            }
            
            files.Add(new FileDto
            {
                Name = Path.GetFileName(fileName),
                FileName = fileName,
                FilePath = fileName,
                ContentType = blobItem.Properties.ContentType,
                FileSize = blobItem.Properties.ContentLength ?? 0,
                CreatedAt = blobItem.Properties.CreatedOn?.UtcDateTime ?? DateTime.UtcNow,
                UpdatedAt = blobItem.Properties.LastModified?.UtcDateTime,
                Url = $"{_containerClient.Uri}/{fileName}"
            });
        }
        
        return files;
    }

    public async Task<bool> CreateFolderAsync(string folderPath)
    {
        var folderMarkerPath = folderPath.TrimEnd('/') + "/.folder";
        var blobClient = _containerClient.GetBlobClient(folderMarkerPath);
        
        using var emptyStream = new MemoryStream();
        await blobClient.UploadAsync(emptyStream, overwrite: true);
        
        return true;
    }

    public async Task<bool> DeleteFolderAsync(string folderPath)
    {
        var prefix = folderPath.TrimEnd('/') + "/";
        var deletedCount = 0;
        
        await foreach (var blobItem in _containerClient.GetBlobsAsync(prefix: prefix))
        {
            var blobClient = _containerClient.GetBlobClient(blobItem.Name);
            await blobClient.DeleteIfExistsAsync();
            deletedCount++;
        }
        
        return deletedCount > 0;
    }

    public async Task<long> GetFileSizeAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        var properties = await blobClient.GetPropertiesAsync();
        return properties.Value.ContentLength;
    }

    public async Task<string> GetContentTypeAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        var properties = await blobClient.GetPropertiesAsync();
        return properties.Value.ContentType ?? "application/octet-stream";
    }

    public async Task<bool> UpdateMetadataAsync(string filePath, Dictionary<string, string> metadata)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        
        if (!await blobClient.ExistsAsync())
        {
            return false;
        }
        
        await blobClient.SetMetadataAsync(metadata);
        return true;
    }

    public async Task<Dictionary<string, string>> GetMetadataAsync(string filePath)
    {
        var blobClient = _containerClient.GetBlobClient(filePath);
        var properties = await blobClient.GetPropertiesAsync();
        return properties.Value.Metadata?.ToDictionary(kvp => kvp.Key, kvp => kvp.Value) ?? new Dictionary<string, string>();
    }

    private async Task EnsureContainerExistsAsync()
    {
        await _containerClient.CreateIfNotExistsAsync();
    }

    private string ExtractAccountKey(string connectionString)
    {
        var parts = connectionString.Split(';');
        var accountKeyPart = parts.FirstOrDefault(p => p.StartsWith("AccountKey="));
        
        if (string.IsNullOrEmpty(accountKeyPart))
        {
            throw new ArgumentException("Failed to find account key in connection string");
        }
        
        return accountKeyPart.Substring("AccountKey=".Length);
    }
}