using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using Fonade.Account;
using System.Web.Security;
using Datos;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Fonade.PlanDeNegocioV2.Evaluacion.Controles
{
    public partial class EncabezadoEval : System.Web.UI.UserControl
    {
        #region Atributos
        public int IdProyecto { get; set; }
        public int IdTabEvaluacion { get; set; }
        public int IdConvocatoria { get; set; }
        [ContextStatic]
        protected FonadeUser usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean EsFaseEvaluacion { get; set; }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && EsFaseEvaluacion && usuario.CodGrupo.Equals(Datos.Constantes.CONST_CoordinadorEvaluador);
            }
            set { }
        }
        #endregion

        #region Metodos
        protected void Page_Load(object sender, EventArgs e)
        {
            EsMiembro = VerificarSiEsMiembroProyecto(IdProyecto, usuario.IdContacto);
            EsRealizado = VerificarSiEsRealizado(IdTabEvaluacion, IdProyecto, IdConvocatoria);
            EsFaseEvaluacion = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getEstadoProyecto(IdProyecto).Equals(Datos.Constantes.CONST_Evaluacion);

            if (!IsPostBack)
                GetUltimaActualizacion();

            btn_guardar_ultima_actualizacion.Visible = AllowUpdate;
            chk_realizado.Enabled = AllowUpdate;
        }

        public void GetUltimaActualizacion()
        {
            TabEvaluacionProyecto entTabEvaluacionProyecto = new TabEvaluacionProyecto()
            {
                CodConvocatoria = IdConvocatoria,
                CodTabEvaluacion = (short)IdTabEvaluacion,
                CodProyecto = IdProyecto,
            };
            TabEvaluacionProyecto entity = Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.GetUltimaActualizacion(entTabEvaluacionProyecto);
            if (entity != null)
            {
                lbl_nombre_user_ult_act.Text = (entity.Contacto.Nombres + " " + entity.Contacto.Apellidos).ToUpper();
                lbl_fecha_formateada.Text = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getFechaAbreviadaConFormato(entity.FechaModificacion, true);
                chk_realizado.Checked = entity.Realizado;
            }
        }

        protected Boolean VerificarSiEsMiembroProyecto(Int32 codigoProyecto, Int32 codigoContacto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from proyectoContacto in db.ProyectoContactos
                              where
                                    proyectoContacto.CodProyecto == codigoProyecto
                                   && proyectoContacto.CodContacto == codigoContacto
                                   && proyectoContacto.Inactivo == false
                                   && proyectoContacto.FechaInicio.Date <= DateTime.Now.Date
                                   && proyectoContacto.FechaFin == null
                              select
                                   proyectoContacto.CodRol
                          ).FirstOrDefault();

                if (entity != 0)
                    HttpContext.Current.Session["CodRol"] = entity;

                return entity != 0 ? true : false;
            }
        }

        protected Boolean VerificarSiEsRealizado(Int32 codigoTab, Int32 codigoProyecto, Int32 codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabEvaluacion in db.TabEvaluacionProyectos
                              where
                                   tabEvaluacion.CodProyecto.Equals(codigoProyecto)
                                   && tabEvaluacion.CodConvocatoria.Equals(codigoConvocatoria)
                                   && tabEvaluacion.CodTabEvaluacion.Equals(codigoTab)
                                   && tabEvaluacion.Realizado.Equals(true)
                              select
                                   tabEvaluacion.Realizado
                             ).Any();

                return entity;
            }
        }

        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            TabEvaluacionProyecto entTabEvaluacionProyecto = new TabEvaluacionProyecto()
            {
                CodConvocatoria = IdConvocatoria,
                CodTabEvaluacion = (short)IdTabEvaluacion,
                CodProyecto = IdProyecto,
                Realizado = chk_realizado.Checked,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now
            };

            if (!Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.TabEvaluacionCompleto(entTabEvaluacionProyecto))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + Negocio.Mensajes.Mensajes.GetMensaje(20) + "');", true);
                return;
            }

            string msg;
            bool? resul = Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetRealizado(entTabEvaluacionProyecto, out msg);

            if (resul == null)
            {
                chk_realizado.Checked = EsRealizado;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + Negocio.Mensajes.Mensajes.GetMensaje(20) + "');", true);
            }

            Response.Redirect(Request.RawUrl);
        }

        #endregion

    }
}