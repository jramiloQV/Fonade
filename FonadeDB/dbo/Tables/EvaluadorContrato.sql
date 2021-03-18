CREATE TABLE [dbo].[EvaluadorContrato] (
    [Id_EvaluadorContrato] INT           IDENTITY (1, 1) NOT NULL,
    [CodContacto]          INT           NOT NULL,
    [numContrato]          INT           NOT NULL,
    [FechaInicio]          SMALLDATETIME NOT NULL,
    [FechaExpiracion]      SMALLDATETIME NULL,
    [Motivo]               VARCHAR (500) NULL,
    CONSTRAINT [PK_EvaluadorContrato] PRIMARY KEY CLUSTERED ([Id_EvaluadorContrato] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluadorContrato_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);

