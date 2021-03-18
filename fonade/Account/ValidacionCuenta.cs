using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fonade.Account
{
    /// <summary>
    /// Clase para validar la cuenta del usuario contra el perfil logueado
    /// </summary>
    public class ValidacionCuenta
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        /// <summary>
        /// Valida el permiso que tiene el usuario
        /// </summary>
        /// <param name="codigoUsuario">ID del usuario.</param>
        /// <param name="rutaPag">La ruta de la pagina.</param>
        /// <returns></returns>
        public bool validarPermiso(int codigoUsuario, string rutaPag)
        {
            bool valido = false;
            int reg = 0;
            int adm = 0;

            //Validar si tiene la asociacion del menu el usuario
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                reg = (from gc in db.GrupoContactos
                       join pg in db.Pagina_Grupos on gc.CodGrupo equals pg.Id_Grupo
                       join p in db.Paginas on pg.Id_Pagina equals p.Id_Pagina
                       where gc.CodContacto == codigoUsuario
                       && p.url_Pagina.ToUpper() == rutaPag.ToUpper()
                       select gc
                       ).Count();

                //validar si el usuario es administrador sistema
                adm = (from gc in db.GrupoContactos                      
                       where gc.CodContacto == codigoUsuario
                       && gc.CodGrupo == Constantes.CONST_AdministradorSistema
                       select gc
                       ).Count();
            }

            valido = ((reg > 0) || (adm > 0)) ? true : false;

            //validar si el usuario es administrador Sistema


            return valido;
        }

        /// <summary>
        /// Generars the reporte.
        /// </summary>
        /// <param name="codUsuario">The cod usuario.</param>
        /// <returns></returns>
        public bool GenerarReporte(int codUsuario)
        {
            bool genReporte = false;
            
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                genReporte = (from u in db.Contacto
                              where u.Id_Contacto == codUsuario
                              select u.flagGeneraReporte).FirstOrDefault() ?? false;
            }

            return genReporte;
        }

        /// <summary>
        /// Verifica si el usuario es acreditador
        /// </summary>
        /// <param name="codUsuario">Id del usuario.</param>
        /// <returns>true o false</returns>
        public bool Acreditador(int codUsuario)
        {
            bool genReporte = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                genReporte = (from u in db.Contacto
                              where u.Id_Contacto == codUsuario
                              select u.flagAcreditador).FirstOrDefault() ?? false;
            }

            return genReporte;
        }

        /// <summary>
        /// Verifica si el usuario puede realizar actas parciales.
        /// </summary>
        /// <param name="codUsuario">Id del usuario.</param>
        /// <returns>true o false</returns>
        public bool ActaParcial(int codUsuario)
        {
            bool genReporte = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                genReporte = (from u in db.Contacto
                              where u.Id_Contacto == codUsuario
                              select u.flagActaParcial).FirstOrDefault() ?? false;
            }

            return genReporte;
        }

        /// <summary>
        /// Ruta del Home.
        /// </summary>
        /// <returns>url del home</returns>
        public string rutaHome()
        {
            return "/FONADE/evaluacion/AccesoDenegado.aspx";
        }
    }
}