CREATE TABLE [dbo].[Rol] (
    [Id_Rol]      TINYINT      IDENTITY (1, 1) NOT NULL,
    [Nombre]      VARCHAR (80) NOT NULL,
    [Abreviacion] VARCHAR (10) NULL,
    [Organizador] BIT          NOT NULL,
    CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED ([Id_Rol] ASC) WITH (FILLFACTOR = 50)
);

