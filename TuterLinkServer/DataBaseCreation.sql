﻿Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'TuterLink_DB')
BEGIN
    DROP DATABASE TutorLink_DB;
END
Go
Create Database TutorLink_DB
Go
Use TutorLink_DB
Go

--סוג משתמשים
CREATE TABLE TypeUser(
    TypeID int PRIMARY KEY,
    TypeName nvarchar(50)
    )

--יצירת טבלת משתמשים
CREATE TABLE Users(
    Id int PRIMARY KEY IDENTITY(1,1),
    Email nvarchar(100),
    Pass nvarchar(25),
    FirstName nvarchar(50),
    LastName nvarchar(50),
    IsAdmin BIT,
    TypeID int,
    FOREIGN KEY(TypeID) REFERENCES TypeUser(TypeID)
    )

-- תלמידים
CREATE TABLE Students(
    StudentID int PRIMARY KEY,
    UserId int,
     FOREIGN KEY(UserId) REFERENCES Users(Id),
    
    )

CREATE TABLE Teachers(
    TeacherID int PRIMARY KEY,
    UserId int,
     FOREIGN KEY(UserId) REFERENCES Users(Id),
    
    )
CREATE TABLE Subjects (
    SubjectID int PRIMARY KEY,
    SubjectName nvarchar(25)
    )

CREATE TABLE StudentToTeachers(
    StudentID int
       FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
    TeacherID int
       FOREIGN KEY(TeacherID) REFERENCES Teachers(TeacherID),
    SubjectID int
        FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID),
    PRIMARY KEY(TeacherID, StudentID, SubjectId)
    )

CREATE TABLE City (
    CityID int PRIMARY KEY,
    CityName nvarchar(25)
    )

-- Create a login for the admin user
CREATE LOGIN [TaskAdminLogin] WITH PASSWORD = 'NoaF1197';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [TaskAdminUser] FOR LOGIN [TaskAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TaskAdminUser];
Go

    
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=TutorLink_DB;User ID=TaskAdminLogin;Password=NoaF1197;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context NoaDBcontext -DataAnnotations -force



