CREATE TABLE [dbo].[ProyectoActividadPOMes] (
    [CodActividad]        INT     NOT NULL,
    [Mes]                 TINYINT NOT NULL,
    [CodTipoFinanciacion] TINYINT NOT NULL,
    [Valor]               MONEY   NOT NULL,
    CONSTRAINT [FK_ProyectoActividadPOMes_TipoFinanciacion] FOREIGN KEY ([CodTipoFinanciacion]) REFERENCES [dbo].[TipoFinanciacion] ([Id_TipoFinanciacion])
);

