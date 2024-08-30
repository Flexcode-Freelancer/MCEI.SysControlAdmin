CREATE DATABASE MCEISysControlAdminDB
GO
    USE MCEISysControlAdminDB
GO
CREATE TABLE [Role](
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] VARCHAR(30) NOT NULL
    );
GO
    CREATE TABLE [User](
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    IdRole INT NOT NULL FOREIGN KEY REFERENCES [Role](Id),
    [Name] VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,
    [Password] VARCHAR(100) NOT NULL,
    [Status] TINYINT NOT NULL,
    RegistrationDate DATETIME NOT NULL
    );
GO
    INSERT INTO [Role] VALUES('Desarrollador');
GO
    INSERT INTO [User] (IdRole, [Name], LastName, Email, [Password], [Status], RegistrationDate) 
    VALUES (1, 'Flexcode', 'Freelancer', 'DesAdmin@elimizalco.com', 'c8aa131427a72781b156ac723ddb917f', 1, SYSDATETIME());
GO
CREATE TABLE ProfessionOrStudy(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    [Name] VARCHAR (100) NOT NULL,
);
GO
CREATE TABLE Membership(
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Dui VARCHAR(10) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Age VARCHAR(3) NOT NULL,
    Gender VARCHAR(20) NOT NULL,
    CivilStatus VARCHAR(20) NOT NULL,
    Phone VARCHAR(9) NOT NULL,
    [Address] VARCHAR(100) NOT NULL,
    IdProfessionOrStudy INT NOT NULL FOREIGN KEY REFERENCES ProfessionOrStudy(Id),
    PlaceOfWorkOrStudy VARCHAR(100) NOT NULL,
    WorkOrStudyPhone VARCHAR(9) NOT NULL,
    ConversionDate DATE NOT NULL,
    PlaceOfConversion VARCHAR(100) NOT NULL,
    WaterBaptism VARCHAR(25) NOT NULL,
    BaptismOfTheHolySpirit VARCHAR(2) NOT NULL,
    PastorsName VARCHAR(100) NOT NULL,
    SupervisorsName VARCHAR(100) NOT NULL,
    LeadersName VARCHAR(100) NOT NULL,
    TimeToGather VARCHAR(50) NOT NULL,
    [Zone] VARCHAR(1) NOT NULL,
    District VARCHAR(1) NOT NULL,
    Sector VARCHAR(1) NOT NULL,
    Cell VARCHAR(1) NOT NULL,
    [Status] TINYINT NOT NULL,
    CommentsOrObservations VARCHAR(100) NOT NULL,
    DateCreated DATETIME NOT NULL,
    DateModification DATETIME NOT NULL,
    ImageData VARBINARY(MAX) NOT NULL,
);
GO
CREATE TABLE Privilege(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    [Name] VARCHAR (100) NOT NULL,
);
GO
CREATE TABLE [Server](
	Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	IdMembership INT NOT NULL FOREIGN KEY REFERENCES Membership(Id),
	IdPrivilege INT NOT NULL FOREIGN KEY REFERENCES Privilege(Id),
	[Status] TINYINT NOT NULL,
	DateCreated DATETIME NOT NULL,
    DateModification DATETIME NOT NULL
);
GO
CREATE TABLE HistoryServer(
	Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	IdMembership INT NOT NULL FOREIGN KEY REFERENCES Membership(Id),
	IdPrivilege INT NOT NULL FOREIGN KEY REFERENCES Privilege(Id),
	[Status] TINYINT NOT NULL,
	DateCreated DATETIME NOT NULL,
    DateModification DATETIME NOT NULL
);
GO
CREATE TABLE Juventud(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
    [Name] VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Age VARCHAR(3) NOT NULL,
    Gender VARCHAR(20) NOT NULL,
    CivilStatus VARCHAR(20) NOT NULL,
    Phone VARCHAR(9) NOT NULL,
    IdProfessionOrStudy INT NOT NULL FOREIGN KEY REFERENCES ProfessionOrStudy(Id),
    WaterBaptism VARCHAR(25) NOT NULL,
    BaptismOfTheHolySpirit VARCHAR(2) NOT NULL,
    [Status] TINYINT NOT NULL,
    CommentsOrObservations VARCHAR(100) NOT NULL,
    DateCreated DATETIME NOT NULL,
    DateModification DATETIME NOT NULL,
    ImageData VARBINARY(MAX) NOT NULL,
);GO
CREATE TABLE ServerJei(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	IdJuventud INT NOT NULL FOREIGN KEY REFERENCES Juventud(Id),
	IdPrivilege INT NOT NULL FOREIGN KEY REFERENCES Privilege(Id),
	[Status] TINYINT NOT NULL,
	DateCreated DATETIME NOT NULL,
    DateModification DATETIME NOT NULL
);GO
CREATE TABLE HistoryServerJei(
    Id INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	IdJuventud INT NOT NULL FOREIGN KEY REFERENCES Juventud(Id),
	IdPrivilege INT NOT NULL FOREIGN KEY REFERENCES Privilege(Id),
	[Status] TINYINT NOT NULL,
	DateCreated DATETIME NOT NULL,
    DateModification DATETIME NOT NULL
);GO