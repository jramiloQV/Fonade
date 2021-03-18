CREATE TABLE [dbo].[ProyectoContacto] (
    [Id_ProyectoContacto] INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]         INT           NOT NULL,
    [CodContacto]         INT           NOT NULL,
    [CodRol]              TINYINT       NOT NULL,
    [FechaInicio]         SMALLDATETIME NOT NULL,
    [FechaFin]            SMALLDATETIME NULL,
    [Inactivo]            BIT           NOT NULL,
    [Beneficiario]        BIT           NULL,
    [Participacion]       FLOAT (53)    NULL,
    [CodConvocatoria]     INT           NULL,
    [HorasProyecto]       INT           NULL,
    [Acreditador]         BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ProyectoContacto] PRIMARY KEY CLUSTERED ([Id_ProyectoContacto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoContacto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_ProyectoContacto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_ProyectoContacto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_ProyectoContacto_Rol] FOREIGN KEY ([CodRol]) REFERENCES [dbo].[Rol] ([Id_Rol])
);


GO
CREATE NONCLUSTERED INDEX [Idx_ProyectoContacto]
    ON [dbo].[ProyectoContacto]([CodProyecto] ASC, [CodContacto] ASC) WITH (FILLFACTOR = 50);

