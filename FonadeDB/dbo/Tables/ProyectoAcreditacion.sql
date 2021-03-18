CREATE TABLE [dbo].[ProyectoAcreditacion] (
    [Id_ProyectoAcreditacion] INT            IDENTITY (1, 1) NOT NULL,
    [CodProyecto]             INT            NOT NULL,
    [CodConvocatoria]         INT            NOT NULL,
    [ObservacionFinal]        VARCHAR (1000) NOT NULL,
    [Fecha]                   DATETIME       NOT NULL,
    [CodEstado]               INT            NOT NULL,
    CONSTRAINT [PK_ProyectoAcreditacion] PRIMARY KEY CLUSTERED ([Id_ProyectoAcreditacion] ASC) WITH (FILLFACTOR = 50)
);

