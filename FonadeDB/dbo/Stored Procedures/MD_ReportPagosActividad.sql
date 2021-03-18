-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_ReportPagosActividad]
	-- Add the parameters for the stored procedure here
	@codGrupo int,
	@codUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @CONST_CoordinadorInterventor int = 13
	declare @CONST_GerenteInterventor int =12
	declare @CONST_EstadoFiduciaria int = 3
	declare @CONST_RolInterventorLider int = 8

    -- Insert statements for procedure here
    if @codGrupo = @CONST_CoordinadorInterventor
    begin
		SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial, PagoActividad.CantidadDinero 
 FROM PagoActividad 
 INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto 
WHERE     (PagoActividad.Estado = @CONST_EstadoFiduciaria)
order by PagoActividad.Id_PagoActividad
	end
	if @codGrupo =  @CONST_GerenteInterventor

	 begin
		SELECT PagoActividad.Id_PagoActividad, PagoActividad.FechaCoordinador, Empresa.razonsocial, PagoActividad.CantidadDinero
		FROM PagoActividad
		INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto
		INNER JOIN EmpresaInterventor ON Empresa.id_empresa = EmpresaInterventor.CodEmpresa
		INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto
		WHERE (PagoActividad.Estado > @CONST_EstadoFiduciaria)
		AND (Interventor.CodCoordinador = @codUsuario)
		AND (EmpresaInterventor.Rol = @CONST_RolInterventorLider)
		AND (EmpresaInterventor.Inactivo = 0)
end
	END