CREATE TABLE [dbo].[Ambito] (
    [id_ambito]     INT           IDENTITY (1, 1) NOT NULL,
    [NomAmbito]     VARCHAR (255) NOT NULL,
    [CodTipoAmbito] INT           NULL,
    CONSTRAINT [PK_Ambito] PRIMARY KEY CLUSTERED ([id_ambito] ASC) WITH (FILLFACTOR = 50)
);

