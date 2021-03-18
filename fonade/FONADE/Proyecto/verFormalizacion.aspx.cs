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

namespace Fonade.FONADE.Proyecto
{
    public partial class verFormalizacion : Negocio.Base_Page
    {
        public string codProyecto;
        protected void Page_Load(object sender, EventArgs e)
        {
            lbl_Titulo.Text = void_establecerTitulo("DOCUMENTO DE FORMALIZACIÓN");
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            codProyecto = Request.QueryString["codProyecto"].ToString();
            //codProyecto = "44432";
            insertarDatos();
            InsertAvalyObserv();
        }

        protected void lds_proyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                int casoAsesor = 2;
                var query = from P in consultas.VerFormalizacion(Convert.ToInt32(codProyecto), Constantes.CONST_RolAsesor, Constantes.CONST_RolAsesorLider, Constantes.CONST_RolEmprendedor, casoAsesor)
                            select P;
                e.Result = query;
                 
            }
            catch (Exception)
            {}

        }

        protected void lds_proyectos_SelectingEmprend(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                lds_proyectos.Dispose();
                var query = from P in consultas.VerFormalizacionEmprendedor(Convert.ToInt32(codProyecto), Constantes.CONST_RolEmprendedor)
                            select P;
                e.Result = query;
            }
            catch (Exception)
            {}
        }

        protected void lds_proyectos_SelectingEmpleo(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                lds_proyectos.Dispose();
                var query = from P in consultas.VerFormalizacionEmpleos(Convert.ToInt32(codProyecto))
                            select P;
                e.Result = query;
            }
            catch (Exception)
            {}
        }
        private void insertarDatos()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                
                SqlCommand cmd = new SqlCommand("MD_VerFormalizacionQuerys", con);
                SqlDataReader r;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proyecto", Convert.ToInt32(codProyecto));
                cmd.Parameters.AddWithValue("@CONST_RolAsesor", Constantes.CONST_RolAsesor);
                cmd.Parameters.AddWithValue("@CONST_RolAsesorLider", Constantes.CONST_RolAsesorLider);
                cmd.Parameters.AddWithValue("@CONST_RolEmprendedor", Constantes.CONST_RolEmprendedor);
                cmd.Parameters.AddWithValue("@caso", 1);
                con.Open();
                r = cmd.ExecuteReader();
                r.Read();
                LabelCiudad.Text = Convert.ToString(r["nomCiudadQ1"]);
                LabelEstado.Text = Convert.ToString(r["nomEstadoQ1"]);
                LabelFechaCreac.Text = Convert.ToDateTime(r["FechaCreacionQ1"]).ToString("dd 'de' MMMM 'de' yyyy");
                LabelID.Text = Convert.ToString(r["idproyectoQ1"]);
                LabelNombre.Text = Convert.ToString(r["nomProyectoQ1"]);
                LabelSector.Text = Convert.ToString(r["nomSectorQ1"]);
                LabelSumario.Text = Convert.ToString(r["sumarioQ1"]);
                LabelTipoP.Text = Convert.ToString(r["nomTipoProyectoQ1"]);
                LabelIdent2.Text = Convert.ToString(r["nomTipoIdentificacionQ1"] + " N° " + r["IdentificacionQ1"]);
                LabelInsti2.Text = Convert.ToString(r["nomInstitucionQ1"]);
                LabelJefe2.Text = Convert.ToString(r["nombresQ1"] + " " + r["apellidosQ1"]);
                LabelUnidad2.Text = Convert.ToString(r["nomUnidadQ1"]);
                LabelCluster3.Text = Convert.ToString(r["ClusterQ1"]);
                LabelPN3.Text = Convert.ToString(r["PlanNacionalQ1"]);
                LabelPR3.Text = Convert.ToString(r["PlanRegionalQ1"]);
                //con.Close();
                //con.Dispose();
                cmd.Dispose();
            }
            catch (SqlException se) { }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        private void InsertAvalyObserv()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                
                SqlCommand cmd = new SqlCommand("MD_VerFormalizacionQuerys", con);
                SqlDataReader r;
                cmd.CommandText = "MD_VerFormalizacionQuerys";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proyecto", Convert.ToInt32(codProyecto));
                cmd.Parameters.AddWithValue("@CONST_RolAsesor", Constantes.CONST_RolAsesor);
                cmd.Parameters.AddWithValue("@CONST_RolAsesorLider", Constantes.CONST_RolAsesorLider);
                cmd.Parameters.AddWithValue("@CONST_RolEmprendedor", Constantes.CONST_RolEmprendedor);
                cmd.Parameters.AddWithValue("@caso", 5);
                con.Open();
                r = cmd.ExecuteReader();
                r.Read();
                LabelAvalQ5.Text = Convert.ToString(r["AvalQ5"]);
                LabelObservacionesQ5.Text = Convert.ToString(r["observacionesQ5"]);
                //con.Close();
                //con.Dispose();
                cmd.Dispose();
            }
            catch (Exception)
            { }
            finally {
                con.Close();
                con.Dispose();
            }
        }

    }
}