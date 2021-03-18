CREATE TABLE [dbo].[pruebas_gio] (
    [NomConvocatoria]     VARCHAR (80)  NOT NULL,
    [CodProyecto]         INT           NOT NULL,
    [NomProyecto]         VARCHAR (80)  NOT NULL,
    [Id_Emprendedor]      INT           NOT NULL,
    [Emprendedor]         VARCHAR (201) NOT NULL,
    [Evaluador]           VARCHAR (201) NOT NULL,
    [Fecha_Formalizacion] SMALLDATETIME NULL
);

