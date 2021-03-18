using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Web.Services;
using System.Web.Script.Serialization;
 

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoOperacionCompras : Negocio.Base_Page
    {
        public string codProyecto;
        public int txtTab = Constantes.CONST_Compras;
        public string codConvocatoria;       

        public bool vldt { get { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"].ToString()); } }

        public bool ejecucion{
            get{
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString())).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo== Constantes.CONST_AdministradorSistema)
            {
                Post_It1.Visible = false;
            }
            if (Request.QueryString["codProyecto"] != null)
            {
                codProyecto = Request.QueryString["codProyecto"].ToString();
            }
            if (Request.QueryString["codConvocatoria"] != null)
                codConvocatoria = Request.QueryString["codConvocatoria"].ToString();
           
            if (!IsPostBack)
            {
                construirEncabezado();
            }
        }

        protected void btn_addInsumo_Click(object sender, EventArgs e)
        {            
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "mdl",                            
            string.Format("window.open('{0}',null,'status:false;dialogWidth:900px;dialogHeight:1500px')",
            Request.Url.AbsoluteUri.Remove(Request.Url.AbsoluteUri.LastIndexOf("/")) + "/CatalogoInsumo.aspx"), true);                  
        }
    }
}