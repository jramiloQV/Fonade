CREATE VIEW dbo.View_1
AS
SELECT     p.Id_Proyecto, p.NomProyecto, tu.NomTareaUsuario, tu.Descripcion, ISNULL(c.Nombres, '') + ' ' + ISNULL(c.Apellidos, '') AS Agendado_A, 
                      ISNULL(c1.Nombres, '') + ' ' + ISNULL(c1.Apellidos, '') AS Agendado_Por, tur.Fecha, tur.Respuesta, tur.FechaCierre, cp.CodConvocatoria
FROM         dbo.TareaUsuario AS tu INNER JOIN
                      dbo.TareaUsuarioRepeticion AS tur ON tu.Id_TareaUsuario = tur.CodTareaUsuario INNER JOIN
                      dbo.Contacto AS c ON tu.CodContacto = c.Id_Contacto INNER JOIN
                      dbo.Contacto AS c1 ON tu.CodContactoAgendo = c1.Id_Contacto INNER JOIN
                      dbo.Proyecto AS p ON tu.CodProyecto = p.Id_Proyecto INNER JOIN
                      dbo.ConvocatoriaProyecto AS cp ON tu.CodProyecto = cp.CodProyecto
WHERE     (YEAR(tur.Fecha) = 2010) AND (tu.CodContactoAgendo IN
                          (SELECT     CodContacto
                            FROM          dbo.GrupoContacto
                            WHERE      (CodGrupo = 10))) AND (tu.CodContacto IN
                          (SELECT     CodContacto
                            FROM          dbo.GrupoContacto AS GrupoContacto_1
                            WHERE      (CodGrupo = 11)))