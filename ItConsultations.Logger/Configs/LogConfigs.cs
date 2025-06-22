using ItConsultations.Logger.Models;

namespace ItConsultations.Logger.Configs;

public class LogConfigs
{
    public LogLevel MinimumLevel { get; set; } = LogLevel.Info;

    public string LogFilePath { get; set; }

    public long? LogFileSize { get; set; }

    public long MaxFileSize { get; set; } = 10 * 1024 * 1024;

    public long MaxFileCount { get; set; } = 5;

    public string DatabaseConnectionString { get; set; }

    public bool EnableConsoleLogging { get; set; } = true;

    public bool EnableFileLogging { get; set; } = true;

    public bool EnableDatabaseLogging { get; set; } = false;

    public string DateTimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";
}
