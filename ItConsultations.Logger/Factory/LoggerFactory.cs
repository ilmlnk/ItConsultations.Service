using ItConsultations.Logger.Configs;
using Microsoft.Extensions.Logging;

namespace ItConsultations.Logger.Factory;

public class LoggerFactory
{
    private readonly LogConfigs _config;
    private readonly Dictionary<string, ILogger> _loggers;
    private readonly object _lockObject = new object();

    public LoggerFactory(LogConfigs config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _loggers = new Dictionary<string, ILogger>();
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (string.IsNullOrEmpty(categoryName))
        {
            categoryName = "Default";
        }

        lock (_lockObject)
        {
            if (_loggers.TryGetValue(categoryName, out var existingLogger))
            {
                return existingLogger;
            }

            var loggerProvider = new LoggerProvider(_config);
            var logger = loggerProvider.CreateLogger(categoryName);
            _loggers[categoryName] = logger;
            
            return logger;
        }
    }

    public ILogger CreateLogger<T>()
    {
        return CreateLogger(typeof(T).Name);
    }

    public ILogger CreateLogger(Type type)
    {
        return CreateLogger(type.Name);
    }

    public void ClearLoggers()
    {
        lock (_lockObject)
        {
            _loggers.Clear();
        }
    }

    public int LoggerCount => _loggers.Count;
} 