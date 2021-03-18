CREATE TABLE [dbo].[Spot_Work_Jobs] (
    [JobID]              UNIQUEIDENTIFIER NULL,
    [LastRunDate]        INT              NULL,
    [LastRunTime]        INT              NULL,
    [LastRecordedChange] DATETIME         NULL,
    [JobName]            [sysname]        NULL,
    [id]                 INT              IDENTITY (1, 1) NOT NULL,
    [CurrentStatus]      INT              NULL,
    [CurrentStepNbr]     INT              NULL,
    [LastRunDateTime]    DATETIME         NULL,
    [NextRunDateTime]    DATETIME         NULL,
    [NextRunDate]        INT              NULL,
    [NextRunTime]        INT              NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [Spot_Jobs_PK]
    ON [dbo].[Spot_Work_Jobs]([JobID] ASC, [id] ASC) WITH (FILLFACTOR = 50);

