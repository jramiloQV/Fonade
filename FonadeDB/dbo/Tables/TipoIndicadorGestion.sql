CREATE TABLE [dbo].[TipoIndicadorGestion] (
    [Id_TipoIndicador] SMALLINT     NOT NULL,
    [nomTipoIndicador] VARCHAR (80) NOT NULL,
    [Protegido]        BIT          NOT NULL,
    CONSTRAINT [PK_TipoIndicadorGestion] PRIMARY KEY CLUSTERED ([Id_TipoIndicador] ASC) WITH (FILLFACTOR = 50)
);

