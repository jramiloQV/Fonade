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

namespace Fonade.PlanDeNegocioV2.Formulacion.Solucion
{
    public partial class Solucion : System.Web.UI.Page
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
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
            {
                Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
                Encabezado.CodigoTab = Constantes.CONST_Parte1Solucion;

                SetPostIt();

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(Constantes.CONST_Parte1Solucion, Encabezado.CodigoProyecto);

                if(!Page.IsPostBack)
                    CargarSolucion();
            }

            divParentContainer.Attributes.Add("class", "parentContainer");
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Parte1Solucion;
        }

        void CargarSolucion()
        {
            ProyectoSolucion entSolucion = new ProyectoSolucion();

            entSolucion = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Solucion.Get(Encabezado.CodigoProyecto);
            if (entSolucion != null)
            {
                CKComoValido.Text = entSolucion.AceptacionProyecto;
                CKComercial.Text = entSolucion.Comercial;
                CKConceptoNegocio.Text = entSolucion.ConceptoNegocio;
                CKConceptoNegocio2.Text = entSolucion.InnovadorConceptoNegocio;
                CKLegal.Text = entSolucion.Legal;
                CKProceso.Text = entSolucion.Proceso;
                CKProductoServicio.Text = entSolucion.ProductoServicio;
                CKTecnicoproductivo.Text = entSolucion.TecnicoProductivo;
            }
        }

        protected void BtnLimpiarCampos_Click(object sender, EventArgs e)
        {
            CKComercial.Text = null;
            CKComoValido.Text = null;
            CKConceptoNegocio.Text = null;
            CKConceptoNegocio2.Text = null;
            CKLegal.Text = null;
            CKProceso.Text = null;
            CKProductoServicio.Text = null;
            CKTecnicoproductivo.Text = null;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {

                ProyectoSolucion entSolucion = new Datos.ProyectoSolucion()
                {
                    AceptacionProyecto = CKComoValido.Text.Trim(),
                    Comercial = CKComercial.Text.Trim(),
                    ConceptoNegocio = CKConceptoNegocio.Text.Trim(),
                    IdProyecto = Encabezado.CodigoProyecto,
                    InnovadorConceptoNegocio = CKConceptoNegocio2.Text.Trim(),
                    Legal = CKLegal.Text.Trim(),
                    Proceso = CKProceso.Text.Trim(),
                    ProductoServicio = CKProductoServicio.Text.Trim(),
                    TecnicoProductivo = CKTecnicoproductivo.Text.Trim()
                };

                string msg;
                bool resul = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Solucion.Insertar(entSolucion, out msg);

                if (resul)
                {
                    Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(Constantes.CONST_Parte1Solucion, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                    ProyectoGeneral.UpdateTab(Datos.Constantes.CONST_Parte1Solucion, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                    Encabezado.ActualizarFecha();
                }

                Alert(msg);

            }
            catch (Exception)
            {
                Alert(Negocio.Mensajes.Mensajes.GetMensaje(7));
            }
        }

        public void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

    }
}