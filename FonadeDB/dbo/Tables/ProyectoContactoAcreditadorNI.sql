CREATE TABLE [dbo].[ProyectoContactoAcreditadorNI] (
    [Id_ProyectoContacto] INT           IDENTITY (1, 1) NOT NULL,
    [CodProyecto]         INT           NOT NULL,
    [CodContacto]         INT           NOT NULL,
    [CodRol]              TINYINT       NOT NULL,
    [FechaInicio]         SMALLDATETIME NOT NULL,
    [FechaFin]            SMALLDATETIME NULL,
    [Inactivo]            BIT           NOT NULL,
    [Beneficiario]        BIT           NULL,
    [Participacion]       FLOAT (53)    NULL,
    [CodConvocatoria]     INT           NULL,
    [HorasProyecto]       INT           NULL,
    [Acreditador]         BIT           NULL
);

