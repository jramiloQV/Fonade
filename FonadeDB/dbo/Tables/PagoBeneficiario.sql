CREATE TABLE [dbo].[PagoBeneficiario] (
    [Id_PagoBeneficiario]   INT           IDENTITY (1, 1) NOT NULL,
    [CodTipoIdentificacion] INT           NULL,
    [NumIdentificacion]     VARCHAR (20)  NULL,
    [Nombre]                VARCHAR (100) NULL,
    [Apellido]              VARCHAR (100) NULL,
    [RazonSocial]           VARCHAR (100) NULL,
    [TipoRazonSocial]       INT           NULL,
    [CodPagoTipoRetencion]  INT           NULL,
    [CodCiudad]             INT           NULL,
    [Telefono]              VARCHAR (20)  NULL,
    [Direccion]             VARCHAR (100) NULL,
    [Fax]                   VARCHAR (20)  NULL,
    [Email]                 VARCHAR (60)  NULL,
    [CodPagoBanco]          INT           NULL,
    [CodPagoSucursal]       INT           NULL,
    [TipoCuenta]            INT           NULL,
    [NumCuenta]             VARCHAR (20)  NULL,
    [CodEmpresa]            INT           NULL,
    [RegistradoFA]          BIT           CONSTRAINT [DF__pagobenef__Regis__4376EBDB] DEFAULT (0) NULL,
    CONSTRAINT [PK_PagoBeneficiario] PRIMARY KEY CLUSTERED ([Id_PagoBeneficiario] ASC) WITH (FILLFACTOR = 50)
);

