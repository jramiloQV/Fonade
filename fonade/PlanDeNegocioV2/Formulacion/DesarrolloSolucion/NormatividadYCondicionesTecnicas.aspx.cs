using Datos;
using Fonade.Account;
using Fonade.Clases;
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
    public partial class NormatividadYCondicionesTecnicas : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        /// <summary>
        /// Estado del proyecto
        /// </summary>
        public int IdDesarrolloProyecto { get; set; }

        /// <summary>
        /// Identificador primario del formulario en escenario de edición
        /// </summary>
        int IdPrimario
        {
            get
            {
                return (int)ViewState["IdPrimario"];
            }

            set
            {
                ViewState["IdPrimario"] = value;
            }
        }

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso3NormatividadCondicionesTecnicas; } }

        /// <summary>
        /// Identifica si el usuario logueado es miembro del equipo de proyecto
        /// </summary>
        public Boolean EsMiembro { get; set; }

        /// <summary>
        /// Identifica si un tab es realizado
        /// </summary>
        public Boolean EsRealizado { get; set; }

        /// <summary>
        /// Muestra u oculta el postit
        /// </summary>
        public Boolean PostitVisible
        {
            get
            {
                return EsMiembro && !EsRealizado;
            }
            set { }
        }

        /// <summary>
        /// Identifica si se permitió chequear el tab
        /// </summary>
        public Boolean AllowCheckTab { get; set; }

        /// <summary>
        /// Indica si permite la edición del formulario
        /// </summary>
        public Boolean AllowUpdate
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor);
            }
            set { }
        }

        /// <summary>
        /// Formulario pestaña 3 Capítulo IV 
        /// </summary>
        public ProyectoNormatividad Formulario { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            //Se almacena el usuario de la sesión
            usuario = HttpContext.Current.Session["usuarioLogged"] != null
                ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"]
                : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

            //Se captura el código del proyecto
            if (Request.QueryString.AllKeys.Contains("codproyecto"))
            {
                Encabezado.CodigoProyecto = int.Parse(Request.QueryString["codproyecto"].ToString());
                Encabezado.CodigoTab = CodigoTab;

                SetPostIt();

                //Se verifica si el usuario es miembro del proyecto y si ya se realizó el registro completo de la pestaña
                EsMiembro = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsMienbroDelProyecto(Encabezado.CodigoProyecto, usuario.IdContacto);
                EsRealizado = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.VerificarTabSiEsRealizado(CodigoTab, Encabezado.CodigoProyecto);
                AllowCheckTab = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.AllowCheckTab(usuario.CodGrupo, Encabezado.CodigoProyecto, CodigoTab, EsMiembro);

                //Se carga el formulario si es un escenario de edición
                if (!IsPostBack)
                {
                    //Se realiza la existencia de este formulario para este proyecto. Si existe se presenta
                    //en los controles
                    Formulario = NormatividadYCondicionTech.getFormulario(Encabezado.CodigoProyecto);

                    if (Formulario != null)
                    {
                        IdPrimario = Formulario.IdNormatividad;
                        CargarFormulario();
                    }
                    else
                    {
                        IdPrimario = 0;
                    }

                }
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Paso3NormatividadCondicionesTecnicas;
        }

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            CKENormEmpresa.Text = "";
            CKENormTribu.Text = "";
            CKNormTecnica.Text = "";
            CKNormLaboral.Text = "";
            CKENormAmbiental.Text = "";
            CKEMarcaProp.Text = "";
            CKEPregunta13.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                bool esNuevo = IdPrimario != 0 ? false : true;

                Formulario = new ProyectoNormatividad()
                {
                    Ambiental = CKENormAmbiental.Text.Trim(),
                    CondicionesTecnicas = CKEPregunta13.Text.Trim(),
                    Empresarial = CKENormEmpresa.Text.Trim(),
                    Laboral = CKNormLaboral.Text.Trim(),
                    RegistroMarca = CKEMarcaProp.Text.Trim(),
                    Tecnica = CKNormTecnica.Text.Trim(),
                    Tributaria = CKENormTribu.Text.Trim(),
                    IdProyecto = Encabezado.CodigoProyecto
                };

                //Si es nuevo se crea el nuevo registro. Si no se actualiza
                if (!esNuevo)
                {
                    Formulario.IdNormatividad = IdPrimario;
                }

                //De acuerdo al resultado se presenta el mensaje de la inserción y/o actualización
                if (NormatividadYCondicionTech.setDatosFormulario(Formulario, esNuevo))
                {
                    //Si es nuevo registro se consulta el id creado
                    if (esNuevo)
                    {
                        IdPrimario = NormatividadYCondicionTech.getFormulario(Encabezado.CodigoProyecto).IdNormatividad;
                    }

                    Utilidades.PresentarMsj(Mensajes.GetMensaje(8), this, "Alert");

                    //Se actualiza la última fecha de actualización y genera el registro del tab
                    Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);

                    //Actualiza la columna de completitud del tab
                    Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(CodigoTab, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                    //Actualiza la fecha de ultima actualización
                    Encabezado.ActualizarFecha();
                }
                else
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
                }

            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga el formulario en escenario de edición
        /// </summary>
        private void CargarFormulario()
        {
            CKENormEmpresa.Text = Formulario.Empresarial;
            CKENormTribu.Text = Formulario.Tributaria;
            CKNormTecnica.Text = Formulario.Tecnica;
            CKNormLaboral.Text = Formulario.Laboral;
            CKENormAmbiental.Text = Formulario.Ambiental;
            CKEMarcaProp.Text = Formulario.RegistroMarca;
            CKEPregunta13.Text = Formulario.CondicionesTecnicas;
        }

        #endregion

    }
}