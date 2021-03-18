CREATE TABLE [dbo].[ContactoActualizoReactivacion] (
    [CodContacto]       INT      NOT NULL,
    [ActualizoDatos]    INT      NULL,
    [CambioClave]       INT      NULL,
    [FechaReactivacion] DATETIME CONSTRAINT [DF_ContactoActualizoReactivacion_FechaReactivacion] DEFAULT (getdate()) NOT NULL
);

