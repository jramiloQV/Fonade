CREATE TABLE [dbo].[Spot_Work2] (
    [spid]       INT             CONSTRAINT [DF_Spot_Work2_spid] DEFAULT (@@spid) NULL,
    [Int1]       INT             NULL,
    [Int2]       INT             NULL,
    [Int3]       INT             NULL,
    [Char50]     VARCHAR (50)    NULL,
    [Char255]    VARCHAR (255)   NULL,
    [id]         INT             IDENTITY (1, 1) NOT NULL,
    [Sysname1]   [sysname]       NULL,
    [Int4]       INT             NULL,
    [Float2]     FLOAT (53)      NULL,
    [Float1]     FLOAT (53)      NULL,
    [Time1]      DATETIME        NULL,
    [Numeric1]   NUMERIC (18)    NULL,
    [Numeric2]   NUMERIC (18)    NULL,
    [VarBinary1] VARBINARY (100) NULL,
    [NText1]     NTEXT           NULL,
    [Float3]     FLOAT (53)      NULL,
    [Char20]     VARCHAR (20)    NULL,
    [Binary85]   VARBINARY (85)  NULL
);


GO
CREATE NONCLUSTERED INDEX [Spot_Work2_Int1_spid]
    ON [dbo].[Spot_Work2]([Int1] ASC, [spid] ASC) WITH (FILLFACTOR = 50);


GO
CREATE CLUSTERED INDEX [Spot_Work2_Spid_Idx]
    ON [dbo].[Spot_Work2]([spid] ASC, [Int1] ASC) WITH (FILLFACTOR = 50);

