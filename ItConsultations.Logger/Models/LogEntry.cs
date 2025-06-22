namespace ItConsultations.Logger.Models;

public class LogEntry
{
    public DateTime Timestamp { get; set; }

    public LogLevel LogLevel { get; set; }

    public string Message { get; set; }

    public string Exception { get; set; }

    public string Source { get; set; }

    public string StackTrace { get; set; }

    public string UserId { get; set; }

    public string SessionId { get; set; }

    public string RequestId { get; set; }

    public LogEntry()
    {
        Timestamp = DateTime.UtcNow;
    }
}
