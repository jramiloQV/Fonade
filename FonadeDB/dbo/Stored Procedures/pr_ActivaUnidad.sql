CREATE PROCEDURE pr_ActivaUnidad
AS
BEGIN

	--Variables para el cursor
	DECLARE @id_institucion	int
	DECLARE @nomUnidad	varchar(160)
	DECLARE @codCiudad	int
	DECLARE @codContacto	int

	--Valors fijos dentro del sp
	DECLARE @Fecha			smallDateTime
	DECLARE @CodTareaPrograma	int
	DECLARE @codGrupo		int
	SET @CodGrupo = 3 -- 3=AdministradorFONADE y 2=Administrador SENA
	SET @CodTareaPrograma = 7 --> 7=asignar jefe de unidad
	SET @Fecha=GetDate()

	--Unidades por Activar
	DECLARE CR_Unidades CURSOR FOR
	SELECT i.id_Institucion, I.NomUnidad + ' (' + I.NomInstitucion + ')', i.codCiudad, C.id_contacto
	FROM Institucion I, GrupoContacto G, Contacto C
	WHERE 	G.codContacto=C.id_contacto and 
		(G.codGrupo=@CodGrupo or G.codGrupo=2) and I.FechaFinInactivo is not null and I.FechaFinInactivo<=@Fecha and I.inactivo=1 

	OPEN CR_Unidades
	FETCH NEXT FROM CR_Unidades INTO @id_institucion, @nomUnidad, @codCiudad, @codContacto
	
	BEGIN TRANSACTION 

	--CREA LAS TAREAS PARA ASIGNAR JEFES DE UNIDAD A LAS UNIDADES REACTIVADAS
	WHILE @@FETCH_STATUS = 0
	BEGIN		
		INSERT INTO TareaUsuario (CodTareaPrograma, CodContacto, NomTareaUsuario, Descripcion, Recurrente,
			RecordatorioEmail, NivelUrgencia, RecordatorioPantalla, RequiereRespuesta, CodContactoAgendo)
		VALUES (@codTareaPrograma, @codContacto, 'Asignar Jefe de Unidad', 'Asignar jefe de Unidad a '+ @nomUnidad, 0,
			0, 1, 1, 0, @codContacto)
		IF @@ERROR<>0 GOTO LBL_REVERSAR

		INSERT INTO TareaUsuarioRepeticion (Fecha, CodTareaUsuario, Parametros)
		VALUES(GETDATE(), IDENT_CURRENT('TareaUsuario'), 'Accion=Editar&CodInstitucion=' + convert(varchar,@id_institucion) + '&SelMun='+convert(varchar,@codCiudad) )
		IF @@ERROR<>0 GOTO LBL_REVERSAR
		FETCH NEXT FROM CR_Unidades INTO @id_institucion, @nomUnidad, @codCiudad, @codContacto
	END
	--ACTIVA LAS UNIDADES
	UPDATE Institucion SET FechaFinInactivo=null, FechaInicioInactivo=null, Inactivo=0 WHERE FechaFinInactivo is not null AND FechaFinInactivo<=@Fecha and inactivo=1 

	lbl_Reversar:
 	IF @@ERROR=0 
 		COMMIT TRANSACTION
 	ELSE BEGIN

 		ROLLBACK TRANSACTION
 	END
	CLOSE CR_Unidades
	DEALLOCATE CR_Unidades
END