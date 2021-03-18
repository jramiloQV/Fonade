using Datos;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public partial class ReqNegocio : System.Web.UI.Page
    {
        #region Variables

        public int IdProyectoInfraestructura
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdProyectoInfraestructura") ? int.Parse(Request.QueryString["IdProyectoInfraestructura"].ToString()) : 0;
            }
        }

        public int IdProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdProyecto") ? int.Parse(Request.QueryString["IdProyecto"].ToString()) : 0;
            }
        }

        public int TipoReq
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("TipoReq") ? int.Parse(Request.QueryString["TipoReq"].ToString()) : 0;
            }
        }

        private ProyectoInfraestructura requerimiento { get; set; }

        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso4Requerimientos; } }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!IsPostBack)
                {
                    CargarCombo();
                }

                if (!IsPostBack && IdProyectoInfraestructura > 0)
                {
                    CargarFormulario();
                }

                SetMaxLength();
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            bool esnuevo = IdProyectoInfraestructura == 0 ? true : false;

            ProyectoInfraestructura item = new ProyectoInfraestructura()
            {
                codProyecto = IdProyecto == 0 ? int.Parse(ViewState["codProyecto"].ToString()) : IdProyecto ,
                NomInfraestructura = txtDescripcion.Text.Trim(),
                CodTipoInfraestructura = TipoReq == 0 ? (byte)ViewState["TipoReqExistente"] : (byte)TipoReq,
                ValorUnidad = Convert.ToDecimal(txtVlrUnitario.Text.Replace(",", "").Replace(".", ",")),
                Cantidad = float.Parse(txtCantidad.Text.Trim()),
                FechaCompra = DateTime.Now,
                ValorCredito = 0,
                PeriodosAmortizacion = 0,
                RequisitosTecnicos = txtReqTecnico.Text.Trim(),
                IdFuente = int.Parse(ddlFuenteFinan.SelectedValue)
            };

            //Si es nuevo se crea el nuevo registro. Si no se actualiza
            if (!esnuevo)
            {
                item.Id_ProyectoInfraestructura = IdProyectoInfraestructura;
            }

            if (!RequerimientosNegocio.setRequerimiento(item, esnuevo))
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
            else
            {
                //actualizar la grilla de la pagina principal
                Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, IdProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "upd", "window.opener.__doPostBack('', 'updGrilla');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "Close", "<script>window.close();</script> ");
            }
        }

        #endregion

        #region Métodos

        // <summary>
        /// Carga el formulario con los datos existentes o vacío si es nuevo
        /// </summary>
        private void CargarFormulario()
        {

            requerimiento = RequerimientosNegocio.getRequerimiento(IdProyectoInfraestructura);

            if(requerimiento != null)
            {
                txtDescripcion.Text = requerimiento.NomInfraestructura;
                txtCantidad.Text = requerimiento.Cantidad.ToString();
                txtVlrUnitario.Text = requerimiento.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
                txtReqTecnico.Text = requerimiento.RequisitosTecnicos;
                ddlFuenteFinan.SelectedValue = requerimiento.IdFuente.ToString();
                ViewState["TipoReqExistente"] = requerimiento.CodTipoInfraestructura;
                ViewState["codProyecto"] = requerimiento.codProyecto;
            }

        }

        /// <summary>
        /// Enlaza la información del combo
        /// </summary>
        private void CargarCombo()
        {
            List<FuenteFinanciacion> fuentes = General.getFuentes();

            fuentes.Insert(0, new FuenteFinanciacion() { IdFuente = 0, DescFuente = "" });

            ddlFuenteFinan.DataSource = fuentes;
            ddlFuenteFinan.DataTextField = "DescFuente";
            ddlFuenteFinan.DataValueField = "IdFuente";
            ddlFuenteFinan.DataBind();
        }

        /// <summary>
        /// Configura los tamaños máximos de los campos multiline
        /// </summary>
        void SetMaxLength()
        {
            string max = "250";
            txtReqTecnico.Attributes.Add("maxlength", max);
        }

        #endregion
    }
}