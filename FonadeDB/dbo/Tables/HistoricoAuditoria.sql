CREATE TABLE [dbo].[HistoricoAuditoria] (
    [Id_Historico_Auditoria] INT      IDENTITY (1, 1) NOT NULL,
    [Auditar_Activo]         BIT      NOT NULL,
    [Insertar]               BIT      NOT NULL,
    [Modificar]              BIT      NOT NULL,
    [Eliminar]               BIT      NOT NULL,
    [ultima_configuracion]   BIT      NOT NULL,
    [fecha_configuracion]    DATETIME NOT NULL,
    [Usuario_configuracion]  INT      NOT NULL,
    CONSTRAINT [PK_HistoricoAuditoria] PRIMARY KEY CLUSTERED ([Id_Historico_Auditoria] ASC)
);

