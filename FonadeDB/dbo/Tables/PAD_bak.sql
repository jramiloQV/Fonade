CREATE TABLE [dbo].[PAD_bak] (
    [Id_ProyectoAcreditacionDocumento] INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto]                      INT            NOT NULL,
    [CodContacto]                      INT            NOT NULL,
    [CodConvocatoria]                  INT            NOT NULL,
    [Pendiente]                        BIT            NOT NULL,
    [FechaPendiente]                   DATETIME       NULL,
    [ObservacionPendiente]             VARCHAR (2000) NULL,
    [Subsanado]                        BIT            NULL,
    [ObservacionSubsanado]             VARCHAR (2000) NULL,
    [FechaSubsanado]                   DATETIME       NULL,
    [Acreditado]                       BIT            NULL,
    [ObservacionAcreditado]            VARCHAR (2000) NULL,
    [FechaAcreditado]                  DATETIME       NULL,
    [NoAcreditado]                     BIT            NULL,
    [ObservacionNoAcreditado]          VARCHAR (2000) NULL,
    [FechaNoAcreditado]                DATETIME       NULL,
    [FlagAnexo1]                       BIT            NULL,
    [FlagAnexo2]                       BIT            NULL,
    [FlagAnexo3]                       BIT            NULL,
    [FlagCertificaciones]              BIT            NULL,
    [FlagDiploma]                      BIT            NULL,
    [FlagActa]                         BIT            NULL,
    [FlagDI]                           BIT            NULL,
    [AsuntoPendiente]                  VARCHAR (150)  NULL,
    [AsuntoSubsanado]                  VARCHAR (150)  NULL,
    [AsuntoAcreditado]                 VARCHAR (150)  NULL,
    [AsuntoNoAcreditado]               VARCHAR (150)  NULL
);

