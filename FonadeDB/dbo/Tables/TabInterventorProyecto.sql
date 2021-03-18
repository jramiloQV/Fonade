CREATE TABLE [dbo].[TabInterventorProyecto] (
    [CodTabInterventor] SMALLINT NOT NULL,
    [CodProyecto]       INT      NOT NULL,
    [CodContacto]       INT      NOT NULL,
    [FechaModificacion] DATETIME NOT NULL,
    [Realizado]         BIT      NULL
);

