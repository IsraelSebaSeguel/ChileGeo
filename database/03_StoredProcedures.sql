/*
    ChileGeo - Script 03: Procedimientos almacenados.
    Toda la integración con la BBDD se realiza 100% mediante procedimientos almacenados.
    La actualización/inserción de Comuna se realiza mediante la instrucción MERGE.
*/
USE ChileGeoDB;
GO

IF OBJECT_ID(N'dbo.usp_Region_GetAll', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Region_GetAll;
GO
CREATE PROCEDURE dbo.usp_Region_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRegion, Region
    FROM dbo.Region
    ORDER BY Region;
END
GO

IF OBJECT_ID(N'dbo.usp_Region_GetById', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Region_GetById;
GO
CREATE PROCEDURE dbo.usp_Region_GetById
    @IdRegion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdRegion, Region
    FROM dbo.Region
    WHERE IdRegion = @IdRegion;
END
GO

IF OBJECT_ID(N'dbo.usp_Comuna_GetByRegion', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Comuna_GetByRegion;
GO
CREATE PROCEDURE dbo.usp_Comuna_GetByRegion
    @IdRegion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdComuna, IdRegion, Comuna, InformacionAdicional
    FROM dbo.Comuna
    WHERE IdRegion = @IdRegion
    ORDER BY Comuna;
END
GO

IF OBJECT_ID(N'dbo.usp_Comuna_GetById', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Comuna_GetById;
GO
CREATE PROCEDURE dbo.usp_Comuna_GetById
    @IdComuna INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdComuna, IdRegion, Comuna, InformacionAdicional
    FROM dbo.Comuna
    WHERE IdComuna = @IdComuna;
END
GO

IF OBJECT_ID(N'dbo.usp_Comuna_Merge', N'P') IS NOT NULL DROP PROCEDURE dbo.usp_Comuna_Merge;
GO
CREATE PROCEDURE dbo.usp_Comuna_Merge
    @IdComuna             INT,
    @IdRegion             INT,
    @Comuna               NVARCHAR(128),
    @InformacionAdicional XML = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Region WHERE IdRegion = @IdRegion)
    BEGIN
        RAISERROR('La región especificada (IdRegion=%d) no existe.', 16, 1, @IdRegion);
        RETURN;
    END

    DECLARE @Salida TABLE (IdComuna INT);

    MERGE dbo.Comuna AS target
    USING (SELECT @IdComuna AS IdComuna) AS source
        ON target.IdComuna = source.IdComuna
    WHEN MATCHED THEN
        UPDATE SET
            IdRegion             = @IdRegion,
            Comuna               = @Comuna,
            InformacionAdicional = @InformacionAdicional
    WHEN NOT MATCHED THEN
        INSERT (IdRegion, Comuna, InformacionAdicional)
        VALUES (@IdRegion, @Comuna, @InformacionAdicional)
    OUTPUT inserted.IdComuna INTO @Salida;

    SELECT c.IdComuna, c.IdRegion, c.Comuna, c.InformacionAdicional
    FROM dbo.Comuna AS c
    INNER JOIN @Salida AS s ON s.IdComuna = c.IdComuna;
END
GO
