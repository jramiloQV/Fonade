CREATE TABLE [dbo].[InterventorRiesgo] (
    [Id_Riesgo]       INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto]     INT            NOT NULL,
    [Riesgo]          VARCHAR (500)  NOT NULL,
    [Mitigacion]      VARCHAR (500)  NOT NULL,
    [CodEjeFuncional] SMALLINT       NULL,
    [Observacion]     VARCHAR (1000) NULL,
    CONSTRAINT [PK_InterventorRiesgo] PRIMARY KEY CLUSTERED ([Id_Riesgo] ASC) WITH (FILLFACTOR = 50)
);

