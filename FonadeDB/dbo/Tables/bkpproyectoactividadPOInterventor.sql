CREATE TABLE [dbo].[bkpproyectoactividadPOInterventor] (
    [Id_Actividad] INT            IDENTITY (1, 1) NOT NULL,
    [NomActividad] VARCHAR (150)  NOT NULL,
    [CodProyecto]  INT            NOT NULL,
    [Item]         SMALLINT       NOT NULL,
    [Metas]        VARCHAR (8000) NULL
);

