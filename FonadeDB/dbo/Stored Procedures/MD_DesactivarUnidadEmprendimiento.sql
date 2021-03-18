

/*

 Nombre: MD_DesactivarUnidadEmprendimiento.
 Fecha: 17:54 26/06/2014
 Descripción: Procedimiento almacenado para desactivar la unidad de emprendimiento seleccionado
 en la pantalla "CatalogoUnidadEmprende.aspx".

*/
CREATE PROCEDURE MD_DesactivarUnidadEmprendimiento
(
 @Caso VARCHAR(10),
 @CodInstitucion INT,
 @FechaInicioInactivo SMALLDATETIME,
 @FechaFinInactivo SMALLDATETIME
)
AS
IF @Caso = 'TRUE'
BEGIN
 UPDATE Institucion Set Inactivo = 1, FechaInicioInactivo = @FechaInicioInactivo, 
 FechaFinInactivo = @FechaFinInactivo 
 WHERE Id_Institucion = @CodInstitucion
END

IF @Caso = 'FALSE'
BEGIN
 UPDATE Institucion SET Inactivo = 1, FechaInicioInactivo = @FechaInicioInactivo 
 WHERE Id_Institucion = @CodInstitucion
END