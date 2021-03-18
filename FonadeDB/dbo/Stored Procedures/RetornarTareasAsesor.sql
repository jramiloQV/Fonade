
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[RetornarTareasAsesor]
@CodUsuario int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	select nomtareaprograma, nomproyecto, nomtareausuario, descripcion, 
nombres+' '+apellidos as agendo,fecha, respuesta, fechacierre, codcontactoagendo, id_proyecto 
from tareausuario tu, tareaprograma, proyecto, contacto c, tareausuariorepeticion 
where id_tareaprograma = codtareaprograma and id_contacto=codcontactoagendo
and id_proyecto=codproyecto and id_tareausuario=codtareausuario and tu.codcontacto=@CodUsuario
END