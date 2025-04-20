USE master;
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'WeatherDB')
    CREATE DATABASE WeatherDB;

USE WeatherDB;
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'dbo')
    EXEC('CREATE SCHEMA dbo');

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'WeatherHistory' AND schema_id = SCHEMA_ID('dbo'))
CREATE TABLE dbo.WeatherHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Date DATETIME NOT NULL,
    TemperatureC DECIMAL(5,2) NOT NULL,
    Summary NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);