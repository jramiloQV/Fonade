CREATE TABLE [dbo].[Spot_TableFragDetails] (
    [CollectDate]              DATETIME     NOT NULL,
    [DBName]                   [sysname]    NOT NULL,
    [TBName]                   [sysname]    NOT NULL,
    [IXName]                   [sysname]    NULL,
    [PagesScanned]             NUMERIC (18) NULL,
    [ExtentsScanned]           NUMERIC (18) NULL,
    [ExtentSwitches]           NUMERIC (18) NULL,
    [AvgPagesPerExtent]        NUMERIC (18) NULL,
    [ScanDensity]              NUMERIC (18) NULL,
    [LogicalScanFragmentation] NUMERIC (18) NULL,
    [ExtentScanFragmentation]  NUMERIC (18) NULL,
    [AvgBytesFreePerPage]      NUMERIC (18) NULL,
    [AvgPageDensity]           NUMERIC (18) NULL,
    [Owner]                    VARCHAR (80) NOT NULL,
    [indid]                    INT          NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_Spot_TableFragDetails]
    ON [dbo].[Spot_TableFragDetails]([DBName] ASC, [TBName] ASC, [IXName] ASC, [Owner] ASC) WITH (FILLFACTOR = 50);

