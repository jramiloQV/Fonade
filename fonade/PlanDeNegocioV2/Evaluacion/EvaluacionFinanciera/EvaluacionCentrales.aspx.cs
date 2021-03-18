using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using System.Configuration;
using System.IO;
using System.Web.UI.HtmlControls;
using Fonade.Clases;
using System.Data.SqlClient;

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class EvaluacionCentrales : Negocio.Base_Page
    {
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int txtTab = Constantes.Const_CentralesDeRiesgoEvaluacionV2;
        public Boolean esMiembro; 
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            EncabezadoEval.IdProyecto = CodigoProyecto;
            EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
            EncabezadoEval.IdTabEvaluacion = txtTab;

            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());            
            var txtSQL = " select codconvocatoria from convocatoriaproyecto where codproyecto = " + CodigoProyecto + " order by codconvocatoria desc ";
            var rs = consultas.ObtenerDataTable(txtSQL, "text");
            var cc = "0";
            if (rs.Rows.Count > 0)
            {
                cc = rs.Rows[0]["codconvocatoria"].ToString();
            }
            

            if (!IsPostBack)
            {
                llenarDemasCampos();                         
            }
            bRealizado = esRealizado(txtTab, CodigoProyecto, CodigoConvocatoria);

            if (esMiembro && !bRealizado)
            { this.div_Post_It1.Visible = true;  Post_It1._mostrarPost = true; }
            else
            {
                this.div_Post_It1.Visible = false; Post_It1._mostrarPost = false;
            }
           
            if (esMiembro && !bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
            {                
                txt_observaciones.Visible = true;
                btn_actualizar.Visible = true;
            }
            else
            {
                btn_actualizar.Visible = false;
                CalendarExtender4.Enabled = false;
                txt_observaciones.Enabled = false;
            }

        }

        protected void lds_Integrantes_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.MostrarIntegrantesCentralesRiesgos(Convert.ToInt32(CodigoProyecto), Constantes.CONST_RolEmprendedor, Convert.ToInt32(CodigoConvocatoria))
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        protected void llenarDemasCampos()
        {
            try
            {
                var query = (from x in consultas.Db.EvaluacionObservacions
                             where x.CodProyecto == Convert.ToInt32(CodigoProyecto)
                             && x.CodConvocatoria == Convert.ToInt32(CodigoConvocatoria)
                             select new
                             {
                                 x.FechaCentralesRiesgo,
                                 x.CentralesRiesgo,
                             }).FirstOrDefault();

                if (query != null)
                    txt_fechareporte.Text = ((DateTime)query.FechaCentralesRiesgo).ToString("dd/MM/yyyy");

                else
                    txt_fechareporte.Text = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;

                txt_observaciones.Text = query.CentralesRiesgo;
                this.div_observaciones.InnerText = query.CentralesRiesgo;
            }
            catch (Exception)
            { }
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            insertarDatos(txt_fechareporte.Text, txt_observaciones.Text);
        }

        protected void insertarDatos(string fecha, string observaciones)
        {
            try
            {
                DateTime fCentralSql = DateTime.ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var fechaCentralSql = fCentralSql.Date.ToString("yyyy-MM-dd HH:mm:ss");

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                SqlCommand cmd = new SqlCommand("MD_InsertUpdateEvaluacionCentrales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodProyecto", Convert.ToInt32(CodigoProyecto));
                cmd.Parameters.AddWithValue("@CodConvocatoria", Convert.ToInt32(CodigoConvocatoria));
                cmd.Parameters.AddWithValue("@CentralRiesgo", observaciones);
                cmd.Parameters.AddWithValue("@FechaCentral", fechaCentralSql);
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                cmd.Dispose();
                cmd2.Dispose();

                UpdateTab();
            }
            catch (Exception)
            { }
        }
       
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

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)txtTab,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };
                        
            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
            EncabezadoEval.GetUltimaActualizacion();
        }

    }
}