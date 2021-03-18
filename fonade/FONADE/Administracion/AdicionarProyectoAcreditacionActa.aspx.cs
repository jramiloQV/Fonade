using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// AdicionarProyectoAcreditacionActa
    /// </summary>    
    public partial class AdicionarProyectoAcreditacionActa : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Código del acta.
        /// </summary>
        String CodActa;

        /// <summary>
        /// Convocatoria del acta seleccionada.
        /// </summary>
        String CodConvocatoria;

        // <summary>
        // Entero (procesado de un booleano). Determina si el acta está o no acreditada.
        // </summary>
        //Int32 strEstadoAcreditacion;

        /// <summary>
        /// Contiene las consultas de SQL.
        /// </summary>
        String txtSQL;

        bool idEstadoAcreditacion;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodActa = HttpContext.Current.Session["bActaAcreditada"] != null ? CodActa = HttpContext.Current.Session["bActaAcreditada"].ToString() : "0";
            CodConvocatoria = HttpContext.Current.Session["CodConvocatoria_Acta"] != null ? CodConvocatoria = HttpContext.Current.Session["CodConvocatoria_Acta"].ToString() : "0";
            if (Session["idActaActreditada"] != null)
            {
                idEstadoAcreditacion = Convert.ToBoolean(Session["idActaActreditada"].ToString());
            }
            
            //strEstadoAcreditacion = HttpContext.Current.Session["bActaAcreditada"] != null ? strEstadoAcreditacion = Convert.ToInt32(HttpContext.Current.Session["bActaAcreditada"].ToString()) : 0;

            if (!IsPostBack)
            {
                if (CodActa == "0" && CodConvocatoria == "0")
                { }//System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); return; }

                //Cargar la grilla de planes seleccionables.
                CargarPlanesSeleccionables("", "", "");
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/07/2014.
        /// Método que carga los planes de negocio para seleccionar.
        /// </summary>
        /// <param name="filtro">Del DropDownList.</param>
        /// <param name="strBuscarNombre">Nombre digitado.</param>
        /// <param name="strBuscarId">Id digitado.</param>
        private void CargarPlanesSeleccionables(String filtro, String strBuscarNombre, String strBuscarId)
        {
            //Inicializar variables.
            DataTable rs = new DataTable();
            // 2015/06/10 Roberto Alvarado
            // Se comentan estas 2 variables pues ya no son necesarias
            //strBuscarNombre = " AND UPPER(nomproyecto) like '%" + strBuscarNombre + "%' ";
            //strBuscarId = " AND Id_proyecto = '" + strBuscarId + "' ";

            try
            {
                var criterios = string.Empty;
                if (!string.IsNullOrEmpty(txtBuscarId.Text) || !string.IsNullOrEmpty(txtBuscar.Text) || HttpContext.Current.Session["upper_letter_selected"] != null)
                {
                    if (HttpContext.Current.Session["upper_letter_selected"] != null)
                    {
                        criterios = " and UPPER(p.nomproyecto) Like '" + HttpContext.Current.Session["upper_letter_selected"].ToString() + "%' ";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtBuscarId.Text) && !string.IsNullOrEmpty(txtBuscar.Text))
                        {
                            criterios = " and p.id_Proyecto = " + txtBuscarId.Text + " and p.nomproyecto ='" + txtBuscar.Text + "'";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(txtBuscarId.Text) && string.IsNullOrEmpty(txtBuscar.Text))
                            {
                                criterios = " and p.id_Proyecto = " + txtBuscarId.Text;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(txtBuscarId.Text) && !string.IsNullOrEmpty(txtBuscar.Text))
                                {
                                    criterios = "  and p.nomproyecto ='" + txtBuscar.Text + "'";
                                }
                            }
                        }
                    }
                }
                else
                {
                    criterios = "";
                }

                var codEstado = string.Empty;
                if(idEstadoAcreditacion)
                {
                    codEstado = Constantes.CONST_Aprobacion_Acreditacion.ToString();
                }
                else
                {
                    codEstado = Constantes.CONST_Aprobacion_No_Acreditacion.ToString();
                }
                //var sqlQuery = "Select t.id_Proyecto, t.nomproyecto, t.CodEstado  from (";
                //sqlQuery += "Select p.id_Proyecto, p.nomproyecto, p.CodEstado, aap.codActa from ConvocatoriaProyecto cp ";
                //sqlQuery += "Left join AcreditacionActaProyecto aap on aap.codProyecto = cp.codProyecto ";
                //sqlQuery += "Inner join proyecto p on p.id_proyecto = cp.codProyecto ";
                //sqlQuery += "where cp.codConvocatoria = " + CodConvocatoria + " and p.codestado = " + codEstado + " " + criterios +") t Where t.codActa Is NULL";

                var sqlQuery = "select id_proyecto, nomproyecto, CodEstado from proyecto inner join ConvocatoriaProyecto on  Id_Proyecto=Codproyecto  where id_Proyecto not in (select CodProyecto from AcreditacionActaProyecto AAP inner join AcreditacionActa AA on AA.Id_Acta = AAP.CodActa where AA.codConvocatoria= " + CodConvocatoria + ") and codestado in (" + codEstado + ") and codconvocatoria = " + CodConvocatoria + " group by id_proyecto,nomproyecto, CodEstado";

                //rs = consultas.ObtenerDataTable(sbQuery.ToString(), "text");
                rs = consultas.ObtenerDataTable(sqlQuery, "text");

                // 2015/06/26 Roberto Alvarado
                // Se adiciona el Id_Proyecto par poder obtenerlo de una manera mas comoda posteiormente
                gvPlanesNegocio.DataKeyNames = new string[] { "id_proyecto" };

                gvPlanesNegocio.DataSource = rs;
                gvPlanesNegocio.DataBind();
            }
            catch
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al cargar los planes de negocio seleccionables.')", true);
                //return;
            }
        }

        #region Métodos de selección del abecedario.

        /// <summary>
        /// 2015/06/10  Roberto Alvarado
        /// Al seleccionar cualquiera de los LinkButton, se establecerá el valor a la propiedad Text o "%" si es Todos a
        /// la variable de sesión "upper_letter_selected".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtn_opcion_todos_Click(object sender, EventArgs e)
        {
            LinkButton obj = (LinkButton)sender;
            String letra = obj.Text;
            if (letra == "Todos")
                letra = "%";
            //HttpContext.Current.Session["upper_letter_selected"] = "%";

            HttpContext.Current.Session["upper_letter_selected"] = letra;
            CargarPlanesSeleccionables(HttpContext.Current.Session["upper_letter_selected"].ToString(), "", "");
        }

        

        #endregion

        /// <summary>
        /// Buscar planes de negocio seleccionables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_buscar_Click(object sender, EventArgs e)
        {
            CargarPlanesSeleccionables("", txtBuscar.Text.Trim(), txtBuscarId.Text.Trim());
        }

        /// <summary>
        /// Chequear todos los planes de negocio seleccionables.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chectodos_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow fila in gvPlanesNegocio.Rows)
            {
                CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                if (box.Enabled)
                    box.Checked = chectodos.Checked;
            }
        }

        /// <summary>
        /// Adicionar el(los) plan(es) de negocio seleccionado(s) al acta seleccionada en "AcreditacionActa.aspx".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Adicionar_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            ClientScriptManager cm = this.ClientScript;
            DataTable rs = new DataTable();
            int Estado = 0;
            var idActaAcreditacion = string.Empty;
            if(Session["idActaAcreditacion"].ToString() != null)
            {
                idActaAcreditacion = Session["idActaAcreditacion"].ToString();
            }

            foreach (GridViewRow fila in gvPlanesNegocio.Rows)
            {
                CheckBox box = (CheckBox)fila.FindControl("chckplanopera");
                HiddenField hdf = (HiddenField)fila.FindControl("hdf_proyecto");

                // 2015/06/26 Roberto Alvarado
                // Se obtiene el Id_Proyecto de esta manera que si se obtiene el valor, ya que en hdf esta siempre como '' 
                string idProyecto = gvPlanesNegocio.DataKeys[fila.RowIndex].Value.ToString();

                if (box != null && hdf != null)
                {
                    if (box.Checked)
                    {
                        //txtSQL = "select CodEstado from proyecto where Id_proyecto = '" + hdf.Value + "'";
                        txtSQL = "select CodEstado from proyecto where Id_proyecto = '" + idProyecto + "'";
                        rs = consultas.ObtenerDataTable(txtSQL, "text");

                        if (rs.Rows.Count > 0)
                        {
                            var cosEstado = rs.Rows[0]["CodEstado"].ToString();
                            if (!String.IsNullOrEmpty(cosEstado))
                            {
                                //if (Int32.Parse(rs.Rows[0]["Estado"].ToString()) == Constantes.CONST_Aprobacion_Acreditacion)
                                if (Int32.Parse(cosEstado) == Constantes.CONST_Aprobacion_Acreditacion)
                                {
                                    Estado = 1;
                                }
                            }
                        }
                        rs = null;

                        //txtSQL = " insert into AcreditacionActaproyecto values (" + CodActa + ", " + hdf.Value + ", " + Estado + ")";
                        txtSQL = " insert into AcreditacionActaproyecto values (" + idActaAcreditacion + ", " + idProyecto + ", " + Estado + ")";
                        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                        try
                        {
                            //NEW RESULTS:
                            
                            SqlCommand cmd = new SqlCommand(txtSQL, con);

                            if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                        catch (SqlException ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al agregar los planes de negocio al acta.')", true);
                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }
                }
            }

            //Recargar la página padre y cerrar la ventana actual.
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location=window.opener.location;window.close();</script>");
        }
    }
}