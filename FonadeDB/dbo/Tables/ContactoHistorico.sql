﻿CREATE TABLE [dbo].[ContactoHistorico] (
    [Id_ContactoHistorico]  INT           IDENTITY (1, 1) NOT NULL,
    [CodContacto]           INT           NOT NULL,
    [Nombres]               VARCHAR (100) NOT NULL,
    [Apellidos]             VARCHAR (100) NOT NULL,
    [CodTipoIdentificacion] SMALLINT      NOT NULL,
    [Identificacion]        FLOAT (53)    NOT NULL,
    [Genero]                CHAR (1)      NULL,
    [FechaNacimiento]       SMALLDATETIME NULL,
    [Cargo]                 VARCHAR (100) NULL,
    [Email]                 VARCHAR (100) NOT NULL,
    [Direccion]             VARCHAR (120) NULL,
    [Telefono]              VARCHAR (100) CONSTRAINT [DF_ContactoHistorico_Telefono] DEFAULT ((0)) NULL,
    [Fax]                   VARCHAR (100) NULL,
    [Experiencia]           VARCHAR (MAX) NULL,
    [Dedicacion]            CHAR (10)     NULL,
    [HojaVida]              TEXT          NULL,
    [Intereses]             VARCHAR (MAX) NULL,
    [CodInstitucion]        INT           NULL,
    [CodCiudad]             INT           NULL,
    [Clave]                 VARCHAR (20)  NOT NULL,
    [Inactivo]              BIT           NOT NULL,
    [InactivoAsignacion]    BIT           CONSTRAINT [DF__ContactoH__Inact__7ED7A8CB] DEFAULT ((0)) NULL,
    [CodTipoAprendiz]       INT           NULL,
    [fechaCreacion]         DATETIME      NULL,
    [InformacionIncompleta] BIT           NULL,
    [LugarExpedicionDI]     INT           NULL,
    [fechaActualizacion]    DATETIME      NULL,
    [numActualizacion]      INT           NOT NULL,
    [fechaRegistro]         DATETIME      CONSTRAINT [DF__ContactoH__fecha__7FCBCD04] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ContactoHistorico] PRIMARY KEY CLUSTERED ([Id_ContactoHistorico] ASC) WITH (FILLFACTOR = 50)
);

