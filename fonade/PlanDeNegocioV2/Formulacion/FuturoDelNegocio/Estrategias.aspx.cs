using Datos;
using Fonade.Account;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio
{
    public partial class Estrategias : System.Web.UI.Page
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
        public List<ProyectoEstrategiaActividade> ListPromocion
        {
            get { return (List<ProyectoEstrategiaActividade>)Session["ListPromocion"]; }
            set { Session["ListPromocion"] = value; }
        }
        public List<ProyectoEstrategiaActividade> ListComunicacion
        {
            get { return (List<ProyectoEstrategiaActividade>)Session["ListComunicacion"]; }
            set { Session["ListComunicacion"] = value; }
        }
        public List<ProyectoEstrategiaActividade> ListDistribucion
        {
            get { return (List<ProyectoEstrategiaActividade>)Session["ListDistribucion"]; }
            set { Session["ListDistribucion"] = value; }
        }
        #endregion

        #region GENERAL

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
            {
                Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
                Encabezado.CodigoTab = Constantes.CONST_Estrategias;

                SetPostIt();

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_Estrategias, Encabezado.CodigoProyecto);

                if (!IsPostBack)
                    CargarFuturo();

                CargarPromocion();
                CargarComunicacion();
                CargarDistribucion();
                divParentContainer.Attributes.Add("class", "parentContainer");

                if (Request["__EVENTARGUMENT"] == "2")
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(300);", true);

                if (Request["__EVENTARGUMENT"] == "3")
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(600);", true);

                gwPromocion.Columns[0].Visible = AllowUpdate;
                gwComunicacion.Columns[0].Visible = AllowUpdate;
                gwDistribucion.Columns[0].Visible = AllowUpdate;

            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Estrategias;
        }

        void CargarFuturo()
        {
            ProyectoFuturoNegocio entFuturo = new ProyectoFuturoNegocio();
            entFuturo = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.FuturoNegocio.Get(Encabezado.CodigoProyecto);
            if (entFuturo != null)
            {
                txtComunicacion.Text = entFuturo.EstrategiaComunicacion;
                txtDistribucion.Text = entFuturo.EstrategiaDistribucion;
                txtPromocion.Text = entFuturo.EstrategiaPromocion;
                txtProposito.Text = entFuturo.EstrategiaPromocionProposito;
                txtPropositoCom.Text = entFuturo.EstrategiaComunicacionProposito;
                txtPropositoDis.Text = entFuturo.EstrategiaDistribucionProposito;
            }

        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            //validar que se haya agregado promocion al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Promocion).Count() <= 0)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(15));
                return;
            }
            //validar que se haya agregado comunicacion al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Comunicacion).Count() <= 0)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(16));
                return;
            }

            //validar que se haya agregado distribucion al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Distribucion).Count() <= 0)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(17));
                return;
            }


            ProyectoFuturoNegocio entFuturo = new ProyectoFuturoNegocio()
            {
                IdProyecto = Encabezado.CodigoProyecto,
                EstrategiaComunicacion = txtComunicacion.Text.Trim(),
                EstrategiaComunicacionProposito = txtPropositoCom.Text.Trim(),
                EstrategiaDistribucion = txtDistribucion.Text.Trim(),
                EstrategiaDistribucionProposito = txtPropositoDis.Text.Trim(),
                EstrategiaPromocion = txtPromocion.Text.Trim(),
                EstrategiaPromocionProposito = txtProposito.Text.Trim()
            };

            string msg;
            bool resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.FuturoNegocio.Insertar(entFuturo, out msg);

            if (resul)
            {
                Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Estrategias, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Estrategias, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                Encabezado.ActualizarFecha();
            }

            Alert(msg);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(900);", true);
        }

        protected void BtnLimpiarCampos_Click(object sender, EventArgs e)
        {
            txtComunicacion.Text = null;
            txtDistribucion.Text = null;
            txtPromocion.Text = null;
            txtProposito.Text = null;
            txtPropositoCom.Text = null;
            txtPropositoDis.Text = null;

        }

        #endregion

        #region PROMOCION

        void CargarPromocion()
        {
            Session["ListPromocion"] = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Promocion);
            gwPromocion.DataSource = ListPromocion;
            gwPromocion.DataBind();
            LabelCosto.Text = ListPromocion.Sum(x => x.Costo).ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        protected void btnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            //validar que se haya agregado actividades al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, int.Parse(btn.Attributes["IdTipoEstrategia"])).Count() <= 1)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(14));
                return;
            }

            string msg;
            bool resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Eliminar(int.Parse(btn.CommandArgument), out msg);
            if (resul)
                CargarPromocion();

            Alert(msg);
        }

        protected void btnAgregarPromocion_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Actividades.aspx?IdProyecto=" + Encabezado.CodigoProyecto + "&IdTipo=" + (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Promocion, "_Blank", "width=620,height=500,top=100,left=" + (pos > 100 ? pos : 100));
        }

        protected void BtnEditarPromocion_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            LinkButton btn = (LinkButton)sender;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Actividades.aspx?IdActividad=" + btn.CommandArgument + "&IdProyecto=" + Encabezado.CodigoProyecto + "&IdTipo=" + (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Promocion, "_Blank", "width=620,height=500,top=100,left=" + (pos > 100 ? pos : 100));
        }

        protected void gwPromocion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwPromocion.PageIndex = e.NewPageIndex;
            gwPromocion.DataSource = ListPromocion;
            gwPromocion.DataBind();
        }

        #endregion

        #region COMUNICACION

        void CargarComunicacion()
        {
            Session["ListComunicacion"] = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Comunicacion);
            gwComunicacion.DataSource = ListComunicacion;
            gwComunicacion.DataBind();
            LabelCostoCom.Text = ListComunicacion.Sum(x => x.Costo).ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        protected void gwComunicacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwComunicacion.PageIndex = e.NewPageIndex;
            gwComunicacion.DataSource = ListComunicacion;
            gwComunicacion.DataBind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(300);", true);
        }

        protected void btnAgregarComunicacion_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Actividades.aspx?IdProyecto=" + Encabezado.CodigoProyecto + "&IdTipo=" + (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Comunicacion, "_Blank", "width=620,height=500,top=100,left=" + (pos > 100 ? pos : 100));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(300);", true);
        }

        protected void btnEliminarCom_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            //validar que se haya agregado actividades al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, int.Parse(btn.Attributes["IdTipoEstrategia"])).Count() <= 1)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(14));
                return;
            }

            string msg;
            bool resul;
            resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Eliminar(int.Parse(btn.CommandArgument), out msg);
            if (resul)
                CargarComunicacion();

            Alert(msg);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(300);", true);
        }

        protected void BtnEditarComunicacion_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            LinkButton btn = (LinkButton)sender;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Actividades.aspx?IdActividad=" + btn.CommandArgument + "&IdProyecto=" + Encabezado.CodigoProyecto + "&IdTipo=" + (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Comunicacion, "_Blank", "width=620,height=500,top=100,left=" + (pos > 100 ? pos : 100));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(300);", true);
        }

        protected void ImgAgregarCom_Click(object sender, ImageClickEventArgs e)
        {
            btnAgregarComunicacion_Click(sender, e);
        }

        #endregion

        #region DISTRIBUCION

        void CargarDistribucion()
        {
            Session["ListDistribucion"] = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Distribucion);
            gwDistribucion.DataSource = ListDistribucion;
            gwDistribucion.DataBind();
            LabelCostoDis.Text = ListDistribucion.Sum(x => x.Costo).ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        protected void btnAgregarDistribucion_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Actividades.aspx?IdProyecto=" + Encabezado.CodigoProyecto + "&IdTipo=" + (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Distribucion, "_Blank", "width=620,height=500,top=100,left=" + (pos > 100 ? pos : 100));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(600);", true);
        }

        protected void imgAgregarDis_Click(object sender, ImageClickEventArgs e)
        {
            btnAgregarDistribucion_Click(sender, e);
        }

        protected void btnEliminarDis_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            //validar que se haya agregado actividades al plan
            if (Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Get(Encabezado.CodigoProyecto, int.Parse(btn.Attributes["IdTipoEstrategia"])).Count() <= 1)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(14));
                return;
            }

            string msg;
            bool resul;
            resul = Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades.Eliminar(int.Parse(btn.CommandArgument), out msg);
            if (resul)
                CargarDistribucion();

            Alert(msg);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(600);", true);
        }

        protected void BtnEditarDistribucion_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 620) / 2) - 20;
            LinkButton btn = (LinkButton)sender;
            Fonade.Proyecto.Proyectos.Redirect(Response, "Actividades.aspx?IdActividad=" + btn.CommandArgument + "&IdProyecto=" + Encabezado.CodigoProyecto + "&IdTipo=" + (int)Negocio.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.TipoEstrategia.Distribucion, "_Blank", "width=620,height=500,top=100,left=" + (pos > 100 ? pos : 100));
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(600);", true);
        }

        protected void gwDistribucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwDistribucion.PageIndex = e.NewPageIndex;
            gwDistribucion.DataSource = ListDistribucion;
            gwDistribucion.DataBind();
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "scroll", "SetScroll(600);", true);
        }

        #endregion

    }
}