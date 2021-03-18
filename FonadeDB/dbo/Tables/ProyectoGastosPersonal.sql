CREATE TABLE [dbo].[ProyectoGastosPersonal] (
    [Id_Cargo]              INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]           INT           NOT NULL,
    [Cargo]                 VARCHAR (100) NOT NULL,
    [TipoContratacion]      VARCHAR (30)  NOT NULL,
    [ValorMensual]          MONEY         NOT NULL,
    [ValorAnual]            MONEY         NOT NULL,
    [OtrosGastos]           MONEY         NOT NULL,
    [Dedicacion]            VARCHAR (50)  NULL,
    [Observacion]           VARCHAR (400) NULL,
    [Funciones]             VARCHAR (250) NULL,
    [Formacion]             VARCHAR (250) NULL,
    [ExperienciaGeneral]    VARCHAR (100) NULL,
    [ExperienciaEspecifica] VARCHAR (100) NULL,
    [UnidadTiempo]          INT           NULL,
    [TiempoVinculacion]     INT           NULL,
    [ValorRemuneracion]     MONEY         NULL,
    [ValorFondoEmprender]   MONEY         NULL,
    [AportesEmprendedor]    MONEY         NULL,
    [IngresosVentas]        MONEY         NULL,
    CONSTRAINT [PK_ProyectoGastosPersonal] PRIMARY KEY CLUSTERED ([Id_Cargo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_ProyectoGastosPersonal_Proyecto] FOREIGN KEY ([CodProyecto]) REFERENCES [dbo].[Proyecto] ([Id_Proyecto]) ON UPDATE CASCADE
);



