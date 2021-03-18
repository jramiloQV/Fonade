CREATE TABLE [dbo].[TareaUsuario] (
    [Id_TareaUsuario]      INT           IDENTITY (1, 1) NOT NULL,
    [CodTareaPrograma]     INT           NOT NULL,
    [CodContacto]          INT           NOT NULL,
    [CodProyecto]          INT           NULL,
    [NomTareaUsuario]      VARCHAR (255) NOT NULL,
    [Descripcion]          TEXT          NULL,
    [Recurrente]           VARCHAR (3)   NOT NULL,
    [RecordatorioEmail]    BIT           NOT NULL,
    [NivelUrgencia]        SMALLINT      NOT NULL,
    [RecordatorioPantalla] BIT           NOT NULL,
    [RequiereRespuesta]    BIT           NOT NULL,
    [CodContactoAgendo]    INT           NOT NULL,
    [DocumentoRelacionado] VARCHAR (255) NULL,
    CONSTRAINT [PK_TareaUsuario] PRIMARY KEY CLUSTERED ([Id_TareaUsuario] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_TareaUsuario_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_TareaUsuario_Contacto1] FOREIGN KEY ([CodContactoAgendo]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_TareaUsuario_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]),
    CONSTRAINT [FK_TareaUsuario_TareaPrograma] FOREIGN KEY ([CodTareaPrograma]) REFERENCES [dbo].[TareaPrograma] ([Id_TareaPrograma])
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_TareaUsuario_6_1911013889__K2_K4_K1]
    ON [dbo].[TareaUsuario]([CodTareaPrograma] ASC, [CodProyecto] ASC, [Id_TareaUsuario] ASC) WITH (FILLFACTOR = 50);


GO
CREATE STATISTICS [_dta_stat_1911013889_1_4_2]
    ON [dbo].[TareaUsuario]([Id_TareaUsuario], [CodProyecto], [CodTareaPrograma]);


GO
CREATE STATISTICS [_dta_stat_1911013889_2_1]
    ON [dbo].[TareaUsuario]([CodTareaPrograma], [Id_TareaUsuario]);


GO
CREATE STATISTICS [_dta_stat_1911013889_4]
    ON [dbo].[TareaUsuario]([CodProyecto]);

