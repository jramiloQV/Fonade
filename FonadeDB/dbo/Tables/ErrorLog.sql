CREATE TABLE [dbo].[ErrorLog] (
    [ErrorID]     INT           IDENTITY (1, 1) NOT NULL,
    [Usuario]     VARCHAR (100) NULL,
    [Page]        VARCHAR (255) NULL,
    [Date]        SMALLDATETIME NULL,
    [Description] TEXT          NULL,
    [solved]      INT           NULL,
    [explanation] VARCHAR (512) NULL
);

