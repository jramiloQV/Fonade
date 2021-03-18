using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class DatosActaDTOController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<DatosActaModelDTO> GetContactosProyecto(int _codProyecto)
        {
            List<DatosActaModelDTO> list = new List<DatosActaModelDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                list = (from pc in db.ProyectoContactos
                        join c in db.Contacto on pc.CodContacto equals c.Id_Contacto
                        join r in db.Rols on pc.CodRol equals r.Id_Rol
                        where pc.CodProyecto == _codProyecto
                        && pc.Inactivo == false && c.Inactivo == false
                        select new DatosActaModelDTO
                        {
                            codProyecto = pc.CodProyecto,
                            codContacto = pc.CodContacto,
                            codRol = pc.CodRol,
                            Nombres = c.Nombres,
                            Apellidos = c.Apellidos,
                            Identificacion = c.Identificacion.ToString(),
                            Email = c.Email,
                            Telefono = c.Telefono,
                            nombreRol = r.Nombre
                        }).ToList();
            }

            return list;
        }

        public List<DatosActaModelDTO> GetContactosInterventor(int _codProyecto)
        {
            List<DatosActaModelDTO> list = new List<DatosActaModelDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                list = (from e in db.Empresas
                        join ei in db.EmpresaInterventors on e.id_empresa equals ei.CodEmpresa
                        join c in db.Contacto on ei.CodContacto equals c.Id_Contacto
                        join r in db.Rols on ei.Rol equals r.Id_Rol
                        where e.codproyecto == _codProyecto
                        && ei.Inactivo == false && c.Inactivo == false
                        select new DatosActaModelDTO
                        {
                            codProyecto = e.codproyecto.HasValue ?
                                            e.codproyecto.Value : 0,
                            codContacto = ei.CodContacto.HasValue ? 
                                            ei.CodContacto.Value : 0,
                            codRol = ei.Rol.HasValue ?
                                        ei.Rol.Value:0,
                            Nombres = c.Nombres,
                            Apellidos = c.Apellidos,
                            Identificacion = c.Identificacion.ToString(),
                            Email = c.Email,
                            Telefono = c.Telefono,
                            nombreRol = r.Nombre
                        }).ToList();
            }

            return list;
        }

        public InfoInterventorModelDTO getInfoEntidadInteventora(int _codProyecto, int _idContacto)
        {
            InfoInterventorModelDTO infoInterventor = new InfoInterventorModelDTO();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                infoInterventor = (from e in db.Empresas
                                   join ei in db.EmpresaInterventors
                                   on e.id_empresa equals ei.CodEmpresa
                                   join ev in db.EntidadInterventors
                                   on ei.CodContacto equals ev.IdContactoInterventor
                                   join evrias in db.EntidadInterventoria
                                   on ev.IdEntidad equals evrias.Id
                                   where e.codproyecto == _codProyecto
                                   && ei.Inactivo == false && ei.CodContacto == _idContacto
                                   select new InfoInterventorModelDTO
                                   {
                                       id = evrias.Id,
                                       nombreInterventor = evrias.Nombre,
                                      rutaLogo = evrias.ImagenLogo
                                   }
                                   ).FirstOrDefault();
            }

                return infoInterventor;
        }

       
    }
}
