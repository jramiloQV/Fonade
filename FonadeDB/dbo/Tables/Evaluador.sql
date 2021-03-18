CREATE TABLE [dbo].[Evaluador] (
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
    [fechaActualizacion]    DATETIME      DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [Evaluador_PK] PRIMARY KEY CLUSTERED ([CodContacto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [CK_Evaluador_Persona] CHECK ([Persona] = 'N' or [Persona] = 'J'),
    CONSTRAINT [FK_Evaluador_Banco] FOREIGN KEY ([CodBanco]) REFERENCES [dbo].[Banco] ([Id_Banco]),
    CONSTRAINT [FK_Evaluador_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_Evaluador_Coordinador] FOREIGN KEY ([CodCoordinador]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_Evaluador_TipoCuenta] FOREIGN KEY ([CodTipoCuenta]) REFERENCES [dbo].[TipoCuenta] ([Id_TipoCuenta])
);

