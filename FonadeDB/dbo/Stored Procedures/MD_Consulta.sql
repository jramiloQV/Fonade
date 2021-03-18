-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE MD_Consulta
	-- Add the parameters for the stored procedure here
	@Cod_grupo int,	@txtpalabra varchar(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
Declare @GerenteAdministrador int = 1
Declare @AdministradorFonade int = 2
Declare @AdministradorSena int = 3
Declare @JefeUnidad int = 4
Declare @Asesor  int = 5
Declare @Emprendedor int = 6
Declare @CallCenter int = 8
Declare @GerenteEvaluador int = 9
Declare @CoordinadorEvaluador int = 10
Declare @Evaluador int = 11
Declare @GerenteInterventor int = 12
Declare @CoordinadorInterventor int = 13
Declare @Interventor int = 14
Declare @PerfilFiduciaria int = 15
Declare @RolInterventorLider int = 8
    -- Insert statements for procedure here
	SELECT P.Id_Proyecto, P.NomProyecto, T.NomTipoIdentificacion, C.Identificacion, C.Id_contacto, C.Nombres, C.Apellidos, C.Email, R.Id_Rol, R.Nombre, nomUnidad+' ('+nomInstitucion+')' nomInstitucion, nomTipoInstitucion
	         ,  (Select Max(CodConvocatoria)  from ConvocatoriaProyecto where CodProyecto = P.Id_Proyecto) as CodConvocatoria
			 FROM Contacto C, TipoIdentificacion T, Proyecto P, ProyectoContacto PC, Rol R, Institucion I, TipoInstitucion TI
			 WHERE C.CodTipoIdentificacion = T.Id_TipoIdentificacion			
			 AND PC.CodContacto = C.Id_Contacto
			 AND C.CodInstitucion = I.Id_Institucion
			 AND I.CodTipoInstitucion = TI.Id_TipoInstitucion
			 AND PC.Inactivo = 0 AND P.inactivo=0 
			 AND P.Id_Proyecto = PC.CodProyecto
			 AND PC.CodRol = R.Id_Rol AND PC.codRol in (@Cod_grupo)
			 AND (C.Nombres LIKE '%'+@txtpalabra+'%' OR C.Apellidos LIKE '%'+@txtpalabra+'%')
END