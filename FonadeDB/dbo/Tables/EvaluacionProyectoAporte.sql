CREATE TABLE [dbo].[EvaluacionProyectoAporte] (
    [Id_Aporte]            INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]          INT           NOT NULL,
    [CodConvocatoria]      INT           NOT NULL,
    [CodTipoIndicador]     SMALLINT      NOT NULL,
    [Nombre]               VARCHAR (255) NOT NULL,
    [Detalle]              VARCHAR (300) NOT NULL,
    [Solicitado]           FLOAT (53)    NOT NULL,
    [Recomendado]          FLOAT (53)    NULL,
    [Protegido]            BIT           NOT NULL,
    [IdFuenteFinanciacion] INT           NULL,
    CONSTRAINT [PK_EvaluacionProyectoAporte] PRIMARY KEY CLUSTERED ([Id_Aporte] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionProyectoAporte_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionProyectoAporte_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_EvaluacionProyectoAporte_Proyecto1] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_EvaluacionProyectoAporte_TipoIndicadorGestion] FOREIGN KEY ([CodTipoIndicador]) REFERENCES [dbo].[TipoIndicadorGestion] ([Id_TipoIndicador])
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Llave foranea de la tabla fuenteFinanciacion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'EvaluacionProyectoAporte', @level2type = N'COLUMN', @level2name = N'IdFuenteFinanciacion';

