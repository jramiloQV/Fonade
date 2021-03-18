CREATE TABLE [dbo].[EvaluadorHistorico] (
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
    [fechaActualizacion]    DATETIME      NULL,
    [Id_EvaluadorHistorico] INT           IDENTITY (1, 1) NOT NULL,
    [fechaRegistro]         DATETIME      NULL,
    CONSTRAINT [EvaluadorHistorico_PK] PRIMARY KEY CLUSTERED ([Id_EvaluadorHistorico] ASC) WITH (FILLFACTOR = 50)
);

