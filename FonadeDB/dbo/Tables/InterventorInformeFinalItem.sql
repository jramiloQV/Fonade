CREATE TABLE [dbo].[InterventorInformeFinalItem] (
    [Id_InterventorInformeFinalItem] INT           IDENTITY (1, 1) NOT NULL,
    [NomInterventorInformeFinalItem] VARCHAR (255) NOT NULL,
    [CodInformeFinalCriterio]        INT           NULL,
    [CodEmpresa]                     INT           CONSTRAINT [DF_InterventorInformeFinalItem_CodEmpresa] DEFAULT (0) NULL,
    CONSTRAINT [PK_InterventorInformeFinalItem] PRIMARY KEY CLUSTERED ([Id_InterventorInformeFinalItem] ASC) WITH (FILLFACTOR = 50)
);

