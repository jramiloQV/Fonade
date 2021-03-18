CREATE TABLE [dbo].[FuenteFinanciacion] (
    [IdFuente]   INT           IDENTITY (1, 1) NOT NULL,
    [DescFuente] NVARCHAR (80) NOT NULL,
    CONSTRAINT [PK_FuenteFinanciacion] PRIMARY KEY CLUSTERED ([IdFuente] ASC)
);

