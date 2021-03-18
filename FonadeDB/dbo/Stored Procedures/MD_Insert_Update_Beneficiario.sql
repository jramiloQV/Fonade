
create PROCEDURE MD_Insert_Update_Beneficiario

	@TipoIdentificacion int
	,@numIdentificacion varchar(20)
	,@txtNombres varchar(100)
	,@txtApellidos varchar(100)
	,@txtRazonSocial varchar(100)
	,@TipoSociedad int
	,@Retencion int
	,@Ciudad int
	,@txtTelefono varchar(20)
	,@txtDireccion varchar(100)
	,@txtFax varchar(20)
	,@txtEmail varchar(60)
	,@Banco int
	,@Sucursal int
	,@TipoCuenta int
	,@txtNumCuenta varchar(20)
	,@CodEmpresa int
	,@IdBeneficiario int
	,@caso varchar(10)

AS

BEGIN

	IF @caso='Create'

		begin

			INSERT INTO PagoBeneficiario (CodTipoIdentificacion,NumIdentificacion,Nombre,Apellido
			,RazonSocial,TipoRazonSocial,CodPagoTipoRetencion,CodCiudad,Telefono
			,Direccion,Fax,Email,CodPagoBanco,CodPagoSucursal,TipoCuenta,NumCuenta,CodEmpresa)

			VALUES(@TipoIdentificacion, @numIdentificacion, @txtNombres, @txtApellidos, @txtRazonSocial
			, @TipoSociedad, @Retencion, @Ciudad, @txtTelefono, @txtDireccion, @txtFax, @txtEmail, @Banco
			, @Sucursal, @TipoCuenta, @txtNumCuenta, @CodEmpresa)

		end


	IF @caso='Update'

		begin

			UPDATE PagoBeneficiario 
			SET CodTipoIdentificacion = @TipoIdentificacion 
			,NumIdentificacion = @numIdentificacion 
			,Nombre = @txtNombres 
			,Apellido = @txtApellidos 
			,RazonSocial = @txtRazonSocial 
			,TipoRazonSocial = @TipoSociedad 
			,CodPagoTipoRetencion = @Retencion 
			,CodCiudad = @Ciudad 
			,Telefono = @txtTelefono 
			,Direccion = @txtDireccion 
			,Fax = @txtFax 
			,Email = @txtEmail 
			,CodPagoBanco = @Banco 
			,CodPagoSucursal = @Sucursal 
			,TipoCuenta = @TipoCuenta 
			,NumCuenta = @txtNumCuenta 
	
			WHERE Id_PagoBeneficiario = @IdBeneficiario
		
		end
END