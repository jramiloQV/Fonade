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
using Fonade.Error;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoContratoEvaluador
    /// </summary>
    
    public partial class CatalogoContratoEvaluador : Negocio.Base_Page
    {
        private string CodEvaluador;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodEvaluador = HttpContext.Current.Session["ContactoEvaluador"].ToString();
            int CodigoEvaluador = Convert.ToInt32(CodEvaluador);
            var query = (from x in consultas.Db.Contacto where x.Id_Contacto == CodigoEvaluador select new { x.Nombres, x.Apellidos }).FirstOrDefault();
            lbl_Titulo.Text = void_establecerTitulo("CONTRATOS DEL EVALUADOR: " + query.Nombres + " " + query.Apellidos);
        }

        /// <summary>
        /// Handles the Selecting event of the lds_listaCoordEval control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_listaCoordEval_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            try
            {
                var query = from P in consultas.Db.MD_VerContratosEvaluador(Convert.ToInt32(CodEvaluador))
                            select P;
                e.Result = query;
            }
            catch (Exception ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
            }

        }

        /// <summary>
        /// Handles the RowCommand event of the gvcCoordinadoresEval control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvcCoordinadoresEval_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "editarContrato":
                    HttpContext.Current.Session["ContactoEvaluador"] = CodEvaluador;
                    HttpContext.Current.Session["ContratoEvaluador"] = e.CommandArgument.ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('ContratoEvaluador.aspx','_blank','width=502,height=300,toolbar=no, scrollbars=no, resizable=no');", true);
                    break;
            }
        }
        /// <summary>
        /// crear contrato
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_crearContrato_Click(object sender, EventArgs e)
        {
            abirnuevocontrato();   
        }
        /// <summary>
        /// 
        /// </summary>redirecciona a contratoevaluador
        private void abirnuevocontrato()
        {
            HttpContext.Current.Session["ContactoEvaluador"] = CodEvaluador;
            HttpContext.Current.Session["ContratoEvaluador"] = "0";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('ContratoEvaluador.aspx','_blank','width=502,height=200,toolbar=no, scrollbars=no, resizable=no');", true);
        }

        /// <summary>
        /// Handles the Click event of the ibtn_crearCntrato control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ibtn_crearCntrato_Click(object sender, ImageClickEventArgs e)
        {
            abirnuevocontrato();
        }

    }
}