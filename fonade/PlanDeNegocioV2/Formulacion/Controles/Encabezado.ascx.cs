using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using Datos;
using System.Web.Security;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;

namespace Fonade.Controles
{
    public partial class Encabezado : System.Web.UI.UserControl
    {


        [ContextStatic]
        protected FonadeUser usuario;

        private Boolean EsMiembro;

        private Boolean EsRealizado;

        private Boolean AllowCheckTab;

        public int CodigoProyecto { get; set; }
        public int CodigoTab { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

            try
            {
                if (CodigoProyecto <= 0)
                    throw new ApplicationException("No se pudo obtener el código del proyecto, inténtenlo de nuevo.");

                EsMiembro = ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, usuario.IdContacto);
                EsRealizado = ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, CodigoProyecto);
                AllowCheckTab = ProyectoGeneral.AllowCheckTab(usuario.CodGrupo, CodigoProyecto, CodigoTab, EsMiembro);
                var isAsesorLider = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsUsuarioLider(CodigoProyecto, usuario.IdContacto);
                btnUpdateTab.Visible = AllowCheckTab && isAsesorLider;
                chkEsRealizado.Enabled = AllowCheckTab && isAsesorLider;
                BtnNuevoDocumento.Visible = EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);

                bool stateChk = chkEsRealizado.Checked;
                ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
                lblUltimaActualizacion.Text = lblUltimaActualizacion.Text.ToUpper();

                if (GetPostBackControl(Page) == btnUpdateTab.ID)
                    chkEsRealizado.Checked = stateChk;

            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        public static string GetPostBackControl(Page page)
        {
            Control control = null;

            string ctrlname = page.Request.Params.Get("__EVENTTARGET");
            if (ctrlname != null && ctrlname != string.Empty)
            {
                control = page.FindControl(ctrlname);
            }
            else
            {
                foreach (string ctl in page.Request.Form)
                {
                    Control c = page.FindControl(ctl);
                    if (c is System.Web.UI.WebControls.Button)
                    {
                        control = c;
                        break;
                    }
                }
            }
            return control != null ? control.ID : "";
        }

        protected void btnUpdateTab_Click(object sender, EventArgs e)
        {
            try
            {
                //verificar que la pestaña este completa para marcarla como realizado
                if (chkEsRealizado.Checked && !Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.VerificarTabSiEstaCompleta(CodigoTab, CodigoProyecto))
                {
                    chkEsRealizado.Checked = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + Negocio.Mensajes.Mensajes.GetMensaje(19) + "');", true);
                    return;
                }

                ProyectoGeneral.UpdateTab(CodigoTab, CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, chkEsRealizado.Checked);
                ActualizarFecha();

                //Recarga la pagina
                Response.Redirect(Request.RawUrl);
            }
            catch (ApplicationException ex)
            {
                chkEsRealizado.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                chkEsRealizado.Checked = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void BtnNuevoDocumento_Click(object sender, ImageClickEventArgs e)
        {
            Fonade.Proyecto.Proyectos.Redirect(Response, "~/PlanDeNegocioV2/Formulacion/Anexos/CatalogoAnexosV2.aspx?Accion=Nuevo&TabInvoca=Anexos&CodigoTab=" + CodigoTab.ToString() + "&CodigoProyecto=" + CodigoProyecto.ToString(), "_Blank",
                    "width=663,height=650,top=100,left=200");

        }

        protected void BtnVerDocumentos_Click(object sender, ImageClickEventArgs e)
        {
            Fonade.Proyecto.Proyectos.Redirect(Response, "~/PlanDeNegocioV2/Formulacion/Anexos/CatalogoAnexosV2.aspx?Accion=Vista&TabInvoca=Anexos&CodigoTab=" + CodigoTab.ToString() + "&CodigoProyecto=" + CodigoProyecto.ToString(), "_Blank",
                    "width=663,height=650,top=100,left=200");

        }

        /// Redireccion url
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="windowFeatures"></param>
        public static void Redirect(HttpResponse response, string url, string target, string windowFeatures)
        {

            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }

        public void ActualizarFecha()
        {
            ProyectoGeneral.GetUltimaActualizacion(lblUltimaActualizacion, lblFechaUltimaActualizacion, chkEsRealizado, btnUpdateTab, CodigoTab, CodigoProyecto);
            lblUltimaActualizacion.Text = lblUltimaActualizacion.Text.ToUpper();
        }
    }
}