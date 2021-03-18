CREATE TABLE [dbo].[LogIngreso] (
    [CodContacto]        INT          NULL,
    [Identificacion]     FLOAT (53)   NULL,
    [FechaUltimoIngreso] DATETIME     NULL,
    [DireccionIP]        VARCHAR (20) NULL,
    [NoLogins]           INT          NULL
);


GO
CREATE NONCLUSTERED INDEX [FechaUltimoIngreso_CodContacto_idx]
    ON [dbo].[LogIngreso]([CodContacto] ASC, [FechaUltimoIngreso] DESC) WITH (FILLFACTOR = 50);

