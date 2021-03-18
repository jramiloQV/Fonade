/*

	Nombre: MD_Create_InformePresupuestal.
	Fecha: 09:00 09/07/2014
	Descripción: Procedimiento almacenado para guardar el nuevo informe presupuestal; 
	éste procedimiento se invoca desde la ventana "AdicionarInformePresupuestal".

*/
CREATE PROCEDURE MD_Create_InformePresupuestal
(
	@NomInformePresupuestal VARCHAR(255),
	@codinterventor INT,
	@codempresa INT,
	@Periodo INT,
	@Fecha DATETIME
)
AS
INSERT INTO InformePresupuestal (NomInformePresupuestal,codinterventor,codempresa,Estado,Periodo,Fecha)
VALUES (@NomInformePresupuestal, @codinterventor,@codempresa,0,@Periodo,@Fecha)