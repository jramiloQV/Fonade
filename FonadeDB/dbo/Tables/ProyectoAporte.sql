CREATE TABLE [dbo].[ProyectoAporte] (
    [Id_Aporte]   INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto] INT            NOT NULL,
    [Nombre]      VARCHAR (100)  NOT NULL,
    [Valor]       MONEY          NOT NULL,
    [TipoAporte]  VARCHAR (30)   NOT NULL,
    [Detalle]     VARCHAR (1000) NULL,
    CONSTRAINT [PK_ProyectoAporte] PRIMARY KEY CLUSTERED ([Id_Aporte] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoAporte_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

