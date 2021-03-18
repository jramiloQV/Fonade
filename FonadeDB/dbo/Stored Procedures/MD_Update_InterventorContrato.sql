/*

	Nombre: MD_Update_InterventorContrato.
	Fecha: 10:58 14/07/2014
	Descripción: Procedimiento almacenado para actualizar la información en la tabla "InterventorContrato"; 
	éste procedimiento se invoca desde la ventana "CatalogoContratoInterventor.aspx".

*/
CREATE PROCEDURE MD_Update_InterventorContrato
(
	@Id_InterventorContrato INT,
	@Motivo VARCHAR(500),
	@FechaInicio SMALLDATETIME,
	@FechaExpiracion SMALLDATETIME
)
AS
UPDATE InterventorContrato
SET FechaInicio = @FechaInicio,
FechaExpiracion = @FechaExpiracion,
Motivo = @Motivo
WHERE id_InterventorContrato = @Id_InterventorContrato