CREATE TABLE [dbo].[ProyectoProyeccion] (
    [IdProyeccion]  INT           NOT NULL,
    [IdProductoFT]  INT           NOT NULL,
    [Cantidades]    VARCHAR (MAX) NOT NULL,
    [PreciosVenta]  VARCHAR (MAX) NOT NULL,
    [FormaPago]     VARCHAR (MAX) NOT NULL,
    [Justificacion] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoProyeccion] PRIMARY KEY CLUSTERED ([IdProyeccion] ASC),
    CONSTRAINT [FK_ProyectoProyeccion_ProyectoSolFichaTecnica] FOREIGN KEY ([IdProductoFT]) REFERENCES [dbo].[ProyectoSolFichaTecnica] ([IdProductoFT])
);

