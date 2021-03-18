CREATE TABLE [dbo].[IndicadorModelo] (
    [id_IndicadorModelo] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Descripcion]        VARCHAR (255) NULL,
    [Tipo]               CHAR (1)      NOT NULL,
    CONSTRAINT [PK_IndicadorModelo] PRIMARY KEY CLUSTERED ([id_IndicadorModelo] ASC) WITH (FILLFACTOR = 50)
);

