CREATE TABLE [dbo].[departamento] (
    [Id_Departamento] INT          IDENTITY (1, 1) NOT NULL,
    [NomDepartamento] VARCHAR (80) NOT NULL,
    [CodPais]         INT          NOT NULL,
    CONSTRAINT [PK_Departamemto] PRIMARY KEY CLUSTERED ([Id_Departamento] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Departamemto_Pais] FOREIGN KEY ([CodPais]) REFERENCES [dbo].[Pais] ([Id_Pais])
);

