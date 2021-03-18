using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Fiduciaria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Fiduciaria
{
    public partial class BuscarInfoPago : Negocio.Base_Page//System.Web.UI.Page
    {
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //Recuperar la url
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))                
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }

            }
        }

        private void buscarInfo(int _codPago, int? _codOperador)
        {
            PagoBeneficiarioModel beneficiarioModel = FiduciariaBLL.getBeneficiario(_codPago, _codOperador);
            if (beneficiarioModel!=null) {
                txtCodPagoBusqueda.Text = beneficiarioModel.codPago.ToString();
                txtActividad.Text = beneficiarioModel.pagoActividad;
                txtcodProyecto.Text = beneficiarioModel.codProyecto.ToString();
                txtNomProyecto.Text = beneficiarioModel.nomProyecto;
                txtTipoIDBeneficiario.Text = beneficiarioModel.tipoIdentificacionBen;
                txtIDBeneficiario.Text = beneficiarioModel.IdentificacionBeneficiario;
                txtNombresBen.Text = beneficiarioModel.NombresBeneficiario;
                txtRazonSocial.Text = beneficiarioModel.RazonSocialBeneficiario;
                txtBancoBeneficiario.Text = beneficiarioModel.BancoBeneficiario;
                txtNumCuenta.Text = beneficiarioModel.NumcuentaBeneficiario;
                panelBuscqueda.Visible = true;
                lblMensaje.Visible = false;
            }
            else
            {
                panelBuscqueda.Visible = false;
                lblMensaje.Visible = true; ;
            }
            
        }

        protected void btnBuscarPago_Click(object sender, EventArgs e)
        {
            int codigo = 0;
            
            if (int.TryParse(txtCodPago.Text, out codigo)) {
                buscarInfo(codigo, usuario.CodOperador);
            }
            
        }
    }
}