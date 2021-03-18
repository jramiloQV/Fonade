CREATE TABLE [dbo].[ProyectoActividadPOMesInterventorTMP] (
    [CodActividad]        INT     NOT NULL,
    [Mes]                 TINYINT NULL,
    [CodTipoFinanciacion] TINYINT NULL,
    [Valor]               MONEY   NULL,
    [Id]                  INT     IDENTITY (1, 1) NOT NULL
);

