CREATE TABLE [dbo].[PasswordModelo] (
    [Id_PasswordModelo] INT      NOT NULL,
    [Palabra]           CHAR (9) NOT NULL,
    CONSTRAINT [PK_PasswordModelo] PRIMARY KEY CLUSTERED ([Id_PasswordModelo] ASC) WITH (FILLFACTOR = 50)
);

