CREATE TABLE [dbo].[tablaprueba] (
    [Id_Proyecto]         INT           NOT NULL,
    [Empresa]             VARCHAR (50)  NULL,
    [Ciudad]              INT           NOT NULL,
    [nomciudad]           VARCHAR (80)  NOT NULL,
    [Id_Departamento]     INT           NOT NULL,
    [Id_Sector]           INT           NOT NULL,
    [Sector]              VARCHAR (150) NOT NULL,
    [NomDepartamento]     VARCHAR (80)  NOT NULL,
    [empleos]             INT           NULL,
    [ValorRecomendado]    SMALLINT      NULL,
    [SalarioMinimo]       BIGINT        NOT NULL,
    [AñoSalario]          INT           NOT NULL,
    [inicio_convocatoria] SMALLDATETIME NOT NULL,
    [recursos]            BIGINT        NULL
);

