CREATE TABLE [dbo].[ConvocatoriaCampo] (
    [codConvocatoria] INT      NOT NULL,
    [codCampo]        SMALLINT NOT NULL,
    [Puntaje]         SMALLINT NULL,
    CONSTRAINT [PK_ConvocatoriaCampo] PRIMARY KEY CLUSTERED ([codConvocatoria] ASC, [codCampo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ConvocatoriaCampo_Campo] FOREIGN KEY ([codCampo]) REFERENCES [dbo].[Campo] ([id_Campo]),
    CONSTRAINT [FK_ConvocatoriaCampo_Convocatoria] FOREIGN KEY ([codConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria])
);

