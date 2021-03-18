CREATE TABLE [dbo].[tareausuarioold] (
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
    [DocumentoRelacionado] VARCHAR (255) NULL
);

