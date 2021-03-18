-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_ModificarNuevoRiesgo]
	-- Add the parameters for the stored procedure here
	@_ID_Riesgo int,
	@_Riesgo varchar(500),
	@_Mitigacion varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE [dbo].[EvaluacionRiesgo]
	SET 
      [Riesgo] = @_Riesgo
      ,[Mitigacion] = @_Mitigacion
	WHERE ID_Riesgo = @_ID_Riesgo
END