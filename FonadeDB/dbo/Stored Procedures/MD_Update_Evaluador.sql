

create PROCEDURE [dbo].[MD_Update_Evaluador]
	
	@telefono varchar(100),
	@direccion varchar(120),
	@experiencia varchar(max),
	@intereses  varchar(max),
	@hojavida text,
	@fax varchar(100),
	@codBanco int, 
	@CodTipocuenta int,
	@txtNumCuenta varchar(20),
	@MaximoPlanes int,
	@txtExpPrincipal varchar(500),
	@txtExpSecundaria varchar(500),
	@codSectorPri int,
	@codSectorSec int,
	@idcontacto int
AS

BEGIN

	UPDATE Contacto SET 
	 Direccion = @direccion
	,Telefono = @telefono
	,Experiencia = @experiencia
	,Intereses = @intereses
	,HojaVida = @hojavida
	,Fax = @fax
	,InformacionIncompleta = 0
	,FechaActualizacion = getdate()
	WHERE Id_Contacto = @idcontacto

	
	declare @conteo int
	SELECT @conteo=count(CodContacto) FROM Evaluador WHERE codContacto=@idcontacto
	IF @conteo=0

		begin
			
			INSERT INTO Evaluador 
			(CodContacto, CodBanco, CodTipoCuenta, Cuenta, MaximoPlanes, ExperienciaPrincipal, Experienciasecundaria) 
			VALUES
			(@idcontacto,@codBanco,@CodTipocuenta,@txtNumCuenta,@MaximoPlanes,@txtExpPrincipal,@txtExpSecundaria)

		end
	ELSE
		
		begin

			UPDATE Evaluador
			SET CodBanco=@codBanco
			,CodTipoCuenta=@CodTipocuenta
			,Cuenta=@txtNumCuenta
			,MaximoPlanes=@MaximoPlanes
			,ExperienciaPrincipal=@txtExpPrincipal
			,ExperienciaSecundaria=@txtExpSecundaria
			WHERE CodContacto=@idcontacto
		
		end

	declare @conteo2 int
	SELECT @conteo2=count(*) FROM EvaluadorSector WHERE codContacto=@idcontacto and Experiencia='P'

	IF @conteo2=0

		begin

			INSERT INTO EvaluadorSector(codContacto, codSector, Experiencia)
			VALUES(@idcontacto,@codSectorPri,'P')

		end
	ELSE
		begin
			UPDATE EvaluadorSector 
			SET 
			codSector=@codSectorPri,
			fechaActualizacion = getdate()
			WHERE codContacto = @idcontacto and Experiencia = 'P'
		end


	declare @conteo3 int
	SELECT @conteo3=count(*) FROM EvaluadorSector WHERE codContacto=@idcontacto and Experiencia='S'

	IF @conteo3=0

		begin

			INSERT INTO EvaluadorSector(codContacto, codSector, Experiencia)
			VALUES(@idcontacto,@codSectorSec,'S')

		end
	ELSE
		begin
			UPDATE EvaluadorSector 
			SET 
			codSector=@codSectorSec,
			fechaActualizacion = getdate()
			WHERE codContacto = @idcontacto and Experiencia = 'S'
		end

END