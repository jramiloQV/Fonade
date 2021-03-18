
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarActividadesPendientes]
	@Email nvarchar(250) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 200 tu.Id_TareaUsuario, tur.Id_TareaUsuarioRepeticion, tu.NomTareaUsuario, tu.Descripcion, tu.CodTareaPrograma, tu.RecordatorioEmail, tu.NivelUrgencia, Id_Proyecto, NomProyecto, tu.RecordatorioPantalla, tu.RequiereRespuesta, tu.CodContactoAgendo, Fecha, convert(char(12),Fecha,107) AS SoloFecha, convert(char(8),Fecha,108) AS SoloHora, tur.Parametros, tp.NomTareaPrograma, tp.Ejecutable, tp.Icono, c.Nombres +' '+ c.Apellidos as nombre, c.Email  FROM TareaPrograma tp, TareaUsuarioRepeticion tur, Contacto c, Proyecto P RIGHT OUTER JOIN TareaUsuario tu on p.Id_Proyecto=tu.CodProyecto WHERE  tur.FechaCierre is null AND tu.CodContacto = @Email AND tu.CodContactoAgendo = c.Id_Contacto AND tur.CodTareaUsuario = tu.Id_TareaUsuario AND Id_TareaPrograma = CodTareaPrograma	
END