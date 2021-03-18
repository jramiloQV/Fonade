CREATE TABLE [dbo].[DesempenoApp] (
    [Id_DesempenoApp] INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]           DATETIME      NULL,
    [Scriptname]      VARCHAR (255) NULL,
    [Segundos]        FLOAT (53)    NULL,
    CONSTRAINT [PK_DesempenoApp] PRIMARY KEY CLUSTERED ([Id_DesempenoApp] ASC) WITH (FILLFACTOR = 50)
);

