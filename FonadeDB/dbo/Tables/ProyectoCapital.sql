CREATE TABLE [dbo].[ProyectoCapital] (
    [Id_Capital]            INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]           INT           NOT NULL,
    [Componente]            VARCHAR (100) NOT NULL,
    [Valor]                 MONEY         NOT NULL,
    [Observacion]           VARCHAR (300) NULL,
    [codFuenteFinanciacion] INT           NULL,
    CONSTRAINT [PK_ProyectoCapital] PRIMARY KEY CLUSTERED ([Id_Capital] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoCapital_FuenteFinanciacion1] FOREIGN KEY ([codFuenteFinanciacion]) REFERENCES [dbo].[FuenteFinanciacion] ([IdFuente]),
    CONSTRAINT [FK_ProyectocCapital_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



