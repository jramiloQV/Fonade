CREATE TABLE [dbo].[ProyectoNormatividad] (
    [IdNormatividad]      INT           IDENTITY (1, 1) NOT NULL,
    [IdProyecto]          INT           NOT NULL,
    [Empresarial]         VARCHAR (MAX) NOT NULL,
    [Tributaria]          VARCHAR (MAX) NOT NULL,
    [Tecnica]             VARCHAR (MAX) NOT NULL,
    [Laboral]             VARCHAR (MAX) NOT NULL,
    [Ambiental]           VARCHAR (MAX) NOT NULL,
    [RegistroMarca]       VARCHAR (MAX) NOT NULL,
    [CondicionesTecnicas] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_ProyectoNormatividad] PRIMARY KEY CLUSTERED ([IdNormatividad] ASC),
    CONSTRAINT [FK_ProyectoNormatividad_Proyecto] FOREIGN KEY ([IdProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

