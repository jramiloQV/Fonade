CREATE TABLE [dbo].[InterventorIndicadorTMP] (
    [Id_IndicadorInter]     INT            NOT NULL,
    [CodProyecto]           INT            NOT NULL,
    [Aspecto]               VARCHAR (300)  NULL,
    [FechaSeguimiento]      VARCHAR (60)   NULL,
    [Numerador]             VARCHAR (100)  NULL,
    [Denominador]           VARCHAR (100)  NULL,
    [Descripcion]           VARCHAR (300)  NULL,
    [RangoAceptable]        TINYINT        NULL,
    [CodTipoIndicadorInter] TINYINT        NULL,
    [Observacion]           VARCHAR (1000) NULL,
    [ChequeoCoordinador]    BIT            NULL,
    [Tarea]                 VARCHAR (50)   CONSTRAINT [DF_InterventorIndicadorTMP_Tarea] DEFAULT ('Adicionar') NULL,
    [ChequeoGerente]        BIT            NULL
);

