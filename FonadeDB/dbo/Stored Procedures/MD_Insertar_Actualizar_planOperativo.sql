-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[MD_Insertar_Actualizar_planOperativo]
	-- Add the parameters for the stored procedure here
	    @Mes int,
		
		@_CodActividad int,
	
		@Aprobada smallint,
		@ObsInterventor Nvarchar(max)

		
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
		BEGIN
	Update AvanceActividadPOMes set 
   ObservacionesInterventor= @ObsInterventor
  ,Aprobada = @Aprobada
    WHERE CodActividad= @_CodActividad AND mes=@Mes --AND CodTipoFinanciacion=1 and valor<>0		
	END
END