-- REPLACE YOUR DATABASE NAME, LOGIN AND PASSWORD IN THE SCRIPT BELOW

USE master;
GO

-- Declare the database name
DECLARE @DatabaseName NVARCHAR(255) = 'NoaDBContext';

-- Generate and execute the kill commands for all active connections
DECLARE @KillCommand NVARCHAR(MAX);

SET @KillCommand = (
    SELECT STRING_AGG('KILL ' + CAST(session_id AS NVARCHAR), '; ')
    FROM sys.dm_exec_sessions
    WHERE database_id = DB_ID(@DatabaseName)
);

IF @KillCommand IS NOT NULL
BEGIN
    EXEC sp_executesql @KillCommand;
    PRINT 'All connections to the database have been terminated.';
END
ELSE
BEGIN
    PRINT 'No active connections to the database.';
END
Go

IF EXISTS (SELECT * FROM sys.databases WHERE name = N'NoaDBContext')
BEGIN
    DROP DATABASE TasksManagementDB;
END
Go
-- Create a login for the admin user
CREATE LOGIN [TutorLinkAdminUser] WITH PASSWORD = 'NoaF1197';
Go

--so user can restore the DB!
ALTER SERVER ROLE sysadmin ADD MEMBER [TutorLinkAdminUser];
Go

CREATE Database NoaDBContext;
Go

