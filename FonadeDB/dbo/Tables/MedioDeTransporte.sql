CREATE TABLE [dbo].[MedioDeTransporte] (
    [Id_MedioDeTransporte] INT          IDENTITY (1, 1) NOT NULL,
    [NomMedioDeTransporte] VARCHAR (50) NULL,
    CONSTRAINT [PK_MedioDeTransporte] PRIMARY KEY CLUSTERED ([Id_MedioDeTransporte] ASC) WITH (FILLFACTOR = 50)
);

