using Datos;
using Fonade.Account;
using Fonade.Clases;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.DesarrolloSolucion;
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
    public partial class IngresosYCondicionesComerciales : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso1IngresoCondicionesComerciales; } }

        /// <summary>
        /// Identifica si el usuario logueado es miembro del equipo de proyecto
        /// </summary>
        public Boolean EsMiembro { get; set; }

        /// <summary>
        /// Identifica si un tab es realizado
        /// </summary>
        public Boolean EsRealizado { get; set; }

        /// <summary>
        /// muestra u oculta el postit
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
        /// Listado de clientes ingresados en pregunta 1.1
        /// </summary>
        List<Datos.DataType.CondicionesCliente> ListClientes
        {
            get
            {
                return (List<Datos.DataType.CondicionesCliente>)ViewState["ListClientes"];
            }

            set
            {
                ViewState["ListClientes"] = value;
            }
        }

        /// <summary>
        /// Formulario pestaña Capítulo IV
        /// </summary>
        ProyectoDesarrolloSolucion Formulario{ get; set; }

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
        /// Indica si los clientes son o no consumidores
        /// </summary>
        public Boolean EsClienteConsumidor { get; set; }

        /// <summary>
        /// Indica si se diligenció la grilla de la pregunta 10 en su totalidad
        /// </summary>
        public Boolean CompletoGrilla { get; set; }

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
        /// Indica si permite la edición incluyendo a que los clientes tienen diferentes perfiles
        /// </summary>
        public Boolean AllowUpdateConsumidor
        {
            get
            {
                return EsMiembro && !EsRealizado && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor) && EsClienteConsumidor;
            }
            set { }
        }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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

                    if (!IsPostBack)
                    {
                        //Se realiza la existencia de este formulario para este proyecto. Si existe se presenta
                        //en los controles
                        Formulario = IngresosYCondicionesComercio.getFormulario(Encabezado.CodigoProyecto);

                        if(Formulario != null)
                        {
                            IdPrimario = Formulario.IdDesarrolloSolucion;
                            CargarFormulario();
                        }
                        else
                        {
                            IdPrimario = 0;
                        }
                    }

                    //Se determina si los clientes tienen la característica de consumidores
                    EsClienteConsumidor = IngresosYCondicionesComercio.esConsumidor(Encabezado.CodigoProyecto);

                    //Se desactiva las validaciones de las preguntas de consumidor si los clientes no manejan este perfil
                    if (!EsClienteConsumidor)
                    {
                        rvPtaConsumidor1.Enabled = false;
                        rvPtaConsumidor2.Enabled = false;
                        rvPtaConsumidor3.Enabled = false;
                        rvPtaConsumidor4.Enabled = false;
                    }

                    //Se realiza la carga de los clientes ingresados anteriormente en la pregunta 1
                    CargarClientes();

                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7),this,"Alert");
            }
        }


        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Paso1IngresoCondicionesComerciales;
        }

        protected void btnEditarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = ((int.Parse(HiddenWidth.Value) - 500) / 2) - 20;
                
                LinkButton btn = (LinkButton)sender;
                Fonade.Proyecto.Proyectos.Redirect(Response, "CondicionesCliente.aspx?IdCliente=" + btn.CommandArgument + "&CodigoProyecto=" + Encabezado.CodigoProyecto.ToString(), "_Blank",
                    "width=650,height=650,top=100,left=" + pos);
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Se validan los campos del formulario
                if (ValidarFormulario())
                {
                    bool esNuevo = IdPrimario != 0 ? false : true;

                    Formulario = new ProyectoDesarrolloSolucion()
                    {
                        CaracteristicasCompra = cke_PtaConsumidor2.Text.Trim(),
                        DondeCompra = cke_PtaConsumidor1.Text.Trim(),
                        FrecuenciaCompra = cke_PtaConsumidor3.Text.Trim(),
                        Ingresos = cke_Pregunta9.Text.Trim(),
                        Precio = cke_PtaConsumidor4.Text.Trim(),
                        IdProyecto = Encabezado.CodigoProyecto
                    };

                    //Si es nuevo se crea el nuevo registro. Si no se actualiza
                    if (!esNuevo)
                    {
                        Formulario.IdDesarrolloSolucion = IdPrimario;
                    }
                    
                    //De acuerdo al resultado se presenta el mensaje de la inserción y/o actualización
                    if (IngresosYCondicionesComercio.setDatosFormulario(Formulario, esNuevo))
                    {
                        //Si es nuevo registro se consulta el id creado
                        if(esNuevo)
                        {
                            IdPrimario = IngresosYCondicionesComercio.getFormulario(Encabezado.CodigoProyecto).IdDesarrolloSolucion;
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
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            cke_Pregunta9.Text = "";
            cke_PtaConsumidor1.Text = "";
            cke_PtaConsumidor2.Text = "";
            cke_PtaConsumidor3.Text = "";
            cke_PtaConsumidor4.Text = "";
        }

        protected void gw_pregunta10_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gw_pregunta10.PageIndex = e.NewPageIndex;
            gw_pregunta10.DataSource = ListClientes;
            gw_pregunta10.DataBind();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga las condiciones de los clientes
        /// </summary>
        private void CargarClientes()
        {
            //Se lista los clientes y sus condiciones
            ListClientes = (Negocio.PlanDeNegocioV2.DesarrolloSolucion.IngresosYCondicionesComercio.getCondicionesClientes(Encabezado.CodigoProyecto)
                .Select(x => new Datos.DataType.CondicionesCliente () { 
                    CaracteristicasCompra = x.CaracteristicasCompra,
                    Cliente = x.Cliente,
                    FormaPago = x.FormaPago,
                    FrecuenciaCompra = x.FrecuenciaCompra,
                    Garantias = x.Garantias,
                    IdCliente = x.IdCliente,
                    Margen = x.Margen,
                    Precio = x.Precio,
                    PrecioCadena = x.Precio.ToString("0,0.00", CultureInfo.InvariantCulture),
                    RequisitosPostVenta = x.RequisitosPostVenta,
                    SitioCompra = x.SitioCompra
                })).ToList();

            gw_pregunta10.DataSource = ListClientes;
            gw_pregunta10.DataBind();
        }

        /// <summary>
        /// Carga la información del formulario en caso de actualización o consulta
        /// </summary>
        private void CargarFormulario()
        {
            cke_Pregunta9.Text = Formulario.Ingresos;
            cke_PtaConsumidor1.Text = Formulario.DondeCompra;
            cke_PtaConsumidor2.Text = Formulario.CaracteristicasCompra;
            cke_PtaConsumidor3.Text = Formulario.FrecuenciaCompra;
            cke_PtaConsumidor4.Text = Formulario.Precio;
        }

        /// <summary>
        /// Realiza el llamado de la validación de la grilla
        /// </summary>
        private bool EstaCompletaGrilla()
        {
            bool operacionOk = true;

            try
            {
                //Si es cliente que tiene diferentes perfiles se valida
                if (EsClienteConsumidor)
                {
                    // Determina si la grilla fue ingresada en su totalidad
                    CompletoGrilla = ListClientes.Where(reg => reg.FrecuenciaCompra == null).Count() == 0;

                    if (!CompletoGrilla)
                    {
                        Utilidades.PresentarMsj(Mensajes.GetMensaje(102),this,"Alert");
                        operacionOk = false;
                    }
                }
            }
            catch (Exception)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7),this,"Alert");
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Valida el diligenciamiento correcto del formulario
        /// </summary>
        /// <returns>Verdadero si esta correcto. False en otro caso</returns>
        private bool ValidarFormulario()
        {
            bool operacionOk = true;

            try
            {

                //Se valida si la grilla de clientes se diligenció en su totalidad
                if (!EstaCompletaGrilla())
                {
                    operacionOk = false;
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;

        }

        #endregion
    }
}