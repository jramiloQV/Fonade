CREATE TABLE [dbo].[InterventorInformeFinal_2Nov2010] (
    [Id_InterventorInformeFinal] INT           IDENTITY (1, 1) NOT NULL,
    [NomInterventorInformeFinal] VARCHAR (255) NULL,
    [CodInterventor]             INT           NULL,
    [FechaInforme]               DATETIME      NULL,
    [Estado]                     TINYINT       NULL,
    [CodEmpresa]                 INT           NULL,
    [FechaEnvio]                 DATETIME      NULL
);

