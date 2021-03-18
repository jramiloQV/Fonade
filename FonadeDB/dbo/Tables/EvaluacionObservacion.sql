CREATE TABLE [dbo].[EvaluacionObservacion] (
    [CodProyecto]              INT            NOT NULL,
    [CodConvocatoria]          INT            NOT NULL,
    [Actividades]              TEXT           NULL,
    [ProductosServicios]       TEXT           NULL,
    [EstrategiaMercado]        TEXT           NULL,
    [ProcesoProduccion]        TEXT           NULL,
    [EstructuraOrganizacional] TEXT           NULL,
    [TamanioLocalizacion]      TEXT           NULL,
    [Generales]                TEXT           NULL,
    [ValorRecomendado]         FLOAT (53)     NULL,
    [CentralesRiesgo]          VARCHAR (1000) NULL,
    [EquipoTrabajo]            VARCHAR (1000) NULL,
    [ConclusionesFinancieras]  VARCHAR (MAX)  NULL,
    [FechaCentralesRiesgo]     SMALLDATETIME  NULL,
    [TiempoProyeccion]         TINYINT        CONSTRAINT [DF_EvaluacionObservacion_TiempoProyeccion] DEFAULT ((3)) NULL,
    [empleosevaluacion]        INT            NULL,
    [Localizacion]             TEXT           NULL,
    [Sector]                   TEXT           NULL,
    [ResumenConcepto]          TEXT           NULL,
    CONSTRAINT [PK_EvaluacionObservacion] PRIMARY KEY CLUSTERED ([CodProyecto] ASC, [CodConvocatoria] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionObservacion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionObservacion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



