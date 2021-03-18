CREATE TABLE [dbo].[ProyectoSolFichaTecnica] (
    [IdProductoFT]       INT           NOT NULL,
    [IdProyecto]         INT           NOT NULL,
    [ProductoEspecifico] VARCHAR (MAX) NOT NULL,
    [UnidadMedida]       VARCHAR (MAX) NOT NULL,
    [Descripcion]        VARCHAR (MAX) NOT NULL,
    [Condiciones]        VARCHAR (MAX) NOT NULL,
    [Composicion]        VARCHAR (MAX) NOT NULL,
    [OtroCual]           VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoSolFichaTecnica] PRIMARY KEY CLUSTERED ([IdProductoFT] ASC),
    CONSTRAINT [FK_ProyectoSolFichaTecnica_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

