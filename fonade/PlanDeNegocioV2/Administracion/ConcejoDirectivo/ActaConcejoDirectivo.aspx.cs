using Datos;
using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo
{
    public partial class ActaConcejoDirectivo : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        public Boolean AllowUpdate
        {
            get
            {
                //return usuario.CodGrupo.Equals(Constantes.CONST_AdministradorFonade) || usuario.CodGrupo.Equals(Constantes.CONST_GerenteEvaluador);
                return validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta);
            }
            set { }
        }
        string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
            }
        }

        public List<ActaConcejo> Get(int startIndex, int maxRows)
        {
            return Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Get(startIndex, maxRows, usuario.CodOperador);
        }

        public int Count()
        {
            return Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Count();
        }

        protected void imgAdd_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ActaDetalle.aspx?codacta=0");
        }

        protected void lnkAdd_Click(object sender, EventArgs e)
        {
            Response.Redirect("ActaDetalle.aspx?codacta=0");
        }

        protected void gvActas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Eliminar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoActa = Convert.ToInt32(e.CommandArgument.ToString());
                        Negocio.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ConcejoDirectivo.Delete(codigoActa);
                        gvActas.DataBind();                        
                    }
                }
                if (e.CommandName.Equals("actualizar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoActa = Convert.ToInt32(e.CommandArgument.ToString());

                        Response.Redirect("ActaDetalle.aspx?codacta=" + codigoActa);
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }
}