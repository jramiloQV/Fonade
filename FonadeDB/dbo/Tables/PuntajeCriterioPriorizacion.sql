CREATE TABLE [dbo].[PuntajeCriterioPriorizacion] (
    [CodProyecto]             INT        NOT NULL,
    [CodConvocatoria]         INT        NOT NULL,
    [CodCriterioPriorizacion] SMALLINT   NOT NULL,
    [Valor]                   FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PuntajeCriterioPriorizacion] PRIMARY KEY CLUSTERED ([CodProyecto] ASC, [CodConvocatoria] ASC, [CodCriterioPriorizacion] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_PuntajeCriterioPriorizacion_Convocatoria1] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_PuntajeCriterioPriorizacion_CriterioPriorizacion] FOREIGN KEY ([CodCriterioPriorizacion]) REFERENCES [dbo].[CriterioPriorizacion] ([Id_Criterio]),
    CONSTRAINT [FK_PuntajeCriterioPriorizacion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

