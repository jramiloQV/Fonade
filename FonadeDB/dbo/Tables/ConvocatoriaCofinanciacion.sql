CREATE TABLE [dbo].[ConvocatoriaCofinanciacion] (
    [CodConvocatoria] INT        NOT NULL,
    [CodCiudad]       INT        NOT NULL,
    [Cofinanciacion]  FLOAT (53) NULL,
    CONSTRAINT [PK_ConvocatoriaCofinanciacion] PRIMARY KEY CLUSTERED ([CodConvocatoria] ASC, [CodCiudad] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaCofinanciacion_Ciudad] FOREIGN KEY ([CodCiudad]) REFERENCES [dbo].[Ciudad] ([Id_Ciudad]),
    CONSTRAINT [FK_ConvocatoriaCofinanciacion_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria])
);

