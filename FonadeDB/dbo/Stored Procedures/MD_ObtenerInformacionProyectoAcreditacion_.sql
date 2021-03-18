-- =============================================
-- Author:		 Alberto Palencia Benedetti
-- Create date:  19/03/2014
-- Description:	 obtengo la informacion del proyecto si esta acreditado, no acreditado, etc
/* 
	ejemplo

	MD_ObtenerInformacionProyectoAcreditacion_ 33006,80
*/
-- =============================================
CREATE PROCEDURE [dbo].[MD_ObtenerInformacionProyectoAcreditacion_]
	
	@codproyecto int
	,@codConvocatoria int 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT 
			PAD.ID_PROYECTOACREDITACIONDOCUMENTO
			,PAD.CODCONTACTO
			,C.NOMBRES + ' ' + C.APELLIDOS Nombre
			,PAD.PENDIENTE
			,PAD.FECHAPENDIENTE
			,PAD.OBSERVACIONPENDIENTE
			,PAD.ASUNTOPENDIENTE
			,PAD.SUBSANADO
			,PAD.OBSERVACIONSUBSANADO
			,PAD.ASUNTOSUBSANADO
			,PAD.FECHASUBSANADO
			,PAD.ACREDITADO
			,PAD.OBSERVACIONACREDITADO
			,PAD.ASUNTOACREDITADO
			,PAD.FECHAACREDITADO
			,PAD.NOACREDITADO
			,PAD.OBSERVACIONNOACREDITADO
			,PAD.ASUNTONOACREDITADO
			,PAD.FECHANOACREDITADO
			, isnull(PAD.FLAGANEXO1,0)FLAGANEXO1
			, isnull(PAD.FLAGANEXO2,0)FLAGANEXO2
			, isnull(PAD.FLAGANEXO3,0)FLAGANEXO3
			, isnull(PAD.FLAGDI,0)FLAGDI
			, isnull(PAD.FLAGCERTIFICACIONES,0)FLAGCERTIFICACIONES
			, isnull(PAD.FLAGDIPLOMA,0)FLAGDIPLOMA
			, isnull(PAD.FLAGACTA,0)FLAGACTA
	FROM PROYECTOACREDITACIONDOCUMENTO PAD JOIN CONTACTO C ON (PAD.CODCONTACTO = C.ID_CONTACTO) 
	WHERE PAD.CODPROYECTO= @codproyecto  AND PAD.CODCONVOCATORIA=  @codConvocatoria


END