CREATE TABLE [dbo].[Bitacora] (
    [Id_Bitacora]       INT            IDENTITY (1, 1) NOT NULL,
    [FechaBitacora]     DATETIME       NOT NULL,
    [CodEventoBitacora] INT            NOT NULL,
    [Accion]            VARCHAR (4000) NULL,
    [CodContacto]       INT            NOT NULL,
    [IP]                VARCHAR (20)   NULL,
    CONSTRAINT [PK__Bitacora__106C4921] PRIMARY KEY CLUSTERED ([Id_Bitacora] ASC) WITH (FILLFACTOR = 50)
);


GO
CREATE NONCLUSTERED INDEX [Fecha_CodContacto_idx]
    ON [dbo].[Bitacora]([FechaBitacora] DESC, [CodContacto] ASC) WITH (FILLFACTOR = 50);

