using AjaxControlToolkit;
using Datos;
using Datos.DataType;
using Fonade.Account;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.Solucion;
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
    public partial class Produccion : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso5Produccion; } }

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
        /// Formulario pestaña 5 Capítulo IV
        /// </summary>
        ProyectoProduccion Formulario { get; set; }

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
        /// Listado de clientes ingresados en pregunta 14.2
        /// </summary>
        List<Datos.DataType.ProductoProceso> ListProcesos
        {
            get
            {
                return (List<Datos.DataType.ProductoProceso>)ViewState["ListProcesos"];
            }

            set
            {
                ViewState["ListProcesos"] = value;
            }
        }

        #endregion

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
                    Formulario = ProduccionNegocio.getFormulario(Encabezado.CodigoProyecto);

                    if (Formulario != null)
                    {
                        IdPrimario = Formulario.IdProduccion;
                        CargarFormulario();
                    }
                    else
                    {
                        IdPrimario = 0;
                    }
                }

                //Se crean los controles de los productos dinámicamente
                CargarProcesosProducto();
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Paso5Produccion;
        }

        /// <summary>
        /// Carga la información del formulario en caso de actualización o consulta
        /// </summary>
        private void CargarFormulario()
        {
            CKEPregunta143.Text = Formulario.CondicionesTecnicas;
            ddlPregunta144.SelectedValue = Formulario.RealizaImportacion ? "1" : "0";
            cke_Pregunta144.Text = Formulario.Justificacion;
            CKEPregunta1441.Text = Formulario.ActivosProveedores;
            CKEPregunta1442.Text = Formulario.IncrementoValor;

        }

        private void CargarProcesosProducto()
        {
            int cont = 1;
            string nombrectrl = "ckeProducto";
            string nombreval = "rvProducto";
            string nombrepan = "pan";

            //Se consulta los productos ingresados anteriormente
            ListProcesos = Producto.GetProductosProceso(Encabezado.CodigoProyecto);

            if (ListProcesos.Count > 0)
            {
                //Se realiza la creación de controles para presentar los productos en la pregunta 15
                foreach (ProductoProceso item in ListProcesos)
                {
                    //Acordeon pane y control texto enriquecido
                    AccordionPane ap1 = new AccordionPane()
                    {
                        ID = nombrepan + cont.ToString(),
                    };

                    CKEditor.NET.CKEditorControl ctrl = new CKEditor.NET.CKEditorControl();
                    ctrl.ID = nombrectrl + cont.ToString();
                    ctrl.Enabled = AllowUpdate;
                    ctrl.ValidationGroup = "grupo1";
                    ctrl.Attributes.Add("IdProducto", item.Id_Producto.ToString());
                    ctrl.Attributes.Add("IdProceso", item.Id_Proceso.ToString());

                    if(item.Id_Proceso != null)
                    {
                        ctrl.Text = item.DescProceso;
                    }

                    //validador
                    RequiredFieldValidator rv = new RequiredFieldValidator()
                    {
                        Display = ValidatorDisplay.None,
                        ErrorMessage = string.Format(Mensajes.GetMensaje(104), item.NomProducto),
                        ID = nombreval + cont.ToString(),
                        ForeColor = System.Drawing.Color.Red,
                        SetFocusOnError = true,
                        ControlToValidate = nombrectrl + cont.ToString(),
                        ToolTip = "Requerido",
                        ValidationGroup = "grupo1"
                    };

                    rv.Font.Bold = true;
                    rv.Font.Size = FontUnit.XLarge;

                    //Se adicionan los controles creados dinámicamente
                    ap1.HeaderContainer.Controls.Add(new LiteralControl(string.Format("{0} - {1}", item.NomProducto, item.Unidad)));
                    ap1.ContentContainer.Controls.Add(rv);
                    ap1.ContentContainer.Controls.Add(ctrl);
                    Accordion1.Panes.Add(ap1);

                    cont++;
                }
            }
        }

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            try
            {
                CKEPregunta143.Text = "";
                cke_Pregunta144.Text = "";
                CKEPregunta1441.Text = "";
                CKEPregunta1442.Text = "";

                foreach (AccordionPane item in Accordion1.Panes)
                {
                    CKEditor.NET.CKEditorControl ctrl = (CKEditor.NET.CKEditorControl) item.ContentContainer.Controls[1];
                    ctrl.Text = "";
                }

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
                List<ProyectoDetalleProceso> lst = new List<ProyectoDetalleProceso>();
                bool esNuevo = IdPrimario != 0 ? false : true;

                Formulario = new ProyectoProduccion()
                {
                    RealizaImportacion = ddlPregunta144.SelectedValue == "1" ? true : false,
                    ActivosProveedores = CKEPregunta1441.Text.Trim(),
                    CondicionesTecnicas = CKEPregunta143.Text.Trim(),
                    Justificacion = cke_Pregunta144.Text.Trim(),
                    IdProyecto = Encabezado.CodigoProyecto,
                    IncrementoValor = CKEPregunta1442.Text.Trim()
                    
                };

                //Se crean los detalles de los procesos en los productos

                foreach (AccordionPane item in Accordion1.Panes)
                {
                    CKEditor.NET.CKEditorControl ctrl = (CKEditor.NET.CKEditorControl)item.ContentContainer.Controls[1];

                    var reg = new ProyectoDetalleProceso()
                    {
                        DescripcionProceso = ctrl.Text.Trim(),
                        IdProducto = int.Parse(ctrl.Attributes["IdProducto"])
                    };

                    if (!ctrl.Attributes["IdProceso"].Equals(""))
                    {
                        reg.IdDetalleProceso = int.Parse(ctrl.Attributes["IdProceso"]);
                    }

                    lst.Add(reg);
                }

                //Si es nuevo se crea el nuevo registro. Si no se actualiza
                if (!esNuevo)
                {
                    Formulario.IdProduccion = IdPrimario;
                }

                //De acuerdo al resultado se presenta el mensaje de la inserción y/o actualización
                if (ProduccionNegocio.setDatosFormulario(Formulario, esNuevo))
                {
                    if (ProduccionNegocio.setDetalleProceso(lst))
                    {
                        //Si es nuevo registro se consulta el id creado
                        if (esNuevo)
                        {
                            IdPrimario = ProduccionNegocio.getFormulario(Encabezado.CodigoProyecto).IdProduccion;
                        }

                        Utilidades.PresentarMsj(Mensajes.GetMensaje(8), this, "Alert");

                        //Se actualiza la última fecha de actualización y genera el registro del tab
                        Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.UpdateTab(CodigoTab, Encabezado.CodigoProyecto, usuario.IdContacto, usuario.CodGrupo, false);

                        //Actualiza la columna de completitud del tab
                        Negocio.PlanDeNegocioV2.Utilidad.TabFormulacion.UpdateTabCompleto(CodigoTab, Encabezado.CodigoProyecto, usuario.IdContacto, true);

                        //Actualiza la fecha de ultima actualización
                        Encabezado.ActualizarFecha();
                    }
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
    }
}