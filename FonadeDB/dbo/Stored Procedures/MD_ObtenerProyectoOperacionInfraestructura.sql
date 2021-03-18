	
-- Author:	 Alberto palencia
-- Create date: 2014-03-07
-- Description:	Crud proyectooperacionInfraestructura-- 
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerProyectoOperacionInfraestructura](
	
		@Tipo int = 0
	   ,@proyecto int
	   ,@text text
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--- si tipo es igual a 0, significa que crearemos una nueva descripcion
	IF @Tipo = 0
		BEGIN 

			INSERT INTO proyectooperacionInfraestructura(CodProyecto,ParametrosTecnicos)
			VALUES(@proyecto,@text)
			select 1;
		END
	
	ELSE IF (@Tipo = 1)
		BEGIN

			UPDATE P
			SET ParametrosTecnicos = @text
			FROM  proyectooperacionInfraestructura P
			Where P.codProyecto = @proyecto
			select 2;

		END

END