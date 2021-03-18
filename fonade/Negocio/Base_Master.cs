using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using Datos;
using System.Web.Security;

namespace Fonade.Negocio
{

    /// <summary>
    /// clase que hereda de masterpage para la validacion de credenciales
    /// </summary>
    public class Base_Master : System.Web.UI.MasterPage
    {
        protected FonadeUser usuario;
        protected Consultas consultas;
        protected string errormessagedetail;
        protected override void OnLoad(EventArgs e)
        {
            consultas = new Consultas();
            usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            if (usuario == null)
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                base.OnLoad(e);
            }               
        }
    }
}