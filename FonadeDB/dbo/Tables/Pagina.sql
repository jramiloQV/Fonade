CREATE TABLE [dbo].[Pagina] (
    [Id_Pagina]  INT           IDENTITY (1, 1) NOT NULL,
    [Titulo]     VARCHAR (255) NOT NULL,
    [url_Pagina] VARCHAR (255) NOT NULL,
    [Orden]      INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id_Pagina] ASC),
    UNIQUE NONCLUSTERED ([Orden] ASC)
);

