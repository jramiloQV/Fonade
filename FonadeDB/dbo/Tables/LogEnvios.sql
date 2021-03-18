CREATE TABLE [dbo].[LogEnvios] (
    [Id_Log]      INT           IDENTITY (1, 1) NOT NULL,
    [Fecha]       DATETIME      NOT NULL,
    [Asunto]      VARCHAR (255) NULL,
    [EnviadoPor]  VARCHAR (255) NULL,
    [EnviadoA]    VARCHAR (255) NULL,
    [Programa]    VARCHAR (255) NULL,
    [CodProyecto] INT           NULL,
    [Exitoso]     BIT           NULL,
    CONSTRAINT [PK_LogEnvios] PRIMARY KEY CLUSTERED ([Id_Log] ASC) WITH (FILLFACTOR = 50)
);

