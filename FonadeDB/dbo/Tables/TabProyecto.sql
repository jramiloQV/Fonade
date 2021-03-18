CREATE TABLE [dbo].[TabProyecto] (
    [CodTab]            SMALLINT NOT NULL,
    [CodProyecto]       INT      NOT NULL,
    [CodContacto]       INT      NOT NULL,
    [FechaModificacion] DATETIME NOT NULL,
    [Realizado]         BIT      NOT NULL,
    [Completo]          BIT      NULL,
    CONSTRAINT [PK_TabProyecto] PRIMARY KEY CLUSTERED ([CodTab] ASC, [CodProyecto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_TabProyecto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_TabProyecto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);



