#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 07 - 2014</Fecha>
// <Archivo>EvaluacionFinanciera.cs</Archivo>

#endregion

#region using

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

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionFinanciera : Negocio.Base_Page
    {
        #region variables globales

        public String codProyecto;
        public String codConvocatoria;
        public int txtTab = Constantes.CONST_RolEvaluador;
        public String Aspecto;

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
            #region recoge datos de session

            try
            {

                Aspecto = Request["txtTab"];
                HttpContext.Current.Session["txtTabAspecto"] = Aspecto;

                codProyecto = HttpContext.Current.Session["codProyecto"].ToString();
                HttpContext.Current.Session["codProyectoval"] = codProyecto;
                codConvocatoria = HttpContext.Current.Session["codConvocatoria"].ToString();
            }
            catch (Exception ex) {
                String ex1 = ex.Message;
            }

            #endregion

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

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// pone un encabezado en la pagina
        /// colocando el nombre del usuario logeado
        /// </summary>
        private void Encabezado()
        {
            //crea un objeto para la conexion a la BD
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            //sentencia que trae la informacion requerida de la BD
            //y crea un comando que permiter la conexion y la ejecucion de la consulta
            SqlCommand cmd = new SqlCommand("SELECT TOP 1([Nombres] + ' ' + [Apellidos]) AS NOMBRE FROM [Contacto] INNER JOIN [ProyectoContacto] ON [CodContacto] = [Id_Contacto] AND [CodProyecto] = " + codProyecto + " AND [CodConvocatoria] = " + codConvocatoria + " AND [CodRol] = " + txtTab + " AND ([FechaFin] IS NULL OR [FechaFin] = (SELECT MAX([FechaFin]) FROM [ProyectoContacto] WHERE [CodProyecto] = " + codProyecto + " AND [CodConvocatoria] = " + codConvocatoria + " AND [CodRol] = " + txtTab + "))ORDER BY [FechaFin]", conn);

            try
            {
                //abre la conexion
                conn.Open();
                //tra a un reader el resultado de la consulta
                SqlDataReader reader = cmd.ExecuteReader();

                //valida si trajo resultados
                if (reader.Read())
                {
                    //mustra la informacion dentro de un label
                    L_Nombre.Text = reader["NOMBRE"].ToString();
                }
                //rompe el libro
                reader.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                //cierra la conexion
                conn.Close();
                conn.Dispose();
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// elimina el item de BD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LB_EliminarItem_Click(object sender, EventArgs e)
        {
            var indicefila = ((GridViewRow)((Control)sender).NamingContainer).RowIndex;
            GridViewRow filaGV = GV_Item.Rows[indicefila];
            Int32 Id_Item = Int32.Parse(GV_Item.DataKeys[filaGV.RowIndex].Value.ToString());

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            try
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM [EvaluacionEvaluador] WHERE [CodProyecto] = " + codProyecto + " AND [CodConvocatoria] = " + codConvocatoria + " AND [CodItem] = " + Id_Item, conn);
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

            //Encabezado();

            //buscarItem();

            //lista();
        }

        public void Editar()
        {

        }

        protected void LB_EditarItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// realiza el paginado de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Item_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_Item.PageIndex = e.NewPageIndex;
            GV_Item.DataBind();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// metodo que actualiza los items de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Actualizar_Click(object sender, EventArgs e)
        {
            //recorre la grilla
            foreach (GridViewRow dr in GV_Item.Rows)
            {
                //recoge el indice del item que se encuentra en el datakey de la grilla
                int indice = Convert.ToInt32(GV_Item.DataKeys[dr.RowIndex].Values[0].ToString());

                //busca el dropdownlist de la fila
                DropDownList ddl = ((DropDownList)dr.FindControl("DDL_Puntaje"));

                //obtiene el puntaje seleccionado para actualizar
                string puntaje = ddl.SelectedValue;

                //string que carga el sql de la actualizacion que se realiza al item
                string txtSQL = "update evaluacionevaluador set Puntaje=" + puntaje +
                    " where CodProyecto=" + codProyecto + " and CodConvocatoria=" + codConvocatoria +
                    " and CodItem = " + indice;
                //realiza la actualizacion
                ejecutaReader(txtSQL, 2);
            }

            Encabezado();
        }

        /// <summary>
        /// Diego QUiñonez
        /// 16 - 07 - 2014
        /// metodo que carga todos los item y sus puntajes en la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_item_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            var result = (from i in consultas.Db.Items
                          from ee in consultas.Db.EvaluacionEvaluadors
                          orderby i.Id_Item
                          where i.Id_Item == ee.CodItem
                          && ee.CodProyecto == Convert.ToInt32(codProyecto)
                          && ee.CodConvocatoria == Convert.ToInt32(codConvocatoria)
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

        /// <summary>
        /// Diego Quiñonez
        /// 16 - 07 - 2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_ddl_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

        }

        /// <summary>
        /// Diego QUiñonez
        /// 16 - 07 - 2014
        /// metodo disparado al crear las lineas de la grilla
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GV_Item_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //SELECT [Texto], [Puntaje] FROM [ItemEscala] WHERE [CodItem] = " + GV_Item.DataKeys[i].Value.ToString() + " ORDER BY [Puntaje] DESC
            try
            {
                //de acuerdo al id del item de cada fila
                //busca la lista de escalas del item
                //y las asigna al dropdownlist ddl_puntaje
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
                else {
                    ((DropDownList)e.Row.FindControl("DDL_Puntaje")).Enabled = true ;
                    ((LinkButton)e.Row.FindControl("LB_EliminarItem")).Visible = true;

                }


                //selecciona de acuerdo al puntaje de la fila
                ((DropDownList)e.Row.FindControl("DDL_Puntaje")).SelectedValue = GV_Item.DataKeys[e.Row.RowIndex].Values[1].ToString();
            }
            catch (Exception) { }
        }
    }
}