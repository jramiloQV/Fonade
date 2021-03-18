CREATE TABLE [dbo].[InterventorRiesgoTMP] (
    [IdTmp]              INT            IDENTITY (1, 1) NOT NULL,
    [Id_Riesgo]          INT            NOT NULL,
    [CodProyecto]        INT            NOT NULL,
    [Riesgo]             VARCHAR (500)  NOT NULL,
    [Mitigacion]         VARCHAR (500)  NULL,
    [CodEjeFuncional]    SMALLINT       NULL,
    [Observacion]        VARCHAR (1000) NULL,
    [ChequeoCoordinador] BIT            NULL,
    [Tarea]              VARCHAR (50)   CONSTRAINT [DF_InterventorRiesgoTMP_Tarea] DEFAULT ('Adicionar') NOT NULL,
    [ChequeoGerente]     BIT            NULL,
    CONSTRAINT [PK_InterventorRiesgoTMP] PRIMARY KEY CLUSTERED ([IdTmp] ASC)
);

