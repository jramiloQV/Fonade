CREATE TABLE [dbo].[InstitucionEducativa] (
    [Id_InstitucionEducativa] INT           NOT NULL,
    [NomInstitucionEducativa] VARCHAR (250) NOT NULL,
    CONSTRAINT [PK_INSTITUCIONEDUCATIVA] PRIMARY KEY CLUSTERED ([Id_InstitucionEducativa] ASC) WITH (FILLFACTOR = 50)
);

