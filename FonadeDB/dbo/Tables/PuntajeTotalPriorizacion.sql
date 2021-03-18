CREATE TABLE [dbo].[PuntajeTotalPriorizacion] (
    [CodProyecto]     INT        NOT NULL,
    [CodConvocatoria] INT        NOT NULL,
    [Total]           FLOAT (53) NOT NULL,
    CONSTRAINT [PK_PuntajeTotalPriorizacion] PRIMARY KEY CLUSTERED ([CodProyecto] ASC, [CodConvocatoria] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_PuntajeTotalPriorizacion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_PuntajeTotalPriorizacion_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

