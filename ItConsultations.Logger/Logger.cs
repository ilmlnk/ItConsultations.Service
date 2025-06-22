using Microsoft.Extensions.Logging;
using log4net;

namespace ItConsultations.Logger;

public class Logger : ILogger
{
    private readonly ILog _log;
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
        Func<TState, Exception?, 
        string> formatter
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
    }
}
