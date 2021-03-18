using Datos;
using Fonade.Account;
using Fonade.Clases;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public partial class PeriodoArranque : System.Web.UI.Page
    {
        #region PROPIEDADES
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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
            Encabezado.CodigoTab = Constantes.CONST_PeriododeArranqueEImproductivo;

            SetPostIt();

            EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
            EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_PeriododeArranqueEImproductivo, Encabezado.CodigoProyecto);

            if (!IsPostBack)
                CargarArranque();

            divParentContainer.Attributes.Add("class", "parentContainer");
        }
        
        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_PeriododeArranqueEImproductivo;
        }

        void CargarArranque()
        {
            ProyectoPeriodoArranque entArranque = new ProyectoPeriodoArranque();
            entArranque = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.PeriodoArranque.Get(Encabezado.CodigoProyecto);
            if (entArranque != null)
            {
                CKArranque.Text = entArranque.PeriodoArranque;
                CKImproductivo.Text = entArranque.PeriodoImproductivo;
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            ProyectoPeriodoArranque entArranque = new ProyectoPeriodoArranque()
            {
                IdProyecto = Encabezado.CodigoProyecto,
                PeriodoArranque = CKArranque.Text.Trim(),
                PeriodoImproductivo = CKImproductivo.Text.Trim()
            };

            string msg;
            bool resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.PeriodoArranque.Insertar(entArranque, out msg);

            if (resul)
            {
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_PeriododeArranqueEImproductivo, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_PeriododeArranqueEImproductivo, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Encabezado.ActualizarFecha();
            }

            Alert(msg);
        }

        protected void BtnLimpiarCampos_Click(object sender, EventArgs e)
        {
            CKArranque.Text = null;
            CKImproductivo.Text = null;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }
    }
}