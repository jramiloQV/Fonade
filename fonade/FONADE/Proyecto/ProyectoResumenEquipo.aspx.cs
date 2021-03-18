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


namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoResumenEquipo : Negocio.Base_Page
    {
        public int codigo;
        protected void Page_Load(object sender, EventArgs e)
        {
            codigo = Convert.ToInt32(Request.QueryString["codProyecto"]);

            if (!IsPostBack)
            {

            }

        }
    }
}