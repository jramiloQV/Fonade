CREATE TABLE [dbo].[DificultadCentral] (
    [Id_DificultadCentral] INT           IDENTITY (1, 1) NOT NULL,
    [NomDificultadCentral] VARCHAR (255) NULL,
    [FechaCreacion]        DATETIME      NULL,
    [FechaActualizacion]   DATETIME      NULL,
    CONSTRAINT [PK_DificultadCentral] PRIMARY KEY CLUSTERED ([Id_DificultadCentral] ASC)
);

