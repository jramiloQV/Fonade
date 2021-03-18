CREATE TABLE [dbo].[EvaluadorSector] (
    [CodContacto]        INT         NOT NULL,
    [CodSector]          INT         NOT NULL,
    [Experiencia]        VARCHAR (1) CONSTRAINT [DF__Evaluador__Exper__5A6F5FCC] DEFAULT ('A') NOT NULL,
    [fechaActualizacion] DATETIME    DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [EvaluadorSector_PK] PRIMARY KEY CLUSTERED ([CodContacto] ASC, [CodSector] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluadorSector_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_EvaluadorSector_Sector] FOREIGN KEY ([CodSector]) REFERENCES [dbo].[Sector] ([Id_Sector])
);

