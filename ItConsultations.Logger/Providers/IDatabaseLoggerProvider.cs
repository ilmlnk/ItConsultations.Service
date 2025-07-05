using ItConsultations.Logger.Models;

namespace ItConsultations.Logger.Providers;

public interface IDatabaseLoggerProvider
{
    Task WriteLogAsync(LogEntry logEntry);
    Task WriteLogAsync(string message, LogLevel level = LogLevel.Info, Exception? exception = null);
    Task WriteLogAsync(string message, LogLevel level, string source, Exception? exception = null);
    Task<IEnumerable<LogEntry>> ReadLogsAsync(DateTime from, DateTime to);
    Task<IEnumerable<LogEntry>> ReadLogsAsync(LogLevel level);
    Task<IEnumerable<LogEntry>> ReadLogsAsync(string source);
    Task ClearLogsAsync(DateTime before);
    Task<bool> IsDatabaseAccessibleAsync();
    Task<int> GetLogCountAsync();
} 