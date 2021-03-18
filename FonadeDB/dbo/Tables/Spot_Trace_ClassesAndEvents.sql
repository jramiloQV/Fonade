CREATE TABLE [dbo].[Spot_Trace_ClassesAndEvents] (
    [Type]                  VARCHAR (15) NOT NULL,
    [EventID]               SMALLINT     NULL,
    [ColumnID]              SMALLINT     NULL,
    [EventName]             VARCHAR (30) NULL,
    [ColumnName]            VARCHAR (30) NULL,
    [EnableForGlobalTrace]  SMALLINT     NULL,
    [EnableForSessionTrace] SMALLINT     NULL,
    [id]                    INT          IDENTITY (1, 1) NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_Spot_Trace_ClassesAndEvents]
    ON [dbo].[Spot_Trace_ClassesAndEvents]([Type] ASC, [EventID] ASC, [ColumnID] ASC) WITH (FILLFACTOR = 50);


GO
CREATE UNIQUE NONCLUSTERED INDEX [Spot_ClassesAndEvents_ID]
    ON [dbo].[Spot_Trace_ClassesAndEvents]([id] ASC) WITH (FILLFACTOR = 50);

