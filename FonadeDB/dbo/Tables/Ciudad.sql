CREATE TABLE [dbo].[Ciudad] (
    [Id_Ciudad]       INT          NOT NULL,
    [NomCiudad]       VARCHAR (80) NOT NULL,
    [CodDepartamento] INT          NOT NULL,
    [IDH]             FLOAT (53)   NULL,
    [CodFiduciaria]   INT          NULL,
    [CodigoDANE]      VARCHAR (50) NULL,
    CONSTRAINT [PK_Ciudad] PRIMARY KEY CLUSTERED ([Id_Ciudad] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Ciudad_Departamemto] FOREIGN KEY ([CodDepartamento]) REFERENCES [dbo].[departamento] ([Id_Departamento])
);

