using Datos;
using Fonade.Account;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using System.Web.Security;

namespace Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public partial class Actividades : System.Web.UI.Page
    {
        public int IdProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdProyecto") ? int.Parse(Request.QueryString["IdProyecto"]) : 0;
            }
        }
        public int IdActividad
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdActividad") ? int.Parse(Request.QueryString["IdActividad"]) : 0;
            }
        }

        public int IdTipo
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdTipo") ? int.Parse(Request.QueryString["IdTipo"]) : 0;
            }
        }

        public string Titulo
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("Titulo") ? Request.QueryString["Titulo"] : null;
            }
        }

        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        protected void Page_Load(object sender, EventArgs e)
        {

            SetMaxLength();

            switch (IdTipo) { case 1: LabelNombreEstrategia.Text = "Estrategia de Promoción"; break; case 2: LabelNombreEstrategia.Text = "Estrategia de Comunicación"; break; case 3: LabelNombreEstrategia.Text = "Estrategia de Distribución"; break; default: break; }

            if (!IsPostBack && IdActividad > 0)
            {
                ConsultarActividad();
            }

        }

        void ConsultarActividad()
        {
            ProyectoEstrategiaActividade entActividad = new ProyectoEstrategiaActividade();
            entActividad = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(IdActividad);

            LabelTitulo.Text = "EDITAR ACTIVIDAD";
            txtActividad.Text = entActividad.Actividad;
            txtCosto.Text = entActividad.Costo.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtMes.Text = entActividad.MesEjecucion;
            txtRecursos.Text = entActividad.RecursosRequeridos;
            txtResponsable.Text = entActividad.Responsable;
        }

        void SetMaxLength()
        {
            // se establece por codigo el max length ya que el control no lo carga en textarea
            string max = "250";
            txtRecursos.Attributes.Add("maxlength", max);
            txtMes.Attributes.Add("maxlength", max);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ProyectoEstrategiaActividade entActividad = new ProyectoEstrategiaActividade()
            {
                Actividad = txtActividad.Text.Trim(),
                Costo = decimal.Parse(txtCosto.Text.Replace(",", "").Replace(".", ",")),
                IdTipoEstrategia = IdTipo,
                IdProyecto = IdProyecto,
                MesEjecucion = txtMes.Text.Trim(),
                RecursosRequeridos = txtRecursos.Text.Trim(),
                Responsable = txtResponsable.Text.Trim()
            };
            string msg;
            bool resul;
            //idcliente > 0 editar
            if (IdActividad > 0)
            {
                entActividad.IdActividad = IdActividad;
                resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Editar(entActividad, out msg);
            }
            else//insertar
            {
                resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Insertar(entActividad, out msg);
            }
            //actualizar la grilla de la pagina principal
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "upd", "window.opener.__doPostBack('', '" + IdTipo + "');", true);

            if (resul)
            {                
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.close();", true);
                ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Estrategias, IdProyecto, usuario.IdContacto, usuario.CodGrupo, false);
            }
            else
                Alert(msg);

        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

    }
}