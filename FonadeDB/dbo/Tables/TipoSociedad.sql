CREATE TABLE [dbo].[TipoSociedad] (
    [Id_TipoSociedad] INT          IDENTITY (1, 1) NOT NULL,
    [NomTipoSociedad] VARCHAR (50) NULL,
    CONSTRAINT [PK_TipoSociedad] PRIMARY KEY CLUSTERED ([Id_TipoSociedad] ASC) WITH (FILLFACTOR = 50)
);

