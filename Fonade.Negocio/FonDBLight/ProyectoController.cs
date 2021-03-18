using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using System.Configuration;
using Datos.Modelos;

namespace Fonade.Negocio.FonDBLight
{
    public class ProyectoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public ProyectoModel GetInfoProyecto(int _codProyecto)
        {
            ProyectoModel proyecto;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                proyecto = (from p in db.Proyecto1s
                            where p.Id_Proyecto == _codProyecto
                            orderby p.Id_Proyecto descending
                            select new ProyectoModel
                            {
                                codProyecto = p.Id_Proyecto,
                                NombreProyecto = p.NomProyecto
                            }).FirstOrDefault();
            }

            return proyecto;
        }

        public int? codOperadorXProyecto(int _codProyecto)
        {
            int? codOperador = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                codOperador = (from p in db.Proyecto
                               where p.Id_Proyecto == _codProyecto
                               select p.codOperador).FirstOrDefault();
            }

            return codOperador;
        }


        public ConvenioConvocatoriaProyecto getConvenioXProyecto(int _codProyecto)
        {
            ConvenioConvocatoriaProyecto convenioConvocatoriaProyecto = new ConvenioConvocatoriaProyecto();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                int _convocatoria = (from cp in db.ConvocatoriaProyectos
                                     where cp.CodProyecto == _codProyecto && cp.Viable == true
                                     orderby cp.CodConvocatoria descending
                                     select cp.CodConvocatoria).FirstOrDefault();

                int _convenio = (from c in db.Convocatoria
                                 where c.Id_Convocatoria == _convocatoria
                                 select c.CodConvenio).FirstOrDefault() ?? 0;

                convenioConvocatoriaProyecto = (from conv in db.Convenios
                                                where conv.Id_Convenio == _convenio
                                                select new ConvenioConvocatoriaProyecto
                                                {
                                                    codContactoFiduciaria = conv.CodcontactoFiduciaria ?? 0,
                                                    Descripcion = conv.Descripcion,
                                                    fechaFin = conv.FechaFin.HasValue ? conv.FechaFin.Value.ToString() : "",
                                                    fechaInicio = conv.Fechainicio.HasValue ? conv.Fechainicio.Value.ToString() : "",
                                                    idConvenio = conv.Id_Convenio,
                                                    nomConvenio = conv.Nomconvenio,
                                                    Anoinicio = conv.Fechainicio.HasValue ? conv.Fechainicio.Value.Year.ToString() : "",
                                                }).FirstOrDefault();
            }

            return convenioConvocatoriaProyecto;
        }

        public string buscarNumActaXProyecto(int codProyecto)
        {
            string numActa = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                numActa = (from aap in db.AsignacionActaProyecto
                           join aa in db.AsignacionActas on aap.CodActa equals aa.Id_Acta
                           where aap.CodProyecto == codProyecto && aap.Asignado == true
                           select aa.NumActa).FirstOrDefault();
            }

            return numActa;
        }

        public string buscarEntidadInterventoria(int codProyecto)
        {
            string entidad = "";


            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                entidad = (from e in db.Empresas
                           join ei in db.EmpresaInterventors on e.id_empresa equals ei.CodEmpresa
                           join inter in db.EntidadInterventors on ei.CodContacto equals inter.IdContactoInterventor
                           join enti in db.EntidadInterventoria on inter.IdEntidad equals enti.Id
                           where e.codproyecto == codProyecto
                           select enti.Nombre
                           ).FirstOrDefault();
            }

            return entidad;
        }

        public string ciudadYDepartamento(int codProyecto)
        {
            string ciudadDPTO = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                ciudadDPTO = (from c in db.Ciudad
                              join d in db.departamento on c.CodDepartamento equals d.Id_Departamento
                              join p in db.Proyecto on c.Id_Ciudad equals p.CodCiudad
                              where p.Id_Proyecto == codProyecto
                              select c.NomCiudad + " - " + d.NomDepartamento).FirstOrDefault();

            }

            return ciudadDPTO;
        }

        public string ciudadxProyecto(int codProyecto)
        {
            string ciudadDPTO = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                ciudadDPTO = (from c in db.Ciudad                              
                              join p in db.Proyecto on c.Id_Ciudad equals p.CodCiudad
                              where p.Id_Proyecto == codProyecto
                              select c.NomCiudad).FirstOrDefault();

            }

            return ciudadDPTO;
        }

        public string ciudadxID(int? lugarExpedicionDI)
        {
            string ciudadDPTO = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                ciudadDPTO = (from c in db.Ciudad                             
                              where c.Id_Ciudad == lugarExpedicionDI
                              select c.NomCiudad).FirstOrDefault();

            }

            return ciudadDPTO;
        }
    }


    public class ConvenioConvocatoriaProyecto
    {
        public int idConvenio { get; set; }
        public string nomConvenio { get; set; }
        public string fechaInicio { get; set; }
        public string Anoinicio { get; set; }
        public string fechaFin { get; set; }
        public string Descripcion { get; set; }
        public int codContactoFiduciaria { get; set; }
    }

}
