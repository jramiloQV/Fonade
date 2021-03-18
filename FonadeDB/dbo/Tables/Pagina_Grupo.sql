CREATE TABLE [dbo].[Pagina_Grupo] (
    [Id_Pagina] INT NOT NULL,
    [Id_Grupo]  INT NOT NULL,
    FOREIGN KEY ([Id_Grupo]) REFERENCES [dbo].[Grupo] ([Id_Grupo]),
    FOREIGN KEY ([Id_Pagina]) REFERENCES [dbo].[Pagina] ([Id_Pagina])
);

