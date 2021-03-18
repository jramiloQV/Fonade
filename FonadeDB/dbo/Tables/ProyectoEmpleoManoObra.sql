CREATE TABLE [dbo].[ProyectoEmpleoManoObra] (
    [CodManoObra]       INT     NOT NULL,
    [SueldoMes]         MONEY   NOT NULL,
    [GeneradoPrimerAno] TINYINT NOT NULL,
    [Joven]             BIT     NOT NULL,
    [Desplazado]        BIT     NOT NULL,
    [Madre]             BIT     NOT NULL,
    [Minoria]           BIT     NOT NULL,
    [Recluido]          BIT     NOT NULL,
    [Desmovilizado]     BIT     NOT NULL,
    [Discapacitado]     BIT     NOT NULL,
    [Desvinculado]      BIT     NOT NULL,
    CONSTRAINT [PK_ProyectoEmpleoManoObra] PRIMARY KEY CLUSTERED ([CodManoObra] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_EmpleoManoObra_ProyectoInsumo] FOREIGN KEY ([CodManoObra]) REFERENCES [dbo].[ProyectoInsumo] ([Id_Insumo])
);

