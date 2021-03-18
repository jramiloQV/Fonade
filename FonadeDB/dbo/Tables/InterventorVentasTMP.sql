CREATE TABLE [dbo].[InterventorVentasTMP] (
    [id_ventas]          INT           NOT NULL,
    [CodProyecto]        INT           NOT NULL,
    [NomProducto]        VARCHAR (100) NULL,
    [ChequeoCoordinador] BIT           NULL,
    [Tarea]              VARCHAR (50)  CONSTRAINT [DF_InterventorVentasTMP_Tarea] DEFAULT ('Adicionar') NULL,
    [ChequeoGerente]     BIT           NULL
);

