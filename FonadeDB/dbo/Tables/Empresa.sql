CREATE TABLE [dbo].[Empresa] (
    [id_empresa]           INT            IDENTITY (1, 1) NOT NULL,
    [razonsocial]          VARCHAR (255)  NULL,
    [codproyecto]          INT            NULL,
    [ObjetoSocial]         VARCHAR (1000) NULL,
    [CapitalSocial]        NUMERIC (18)   NULL,
    [CodTipoSociedad]      INT            NULL,
    [NumEscrituraPublica]  VARCHAR (255)  NULL,
    [DomicilioEmpresa]     VARCHAR (255)  NULL,
    [CodCiudad]            INT            NULL,
    [Telefono]             VARCHAR (100)  NULL,
    [Email]                VARCHAR (100)  NULL,
    [Nit]                  VARCHAR (50)   NULL,
    [RegimenEspecial]      BIT            NULL,
    [RENorma]              VARCHAR (50)   NULL,
    [REFechaNorma]         DATETIME       NULL,
    [Contribuyente]        BIT            NULL,
    [CNorma]               VARCHAR (50)   NULL,
    [CFechaNorma]          DATETIME       NULL,
    [AutoRetenedor]        BIT            NULL,
    [ARNorma]              VARCHAR (50)   NULL,
    [ARFechaNorma]         DATETIME       NULL,
    [Declarante]           BIT            NULL,
    [DNorma]               VARCHAR (50)   NULL,
    [DFechaNorma]          DATETIME       NULL,
    [ExentoRetefuente]     BIT            NULL,
    [ERFNorma]             VARCHAR (50)   NULL,
    [ERFFechaNorma]        DATETIME       NULL,
    [TipoRegimen]          VARCHAR (5)    NULL,
    [GranContribuyente]    BIT            NULL,
    [GCNorma]              VARCHAR (50)   NULL,
    [GCFechaNorma]         DATETIME       NULL,
    [ObservacionesInt]     TEXT           NULL,
    [codDificultadCentral] INT            NULL,
    CONSTRAINT [PK_Empresa] PRIMARY KEY CLUSTERED ([id_empresa] ASC) WITH (FILLFACTOR = 50)
);

