-- Person Data Management — assignment queries and maintenance (SQL Server)
USE PersonDataManagement;
GO

-- How many person records exist?
SELECT COUNT(*) AS PersonCount
FROM dbo.Person;

-- How many persons live in Dresden?
SELECT COUNT(DISTINCT PersonId) AS PersonsInDresden
FROM dbo.Address
WHERE City = 'Dresden';

-- How many persons have more than one phone number?
SELECT COUNT(*) AS PersonsWithMultiplePhones
FROM (
    SELECT PersonId
    FROM dbo.Phone
    GROUP BY PersonId
    HAVING COUNT(*) > 1
) AS MultiPhone;

-- Number of persons per city
SELECT a.City, COUNT(DISTINCT a.PersonId) AS PersonCount
FROM dbo.Address AS a
GROUP BY a.City
ORDER BY a.City;

-- View: persons with addresses and phone numbers
IF OBJECT_ID('dbo.PersonDetails', 'V') IS NOT NULL
    DROP VIEW dbo.PersonDetails;
GO

CREATE VIEW dbo.PersonDetails
AS
SELECT
    p.Id AS PersonId,
    p.LastName,
    p.FirstName,
    p.BirthDate,
    a.PostalCode,
    a.City,
    a.Street,
    a.HouseNumber,
    ph.Number AS PhoneNumber
FROM dbo.Person AS p
LEFT JOIN dbo.Address AS a ON a.PersonId = p.Id
LEFT JOIN dbo.Phone AS ph ON ph.PersonId = p.Id;
GO

-- Delete phone numbers that do not start with '0' or '+'
DELETE FROM dbo.Phone
WHERE Number NOT LIKE '0%'
  AND Number NOT LIKE '+%';
GO

-- Add column for uppercase last name, then fill it
IF COL_LENGTH('dbo.Person', 'LastNameUpper') IS NULL
    ALTER TABLE dbo.Person ADD LastNameUpper NVARCHAR(100) NULL;
GO

UPDATE dbo.Person
SET LastNameUpper = UPPER(LastName);
GO
