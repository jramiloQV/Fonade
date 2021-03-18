using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;


namespace Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal
{
    public partial class CatalogoAporteEvaluacion : Negocio.Base_Page
    {
        //Cadena de conexión
        private string conexionstr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        //variables 

        string nombre;
        string detalle;
        float solicitud;
        double recomendado;
        int tipodeaporte;
        int txtTab = Datos.Constantes.Const_AportesV2;
        int CodigoAporte
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["Aporte"]);
            }
            set { }
        }
        public int CodigoProyecto {
            get {
                    return Convert.ToInt32(Request.QueryString["codproyecto"]); }
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
        public int CodigoTab { get { return Constantes.Const_AportesV2; } set { } }
        public string Accion
        {
            get
            {
                return Request.QueryString["Accion"];
            }
            set { }
        }
        Consultas oConsultas = new Consultas();        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtsolicitado.Text = "0";
                txtsolicitado.ReadOnly = true;
                txtsolicitado.Enabled = true;
                lblfecha.Text = DateTime.Now.ToShortDateString();
                
                if (!string.IsNullOrEmpty(Accion))
                {
                    switch (Accion)
                    {
                        case "Nuevo":
                        case "Crear":
                            title.Text = "Crear Aporte";
                            txtsolicitado.Enabled = false;
                            break;
                        case "Actualizar":
                        case "Editar":
                            title.Text = "Modificar Aporte";
                            break;
                    }
                }
                                                
                dpl_tipo.DataSource = oConsultas.Db.TipoIndicadorGestions.ToList();
                dpl_tipo.DataTextField = "nomTipoIndicador";
                dpl_tipo.DataValueField = "Id_TipoIndicador";
                dpl_tipo.DataBind();

                if (CodigoAporte > 0)
                {
                    if (Accion != "eliminar")
                    {                        
                        MD_EvaluacionProyectoAporte_SelectRow(CodigoAporte);

                    }
                    else
                    {                        
                        MD_EvaluacionProyectoAporte_DeleteRow(CodigoAporte);
                        Redireccionar("Registro Eliminado Exitosamente");
                    }
                }
                else
                {
                    btn_crearaporte.Text = "Crear";
                }               
            }
        }

        protected void btn_crearaporte_Click(object sender, EventArgs e)
        {
            nombre = TxtNombre.Text;
            recomendado = Convert.ToDouble(!string.IsNullOrEmpty(txtRecomendado.Text) ? Convert.ToDecimal(txtRecomendado.Text) : 0);
            tipodeaporte = int.Parse(dpl_tipo.SelectedValue);
            detalle = txt_detalle.Text;
            solicitud = float.Parse(txtsolicitado.Text);

            if (CodigoAporte > 0)
            {                
                MD_EvaluacionProyectoAporte_Update(CodigoAporte, CodigoProyecto, CodigoConvocatoria, tipodeaporte, nombre, detalle, solicitud, recomendado, false);
                Redireccionar("Registro Actualizado Exitosamente!");
            }
            else
            {
                MD_EvaluacionProyectoAporte_Insert(CodigoProyecto, CodigoConvocatoria, tipodeaporte, nombre, detalle, solicitud, recomendado, false);
                Redireccionar("Registro Creado Exitosamente!");
            }
        }

        public void Redireccionar(string mensaje)
        {            
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.opener.location.reload();window.close();", true);
        }
        // #aporte

        //Crear Indicador x id
        public int MD_EvaluacionProyectoAporte_Insert(int CodProyecto, int CodConvocatoria, int CodTipoIndicador, string Nombre, string Detalle, float Solicitado, double Recomendado, Boolean Protegido)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_EvaluacionProyectoAporte_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodProyecto", CodProyecto);
            cmd.Parameters.AddWithValue("@CodConvocatoria", CodConvocatoria);
            cmd.Parameters.AddWithValue("@CodTipoIndicador", CodTipoIndicador);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Detalle", Detalle);
            cmd.Parameters.AddWithValue("@Solicitado", Solicitado);
            cmd.Parameters.AddWithValue("@Recomendado", Recomendado);
            cmd.Parameters.AddWithValue("@Protegido", Protegido);

            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();

                cmd.Dispose();
                UpdateTab();
                return id;
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                return 0;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
        
        public int MD_EvaluacionProyectoAporte_Update(int Id_Aporte, int CodProyecto, int CodConvocatoria, int CodTipoIndicador, string Nombre, string Detalle, float Solicitado, double Recomendado, Boolean Protegido)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_EvaluacionProyectoAporte_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id_Aporte", Id_Aporte);
            cmd.Parameters.AddWithValue("@CodProyecto", CodProyecto);
            cmd.Parameters.AddWithValue("@CodConvocatoria", CodConvocatoria);
            cmd.Parameters.AddWithValue("@CodTipoIndicador", CodTipoIndicador);
            cmd.Parameters.AddWithValue("@Nombre", Nombre);
            cmd.Parameters.AddWithValue("@Detalle", Detalle);
            cmd.Parameters.AddWithValue("@Solicitado", Solicitado);
            cmd.Parameters.AddWithValue("@Recomendado", Recomendado);
            cmd.Parameters.AddWithValue("@Protegido", Protegido);

            try
            {
                con.Open();
                int id = (int)cmd.ExecuteScalar();

                cmd.Dispose();
                UpdateTab();
                return id;
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
                return 0;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }
                
        public List<EvaluacionProyectoAporte> MD_EvaluacionProyectoAporte_SelectRow(int Id_Aporte)
        {
            List<EvaluacionProyectoAporte> lst = null;
            if (Id_Aporte != 0)
            {
                lst = oConsultas.Db.EvaluacionProyectoAportes.Where(a => a.Id_Aporte == Id_Aporte).ToList();

                if (lst.Any())
                {
                    foreach (var dr_aporte in lst)
                    {
                        TxtNombre.Text = dr_aporte.Nombre;
                        txt_detalle.Text = dr_aporte.Detalle;
                        txtsolicitado.Text = dr_aporte.Solicitado.ToString();
                        txtRecomendado.Text = dr_aporte.Recomendado.ToString();
                        dpl_tipo.SelectedValue = dr_aporte.CodTipoIndicador.ToString();
                        btn_crearaporte.Text = "Actualizar";
                    }

                    TxtNombre.Enabled = false;
                    txtsolicitado.ReadOnly = true;
                    dpl_tipo.Enabled = false;
                }

            }
            return lst;
        }

        public void MD_EvaluacionProyectoAporte_DeleteRow(int Id_Aporte)
        {
            using (var con = new SqlConnection(conexionstr))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_EvaluacionProyectoAporte_DeleteRow";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    // Validar que no guarde espacios en blanco
                    com.Parameters.AddWithValue("@Id_Aporte", Id_Aporte);
                    try
                    {
                        con.Open();
                        com.ExecuteNonQuery();
                        UpdateTab();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        com.Dispose();
                        con.Close();
                        con.Dispose();
                    }
                }
            }

        }

        //Consultar tipo indicador x id
        public DataTable MD_TipoIndicadorGestion_SelectAll()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_TipoIndicadorGestion_SelectAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new DataTable();

                }
                return null;
            }
        }
                
        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.opener.location.reload();window.close();", true);
        }

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)CodigoTab,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
            Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");         
        }
    }
}