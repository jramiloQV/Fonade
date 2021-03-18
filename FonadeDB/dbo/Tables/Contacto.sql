CREATE TABLE [dbo].[Contacto] (
    [Id_Contacto]           INT           IDENTITY (1, 1) NOT NULL,
    [Nombres]               VARCHAR (100) NOT NULL,
    [Apellidos]             VARCHAR (100) NOT NULL,
    [CodTipoIdentificacion] SMALLINT      NOT NULL,
    [Identificacion]        FLOAT (53)    NOT NULL,
    [Genero]                CHAR (1)      NULL,
    [FechaNacimiento]       SMALLDATETIME NULL,
    [Cargo]                 VARCHAR (100) NULL,
    [Email]                 VARCHAR (100) NOT NULL,
    [Direccion]             VARCHAR (120) NULL,
    [Telefono]              VARCHAR (100) CONSTRAINT [DF_Contacto_Telefono] DEFAULT ((0)) NULL,
    [Fax]                   VARCHAR (100) NULL,
    [Experiencia]           VARCHAR (MAX) NULL,
    [Dedicacion]            CHAR (10)     NULL,
    [HojaVida]              TEXT          NULL,
    [Intereses]             VARCHAR (MAX) NULL,
    [CodInstitucion]        INT           NULL,
    [CodCiudad]             INT           NULL,
    [Clave]                 VARCHAR (20)  NOT NULL,
    [Inactivo]              BIT           NOT NULL,
    [InactivoAsignacion]    BIT           CONSTRAINT [DF__Contacto__Inacti__0B3292B8] DEFAULT ((0)) NULL,
    [CodTipoAprendiz]       INT           NULL,
    [fechaCreacion]         DATETIME      NULL,
    [LugarExpedicionDI]     INT           NULL,
    [InformacionIncompleta] BIT           NULL,
    [fechaActualizacion]    DATETIME      NULL,
    [fechaCambioClave]      DATETIME      NULL,
    [flagAcreditador]       BIT           DEFAULT ((0)) NULL,
    [flagActaParcial]       BIT           DEFAULT ((0)) NULL,
    [flagGeneraReporte]     BIT           DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Contacto] PRIMARY KEY CLUSTERED ([Id_Contacto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Contacto_Ciudad] FOREIGN KEY ([CodCiudad]) REFERENCES [dbo].[Ciudad] ([Id_Ciudad]),
    CONSTRAINT [FK_Contacto_Institucion] FOREIGN KEY ([CodInstitucion]) REFERENCES [dbo].[Institucion] ([Id_Institucion]),
    CONSTRAINT [FK_Contacto_TipoIdentificacion] FOREIGN KEY ([CodTipoIdentificacion]) REFERENCES [dbo].[TipoIdentificacion] ([Id_TipoIdentificacion])
);


GO
CREATE NONCLUSTERED INDEX [email_idx]
    ON [dbo].[Contacto]([Email] ASC) WITH (FILLFACTOR = 50, ALLOW_ROW_LOCKS = OFF);


GO
CREATE NONCLUSTERED INDEX [FechaCreacion_idx]
    ON [dbo].[Contacto]([fechaCreacion] ASC) WITH (FILLFACTOR = 50);


GO
CREATE TRIGGER [dbo].[Crearclave] ON dbo.Contacto 
FOR INSERT, UPDATE, DELETE 
AS
insert into dbo.clave (NomClave, FechaVigencia, FechaDebeActualizar, DebeCambiar, Estado, YaAvisoExpiracion, CodContacto)
select clave, null, null, 0,0,0,id_contacto from contacto
where Id_Contacto not in (select distinct CodContacto from Clave)