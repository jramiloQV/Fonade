using Datos;
using Datos.DataType;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
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
    public partial class ProductividadYEquipoDeTrabajo : System.Web.UI.Page
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
        /// Listado de emprendedores y sus perfiles en el plan de negocio
        /// </summary>
        List<Datos.DataType.EquipoTrabajo> ListEmprendedores
        {
            get
            {
                return (List<Datos.DataType.EquipoTrabajo>)ViewState["ListEmprendedores"];
            }

            set
            {
                ViewState["ListEmprendedores"] = value;
            }
        }

        /// <summary>
        /// Listado de cargos del plan de negocio
        /// </summary>
        List<ProyectoGastosPersonal> ListCargos { get; set; }

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso6ProductividadEquipoDeTrabajo; } }

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
        /// Formulario pestaña 6 Capítulo IV 
        /// </summary>
        public ProyectoProductividad Formulario { get; set; }

        /// <summary>
        /// Total solicitado fondo emprender
        /// </summary>
        public string TotalFondoEmprender { get; set; }

        /// <summary>
        /// Total aportes emprendedor
        /// </summary>
        public string TotalAportesEmprendedor { get; set; }

        /// <summary>
        /// Total ingresos por ventas
        /// </summary>
        public string TotalIngresosVentas { get; set; }

        #endregion

        #region Eventos

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
                        Formulario = Productividad.getFormulario(Encabezado.CodigoProyecto);

                        if (Formulario != null)
                        {
                            IdPrimario = Formulario.IdProductividad;
                            CargarFormulario();
                        }
                        else
                        {
                            IdPrimario = 0;
                        }
                    }

                    //Se realiza la carga de los emprendedores activos del plan del proyecto
                    CargarEmprendedores();

                    //Se realiza la carga de los cargos que tiene el plan de negocio
                    CargarCargos();
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }

            //Se almacena el usuario de la sesión

        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Paso6ProductividadEquipoDeTrabajo;
        }

        protected void btnEditarEquipo_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = ((int.Parse(HiddenWidth.Value) - 500) / 2) - 20;

                LinkButton btn = (LinkButton)sender;

                string id = btn.CommandArgument.Split('|')[0].Trim().Count() > 0 ? btn.CommandArgument.Split('|')[0] : "0";
                string idContacto = btn.CommandArgument.Split('|')[2].Trim().Count() > 0 ? btn.CommandArgument.Split('|')[2] : "0";
                string nombre = btn.CommandArgument.Split('|')[1];

                Fonade.Proyecto.Proyectos.Redirect(Response, "PerfilEmprendedor.aspx?IdEmprendedorPerfil=" + id + "&NomEmprendedor=" + nombre + "&IdContacto=" + idContacto + "&CodigoProyecto=" + Encabezado.CodigoProyecto.ToString(), "_Blank",
                    "width=650,height=350,top=100,left=" + pos);
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            cke_Pregunta16.Text = "";
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Se validan la completitud de la grilla
                if (EstaCompletaGrilla())
                {
                    bool esNuevo = IdPrimario != 0 ? false : true;

                    Formulario = new ProyectoProductividad()
                    {
                        IdProyecto = Encabezado.CodigoProyecto,
                        CapacidadProductiva = cke_Pregunta16.Text.Trim()
                    };

                    //Si es nuevo se crea el nuevo registro. Si no se actualiza
                    if (!esNuevo)
                    {
                        Formulario.IdProductividad = IdPrimario;
                    }

                    //De acuerdo al resultado se presenta el mensaje de la inserción y/o actualización
                    if (Productividad.setDatosFormulario(Formulario, esNuevo))
                    {
                        //Si es nuevo registro se consulta el id creado
                        if (esNuevo)
                        {
                            IdPrimario = Productividad.getFormulario(Encabezado.CodigoProyecto).IdProductividad;
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

        protected void btnAddCargo172_Click(object sender, EventArgs e)
        {
            AbrirVentanaCargos("", true, true);

        }

        protected void btnEditar172_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            AbrirVentanaCargos(btn.CommandArgument, true, false);
        }

        protected void btnDetalle172_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            AbrirVentanaCargos(btn.CommandArgument, false, false);
        }

        protected void btnEliminar172_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (gwPregunta172.Rows.Count == 1)
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(14), this, "Alert");
                }
                else
                {
                    ImageButton btn = (ImageButton)sender;

                    if (Productividad.delCargo(int.Parse(btn.CommandArgument)))
                    {
                        Utilidades.PresentarMsj(Mensajes.GetMensaje(9), this, "Alert");
                        CargarCargos();
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

        protected void gwPregunta172_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwPregunta172.PageIndex = e.NewPageIndex;
            CargarCargos();
        }

        protected void gw_pregunta171_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gw_pregunta171.PageIndex = e.NewPageIndex;
            CargarEmprendedores();
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga el listado de emprendedores y sus perfiles
        /// </summary>
        private void CargarEmprendedores()
        {
            ListEmprendedores = Negocio.PlanDeNegocioV2.Utilidad.General.getEquipoTrabajo(Encabezado.CodigoProyecto);

            gw_pregunta171.DataSource = ListEmprendedores;
            gw_pregunta171.DataBind();
        }

        /// <summary>
        /// Carga el listado de emprendedores y sus perfiles
        /// </summary>
        private void CargarCargos()
        {
            ListCargos = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Productividad.getCargos(Encabezado.CodigoProyecto);

            List<CargosPlanNegocio> lst = ListCargos.Select(
                   x => new CargosPlanNegocio()
                   {
                       AportesEmprendedorCadena = x.AportesEmprendedor.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                       IngresosVentasCadena = x.IngresosVentas.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                       OtrosGastosCadena = x.OtrosGastos.ToString("0,0.00", CultureInfo.InvariantCulture),
                       Cargo = x.Cargo,
                       TiempoVinculacion = x.TiempoVinculacion,
                       UnidadTiempo = x.UnidadTiempo == Constantes.CONST_UniTiempoMes ? "Mes" : "Días",
                       Id_Cargo = x.Id_Cargo,
                       ValorFondoEmprenderCadena = x.ValorFondoEmprender.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                       ValorRemunCadena = x.ValorRemuneracion.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                       ValorPrestacionesCadena = (x.ValorRemuneracion + x.OtrosGastos).Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                       ValorRemunPrimerAnioCadena = ((x.ValorRemuneracion + x.OtrosGastos) * x.TiempoVinculacion).Value.ToString("0,0.00", CultureInfo.InvariantCulture)
                   }
                ).ToList();

            //Se calculan los totales

            TotalAportesEmprendedor = ListCargos.Sum(x => x.AportesEmprendedor).Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            TotalFondoEmprender = ListCargos.Sum(x => x.ValorFondoEmprender).Value.ToString("0,0.00", CultureInfo.InvariantCulture);
            TotalIngresosVentas = ListCargos.Sum(x => x.IngresosVentas).Value.ToString("0,0.00", CultureInfo.InvariantCulture);

            gwPregunta172.DataSource = lst;
            gwPregunta172.DataBind();

            gwPregunta172.Columns[0].Visible = !AllowUpdate;
            gwPregunta172.Columns[1].Visible = AllowUpdate;

        }

        /// <summary>
        /// Realiza el llamado de la validación de la grilla
        /// </summary>
        private bool EstaCompletaGrilla()
        {
            bool operacionOk = true;

            try
            {
                // Determina si la grilla fue ingresada en su totalidad
                bool completoGrilla = ListEmprendedores.Where(reg => reg.Perfil == null).Count() == 0;

                if (!completoGrilla)
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(128), this, "Alert");
                    operacionOk = false;
                }
                else
                {
                    //Validar que se haya ingresado al menos un cargo 
                    completoGrilla = gwPregunta172.Rows.Count > 0;

                    if (!completoGrilla)
                    {
                        Utilidades.PresentarMsj(Mensajes.GetMensaje(152), this, "Alert");
                        operacionOk = false;
                    }

                }

            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Carga la información del formulario en caso de actualización o consulta
        /// </summary>
        private void CargarFormulario()
        {
            cke_Pregunta16.Text = Formulario.CapacidadProductiva;
        }

        private void AbrirVentanaCargos(string argumento, bool esEdicion, bool esNuevo)
        {
            try
            {
                int pos = ((int.Parse(HiddenWidth.Value) - 500) / 2) - 20;
                string cadRedirect = null;

                if (esNuevo)
                {
                    cadRedirect = "CargosProyecto.aspx?CodigoProyecto=" + Encabezado.CodigoProyecto + "&EsEdicion=" + esEdicion.ToString();
                }
                else
                {
                    cadRedirect = "CargosProyecto.aspx?CodigoProyecto=" + Encabezado.CodigoProyecto + "&EsEdicion=" + esEdicion.ToString() + "&IdCargo=" + argumento;
                }

                Fonade.Proyecto.Proyectos.Redirect(Response, cadRedirect, "_Blank",
                    "width=650,height=650,top=100,left=" + pos);
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }


        #endregion










    }
}