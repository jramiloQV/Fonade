-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_BuscarReportePlanesEnAcreditacion]
	-- Add the parameters for the stored procedure here
	@_ConvocatoriaID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
			SELECT     CO.Id_Convocatoria as CodConvocatoria, CO.NomConvocatoria, PR.Id_Proyecto as CodProyecto, PR.NomProyecto
		,PC.Acreditador, C.Nombres AS NombreAcreditador, C.Apellidos AS ApellidosAcreditado
		,PA.Fecha as FechaAcreditacion, PA.ObservacionFinal, PA.CodEstado AS Cod_DeEstado, EM.Nombres AS NombreEmprendedor
		,EM.Apellidos AS ApellidosEmprendedor, I.NomInstitucion, I.NomUnidad, CI.NomCiudad
		,DE.NomDepartamento, SE.NomSector, SUB.NomSubSector, PAD.Pendiente, PAD.FechaPendiente
		,PAD.ObservacionPendiente, PAD.Subsanado, PAD.ObservacionSubsanado, PAD.FechaSubsanado
		,PAD.Acreditado, PAD.ObservacionAcreditado, PAD.FechaAcreditado, PAD.NoAcreditado
		,PAD.ObservacionNoAcreditado, PAD.FechaNoAcreditado, PFI.Recursos, PR.CodEstado, E.NomEstado
		,TBL.FechaActa as Fecha
		FROM        Proyecto AS PR  
		INNER JOIN ConvocatoriaProyecto as CP ON CP.CodProyecto = PR.Id_Proyecto  
		INNER JOIN Convocatoria AS CO ON CO.Id_Convocatoria = CP.CodConvocatoria
		left OUTER JOIN Estado AS E ON PR.CodEstado = E.Id_Estado 
		LEFT OUTER JOIN SubSector AS SUB ON PR.CodSubSector = SUB.Id_SubSector
		LEFT OUTER JOIN Sector AS SE ON SUB.CodSector = SE.Id_Sector  
		LEFT OUTER JOIN Institucion AS I ON PR.CodInstitucion = I.Id_Institucion 
		LEFT OUTER JOIN ProyectoFinanzasIngresos AS PFI ON PR.Id_Proyecto = PFI.CodProyecto 
		LEFT OUTER JOIN Ciudad AS CI ON PR.CodCiudad = CI.Id_Ciudad 
		LEFT OUTER JOIN departamento AS DE ON CI.CodDepartamento = DE.Id_Departamento 
		LEFT OUTER JOIN ProyectoContacto AS PC ON PR.Id_Proyecto = PC.CodProyecto and PC.Acreditador = 1 and PC.Inactivo = 0 and PC.FechaFin is null
		LEFT OUTER JOIN Contacto AS C ON PC.CodContacto = C.Id_Contacto  
		LEFT OUTER JOIN ProyectoAcreditacionDocumento AS PAD ON PR.Id_Proyecto =  PAD.CodProyecto
		LEFT OUTER JOIN ProyectoAcreditacionUltimo as PA ON PAD.CodProyecto = PA.CodProyecto and PAD.CodConvocatoria = PA.CodConvocatoria
		LEFT OUTER JOIN Contacto AS EM ON PAD.CodContacto = EM.Id_Contacto
		LEFT OUTER JOIN (SELECT     AA.*, AAP.CodProyecto
		FROM         AcreditacionActaProyecto AS AAP INNER JOIN
		AcreditacionActa AS AA ON AAP.CodActa = AA.Id_Acta
		WHERE     (AA.CodConvocatoria = @_ConvocatoriaID))TBL ON PR.Id_Proyecto = TBL.CodProyecto
		where  CO.Id_Convocatoria = @_ConvocatoriaID
		ORDER BY CodProyecto
END