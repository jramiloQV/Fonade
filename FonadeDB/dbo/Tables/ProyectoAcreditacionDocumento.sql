﻿CREATE TABLE [dbo].[ProyectoAcreditacionDocumento] (
    [Id_ProyectoAcreditacionDocumento] INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto]                      INT            NOT NULL,
    [CodContacto]                      INT            NOT NULL,
    [CodConvocatoria]                  INT            NOT NULL,
    [Pendiente]                        BIT            CONSTRAINT [DF__ProyectoA__Pendi__0BFC99BF] DEFAULT ((0)) NOT NULL,
    [FechaPendiente]                   DATETIME       NULL,
    [ObservacionPendiente]             VARCHAR (2000) NULL,
    [Subsanado]                        BIT            CONSTRAINT [DF__ProyectoA__Subsa__0CF0BDF8] DEFAULT ((0)) NULL,
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
    [AsuntoNoAcreditado]               VARCHAR (150)  NULL,
    CONSTRAINT [PK_ProyectoAcreditacionDocumento] PRIMARY KEY CLUSTERED ([Id_ProyectoAcreditacionDocumento] ASC) WITH (FILLFACTOR = 50)
);

