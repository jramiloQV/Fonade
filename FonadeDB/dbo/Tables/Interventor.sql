CREATE TABLE [dbo].[Interventor] (
    [CodContacto]           INT           NOT NULL,
    [CodBanco]              INT           NULL,
    [CodTipoCuenta]         TINYINT       NULL,
    [Cuenta]                VARCHAR (20)  NULL,
    [MaximoPlanes]          TINYINT       NULL,
    [ExperienciaPrincipal]  VARCHAR (500) NULL,
    [Experienciasecundaria] VARCHAR (500) NULL,
    [CodCoordinador]        INT           NULL,
    [numContrato]           INT           NULL,
    [FechaInicio]           SMALLDATETIME NULL,
    [FechaExpiracion]       SMALLDATETIME NULL,
    [Persona]               VARCHAR (1)   NULL,
    [Celular]               VARCHAR (20)  NULL,
    [Salario]               MONEY         NULL,
    [fechaActualizacion]    DATETIME      DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Interventor] PRIMARY KEY CLUSTERED ([CodContacto] ASC) WITH (FILLFACTOR = 50)
);

