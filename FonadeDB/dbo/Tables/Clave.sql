CREATE TABLE [dbo].[Clave] (
    [Id_Clave]            INT           IDENTITY (1, 1) NOT NULL,
    [NomClave]            VARCHAR (50)  NOT NULL,
    [FechaVigencia]       SMALLDATETIME NULL,
    [FechaDebeActualizar] SMALLDATETIME NULL,
    [DebeCambiar]         INT           NULL,
    [Estado]              INT           NULL,
    [YaAvisoExpiracion]   INT           NULL,
    [codContacto]         INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id_Clave] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK__Clave__codContac__0E8400AF] FOREIGN KEY ([codContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto])
);

