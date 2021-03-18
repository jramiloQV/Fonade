/*
	Fecha: 16/09/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_CrearInformeVisita
	Descripción: Registrar o actualizar el informe de visita que se manipula 
	desde la ventana "AdicionarInformeVisitaProyecto.aspx".
*/
CREATE PROCEDURE MD_CrearInformeVisita
(
	@PARAMETRO VARCHAR(11),
	@NombreInforme NVARCHAR(255),
	@CodCiudadOrigen INT, 
	@CodCiudadDestino INT, 
	@FechaSalida SMALLDATETIME,
	@FechaRegreso SMALLDATETIME, 
	@CodEmpresa INT, 
	@CodInterventor INT, 
	@InformacionTecnica TEXT, 
	@InformacionFinanciera TEXT,
	/*Parametros de actulización*/
	@CostoVisita NUMERIC(18,0),
	@Id_Informe INT
)
AS

IF @PARAMETRO = 'Nuevo'
BEGIN
	INSERT INTO InformeVisitaInterventoria(NombreInforme,CodCiudadOrigen, CodCiudadDestino, FechaSalida,
	FechaRegreso, CodEmpresa, FechaInforme, CodInterventor, InformacionTecnica, InformacionFinanciera, Estado)
	VALUES(@NombreInforme,@CodCiudadOrigen,@CodCiudadDestino, @FechaSalida, @FechaRegreso, @CodEmpresa, GETDATE(),@CodInterventor, @InformacionTecnica, @InformacionFinanciera,0)
END
IF @PARAMETRO = 'Actualizar'
BEGIN
	UPDATE InformeVisitaInterventoria 
	SET NombreInforme = @NombreInforme,
		CodCiudadOrigen = @CodCiudadOrigen, 
		CodCiudadDestino = @CodCiudadDestino,
		FechaSalida = @FechaSalida, 
		FechaRegreso = @FechaRegreso,
		CostoVisita = @CostoVisita, 
		InformacionTecnica = @InformacionTecnica,
		InformacionFinanciera = @InformacionFinanciera
	WHERE Id_Informe = @Id_Informe
END