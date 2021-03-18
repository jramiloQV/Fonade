CREATE TABLE [dbo].[IndicadorGenerico] (
    [Id_IndicadorGenerico] INT           IDENTITY (1, 1) NOT NULL,
    [CodEmpresa]           INT           NOT NULL,
    [NombreIndicador]      VARCHAR (50)  NOT NULL,
    [Descripcion]          VARCHAR (100) NOT NULL,
    [Numerador]            INT           NULL,
    [Denominador]          INT           NULL,
    [Evaluacion]           VARCHAR (100) NULL,
    [Observacion]          TEXT          NULL,
    CONSTRAINT [PK_IndicadorGenerico] PRIMARY KEY CLUSTERED ([Id_IndicadorGenerico] ASC) WITH (FILLFACTOR = 50)
);

