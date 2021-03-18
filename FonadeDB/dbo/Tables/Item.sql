CREATE TABLE [dbo].[Item] (
    [Id_Item]          INT           IDENTITY (1, 1) NOT NULL,
    [NomItem]          VARCHAR (255) NOT NULL,
    [CodTabEvaluacion] SMALLINT      NOT NULL,
    [Protegido]        BIT           NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id_Item] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Item_TabEvaluacion] FOREIGN KEY ([CodTabEvaluacion]) REFERENCES [dbo].[TabEvaluacion] ([Id_TabEvaluacion])
);

