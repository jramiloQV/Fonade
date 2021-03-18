CREATE TABLE [dbo].[ActividadDefault] (
    [id_actdefault] INT           IDENTITY (1, 1) NOT NULL,
    [item]          INT           NULL,
    [NomActividad]  VARCHAR (150) NULL,
    CONSTRAINT [PK_ActividadDefault] PRIMARY KEY CLUSTERED ([id_actdefault] ASC)
);

