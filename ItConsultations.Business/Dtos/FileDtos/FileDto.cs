namespace ItConsultations.Business.Dtos.FileDtos;

public class FileDto
{
    public long Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public string? Url { get; set; }

    public string ContentType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Dictionary<string, string>? Metadata { get; set; }

    public string? Hash { get; set; }

    public bool IsPublic { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
