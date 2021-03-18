-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_Home]
	-- Add the parameters for the stored procedure here
	@id_usuario int,	@Cod_grupo int,	@Cod_institucion int,	@accion varchar(255)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevenHomet extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT tu.Id_TareaUsuario, tur.Id_TareaUsuarioRepeticion, tu.NomTareaUsuario, tu.Descripcion, tu.CodTareaPrograma, tu.RecordatorioEmail, tu.NivelUrgencia, Id_Proyecto, NomProyecto,
	--tu.RecordatorioPantalla, tu.RequiereRespuesta, tu.CodContactoAgendo, 
	--Fecha, convert(char(12),Fecha,107) AS SoloFecha, convert(char(8),
	--Fecha,108) AS SoloHora, tur.Parametros, tp.NomTareaPrograma, tp.Ejecutable,
	--tp.Icono, c.Nombres +' '+ c.Apellidos as nombre, c.Email 
	--FROM TareaPrograma tp, TareaUsuarioRepeticion tur, Contacto c, Proyecto P RIGHT OUTER JOIN TareaUsuario tu
	--on p.Id_Proyecto=tu.CodProyecto WHERE  tur.FechaCierre is null AND tu.CodContacto = @id_usuario  AND tu.CodContactoAgendo = c.Id_Contacto AND tur.CodTareaUsuario = tu.Id_TareaUsuario AND tp.Id_TareaPrograma =  tu.CodTareaPrograma

	/*	SELECT top 1000 tu.Id_TareaUsuario, tur.Id_TareaUsuarioRepeticion, tu.NomTareaUsuario, tu.Descripcion,
	tu.CodTareaPrograma, tu.RecordatorioEmail, tu.NivelUrgencia, Id_Proyecto, NomProyecto,
	tu.RecordatorioPantalla, tu.RequiereRespuesta, tu.CodContactoAgendo, Fecha,
	convert(char(12),Fecha,107) AS SoloFecha, convert(char(8),Fecha,108) AS SoloHora,
	tur.Parametros, tp.NomTareaPrograma, tp.Ejecutable, tp.Icono, c.Nombres +' '+ c.Apellidos as nombre, c.Email, tu.CodContactoAgendo
	FROM TareaPrograma tp, TareaUsuarioRepeticion tur, Contacto c, Proyecto P RIGHT OUTER JOIN TareaUsuario tu
	on p.Id_Proyecto=tu.CodProyecto
	WHERE  tu.CodContacto = @id_usuario
	 AND tur.CodTareaUsuario = tu.Id_TareaUsuario*/

	 SELECT tu.Id_TareaUsuario, tur.Id_TareaUsuarioRepeticion, tu.NomTareaUsuario, tu.Descripcion, tu.CodTareaPrograma, tu.RecordatorioEmail, tu.NivelUrgencia, Id_Proyecto, NomProyecto, 
	tu.RecordatorioPantalla, tu.RequiereRespuesta, tu.CodContactoAgendo, Fecha, convert(char(12),Fecha,107) AS SoloFecha, convert(char(8),Fecha,108) AS SoloHora, tur.Parametros,
	tp.NomTareaPrograma, tp.Ejecutable, tp.Icono, c.Nombres +' '+ c.Apellidos as nombre, c.Email  
	FROM TareaPrograma tp, TareaUsuarioRepeticion tur, Contacto c, Proyecto P RIGHT OUTER JOIN TareaUsuario tu 
    on p.Id_Proyecto=tu.CodProyecto
	 WHERE  tur.FechaCierre is null AND tu.CodContacto = @id_usuario
	 AND tu.CodContactoAgendo = c.Id_Contacto AND tur.CodTareaUsuario = tu.Id_TareaUsuario AND Id_TareaPrograma = CodTareaPrograma 
END