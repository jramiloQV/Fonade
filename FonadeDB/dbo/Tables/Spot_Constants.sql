CREATE TABLE [dbo].[Spot_Constants] (
    [ConstantName]  [sysname] NOT NULL,
    [ConstantValue] [sysname] NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_Spot_Constants]
    ON [dbo].[Spot_Constants]([ConstantName] ASC) WITH (FILLFACTOR = 50);

