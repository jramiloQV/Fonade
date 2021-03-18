CREATE TABLE [dbo].[TareaUsuarioRepeticion] (
    [Id_TareaUsuarioRepeticion] INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]                     SMALLDATETIME NOT NULL,
    [CodTareaUsuario]           INT           NOT NULL,
    [Parametros]                VARCHAR (255) NULL,
    [Respuesta]                 TEXT          NULL,
    [FechaCierre]               SMALLDATETIME NULL,
    [GUID]                      VARCHAR (128) NULL,
    CONSTRAINT [PK_TareaUsuarioRepeticion] PRIMARY KEY CLUSTERED ([Id_TareaUsuarioRepeticion] ASC) WITH (FILLFACTOR = 50)
);


GO
CREATE NONCLUSTERED INDEX [_dta_index_TareaUsuarioRepeticion_6_34099162__K3_K6_K4]
    ON [dbo].[TareaUsuarioRepeticion]([CodTareaUsuario] ASC, [FechaCierre] ASC, [Parametros] ASC) WITH (FILLFACTOR = 50);


GO
CREATE STATISTICS [_dta_stat_34099162_4_6_3]
    ON [dbo].[TareaUsuarioRepeticion]([Parametros], [FechaCierre], [CodTareaUsuario]);

