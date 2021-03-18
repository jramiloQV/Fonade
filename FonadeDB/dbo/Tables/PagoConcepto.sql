CREATE TABLE [dbo].[PagoConcepto] (
    [Id_PagoConcepto]    INT           IDENTITY (1, 1) NOT NULL,
    [CodigoPagoConcepto] CHAR (10)     NULL,
    [NomPagoConcepto]    VARCHAR (255) NULL,
    CONSTRAINT [PK_PagoConcepto] PRIMARY KEY CLUSTERED ([Id_PagoConcepto] ASC) WITH (FILLFACTOR = 50)
);

