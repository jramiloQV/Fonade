using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
    public partial class MenuTablaDeEvaluacion : System.Web.UI.Page
    {

        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }

        public int? CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ValidateParameters();
                TabVisibility();
            }
            catch (Exception)
            {
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }            
        }

        protected void ValidateParameters()
        {
            if (!(Request.QueryString.AllKeys.Contains("codproyecto") ))                            
                throw new Exception("No se logro obtener la información necesaria para continuar.");

            int value;
            if (!int.TryParse(Request.QueryString["codproyecto"], out value))
                throw new Exception("No se logro obtener la información necesaria para continuar.");
        }

        protected void TabVisibility() {
            tabProtagonista.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0),Constantes.Const_AspectoProtagonistaV2);
            tabOportunidadMercado.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0), Constantes.Const_AspectoOportunidadMercadoV2);
            tabMiSolucion.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0), Constantes.Const_AspectoCualEsMiSolucionV2);
            tabDesarrolloSolucion.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0), Constantes.Const_AspectoDesarrolloSolucionV2);
            tabFuturoNegocio.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0), Constantes.Const_AspectoFuturoNegocioV2);
            tabRiesgos.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0), Constantes.Const_AspectoRiesgosV2);
            tabResumenEjecutivo.Visible = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.ExistAspectoByConvocatoria(CodigoConvocatoria.GetValueOrDefault(0), Constantes.Const_AspectoResumenEjecutivoV2New);
        }
    }
}