-- =============================================
-- Author:		<Albert Palencia Benedetty>
-- Create date: <22-02-2014>
-- Description:	<Selecciona las infraestructas y su valor, por tipo de proyecto y codigo infraestructura>
-- =============================================
 -- exec MD_ObtenerInfraestructura '49764'
CREATE PROCEDURE [dbo].[MD_ObtenerInfraestructura]
	-- Add the parameters for the stored procedure here
	@CodProyecto varchar(100)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT 
				Id_ProyectoInfraestructura Cod_Infraestructura
				,NomInfraestructura Nombre
				,CodTipoInfraestructura
				,Unidad
				,ValorUnidad
				,Cantidad
				,FechaCompra
				,ValorCredito
				,PeriodosAmortizacion
				,SistemaDepreciacion
				,T.nomTipoInfraestructura TipoInfraestructura
				,codProyecto
    FROM	  proyectoInfraestructura P JOIN TipoInfraestructura T on	    
				 T.id_TipoInfraestructura = P.codTipoInfraestructura
	  WHERE  P.codProyecto = @CodProyecto

END