namespace ItConsultations.Business.Dtos.FileDtos;

public class UploadFileDto
{
    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public string? FilePath { get; set; }

    public Dictionary<string, string>? Metadata { get; set; }

    public bool IsPublic { get; set; }

    public DateTime? ExpiresAt { get; set; }
} 