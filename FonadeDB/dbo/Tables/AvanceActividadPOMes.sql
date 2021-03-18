CREATE TABLE [dbo].[AvanceActividadPOMes] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [CodActividad]             INT            NOT NULL,
    [Mes]                      TINYINT        NOT NULL,
    [CodTipoFinanciacion]      TINYINT        NOT NULL,
    [Valor]                    MONEY          NOT NULL,
    [Observaciones]            VARCHAR (5120) NULL,
    [CodContacto]              INT            NULL,
    [ObservacionesInterventor] VARCHAR (5120) NULL,
    [Aprobada]                 BIT            NULL,
    CONSTRAINT [PK_AvanceActividadPOMes] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_codactividadmes]
    ON [dbo].[AvanceActividadPOMes]([CodActividad] ASC, [Mes] ASC) WITH (FILLFACTOR = 50);

