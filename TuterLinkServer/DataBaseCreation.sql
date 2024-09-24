﻿Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'TuterLink_DB')
BEGIN
    DROP DATABASE TuterLink_DB;
END
Go
Create Database TuterLink_DB
Go
Use TuterLink_DB
Go

--יצירת טבלת משתמשים
CREATE TABLE Users(
    Email nvarchar(100) PRIMARY KEY ,
    Pass nvarchar(25) ,
    TypeID int,
    FOREIGN KEY(TypeID) REFERENCES TypeUser(TypeID)
    )

--סוג משתמשים
CREATE TABLE TypeUser(
    TypeID int PRIMARY KEY,
    TypeName nvarchar(50)
    )

-- תלמידים
CREATE TABLE Students(
    StudentID int PRIMARY KEY,
    Email nvarchar(100),
     FOREIGN KEY(Email) REFERENCES Users(Email),
    FirstName nvarchar(50),
    LastName nvarchar(50)
    )


