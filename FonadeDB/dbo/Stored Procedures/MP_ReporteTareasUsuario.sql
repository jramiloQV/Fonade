-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE MP_ReporteTareasUsuario
	-- Add the parameters for the stored procedure here
	@Id_TareaUsuarioRepeticion int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  tu.Id_TareaUsuario, tur.Id_TareaUsuarioRepeticion, Id_Proyecto, NomProyecto, tu.NomTareaUsuario, tu.Descripcion, tu.CodTareaPrograma, tu.RecordatorioEmail, tu.NivelUrgencia,
	tu.RecordatorioPantalla, tu.RequiereRespuesta, tu.CodContactoAgendo, tu.DocumentoRelacionado, u2.Nombres +' '+u2.Apellidos as NomUsuario, tur.Fecha, convert(char(12),Fecha,107) AS SoloFecha, convert(char(8),Fecha,108) AS SoloHora, tur.Parametros, tp.NomTareaPrograma, tp.Ejecutable, tp.Icono, u.Nombres +' '+u.Apellidos AS NomUsuarioAgendo,
	tur.FechaCierre, tur.Respuesta
	FROM TareaPrograma tp, TareaUsuarioRepeticion tur, Contacto u, Contacto u2, Proyecto right outer join TareaUsuario tu
	on Id_Proyecto=tu.CodProyecto WHERE tu.CodContacto = u2.Id_Contacto AND tu.CodContactoAgendo = u.Id_Contacto AND tur.CodTareaUsuario = tu.Id_TareaUsuario AND Id_TareaPrograma = CodTareaPrograma AND Id_TareaUsuarioRepeticion=@Id_TareaUsuarioRepeticion
END