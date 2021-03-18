using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Anexos
{
    public partial class DocumentosAcreditacion : System.Web.UI.Page
    {

        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
            {
                int value;
                if (int.TryParse(Request.QueryString["codproyecto"], out value))
                {
                    CargarGridDocumentosAcreditacion();
                }
            }
        }

        protected void CargarGridDocumentosAcreditacion()
        {

            //Se consulta el listado de documentos de acreditación
            List<Datos.DataType.GrillaDocumentos> datos = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentosAcreditacion(CodigoProyecto);

            gw_DocumentosAcreditacion.DataSource = datos;
            gw_DocumentosAcreditacion.DataBind();
        }

        protected void gw_DocumentosAcreditacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void AccionGrid(string accion, string argumento)
        {
            try
            {                
                switch (accion)
                {
                    case "VerDocumento":
                        string url = ConfigurationManager.AppSettings.Get("RutaIP") + argumento;
                        Utilidades.DescargarArchivo(url);
                        break;                                    
                }
            }
            catch (Exception ex)
            {
                Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }
    }
}