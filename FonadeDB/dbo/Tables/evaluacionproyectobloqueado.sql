CREATE TABLE [dbo].[evaluacionproyectobloqueado] (
    [id_evaluacionproyectobloqueado] INT IDENTITY (1, 1) NOT NULL,
    [codproyecto]                    INT NULL,
    [bloqueado]                      BIT NULL,
    CONSTRAINT [pk_evaluacionproyectobloqueado] PRIMARY KEY CLUSTERED ([id_evaluacionproyectobloqueado] ASC) WITH (FILLFACTOR = 50)
);

