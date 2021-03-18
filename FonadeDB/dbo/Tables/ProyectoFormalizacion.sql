CREATE TABLE [dbo].[ProyectoFormalizacion] (
    [Id_ProyectoFormalizacion] INT           IDENTITY (1, 1) NOT NULL,
    [codProyecto]              INT           NOT NULL,
    [codContacto]              INT           NOT NULL,
    [Fecha]                    SMALLDATETIME NOT NULL,
    [Aval]                     VARCHAR (500) NOT NULL,
    [Observaciones]            VARCHAR (500) NOT NULL,
    [CodConvocatoria]          INT           NULL,
    CONSTRAINT [PK_ProyectoFormalizacion] PRIMARY KEY CLUSTERED ([Id_ProyectoFormalizacion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoFormalizacion_Contacto] FOREIGN KEY ([codContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_ProyectoFormalizacion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_ProyectoFormalizacion_Proyecto] FOREIGN KEY ([codProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

