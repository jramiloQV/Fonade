CREATE TABLE [dbo].[ProyectoInversion] (
    [Id_Inversion]         INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]          INT           NOT NULL,
    [Concepto]             VARCHAR (255) NOT NULL,
    [Valor]                MONEY         NOT NULL,
    [AportadoPor]          VARCHAR (100) NOT NULL,
    [Semanas]              SMALLINT      NULL,
    [TipoInversion]        VARCHAR (10)  NULL,
    [IdFuenteFinanciacion] INT           NULL,
    CONSTRAINT [PK_ProyectoInversion] PRIMARY KEY CLUSTERED ([Id_Inversion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoInversion_FuenteFinanciacion] FOREIGN KEY ([IdFuenteFinanciacion]) REFERENCES [dbo].[FuenteFinanciacion] ([IdFuente]),
    CONSTRAINT [FK_ProyectoInversion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



