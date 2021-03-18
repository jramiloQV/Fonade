CREATE TABLE [dbo].[AvanceVentasPOMes] (
    [Id]                       INT            IDENTITY (1, 1) NOT NULL,
    [CodProducto]              INT            NOT NULL,
    [Mes]                      TINYINT        NOT NULL,
    [CodTipoFinanciacion]      TINYINT        NOT NULL,
    [Valor]                    MONEY          NOT NULL,
    [Observaciones]            VARCHAR (5120) NULL,
    [CodContacto]              INT            NULL,
    [ObservacionesInterventor] VARCHAR (5120) NULL,
    [Aprobada]                 BIT            NULL,
    CONSTRAINT [PK_AvanceVentasPOMes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

