CREATE TABLE [dbo].[TipoCuenta] (
    [Id_TipoCuenta] TINYINT      IDENTITY (1, 1) NOT NULL,
    [NomTipoCuenta] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_TipoCuenta] PRIMARY KEY CLUSTERED ([Id_TipoCuenta] ASC) WITH (FILLFACTOR = 50)
);

