﻿CREATE TABLE [dbo].[tareausuariorepeticion25ago2010] (
    [Id_TareaUsuarioRepeticion] INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]                     SMALLDATETIME NOT NULL,
    [CodTareaUsuario]           INT           NOT NULL,
    [Parametros]                VARCHAR (255) NULL,
    [Respuesta]                 TEXT          NULL,
    [FechaCierre]               SMALLDATETIME NULL,
    [GUID]                      VARCHAR (128) NULL
);

