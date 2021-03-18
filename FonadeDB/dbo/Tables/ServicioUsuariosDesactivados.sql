CREATE TABLE [dbo].[ServicioUsuariosDesactivados] (
    [Id_ServicioUsuariosDesactivados]  INT      IDENTITY (1, 1) NOT NULL,
    [CodServicioRegistroDesactivacion] INT      NOT NULL,
    [CodContacto]                      INT      NOT NULL,
    [FechaUltimoAcceso]                DATETIME NULL,
    [FechaNotificacion]                DATETIME NULL,
    [EnvioExitoso]                     BIT      CONSTRAINT [DF_ServicioUsuariosDesactivados_EnvioExitoso] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ServicioUsuariosDesactivados] PRIMARY KEY CLUSTERED ([Id_ServicioUsuariosDesactivados] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ServicioUsuariosDesactivados_Contactos] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Determina si el envio de correo presentó algún error', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioUsuariosDesactivados', @level2type = N'COLUMN', @level2name = N'EnvioExitoso';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Momento en que se procesa el registro para enviar el correo de notificación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioUsuariosDesactivados', @level2type = N'COLUMN', @level2name = N'FechaNotificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registro de la fecha encontrada en el sistema', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ServicioUsuariosDesactivados', @level2type = N'COLUMN', @level2name = N'FechaUltimoAcceso';

