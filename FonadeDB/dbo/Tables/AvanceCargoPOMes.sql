CREATE TABLE [dbo].[AvanceCargoPOMes] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [CodCargo]                 INT            NOT NULL,
    [Mes]                      TINYINT        NOT NULL,
    [CodTipoFinanciacion]      TINYINT        NOT NULL,
    [Valor]                    MONEY          NOT NULL,
    [Observaciones]            VARCHAR (5120) NULL,
    [CodContacto]              INT            NULL,
    [ObservacionesInterventor] VARCHAR (5120) NULL,
    [Aprobada]                 BIT            NULL,
    CONSTRAINT [PK_AvanceCargoPOMes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ID_AvanceCargoPOMes_CodCargoMes]
    ON [dbo].[AvanceCargoPOMes]([CodCargo] ASC, [Mes] ASC) WITH (FILLFACTOR = 50);

