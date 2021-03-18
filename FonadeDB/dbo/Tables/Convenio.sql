CREATE TABLE [dbo].[Convenio] (
    [Id_Convenio]           INT           IDENTITY (1, 1) NOT NULL,
    [Nomconvenio]           VARCHAR (50)  NULL,
    [Fechainicio]           DATETIME      NULL,
    [FechaFin]              DATETIME      NULL,
    [Descripcion]           VARCHAR (250) NULL,
    [CodcontactoFiduciaria] INT           NULL,
    CONSTRAINT [PK_Convenio] PRIMARY KEY CLUSTERED ([Id_Convenio] ASC)
);

