using Datos;
using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
    public partial class EvaluacionHojaAvance : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        int IdProyecto;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
                IdProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());

            if (!IsPostBack)
                CargarAvance();

            btnEnviar.Visible = usuario.CodGrupo.Equals(Datos.Constantes.CONST_Evaluador);
            btnLimpiar.Visible = usuario.CodGrupo.Equals(Datos.Constantes.CONST_Evaluador);
        }

        void CargarAvance()
        {
            EvaluacionSeguimientoV2 entity = new EvaluacionSeguimientoV2()
            {
                IdConvocatoria = (int)Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(IdProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0),
                IdProyecto = IdProyecto
            };

            entity = Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.HojaAvance.GetAvance(entity);
            if (entity != null)
            {
                CheckAntecedente.Checked = entity.Antecedentes; CheckAntecedente.Enabled = !entity.Antecedentes;
                CheckCompetencia.Checked = entity.Competencia; CheckCompetencia.Enabled = !entity.Competencia;
                CheckCondicion.Checked = entity.CondicionesComercializacion; CheckCondicion.Enabled = !entity.CondicionesComercializacion;
                CheckEquipo.Checked = entity.EquipoTrabajo; CheckEquipo.Enabled = !entity.EquipoTrabajo;
                CheckEstrategia.Checked = entity.EstrategiasComercializacion; CheckEstrategia.Enabled = !entity.EstrategiasComercializacion;
                CheckFuerza.Checked = entity.FuerzaMercado; CheckFuerza.Enabled = !entity.FuerzaMercado;
                CheckIdentificacion.Checked = entity.IdentificacionMercado; CheckIdentificacion.Enabled = !entity.IdentificacionMercado;
                CheckIndiSegui.Checked = entity.IndicadoresSeguimiento; CheckIndiSegui.Enabled = !entity.IndicadoresSeguimiento;
                CheckIndiRenta.Checked = entity.IndiceRentabilidad; CheckIndiRenta.Enabled = !entity.IndiceRentabilidad;
                CheckInforme.Checked = entity.InformeEvaluacion; CheckInforme.Enabled = !entity.InformeEvaluacion;
                CheckLectura.Checked = entity.LecturaPlan; CheckLectura.Enabled = !entity.LecturaPlan;
                CheckModelo.Checked = entity.Modelo; CheckModelo.Enabled = !entity.Modelo;
                CheckNecesidad.Checked = entity.NecesidadClientes; CheckNecesidad.Enabled = !entity.NecesidadClientes;
                CheckNormatividad.Checked = entity.Normatividad; CheckNormatividad.Enabled = !entity.Normatividad;
                CheckOperacion.Checked = entity.OperacionNegocio; CheckOperacion.Enabled = !entity.OperacionNegocio;
                CheckPeriodo.Checked = entity.PeriodoImproductivo; CheckPeriodo.Enabled = !entity.PeriodoImproductivo;
                CheckPlan.Checked = entity.PlanOperativo; CheckPlan.Enabled = !entity.PlanOperativo;
                CheckPlanOperativo.Checked = entity.PlanOperativo2; CheckPlanOperativo.Enabled = !entity.PlanOperativo2;
                CheckPropuesta.Checked = entity.PropuestaValor; CheckPropuesta.Enabled = !entity.PropuestaValor;
                CheckRiesgos.Checked = entity.Riesgos; CheckRiesgos.Enabled = !entity.Riesgos;
                CheckSolicitud.Checked = entity.SolicitudInformacion; CheckSolicitud.Enabled = !entity.SolicitudInformacion;
                CheckSostenibilidad.Checked = entity.Sostenibilidad; CheckSostenibilidad.Enabled = !entity.Sostenibilidad;
                CheckTendencias.Checked = entity.TendenciasMercado; CheckTendencias.Enabled = !entity.TendenciasMercado;
                CheckValidacion.Checked = entity.ValidacionMercado; CheckValidacion.Enabled = !entity.ValidacionMercado;
                CheckViabilidad.Checked = entity.Viabilidad; CheckViabilidad.Enabled = !entity.Viabilidad;
                CheckIndiGestion.Checked = entity.IndicadoresGestion; CheckIndiGestion.Enabled = !entity.IndicadoresGestion;
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            EvaluacionSeguimientoV2 entity = new EvaluacionSeguimientoV2()
            {
                Antecedentes = CheckAntecedente.Checked,
                Competencia = CheckCompetencia.Checked,
                CondicionesComercializacion = CheckCondicion.Checked,
                EquipoTrabajo = CheckEquipo.Checked,
                EstrategiasComercializacion = CheckEstrategia.Checked,
                FechaActualizacion = DateTime.Now,
                FuerzaMercado = CheckFuerza.Checked,
                IdContacto = usuario.IdContacto,
                IdConvocatoria = (int)Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(IdProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0),
                IdentificacionMercado = CheckIdentificacion.Checked,
                IdProyecto = IdProyecto,
                IndicadoresGestion = CheckIndiGestion.Checked,
                IndicadoresSeguimiento = CheckIndiSegui.Checked,
                IndiceRentabilidad = CheckIndiRenta.Checked,
                LecturaPlan = CheckLectura.Checked,
                Modelo = CheckModelo.Checked,
                NecesidadClientes = CheckNecesidad.Checked,
                Normatividad = CheckNormatividad.Checked,
                OperacionNegocio = CheckOperacion.Checked,
                PeriodoImproductivo = CheckPeriodo.Checked,
                PlanOperativo = CheckPlan.Checked,
                PlanOperativo2 = CheckPlanOperativo.Checked,
                PropuestaValor = CheckPropuesta.Checked,
                Riesgos = CheckRiesgos.Checked,
                SolicitudInformacion = CheckSolicitud.Checked,
                Sostenibilidad = CheckSostenibilidad.Checked,
                TendenciasMercado = CheckTendencias.Checked,
                ValidacionMercado = CheckValidacion.Checked,
                Viabilidad = CheckViabilidad.Checked,
                InformeEvaluacion = CheckInforme.Checked
            };

            string msg;
            bool resul = Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.HojaAvance.InsertarAvance(entity, out msg);
            if (!resul)
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + msg + "');", true);

            CargarAvance();

        }

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CargarAvance();
        }

    }
}