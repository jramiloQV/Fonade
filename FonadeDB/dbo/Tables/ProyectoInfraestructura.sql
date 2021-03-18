CREATE TABLE [dbo].[ProyectoInfraestructura] (
    [Id_ProyectoInfraestructura] INT           IDENTITY (1, 1) NOT NULL,
    [codProyecto]                INT           NOT NULL,
    [NomInfraestructura]         VARCHAR (100) NOT NULL,
    [CodTipoInfraestructura]     TINYINT       NOT NULL,
    [Unidad]                     VARCHAR (10)  NULL,
    [ValorUnidad]                MONEY         NULL,
    [Cantidad]                   FLOAT (53)    NULL,
    [FechaCompra]                SMALLDATETIME NOT NULL,
    [ValorCredito]               FLOAT (53)    NOT NULL,
    [PeriodosAmortizacion]       TINYINT       NOT NULL,
    [SistemaDepreciacion]        VARCHAR (50)  NULL,
    [RequisitosTecnicos]         VARCHAR (MAX) NULL,
    [IdFuente]                   INT           NULL,
    CONSTRAINT [PK_ProyectoInfraestructura] PRIMARY KEY CLUSTERED ([Id_ProyectoInfraestructura] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Proyecto_TipoInfraestructura] FOREIGN KEY ([CodTipoInfraestructura]) REFERENCES [dbo].[TipoInfraestructura] ([Id_TipoInfraestructura]),
    CONSTRAINT [FK_ProyectoInfraestructura_FuenteFinanciacion] FOREIGN KEY ([IdFuente]) REFERENCES [dbo].[FuenteFinanciacion] ([IdFuente])
);






GO
CREATE NONCLUSTERED INDEX [_dta_index_ProyectoInfraestructura_6_192719739__K2_K4_1_3_5_6_7_8_9_10_11]
    ON [dbo].[ProyectoInfraestructura]([codProyecto] ASC, [CodTipoInfraestructura] ASC)
    INCLUDE([Id_ProyectoInfraestructura], [Unidad], [ValorUnidad], [Cantidad], [FechaCompra], [ValorCredito], [PeriodosAmortizacion], [SistemaDepreciacion], [NomInfraestructura]) WITH (FILLFACTOR = 50);


GO


