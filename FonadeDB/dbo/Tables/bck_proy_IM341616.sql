CREATE TABLE [dbo].[bck_proy_IM341616] (
    [Id_Proyecto]         INT            IDENTITY (1, 1) NOT NULL,
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

