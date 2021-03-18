CREATE TABLE [dbo].[InterventorInformeFinalCriterio] (
    [Id_InterventorInformeFinalCriterio] INT           IDENTITY (1, 1) NOT NULL,
    [NomInterventorInformeFinalCriterio] VARCHAR (255) NULL,
    [CodEmpresa]                         INT           NULL,
    CONSTRAINT [PK_InterventorInformeFinalCriterio] PRIMARY KEY CLUSTERED ([Id_InterventorInformeFinalCriterio] ASC) WITH (FILLFACTOR = 50)
);

