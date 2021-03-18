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
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.IO;
using System.Text;
using System.Globalization;
using System.Web.UI.HtmlControls;


namespace Fonade.FONADE.Auditoria
{
    /// <summary>
    /// AuditoriaReportes
    /// </summary>    
    public partial class AuditoriaReportes : Negocio.Base_Page
    {
        #region Eventos        
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Descarga"] !=null)
            {
                lnkDescargar.NavigateUrl = Session["Descarga"].ToString();
                lnkDescargar.Visible = true;
                btn_generareporte.Visible = false;
                Session["Descarga"] = null;
            }
            lbl_Titulo.Text = void_establecerTitulo("REPORTES DE AUDITORÍA");
            if (!IsPostBack)
            {
                CargarCombo();
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlTablas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlTablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarGrid();
        }

        /// <summary>
        /// Handles the Click event of the btn_generareporte control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_generareporte_Click(object sender, EventArgs e)
        {
            GenerarReporte();
        }

        /// <summary>
        /// Handles the RowDataBound event of the grvCriterios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void grvCriterios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            VisibilidadGrid(e);
        }
        #endregion

        #region Metodos
        private void CargarCombo()
        {

            ddlTablas.DataSource = (from re in consultas.Db.ReporteAuditoria
                                    select new
                                    {
                                        nomrbes = re.Tabla
                                    }).Distinct().OrderBy(x => x.nomrbes);

            ddlTablas.DataValueField = "nomrbes";
            ddlTablas.DataTextField = "nomrbes";
            ddlTablas.DataBind();
            ddlTablas.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        private void CargarGrid()
        {
            var criterios = (from ra in consultas.Db.ReporteAuditoria
                             where ra.Tabla == ddlTablas.SelectedValue
                             select ra).ToList();
            grvCriterios.DataSource = criterios;
            grvCriterios.DataBind();
            lblNotas.Visible = true;
            btn_generareporte.Visible = true;
        }

        private void VisibilidadGrid(GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                var lblCampo = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblCampo");
                var lblTipodato = (System.Web.UI.WebControls.Label)e.Row.FindControl("lblTipoDato");
                var pnlTexto = (System.Web.UI.WebControls.Panel)e.Row.FindControl("pnlTexto");
                var txtCondicion = (System.Web.UI.WebControls.TextBox)e.Row.FindControl("txtCondicion");
                var chkPorcenIni = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("chkPorcenInico");
                var chkPorcenFin = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("chkPorcenFin");
                var pnlFechas = (System.Web.UI.WebControls.Panel)e.Row.FindControl("pnlFechas");
                var chkSino = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("chkCondicion");

                switch(lblTipodato.Text)
                {
                    //Numericos
                    case "int":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "smallint":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "float":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "money":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "tinyint":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "decimal":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "smallmoney":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "bigint":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    case "real":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    //Texto
                    case "varchar":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        chkPorcenIni.Visible = true;
                        chkPorcenFin.Visible = true;
                        break;
                    case "text":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        chkPorcenIni.Visible = true;
                        chkPorcenFin.Visible = true;
                        break;
                    case "nvarchar":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        chkPorcenIni.Visible = true;
                        chkPorcenFin.Visible = true;
                        break;
                    case "ntext":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        chkPorcenIni.Visible = true;
                        chkPorcenFin.Visible = true;
                        break;
                    case "char":
                        pnlTexto.Visible = true;
                        txtCondicion.Visible = true;
                        break;
                    //Fecha
                    case "datetime":
                        pnlFechas.Visible = true;
                        break;
                    case "smalldatetime":
                        pnlFechas.Visible = true;
                        break;
                    case "date":
                        pnlFechas.Visible = true;
                        break;
                    //Si/No
                    case "bit":
                        chkSino.Visible = true;
                        break;
                }
            }
        }

        private void GenerarReporte()
        {
            var query = string.Empty;

            query = "Select * from " + ddlTablas.SelectedValue + " Where ";

            foreach(GridViewRow fila in grvCriterios.Rows)
            {
                var lblCampo = (System.Web.UI.WebControls.Label)fila.FindControl("lblCampo");
                var lblTipodato = (System.Web.UI.WebControls.Label)fila.FindControl("lblTipoDato");
                var pnlTexto = (System.Web.UI.WebControls.Panel)fila.FindControl("pnlTexto");
                var txtCondicion = (System.Web.UI.WebControls.TextBox)fila.FindControl("txtCondicion");
                var chkPorcenIni = (System.Web.UI.WebControls.CheckBox)fila.FindControl("chkPorcenInico");
                var chkPorcenFin = (System.Web.UI.WebControls.CheckBox)fila.FindControl("chkPorcenFin");
                var txtFechaInicio = (System.Web.UI.WebControls.TextBox)fila.FindControl("dteFechaInicio");
                var txtFechaFin = (System.Web.UI.WebControls.TextBox)fila.FindControl("dteFechaFin");
                var chkSino = (System.Web.UI.WebControls.CheckBox)fila.FindControl("chkCondicion");
                var union = (RadioButtonList)fila.FindControl("rbnAndOr");

                if(!string.IsNullOrEmpty(txtCondicion.Text))
                {
                    if(lblTipodato.Text == "int" || lblTipodato.Text == "smallint" || lblTipodato.Text == "float" ||
                        lblTipodato.Text == "money" || lblTipodato.Text == "tinyint" || lblTipodato.Text == "decimal" || lblTipodato.Text == "smallmoney" || 
                        lblTipodato.Text == "bigint" || lblTipodato.Text == "real")
                    {
                        query += lblCampo.Text + " = " + txtCondicion.Text + " ";
                        foreach(ListItem item in union.Items)
                        {
                            if(item.Selected)
                            {
                                query += item.Value + " ";
                            }
                        }
                    }

                    if (lblTipodato.Text == "varchar" || lblTipodato.Text == "text" || lblTipodato.Text == "nvarchar" || lblTipodato.Text == "ntext" ||
                        lblTipodato.Text == "char")
                    {
                        if (chkPorcenIni.Checked && chkPorcenFin.Checked)
                        {
                            query += lblCampo.Text + " LIKE '%" + txtCondicion.Text + "%' ";
                            foreach(ListItem item in union.Items)
                            {
                                if(item.Selected)
                                {
                                    query += item.Value + " ";
                                }
                            }
                        }
                        else
                        {
                            if(chkPorcenIni.Checked && !chkPorcenFin.Checked)
                            {
                                query += lblCampo.Text + " LIKE '%" + txtCondicion.Text + "' ";
                                foreach(ListItem item in union.Items)
                                {
                                    if(item.Selected)
                                    {
                                        query += item.Value + " ";
                                    }
                                }
                            }
                            else
                            {
                                if(!chkPorcenIni.Checked && chkPorcenFin.Checked)
                                {
                                    query += lblCampo.Text + " LIKE '" + txtCondicion.Text + "%' ";
                                    foreach(ListItem item in union.Items)
                                    {
                                        if(item.Selected)
                                        {
                                            query += item.Value + " ";
                                        }
                                    }
                                }
                                else
                                {
                                    query += lblCampo.Text + " = '" + txtCondicion.Text + "'";
                                }
                            }
                        }
                    }
                }
                if(!string.IsNullOrEmpty(txtFechaInicio.Text))
                {
                    if (lblTipodato.Text == "datetime" || lblTipodato.Text == "smalldatetime" || lblTipodato.Text == "date")
                    {
                        query += "Cast(" + lblCampo.Text + " as date) Between Cast('" + txtFechaInicio.Text + "' As date) And Cast('" + txtFechaFin.Text + "' as date) ";
                        foreach (ListItem item in union.Items)
                        {
                            if (item.Selected)
                            {
                                query += item.Value + " ";
                            }
                        }
                    }
                }
                if(lblTipodato.Text == "bit")
                {
                    if(chkSino.Checked)
                    {
                        query += lblCampo.Text + " = 1 ";
                    }
                }
            }
            var strSeparado = query.Replace("Where", "@");
            var querySplit = strSeparado.Split('@');
            if(string.IsNullOrEmpty(querySplit[1].Trim()))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Debe ingresar por lo menos un dato de busqueda.')", true);
            }
            else
            {
                var dt = consultas.ObtenerDataTable(query, "text");
                ExportarExcel(dt);
            }
            
        }

        private void ExportarExcel(System.Data.DataTable dt)
        {
            var path = Server.MapPath("~") + @"FONADE\FileReports\Reporte" + ddlTablas.SelectedValue + ".xls";
            if(File.Exists(path))
            {
                File.Delete(path); ;
            }
            StreamWriter wr = new StreamWriter(path, false, Encoding.Unicode);

            try
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }

                wr.WriteLine();

                //write rows to excel file
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write("=\"" + Convert.ToString(dt.Rows[i][j]) + "\"" + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    //go to next line
                    wr.WriteLine();
                }
                //close file
                wr.Close();
                var url = Request.Url.AbsoluteUri.Split('/');
                var url2 = url[0] + "//" + url[2] + "/" + url[3] + "/FileReports/Reporte" + ddlTablas.SelectedValue + ".xls";
                lnkDescargar.NavigateUrl = url2;
                Session["Descarga"] = url2;
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "location.reload(); ", true);
            }
            catch (Exception)
            {

            }

            //var attachment = "attachment; filename=Reporte" + ddlTablas.SelectedValue +  ".xls";
            //Response.ClearContent();
            //Response.AddHeader("content-disposition", attachment);
            //Response.ContentType = "application/vnd.ms-excel";
            //string tab = "";
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    Response.Write(tab + dc.ColumnName);
            //    tab = "\t";
            //}
            //Response.Write("\n");
            //int i;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    tab = "";
            //    for (i = 0; i < dt.Columns.Count; i++)
            //    {
            //        Response.Write(tab + dr[i].ToString());
            //        tab = "\t";
            //    }
            //    Response.Write("\n");
            //}
            //Response.End();
        }
        #endregion

    }
}