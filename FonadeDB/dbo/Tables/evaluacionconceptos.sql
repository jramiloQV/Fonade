CREATE TABLE [dbo].[evaluacionconceptos] (
    [id_evaluacionconceptos] INT            IDENTITY (1, 1) NOT NULL,
    [nomevaluacionconceptos] VARCHAR (2500) NULL,
    CONSTRAINT [pk_evaluacionconceptos] PRIMARY KEY CLUSTERED ([id_evaluacionconceptos] ASC) WITH (FILLFACTOR = 50)
);

