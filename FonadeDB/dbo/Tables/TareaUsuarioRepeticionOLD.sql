CREATE TABLE [dbo].[TareaUsuarioRepeticionOLD] (
    [Id_TareaUsuarioRepeticion] INT           NOT NULL,
    [Fecha]                     SMALLDATETIME NOT NULL,
    [CodTareaUsuario]           INT           NOT NULL,
    [Parametros]                VARCHAR (255) NULL,
    [Respuesta]                 TEXT          NULL,
    [FechaCierre]               SMALLDATETIME NULL,
    [GUID]                      VARCHAR (128) NULL
);

