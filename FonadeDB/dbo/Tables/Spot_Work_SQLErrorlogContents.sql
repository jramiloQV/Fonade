CREATE TABLE [dbo].[Spot_Work_SQLErrorlogContents] (
    [MessageDateTime] DATETIME       NULL,
    [SPIDMsg]         VARCHAR (20)   NULL,
    [MessageText]     VARCHAR (1000) NULL,
    [ContinuationRow] INT            NULL,
    [id]              INT            IDENTITY (1, 1) NOT NULL,
    [ErrorNbr]        INT            CONSTRAINT [DF_Spot_Work_SQLErrorlogContents_ErrorNbr] DEFAULT (0) NULL,
    [SeverityNbr]     INT            CONSTRAINT [DF_Spot_Work_SQLErrorlogContents_SeverityNbr] DEFAULT (0) NULL,
    [StateNbr]        INT            CONSTRAINT [DF_Spot_Work_SQLErrorlogContents_StateNbr] DEFAULT (0) NULL,
    [ErrorLogNumber]  SMALLINT       NULL,
    [CollectDateTime] DATETIME       CONSTRAINT [DF_Spot_Work_SQLErrorlogContents_CollectDateTime] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_ErrorlogContents] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 50)
);

