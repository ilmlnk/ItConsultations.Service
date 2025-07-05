using Microsoft.Extensions.Logging;
using ItConsultations.Logger.Configs;

namespace ItConsultations.Logger;

public class LoggerProvider : ILoggerProvider
{
    private readonly LogConfigs _config;
    private readonly Dictionary<string, Logger> _loggers;
    private bool _disposed;

    public LoggerProvider(LogConfigs config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _loggers = new Dictionary<string, Logger>();
        
        InitializeLog4Net();
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            categoryName = "Default";
        }

        lock (_loggers)
        {
            if (_loggers.TryGetValue(categoryName, out var existingLogger))
            {
                return existingLogger;
            }

            var logger = new Logger(categoryName);
            _loggers[categoryName] = logger;
            return logger;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            lock (_loggers)
            {
                _loggers.Clear();
            }
            _disposed = true;
        }
    }

    private void InitializeLog4Net()
    {
        try
        {
            var logRepository = log4net.LogManager.GetRepository();
            
            //var config = new log4net.Config.BasicConfigurator();
            
            if (_config.EnableFileLogging && !string.IsNullOrEmpty(_config.LogFilePath))
            {
                var fileAppender = new log4net.Appender.RollingFileAppender
                {
                    File = _config.LogFilePath,
                    AppendToFile = true,
                    RollingStyle = log4net.Appender.RollingFileAppender.RollingMode.Size,
                    MaxSizeRollBackups = (int)_config.MaxFileCount,
                    MaximumFileSize = _config.MaxFileSize.ToString(),
                    Layout = new log4net.Layout.PatternLayout("%date [%thread] %-5level %logger - %message%newline")
                };
                fileAppender.ActivateOptions();
                //logRepository.Root.AddAppender(fileAppender);
            }

            if (_config.EnableConsoleLogging)
            {
                var consoleAppender = new log4net.Appender.ConsoleAppender
                {
                    Layout = new log4net.Layout.PatternLayout("%date [%thread] %-5level %logger - %message%newline")
                };
                consoleAppender.ActivateOptions();
                //logRepository.Root.AddAppender(consoleAppender);
            }

            logRepository.Threshold = ConvertToLog4NetLevel(_config.MinimumLevel);
            
            //config.Configure(logRepository);
        }
        catch (Exception ex)
        {
            // Fallback к базовой конфигурации
            log4net.Config.BasicConfigurator.Configure();
            System.Diagnostics.Debug.WriteLine($"Failed to initialize log4net: {ex.Message}");
        }
    }

    private log4net.Core.Level ConvertToLog4NetLevel(Models.LogLevel level)
    {
        return level switch
        {
            Models.LogLevel.Debug => log4net.Core.Level.Debug,
            Models.LogLevel.Info => log4net.Core.Level.Info,
            Models.LogLevel.Warn => log4net.Core.Level.Warn,
            Models.LogLevel.Error => log4net.Core.Level.Error,
            _ => log4net.Core.Level.Info
        };
    }
}
