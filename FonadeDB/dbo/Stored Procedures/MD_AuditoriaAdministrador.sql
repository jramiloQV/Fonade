CREATE PROCEDURE [MD_AuditoriaAdministrador]
AS
BEGIN
	SET NOCOUNT ON;
	select * from(
		(
			SELECT distinct c.Id_Contacto, c.Nombres, c.Apellidos, c.CodTipoIdentificacion, c.Identificacion, c.Cargo, c.Email, gc.CodGrupo, g.NomGrupo
			FROM Contacto AS c
			INNER JOIN GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto
			INNER JOIN Grupo AS g ON G.Id_Grupo = gc.CodGrupo
			INNER JOIN ProyectoContacto AS pc ON pc.CodContacto = c.Id_Contacto
			WHERE c.Inactivo = 0 and pc.Inactivo = 0 AND  gc.CodGrupo in (5,6,11)
		)
		UNION
		(
			SELECT distinct c.Id_Contacto, c.Nombres, c.Apellidos, c.CodTipoIdentificacion, c.Identificacion, c.Cargo, c.Email, gc.CodGrupo, g.NomGrupo
			FROM Contacto AS c
			INNER JOIN GrupoContacto AS gc ON c.Id_Contacto = gc.CodContacto
			INNER JOIN Grupo AS g ON G.Id_Grupo = gc.CodGrupo
			INNER JOIN EmpresaInterventor ei  ON ei.CodContacto = c.Id_Contacto
			WHERE c.Inactivo = 0 AND ei.inactivo=0  and gc.CodGrupo in (14)
		)
	)tbl
	order by NomGrupo,Id_contacto;
END