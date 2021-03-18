CREATE TABLE [dbo].[InterventorNominaTMP] (
    [Id_Nomina]          INT           NOT NULL,
    [CodProyecto]        INT           NOT NULL,
    [Cargo]              VARCHAR (100) NULL,
    [Tipo]               VARCHAR (50)  NULL,
    [ChequeoCoordinador] BIT           NULL,
    [Tarea]              VARCHAR (50)  CONSTRAINT [DF_InterventorNominaTMP_Tarea] DEFAULT ('Adicionar') NULL,
    [ChequeoGerente]     BIT           NULL
);

