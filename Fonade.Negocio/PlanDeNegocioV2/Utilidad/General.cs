using Datos;
using Datos.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    /// <summary>
    /// Clase que incluye métodos generales del sistema
    /// </summary>
    public class General
    {
        #region Variables

        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Consulta las fuentes de financiación
        /// </summary>
        /// <returns>Listado consultado</returns>
        public static List<FuenteFinanciacion> getFuentes()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                return (from datos in db.FuenteFinanciacions
                        orderby datos.DescFuente
                        select datos).ToList();
            }
        }

        public static List<Datos.departamento> GetDepartamentos()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                return (from departamentos in db.departamento
                        orderby departamentos.NomDepartamento ascending
                        select departamentos)
                        .ToList();
            }
        }

        public static List<Datos.departamento> GetDepartamentos(int idEntidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                return (from departamentos in db.departamento
                        orderby departamentos.NomDepartamento ascending
                        select departamentos)
                        .ToList();
            }
        }

        public static List<EquipoTrabajo> getEquipoTrabajo(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var consulta = (from contacto in db.Contacto
                                join pc in db.ProyectoContactos
                                on contacto.Id_Contacto equals pc.CodContacto into tb1
                                from reg1 in tb1
                                join regquipo in db.ProyectoEmprendedorPerfils
                                on reg1.CodContacto equals regquipo.IdContacto into tb2
                                from reg2 in tb2.DefaultIfEmpty()
                                where reg1.CodProyecto == codigoproyecto
                                && reg1.Inactivo == false
                                && reg1.CodRol == Constantes.CONST_RolEmprendedor
                                orderby contacto.Nombres
                                select new EquipoTrabajo()
                                {
                                    IdEmprendedorPerfil = reg2.IdEmprendedorPerfil,
                                    IdContacto = contacto.Id_Contacto,
                                    NombreEmprendedor = contacto.Nombres + " " + contacto.Apellidos,
                                    Perfil = reg2.Perfil,
                                    Rol = reg2.Rol
                                }).ToList();

                return consulta != null ? consulta : new List<EquipoTrabajo>();
            }
        }

        public static EquipoTrabajo getPerfilEmprendedor(int idperfilemprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                return  (from contacto in db.Contacto
                                join pc in db.ProyectoContactos
                                on contacto.Id_Contacto equals pc.CodContacto into tb1
                                from reg1 in tb1
                                join regquipo in db.ProyectoEmprendedorPerfils
                                on reg1.CodContacto equals regquipo.IdContacto into tb2
                                from reg2 in tb2.DefaultIfEmpty()
                                where reg2.IdEmprendedorPerfil == idperfilemprendedor
                                select new EquipoTrabajo()
                                {
                                    NombreEmprendedor = contacto.Nombres + " " + contacto.Apellidos,
                                    Perfil = reg2.Perfil,
                                    Rol = reg2.Rol
                                }
                                ).FirstOrDefault();
            }
        }

        public static string getNombreTab(int codigoTab)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                Tab consulta = (from tab in db.Tabs
                                where tab.Id_Tab == codigoTab
                                select tab).SingleOrDefault();

                if (consulta != null)
                {
                    if (consulta.CodTab != null)
                    {
                        Tab consultaPadre = (from tab in db.Tabs
                                             where tab.Id_Tab == consulta.CodTab
                                             select tab).SingleOrDefault();

                        if (consultaPadre != null)
                        {
                            return string.Format("{0}\\{1}", consultaPadre.NomTab, consulta.NomTab);
                        }
                        else
                        {
                            return consulta.NomTab;
                        }
                    }
                    else
                    {
                        return consulta.NomTab;
                    }
                }
                else
                {

                    return "";
                }


            }
        }

        #endregion
    }
}
