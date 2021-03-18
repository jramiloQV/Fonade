using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Reportes
{
    public partial class ReportesBI : Negocio.Base_Page//System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //validar perfil
            if (!IsPostBack)
            {
                if (usuario.CodGrupo== Constantes.CONST_CallCenter
                    || usuario.CodGrupo == Constantes.CONST_CallCenterOperador)
                {
                    id_Evaluacion.Visible = false;
                    id_Tarea_Usuario.Visible = false;
                    id_Respuesta_Evaluacion_Emprendedor.Visible = false;
                }
            }



            //id_Cierre_Convocatoria.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fmesa%2fcierresConvocatoria";            
            //id_Evaluacion.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fmesa%2fObservacionesEvaluacion";            
            //id_Tarea_Usuario.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f4+Tarea+Usuario";
            //id_Respuesta_Evaluacion_Emprendedor.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f5+Respuesta+Evaluacion+Emprendedores";

            //id_Cierre_Convocatoria.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f1+Cierre+Convocatoria";
            //id_Acreditacion.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f2+Acreditacion";
            //id_Evaluacion.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f3+Observacion+Evaluacion";
            //id_Tarea_Usuario.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f4+Tarea+Usuario";
            //id_Viabilidad.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f6+Viabilidad";
            //id_Empleo_Generado.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f7+Empleos+Generados+Sql";
            //ID_Indicador_Gestion.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f8+Indicadores+Gestion+Sql";
            //Id_Pagos_Detallados.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f11+Pagos+Detallados+Proyecto+Sql";
            //id_Reporte_Contratos.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f10+Reporte+Contratos";
            //id_Pagos_Detalle_Proyecto.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f9+Pagos+Detallados";
            //id_Asesores_Regional.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaHttp") + "/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f12+Asesores+Regional";


        }
    }
}