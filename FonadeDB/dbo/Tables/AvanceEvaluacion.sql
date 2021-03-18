CREATE TABLE [dbo].[AvanceEvaluacion] (
    [IdEvaluacionSeguimiento] INT      IDENTITY (1, 1) NOT NULL,
    [IdConvocatoria]          INT      NOT NULL,
    [IdProyecto]              INT      NOT NULL,
    [IdCampo]                 INT      NOT NULL,
    [IdContacto]              INT      NOT NULL,
    [FechaActualizacion]      DATETIME CONSTRAINT [DF_AvanceEvaluacion_FechaActualizacion] DEFAULT (getdate()) NOT NULL,
    [Avance]                  BIT      NOT NULL,
    CONSTRAINT [PK_AvanceEvaluacion] PRIMARY KEY CLUSTERED ([IdEvaluacionSeguimiento] ASC),
    CONSTRAINT [FK_AvanceEvaluacion_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



