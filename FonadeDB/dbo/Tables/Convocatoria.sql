CREATE TABLE [dbo].[Convocatoria] (
    [Id_Convocatoria]   INT           IDENTITY (1, 1) NOT NULL,
    [NomConvocatoria]   VARCHAR (80)  NOT NULL,
    [Descripcion]       VARCHAR (500) NULL,
    [FechaInicio]       SMALLDATETIME NOT NULL,
    [FechaFin]          SMALLDATETIME NOT NULL,
    [Presupuesto]       FLOAT (53)    NOT NULL,
    [MinimoPorPlan]     FLOAT (53)    NOT NULL,
    [CodContacto]       INT           NOT NULL,
    [Publicado]         BIT           NULL,
    [encargofiduciario] VARCHAR (20)  NULL,
    [CodConvenio]       INT           NULL,
    [IdVersionProyecto] INT           NULL,
    CONSTRAINT [PK_Convocatoria] PRIMARY KEY CLUSTERED ([Id_Convocatoria] ASC) WITH (FILLFACTOR = 50),
    CONSTRAINT [FK_Convocatoria_Contacto] FOREIGN KEY ([CodContacto]) REFERENCES [dbo].[Contacto] ([Id_Contacto]),
    CONSTRAINT [FK_Convocatoria_VersionProyecto] FOREIGN KEY ([IdVersionProyecto]) REFERENCES [dbo].[VersionProyecto] ([IdVersionProyecto])
);




GO
CREATE TRIGGER tr_Convocatoria_U
ON Convocatoria
FOR UPDATE
AS
BEGIN
	DECLARE @FEC_D AS SMALLDATETIME,	@PRE_D AS FLOAT,
		@FEC_I AS SMALLDATETIME,	@PRE_I AS FLOAT
	
	SELECT @FEC_D = FechaFin, @PRE_D = Presupuesto	FROM DELETED
	SELECT @FEC_I = FechaFin, @PRE_I = Presupuesto	FROM inserted
	--columnmas 5 6 o 7 actulizadas
	IF @FEC_D <> @FEC_I OR @PRE_D <> @PRE_I 
	   INSERT INTO ConvocatoriaHistoria (CodConvocatoria, FechaFin, Presupuesto, CodContacto)
	   SELECT D.id_Convocatoria, D.FechaFin, D.Presupuesto, D.CodContacto FROM DELETED D
END