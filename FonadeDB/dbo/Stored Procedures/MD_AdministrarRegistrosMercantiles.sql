

/*
	Fecha: 18/09/2014.
	Nombre: Mauricio Arias Olave.
	Procedimiento: MD_AdministrarRegistrosMercantiles
	Descripción: Registrar o actualizar la información de registro mercantil
	siguiendo este flujo: ProyectoFrameSet / Empresa / Registro Mercantil.
*/
CREATE PROCEDURE MD_AdministrarRegistrosMercantiles
(
	@CASO VARCHAR(11),
	@CodProyecto INT, 
	@RazonSocial VARCHAR(255), 
	@ObjetoSocial VARCHAR(1000), 
	@CapitalSocial NUMERIC(18,0), 
	@CodTipoSociedad INT,
	@NumEscrituraPublica VARCHAR(255), 
	@DomicilioEmpresa VARCHAR(255), 
	@CodCiudad INT, 
	@Telefono VARCHAR(100), 
	@Email VARCHAR(100), 
	@Nit VARCHAR(50), 
	@RegimenEspecial BIT, 
	@RENorma VARCHAR(50), 
	@REFechaNorma DATETIME, 
	@Contribuyente BIT, 
	@CNorma VARCHAR(50), 
	@CFechaNorma DATETIME, 
	@AutoRetenedor BIT, 
	@ARNorma VARCHAR(50), 
	@ARFechaNorma DATETIME, 
	@Declarante BIT, 
	@DNorma VARCHAR(50), 
	@DFechaNorma DATETIME, 
	@ExentoRetefuente BIT, 
	@ERFNorma VARCHAR(50), 
	@ERFFechaNorma DATETIME, 
	@TipoRegimen VARCHAR(5), 
	@GranContribuyente BIT, 
	@GCNorma VARCHAR(50), 
	@GCFechaNorma DATETIME
)
AS
IF @CASO = 'Nuevo'
BEGIN
INSERT INTO Empresa (CodProyecto, RazonSocial, ObjetoSocial, CapitalSocial, CodTipoSociedad,
					 NumEscrituraPublica, DomicilioEmpresa, CodCiudad, Telefono, Email, Nit, 
					 RegimenEspecial, RENorma, REFechaNorma, Contribuyente, CNorma, CFechaNorma, 
					 AutoRetenedor, ARNorma, ARFechaNorma, Declarante, DNorma, DFechaNorma, 
					 ExentoRetefuente, ERFNorma, ERFFechaNorma, TipoRegimen, GranContribuyente, 
					 GCNorma, GCFechaNorma)
VALUES (@CodProyecto, @RazonSocial, @ObjetoSocial, @CapitalSocial, @CodTipoSociedad,
					 @NumEscrituraPublica, @DomicilioEmpresa, @CodCiudad, @Telefono, @Email, @Nit, 
					 @RegimenEspecial, @RENorma, @REFechaNorma, @Contribuyente, @CNorma, @CFechaNorma, 
					 @AutoRetenedor, @ARNorma, @ARFechaNorma, @Declarante, @DNorma, @DFechaNorma, 
					 @ExentoRetefuente, @ERFNorma, @ERFFechaNorma, @TipoRegimen, @GranContribuyente, 
					 @GCNorma, @GCFechaNorma)
END
IF @CASO = 'Actualizar'
BEGIN
	UPDATE Empresa 
	SET CodProyecto = @CodProyecto, 
	RazonSocial = @RazonSocial,
	ObjetoSocial = @ObjetoSocial,
	CapitalSocial = @CapitalSocial,
	CodTipoSociedad = @CodTipoSociedad,
	NumEscrituraPublica = @NumEscrituraPublica,
	DomicilioEmpresa = @DomicilioEmpresa,
	CodCiudad = @CodCiudad,
	Telefono = @Telefono,
	Email = @Email,
	Nit = @Nit,
	RegimenEspecial = @RegimenEspecial,
	RENorma = @RENorma,
	REFechaNorma = @REFechaNorma,
	Contribuyente = @Contribuyente,
	CNorma = @CNorma,
	CFechaNorma = @CFechaNorma,
	AutoRetenedor = @AutoRetenedor,
	ARNorma = @ARNorma,
	ARFechaNorma = @ARFechaNorma,
	Declarante = @Declarante,
	DNorma = @DNorma,
	DFechaNorma = @DFechaNorma,
	ExentoRetefuente = @ExentoRetefuente,
	ERFNorma = @ERFNorma,
	ERFFechaNorma = @ERFFechaNorma,
	TipoRegimen = @TipoRegimen,
	GranContribuyente = @GranContribuyente,
	GCNorma = @GCNorma,
	GCFechaNorma = @GCFechaNorma
	where codproyecto = @CodProyecto
END