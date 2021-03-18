CREATE TABLE [dbo].[Bitacora20131019] (
    [Id_Bitacora]       INT            IDENTITY (1, 1) NOT NULL,
    [FechaBitacora]     DATETIME       NOT NULL,
    [CodEventoBitacora] INT            NOT NULL,
    [Accion]            VARCHAR (4000) NULL,
    [CodContacto]       INT            NOT NULL,
    [IP]                VARCHAR (20)   NULL
);

