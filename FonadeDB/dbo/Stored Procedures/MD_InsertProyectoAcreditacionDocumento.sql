-- =============================================
-- Author:		alberto palencia benedetti
-- Create date: 18/03/2014
-- Description:	crea el proyecto en acreditacion
-- =============================================
CREATE PROCEDURE MD_InsertProyectoAcreditacionDocumento
	-- Add the parameters for the stored procedure here
	@proyectoId int 
	,@convocatoriaId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		INSERT INTO PROYECTOACREDITACIONDOCUMENTO 
		(CODPROYECTO,CODCONTACTO,CODCONVOCATORIA,PENDIENTE,SUBSANADO,ACREDITADO,NOACREDITADO,FLAGANEXO1
		,FLAGANEXO2,FLAGANEXO3,FLAGCERTIFICACIONES,FLAGDIPLOMA,FLAGACTA) 
		SELECT @proyectoId,CODCONTACTO,@convocatoriaId,0,0,0,0,0,0,0,0,0,0 
		FROM PROYECTOCONTACTO WHERE INACTIVO =0 AND CODROL=3 AND CODPROYECTO=@proyectoId
		        

END