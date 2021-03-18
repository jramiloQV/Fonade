CREATE TABLE [dbo].[dim_tiempo] (
    [ano]          INT           NULL,
    [mes]          INT           NULL,
    [anomes]       VARCHAR (60)  NULL,
    [mes_nombre]   NVARCHAR (30) NULL,
    [trim_nombre]  NVARCHAR (40) NULL,
    [sem_nombre]   VARCHAR (10)  NOT NULL,
    [año_semestre] VARCHAR (41)  NULL,
    [añoTrimestre] NVARCHAR (71) NULL
);

