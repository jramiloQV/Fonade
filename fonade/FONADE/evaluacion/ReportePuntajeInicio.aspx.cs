#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 03 - 2014</Fecha>
// <Archivo>ReportePuntajeInicio.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data;
using Fonade.Negocio;
using System.Web;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using Fonade.Negocio.PlanDeNegocioV2.ReportePuntaje;
using Datos;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class ReportePuntajeInicio : Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CargaConvocatorias();
                }
            }
            catch (Exception ex)
            {

                PlanDeNegocioV2.Formulacion.Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }


        private void CargaConvocatorias()
        {
            DataTable dtConvocatoria =
                consultas.ObtenerDataTable("SELECT Id_Convocatoria, NomConvocatoria FROM Convocatoria where codOperador=" + usuario.CodOperador, "text");

            if (dtConvocatoria.Rows.Count!=0)
            {
                HttpContext.Current.Session["dtConvocatoria"] = dtConvocatoria;
                GrvConvocatorias.DataSource = HttpContext.Current.Session["dtConvocatoria"];
                GrvConvocatorias.DataBind();
            }
        }

        protected void GrvConvocatorias_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GrvConvocatorias.PageIndex = e.NewPageIndex;
            GrvConvocatorias.DataSource = HttpContext.Current.Session["dtConvocatoria"];
            GrvConvocatorias.DataBind();
        }

        protected void GrvConvocatorias_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            int codConvocatoria = int.Parse(e.CommandArgument.ToString());

            Datos.Convocatoria convocatoria = ReportePtajeEvaluacion.getConvocatoria(codConvocatoria);

            if (e.CommandName == "detallado")
            {
                if (convocatoria.IdVersionProyecto == Constantes.CONST_PlanV1)
                {
                    HttpContext.Current.Session["codConvocatoria"] = e.CommandArgument;
                    Response.Redirect("ReportePuntajeDetallado.aspx");
                }
                else
                {
                    Fonade.Proyecto.Proyectos.Redirect(Response, @"~\PlanDeNegocioV2\ReportePuntajeEval\ReportePuntajeEval.aspx?IdConvocatoria=" + convocatoria.Id_Convocatoria.ToString() +"&NomConvocatoria=" + convocatoria.NomConvocatoria, "_self", string.Empty);
                }
            }
            else if (e.CommandName == "descargar")
            {
                if (convocatoria.IdVersionProyecto == Constantes.CONST_PlanV1)
                {
                    HttpContext.Current.Session["codExcel"] = e.CommandArgument;
                    //HttpContext.Current.Session["codConvocatoria"] = e.CommandArgument;
                    Response.Redirect("ReportePuntajeDetallado.aspx");
                }
                else
                {
                    Fonade.Proyecto.Proyectos.Redirect(Response, @"~\PlanDeNegocioV2\ReportePuntajeEval\ReportePuntajeEval.aspx?IdConvocatoria=" + convocatoria.Id_Convocatoria.ToString() + "&EsDescarga=" + true, "_self", string.Empty);
                }
            }
        }
    }
}