using ItConsultations.Logger.Configs;
using ItConsultations.Logger.Models;
using Npgsql;
using System.Data;

namespace ItConsultations.Logger.Providers;

public class DatabaseLoggerProvider : IDatabaseLoggerProvider
{
    private readonly LogConfigs _config;
    private readonly string _connectionString;

    public DatabaseLoggerProvider(LogConfigs config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _connectionString = config.DatabaseConnectionString ?? throw new ArgumentException("Database connection string is required");
    }

    public async Task WriteLogAsync(LogEntry logEntry)
    {
        if (logEntry == null)
            throw new ArgumentNullException(nameof(logEntry));

        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO LogEntries (Timestamp, LogLevel, Message, Exception, Source, StackTrace, UserId, SessionId, RequestId)
                VALUES (@Timestamp, @LogLevel, @Message, @Exception, @Source, @StackTrace, @UserId, @SessionId, @RequestId)";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Timestamp", logEntry.Timestamp);
            command.Parameters.AddWithValue("@LogLevel", logEntry.LogLevel.ToString());
            command.Parameters.AddWithValue("@Message", logEntry.Message ?? "");
            command.Parameters.AddWithValue("@Exception", logEntry.Exception ?? "");
            command.Parameters.AddWithValue("@Source", logEntry.Source ?? "");
            command.Parameters.AddWithValue("@StackTrace", logEntry.StackTrace ?? "");
            command.Parameters.AddWithValue("@UserId", logEntry.UserId ?? "");
            command.Parameters.AddWithValue("@SessionId", logEntry.SessionId ?? "");
            command.Parameters.AddWithValue("@RequestId", logEntry.RequestId ?? "");

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to write log to database: {ex.Message}");
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
        var logs = new List<LogEntry>();

        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT Timestamp, LogLevel, Message, Exception, Source, StackTrace, UserId, SessionId, RequestId
                FROM LogEntries 
                WHERE Timestamp BETWEEN @From AND @To
                ORDER BY Timestamp DESC";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@From", from);
            command.Parameters.AddWithValue("@To", to);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new LogEntry
                {
                    Timestamp = reader.GetDateTime("Timestamp"),
                    LogLevel = Enum.Parse<LogLevel>(reader.GetString("LogLevel")),
                    Message = reader.GetString("Message"),
                    Exception = reader.GetString("Exception"),
                    Source = reader.GetString("Source"),
                    StackTrace = reader.GetString("StackTrace"),
                    UserId = reader.GetString("UserId"),
                    SessionId = reader.GetString("SessionId"),
                    RequestId = reader.GetString("RequestId")
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to read logs from database: {ex.Message}");
        }

        return logs;
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync(LogLevel level)
    {
        var logs = new List<LogEntry>();

        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT Timestamp, LogLevel, Message, Exception, Source, StackTrace, UserId, SessionId, RequestId
                FROM LogEntries 
                WHERE LogLevel = @LogLevel
                ORDER BY Timestamp DESC";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@LogLevel", level.ToString());

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new LogEntry
                {
                    Timestamp = reader.GetDateTime("Timestamp"),
                    LogLevel = Enum.Parse<LogLevel>(reader.GetString("LogLevel")),
                    Message = reader.GetString("Message"),
                    Exception = reader.GetString("Exception"),
                    Source = reader.GetString("Source"),
                    StackTrace = reader.GetString("StackTrace"),
                    UserId = reader.GetString("UserId"),
                    SessionId = reader.GetString("SessionId"),
                    RequestId = reader.GetString("RequestId")
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to read logs from database: {ex.Message}");
        }

        return logs;
    }

    public async Task<IEnumerable<LogEntry>> ReadLogsAsync(string source)
    {
        var logs = new List<LogEntry>();

        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
                SELECT Timestamp, LogLevel, Message, Exception, Source, StackTrace, UserId, SessionId, RequestId
                FROM LogEntries 
                WHERE Source = @Source
                ORDER BY Timestamp DESC";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Source", source);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                logs.Add(new LogEntry
                {
                    Timestamp = reader.GetDateTime("Timestamp"),
                    LogLevel = Enum.Parse<LogLevel>(reader.GetString("LogLevel")),
                    Message = reader.GetString("Message"),
                    Exception = reader.GetString("Exception"),
                    Source = reader.GetString("Source"),
                    StackTrace = reader.GetString("StackTrace"),
                    UserId = reader.GetString("UserId"),
                    SessionId = reader.GetString("SessionId"),
                    RequestId = reader.GetString("RequestId")
                });
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to read logs from database: {ex.Message}");
        }

        return logs;
    }

    public async Task ClearLogsAsync(DateTime before)
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "DELETE FROM LogEntries WHERE Timestamp < @Before";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Before", before);

            await command.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to clear logs from database: {ex.Message}");
        }
    }

    public async Task<bool> IsDatabaseAccessibleAsync()
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            
            // Проверяем, существует ли таблица
            var sql = @"
                SELECT COUNT(*) 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_NAME = 'LogEntries'";

            using var command = new NpgsqlCommand(sql, connection);
            var result = await command.ExecuteScalarAsync();
            
            return result != null && Convert.ToInt32(result) > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<int> GetLogCountAsync()
    {
        try
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "SELECT COUNT(*) FROM LogEntries";
            using var command = new NpgsqlCommand(sql, connection);
            var result = await command.ExecuteScalarAsync();
            
            return result != null ? Convert.ToInt32(result) : 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to get log count from database: {ex.Message}");
            return 0;
        }
    }
} 