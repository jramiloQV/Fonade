using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoResumenFrame : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region MyRegion
            //if (Request.QueryString["codProyecto"] != null)
            //{
            //    codProyecto = Request.QueryString["codProyecto"].ToString();
            //}
            //if (Request.QueryString["codConvocatoria"] != null)
            //{
            //    codConvocatoria = Request.QueryString["codConvocatoria"].ToString();
            //} 
            #endregion

            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";
        }
    }
}