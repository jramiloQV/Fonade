CREATE TABLE [dbo].[OProyectoSel] (
    [Id_Proyecto]         INT            NOT NULL,
    [NomProyecto]         VARCHAR (80)   NOT NULL,
    [Sumario]             VARCHAR (1024) NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [FechaDesactivacion]  DATETIME       NULL,
    [MotivoDesactivacion] VARCHAR (300)  NULL,
    [CodTipoProyecto]     SMALLINT       NOT NULL,
    [CodEstado]           TINYINT        NOT NULL,
    [CostoTotal]          MONEY          NULL,
    [CodCiudad]           INT            NOT NULL,
    [CodSubSector]        INT            NOT NULL,
    [CodContacto]         INT            NOT NULL,
    [CodInstitucion]      INT            NOT NULL,
    [Inactivo]            BIT            NOT NULL,
    [Latitud]             INT            NULL,
    [Longitud]            INT            NULL
);

