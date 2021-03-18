CREATE TABLE [dbo].[TipoInstitucion] (
    [Id_TipoInstitucion] TINYINT       IDENTITY (1, 1) NOT NULL,
    [NomTipoInstitucion] VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_TipoInstitucion] PRIMARY KEY CLUSTERED ([Id_TipoInstitucion] ASC) WITH (FILLFACTOR = 50)
);

