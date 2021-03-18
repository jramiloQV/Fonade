CREATE TABLE [dbo].[Spot_VersionControl] (
    [Item]             VARCHAR (10) NOT NULL,
    [ExeVersion]       VARCHAR (30) NULL,
    [DBObjectsVersion] VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_Spot_VersionControl] PRIMARY KEY CLUSTERED ([Item] ASC) WITH (FILLFACTOR = 50)
);

