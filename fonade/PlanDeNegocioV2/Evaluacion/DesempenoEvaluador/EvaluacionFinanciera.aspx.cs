using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.DesempenoEvaluador
{
    public partial class EvaluacionFinanciera : Negocio.Base_Page
    {
        #region variables globales

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
        public int txtTab = Constantes.CONST_RolEvaluador;       

        public String Aspecto
        {
            get
            {
                return Request.QueryString["codaspecto"];
            }
            set { }
        }

        DataTable datatable;
        DataTable listaGrilla;

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {                                                                                       
            Int32 total = 0;

            foreach (GridViewRow gvr in GV_Item.Rows)
            {
                try
                {
                    total += Convert.ToInt32(((DropDownList)gvr.FindControl("DDL_Puntaje")).SelectedValue.ToString());
                }
                catch (FormatException) { }
            }

            L_TotalPuntajeObtenido.Text = "" + total;

            Encabezado();

            if (!IsPostBack)
            {
                #region valida la session

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                {
                    GV_Item.Columns[0].Visible = true;


                    foreach (GridViewRow dr in GV_Item.Rows)
                    {
                        LinkButton ddl = ((LinkButton)dr.FindControl("LB_EditarItem"));
                        ddl.Enabled = true;
                    }

                    btn_Actualizar.Visible = true;
                    btn_Actualizar.Enabled = true;
                }
                else
                {
                    GV_Item.Columns[0].Visible = false;

                    foreach (GridViewRow dr in GV_Item.Rows)
                    {
                        LinkButton ddl = ((LinkButton)dr.FindControl("LB_EditarItem"));
                        ddl.Enabled = false;
                    }

                    btn_Actualizar.Visible = false;
                    btn_Actualizar.Enabled = false;
                }

                if (usuario.CodGrupo != Constantes.CONST_CoordinadorEvaluador)
                {
                    foreach (GridViewRow dr in GV_Item.Rows)
                    {
                        DropDownList ddl = ((DropDownList)dr.FindControl("DDL_Puntaje"));
                        ddl.Enabled = false;
                    }
                }

                if (usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
                {
                    pnladicionar.Visible = false;
                }

                #endregion
            }
        }

        private void Encabezado()
        {            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());           
            SqlCommand cmd = new SqlCommand("SELECT TOP 1([Nombres] + ' ' + [Apellidos]) AS NOMBRE FROM [Contacto] INNER JOIN [ProyectoContacto] ON [CodContacto] = [Id_Contacto] AND [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria + " AND [CodRol] = " + txtTab + " AND ([FechaFin] IS NULL OR [FechaFin] = (SELECT MAX([FechaFin]) FROM [ProyectoContacto] WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria + " AND [CodRol] = " + txtTab + "))ORDER BY [FechaFin]", conn);

            try
            {
                
                conn.Open();
                
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    L_Nombre.Text = reader["NOMBRE"].ToString();
                }
                reader.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void LB_EliminarItem_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow filaGV = GV_Item.Rows[indicefila];
            Int32 Id_Item = Int32.Parse(GV_Item.DataKeys[filaGV.RowIndex].Value.ToString());

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM [EvaluacionEvaluador] WHERE [CodProyecto] = " + CodigoProyecto + " AND [CodConvocatoria] = " + CodigoConvocatoria + " AND [CodItem] = " + Id_Item, conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                cmd = new SqlCommand("DELETE FROM [ItemEscala] WHERE [CodItem] = " + Id_Item, conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();

                cmd = new SqlCommand("DELETE FROM [Item] WHERE [Id_Item] = " + Id_Item, conn);
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            Response.Redirect(Request.RawUrl);

        }

        public void Editar()
        {

        }

        protected void LB_EditarItem_Click(object sender, EventArgs e)
        {

        }

        protected void GV_Item_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Item.PageIndex = e.NewPageIndex;
            GV_Item.DataBind();
        }

        protected void btn_Actualizar_Click(object sender, EventArgs e)
        {
           
            foreach (GridViewRow dr in GV_Item.Rows)
            {
                
                int indice = Convert.ToInt32(GV_Item.DataKeys[dr.RowIndex].Values[0].ToString());

               
                DropDownList ddl = ((DropDownList)dr.FindControl("DDL_Puntaje"));

                
                string puntaje = ddl.SelectedValue;

                
                string txtSQL = "update evaluacionevaluador set Puntaje=" + puntaje +
                    " where CodProyecto=" + CodigoProyecto + " and CodConvocatoria=" + CodigoConvocatoria +
                    " and CodItem = " + indice;
                
                ejecutaReader(txtSQL, 2);
            }

            Encabezado();
        }

        protected void lds_item_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            var result = (from i in consultas.Db.Items
                          from ee in consultas.Db.EvaluacionEvaluadors
                          orderby i.Id_Item
                          where i.Id_Item == ee.CodItem
                          && ee.CodProyecto == Convert.ToInt32(CodigoProyecto)
                          && ee.CodConvocatoria == Convert.ToInt32(CodigoConvocatoria)
                          && i.CodTabEvaluacion == short.Parse(Aspecto)
                          select new
                          {
                              i.Id_Item,
                              i.NomItem,
                              i.Protegido,
                              ee.Puntaje
                          });

            e.Result = result.ToList();
        }
       
        protected void lds_ddl_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }
        
        protected void GV_Item_RowCreated(object sender, GridViewRowEventArgs e)
        {            
            try
            {        
                var result = (from ie in consultas.Db.ItemEscalas
                              orderby ie.Puntaje descending
                              where ie.CodItem == Convert.ToInt32(GV_Item.DataKeys[e.Row.RowIndex].Values[0].ToString())
                              select new
                              {
                                  ie.Texto,
                                  ie.Puntaje
                              });

                ((DropDownList)e.Row.FindControl("DDL_Puntaje")).DataSource = result;
                ((DropDownList)e.Row.FindControl("DDL_Puntaje")).DataTextField = "Texto";
                ((DropDownList)e.Row.FindControl("DDL_Puntaje")).DataValueField = "Puntaje";
                string wprote = e.Row.DataItem.Equals("Protegido").ToString();
                if (wprote == "false")
                {
                    ((DropDownList)e.Row.FindControl("DDL_Puntaje")).Enabled = false;
                    ((LinkButton)e.Row.FindControl("LB_EliminarItem")).Visible = false;
                }
                else
                {
                    ((DropDownList)e.Row.FindControl("DDL_Puntaje")).Enabled = true;
                    ((LinkButton)e.Row.FindControl("LB_EliminarItem")).Visible = true;

                }
                
                ((DropDownList)e.Row.FindControl("DDL_Puntaje")).SelectedValue = GV_Item.DataKeys[e.Row.RowIndex].Values[1].ToString();
            }
            catch (Exception) { }
        }
    }
}