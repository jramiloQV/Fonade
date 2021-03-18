CREATE TABLE [dbo].[ReporteAuditoria] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Tabla]    VARCHAR (100) NOT NULL,
    [Campo]    VARCHAR (100) NOT NULL,
    [TipoDato] VARCHAR (50)  NULL,
    CONSTRAINT [PK_ReporteAuditoria] PRIMARY KEY CLUSTERED ([Id] ASC)
);

