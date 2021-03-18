using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// VerImpresion
    /// </summary>
    
    public partial class VerImpresion : Negocio.Base_Page
    {
        /// <summary>
        /// The identifier proyecto
        /// </summary>
        public int IdProyecto;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IdProyecto = Convert.ToInt32(HttpContext.Current.Session["Id_ProyImp"]);
                lbl_Titulo.Text = void_establecerTitulo("RESUMEN DE PROYECTO");
                l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
                LLenarResumen(IdProyecto);
                llenarInfo(IdProyecto);
            }
        }

        /// <summary>
        /// Llenars the information.
        /// </summary>
        /// <param name="idP">The identifier p.</param>
        protected void llenarInfo(int idP)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_MostrarInfoProyectoImprimir", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodProyecto", idP);
            con.Open();
            SqlDataReader r = cmd.ExecuteReader();
            r.Read();
            l_ciudad.Text = Convert.ToString(r["Lugar"]);
            l_fecha.Text = Convert.ToString(r["FechaCreacion"]);
            l_institucion.Text = Convert.ToString(r["nomunidad"]);
            l_nombre.Text = Convert.ToString(r["NomProyecto"]);
            l_recursos.Text = Convert.ToString(r["Recursos"]);
            l_subsector.Text = Convert.ToString(r["nomSubsector"]);
            l_sumario.Text = Convert.ToString(r["Sumario"]);
            con.Close();
            con.Dispose();
        }

        /// <summary>
        /// llenar resumen.
        /// </summary>
        /// <param name="codigo">The codigo.</param>
        private void LLenarResumen(int codigo)
        {
            var sqlQuery = (from resumen in consultas.Db.ProyectoResumenEjecutivos
                            where resumen.CodProyecto == codigo
                            select new
                            {
                                resumen,
                            }).FirstOrDefault();
            if (sqlQuery != null)
            {
                lit_conceptoNegocios.Text = sqlQuery.resumen.ConceptoNegocio;
                lit_conclusiones.Text = sqlQuery.resumen.ConclusionesFinancieras;
                lit_potencial.Text = sqlQuery.resumen.PotencialMercados;
                lit_proyecciones.Text = sqlQuery.resumen.Proyecciones;
                lit_inversiones.Text = sqlQuery.resumen.ResumenInversiones;
                lit_ventajas.Text = sqlQuery.resumen.VentajasCompetitivas;
            }
        }

        /// <summary>
        /// Handles the Selecting event of the lds_equipo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_equipo_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.Db.MD_Mostrar_resumen_equipo(Constantes.CONST_RolEmprendedor, Constantes.CONST_RolAsesor, Constantes.CONST_RolAsesorLider, IdProyecto, "Equipo")
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }
    }
}