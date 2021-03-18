CREATE TABLE [dbo].[ProyectoActividadPOInterventorTMP] (
    [Id_Actividad]       INT            NOT NULL,
    [NomActividad]       VARCHAR (150)  NULL,
    [CodProyecto]        INT            NOT NULL,
    [Item]               SMALLINT       NULL,
    [Metas]              VARCHAR (8000) NULL,
    [ChequeoCoordinador] BIT            NULL,
    [Tarea]              VARCHAR (50)   CONSTRAINT [DF_ProyectoActividadPOInterventorTMP_Tarea] DEFAULT ('Adicionar') NULL,
    [ChequeoGerente]     BIT            NULL
);

