/*
    ChileGeo - Script 01: Creación de la base de datos.
    Motor: SQL Server 2012 o superior.
*/
IF DB_ID(N'ChileGeoDB') IS NULL
BEGIN
    CREATE DATABASE ChileGeoDB;
END
GO
