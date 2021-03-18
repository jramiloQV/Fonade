CREATE TABLE [dbo].[LogTrasladoProyectos] (
    [id_LogTrasladoProyectos] INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]                   DATETIME      NOT NULL,
    [CodContacto]             INT           NOT NULL,
    [CodInstituciónOld]       INT           NULL,
    [CodInstituciónNew]       INT           NULL,
    [txtSQL]                  VARCHAR (500) NOT NULL,
    CONSTRAINT [PK_LogTrasladoProyectos] PRIMARY KEY CLUSTERED ([id_LogTrasladoProyectos] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_LogTrasladoProyectos_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_LogTrasladoProyectos_Institucion] FOREIGN KEY ([CodInstituciónOld]) REFERENCES [dbo].[Institucion] ([Id_Institucion]),
    CONSTRAINT [FK_LogTrasladoProyectos_Institucion1] FOREIGN KEY ([CodInstituciónNew]) REFERENCES [dbo].[Institucion] ([Id_Institucion])
);

