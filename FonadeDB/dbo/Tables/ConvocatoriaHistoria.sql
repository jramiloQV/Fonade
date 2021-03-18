CREATE TABLE [dbo].[ConvocatoriaHistoria] (
    [Id_ConvocatoriaHistoria] INT           IDENTITY (1, 1) NOT NULL,
    [CodConvocatoria]         INT           NOT NULL,
    [FechaFin]                SMALLDATETIME NOT NULL,
    [Presupuesto]             FLOAT (53)    NOT NULL,
    [CodContacto]             INT           NOT NULL,
    CONSTRAINT [PK_ConvocatoriaHistoria] PRIMARY KEY CLUSTERED ([Id_ConvocatoriaHistoria] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaHistoria_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_ConvocatoriaHistoria_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria])
);

