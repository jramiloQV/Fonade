using Datos;
using Datos.DataType;
using Fonade.Account;
using Fonade.Clases;
using Fonade.FONADE.PlandeNegocio;
using Fonade.Negocio.Proyecto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.ResumenEjecutivo
{
    public partial class ResumenEjecutivo : System.Web.UI.Page
    {

        protected FonadeUser Usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        public Boolean EsMiembro { get; set; }
        public Boolean EsRealizado { get; set; }
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && Usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
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

        public List<Emprendedor> ListEmprendedor
        {
            get { return (List<Emprendedor>)Session["ListEmprendedor"]; }
            set { Session["ListEmprendedor"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
            Encabezado.CodigoTab = Constantes.CONST_ResumenEjecutivoV2;

            SetPostIt();

            EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, Usuario.IdContacto);
            EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_ResumenEjecutivoV2, Encabezado.CodigoProyecto);

            divParentContainer.Attributes.Add("class", "parentContainer");

            CargarEmprendedores();

            if (!IsPostBack)
                CargarResumen();

        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = Usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_ResumenEjecutivoV2;
        }

        void CargarResumen()
        {


            ProyectoResumenEjecutivoV2 entResumen = Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen.Get(Encabezado.CodigoProyecto);

            if (entResumen != null)
            {
                CKConcepto.Text = entResumen.ConceptoNegocio;
                txtEmpleo.Text = entResumen.IndicadorEmpleos;
                txtIndirectos.Text = entResumen.IndicadorEmpleosDirectos;
                txtMercadeo.Text = entResumen.IndicadorMercadeo;
                txtSena.Text = entResumen.IndicadorContraPartido;
                txtVentas.Text = entResumen.IndicadorVentas;
                txtPeriodoImproductivo.Text = entResumen.PeriodoImproductivo.ToString();
                txtRecursosAportados.Text = entResumen.RecursosAportadosEmprendedor.ToString().Trim();
                txtEnlaceVideoEmprendedor.Text = entResumen.VideoEmprendedor;
            }
            else
            {
                CKConcepto.Text = "N/A";
                txtEmpleo.Text = "N/A";
                txtIndirectos.Text = "N/A";
                txtMercadeo.Text = "N/A";
                txtSena.Text = "N/A";
                txtVentas.Text = "N/A";
                txtEnlaceVideoEmprendedor.Text = "";
            }

            try
            {
                if (txtEnlaceVideoEmprendedor.Text != "" && txtEnlaceVideoEmprendedor.Text.Trim().Contains("https://www.youtube.com/watch?v"))
                {
                    string video = txtEnlaceVideoEmprendedor.Text.Replace("watch?v=", "embed/");

                    string rutaVideo = "";

                    if (video.Contains("&"))
                    {
                        rutaVideo = video.Split('&')[0];
                    }
                    else
                    {
                        rutaVideo = video;
                    }

                    FrmVideoEmprendedor.InnerHtml = "<iframe width=\"700\" style=\"height: 400px\" target=\"_parent\"" +
                                        "src = \"" + rutaVideo + "\"" +
                                        "frameborder = \"0\" allow = \"accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture\"" +
                                        "allowfullscreen ></iframe> \"";
                }
                else
                {
                    FrmVideoEmprendedor.InnerHtml = "<h4>Por favor valide la url del video.</h4>";
                }
            }
            catch (Exception)
            {
                FrmVideoEmprendedor.InnerHtml = "<h4>Por favor valide la url del video, ya que no se encontró el video en Youtube.</h4>";
            }


            var empleosDetectados = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetCargos(Encabezado.CodigoProyecto);
            var contrapartidas = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetContrapartidas(Encabezado.CodigoProyecto);
            var ejecucionPresupuestal = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetPresupuesto(Encabezado.CodigoProyecto);
            var ventasTotales = FieldValidate.moneyFormat(Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetVentas(Encabezado.CodigoProyecto), true);
            var mercadeo = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetMercadeo(Encabezado.CodigoProyecto);
            var idh = Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetIDH(Encabezado.CodigoProyecto);

            txtEmpleosDetectados.Text = empleosDetectados.ToString();
            txtContrapartidas.Text = contrapartidas.ToString();
            txtEjecucionPresupuestal.Text = ejecucionPresupuestal.ToString();
            txtVentasProductos.Text = ventasTotales.ToString();
            txtMercadeoTotal.Text = mercadeo.ToString();
            txtIDH.Text = idh.ToString();

            //cargar sectores
            cargarSector(Encabezado.CodigoProyecto);
            cargarSubSector(codSector(Encabezado.CodigoProyecto));
            //cmbSubSector.Items.FindByValue(codSubSector(Encabezado.CodigoProyecto).ToString()).Selected = true;
            cmbSector.SelectedValue = codSector(Encabezado.CodigoProyecto).ToString();
            cmbSubSector.SelectedValue = codSubSector(Encabezado.CodigoProyecto).ToString();

            if (!(proyectoController.estadoProyecto(Encabezado.CodigoProyecto) == Constantes.CONST_Registro_y_Asesoria))
            {
                cmbSector.Enabled = false;
                cmbSubSector.Enabled = false;
            }
        }

        ProyectoController proyectoController = new ProyectoController();

        void CargarEmprendedores()
        {
            Session["ListEmprendedor"] = Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen.GetEmprendedoresYEquipoTrabajo(Encabezado.CodigoProyecto);
            gwEmprendedores.DataSource = ListEmprendedor;
            gwEmprendedores.DataBind();
        }
        Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen resumen
            = new Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen();
        private void cargarSector(int _codProyecto)
        {
            cmbSector.DataSource = resumen.getSector(codSector(_codProyecto));
            cmbSector.DataBind();

        }

        private int codSector(int codProyecto)
        {
            int codSector = 0;
            using (FonadeDBDataContext db = new FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int codSubSector = (from p in db.Proyecto
                                    where p.Id_Proyecto == codProyecto
                                    select p.CodSubSector).FirstOrDefault();

                codSector = (from p in db.SubSector
                             where p.Id_SubSector == codSubSector
                             select p.CodSector).FirstOrDefault();
            }

            return codSector;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateNumeric("Periodo improductivo", txtPeriodoImproductivo.Text, true);
                FieldValidate.ValidateString("Recursos aportados", txtRecursosAportados.Text.Trim(), true, 255);

                int? idProductoRepresentativo = GetProductoRepresentativo();

                if (idProductoRepresentativo == null)
                    throw new ApplicationException("Debe seleccionar el producto mas presentativo.");

                if (!txtEnlaceVideoEmprendedor.Text.Trim().Contains("https://www.youtube.com/watch?v"))
                {
                    lblErrorVideo.Visible = true;
                    lblErrorVideo.Text = "Advertencia: La url del video no tiene el formato correcto.";
                    throw new ApplicationException("La url del video no tiene el formato correcto.");
                }


                ProyectoResumenEjecutivoV2 entResumen = new ProyectoResumenEjecutivoV2()
                {
                    ConceptoNegocio = CKConcepto.Text.Trim(),
                    IdProyecto = Encabezado.CodigoProyecto,
                    IndicadorContraPartido = txtSena.Text.Trim(),
                    IndicadorEmpleos = txtEmpleo.Text.Trim(),
                    IndicadorEmpleosDirectos = txtIndirectos.Text.Trim(),
                    IndicadorMercadeo = txtMercadeo.Text.Trim(),
                    IndicadorVentas = txtVentas.Text.Trim(),
                    PeriodoImproductivo = Convert.ToInt32(txtPeriodoImproductivo.Text),
                    RecursosAportadosEmprendedor = txtRecursosAportados.Text.Trim(),
                    ProductoMasRepresentativo = idProductoRepresentativo,
                    VideoEmprendedor = txtEnlaceVideoEmprendedor.Text.Trim()
                };

                string msg;
                bool resul = Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen.Insertar(entResumen, out msg);

                if (resul)
                {
                    Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_ResumenEjecutivoV2, Encabezado.CodigoProyecto, Usuario.IdContacto, true);

                    ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_ResumenEjecutivoV2, Encabezado.CodigoProyecto, Usuario.IdContacto, Usuario.CodGrupo, false);
                    Encabezado.ActualizarFecha();

                    //Actualizar Sector
                    actualizarSectorProyecto(Convert.ToInt32(cmbSubSector.SelectedValue), Encabezado.CodigoProyecto);

                    CargarResumen();
                }

                Alert(msg);
                lblError.Visible = false;
                lblErrorVideo.Visible = false;
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }

        private void actualizarSectorProyecto(int _codSubSector, int _codProyecto)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var proyecto = (from p in db.Proyecto
                                where p.Id_Proyecto == _codProyecto
                                select p).FirstOrDefault();

                proyecto.CodSubSector = _codSubSector;
                db.SubmitChanges();
            }
        }

        public int? GetProductoRepresentativo()
        {
            int? idProducto = null;
            foreach (GridViewRow currentRow in gvProductos.Rows)
            {
                CheckBox rdbtnProducto = (RadioButton)currentRow.FindControl("rdProductoSeleccionado");
                HiddenField hdIdProducto = (HiddenField)currentRow.FindControl("hdCodigoProducto");

                if (rdbtnProducto.Checked)
                {
                    idProducto = Convert.ToInt32(hdIdProducto.Value);
                }
            }

            return idProducto;
        }

        private void Alert(string mensaje)
        {
            //ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
            lblError.Visible = true;
            lblError.Text = mensaje;
        }

        protected void BtnLimpiarCampos_Click(object sender, EventArgs e)
        {
            CKConcepto.Text = null;
            txtEmpleo.Text = null;
            txtIndirectos.Text = null;
            txtMercadeo.Text = null;
            txtSena.Text = null;
            txtVentas.Text = null;
            txtPeriodoImproductivo.Text = null;
            txtRecursosAportados.Text = null;
            txtEnlaceVideoEmprendedor.Text = null;
        }

        protected void BtnVerDetalle_Click(object sender, EventArgs e)
        {
            int pos = ((int.Parse(HiddenWidth.Value) - 400) / 2) - 20;
            LinkButton btn = (LinkButton)sender;
            Fonade.Proyecto.Proyectos.Redirect(Response, "DetalleEmprendedor.aspx?IdContacto=" + btn.CommandArgument, "_Blank", "scrollbars=no,width=400,height=200,top=300,left=" + (pos > 100 ? pos : 100));
            CargarResumen();
        }

        protected void gwEmprendedores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwEmprendedores.PageIndex = e.NewPageIndex;
        }

        public List<Negocio.PlanDeNegocioV2.Utilidad.ProduccionDTO> GetProductos(Int32 codigoProyecto)
        {
            List<Negocio.PlanDeNegocioV2.Utilidad.ProduccionDTO>
                list = new List<Negocio.PlanDeNegocioV2.Utilidad.ProduccionDTO>();

            try
            {
                return Negocio.PlanDeNegocioV2.Utilidad.IndicadorFormulacion.GetProductos(codigoProyecto);
            }
            catch (Exception ex)
            {
                Alert("Falta llenar la proyección.");
                return list;
            }
        }

        private int codSubSector(int codProyecto)
        {
            int codSubSector = 0;
            using (FonadeDBDataContext db = new FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                codSubSector = (from p in db.Proyecto
                                where p.Id_Proyecto == codProyecto
                                select p.CodSubSector).FirstOrDefault();
            }

            return codSubSector;
        }

        protected void cmbSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cargar subSector
            cargarSubSector(Convert.ToInt32(cmbSector.SelectedValue));
            cmbSubSector.Focus();
        }

        private void cargarSubSector(int codSubSectorProyecto)
        {
            cmbSubSector.DataSource = resumen.getSubSector(codSubSectorProyecto);
            cmbSubSector.DataBind();
        }
    }
}