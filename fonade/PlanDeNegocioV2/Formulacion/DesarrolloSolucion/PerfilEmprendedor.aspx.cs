using Datos;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public partial class PerfilEmprendedor : System.Web.UI.Page
    {
        #region Variables

        public int IdEmprendedorPerfil
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdEmprendedorPerfil") 
                    ? int.Parse(Request.QueryString["IdEmprendedorPerfil"].ToString()) : 0;
            }
        }

        public int IdContacto
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("IdContacto")
                    ? int.Parse(Request.QueryString["IdContacto"].ToString()) : 0;
            }
        }

        public string NomEmprendedor
        {
            get
            {
                return Request.QueryString.AllKeys.Contains("NomEmprendedor")
                    ? Request.QueryString["NomEmprendedor"].ToString() : "";
            }
        }

        private Datos.DataType.EquipoTrabajo Perfil { get; set; }

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
        int CodigoTab { get { return Constantes.CONST_Paso6ProductividadEquipoDeTrabajo; } }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               if(!IsPostBack && IdEmprendedorPerfil > 0)
               {
                   CargarFormulario();
               }
               else
               {
                   txtNombreEmprendedor.Text = NomEmprendedor.Trim();
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
            ProyectoEmprendedorPerfil item = new ProyectoEmprendedorPerfil()
            {
                IdContacto = IdContacto,
                Perfil = txtPerfil.Text.Trim(),
                Rol = txtRol.Text.Trim()
            };

            if(IdEmprendedorPerfil > 0)
            {
                item.IdEmprendedorPerfil = IdEmprendedorPerfil;
            }

            if (!Productividad.setDatosPerfil(item))
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

        /// <summary>
        /// Carga el formulario con los datos existentes o vacío si es nuevo
        /// </summary>
        private void CargarFormulario()
        {
            Perfil = Negocio.PlanDeNegocioV2.Utilidad.General.getPerfilEmprendedor(IdEmprendedorPerfil);

            txtNombreEmprendedor.Text = Perfil.NombreEmprendedor;
            txtPerfil.Text = Perfil.Perfil;
            txtRol.Text = Perfil.Rol;
        }

        /// <summary>
        /// Configura los tamaños máximos de los campos multiline
        /// </summary>
        void SetMaxLength()
        {
            string max = "1000";
            txtPerfil.Attributes.Add("maxlength", max);
        }
    }
}