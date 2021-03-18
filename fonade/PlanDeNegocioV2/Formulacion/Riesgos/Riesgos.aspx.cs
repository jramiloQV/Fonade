using Datos;
using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;

namespace Fonade.PlanDeNegocioV2.Formulacion.Riesgos
{
    public partial class Riesgos : System.Web.UI.Page
    {

        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
            }
            set { }
        }
        public Boolean PostitVisible
        {
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
            Encabezado.CodigoTab = Constantes.CONST_Riesgos;

            SetPostIt();

            EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
            EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_Riesgos, Encabezado.CodigoProyecto);

            divParentContainer.Attributes.Add("class", "parentContainer");

            if (!IsPostBack)
                CargarRiesgos();

        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Riesgos;
        }

        void CargarRiesgos()
        {
            ProyectoRiesgo entRiesgo = new ProyectoRiesgo();
            entRiesgo = Negocio.PlanDeNegocioV2.Formulacion.Riesgos.Riesgos.Get(Encabezado.CodigoProyecto);
            if (entRiesgo != null)
            {
                CKActores.Text = entRiesgo.ActoresExternos;
                CKFactores.Text = entRiesgo.FactoresExternos;
            }

        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            ProyectoRiesgo entRiesgo = new ProyectoRiesgo()
            {
                IdProyecto = Encabezado.CodigoProyecto,
                ActoresExternos = CKActores.Text.Trim(),
                FactoresExternos = CKFactores.Text.Trim()
            };

            string msg;
            bool resul = Negocio.PlanDeNegocioV2.Formulacion.Riesgos.Riesgos.Insertar(entRiesgo, out msg);

            if (resul)
            {
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Riesgos, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Riesgos, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Encabezado.ActualizarFecha();
            }

            Alert(msg);
        }

        protected void BtnLimpiarCampos_Click(object sender, EventArgs e)
        {
            CKActores.Text = null;
            CKFactores.Text = null;
        }


        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }
    }
}