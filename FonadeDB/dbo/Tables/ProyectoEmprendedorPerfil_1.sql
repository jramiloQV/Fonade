CREATE TABLE [dbo].[ProyectoEmprendedorPerfil] (
    [IdEmprendedorPerfil] INT           IDENTITY (1, 1) NOT NULL,
    [IdContacto]          INT           NOT NULL,
    [Perfil]              VARCHAR (MAX) NOT NULL,
    [Rol]                 VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoEmprendedorPerfil] PRIMARY KEY CLUSTERED ([IdEmprendedorPerfil] ASC),
    CONSTRAINT [FK_ProyectoEmprendedorPerfil_Contacto] FOREIGN KEY ([IdContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);

