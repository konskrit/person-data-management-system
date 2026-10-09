-- Person Data Management — schema, constraints, and sample data (SQL Server)

IF DB_ID('PersonDataManagement') IS NULL
    CREATE DATABASE PersonDataManagement;
GO

USE PersonDataManagement;
GO

IF OBJECT_ID('dbo.Phone', 'U') IS NOT NULL DROP TABLE dbo.Phone;
IF OBJECT_ID('dbo.Address', 'U') IS NOT NULL DROP TABLE dbo.Address;
IF OBJECT_ID('dbo.Person', 'U') IS NOT NULL DROP TABLE dbo.Person;
GO

CREATE TABLE dbo.Person
(
    Id          INT            NOT NULL IDENTITY(1, 1) CONSTRAINT PK_Person PRIMARY KEY,
    LastName    NVARCHAR(100)  NOT NULL,
    FirstName   NVARCHAR(100)  NOT NULL,
    BirthDate   DATE           NOT NULL
);

CREATE TABLE dbo.Address
(
    Id           INT           NOT NULL IDENTITY(1, 1) CONSTRAINT PK_Address PRIMARY KEY,
    PersonId     INT           NOT NULL,
    PostalCode   NVARCHAR(20)  NOT NULL,
    City         NVARCHAR(100) NOT NULL,
    Street       NVARCHAR(150) NOT NULL,
    HouseNumber  NVARCHAR(20)  NOT NULL,
    CONSTRAINT FK_Address_Person
        FOREIGN KEY (PersonId) REFERENCES dbo.Person (Id)
        ON DELETE NO ACTION
);

CREATE TABLE dbo.Phone
(
    Id        INT           NOT NULL IDENTITY(1, 1) CONSTRAINT PK_Phone PRIMARY KEY,
    PersonId  INT           NOT NULL,
    Number    NVARCHAR(40)  NOT NULL,
    CONSTRAINT FK_Phone_Person
        FOREIGN KEY (PersonId) REFERENCES dbo.Person (Id)
        ON DELETE NO ACTION
);

GO

INSERT INTO dbo.Person (LastName, FirstName, BirthDate) VALUES
(N'Müller',  'Anna',   '1990-03-12'),
('Schmidt',  'Thomas', '1985-07-22'),
('Weber',    'Lisa',   '1995-11-05'),
('Fischer',  'Markus', '1978-01-30');

INSERT INTO dbo.Address (PersonId, PostalCode, City, Street, HouseNumber) VALUES
(1, '01067', 'Dresden',  'Altmarkt',           '1'),
(1, '01159', 'Dresden',  N'Könneritzstraße',  '25'),
(2, '04109', 'Leipzig',  N'Petersstraße',     '12'),
(3, '01067', 'Dresden',  N'Prager Straße',    '8'),
(4, '10115', 'Berlin',   N'Invalidenstraße',  '40'),
(4, '80331', N'München', 'Marienplatz',       '3');

INSERT INTO dbo.Phone (PersonId, Number) VALUES
(1, '+493511234567'),
(1, '017612345678'),
(2, '0341123456'),
(3, '+493519876543'),
(3, '015198765432'),
(4, '03012345678'),
(4, 'abc-invalid'); -- bad data on purpose for the deletion step later
GO
