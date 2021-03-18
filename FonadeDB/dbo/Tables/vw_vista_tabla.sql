CREATE TABLE [dbo].[vw_vista_tabla] (
    [Id_Proyecto]     INT           NOT NULL,
    [Ciudad]          INT           NOT NULL,
    [Empresa]         VARCHAR (50)  NULL,
    [Id_Departamento] INT           NOT NULL,
    [Id_Sector]       INT           NOT NULL,
    [nomciudad]       VARCHAR (80)  NOT NULL,
    [NomDepartamento] VARCHAR (80)  NOT NULL,
    [recursos]        FLOAT (53)    NULL,
    [Sector]          VARCHAR (150) NOT NULL,
    [Empleos]         INT           NULL
);

