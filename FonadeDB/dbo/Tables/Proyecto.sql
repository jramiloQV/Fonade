CREATE TABLE [dbo].[Proyecto] (
    [Id_Proyecto]         INT            IDENTITY (1, 1) NOT NULL,
    [NomProyecto]         VARCHAR (80)   NOT NULL,
    [Sumario]             VARCHAR (1024) NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [FechaDesactivacion]  DATETIME       NULL,
    [MotivoDesactivacion] VARCHAR (300)  NULL,
    [CodTipoProyecto]     SMALLINT       NOT NULL,
    [CodEstado]           TINYINT        NOT NULL,
    [CostoTotal]          MONEY          NULL,
    [CodCiudad]           INT            NOT NULL,
    [CodSubSector]        INT            NOT NULL,
    [CodContacto]         INT            NOT NULL,
    [CodInstitucion]      INT            NOT NULL,
    [Inactivo]            BIT            NOT NULL,
    [Latitud]             INT            NULL,
    [Longitud]            INT            NULL,
    [IdVersionProyecto]   INT            NULL,
    CONSTRAINT [PK_Proyecto] PRIMARY KEY CLUSTERED ([Id_Proyecto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Proyecto_Ciudad] FOREIGN KEY ([CodCiudad]) REFERENCES [dbo].[Ciudad] ([Id_Ciudad]),
    CONSTRAINT [FK_Proyecto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_Proyecto_Estado] FOREIGN KEY ([CodEstado]) REFERENCES [dbo].[Estado] ([Id_Estado]),
    CONSTRAINT [FK_Proyecto_Institucion] FOREIGN KEY ([CodInstitucion]) REFERENCES [dbo].[Institucion] ([Id_Institucion]),
    CONSTRAINT [FK_Proyecto_SubSector] FOREIGN KEY ([CodSubSector]) REFERENCES [dbo].[SubSector] ([Id_SubSector]),
    CONSTRAINT [FK_Proyecto_TipoProyecto] FOREIGN KEY ([CodTipoProyecto]) REFERENCES [dbo].[TipoProyecto] ([Id_TipoProyecto]),
    CONSTRAINT [FK_Proyecto_VersionProyecto] FOREIGN KEY ([IdVersionProyecto]) REFERENCES [dbo].[VersionProyecto] ([IdVersionProyecto])
);



