CREATE TABLE [dbo].[InterventorProduccionTMP] (
    [id_produccion]      INT           NOT NULL,
    [CodProyecto]        INT           NOT NULL,
    [NomProducto]        VARCHAR (100) NULL,
    [ChequeoCoordinador] BIT           NULL,
    [Tarea]              VARCHAR (50)  CONSTRAINT [DF_InterventorProduccionTMP_Tarea] DEFAULT ('Adicionar') NULL,
    [ChequeoGerente]     BIT           NULL
);

