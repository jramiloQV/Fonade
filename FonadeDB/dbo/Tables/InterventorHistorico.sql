CREATE TABLE [dbo].[InterventorHistorico] (
    [CodContacto]             INT           NOT NULL,
    [CodBanco]                INT           NULL,
    [CodTipoCuenta]           TINYINT       NULL,
    [Cuenta]                  VARCHAR (20)  NULL,
    [MaximoPlanes]            TINYINT       NULL,
    [ExperienciaPrincipal]    VARCHAR (500) NULL,
    [Experienciasecundaria]   VARCHAR (500) NULL,
    [CodCoordinador]          INT           NULL,
    [numContrato]             INT           NULL,
    [FechaInicio]             SMALLDATETIME NULL,
    [FechaExpiracion]         SMALLDATETIME NULL,
    [Persona]                 VARCHAR (1)   NULL,
    [Celular]                 VARCHAR (20)  NULL,
    [Salario]                 MONEY         NULL,
    [fechaActualizacion]      DATETIME      NULL,
    [Id_InterventorHistorico] INT           IDENTITY (1, 1) NOT NULL,
    [fechaRegistro]           DATETIME      NULL,
    CONSTRAINT [PK_InterventorHistorico] PRIMARY KEY CLUSTERED ([Id_InterventorHistorico] ASC) WITH (FILLFACTOR = 50)
);

