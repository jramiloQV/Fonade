using Datos.DataType;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.ReportePuntaje;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.ReportePuntajeEval
{
    public partial class ReportePuntajeEval : System.Web.UI.Page
    {
        /// <summary>
        /// Identificador primario de la convocatoria
        /// </summary>
        public int IdConvocatoria
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdConvocatoria") ? int.Parse(Request.QueryString["IdConvocatoria"].ToString()) : 0;
            }
        }

        
        /// <summary>
        /// Nombre de la convocatoria
        /// </summary>
        public string NombreConvocatoria
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("NomConvocatoria") ? Request.QueryString["NomConvocatoria"].ToString() : "";
            }
        }

        /// <summary>
        /// Determina si la opción seleccionada es descarga del reporte en excel
        /// </summary>
        public bool EsDescarga
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("EsDescarga") ? bool.Parse(Request.QueryString["EsDescarga"].ToString()) : false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               lblTituloConvocatoria.Text = NombreConvocatoria;
               CargarDatosReporte();
            }
            catch (ThreadAbortException ex1)
            {

            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        /// <summary>
        /// Lista en la grilla los datos del reporte
        /// </summary>
        private void CargarDatosReporte()
        {
            List<ReportePuntaje> lst = ReportePtajeEvaluacion.getReporte(IdConvocatoria);

            gwReporte.DataSource = lst;
            gwReporte.DataBind();

            if(EsDescarga)
            {
                if (lst.Count > 0)
                {
                    string nomarchivo = string.Format("ReporteEvaluacion{0}.xls", DateTime.Now.Date);

                    gwReporte.HeaderRow.ForeColor = System.Drawing.Color.DarkBlue;
                    Utilidades.DescargarArchExcel(nomarchivo, gwReporte);
                }
            }
            
        }
    }
}