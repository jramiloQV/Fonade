CREATE TABLE [dbo].[docperdidosAnalisis] (
    [id_proyecto]     INT          NOT NULL,
    [nomproyecto]     VARCHAR (80) NOT NULL,
    [nomunidad]       VARCHAR (80) NOT NULL,
    [nomestado]       VARCHAR (80) NOT NULL,
    [id_convocatoria] INT          NULL,
    [Total]           MONEY        NULL
);

