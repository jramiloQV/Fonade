CREATE TABLE [dbo].[InterventorProduccion] (
    [id_produccion] INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]   INT           NOT NULL,
    [NomProducto]   VARCHAR (100) NULL,
    CONSTRAINT [PK_InterventorProduccion] PRIMARY KEY CLUSTERED ([id_produccion] ASC) WITH (FILLFACTOR = 50)
);

