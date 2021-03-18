CREATE TABLE [dbo].[PosicionArancelaria] (
    [PosicionArancelaria] CHAR (10)     NOT NULL,
    [Año]                 CHAR (4)      NOT NULL,
    [Cuode]               CHAR (4)      NOT NULL,
    [CIIU]                CHAR (4)      NOT NULL,
    [Descripcion]         VARCHAR (800) NOT NULL,
    [Decreto]             CHAR (4)      NULL,
    [Fecha_Ex]            SMALLDATETIME NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_PosicionArancelaria]
    ON [dbo].[PosicionArancelaria]([PosicionArancelaria] ASC) WITH (FILLFACTOR = 50);


GO
CREATE NONCLUSTERED INDEX [IX_PosicionArancelaria_1]
    ON [dbo].[PosicionArancelaria]([Descripcion] ASC) WITH (FILLFACTOR = 50);

