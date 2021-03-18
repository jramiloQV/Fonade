CREATE TABLE [dbo].[ProyectoGastos] (
    [Id_Gasto]    INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto] INT           NOT NULL,
    [Descripcion] VARCHAR (255) NOT NULL,
    [Valor]       MONEY         NOT NULL,
    [Tipo]        CHAR (10)     NOT NULL,
    [Protegido]   BIT           NOT NULL,
    CONSTRAINT [PK_ProyectoGastos] PRIMARY KEY CLUSTERED ([Id_Gasto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoGastos_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

