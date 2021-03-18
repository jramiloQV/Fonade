CREATE TABLE [dbo].[EvaluacionContacto] (
    [CodProyecto]        INT           NOT NULL,
    [CodConvocatoria]    INT           NOT NULL,
    [CodContacto]        INT           NOT NULL,
    [AporteDinero]       FLOAT (53)    NOT NULL,
    [AporteEspecie]      FLOAT (53)    NOT NULL,
    [DetalleEspecie]     VARCHAR (300) NOT NULL,
    [Entidades]          VARCHAR (300) NOT NULL,
    [PeorCalificacion]   CHAR (3)      NULL,
    [CuentasCorrientes]  INT           NULL,
    [ValorCartera]       FLOAT (53)    NOT NULL,
    [ValorOtrasCarteras] FLOAT (53)    NOT NULL,
    CONSTRAINT [PK_EvaluacionContacto] PRIMARY KEY CLUSTERED ([CodProyecto] ASC, [CodConvocatoria] ASC, [CodContacto] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EvaluacionContacto_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_EvaluacionContacto_Convocatoria] FOREIGN KEY ([CodConvocatoria]) REFERENCES [dbo].[Convocatoria] ([Id_Convocatoria]),
    CONSTRAINT [FK_EvaluacionContacto_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto])
);

