using Datos;
using Fonade.Account;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion
{
    public partial class DatosGenerales : System.Web.UI.Page
    {                
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_DatosGeneralesV2; } set { } }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        public Boolean PostitVisible {                    
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Evaluador);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request["codproyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");

                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = CodigoTab;

                EsMiembro = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (!Page.IsPostBack)
                    GetDetails(CodigoProyecto, CodigoConvocatoria);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }        
        }

        protected void GetDetails(int CodigoProyecto,int codigoConvocatoria) 
        {
            var entity = Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.DatosGenerales.Get(CodigoProyecto, codigoConvocatoria);

            if (entity != null)
            {
                txtLocalizacion.Text = entity.Localizacion;
                txtSector.Text = entity.Sector;
                txtResumenConcepto.Text = entity.ResumenConcepto;
            }
        }

        protected void ValidateFields()
        {
            FieldValidate.ValidateString("Localización", txtLocalizacion.Text , true, 1500);
            FieldValidate.ValidateString("Sector", txtSector.Text, true, 1500);
            FieldValidate.ValidateString("Resumen concepto general - compromisos y condiciones", txtResumenConcepto.Text, true, 1500);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidateFields();

                EvaluacionObservacion observacion = new EvaluacionObservacion {
                    Sector = txtSector.Text,
                    Localizacion = txtLocalizacion.Text,
                    ResumenConcepto = txtResumenConcepto.Text,
                    CodProyecto = CodigoProyecto,
                    CodConvocatoria = CodigoConvocatoria
                };

                Negocio.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.DatosGenerales.Update(observacion);

                TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto() {
                    CodProyecto = CodigoProyecto,
                    CodConvocatoria = CodigoConvocatoria,
                    CodTabEvaluacion = (Int16)CodigoTab,
                    CodContacto = usuario.IdContacto,
                    FechaModificacion = DateTime.Now,
                    Realizado = false
                };

                string messageResult;
                Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);
                EncabezadoEval.GetUltimaActualizacion();


                Formulacion.Utilidad.Utilidades.PresentarMsj(messageResult, this, "Alert");
            }
            catch (ApplicationException ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj(ex.Message, this, "Alert");
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj("Sucedio un error al guardar.", this, "Alert");
            }
        }
    }
}