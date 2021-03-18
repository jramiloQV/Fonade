CREATE TABLE [dbo].[ContactoReActivacion] (
    [Id_ContactoReActivacion] INT      IDENTITY (1, 1) NOT NULL,
    [CodContacto]             INT      NULL,
    [FechaReActivacion]       DATETIME NULL,
    [CodContactoQReActiva]    INT      NULL,
    CONSTRAINT [PK_ContactoReActivacion] PRIMARY KEY CLUSTERED ([Id_ContactoReActivacion] ASC) WITH (FILLFACTOR = 50)
);

