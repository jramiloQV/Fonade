CREATE TABLE [dbo].[ProyectoRequerimientos] (
    [IdRequerimiento]  INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]       INT           NOT NULL,
    [LugarFisico]      VARCHAR (MAX) NOT NULL,
    [TieneLugarFisico] BIT           NOT NULL,
    CONSTRAINT [PK_ProyectoRequerimientos] PRIMARY KEY CLUSTERED ([IdRequerimiento] ASC),
    CONSTRAINT [FK_ProyectoRequerimientos_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

