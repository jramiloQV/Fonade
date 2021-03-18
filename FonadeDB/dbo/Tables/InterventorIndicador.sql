CREATE TABLE [dbo].[InterventorIndicador] (
    [Id_IndicadorInter]     INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto]           INT            NOT NULL,
    [Aspecto]               VARCHAR (300)  NOT NULL,
    [FechaSeguimiento]      VARCHAR (60)   NOT NULL,
    [Numerador]             VARCHAR (100)  NOT NULL,
    [Denominador]           VARCHAR (100)  NOT NULL,
    [Descripcion]           VARCHAR (300)  NOT NULL,
    [RangoAceptable]        TINYINT        NOT NULL,
    [CodTipoIndicadorInter] TINYINT        NOT NULL,
    [Observacion]           VARCHAR (1000) NULL,
    CONSTRAINT [PK_InterventorIndicador] PRIMARY KEY CLUSTERED ([Id_IndicadorInter] ASC) WITH (FILLFACTOR = 50)
);

