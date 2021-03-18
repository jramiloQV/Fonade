CREATE TABLE [dbo].[InformeVisitaInterventoriabkp] (
    [Id_Informe]            INT           NOT NULL,
    [NombreInforme]         VARCHAR (255) NOT NULL,
    [CodCiudadOrigen]       INT           NOT NULL,
    [CodCiudadDestino]      INT           NOT NULL,
    [FechaSalida]           SMALLDATETIME NOT NULL,
    [FechaRegreso]          SMALLDATETIME NOT NULL,
    [CostoVisita]           NUMERIC (18)  NULL,
    [InformacionTecnica]    TEXT          NULL,
    [InformacionFinanciera] TEXT          NULL,
    [CodEmpresa]            INT           NOT NULL,
    [FechaInforme]          SMALLDATETIME NOT NULL,
    [CodInterventor]        INT           NOT NULL,
    [Estado]                BIT           NOT NULL
);

