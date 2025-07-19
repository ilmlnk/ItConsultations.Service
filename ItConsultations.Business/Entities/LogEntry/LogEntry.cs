using System.ComponentModel.DataAnnotations;

namespace ItConsultations.Business.Entities.LogEntry;

public class LogEntry : Entity<long>
{
    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    [MaxLength(20)]
    public string LogLevel { get; set; } = string.Empty;

    [Required]
    [MaxLength(4000)]
    public string Message { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string Exception { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Source { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string StackTrace { get; set; } = string.Empty;

    [MaxLength(100)]
    public string UserId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string SessionId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string RequestId { get; set; } = string.Empty;
} 