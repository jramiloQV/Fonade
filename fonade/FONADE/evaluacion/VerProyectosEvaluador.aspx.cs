#region using

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

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class VerProyectosEvaluador : Negocio.Base_Page
    {
        protected int CodContacto;

        /// <summary>
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodContacto = Convert.ToInt32(HttpContext.Current.Session["ContactoEvaluador"]);

            var query = (from x in consultas.Db.Contacto
                         where x.Id_Contacto == CodContacto
                         select new
                         {
                             nombre = x.Nombres + " " + x.Apellidos,
                         }).FirstOrDefault();

            lbl_Titulo.Text = void_establecerTitulo("PLANES DE NEGOCIO PARA " + query.nombre.ToUpper());
            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
        }

        /// <summary>
        /// enlaza los proyectos de un evaluador a la grilla
        /// en base a id del contacto y al rol del usuario (evaluador)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_eval_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.VerProyectosEvaluador(CodContacto, Constantes.CONST_Evaluacion)
                            select P;
                e.Result = query;
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// cierra la pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
        }
    }
}