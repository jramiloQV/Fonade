CREATE TABLE [dbo].[ContactoClaveHistorico] (
    [Id_ContactoClaveHistorico] INT          IDENTITY (1, 1) NOT NULL,
    [NomClave]                  VARCHAR (50) NOT NULL,
    [FechaIngreso]              DATETIME     NULL,
    [codContacto]               INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id_ContactoClaveHistorico] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK__ContactoC__codCo__782AAB3C] FOREIGN KEY ([codContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);

