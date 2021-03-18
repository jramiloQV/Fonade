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

namespace Fonade.PlanDeNegocioV2.Formulacion.OportunidadMercado
{
    public partial class MenuOportunidadMercado : System.Web.UI.Page
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
        public List<ProyectoOportunidadMercadoCompetidore> ListCompetidores
        {
            get { return (List<ProyectoOportunidadMercadoCompetidore>)Session["ListCompetidores"]; }
            set { Session["ListCompetidores"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
            {
                Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
                Encabezado.CodigoTab = Constantes.CONST_OportunidadMercado;

                SetPostIt();

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_OportunidadMercado, Encabezado.CodigoProyecto);

                if (!IsPostBack)
                    CargarOportunidad();
                //if (!IsPostBack || Request["__EVENTARGUMENT"] == "updGrillaCompetidores")
                CargarCompetidores();

                gwCompetidores.Columns[0].Visible = AllowUpdate;
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_OportunidadMercado;
        }

        void CargarOportunidad()
        {
            ProyectoOportunidadMercado entOportunidad = new ProyectoOportunidadMercado();
            entOportunidad = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Oportunidad.GetOportunidad(Encabezado.CodigoProyecto);
            if (entOportunidad != null)
            {
                CKTendencia.Text = entOportunidad.TendenciaCrecimiento;
            }

        }

        void CargarCompetidores()
        {
            ListCompetidores = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.GetCompetidores(Encabezado.CodigoProyecto);
            gwCompetidores.DataSource = ListCompetidores;
            gwCompetidores.DataBind();
        }

        protected void btnAgregarCompetidor_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Competidores.aspx?IdProyecto=" + Encabezado.CodigoProyecto, "_Blank", "width=620,height=660,top=100,left=" + (pos > 100 ? pos : 100));
        }

        public void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            //validar que se haya agregado competidores al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.GetCompetidores(Encabezado.CodigoProyecto).Count() <= 0)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(12));
                return;
            }

            ProyectoOportunidadMercado entOportunidad = new ProyectoOportunidadMercado()
            {
                IdProyecto = Encabezado.CodigoProyecto,
                TendenciaCrecimiento = CKTendencia.Text.Trim()
            };

            string msg;
            bool resul = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Oportunidad.InsertarOportunidad(entOportunidad, out msg);

            //FonadeUser usuario = (FonadeUser)Session["usuarioLogged"];
            FonadeUser usuario = HttpContext.Current.Session["usuarioLogged"] != null ?
                    (FonadeUser)HttpContext.Current.Session["usuarioLogged"] :
                    (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);


            if (resul)
            {
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_OportunidadMercado, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_OportunidadMercado, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Encabezado.ActualizarFecha();
            }

            Alert(msg);
        }

        protected void btnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            //validar que se haya agregado competidores al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.GetCompetidores(Encabezado.CodigoProyecto).Count() <= 1)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(14));
                gwCompetidores.DataSource = ListCompetidores;
                gwCompetidores.DataBind();
                return;
            }

            ImageButton btn = (ImageButton)sender;
            string msg;
            bool resul;
            resul = Negocio.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores.EliminarCompetidor(int.Parse(btn.CommandArgument), out msg);
            if (resul)
                CargarCompetidores();

            Alert(msg);
        }

        protected void BtnEditarCompetidor_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            LinkButton btn = (LinkButton)sender;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Competidores.aspx?IdProyecto=" + Encabezado.CodigoProyecto + "&IdCompetidor=" + btn.CommandArgument, "_Blank", "width=620,height=660,top=100,left=" + (pos > 100 ? pos : 100));
        }

        protected void gwCompetidores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwCompetidores.PageIndex = e.NewPageIndex;
            gwCompetidores.DataSource = ListCompetidores;
            gwCompetidores.DataBind();
        }

        protected void BtnLimpiarCampos_Click(object sender, EventArgs e)
        {
            CKTendencia.Text = null;
        }
    }
}