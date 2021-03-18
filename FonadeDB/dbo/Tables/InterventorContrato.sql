CREATE TABLE [dbo].[InterventorContrato] (
    [Id_InterventorContrato] INT           IDENTITY (1, 1) NOT NULL,
    [CodContacto]            INT           NOT NULL,
    [numContrato]            INT           NOT NULL,
    [FechaInicio]            SMALLDATETIME NOT NULL,
    [FechaExpiracion]        SMALLDATETIME NULL,
    [Motivo]                 VARCHAR (500) NULL,
    CONSTRAINT [PK_InterventorContrato] PRIMARY KEY CLUSTERED ([Id_InterventorContrato] ASC) WITH (FILLFACTOR = 50)
);

