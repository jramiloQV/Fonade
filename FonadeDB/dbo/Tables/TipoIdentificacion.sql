CREATE TABLE [dbo].[TipoIdentificacion] (
    [Id_TipoIdentificacion] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [NomTipoIdentificacion] VARCHAR (50) NOT NULL,
    [Sigla]                 VARCHAR (2)  NULL,
    [TipoNaturaleza]        VARCHAR (2)  NULL,
    CONSTRAINT [PK_TipoIdentificacion] PRIMARY KEY CLUSTERED ([Id_TipoIdentificacion] ASC) WITH (FILLFACTOR = 50)
);

