CREATE  VIEW ContactosRol AS 


select id_Contacto, identificacion, Nombres + ' ' + Apellidos Contacto, email, r.nombre Rol, p.id_proyecto CodigoProyecto, p.nomproyecto NombreProyecto,  m.nomciudad Ciudad, d.nomdepartamento departamento 
from proyecto p, ciudad m, departamento d, contacto c, rol r, ProyectoContacto PC
where	p.codciudad=m.id_ciudad and 
	m.coddepartamento = d.id_departamento and 
	c.id_contacto = pc.codcontacto and 
	p.id_proyecto = pc.codproyecto and 
	pc.codrol= r.id_rol and pc.inactivo = 0