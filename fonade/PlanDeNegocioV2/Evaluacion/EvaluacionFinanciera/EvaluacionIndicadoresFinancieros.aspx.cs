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
using System.Text;

namespace Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera
{
    public partial class EvaluacionIndicadoresFinancieros : Negocio.Base_Page
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
                       
        public int txtTab = Constantes.Const_IndicadoresFinancierosEvaluacionV2;
        private ProyectoMercadoProyeccionVenta pm;
        private string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public Boolean esMiembro;        
        public Boolean bRealizado;
        public bool rel = false;
                
        protected void Page_Load(object sender, EventArgs e)
        {

            EncabezadoEval.IdProyecto = CodigoProyecto;
            EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
            EncabezadoEval.IdTabEvaluacion = txtTab;

            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());                      
            bRealizado = esRealizado(txtTab, CodigoProyecto, CodigoConvocatoria);

            if (!IsPostBack)
            {
                gv_evaluacionindicadores.DataSource = sp_EvaluacionProyectoIndicador_SelectAll(CodigoProyecto,CodigoConvocatoria);
                gv_evaluacionindicadores.DataBind();                                                          
            }
            if (esMiembro && !bRealizado)
            { this.div_Post_It1.Visible = true;  Post_It1._mostrarPost = true; }

            if (usuario.CodGrupo == Constantes.CONST_Evaluador && !bRealizado)
            {
                ImageB.Visible = true;
                LB_InsertarIndicadores.Visible = true;
            }
            if (bRealizado == true)
            {
                rel = true;
            }
        }

        protected void gv_evaluacionindicadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                string[] idCapital = e.CommandArgument.ToString().Split(';');

                Response.Redirect("CatalogoIndicador.aspx?codindicador="+ idCapital[0]+ "&accion=Modificar"+"&descripcion="+ idCapital[1] +"&codproyecto="+CodigoProyecto);
            }
            if (e.CommandName == "eliminar")
            {
                string[] idCapital = e.CommandArgument.ToString().Split(';');                
                EliminarIndicadorFinanciero(idCapital[0]);
            }
        }
        
        public DataTable sp_EvaluacionProyectoIndicador_SelectAll(int codProyecto, int codConvocatoria)
        {
            DataTable dt = new DataTable();
                       
            StringBuilder querySqlIndicadoresFinancieros = new StringBuilder();

            querySqlIndicadoresFinancieros.AppendFormat("SELECT ind.id_indicador,ind.Descripcion,ind.Tipo,ind.Valor,ind.Protegido FROM EvaluacionProyectoIndicador ind inner join ( select distinct Descripcion, min(id_Indicador) as id from EvaluacionProyectoIndicador WHERE codProyecto = {0} and codConvocatoria = {1}  group by Descripcion ) as ind2 on ind.Descripcion = ind2.Descripcion and ind.id_Indicador = ind2.id WHERE ind.codProyecto = {0} and codConvocatoria = {1}", codProyecto, codConvocatoria);

            dt = consultas.ObtenerDataTable(querySqlIndicadoresFinancieros.ToString(), "text");

            dt.Columns.Add("Valor1");
                        

            Double valor = 0;
            String convertido = "";

            foreach (DataRow fila in dt.Rows)
            {
                switch (fila["Tipo"].ToString())
                {
                    case "$":                        
                        valor = Convert.ToDouble(fila["Valor"].ToString());
                        convertido = valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        if (convertido.Contains("(") && convertido.Contains(")"))
                        {
                            convertido = convertido.Replace("(", "");
                            convertido = convertido.Replace(")", "");

                            if (convertido.Contains("$"))
                            {
                                if (valor.ToString().Contains("-"))
                                {
                                    convertido = convertido.Replace("$", "<b>$ </b>-");
                                }
                                else
                                {
                                    convertido = convertido.Replace("$", "<b>$ </b>");
                                }
                            }
                        }
                        fila["Valor1"] = convertido;
                        
                                                
                        break;
                    case "%":
                        valor = Convert.ToDouble(fila["Valor"].ToString());
                        convertido = String.Format("{0:0.00}", valor);
                        fila["Valor1"] = convertido + " <b>%</b>";
                        break;
                    case "#":
                        valor = Convert.ToDouble(fila["Valor"].ToString());
                        convertido = String.Format("{0:0.00}", valor);
                        fila["Valor1"] = convertido;
                        break;                        
                }
            }
                        
            return dt;            
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
                       
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;          
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(CodigoProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }             
                        
        protected void gv_evaluacionindicadores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Controles de modificar indicador seleccionado.
                var lnk = e.Row.FindControl("lnk_btn_descripcion") as LinkButton;
                var hdf = e.Row.FindControl("oculto") as HiddenField;

                //Controles de eliminaciòn del indicador.
                var lnk_2 = e.Row.FindControl("lnkeliminar") as LinkButton;
                var img = e.Row.FindControl("imgeditar") as Image;

                bool estaProtegido = false;

                if (lnk_2 != null && img != null)
                {
                    estaProtegido = Boolean.Parse(hdf.Value);

                    if (!estaProtegido && usuario.CodGrupo == Constantes.CONST_Evaluador && !rel)
                    {
                        //Mostrar botones de eliminación.
                        lnk_2.Visible = true;
                        img.Visible = true;
                    }
                }

                if (lnk != null && hdf != null)
                {
                    //Habilitar el LinkButton para editar.
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador && !rel)
                    { lnk.Enabled = true; }
                }

                if (bRealizado && usuario.CodGrupo == Constantes.CONST_Evaluador)
                {
                    img.Visible = false;
                    lnk_2.Visible = false;
                    lnk.Enabled = false;
                }
            }
        }

        protected void ImageB_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("CatalogoIndicador.aspx?codindicador=0&accion=Crear&codproyecto="+CodigoProyecto);
        }

        protected void LB_InsertarIndicadores_Click(object sender, EventArgs e)
        {
            Response.Redirect("CatalogoIndicador.aspx?codindicador=0&accion=Crear&codproyecto=" + CodigoProyecto);
        }

        private void EliminarIndicadorFinanciero(String CodIndicador)
        {                        
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            String bRepetido = "No se pudo eliminar el indicador financiero seleccionado.";

            try
            {                
                txtSQL = " Delete from EvaluacionProyectoIndicador where protegido = 0 and Id_Indicador = " + CodIndicador;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    
                    prActualizarTabEval(Constantes.ConstSubIndicadoresFinancieros.ToString(), CodigoProyecto.ToString(), CodigoConvocatoria.ToString()); 
                    
                    gv_evaluacionindicadores.DataSource = sp_EvaluacionProyectoIndicador_SelectAll(CodigoProyecto, CodigoConvocatoria);
                    gv_evaluacionindicadores.DataBind();


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
                    EncabezadoEval.GetUltimaActualizacion();


                    Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
                }
                catch
                {

                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + bRepetido + "')", true);
                    return;
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

            }
            catch
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + bRepetido + "')", true);
                return;
            }
        }
    }
}