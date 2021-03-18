CREATE TABLE [dbo].[vw_tabla_total] (
    [Id_Proyecto]     INT           NOT NULL,
    [Empresa]         VARCHAR (50)  NULL,
    [Ciudad]          INT           NOT NULL,
    [nomciudad]       VARCHAR (80)  NOT NULL,
    [Id_Departamento] INT           NOT NULL,
    [Id_Sector]       INT           NOT NULL,
    [Sector]          VARCHAR (150) NOT NULL,
    [NomDepartamento] VARCHAR (80)  NOT NULL,
    [empleos]         INT           NULL,
    [recursos]        BIGINT        NULL
);

