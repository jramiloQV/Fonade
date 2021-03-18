
CREATE PROCEDURE [dbo].[MD_VerFormalizacionEmpleos]
	@id_proyecto int
AS

BEGIN

	select CASE 
                  WHEN GeneradoPrimerAno = 0 
                     THEN ' '
				  WHEN GeneradoPrimerAno IS NULL
					THEN ' '
                  ELSE  'Mes ' + CAST( GeneradoPrimerAno as varchar(5) ) 
             END as GeneradoPrimerAnoQ3,
	 id_cargo AS id_cargoQ3, ISNULL(cargo,0) AS cargoQ3, ISNULL(valormensual,0) AS valormensualQ3, 
	 ISNULL(Joven,0) AS JovenQ3, ISNULL(Desplazado,0) AS DesplazadoQ3,
	ISNULL(Madre,0) AS MadreQ3, ISNULL(Minoria,0) AS MinoriaQ3, ISNULL(Recluido,0) AS RecluidoQ3, ISNULL(Desmovilizado,0) AS DesmovilizadoQ3, 
	ISNULL(Discapacitado,0) AS DiscapacitadoQ3, ISNULL(Desvinculado,0) AS DesvinculadoQ3
	from proyectogastospersonal left outer join proyectoempleocargo
	on id_cargo=codcargo where codproyecto=@id_proyecto

END