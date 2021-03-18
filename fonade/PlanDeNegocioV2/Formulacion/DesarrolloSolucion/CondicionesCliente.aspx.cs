using Datos;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.DesarrolloSolucion;
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
    public partial class CondicionesCliente : System.Web.UI.Page
    {
        #region Variables

        public int IdCliente
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdCliente") ? int.Parse(Request.QueryString["IdCliente"].ToString()) : 0;
            }
        }

        private Datos.DataType.CondicionesCliente condicion { get; set; }

        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        public int CodigoProyecto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("CodigoProyecto") ? int.Parse(Request.QueryString["CodigoProyecto"].ToString()) : 0;
            }
        }

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso1IngresoCondicionesComerciales; } }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack && IdCliente > 0)
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

        protected void btnGuardarCondicionCliente_Click(object sender, EventArgs e)
        {

            ProyectoCondicionesComerciale item = new ProyectoCondicionesComerciale()
            {
                IdCliente = IdCliente,
                CaracteristicasCompra = txtCaracteristicasCompra.Text.Trim(),
                Garantias = txtGarantias.Text.Trim(),
                Margen = txtMargen.Text.Trim(),
                RequisitosPostVenta = txtReqPostVenta.Text.Trim(),
                FrecuenciaCompra = txtVolumenFrecuencia.Text.Trim(),
                FormaPago = txtFormaPago.Text.Trim(),
                SitioCompra = txtSitioCompra.Text.Trim(),
                Precio = Convert.ToDecimal(txtPrecio.Text.Replace(",", "").Replace(".", ","))
            };

            if (!IngresosYCondicionesComercio.setCondicionesCliente(item))
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
            else
            {
                //actualizar la grilla de la pagina principal
                Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "upd", "window.opener.__doPostBack('', 'updGrilla');", true);
                ClientScript.RegisterStartupScript(this.GetType(), "Close", "<script>window.close();</script> ");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga el formulario con los datos existentes o vacío si es nuevo
        /// </summary>
        private void CargarFormulario()
        {
            condicion = Negocio.PlanDeNegocioV2.DesarrolloSolucion.IngresosYCondicionesComercio.getCondicionCliente(IdCliente);

            txtNombreCliente.Text = condicion.Cliente;
            txtCaracteristicasCompra.Text = condicion.CaracteristicasCompra;
            txtGarantias.Text = condicion.Garantias;
            txtMargen.Text = condicion.Margen;
            txtReqPostVenta.Text = condicion.RequisitosPostVenta;
            txtVolumenFrecuencia.Text = condicion.FrecuenciaCompra;
            txtFormaPago.Text = condicion.FormaPago;
            txtSitioCompra.Text = condicion.SitioCompra;
            txtPrecio.Text = condicion.Precio.ToString("0,0.00", CultureInfo.InvariantCulture);
        }

        void SetMaxLength()
        {
            // se establece por codigo el max length ya que el control no lo carga en textarea
            string max = "250";
            txtCaracteristicasCompra.Attributes.Add("maxlength", max);
            txtFormaPago.Attributes.Add("maxlength", max);
            txtGarantias.Attributes.Add("maxlength", max);
            txtMargen.Attributes.Add("maxlength", max);
            txtReqPostVenta.Attributes.Add("maxlength", max);
            txtSitioCompra.Attributes.Add("maxlength", max);
            txtVolumenFrecuencia.Attributes.Add("maxlength", max);
        }

        #endregion

        
    }
}