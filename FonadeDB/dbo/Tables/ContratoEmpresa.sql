CREATE TABLE [dbo].[ContratoEmpresa] (
    [Id_Contrato]               INT           IDENTITY (1, 1) NOT NULL,
    [CodEmpresa]                INT           NOT NULL,
    [NumeroContrato]            CHAR (15)     NOT NULL,
    [AnoContrato]               INT           NULL,
    [NumeroCdpDelContrato]      INT           NULL,
    [VigenciaCdpDelContrato]    INT           NULL,
    [ObjetoContrato]            VARCHAR (250) NULL,
    [ClaseContrato]             CHAR (3)      NULL,
    [FechaFirmaDelContrato]     SMALLDATETIME NULL,
    [FechaAprobPolizasContrato] SMALLDATETIME NULL,
    [FechaDeInicioContrato]     SMALLDATETIME NULL,
    [MediacionServDirecto]      CHAR (1)      NULL,
    [DomicilioEnBogota]         CHAR (1)      NULL,
    [ServicioEnBogota]          CHAR (1)      NULL,
    [ValorInicialEnPesos]       INT           NULL,
    [PlazoContratoMeses]        TINYINT       NULL,
    [NumeroAPContrato]          INT           NULL,
    [FechaAP]                   DATETIME      NULL,
    [NumeroPoliza]              VARCHAR (100) NULL,
    [CompaniaSeguros]           VARCHAR (100) NULL,
    CONSTRAINT [PK_ContratoEmpresa] PRIMARY KEY CLUSTERED ([Id_Contrato] ASC) WITH (FILLFACTOR = 50)
);

