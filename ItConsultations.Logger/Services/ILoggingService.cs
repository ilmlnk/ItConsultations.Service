using ItConsultations.Logger.Models;

namespace ItConsultations.Logger.Services;

public interface ILoggingService
{
    Task LogAsync(string message, LogLevel level = LogLevel.Info, Exception? exception = null);

    Task LogAsync(string message, LogLevel level, string source, Exception? exception = null);

    Task LogAsync(LogEntry logEntry);

    Task<IEnumerable<LogEntry>> GetLogsAsync(DateTime from, DateTime to);

    Task<IEnumerable<LogEntry>> GetLogsAsync(LogLevel level);

    Task<IEnumerable<LogEntry>> GetLogsAsync(string source);

    Task ClearLogsAsync(DateTime before);

    Task<bool> IsHealthyAsync();

    Task<int> GetLogCountAsync();
} 