CREATE TABLE [dbo].[InformeMedioTransporte] (
    [Id_InfMedioTransporte] INT   IDENTITY (1, 1) NOT NULL,
    [CodInforme]            INT   NOT NULL,
    [CodMedioTransporte]    INT   NOT NULL,
    [Valor]                 MONEY NOT NULL,
    CONSTRAINT [PK_InformeMedioTransporte] PRIMARY KEY CLUSTERED ([Id_InfMedioTransporte] ASC) WITH (FILLFACTOR = 50)
);

