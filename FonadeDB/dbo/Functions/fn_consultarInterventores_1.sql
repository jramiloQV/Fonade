-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[fn_consultarInterventores](@IdProyecto int)
RETURNS TABLE 
AS
RETURN 
(
Select top 1 * from
(Select p.Id_Proyecto IdProject, p.NomProyecto,  c.Id_Contacto ID, c.Nombres, c.Apellidos, c.Email, c.Clave, Rol.Nombre Rol 
from EmpresaInterventor ei
Inner Join Empresa e on e.id_empresa = ei.CodEmpresa
Inner Join  Contacto c on c.Id_Contacto = ei.CodContacto
Inner join Proyecto p on p.Id_Proyecto = e.codproyecto
Inner join rol on rol.Id_Rol = ei.Rol
where e.codproyecto = @IdProyecto
Union
SELECT DISTINCT Proyecto.Id_Proyecto IdProject, Proyecto.NomProyecto, contacto.Id_Contacto ID, contacto.nombres, contacto.apellidos, contacto.email, contacto.clave, grupo.nomgrupo Rol
FROM proyecto 
INNER JOIN estado ON estado.id_estado = proyecto.codestado 
INNER JOIN proyectoContacto ON proyectocontacto.codproyecto = proyecto.id_proyecto and proyectoContacto.Inactivo = 0 
INNER JOIN contacto ON contacto.id_contacto = proyectocontacto.codcontacto 
INNER JOIN GrupoContacto on contacto.id_contacto = GrupoContacto.CodContacto 
INNER JOIN grupo on grupo.id_grupo = GrupoContacto.codgrupo  
WHERE grupo.nomgrupo like'Interventor' and contacto.email like'%%' 
And Contacto.Inactivo = 0 and proyecto.id_proyecto = @IdProyecto ) t
order by t.Rol DESC,t.ID ASC
)