CREATE TABLE [dbo].[InterventorNomina] (
    [Id_Nomina]   INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto] INT           NOT NULL,
    [Cargo]       VARCHAR (100) NULL,
    [Tipo]        VARCHAR (50)  NULL,
    CONSTRAINT [PK_InterventorNomina] PRIMARY KEY CLUSTERED ([Id_Nomina] ASC) WITH (FILLFACTOR = 50)
);

