CREATE TABLE [dbo].[TipoAmbito] (
    [Id_TipoAmbito] INT  IDENTITY (1, 1) NOT NULL,
    [NomTipoAmbito] TEXT NULL,
    CONSTRAINT [PK_TipoAmbito] PRIMARY KEY CLUSTERED ([Id_TipoAmbito] ASC) WITH (FILLFACTOR = 50)
);

