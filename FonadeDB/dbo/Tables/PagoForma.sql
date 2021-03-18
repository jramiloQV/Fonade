CREATE TABLE [dbo].[PagoForma] (
    [id_PagoForma] INT          IDENTITY (1, 1) NOT NULL,
    [NomPagoForma] VARCHAR (50) NULL,
    CONSTRAINT [PK_PagoForma] PRIMARY KEY CLUSTERED ([id_PagoForma] ASC) WITH (FILLFACTOR = 50)
);

