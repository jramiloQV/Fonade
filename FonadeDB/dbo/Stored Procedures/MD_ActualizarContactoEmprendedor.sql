-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE MD_ActualizarContactoEmprendedor
	-- Add the parameters for the stored procedure here
	@nombreContacto varchar(100),
	@apellidoContacto varchar(100),
	@CodTipoIdentificacion SMALLINT,
	@Identificacion FLOAT,
	@Genero char(1),
	@FechaNacimiento smalldatetime,
	@Email varchar(100),
	@Telefono varchar(100),
	@CodCiudad INT,
	@LugarExpedicionDI INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[Contacto]
	   SET [Nombres] = @nombreContacto
		  ,[Apellidos] = @apellidoContacto
		  ,[CodTipoIdentificacion] = @CodTipoIdentificacion
		  ,[Genero] =@Genero
		  ,[FechaNacimiento] = @FechaNacimiento
		  ,[Email] = @Email
		  ,[Telefono] = @Telefono
		  ,[CodCiudad] = @CodCiudad
		  ,[LugarExpedicionDI] = @LugarExpedicionDI
		  ,[fechaActualizacion] = GETDATE()
	 WHERE Identificacion = @Identificacion
END