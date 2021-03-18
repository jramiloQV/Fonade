CREATE TABLE [dbo].[ProyectoActividadPOInterventorTMP_2] (
    [Id_Actividad]       INT            NOT NULL,
    [NomActividad]       VARCHAR (150)  NULL,
    [CodProyecto]        INT            NOT NULL,
    [Item]               SMALLINT       NULL,
    [Metas]              VARCHAR (8000) NULL,
    [ChequeoCoordinador] BIT            NULL,
    [Tarea]              VARCHAR (50)   NULL,
    [ChequeoGerente]     BIT            NULL
);

