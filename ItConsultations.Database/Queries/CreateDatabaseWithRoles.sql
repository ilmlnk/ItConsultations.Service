IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ItConsultationsDB')
BEGIN
    CREATE DATABASE ItConsultationsDB;
END
GO

USE ItConsultationsDB;
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        FirebaseUid NVARCHAR(128) NOT NULL UNIQUE,
        Email NVARCHAR(255) NOT NULL UNIQUE,
        DisplayName NVARCHAR(100),
        PhotoUrl NVARCHAR(500),
        EmailVerified BIT NOT NULL DEFAULT 0,
        Role INT NOT NULL DEFAULT 1, -- 1=Student, 2=Coach, 3=Admin
        LastLoginAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        CoachId BIGINT NULL,
        StudentId BIGINT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RefreshTokens')
BEGIN
    CREATE TABLE RefreshTokens (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Token NVARCHAR(500) NOT NULL UNIQUE,
        UserId BIGINT NOT NULL,
        ExpiresAt DATETIME2 NOT NULL,
        IsRevoked BIT NOT NULL DEFAULT 0,
        RevokedAt DATETIME2 NULL,
        RevokedBy NVARCHAR(50) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Coaches')
BEGIN
    CREATE TABLE Coaches (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        CoachConsId NVARCHAR(32) NOT NULL UNIQUE,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100),
        BirthDate DATETIME2,
        Description NVARCHAR(1000),
        Email NVARCHAR(255) NOT NULL,
        PictureUrl NVARCHAR(500),
        LinkedInUrl NVARCHAR(500),
        GitHubUrl NVARCHAR(500),
        AverageRating DECIMAL(3,2) NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Students')
BEGIN
    CREATE TABLE Students (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        StudentConsId NVARCHAR(32) NOT NULL UNIQUE,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100),
        BirthDate DATETIME2,
        Email NVARCHAR(255) NOT NULL,
        PictureUrl NVARCHAR(500),
        GitHubUrl NVARCHAR(500),
        LinkedInUrl NVARCHAR(500),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Articles')
BEGIN
    CREATE TABLE Articles (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(255) NOT NULL,
        Content NVARCHAR(MAX) NOT NULL,
        AuthorId BIGINT NOT NULL,
        PublishedAt DATETIME2,
        IsPublished BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Consultations')
BEGIN
    CREATE TABLE Consultations (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Title NVARCHAR(255) NOT NULL,
        Description NVARCHAR(1000),
        CoachId BIGINT NOT NULL,
        StartTime DATETIME2 NOT NULL,
        EndTime DATETIME2 NOT NULL,
        MaxStudents INT NOT NULL DEFAULT 1,
        Price DECIMAL(10,2),
        Status INT NOT NULL DEFAULT 1, -- 1=Scheduled, 2=InProgress, 3=Completed, 4=Cancelled
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reviews')
BEGIN
    CREATE TABLE Reviews (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Text NVARCHAR(2000),
        Rating INT NOT NULL,
        ReviewerId BIGINT NOT NULL,
        CoachId BIGINT NOT NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Attachments')
BEGIN
    CREATE TABLE Attachments (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(255) NOT NULL,
        FileName NVARCHAR(255) NOT NULL,
        FilePath NVARCHAR(500) NOT NULL,
        FileSize BIGINT NOT NULL,
        ContentType NVARCHAR(100) NOT NULL,
        EntityId BIGINT,
        EntityName NVARCHAR(100),
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
    );
END
GO

/*CREATE INDEX IF NOT EXISTS IX_Users_FirebaseUid ON Users(FirebaseUid);
CREATE INDEX IF NOT EXISTS IX_Users_Email ON Users(Email);
CREATE INDEX IF NOT EXISTS IX_Users_Role ON Users(Role);
CREATE INDEX IF NOT EXISTS IX_Users_IsActive ON Users(IsActive);

CREATE INDEX IF NOT EXISTS IX_RefreshTokens_Token ON RefreshTokens(Token);
CREATE INDEX IF NOT EXISTS IX_RefreshTokens_UserId ON RefreshTokens(UserId);
CREATE INDEX IF NOT EXISTS IX_RefreshTokens_ExpiresAt ON RefreshTokens(ExpiresAt);
CREATE INDEX IF NOT EXISTS IX_RefreshTokens_IsRevoked ON RefreshTokens(IsRevoked);

CREATE INDEX IF NOT EXISTS IX_Coaches_CoachConsId ON Coaches(CoachConsId);
CREATE INDEX IF NOT EXISTS IX_Coaches_Email ON Coaches(Email);

CREATE INDEX IF NOT EXISTS IX_Students_StudentConsId ON Students(StudentConsId);
CREATE INDEX IF NOT EXISTS IX_Students_Email ON Students(Email);

CREATE INDEX IF NOT EXISTS IX_Articles_AuthorId ON Articles(AuthorId);
CREATE INDEX IF NOT EXISTS IX_Articles_PublishedAt ON Articles(PublishedAt);
CREATE INDEX IF NOT EXISTS IX_Articles_IsPublished ON Articles(IsPublished);

CREATE INDEX IF NOT EXISTS IX_Consultations_CoachId ON Consultations(CoachId);
CREATE INDEX IF NOT EXISTS IX_Consultations_StartTime ON Consultations(StartTime);
CREATE INDEX IF NOT EXISTS IX_Consultations_Status ON Consultations(Status);

CREATE INDEX IF NOT EXISTS IX_Reviews_ReviewerId ON Reviews(ReviewerId);
CREATE INDEX IF NOT EXISTS IX_Reviews_CoachId ON Reviews(CoachId);
CREATE INDEX IF NOT EXISTS IX_Reviews_Rating ON Reviews(Rating);

CREATE INDEX IF NOT EXISTS IX_Attachments_EntityId ON Attachments(EntityId);
CREATE INDEX IF NOT EXISTS IX_Attachments_EntityName ON Attachments(EntityName);*/

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Coaches')
BEGIN
    ALTER TABLE Users ADD CONSTRAINT FK_Users_Coaches 
        FOREIGN KEY (CoachId) REFERENCES Coaches(Id) ON DELETE SET NULL;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Users_Students')
BEGIN
    ALTER TABLE Users ADD CONSTRAINT FK_Users_Students 
        FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE SET NULL;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Consultations_Coaches')
BEGIN
    ALTER TABLE Consultations ADD CONSTRAINT FK_Consultations_Coaches 
        FOREIGN KEY (CoachId) REFERENCES Coaches(Id) ON DELETE RESTRICT;
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Reviews_Coaches')
BEGIN
    ALTER TABLE Reviews ADD CONSTRAINT FK_Reviews_Coaches 
        FOREIGN KEY (CoachId) REFERENCES Coaches(Id) ON DELETE RESTRICT;
END

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Users_UpdateTimestamp')
BEGIN
    EXEC('
        CREATE TRIGGER TR_Users_UpdateTimestamp
        ON Users
        AFTER UPDATE
        AS
        BEGIN
            UPDATE Users 
            SET UpdatedAt = GETUTCDATE()
            FROM Users u
            INNER JOIN inserted i ON u.Id = i.Id;
        END
    ');
END

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Coaches_UpdateTimestamp')
BEGIN
    EXEC('
        CREATE TRIGGER TR_Coaches_UpdateTimestamp
        ON Coaches
        AFTER UPDATE
        AS
        BEGIN
            UPDATE Coaches 
            SET UpdatedAt = GETUTCDATE()
            FROM Coaches c
            INNER JOIN inserted i ON c.Id = i.Id;
        END
    ');
END

IF NOT EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Students_UpdateTimestamp')
BEGIN
    EXEC('
        CREATE TRIGGER TR_Students_UpdateTimestamp
        ON Students
        AFTER UPDATE
        AS
        BEGIN
            UPDATE Students 
            SET UpdatedAt = GETUTCDATE()
            FROM Students s
            INNER JOIN inserted i ON s.Id = i.Id;
        END
    ');
END

PRINT 'ItConsultationsDB was created with role support!'; 