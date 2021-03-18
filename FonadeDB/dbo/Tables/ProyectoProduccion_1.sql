CREATE TABLE [dbo].[ProyectoProduccion] (
    [IdProduccion]        INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]          INT           NOT NULL,
    [CondicionesTecnicas] VARCHAR (MAX) NOT NULL,
    [RealizaImportacion]  BIT           NOT NULL,
    [Justificacion]       VARCHAR (MAX) NOT NULL,
    [ActivosProveedores]  VARCHAR (MAX) NOT NULL,
    [IncrementoValor]     VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoProduccion] PRIMARY KEY CLUSTERED ([IdProduccion] ASC),
    CONSTRAINT [FK_ProyectoProduccion_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

