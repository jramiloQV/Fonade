CREATE TABLE [dbo].[ProyectoProtagonistaClientes] (
    [IdCliente]     INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]    INT           NOT NULL,
    [Nombre]        VARCHAR (100) NOT NULL,
    [Perfil]        VARCHAR (250) NOT NULL,
    [Localizacion]  VARCHAR (250) NOT NULL,
    [Justificacion] VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_ProyectoProtagonistaClientes] PRIMARY KEY CLUSTERED ([IdCliente] ASC),
    CONSTRAINT [FK_ProyectoProtagonistaClientes_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

