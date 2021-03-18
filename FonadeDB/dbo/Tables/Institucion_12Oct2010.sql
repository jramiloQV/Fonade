CREATE TABLE [dbo].[Institucion_12Oct2010] (
    [Id_Institucion]      INT            IDENTITY (1, 1) NOT NULL,
    [NomInstitucion]      VARCHAR (80)   NOT NULL,
    [NomUnidad]           VARCHAR (80)   NOT NULL,
    [Nit]                 NUMERIC (18)   NOT NULL,
    [RegistroIcfes]       VARCHAR (25)   NULL,
    [FechaRegistro]       SMALLDATETIME  NULL,
    [Direccion]           VARCHAR (100)  NOT NULL,
    [Telefono]            VARCHAR (20)   NOT NULL,
    [Fax]                 VARCHAR (20)   NOT NULL,
    [WebSite]             VARCHAR (100)  NULL,
    [Comentario]          TEXT           NULL,
    [CodCiudad]           INT            NOT NULL,
    [Inactivo]            BIT            NOT NULL,
    [FechaInicioInactivo] SMALLDATETIME  NULL,
    [FechaFinInactivo]    SMALLDATETIME  NULL,
    [CriteriosSeleccion]  VARCHAR (1000) NULL,
    [CodTipoInstitucion]  TINYINT        NOT NULL,
    [MotivoCambioJefe]    VARCHAR (500)  NULL
);

