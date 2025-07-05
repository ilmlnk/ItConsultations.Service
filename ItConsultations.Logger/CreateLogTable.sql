CREATE TABLE LogEntries (
    Id BIGINT IDENTITY(1,1) PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL,
    LogLevel NVARCHAR(10) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Exception NVARCHAR(MAX),
    Source NVARCHAR(255),
    StackTrace NVARCHAR(MAX),
    UserId NVARCHAR(100),
    SessionId NVARCHAR(100),
    RequestId NVARCHAR(100),
    CreatedAt DATETIME2 DEFAULT GETUTCDATE()
);

CREATE INDEX IX_LogEntries_Timestamp ON LogEntries(Timestamp DESC);
CREATE INDEX IX_LogEntries_LogLevel ON LogEntries(LogLevel);
CREATE INDEX IX_LogEntries_Source ON LogEntries(Source);
CREATE INDEX IX_LogEntries_UserId ON LogEntries(UserId);
CREATE INDEX IX_LogEntries_SessionId ON LogEntries(SessionId);

CREATE VIEW vw_RecentLogs AS
SELECT TOP 1000
    Id,
    Timestamp,
    LogLevel,
    Message,
    Exception,
    Source,
    UserId,
    SessionId,
    RequestId
FROM LogEntries
ORDER BY Timestamp DESC;

-- Создание представления для ошибок
CREATE VIEW vw_ErrorLogs AS
SELECT
    Id,
    Timestamp,
    LogLevel,
    Message,
    Exception,
    Source,
    StackTrace,
    UserId,
    SessionId,
    RequestId
FROM LogEntries
WHERE LogLevel = 'Error'
ORDER BY Timestamp DESC;

CREATE PROCEDURE sp_ClearOldLogs
    @DaysToKeep INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @CutoffDate DATETIME2 = DATEADD(DAY, -@DaysToKeep, GETUTCDATE());
    
    DELETE FROM LogEntries 
    WHERE Timestamp < @CutoffDate;
    
    SELECT @@ROWCOUNT AS DeletedRows;
END;

CREATE PROCEDURE sp_GetLogStatistics
    @FromDate DATETIME2 = NULL,
    @ToDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @FromDate IS NULL
        SET @FromDate = DATEADD(DAY, -7, GETUTCDATE());
    
    IF @ToDate IS NULL
        SET @ToDate = GETUTCDATE();
    
    SELECT 
        LogLevel,
        COUNT(*) AS Count,
        MIN(Timestamp) AS FirstOccurrence,
        MAX(Timestamp) AS LastOccurrence
    FROM LogEntries
    WHERE Timestamp BETWEEN @FromDate AND @ToDate
    GROUP BY LogLevel
    ORDER BY Count DESC;
END; 