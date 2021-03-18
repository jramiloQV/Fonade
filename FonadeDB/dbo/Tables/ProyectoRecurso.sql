CREATE TABLE [dbo].[ProyectoRecurso] (
    [Id_Recurso]  INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto] INT            NOT NULL,
    [Tipo]        VARCHAR (10)   NOT NULL,
    [Cuantia]     MONEY          NOT NULL,
    [Plazo]       VARCHAR (30)   NULL,
    [Formapago]   VARCHAR (50)   NULL,
    [Interes]     SMALLINT       NULL,
    [Destinacion] VARCHAR (1000) NULL,
    CONSTRAINT [PK_ProyectoRecurso] PRIMARY KEY CLUSTERED ([Id_Recurso] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoRecursos_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

