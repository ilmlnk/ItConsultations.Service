using ItConsultations.Business.Dtos.FileDtos;
using ItConsultations.Business.Services.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileController> _logger;

    public FileController(IFileService fileService, ILogger<FileController> logger)
    {
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(
        IFormFile file,
        [FromQuery] string? folderPath = null,
        [FromQuery] bool isPublic = false)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File not selected or empty.");
            }

            var uploadDto = new UploadFileDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                FileSize = file.Length,
                FilePath = folderPath,
                IsPublic = isPublic
            };

            using var stream = file.OpenReadStream();
            var result = await _fileService.UploadAsync(uploadDto, stream);

            _logger.LogInformation("File {FileName} successfully uploaded", file.FileName);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error when uploading file {FileName}", file?.FileName);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file {FileName}", file?.FileName);
            return StatusCode(500, "Internal server error when uploading file");
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            var fileInfo = await _fileService.GetFileInfoAsync(filePath);

            if (fileInfo == null)
            {
                return NotFound("File not found");
            }

            var stream = await _fileService.DownloadAsync(filePath);
            return File(stream, fileInfo.ContentType, fileInfo.FileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downloading file {FilePath}", filePath);
            return StatusCode(500, "Internal server error when downloading file");
        }
    }

    [HttpGet("download-url")]
    public async Task<IActionResult> GetDownloadUrl(
        [FromQuery] string filePath,
        [FromQuery] int expirationMinutes = 60)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            var url = await _fileService.GetDownloadUrlAsync(filePath, expirationMinutes);
            return Ok(new { DownloadUrl = url });
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting download URL for file {FilePath}", filePath);
            return StatusCode(500, "Internal server error when getting download URL");
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFile([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            var result = await _fileService.DeleteAsync(filePath);
            
            if (result)
            {
                _logger.LogInformation("File {FilePath} successfully deleted", filePath);
                return Ok(result);
            }
            else
            {
                return NotFound("File not found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file {FilePath}", filePath);
            return StatusCode(500, "Internal server error when deleting file");
        }
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetFileInfo([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            var fileInfo = await _fileService.GetFileInfoAsync(filePath);

            if (fileInfo == null)
            {
                return NotFound("File not found");
            }

            return Ok(fileInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file info {FilePath}", filePath);
            return StatusCode(500, "Internal server error when getting file info");
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFiles([FromQuery] FileSearchDto searchDto)
    {
        try
        {
            var files = await _fileService.SearchFilesAsync(searchDto);
            return Ok(files);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching files");
            return StatusCode(500, "Internal server error when searching files");
        }
    }

    [HttpPost("copy")]
    public async Task<IActionResult> CopyFile(
        [FromQuery] string sourcePath,
        [FromQuery] string destinationPath)
    {
        try
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destinationPath))
            {
                return BadRequest("Source and destination paths must be specified");
            }

            var result = await _fileService.CopyAsync(sourcePath, destinationPath);
            _logger.LogInformation("File {SourcePath} copied to {DestinationPath}", sourcePath, destinationPath);
            return Ok(result);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error copying file {SourcePath} to {DestinationPath}", sourcePath, destinationPath);
            return StatusCode(500, "Internal server error when copying file");
        }
    }

    [HttpPost("move")]
    public async Task<IActionResult> MoveFile(
        [FromQuery] string sourcePath,
        [FromQuery] string destinationPath)
    {
        try
        {
            if (string.IsNullOrEmpty(sourcePath) || string.IsNullOrEmpty(destinationPath))
            {
                return BadRequest("Source and destination paths must be specified");
            }

            var result = await _fileService.MoveAsync(sourcePath, destinationPath);
            _logger.LogInformation("File {SourcePath} moved to {DestinationPath}", sourcePath, destinationPath);
            return Ok(result);
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error moving file {SourcePath} to {DestinationPath}", sourcePath, destinationPath);
            return StatusCode(500, "Internal server error when moving file");
        }
    }

    [HttpPut("metadata")]
    public async Task<IActionResult> UpdateMetadata(
        [FromQuery] string filePath,
        [FromBody] Dictionary<string, string> metadata)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            if (metadata == null || !metadata.Any())
            {
                return BadRequest("Metadata not specified");
            }

            var result = await _fileService.UpdateMetadataAsync(filePath, metadata);
            
            if (result)
            {
                _logger.LogInformation("File metadata {FilePath} updated", filePath);
                return Ok(result);
            }
            else
            {
                return NotFound("File not found");
            }
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating file metadata {FilePath}", filePath);
            return StatusCode(500, "Internal server error when updating file metadata");
        }
    }

    [HttpGet("metadata")]
    public async Task<IActionResult> GetMetadata([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            var metadata = await _fileService.GetMetadataAsync(filePath);
            return Ok(metadata);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file metadata {FilePath}", filePath);
            return StatusCode(500, "Internal server error when getting file metadata");
        }
    }

    [HttpGet("exists")]
    public async Task<IActionResult> FileExists([FromQuery] string filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return BadRequest("File path not specified");
            }

            var exists = await _fileService.ExistsAsync(filePath);
            return Ok(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if file exists {FilePath}", filePath);
            return StatusCode(500, "Internal server error when checking if file exists");
        }
    }
} 