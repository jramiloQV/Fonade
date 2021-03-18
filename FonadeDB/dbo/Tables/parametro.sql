CREATE TABLE [dbo].[parametro] (
    [id_parametro] INT           IDENTITY (1, 1) NOT NULL,
    [nomparametro] VARCHAR (255) NULL,
    [valor]        VARCHAR (255) NULL,
    CONSTRAINT [pk_parametro] PRIMARY KEY CLUSTERED ([id_parametro] ASC) WITH (FILLFACTOR = 50)
);

