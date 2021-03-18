CREATE TABLE [dbo].[CriterioPriorizacion] (
    [Id_Criterio] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [NomCriterio] VARCHAR (50)  NOT NULL,
    [Sigla]       VARCHAR (10)  NOT NULL,
    [CodFactor]   TINYINT       NOT NULL,
    [Componente]  VARCHAR (50)  NOT NULL,
    [Indicador]   VARCHAR (255) NOT NULL,
    [ValorBase]   VARCHAR (255) NOT NULL,
    [Formulacion] VARCHAR (255) NOT NULL,
    [Query]       VARCHAR (50)  NOT NULL,
    [Parametros]  VARCHAR (100) NULL,
    CONSTRAINT [PK_CriterioPriorizacion] PRIMARY KEY CLUSTERED ([Id_Criterio] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_CriterioPriorizacion_Factor] FOREIGN KEY ([CodFactor]) REFERENCES [dbo].[Factor] ([Id_Factor])
);

