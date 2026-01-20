using ItConsultations.Business.Dtos.FileDtos;
using ItConsultations.Business.Services.FileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItConsultations.WebApi.Controllers;

[Route("api/files")]
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
        return Ok(result);
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string filePath)
    {
        var fileInfo = await _fileService.GetFileInfoAsync(filePath);
        var stream = await _fileService.DownloadAsync(filePath);
        return File(stream, fileInfo.ContentType, fileInfo.FileName);
    }

    [HttpGet("download-url")]
    public async Task<IActionResult> GetDownloadUrl(
        [FromQuery] string filePath,
        [FromQuery] int expirationMinutes = 60)
    {
        var url = await _fileService.GetDownloadUrlAsync(filePath, expirationMinutes);
        return Ok(new { DownloadUrl = url });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteFile([FromQuery] string filePath)
    {
        var result = await _fileService.DeleteAsync(filePath);
        return Ok(result);
    }

    [HttpGet("info")]
    public async Task<IActionResult> GetFileInfo([FromQuery] string filePath)
    {
        var fileInfo = await _fileService.GetFileInfoAsync(filePath);
        return Ok(fileInfo);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFiles([FromQuery] FileSearchDto searchDto)
    {
        var files = await _fileService.SearchFilesAsync(searchDto);
        return Ok(files);
    }

    [HttpPost("copy")]
    public async Task<IActionResult> CopyFile(
        [FromQuery] string sourcePath,
        [FromQuery] string destinationPath)
    {
        var result = await _fileService.CopyAsync(sourcePath, destinationPath);
        return Ok(result);
    }

    [HttpPost("move")]
    public async Task<IActionResult> MoveFile(
        [FromQuery] string sourcePath,
        [FromQuery] string destinationPath)
    {
        var result = await _fileService.MoveAsync(sourcePath, destinationPath);
        return Ok(result);
    }

    [HttpPut("metadata")]
    public async Task<IActionResult> UpdateMetadata(
        [FromQuery] string filePath,
        [FromBody] Dictionary<string, string> metadata)
    {
        var result = await _fileService.UpdateMetadataAsync(filePath, metadata);
        return Ok(result);
    }

    [HttpGet("metadata")]
    public async Task<IActionResult> GetMetadata([FromQuery] string filePath)
    {
        var metadata = await _fileService.GetMetadataAsync(filePath);
        return Ok(metadata);
    }

    [HttpGet("exists")]
    public async Task<IActionResult> FileExists([FromQuery] string filePath)
    {
        var exists = await _fileService.ExistsAsync(filePath);
        return Ok(exists);
    }
}