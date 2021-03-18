-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_NuevaTareaInsert]
	-- Add the parameters for the stored procedure here
	@CodTareaPrograma int
    ,@CodContacto int
    ,@CodProyecto int
    ,@NomTareaUsuario varchar(255)
    ,@Descripcion text
    ,@Recurrente varchar(3)
    ,@RecordatorioEmail bit
    ,@NivelUrgencia smallint
    ,@RecordatorioPantalla bit
    ,@RequiereRespuesta bit
    ,@CodContactoAgendo int
    ,@DocumentoRelacionado varchar(255)
	,@UltimoRegistroInsertado INT = NULL OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[TareaUsuario]
           ([CodTareaPrograma]
           ,[CodContacto]
           ,[CodProyecto]
           ,[NomTareaUsuario]
           ,[Descripcion]
           ,[Recurrente]
           ,[RecordatorioEmail]
           ,[NivelUrgencia]
           ,[RecordatorioPantalla]
           ,[RequiereRespuesta]
           ,[CodContactoAgendo]
           ,[DocumentoRelacionado])
     VALUES
           (@CodTareaPrograma
           ,@CodContacto
           ,@CodProyecto
           ,@NomTareaUsuario
           ,@Descripcion
           ,@Recurrente
           ,@RecordatorioEmail
           ,@NivelUrgencia
           ,@RecordatorioPantalla
           ,@RequiereRespuesta
           ,@CodContactoAgendo
           ,@DocumentoRelacionado)
		   SET @UltimoRegistroInsertado = SCOPE_IDENTITY();
		   
		   return @UltimoRegistroInsertado
END