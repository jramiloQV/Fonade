CREATE TABLE [dbo].[ServicioRegistroDesactivacion] (
    [Id_ServicioRegistroDesactivacion] INT      IDENTITY (1, 1) NOT NULL,
    [FechaReferencia]                  DATETIME NOT NULL,
    [InicioDesactivacion]              DATETIME CONSTRAINT [DF_ServicioRegistroDesactivacion_InicioDesactivacion] DEFAULT (getdate()) NOT NULL,
    [FinDesactivacion]                 DATETIME NULL,
    [TotalDesactivados]                INT      CONSTRAINT [DF_ServicioRegistroDesactivacion_TotalDesactivados] DEFAULT ((0)) NOT NULL,
    [FechaProcesoNotificacion]         DATETIME NULL,
    CONSTRAINT [PK_ServicioRegistroDesactivacion] PRIMARY KEY CLUSTERED ([Id_ServicioRegistroDesactivacion] ASC) WITH (FILLFACTOR = 50)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Se actualiza cuando se ejecuta el servicio de notificación al usuario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioRegistroDesactivacion', @level2type = N'COLUMN', @level2name = N'FechaProcesoNotificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de comparación para la comprobación de creación, último acceso y reactivación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioRegistroDesactivacion', @level2type = N'COLUMN', @level2name = N'FechaReferencia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Momento en que finaliza la ejecución del SP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioRegistroDesactivacion', @level2type = N'COLUMN', @level2name = N'FinDesactivacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Momento en que se crea el registro, correspondiente al inicio de ejecución del SP', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioRegistroDesactivacion', @level2type = N'COLUMN', @level2name = N'InicioDesactivacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cantidad de registros procesados', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioRegistroDesactivacion', @level2type = N'COLUMN', @level2name = N'TotalDesactivados';

