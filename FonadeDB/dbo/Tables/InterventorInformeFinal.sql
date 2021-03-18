CREATE TABLE [dbo].[InterventorInformeFinal] (
    [Id_InterventorInformeFinal] INT           IDENTITY (1, 1) NOT NULL,
    [NomInterventorInformeFinal] VARCHAR (255) NULL,
    [CodInterventor]             INT           NULL,
    [FechaInforme]               DATETIME      NULL,
    [Estado]                     TINYINT       NULL,
    [CodEmpresa]                 INT           NULL,
    [FechaEnvio]                 DATETIME      NULL,
    CONSTRAINT [PK_InterventorInformeFinal] PRIMARY KEY CLUSTERED ([Id_InterventorInformeFinal] ASC) WITH (FILLFACTOR = 50)
);

