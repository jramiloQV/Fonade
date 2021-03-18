CREATE TABLE [dbo].[PagoActividadHistorico] (
    [CodPagoActividad]          INT           NOT NULL,
    [Estado]                    INT           NOT NULL,
    [ObservacionesFA]           TEXT          NULL,
    [FechaRtaFA]                DATETIME      NULL,
    [valorretefuente]           INT           DEFAULT ((0)) NULL,
    [valorreteiva]              INT           DEFAULT ((0)) NULL,
    [valorreteica]              INT           DEFAULT ((0)) NULL,
    [otrosdescuentos]           INT           DEFAULT ((0)) NULL,
    [codigopago]                VARCHAR (255) NULL,
    [valorpagado]               INT           DEFAULT ((0)) NULL,
    [fechapago]                 DATETIME      NULL,
    [fecharechazo]              DATETIME      NULL,
    [ObservacionCambio]         TEXT          NULL,
    [Id_PagoActividadHistorico] INT           IDENTITY (1, 1) NOT NULL,
    [fechaRegistro]             DATETIME      DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_PagoActividadHistorico] PRIMARY KEY CLUSTERED ([Id_PagoActividadHistorico] ASC) WITH (FILLFACTOR = 50)
);

