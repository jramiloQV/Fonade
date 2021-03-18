CREATE TABLE [dbo].[ConvocatoriaCriterioPriorizacion] (
    [CodConvocatoria]         INT          NOT NULL,
    [CodCriterioPriorizacion] SMALLINT     NOT NULL,
    [Parametros]              VARCHAR (50) NULL,
    [Incidencia]              FLOAT (53)   NOT NULL,
    CONSTRAINT [PK_ConvocatoriaCriterioPriorizacion] PRIMARY KEY CLUSTERED ([CodConvocatoria] ASC, [CodCriterioPriorizacion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaCriterioPriorizacion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_ConvocatoriaCriterioPriorizacion_CriterioPriorizacion] FOREIGN KEY ([CodCriterioPriorizacion]) REFERENCES [dbo].[CriterioPriorizacion] ([Id_Criterio])
);

