CREATE TABLE [dbo].[TareaPrograma] (
    [Id_TareaPrograma] INT           IDENTITY (1, 1) NOT NULL,
    [NomTareaPrograma] VARCHAR (100) NOT NULL,
    [Ejecutable]       VARCHAR (255) NOT NULL,
    [Icono]            VARCHAR (100) NULL,
    [Orden]            SMALLINT      NOT NULL,
    [delSistema]       SMALLINT      NULL,
    CONSTRAINT [PK_TareaPrograma] PRIMARY KEY CLUSTERED ([Id_TareaPrograma] ASC) WITH (FILLFACTOR = 50)
);

