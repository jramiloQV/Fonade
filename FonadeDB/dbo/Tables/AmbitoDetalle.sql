CREATE TABLE [dbo].[AmbitoDetalle] (
    [Id_AmbitoDetalle] INT  IDENTITY (1, 1) NOT NULL,
    [CodAmbito]        INT  NOT NULL,
    [Cumplimiento]     TEXT NULL,
    [Observacion]      TEXT NULL,
    [Cumple]           BIT  NULL,
    [Seguimiento]      BIT  NULL,
    [CodInforme]       INT  NULL
);

