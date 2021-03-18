CREATE TABLE [dbo].[EvaluacionProyectoIndicador] (
    [id_Indicador]    INT           IDENTITY (1, 1) NOT NULL,
    [codProyecto]     INT           NOT NULL,
    [codConvocatoria] INT           NOT NULL,
    [Descripcion]     VARCHAR (255) NOT NULL,
    [Tipo]            CHAR (1)      NOT NULL,
    [Valor]           FLOAT (53)    NOT NULL,
    [Protegido]       BIT           NOT NULL,
    CONSTRAINT [PK_EvaluacionProyectoIndicador] PRIMARY KEY CLUSTERED ([id_Indicador] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionProyectoIndicador_Convocatoria] FOREIGN KEY ([codConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionProyectoIndicador_Proyecto] FOREIGN KEY ([codProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

