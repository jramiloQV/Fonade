using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade
{
    public partial class Formulario_web1 : Negocio.Base_Page
    {
        int Id_Emprendedor;
        string NomEstado;
        bool EstPendiente;
        bool Estsubsanado;
        bool EstAcreditado;
        bool EstNoAcreditado;

        protected void Page_Load(object sender, EventArgs e)
        {
            Id_Emprendedor = HttpContext.Current.Session["Id_Emprendedor"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_Emprendedor"].ToString()) ? int.Parse(HttpContext.Current.Session["Id_Emprendedor"].ToString()) : 0;
            NomEstado = HttpContext.Current.Session["NomEstado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NomEstado"].ToString()) ? HttpContext.Current.Session["NomEstado"].ToString() : string.Empty;
            EstPendiente = HttpContext.Current.Session["imgPendiente"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgPendiente"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgPendiente"].ToString()) : false;
            Estsubsanado = HttpContext.Current.Session["imgsubsanado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgsubsanado"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgsubsanado"].ToString()) : false;
            EstAcreditado = HttpContext.Current.Session["imgAcreditado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgAcreditado"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgAcreditado"].ToString()) : false;
            EstNoAcreditado = HttpContext.Current.Session["imgNoAcreditado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgNoAcreditado"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgNoAcreditado"].ToString()) : false;

            lblEmprendedor.Text = (from c in consultas.Db.Contacto where c.Id_Contacto == Id_Emprendedor select (c.Nombres + " " + c.Apellidos)).First();
            lblEstado.Text = NomEstado;
            lblEstadoR.Text = NomEstado;

            switch (NomEstado)
            {
                case "Pendiente":
                    ckkEstado.Checked = EstPendiente;
                    break;
                case "Subsanado":
                    ckkEstado.Checked = Estsubsanado;
                    break;
                case "Acreditado":
                    ckkEstado.Checked = EstAcreditado;
                    break;
                case "NoAcreditado":
                    ckkEstado.Checked = EstNoAcreditado;
                    break;
            }
        }

        protected void btnGuardarEnviar_Click(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validarModificarContenido())
            {
                if (ckkEstado.Checked)
                {

                }
                else
                {

                }
            }
        }

        private bool validarModificarContenido()
        {
            bool result = true;

            if (ckkEstado.Checked && string.IsNullOrEmpty(txtObservacion.Text))
            {
                result = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Debe especificar una observación');", true);
                txtObservacion.Focus();
            }else if(!ckkEstado.Checked){
                result = puedeDesactivarse();
            }

            return result;
        }

        private bool puedeDesactivarse()
        {
            bool result = true;

            switch (NomEstado)
            {
                case "Subsanado":
                    if (EstPendiente && EstAcreditado)
                    {
                        result = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede Desactivar el estado \"Subsanado\" si se encuentran activados los estados \"Pendiente\" y \"Acreditado\"');", true);
                    }
                    break;
            }

            return result;
        }
    }
}