CREATE TABLE [dbo].[ProyectoSolucion] (
    [IdSolucion]               INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]               INT           NOT NULL,
    [ConceptoNegocio]          VARCHAR (MAX) NOT NULL,
    [InnovadorConceptoNegocio] VARCHAR (MAX) NOT NULL,
    [ProductoServicio]         VARCHAR (MAX) NOT NULL,
    [Proceso]                  VARCHAR (MAX) NOT NULL,
    [AceptacionProyecto]       VARCHAR (MAX) NOT NULL,
    [TecnicoProductivo]        VARCHAR (MAX) NOT NULL,
    [Comercial]                VARCHAR (MAX) NOT NULL,
    [Legal]                    VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoSolucion] PRIMARY KEY CLUSTERED ([IdSolucion] ASC),
    CONSTRAINT [FK_ProyectoSolucion_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

