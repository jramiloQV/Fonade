-- =============================================
-- Author:		Alberto Palencia Benedetti
-- Create date: 23/03/2014
-- Description:	Obtenemos las notificaciones enviadas por el emprendedor
-- =============================================
CREATE PROCEDURE MD_NotificacionEnviadas
	-- Add the parameters for the stored procedure here
	 @codproyecto int
	,@codconvocatoria int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		SELECT 
			DISTINCT HEA.ID_HISTORICOEMAILACREDITACION
			,C.NOMBRES + ' ' + C.APELLIDOS 'NOMBRE'
			,HEA.EMAIL,HEA.FECHA
			,C1.NOMBRES + ' ' + C1.APELLIDOS 'NOMCC'
			,(SELECT TOP 1 R.NOMBRE FROM ROL R JOIN PROYECTOCONTACTO PC ON (R.ID_ROL = PC.CODROL)
			 WHERE CODROL IN (1,2) AND CODCONTACTO=C1.ID_CONTACTO AND CODPROYECTO= HEA.CODPROYECTO) 'ROL'
			,E.NOMESTADO 
		FROM CONTACTO C JOIN HISTORICOEMAILACREDITACION HEA ON (HEA.CODCONTACTO = C.ID_CONTACTO) 
			JOIN ESTADO E ON (E.ID_ESTADO = HEA.CODESTADOACREDITACION)
			LEFT JOIN CONTACTOHISTORICOEMAILACREDITACION CHEA ON  (CHEA.CODHISTORICOEMAILACREDITACION=HEA.ID_HISTORICOEMAILACREDITACION)
			LEFT JOIN CONTACTO C1 ON (C1.ID_CONTACTO = CHEA.CODCONTACTO)
		WHERE HEA.CODPROYECTO= @codproyecto
			AND HEA.CODCONVOCATORIA= @codconvocatoria
			ORDER BY HEA.FECHA DESC

END