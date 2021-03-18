CREATE TABLE [dbo].[ProyectoProrroga] (
    [CodProyecto] INT NOT NULL,
    [Prorroga]    INT NOT NULL,
    [idProrroga]  INT IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ProyectoProrroga_1] PRIMARY KEY CLUSTERED ([idProrroga] ASC),
    CONSTRAINT [IX_ProyectoProrroga] UNIQUE NONCLUSTERED ([CodProyecto] ASC)
);

