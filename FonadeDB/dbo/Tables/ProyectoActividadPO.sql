CREATE TABLE [dbo].[ProyectoActividadPO] (
    [Id_Actividad] INT            IDENTITY (1, 1) NOT NULL,
    [NomActividad] VARCHAR (150)  NOT NULL,
    [CodProyecto]  INT            NOT NULL,
    [Item]         SMALLINT       NOT NULL,
    [Metas]        VARCHAR (1000) NULL,
    CONSTRAINT [PK_ProyectoActividadPlanOperativo] PRIMARY KEY CLUSTERED ([Id_Actividad] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoActividadPlanOperativo_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

