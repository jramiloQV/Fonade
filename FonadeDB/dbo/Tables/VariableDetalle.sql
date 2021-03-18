CREATE TABLE [dbo].[VariableDetalle] (
    [Id_VariableDetalle] INT  IDENTITY (1, 1) NOT NULL,
    [CodVariable]        INT  NOT NULL,
    [Cumplimiento]       TEXT NULL,
    [Observacion]        TEXT NULL,
    [Cumple]             BIT  NULL,
    [Seguimiento]        BIT  NULL,
    [IndicadorAsociado]  TEXT NULL,
    [CodInforme]         INT  NULL
);

