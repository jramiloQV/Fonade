CREATE TABLE [dbo].[LegalizacionActaProyecto] (
    [CodActa]       INT NOT NULL,
    [CodProyecto]   INT NOT NULL,
    [Garantia]      BIT NULL,
    [Pagare]        BIT NULL,
    [Contrato]      BIT NULL,
    [PlanOperativo] BIT NULL,
    [Legalizado]    BIT NULL,
    CONSTRAINT [PK_LegalizacionActaProyecto] PRIMARY KEY CLUSTERED ([CodActa] ASC, [CodProyecto] ASC) WITH (FILLFACTOR = 50)
);

