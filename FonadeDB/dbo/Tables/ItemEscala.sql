CREATE TABLE [dbo].[ItemEscala] (
    [CodItem] INT           NOT NULL,
    [Texto]   VARCHAR (255) NOT NULL,
    [Puntaje] SMALLINT      NOT NULL,
    CONSTRAINT [FK_ItemEscala_Item] FOREIGN KEY ([CodItem]) REFERENCES [dbo].[Item] ([Id_Item])
);

