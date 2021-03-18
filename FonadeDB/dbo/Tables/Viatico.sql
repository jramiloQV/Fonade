CREATE TABLE [dbo].[Viatico] (
    [Id_Viatico]     INT   IDENTITY (1, 1) NOT NULL,
    [LimiteInferior] MONEY NOT NULL,
    [LimiteSuperior] MONEY NOT NULL,
    [Valor]          MONEY NOT NULL,
    CONSTRAINT [PK_Viatico] PRIMARY KEY CLUSTERED ([Id_Viatico] ASC) WITH (FILLFACTOR = 50)
);

