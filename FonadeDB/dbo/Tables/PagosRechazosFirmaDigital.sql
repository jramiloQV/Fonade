CREATE TABLE [dbo].[PagosRechazosFirmaDigital] (
    [Id_Rechazo]  INT           IDENTITY (1, 1) NOT NULL,
    [Descripcion] VARCHAR (255) NULL,
    CONSTRAINT [PK_PagosRechazosFirmaDigital] PRIMARY KEY CLUSTERED ([Id_Rechazo] ASC) WITH (FILLFACTOR = 50)
);

