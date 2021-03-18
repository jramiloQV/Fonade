CREATE TABLE [dbo].[EjeFuncional] (
    [Id_EjeFuncional] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [NomEjeFuncional] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_EjeFuncional] PRIMARY KEY CLUSTERED ([Id_EjeFuncional] ASC) WITH (FILLFACTOR = 50)
);

