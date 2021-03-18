CREATE TABLE [dbo].[IndicadoresGenericosModelo] (
    [NombreIndicador] VARCHAR (50)  NOT NULL,
    [Descripcion]     VARCHAR (100) NOT NULL,
    [Numerador]       INT           NULL,
    [Denominador]     INT           NULL,
    [Evaluacion]      VARCHAR (100) NULL,
    [Observacion]     TEXT          NULL
);

