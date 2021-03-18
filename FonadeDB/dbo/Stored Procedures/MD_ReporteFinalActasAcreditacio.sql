﻿

CREATE PROCEDURE [MD_ReporteFinalActasAcreditacio]
   (@_IdConvocatoria int)
AS
BEGIN
   SELECT DISTINCT
	P.ID_PROYECTO,P.NOMPROYECTO,E.NOMESTADO,D.NOMDEPARTAMENTO,CIU.NOMCIUDAD,C.ID_CONTACTO,C.NOMBRES,C.APELLIDOS,C.TELEFONO 'TELEFONOEMPRENDEDOR',R.NOMBRE 'NOMROL',C.IDENTIFICACION,CIU2.NOMCIUDAD 'NOMCIUDADEXPEDICION',C.FECHANACIMIENTO,
	C.EMAIL,'NOMASESOR','EMAILASESOR','NOMROLASESOR', 'NOMASESORLIDER','EMAILASESORLIDER','NOMROLASESORLIDER',C.TELEFONO,CE.TITULOOBTENIDO,CE.INSTITUCION,
	CE.SEMESTRESCURSADOS,CE.FECHAINICIO,CE.FECHAFINMATERIAS,CE.FECHAGRADO,I.NOMINSTITUCION, I.NOMUNIDAD,PF.FECHA 'FECHAAVALPLAN',S.NOMSECTOR,
	SS.NOMSUBSECTOR,PAD.PENDIENTE, PAD.OBSERVACIONPENDIENTE,PAD.SUBSANADO, PAD.OBSERVACIONSUBSANADO,PAD.ACREDITADO, PAD.OBSERVACIONACREDITADO,PAD.NOACREDITADO, PAD.OBSERVACIONNOACREDITADO,'PLAN ACREDITADO','PLAN NO ACREDITADO',
	PAD.FLAGANEXO1,PAD.FLAGANEXO2,PAD.FLAGANEXO3,PAD.FLAGDI,PAD.FLAGCERTIFICACIONES, PAD.FLAGDIPLOMA,PAD.FLAGACTA,PADC.CRIF,
	(SELECT COUNT(*) FROM PROYECTOACREDITACIONDOCUMENTO WHERE CODCONVOCATORIA = CP.CODCONVOCATORIA AND CODPROYECTO= P.ID_PROYECTO) 'CANTIDAD',
	(SELECT TOP 1  OBSERVACIONFINAL FROM PROYECTOACREDITACION WHERE OBSERVACIONFINAL <> '' AND CODPROYECTO = P.ID_PROYECTO AND CODCONVOCATORIA = CP.CODCONVOCATORIA ORDER BY FECHA DESC) 'OBSERVACIONFINAL',
	(C1.NOMBRES + ' ' + C1.APELLIDOS )'ACREDITADOR' 
	FROM PROYECTO P JOIN PROYECTOCONTACTO PC ON (P.ID_PROYECTO = PC.CODPROYECTO)
	JOIN CONTACTO C ON (C.ID_CONTACTO=PC.CODCONTACTO)
	JOIN ESTADO E ON (E.ID_ESTADO = P.CODESTADO)
	JOIN INSTITUCION I ON (I.ID_INSTITUCION = C.CODINSTITUCION)
	JOIN SUBSECTOR SS ON (SS.ID_SUBSECTOR = P.CODSUBSECTOR)
	JOIN SECTOR S ON (S.ID_SECTOR = SS.CODSECTOR)
	JOIN PROYECTOACREDITACIONDOCUMENTO PAD ON (PAD.CODCONTACTO = C.ID_CONTACTO)
	JOIN CONVOCATORIAPROYECTO CP ON (CP.CODPROYECTO = P.ID_PROYECTO)
	LEFT JOIN CIUDAD CIU ON (CIU.ID_CIUDAD = C.CODCIUDAD)
	LEFT JOIN DEPARTAMENTO D ON (D.ID_DEPARTAMENTO = CIU.CODDEPARTAMENTO)
	LEFT JOIN  PROYECTOFORMALIZACION PF ON (PF.CODCONVOCATORIA = CP.CODCONVOCATORIA AND PF.CODPROYECTO =P.ID_PROYECTO)
	LEFT JOIN ROL R ON (R.ID_ROL = PC.CODROL)
	LEFT JOIN CIUDAD CIU2 ON (CIU2.ID_CIUDAD = C.LUGAREXPEDICIONDI)
	LEFT JOIN PROYECTOACREDITACIONDOCUMENTOSCRIF PADC ON (PADC.ID_PROYECTOACREDITACIONDOCUMENTOSCRIF = (SELECT TOP 1 ID_PROYECTOACREDITACIONDOCUMENTOSCRIF FROM PROYECTOACREDITACIONDOCUMENTOSCRIF WHERE CODPROYECTO= P.ID_PROYECTO AND CODCONVOCATORIA = CP.CODCONVOCATORIA ORDER BY FECHA DESC))
	LEFT JOIN CONTACTOESTUDIO CE ON (CE.CODCONTACTO= C.ID_CONTACTO AND  CE.ID_CONTACTOESTUDIO = (SELECT TOP 1 ID_CONTACTOESTUDIO FROM CONTACTOESTUDIO CE WHERE CE.CODCONTACTO=C.ID_CONTACTO ORDER BY FLAGINGRESADOASESOR DESC, FINALIZADO DESC, FECHAINICIO ASC))
	LEFT JOIN CONTACTO C1 ON (C1.ID_CONTACTO =  (SELECT TOP 1 CODCONTACTO FROM PROYECTOCONTACTO WHERE INACTIVO=0 AND ACREDITADOR=1 AND CODCONVOCATORIA= CP.CODCONVOCATORIA AND CODPROYECTO=P.ID_PROYECTO))
	WHERE CP.CODCONVOCATORIA =@_IdConvocatoria ORDER BY P.ID_PROYECTO
END