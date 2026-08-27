/*
    ChileGeo - Script 04: Datos de ejemplo.
    Los valores de Superficie/Población/Densidad son referenciales (no requieren exactitud).
*/
USE ChileGeoDB;
GO

SET NOCOUNT ON;

DELETE FROM dbo.Comuna;
DELETE FROM dbo.Region;
DBCC CHECKIDENT ('dbo.Comuna', RESEED, 0);
DBCC CHECKIDENT ('dbo.Region', RESEED, 0);
GO

INSERT INTO dbo.Region (Region) VALUES (N'Región Metropolitana de Santiago');
INSERT INTO dbo.Region (Region) VALUES (N'Región de Valparaíso');
INSERT INTO dbo.Region (Region) VALUES (N'Región del Biobío');
GO

DECLARE @IdRM INT = (SELECT IdRegion FROM dbo.Region WHERE Region = N'Región Metropolitana de Santiago');
DECLARE @IdValpo INT = (SELECT IdRegion FROM dbo.Region WHERE Region = N'Región de Valparaíso');
DECLARE @IdBiobio INT = (SELECT IdRegion FROM dbo.Region WHERE Region = N'Región del Biobío');

INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdRM, N'Santiago',
        N'<Info><Superficie>22.4</Superficie><Poblacion Densidad="19870.5">404495</Poblacion></Info>');
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdRM, N'Providencia',
        N'<Info><Superficie>14.4</Superficie><Poblacion Densidad="8302.9">142079</Poblacion></Info>');
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdRM, N'Maipú',
        N'<Info><Superficie>133.5</Superficie><Poblacion Densidad="4022.1">536781</Poblacion></Info>');
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdValpo, N'Valparaíso',
        N'<Info><Superficie>401.6</Superficie><Poblacion Densidad="705.3">283180</Poblacion></Info>');
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdValpo, N'Viña del Mar',
        N'<Info><Superficie>121.6</Superficie><Poblacion Densidad="2436.1">296316</Poblacion></Info>');
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdBiobio, N'Concepción',
        N'<Info><Superficie>221.6</Superficie><Poblacion Densidad="1067.7">236634</Poblacion></Info>');
INSERT INTO dbo.Comuna (IdRegion, Comuna, InformacionAdicional) VALUES
    (@IdBiobio, N'Talcahuano',
        N'<Info><Superficie>99.1</Superficie><Poblacion Densidad="1541.4">152778</Poblacion></Info>');
GO
