CREATE TABLE [dbo].[TipoAprendiz] (
    [Id_TipoAprendiz] INT           IDENTITY (1, 1) NOT NULL,
    [NomTipoAprendiz] VARCHAR (500) NULL,
    [activo]          BIT           NULL,
    CONSTRAINT [PK_TipoAprendiz] PRIMARY KEY CLUSTERED ([Id_TipoAprendiz] ASC) WITH (FILLFACTOR = 50)
);

