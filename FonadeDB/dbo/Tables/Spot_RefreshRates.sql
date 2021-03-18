CREATE TABLE [dbo].[Spot_RefreshRates] (
    [CounterName]              VARCHAR (20)  NOT NULL,
    [DBName]                   VARCHAR (128) NULL,
    [MinSecondsBetweenRefresh] INT           NOT NULL,
    [LastRefreshTime]          DATETIME      NULL,
    [LastRefreshDuration1]     INT           NULL,
    [LastRefreshDuration2]     INT           NULL,
    [LastRefreshDuration3]     INT           NULL,
    [ExtraDataDecimal]         DECIMAL (13)  NULL,
    [ExtraDataDecimal2]        DECIMAL (13)  NULL,
    [ExtraDataTime]            DATETIME      NULL,
    [LastRefreshTime2]         DATETIME      NULL,
    [LastRefreshTime3]         DATETIME      NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_Spot_RefreshRates]
    ON [dbo].[Spot_RefreshRates]([CounterName] ASC, [DBName] ASC) WITH (FILLFACTOR = 50);

