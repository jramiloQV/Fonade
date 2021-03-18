CREATE TABLE [dbo].[Spot_SuppressedFeatures] (
    [Feature]            VARCHAR (20) NOT NULL,
    [spid]               INT          NULL,
    [Suppressed]         CHAR (1)     NULL,
    [LastUpdateDateTime] DATETIME     NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [IX_Spot_SuppressedFeatures]
    ON [dbo].[Spot_SuppressedFeatures]([Feature] ASC, [spid] ASC) WITH (FILLFACTOR = 50);

