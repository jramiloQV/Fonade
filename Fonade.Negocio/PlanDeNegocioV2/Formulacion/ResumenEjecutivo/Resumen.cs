using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Datos.DataType;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo
{
    public class Resumen
    {


        public static List<Emprendedor> GetEmprendedores(int idProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var query = (from con in db.Contacto
                             join proCon in db.ProyectoContactos
                             on con.Id_Contacto equals proCon.CodContacto
                             join rol in db.Rols
                             on proCon.CodRol equals rol.Id_Rol
                             join ciu in db.Ciudad
                             on con.CodCiudad equals ciu.Id_Ciudad
                             join dep in db.departamento
                             on ciu.CodDepartamento equals dep.Id_Departamento
                             where rol.Id_Rol == 3 && proCon.CodProyecto == idProyecto
                             && proCon.Inactivo == false
                             select new Emprendedor()
                             {
                                 Email = con.Email,
                                 FechaNac = con.FechaNacimiento,
                                 IdContacto = con.Id_Contacto,
                                 LugarNac = ciu.NomCiudad + " (" + dep.NomDepartamento + ")",
                                 Nombre = con.Nombres + " " + con.Apellidos,
                                 Rol = rol.Nombre
                             }).ToList();

                return query;
            }
        }

        public static List<Emprendedor> GetEmprendedoresYEquipoTrabajo(int idProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = (from con in db.Contacto
                             join proCon in db.ProyectoContactos
                             on con.Id_Contacto equals proCon.CodContacto
                             join rol in db.Rols
                             on proCon.CodRol equals rol.Id_Rol
                             join ciu in db.Ciudad on con.CodCiudad equals ciu.Id_Ciudad into ciudades
                             from ciudadEquipo in ciudades.DefaultIfEmpty()
                             join dep in db.departamento on ciudadEquipo.CodDepartamento equals dep.Id_Departamento into departamentos
                             from departamentoEquipo in departamentos.DefaultIfEmpty()
                             where
                                (rol.Id_Rol == Constantes.CONST_RolAsesor 
                                 || rol.Id_Rol == Constantes.CONST_RolAsesorLider 
                                 || rol.Id_Rol == Constantes.CONST_RolEmprendedor)
                                && proCon.CodProyecto == idProyecto
                                && proCon.FechaFin == null
                                && proCon.Inactivo.Equals(false)
                             orderby rol.Id_Rol descending
                             select new Emprendedor()
                             {
                                 Email = con.Email,
                                 FechaNac = con.FechaNacimiento,
                                 IdContacto = con.Id_Contacto,
                                 LugarNac = ciudadEquipo != null ? ciudadEquipo.NomCiudad + " (" + departamentoEquipo.NomDepartamento + ")" : "N/A",
                                 Nombre = con.Nombres + " " + con.Apellidos,
                                 Rol = rol.Nombre
                             }).ToList();

                return query;
            }
        }

        public static bool Insertar(ProyectoResumenEjecutivoV2 entResumen, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entResumendb = (from row in db.ProyectoResumenEjecutivoV2
                                        where row.IdProyecto == entResumen.IdProyecto
                                        select row).FirstOrDefault();

                    //insert-update
                    if (entResumendb == null)
                        db.ProyectoResumenEjecutivoV2.InsertOnSubmit(entResumen);
                    else
                    {
                        entResumendb.ConceptoNegocio = entResumen.ConceptoNegocio;
                        entResumendb.IndicadorContraPartido = entResumen.IndicadorContraPartido;
                        entResumendb.IndicadorEmpleos = entResumen.IndicadorEmpleos;
                        entResumendb.IndicadorEmpleosDirectos = entResumen.IndicadorEmpleosDirectos;
                        entResumendb.IndicadorMercadeo = entResumen.IndicadorMercadeo;
                        entResumendb.IndicadorVentas = entResumen.IndicadorVentas;
                        entResumendb.PeriodoImproductivo = entResumen.PeriodoImproductivo;
                        entResumendb.RecursosAportadosEmprendedor = entResumen.RecursosAportadosEmprendedor;
                        entResumendb.ProductoMasRepresentativo = entResumen.ProductoMasRepresentativo;
                        entResumendb.VideoEmprendedor = entResumen.VideoEmprendedor;
                    }
                    db.SubmitChanges();

                    msg = Mensajes.Mensajes.GetMensaje(8);
                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }
        }

        public static ProyectoResumenEjecutivoV2 Get(int idProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entResumendb = (from row in db.ProyectoResumenEjecutivoV2
                                   where row.IdProyecto == idProyecto
                                   select row).FirstOrDefault();

                return entResumendb;
            }
        }
        private string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<Sectores> getSector(int _codSectorSeleccionado)
        {
            List<Sectores> sectores = new List<Sectores>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                sectores = (from s in db.Sector
                            where s.Activo == true || s.Id_Sector == _codSectorSeleccionado
                            orderby s.NomSector
                            select new Sectores
                            {
                                idSector = s.Id_Sector,
                                nomSector = s.NomSector
                            }).ToList();

            }

                return sectores;
        }

        public List<Sectores> getSubSector(int _codSector)
        {
            List<Sectores> sectores = new List<Sectores>();
                        
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                sectores = (from subsector in db.SubSector
                                   where subsector.CodSector == _codSector &&
                                    (subsector.Activo == true )
                                   orderby subsector.NomSubSector
                                   select new Sectores
                                   {
                                       idSector = subsector.Id_SubSector,
                                       nomSector = subsector.Codigo +" - "+ subsector.NomSubSector
                                   }
                                    ).ToList();

                //sectores.Insert(0, new Sectores() { idSector = 0, nomSector = "Seleccione un subsector" });

                
            }
            return sectores;
        }
    }

    public class Sectores
    {
        public int idSector { get; set; }
        public string nomSector { get; set; }
    }
}
