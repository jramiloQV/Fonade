CREATE TABLE [dbo].[EmpresaContacto] (
    [id_empresacontacto] INT          IDENTITY (1, 1) NOT NULL,
    [codempresa]         INT          NULL,
    [codcontacto]        INT          NULL,
    [participacion]      NUMERIC (18) NULL,
    [RepresentanteLegal] INT          NULL,
    [Suplente]           INT          NULL,
    CONSTRAINT [PK_EmpresaContacto] PRIMARY KEY CLUSTERED ([id_empresacontacto] ASC) WITH (FILLFACTOR = 50)
);

