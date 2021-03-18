CREATE TABLE [dbo].[ProyectoEmpleoCargo] (
    [CodCargo]          INT     NOT NULL,
    [GeneradoPrimerAno] TINYINT NOT NULL,
    [Joven]             BIT     NOT NULL,
    [Desplazado]        BIT     NOT NULL,
    [Madre]             BIT     NOT NULL,
    [Minoria]           BIT     NOT NULL,
    [Recluido]          BIT     NOT NULL,
    [Desmovilizado]     BIT     NOT NULL,
    [Discapacitado]     BIT     NOT NULL,
    [Desvinculado]      BIT     NOT NULL,
    CONSTRAINT [PK_ProyectoEmpleoCargo] PRIMARY KEY CLUSTERED ([CodCargo] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_CargoEmpleo_ProyectoGastosPersonal] FOREIGN KEY ([CodCargo]) REFERENCES [dbo].[ProyectoGastosPersonal] ([Id_Cargo])
);

