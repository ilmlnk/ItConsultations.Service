namespace ItConsultations.Business.Dtos.FileDtos;

public class FileSearchDto
{
    public string? FolderPath { get; set; }

    public string? SearchPattern { get; set; }

    public string? ContentType { get; set; }

    public long? MinFileSize { get; set; }

    public long? MaxFileSize { get; set; }

    public DateTime? CreatedFrom { get; set; }

    public DateTime? CreatedTo { get; set; }

    public bool? IsPublic { get; set; }

    public int PageSize { get; set; } = 20;

    public int PageNumber { get; set; } = 1;

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; } = "desc";
} 