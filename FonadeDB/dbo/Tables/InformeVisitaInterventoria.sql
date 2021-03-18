CREATE TABLE [dbo].[InformeVisitaInterventoria] (
    [Id_Informe]            INT           IDENTITY (1, 1) NOT NULL,
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
    [Estado]                BIT           NOT NULL,
    CONSTRAINT [PK_InformeVisitaInterventoria] PRIMARY KEY CLUSTERED ([Id_Informe] ASC) WITH (FILLFACTOR = 50)
);

