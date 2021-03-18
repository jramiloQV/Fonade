CREATE TABLE [dbo].[Spot_Work_Locks] (
    [LockSpid]             INT           NULL,
    [dbid]                 INT           NULL,
    [objid]                INT           NULL,
    [indid]                INT           NULL,
    [ResourceType]         [sysname]     NULL,
    [RequestModeInt]       INT           NULL,
    [RequestStatusInt]     INT           NULL,
    [RequestStatus]        VARCHAR (50)  NULL,
    [Resource]             VARCHAR (255) NULL,
    [BlockedBlocking]      VARCHAR (50)  NULL,
    [BlockedBySpid]        INT           NULL,
    [BlockingLockModeInt]  INT           NULL,
    [BlockingResourceType] [sysname]     NULL,
    [id]                   INT           IDENTITY (1, 1) NOT NULL
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Spot_Work_Locks_idIdx]
    ON [dbo].[Spot_Work_Locks]([id] ASC) WITH (FILLFACTOR = 50);


GO
CREATE NONCLUSTERED INDEX [Spot_Work_Locks_LockSpidIdx]
    ON [dbo].[Spot_Work_Locks]([LockSpid] ASC) WITH (FILLFACTOR = 50);


GO
CREATE CLUSTERED INDEX [Spot_Work_Locks_ObjectIdx]
    ON [dbo].[Spot_Work_Locks]([objid] ASC, [indid] ASC, [dbid] ASC, [Resource] ASC) WITH (FILLFACTOR = 50);

