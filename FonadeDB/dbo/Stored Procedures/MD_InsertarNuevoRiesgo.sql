-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_InsertarNuevoRiesgo]
	-- Add the parameters for the stored procedure here
	@_CodProyecto int,
	@_CodConvocatoria int,
	@_Riesgo varchar(500),
	@_Mitigacion varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [dbo].[EvaluacionRiesgo]
           ([CodProyecto]
           ,[CodConvocatoria]
           ,[Riesgo]
           ,[Mitigacion])
     VALUES
           (@_CodProyecto, @_CodConvocatoria, @_Riesgo, @_Mitigacion)
END