using ItConsultations.Logger.Configs;
using ItConsultations.Logger.Models;
using ItConsultations.Logger.Providers;
using log4net.Core;

namespace ItConsultations.Logger.Services;

public class LoggingService : ILoggingService
{
    private readonly LogConfigs _config;
    private readonly IFileLoggerProvider _fileLoggerProvider;
    private readonly IDatabaseLoggerProvider _databaseLoggerProvider;
    private readonly ILogger _logger;

    public LoggingService(LogConfigs config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        
        _fileLoggerProvider = new FileLoggerProvider(config);
        
        if (_config.EnableDatabaseLogging && !string.IsNullOrEmpty(_config.DatabaseConnectionString))
        {
            _databaseLoggerProvider = new DatabaseLoggerProvider(config);
        }
        
        var loggerFactory = new Factory.LoggerFactory(config);
        //_logger = loggerFactory.CreateLogger<LoggingService>();
    }

    public async Task LogAsync(string message, LogLevel level = LogLevel.Info, Exception? exception = null)
    {
        var logEntry = new LogEntry
        {
            Message = message,
            LogLevel = level,
            Exception = exception?.ToString() ?? string.Empty,
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = "Application"
        };

        await LogAsync(logEntry);
    }

    public async Task LogAsync(string message, LogLevel level, string source, Exception? exception = null)
    {
        var logEntry = new LogEntry
        {
            Message = message,
            LogLevel = level,
            Exception = exception?.ToString() ?? string.Empty,
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = source
        };

        await LogAsync(logEntry);
    }

    public async Task LogAsync(LogEntry logEntry)
    {
        if (logEntry == null)
            throw new ArgumentNullException(nameof(logEntry));

        var tasks = new List<Task>();

        if (_config.EnableFileLogging)
        {
            tasks.Add(_fileLoggerProvider.WriteLogAsync(logEntry));
        }

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            tasks.Add(_databaseLoggerProvider.WriteLogAsync(logEntry));
        }

        if (_config.EnableConsoleLogging)
        {
            var consoleMessage = FormatConsoleMessage(logEntry);
            tasks.Add(Task.Run(() => Console.WriteLine(consoleMessage)));
        }

        await Task.WhenAll(tasks);
    }

    public async Task<IEnumerable<LogEntry>> GetLogsAsync(DateTime from, DateTime to)
    {
        var logs = new List<LogEntry>();

        if (_config.EnableFileLogging)
        {
            try
            {
                var fileLogs = await _fileLoggerProvider.ReadLogsAsync(from, to);
                logs.AddRange(fileLogs);
            }
            catch (Exception ex)
            {
                await LogAsync($"Failed to read logs from file: {ex.Message}", LogLevel.Error);
            }
        }

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            try
            {
                var dbLogs = await _databaseLoggerProvider.ReadLogsAsync(from, to);
                logs.AddRange(dbLogs);
            }
            catch (Exception ex)
            {
                await LogAsync($"Failed to read logs from database: {ex.Message}", LogLevel.Error);
            }
        }

        return logs.OrderByDescending(l => l.Timestamp);
    }

    public async Task<IEnumerable<LogEntry>> GetLogsAsync(LogLevel level)
    {
        var logs = new List<LogEntry>();

        if (_config.EnableFileLogging)
        {
            try
            {
                var fileLogs = await _fileLoggerProvider.ReadLogsAsync(level);
                logs.AddRange(fileLogs);
            }
            catch (Exception ex)
            {
                await LogAsync($"Failed to read logs from file: {ex.Message}", LogLevel.Error);
            }
        }

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            try
            {
                var dbLogs = await _databaseLoggerProvider.ReadLogsAsync(level);
                logs.AddRange(dbLogs);
            }
            catch (Exception ex)
            {
                await LogAsync($"Failed to read logs from database: {ex.Message}", LogLevel.Error);
            }
        }

        return logs.OrderByDescending(l => l.Timestamp);
    }

    public async Task<IEnumerable<LogEntry>> GetLogsAsync(string source)
    {
        var logs = new List<LogEntry>();

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            try
            {
                var dbLogs = await _databaseLoggerProvider.ReadLogsAsync(source);
                logs.AddRange(dbLogs);
            }
            catch (Exception ex)
            {
                await LogAsync($"Failed to read logs from database: {ex.Message}", LogLevel.Error);
            }
        }

        return logs.OrderByDescending(l => l.Timestamp);
    }

    public async Task ClearLogsAsync(DateTime before)
    {
        var tasks = new List<Task>();

        if (_config.EnableFileLogging)
        {
            tasks.Add(_fileLoggerProvider.ClearLogsAsync());
        }

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            tasks.Add(_databaseLoggerProvider.ClearLogsAsync(before));
        }

        await Task.WhenAll(tasks);
    }

    public async Task<bool> IsHealthyAsync()
    {
        var healthChecks = new List<Task<bool>>();

        if (_config.EnableFileLogging)
        {
            healthChecks.Add(_fileLoggerProvider.IsLogFileAccessibleAsync());
        }

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            healthChecks.Add(_databaseLoggerProvider.IsDatabaseAccessibleAsync());
        }

        if (healthChecks.Count == 0)
            return false;

        var results = await Task.WhenAll(healthChecks);
        return results.All(r => r);
    }

    public async Task<int> GetLogCountAsync()
    {
        var count = 0;

        if (_config.EnableDatabaseLogging && _databaseLoggerProvider != null)
        {
            try
            {
                count = await _databaseLoggerProvider.GetLogCountAsync();
            }
            catch (Exception ex)
            {
                await LogAsync($"Failed to get log count from database: {ex.Message}", LogLevel.Error);
            }
        }

        return count;
    }

    private string FormatConsoleMessage(LogEntry logEntry)
    {
        var timestamp = logEntry.Timestamp.ToString(_config.DateTimeFormat);
        var level = logEntry.LogLevel.ToString().ToUpper().PadRight(5);
        var source = logEntry.Source ?? "Unknown";
        var message = logEntry.Message ?? "";

        var formattedMessage = $"[{timestamp}] [{level}] [{source}] {message}";

        if (!string.IsNullOrEmpty(logEntry.Exception))
        {
            formattedMessage += $"\nException: {logEntry.Exception}";
        }

        if (!string.IsNullOrEmpty(logEntry.StackTrace))
        {
            formattedMessage += $"\nStackTrace: {logEntry.StackTrace}";
        }

        return formattedMessage;
    }
} 