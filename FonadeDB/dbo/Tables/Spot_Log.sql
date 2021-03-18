CREATE TABLE [dbo].[Spot_Log] (
    [id]              INT           IDENTITY (1, 1) NOT NULL,
    [MessageDateTime] DATETIME      NOT NULL,
    [MessageText]     VARCHAR (255) NOT NULL,
    [MessageNbr]      INT           NULL,
    [ProcName]        [sysname]     NULL,
    [spid]            INT           NULL,
    [CPU]             NUMERIC (18)  NULL,
    [IO]              NUMERIC (18)  NULL,
    [logintime]       DATETIME      NULL,
    [lastbatch]       DATETIME      NULL,
    [hostname]        [sysname]     NULL,
    [prog]            [sysname]     NULL,
    [nt_domain]       [sysname]     NULL,
    [nt_username]     [sysname]     NULL,
    [net_library]     [sysname]     NULL,
    [SQLLogin]        [sysname]     NULL,
    [SQLStatement]    VARCHAR (255) NULL,
    [OccurenceCount]  INT           NULL
);


GO
CREATE CLUSTERED INDEX [CL_Spot_Log]
    ON [dbo].[Spot_Log]([MessageDateTime] ASC) WITH (FILLFACTOR = 50);


GO
CREATE NONCLUSTERED INDEX [OC_Spot_Log]
    ON [dbo].[Spot_Log]([MessageText] ASC, [MessageNbr] ASC) WITH (FILLFACTOR = 50);


GO
CREATE UNIQUE NONCLUSTERED INDEX [PK_Spot_Log]
    ON [dbo].[Spot_Log]([id] ASC) WITH (FILLFACTOR = 50);

