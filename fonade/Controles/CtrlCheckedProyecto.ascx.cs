using System;
using System.Web.UI;
using Fonade.Negocio;
using System.Web;

namespace Fonade.Controles
{
    /// <summary>
    /// CtrlCheckedProyecto
    /// </summary>
    /// <seealso cref="System.Web.UI.UserControl" />
    public partial class CtrlCheckedProyecto : UserControl
    {
        /// <summary>
        /// Gets or sets the tab cod.
        /// </summary>
        /// <value>
        /// The tab cod.
        /// </value>
        public int tabCod { get; set; }

        /// <summary>
        /// Gets or sets the cod proyecto.
        /// </summary>
        /// <value>
        /// The cod proyecto.
        /// </value>
        public string CodProyecto { get; set; }
        private Base_Page basePage;

        /// <summary>
        /// Tab del frame "se utiliza para cargarle el valor de la última actualización en la pestaña indicada".
        /// </summary>
        private String sTab;
        /// <summary>
        /// Código del proyecto.
        /// </summary>
        private String sCodProyecto;
        /// <summary>
        /// Código de la convocatoria.
        /// </summary>
        private String sCodConvocatoria;
        /// <summary>
        /// Código del rol del usuario "Rol".
        /// </summary>
        private String CodGrupo_RolUsuario;
        /// <summary>
        /// Id del usuario.
        /// </summary>
        private String Id_User;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            sTab = HttpContext.Current.Session["sTab"] != null ? sTab = HttpContext.Current.Session["sTab"].ToString() : "";
            sCodProyecto = HttpContext.Current.Session["CodProyecto"] != null ? sCodProyecto = HttpContext.Current.Session["CodProyecto"].ToString() : "";
            sCodConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null ? sCodConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString() : "";
            CodGrupo_RolUsuario = HttpContext.Current.Session["RolUsuario"] != null ? CodGrupo_RolUsuario = HttpContext.Current.Session["RolUsuario"].ToString() : "";
            Id_User = HttpContext.Current.Session["IdUsuario"] != null ? CodGrupo_RolUsuario = HttpContext.Current.Session["IdUsuario"].ToString() : "";
            ObtenerUltimaActualizacion(sTab, sCodProyecto, sCodConvocatoria, CodGrupo_RolUsuario, Id_User);
            basePage = new Base_Page();
        }

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Versión del método "fnMiembroProyecto" localizado en "Base_Page".
        /// </summary>
        /// <param name="codProyecto">Código del proyecto.</param>
        /// <param name="IdContacto">Id del contacto.</param>
        /// <returns>Booleano que indica si es o no un miembro...</returns>
        private bool fnMiembro(String codProyecto, String IdContacto)
        {
            //Inicializar variables.
            String query = "";
            Datos.Consultas consultas = new Datos.Consultas();

            try
            {
                //Ejecutar consulta.
                query = " SELECT COUNT(Id_ProyectoContacto) AS Conteo " +
                               " FROM ProyectoContacto " +
                               " WHERE CodProyecto = " + codProyecto + " AND CodContacto = " + IdContacto +
                               " AND Inactivo = 0 AND FechaInicio<GETDATE() AND FechaFin = NULL";

                //Asignar resultado de la consulta a variable DataTable.
                var tabla_sql = consultas.ObtenerDataTable(query, "text");
          
                int conteo = Convert.ToInt32(tabla_sql.Rows[0]["Conteo"].ToString());
           
                if (conteo > 0)
                    return false;
                else
                    return true;
            }
            catch
            { 
                return false; 
            }
        }

        /// <summary>
        /// Según FONADE clásico, el archivo "", se usa este método para determinar si el proyecto está 
        /// incluido en un acta de comité evaluador.
        /// </summary>
        /// <param name="codProyecto">Código del proyecto.</param>
        /// <param name="sCodConvocatoria">Código de la convocatoria.</param>
        private bool EstaEnActa(String codProyecto, String sCodConvocatoria)
        {
            //Inicializar variables.
            String query = "";
            Datos.Consultas consultas = new Datos.Consultas();

            try
            {
                //Ejecutar consulta.
                query = " SELECT COUNT(codproyecto) AS Conteo from evaluacionactaproyecto, evaluacionacta " +
                        " WHERE id_acta = codacta AND codproyecto = " + codProyecto +
                        " AND codconvocatoria = " + sCodConvocatoria;

                //Asignar resultado de la consulta a variable DataTable.
                var tabla_sql = consultas.ObtenerDataTable(query, "text");

                //Obtener el valor usable en la siguiente condición.
                int conteo = Convert.ToInt32(tabla_sql.Rows[0]["Conteo"].ToString());

                //Si es mayor a cero...
                if (conteo > 0)
                    return true;
                else
                    return false;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// Obtener la última actualización que se ha hecho en la página "tab".
        /// </summary>
        /// <param name="tab">Tab a invocar.</param>
        /// <param name="codProyecto">Código del proyecto.</param>
        /// <param name="codConvocatoria">Código de la convocatoria.</param>
        /// <param name="sCodGrupo">Código del grupo del usuario.</param>
        /// <param name="Id_User">Id del usuario en sesión.</param>
        /// <returns>String con resultados de la consulta SQL o consulta LINQ.</returns>
        public string ObtenerUltimaActualizacion(String tab = null, String codProyecto = null, String codConvocatoria = null, String sCodGrupo = null, String Id_User = null)
        {
            //Inicializar variables.
            Datos.Consultas consultas = new Datos.Consultas();
            String resultado = "";
            DateTime fecha = DateTime.Now;
            String fechaFomateda = "";
            String checked_string = "";
            String disabled = "";
            bool EsMiembro = false;
            bool EsActa = false;

            //Si el usuario que ha iniciado sesión es "Gerente Evaluador", NO puede chequear el CheckBox.
            if (sCodGrupo == Datos.Constantes.CONST_GerenteEvaluador.ToString()) { disabled = "disabled"; }

            //Obtener el valor "EsMiembro".
            EsMiembro = fnMiembro(codProyecto, Id_User);

            //Obtener si "está en acta"...
            EsActa = EstaEnActa(codProyecto, codConvocatoria);

            //Consultar la información concerniente a la última actualización.
            String sqlConsulta = " SELECT Nombres + ' ' + Apellidos AS nombre, FechaModificacion, Realizado " +
                                 " FROM TabEvaluacionProyecto, Contacto " +
                                 " WHERE Id_Contacto = CodContacto AND CodTabEvaluacion = " + sTab +
                                 " AND CodProyecto = " + sCodProyecto + " AND CodConvocatoria = " + sCodConvocatoria;

            //Asignar resultado de la consulta a variable DataTable.
            var tabla_sql = consultas.ObtenerDataTable(sqlConsulta, "text");

            //Convertir fecha.
            try { fecha = Convert.ToDateTime(tabla_sql.Rows[0]["FechaModificacion"].ToString()); }
            catch { return "Error al obtener la fecha de actualización."; /*Error al obtener la fecha.*/ }

            //Obtener el nombre del mes (las primeras tres letras).
            string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

            //Obtener la hora en minúscula.
            string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();

            //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
            fechaFomateda = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

            //Si el valor "Realizado es 1 o TRUE, se establece el string "checked_string" vacío (lo cual indica que 
            //es chequeable el CheckBox), de lo contrario, el string "checked_string" no tendrá valor, estará vacío.
            if (tabla_sql.Rows[0]["Realizado"].ToString() == "True" || tabla_sql.Rows[0]["Realizado"].ToString() == "1")
            { checked_string = "checked"; }

            //Establecer si el campo está o no inhabilitado.
            if (!(EsMiembro && sCodGrupo == Datos.Constantes.CONST_RolCoordinadorEvaluador.ToString()) || tabla_sql.Rows[0]["nombre"].ToString().Trim() == "" || EsActa)
            { disabled = "disabled"; }

            //Establecer string con código HTML y valores depurados.
            resultado = " <div> " +
                        "     <div class=\"marcar-realizado\"> " +
                        "         <div style=\"display: inline-block\"> " +
                        "             <span>ULTIMA ACTUALIZACIÓN:</span> " +
                        "             <span>" + tabla_sql.Rows[0]["nombre"].ToString().ToUpperInvariant() + "&nbsp;&nbsp;</span> " +
                        "             <span>" + fechaFomateda + "</span> " +
                        "         </div> " +
                        "         <div style=\"display: inline-block\" class=\"menu\"> " +
                        "             <span>MARCAR COMO REALIZADO:</span> " +
                        "             <input type=\"checkbox\" " + checked_string + " " + disabled + "><!----> " +
                        "         </div> " +
                        "     </div> " +
                        "     <br/> " +
                        " </div>";

           
            return resultado;
         
        }
    }
}