-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [MD_Fiduciaria]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from (
	SELECT Empresa.Id_Empresa, PagoActividad.Id_PagoActividad, Empresa.razonsocial, PagoActividad.FechaInterventor AS Fecha,
			PagoActividad.CantidadDinero AS Valor, PagoBeneficiario.NumIdentificacion, Empresa.codproyecto,codactafonade, PagoActividad.ObservaInterventor,
			( select top 1 CodContactoFiduciaria from convocatoria, Convocatoriaproyecto ,convenio where id_convocatoria=codconvocatoria and id_convenio=codconvenio
			and id_convocatoria in(select max(CodConvocatoria) as CodConvocatoria from Convocatoriaproyecto where viable=1 and Codproyecto=Empresa.codproyecto)
			and  Codproyecto=Empresa.codproyecto ) as codcontactofiduciaria
	FROM PagoActividad
	INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto
	INNER JOIN PagoBeneficiario ON PagoActividad.CodPagoBeneficiario = PagoBeneficiario.Id_PagoBeneficiario
	left join (
		SELECT distinct PagoActividad.Id_PagoActividad as tablaId_PagoActividad,codactafonade
		FROM PagoActividad
			INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto
			INNER JOIN PagoBeneficiario ON PagoActividad.CodPagoBeneficiario = PagoBeneficiario.Id_PagoBeneficiario
			inner join PagosActaSolicitudPagos on (PagoActividad.id_PagoActividad = PagosActaSolicitudPagos.codPagoActividad)
			inner join PagosActaSolicitudes on (PagosActaSolicitudPagos.codPagosActaSolicitudes=PagosActaSolicitudes.id_acta and PagosActaSolicitudes.tipo='Fiduciaria')
			WHERE PagoActividad.Estado = 2) as tabla on (tablaId_PagoActividad=id_PagoActividad)
	WHERE PagoActividad.Estado = 2
) as tabla
END