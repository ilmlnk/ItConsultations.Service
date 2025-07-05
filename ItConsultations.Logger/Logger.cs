using Microsoft.Extensions.Logging;
using log4net;

namespace ItConsultations.Logger;

public class Logger : ILogger
{
    private readonly ILog _log;
    private readonly string _categoryName;

    public Logger(string categoryName)
    {
        _categoryName = categoryName;
        _log = LogManager.GetLogger(categoryName);
    }

    public IDisposable? BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Critical => _log.IsFatalEnabled,
            LogLevel.Error => _log.IsErrorEnabled,
            LogLevel.Debug => _log.IsDebugEnabled,
            LogLevel.Trace => _log.IsDebugEnabled,
            LogLevel.Warning => _log.IsWarnEnabled,
            LogLevel.Information => _log.IsInfoEnabled,
            LogLevel.None => false,
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }

    public void Log<TState>(
        LogLevel logLevel, 
        EventId eventId, 
        TState state, 
        Exception? exception, 
        Func<TState, Exception?, string> formatter
        )
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            return;
        }

        var message = formatter(state, exception);
        var formattedMessage = $"[{_categoryName}] {message}";

        switch (logLevel)
        {
            case LogLevel.Critical:
                _log.Fatal(formattedMessage, exception);
                break;
            case LogLevel.Error:
                _log.Error(formattedMessage, exception);
                break;
            case LogLevel.Warning:
                _log.Warn(formattedMessage, exception);
                break;
            case LogLevel.Information:
                _log.Info(formattedMessage, exception);
                break;
            case LogLevel.Debug:
                _log.Debug(formattedMessage, exception);
                break;
            case LogLevel.Trace:
                _log.Debug(formattedMessage, exception);
                break;
        }
    }
}
