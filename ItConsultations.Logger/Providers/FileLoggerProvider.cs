using ItConsultations.Logger.Configs;
using ItConsultations.Logger.Models;
using System.Text.Json;

namespace ItConsultations.Logger.Providers;

public class FileLoggerProvider : IFileLoggerProvider
{
    private readonly LogConfigs _config;
    private readonly string _logFilePath;
    private readonly SemaphoreSlim _fileLock = new SemaphoreSlim(1, 1);

    public FileLoggerProvider(LogConfigs config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _logFilePath = GetLogFilePath();
        EnsureLogDirectoryExists();
    }

    public async Task WriteLogAsync(LogEntry logEntry)
    {
        if (logEntry == null)
            throw new ArgumentNullException(nameof(logEntry));

        await _fileLock.WaitAsync();
        try
        {
            var logLine = FormatLogEntry(logEntry);
            await File.AppendAllTextAsync(_logFilePath, logLine + Environment.NewLine);
            
            // Проверка размера файла и ротация при необходимости
            await CheckAndRotateLogFile();
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task WriteLogAsync(string message, LogLevel level = LogLevel.Info, Exception? exception = null)
    {
        var logEntry = new LogEntry
        {
            Message = message,
            LogLevel = level,
            Exception = exception?.ToString() ?? string.Empty,
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = "Application"
        };

        await WriteLogAsync(logEntry);
    }

    public async Task WriteLogAsync(string message, LogLevel level, string source, Exception? exception = null)
    {
        var logEntry = new LogEntry
        {
            Message = message,
            LogLevel = level,
            Exception = exception?.ToString() ?? string.Empty,
            StackTrace = exception?.StackTrace ?? string.Empty,
            Source = source
        };

        await WriteLogAsync(logEntry);
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync(DateTime from, DateTime to)
    {
        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(_logFilePath))
                return Enumerable.Empty<LogEntry>();

            var lines = await File.ReadAllLinesAsync(_logFilePath);
            var logs = new List<LogEntry>();

            foreach (var line in lines)
            {
                try
                {
                    var logEntry = ParseLogEntry(line);
                    if (logEntry != null && logEntry.Timestamp >= from && logEntry.Timestamp <= to)
                    {
                        logs.Add(logEntry);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return logs.OrderByDescending(l => l.Timestamp);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync(LogLevel level)
    {
        await _fileLock.WaitAsync();
        try
        {
            if (!File.Exists(_logFilePath))
                return Enumerable.Empty<LogEntry>();

            var lines = await File.ReadAllLinesAsync(_logFilePath);
            var logs = new List<LogEntry>();

            foreach (var line in lines)
            {
                try
                {
                    var logEntry = ParseLogEntry(line);
                    if (logEntry != null && logEntry.LogLevel == level)
                    {
                        logs.Add(logEntry);
                    }
                }
                catch
                {
                    continue;
                }
            }

            return logs.OrderByDescending(l => l.Timestamp);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task ClearLogsAsync()
    {
        await _fileLock.WaitAsync();
        try
        {
            if (File.Exists(_logFilePath))
            {
                await File.WriteAllTextAsync(_logFilePath, string.Empty);
            }
        }
        finally
        {
            _fileLock.Release();
        }
    }

    public async Task<bool> IsLogFileAccessibleAsync()
    {
        try
        {
            await _fileLock.WaitAsync();
            try
            {
                var testContent = $"Test write at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}";
                await File.AppendAllTextAsync(_logFilePath, testContent + Environment.NewLine);
                return true;
            }
            finally
            {
                _fileLock.Release();
            }
        }
        catch
        {
            return false;
        }
    }

    private string GetLogFilePath()
    {
        if (!string.IsNullOrEmpty(_config.LogFilePath))
        {
            return _config.LogFilePath;
        }

        var logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        var fileName = $"ItConsultations_{DateTime.Now:yyyy-MM-dd}.log";
        return Path.Combine(logDirectory, fileName);
    }

    private void EnsureLogDirectoryExists()
    {
        var directory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    private string FormatLogEntry(LogEntry logEntry)
    {
        var json = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions
        {
            WriteIndented = false
        });
        return json;
    }

    private LogEntry? ParseLogEntry(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        try
        {
            return JsonSerializer.Deserialize<LogEntry>(line);
        }
        catch
        {
            return null;
        }
    }

    private async Task CheckAndRotateLogFile()
    {
        if (!File.Exists(_logFilePath))
            return;

        var fileInfo = new FileInfo(_logFilePath);
        if (fileInfo.Length <= _config.MaxFileSize)
            return;

        var backupPath = _logFilePath.Replace(".log", $"_{DateTime.Now:yyyyMMdd_HHmmss}.log");
        File.Move(_logFilePath, backupPath);

        var logDirectory = Path.GetDirectoryName(_logFilePath);
        if (!string.IsNullOrEmpty(logDirectory))
        {
            var logFiles = Directory.GetFiles(logDirectory, "*.log")
                .OrderByDescending(f => f)
                .Skip((int)_config.MaxFileCount);

            foreach (var oldFile in logFiles)
            {
                try
                {
                    File.Delete(oldFile);
                }
                catch
                {
                }
            }
        }
    }
} 