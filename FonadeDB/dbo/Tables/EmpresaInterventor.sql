CREATE TABLE [dbo].[EmpresaInterventor] (
    [Id_EmpresaInterventor] INT      IDENTITY (1, 1) NOT NULL,
    [CodEmpresa]            INT      NULL,
    [CodContacto]           INT      NULL,
    [Rol]                   INT      NULL,
    [FechaInicio]           DATETIME NULL,
    [FechaFin]              DATETIME NULL,
    [Inactivo]              BIT      NULL,
    CONSTRAINT [PK_EmpresaInterventor] PRIMARY KEY CLUSTERED ([Id_EmpresaInterventor] ASC) WITH (FILLFACTOR = 50)
);

