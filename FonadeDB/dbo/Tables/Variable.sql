CREATE TABLE [dbo].[Variable] (
    [id_Variable]     INT           IDENTITY (1, 1) NOT NULL,
    [NomVariable]     VARCHAR (255) NOT NULL,
    [CodTipoVariable] INT           NULL,
    CONSTRAINT [PK_Variable] PRIMARY KEY CLUSTERED ([id_Variable] ASC) WITH (FILLFACTOR = 50)
);

