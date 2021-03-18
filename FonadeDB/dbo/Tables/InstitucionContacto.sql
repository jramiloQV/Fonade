CREATE TABLE [dbo].[InstitucionContacto] (
    [CodInstitucion]   INT           NOT NULL,
    [CodContacto]      INT           NOT NULL,
    [FechaInicio]      SMALLDATETIME NOT NULL,
    [FechaFin]         SMALLDATETIME NULL,
    [MotivoCambioJefe] VARCHAR (500) NULL,
    CONSTRAINT [PK_InstitucionContacto] PRIMARY KEY CLUSTERED ([CodInstitucion] ASC, [CodContacto] ASC, [FechaInicio] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_InstitucionContacto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_InstitucionContacto_Institucion] FOREIGN KEY ([CodInstitucion]) REFERENCES [dbo].[Institucion] ([Id_Institucion])
);

