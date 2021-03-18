CREATE TABLE [dbo].[Campo] (
    [id_Campo]          SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Campo]             VARCHAR (350) NOT NULL,
    [codCampo]          SMALLINT      NULL,
    [Inactivo]          BIT           NULL,
    [IdVersionProyecto] INT           NULL,
    [ValorPorDefecto]   INT           NULL,
    [TipoCampo]         INT           NULL,
    CONSTRAINT [PK_Campo] PRIMARY KEY CLUSTERED ([id_Campo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Campo_Campo] FOREIGN KEY ([codCampo]) REFERENCES [dbo].[Campo] ([id_Campo]),
    CONSTRAINT [FK_Campo_Convocatoria] FOREIGN KEY ([IdVersionProyecto]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_Campo_VersionProyecto] FOREIGN KEY ([IdVersionProyecto]) REFERENCES [dbo].[VersionProyecto] ([IdVersionProyecto])
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'0 para valores numericos, 1 para valores de Si O No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Campo', @level2type = N'COLUMN', @level2name = N'TipoCampo';

