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

namespace Fonade.FONADE.interventoria
{
    public partial class AsignacionCoordinacionInterventores : Negocio.Base_Page
    {

        protected int CoordinadorIntervAsignado;

        protected static string CodCoordinador { get; set; }

        //Visible="false"
        protected void Page_Load(object sender, EventArgs e)
        {
            oculta_panel();
            lbl_Titulo.Text = void_establecerTitulo("ASIGNAR COORDINADORES A INTERVENTORES");
            if (!IsPostBack)
            {
                llenarInterventores();
            }
        }

        
        protected void llenarInterventores() 
        {         
            var query = from x in consultas.Db.MD_VerInterventoresActivos(Constantes.CONST_Interventor, usuario.CodOperador)
                        select new
                        {
                            nombre = x.nombre,
                            id_Contacto = x.Id_Contacto,
                        };
            ddl_evals.DataSource = query;
            ddl_evals.DataTextField = "nombre";
            ddl_evals.DataValueField = "Id_Contacto";
            ddl_evals.DataBind();
            ltitulo.Text = string.Format("Planes de negocio de {0}",ddl_evals.SelectedItem!=null?ddl_evals.SelectedItem.Text:string.Empty);
        }

        protected void lds_listadoproy_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
            var iuy = string.IsNullOrEmpty(ddl_evals.SelectedValue) ? "31547" : ddl_evals.SelectedValue;
                int CodInterventor = Convert.ToInt32(iuy);
                var query = from P in consultas.CargarProyectosInterv(Constantes.CONST_RolInterventor, CodInterventor, Constantes.CONST_RolInterventorLider)
                            select P;
                if (query.Count() == 0)
                {
                    Panelproyectos.Visible = false;
                    lmensajeproy.Visible = true;
                }
                else
                {
                    Panelproyectos.Visible = true;
                    lmensajeproy.Visible = false;
                }
                e.Result = query;
            }
            catch (Exception exc)
            {}
        }

        protected void lds_CoordAignado_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                int codigoInternventor = string.IsNullOrEmpty(ddl_evals.SelectedValue) ? 0 : Convert.ToInt32(ddl_evals.SelectedValue);

                var coordinadoresAsignados = (from contactos in consultas.Db.Contacto
                                             join coordinadores in consultas.Db.Interventors on contactos.Id_Contacto equals coordinadores.CodCoordinador
                                             where coordinadores.CodContacto == codigoInternventor
                                             select new
                                             {
                                                 Id = contactos.Id_Contacto,
                                                 Nombres = contactos.Nombres + " " + contactos.Apellidos,
                                                 Email = contactos.Email,
                                                 CodigoCoordinador = coordinadores.CodCoordinador
                                             }).ToList();

                e.Result = coordinadoresAsignados;

                var coordinadorAsignado = coordinadoresAsignados.Count != 0 ? coordinadoresAsignados.First().CodigoCoordinador : 0;
                CodCoordinador = coordinadorAsignado.ToString();

                llenarProgramas(coordinadorAsignado.ToString());
                PanellistadoCoordinadores.Visible = false;
            }
            catch (Exception exc)
            {}
        }

        protected void ddl_evals_SelectedIndexChanged(object sender, EventArgs e)
        {
            ltitulo.Text = "Planes de negocio de " + ddl_evals.SelectedItem.Text;
            DltEvaluacion.DataBind();
            gvcoorAsigando.DataBind();
            //CodCoordinador = Convert.ToInt32(ddl_evals.SelectedValue);
        }

        protected void btn_asignar_Click(object sender, EventArgs e)
        {
            llenarProgramas(CodCoordinador.ToString());
            PanellistadoCoordinadores.Visible = true;
        }

        protected void llenarProgramas(string coordinador)
        {
            rbl_coordinEval.ClearSelection();
            rbl_coordinEval.Items.Clear();
            rbl_coordinEval.Dispose();

            var query = from x in consultas.Db.MD_VerInterventoresActivos(Constantes.CONST_CoordinadorInterventor, usuario.CodOperador)
                        select new
                        {
                            nombreCoord = x.nombre,
                            id_coord = x.Id_Contacto,
                        };
            rbl_coordinEval.DataSource = query.ToList();
            rbl_coordinEval.DataTextField = "nombreCoord";
            rbl_coordinEval.DataValueField = "id_coord";
            rbl_coordinEval.DataBind();

            if (coordinador != "0")
            {
                if (rbl_coordinEval.Items.FindByValue(coordinador) != null)
                {
                    rbl_coordinEval.SelectedValue = coordinador;    
                }                
            }            
        }

        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_updateCoordIntervAsignado", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@codinterv", Convert.ToInt32(ddl_evals.SelectedValue));
                cmd.Parameters.AddWithValue("@CodCoordInterv", Convert.ToInt32(rbl_coordinEval.SelectedValue));
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                cmd2.Dispose();
                cmd.Dispose();
                gvcoorAsigando.DataBind();
                oculta_panel();
            }
            finally {
                con.Close();
                con.Dispose();
            }
        }
                
        protected void oculta_panel()
        {
            PanellistadoCoordinadores.Visible = false;
        }
    }
}