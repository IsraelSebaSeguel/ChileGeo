/*
    ChileGeo - Script 02: Creación de tablas.
    Region (1) --- (N) Comuna
*/
USE ChileGeoDB;
GO

IF OBJECT_ID(N'dbo.Comuna', N'U') IS NOT NULL DROP TABLE dbo.Comuna;
IF OBJECT_ID(N'dbo.Region', N'U') IS NOT NULL DROP TABLE dbo.Region;
GO

CREATE TABLE dbo.Region
(
    IdRegion INT IDENTITY(1,1) NOT NULL,
    Region   NVARCHAR(128) NULL,
    CONSTRAINT PK_Region PRIMARY KEY CLUSTERED (IdRegion)
);
GO

CREATE TABLE dbo.Comuna
(
    IdComuna              INT IDENTITY(1,1) NOT NULL,
    IdRegion              INT NULL,
    Comuna                NVARCHAR(128) NULL,
    InformacionAdicional  XML NULL,
    CONSTRAINT PK_Comuna PRIMARY KEY CLUSTERED (IdComuna),
    CONSTRAINT FK_Comuna_Region FOREIGN KEY (IdRegion) REFERENCES dbo.Region (IdRegion)
);
GO

CREATE INDEX IX_Comuna_IdRegion ON dbo.Comuna (IdRegion);
GO
