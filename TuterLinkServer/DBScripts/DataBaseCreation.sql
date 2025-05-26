Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'TutorLink_DB')
BEGIN
    DROP DATABASE TutorLink_DB;
END
Go
Create Database TutorLink_DB
Go
Use TutorLink_DB
Go

--Teachers
CREATE TABLE Teachers(
    TeacherId int PRIMARY KEY IDENTITY(1,1),
    Email nvarchar(100) not null,
    Pass nvarchar(25) not null,
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
    IsAdmin BIT default(0),
    UserAddress nvarchar (100) not null,
    MaxDistance float not null,
    GoToStudent bit,
    TeachAtHome bit,
    Vetek int not null, 
    PricePerHour int not null,
    IsBlocked bit not null default(0)
        )

    
-- Students
CREATE TABLE Students(
    StudentID int PRIMARY KEY IDENTITY(1,1),
    Email nvarchar(100) not null,
    Pass nvarchar(25) not null,
    FirstName nvarchar(50) not null,
    LastName nvarchar(50) not null,
    IsAdmin BIT default(0),
    UserAddress nvarchar (100) not null,
    CurrentClass int not null,
    IsBlocked bit not null default(0)
    )


    
--Subjects    
CREATE TABLE Subjects (
    SubjectID int PRIMARY KEY IDENTITY(1,1),
    SubjectName nvarchar(25) not null
    )

--Teachers and Subjects
CREATE TABLE TeachersSubject(
    TeacherID int not null
       FOREIGN KEY(TeacherID) REFERENCES Teachers(TeacherID),
    SubjectID int not null
        FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID),
    ID int primary key IDENTITY(1,1), 
    MinClass int not null,
    MaxClass int not null
    )

--Reviews
CREATE TABLE TeacherReview(
ReviewID int primary key IDENTITY(1,1),
 TeacherID int not null
       FOREIGN KEY(TeacherID) REFERENCES Teachers(TeacherID),
 StudentID int not null
        FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
TimeOfReview datetime not null,
ReviewText nvarchar(400) not null,
Score int not null
)

--Messages 
Create Table ChatMessages(
MessageID int primary key IDENTITY(1,1),
 TeacherID int not null
       FOREIGN KEY(TeacherID) REFERENCES Teachers(TeacherID),
 StudentID int not null
        FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
IsTeacherSender Bit not null,
MessageText nvarchar(500),
TextTime datetime not null
)

--Reports
Create Table Reports(
ReportID int primary key IDENTITY(1,1),
TeacherID int not null
       FOREIGN KEY(TeacherID) REFERENCES Teachers(TeacherID),
StudentID int not null
        FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
ReportedByStudent BIT not null,
ReportText nvarchar(500),
Processed bit not null default(0)
)

-- Lessons 
Create Table Lessons(
LessonID int primary key IDENTITY(1,1),
TeacherID int not null
       FOREIGN KEY(TeacherID) REFERENCES Teachers(TeacherID),
StudentID int not null
        FOREIGN KEY(StudentID) REFERENCES Students(StudentID),
SubjectID int not null
        FOREIGN KEY(SubjectID) REFERENCES Subjects(SubjectID),
TimeOfLesson datetime not null,
)



INSERT INTO Subjects (SubjectName) VALUES ('Math')
INSERT INTO Subjects (SubjectName) VALUES ('Computer Science')
INSERT INTO Subjects (SubjectName) VALUES ('English')
INSERT INTO Subjects (SubjectName) VALUES ('Biology')
INSERT INTO Subjects (SubjectName) VALUES ('Chemistry')
INSERT INTO Subjects (SubjectName) VALUES ('Physics')
INSERT INTO Subjects (SubjectName) VALUES ('History')
INSERT INTO Subjects (SubjectName) VALUES ('Geography')
INSERT INTO Subjects (SubjectName) VALUES ('Literature')
INSERT INTO Subjects (SubjectName) VALUES ('Art')
INSERT INTO Subjects (SubjectName) VALUES ('Music')
INSERT INTO Subjects (SubjectName) VALUES ('Civics')
INSERT INTO Subjects (SubjectName) VALUES ('Physical Education')
INSERT INTO Subjects (SubjectName) VALUES ('Psychology')
INSERT INTO Subjects (SubjectName) VALUES ('Economics')
INSERT INTO Subjects (SubjectName) VALUES ('Philosophy')




INSERT INTO Teachers (Email, Pass, LastName, FirstName, IsAdmin, UserAddress, GoToStudent, TeachAtHome, Vetek, PricePerHour, MaxDistance)
VALUES ('ofer@ofer.com', '1234', 'Zadikario', 'Ofer', 0, 'Hashachar, 57, Raanana', 0, 1, 3, 100, 0)
INSERT INTO Teachers (Email, Pass, LastName, FirstName, IsAdmin,UserAddress, GoToStudent, TeachAtHome, Vetek, PricePerHour,MaxDistance)
VALUES ('nirfisher@gmail.com', '1234', 'Fisher', 'Nir', 0,'Hashachar, 57, Hod Hasharon', 0, 1, 3, 150,0)
INSERT INTO Teachers (Email, Pass, LastName, FirstName, IsAdmin, UserAddress, GoToStudent, TeachAtHome, Vetek, PricePerHour,MaxDistance)
VALUES ('galip@gmail.com', '1234', 'Fisher', 'Nir', 0,'Hashachar, 57, Hod Hasharon', 0, 1, 3, 150,0)
INSERT INTO Teachers (Email, Pass, LastName, FirstName, IsAdmin,UserAddress, GoToStudent, TeachAtHome, Vetek, PricePerHour,MaxDistance)
VALUES ('eilonp12@gmail.com', '0000', 'Peretz', 'Eilon', 1,'Hashachar, 57, Hod Hasharon' ,0, 1, 3, 150,0)



INSERT INTO Students(Email, Pass, LastName, FirstName, IsAdmin, UserAddress, CurrentClass)
VALUES ('kuku@ofer.com', '1234', 'Kuku', 'Kuku', 0, 'Hashachar, 57, Hod Hasharon', 12)
INSERT INTO Students(Email, Pass, LastName, FirstName, IsAdmin, UserAddress, CurrentClass)
VALUES ('kuku2@ofer.com', '1234', 'Kuku2', 'Kuku2', 0, 'Hashachar, 57, Hod Hasharon', 12)
INSERT INTO Students(Email, Pass, LastName, FirstName, IsAdmin,UserAddress, CurrentClass)
VALUES ('amitchacham1@gmail.com', '1111', 'Chacham', 'Amit', 0,'Hashachar, 57, Hod Hasharon', 12)
INSERT INTO Students(Email, Pass, LastName, FirstName, IsAdmin,UserAddress, CurrentClass)
VALUES ('noa.fisher.2007@gmail.com', '1197', 'Fisher', 'Noa', 0,'Hashachar, 57, Hod Hasharon', 12)


INSERT INTO Lessons (TeacherID, StudentID, SubjectID,TimeOfLesson)
VALUES
 (1,1,1, GETDATE())


select * from Subjects
select * from TeachersSubject
select * from Teachers
select * from Students
insert into TeachersSubject(TeacherID, SubjectID, MinClass, MaxClass) VALUES (1, 2, 10,12)

-- Create a login for the admin user
CREATE LOGIN [TutorLinkAdminLogin] WITH PASSWORD = 'NoaF1197';
Go

CREATE USER [TutorLinkAdminUser] FOR LOGIN [TutorLinkAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TutorLinkAdminUser];
Go

--iinsert a fake message
INSERT INTO ChatMessages VALUES (1,1,1,'this is a fake message', GETDATE())
Go
INSERT INTO ChatMessages VALUES (1,1,0,'this is a fake message2', GETDATE())
Go
INSERT INTO ChatMessages VALUES (3,1,1,'from tal to kuku', GETDATE())
Go

select * from ChatMessages
--scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=TutorLink_DB;User ID=TutorLinkAdminLogin;Password=NoaF1197;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context NoaDBcontext -DataAnnotations -force



INSERT INTO Teachers (Email, Pass, LastName, FirstName, IsAdmin, UserAddress, GoToStudent, TeachAtHome, Vetek, PricePerHour, MaxDistance)
VALUES ('a@a.com', 'a', 'Zadikario', 'Admin', 1, 'Hashachar, 57, Raanana', 0, 1, 3, 100, 0)
