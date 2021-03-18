CREATE TABLE [dbo].[AsignacionActaProyecto] (
    [CodActa]     INT NOT NULL,
    [CodProyecto] INT NOT NULL,
    [Asignado]    BIT NULL,
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_AsignacionActaProyecto] PRIMARY KEY CLUSTERED ([Id] ASC)
);

