﻿CREATE TABLE [dbo].[bkp_observacionesevaluacion2] (
    [Id_ObservacionesEvaluacion] INT             IDENTITY (1, 1) NOT NULL,
    [NomObservacionesEvaluacion] VARCHAR (250)   NULL,
    [Codconvocatoria]            INT             NULL,
    [CodProyecto]                INT             NULL,
    [NombreConvocatoria]         VARCHAR (250)   NULL,
    [Nombres]                    VARCHAR (250)   NULL,
    [Email]                      VARCHAR (250)   NULL,
    [Perfil]                     VARCHAR (250)   NULL,
    [Comentarios]                NVARCHAR (4000) NULL,
    [Fecha]                      DATETIME        NULL
);

