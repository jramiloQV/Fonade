CREATE TABLE [dbo].[Spot_Work3] (
    [spid]     INT            CONSTRAINT [DF_Spot_Work3_spid] DEFAULT (@@spid) NULL,
    [Sysname1] [sysname]      NULL,
    [Int1]     INT            NULL,
    [Time1]    DATETIME       NULL,
    [Char255]  VARCHAR (255)  NULL,
    [Numeric1] NUMERIC (18)   NULL,
    [Char20a]  VARCHAR (20)   NULL,
    [Char20b]  VARCHAR (20)   NULL,
    [Numeric2] NUMERIC (18)   NULL,
    [Numeric3] NUMERIC (18)   NULL,
    [Char20c]  VARCHAR (20)   NULL,
    [Int2]     INT            NULL,
    [Sysname2] [sysname]      NULL,
    [Sysname3] [sysname]      NULL,
    [Int3]     INT            NULL,
    [Char20d]  VARCHAR (20)   NULL,
    [Sysname4] [sysname]      NULL,
    [Binary85] VARBINARY (85) NULL
);


GO
CREATE CLUSTERED INDEX [PK_Spot_Work3]
    ON [dbo].[Spot_Work3]([Sysname1] ASC) WITH (FILLFACTOR = 50);

