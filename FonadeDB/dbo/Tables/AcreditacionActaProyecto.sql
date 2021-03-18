CREATE TABLE [dbo].[AcreditacionActaProyecto] (
    [CodActa]     INT NOT NULL,
    [CodProyecto] INT NOT NULL,
    [Acreditado]  BIT NOT NULL,
    CONSTRAINT [PK_AcreditacionActaProyecto] PRIMARY KEY CLUSTERED ([CodActa] ASC, [CodProyecto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_AcreditacionActaProyecto _Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_AcreditacionActaProyecto_AcreditacionActa] FOREIGN KEY ([CodActa]) REFERENCES [dbo].[AcreditacionActa] ([Id_Acta])
);

