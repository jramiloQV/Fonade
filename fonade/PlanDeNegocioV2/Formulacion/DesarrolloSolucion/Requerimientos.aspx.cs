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
    public partial class Requerimientos : System.Web.UI.Page
    {
        #region Variables

        /// <summary>
        /// Usuario logueado
        /// </summary>
        protected FonadeUser usuario;

        /// <summary>
        /// Código tab
        /// </summary>
        int CodigoTab { get { return Constantes.CONST_Paso4Requerimientos; } }

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
        /// Listado de clientes ingresados en pregunta 14.2
        /// </summary>
        List<Datos.DataType.RequerimientosNeg> ListRequerimientos
        {
            get
            {
                return (List<Datos.DataType.RequerimientosNeg>)ViewState["ListRequerimientos"];
            }

            set
            {
                ViewState["ListRequerimientos"] = value;
            }
        }

        /// <summary>
        /// Formulario pestaña Capítulo IV
        /// </summary>
        ProyectoRequerimiento Formulario { get; set; }

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
        /// Total requerimientos Infraestructura
        /// </summary>
        public string TotalG1421 { get; set; }

        /// <summary>
        /// Total requerimientos Maquinaria y Equipo
        /// </summary>
        public string TotalG1422 { get; set; }

        /// <summary>
        /// Total requerimientos Equipo de Comunicación y Computación
        /// </summary>
        public string TotalG1423 { get; set; }

        /// <summary>
        /// Total requerimientos Muebles, Enseres y Otros
        /// </summary>
        public string TotalG1424 { get; set; }

        /// <summary>
        /// Total requerimientos Otros
        /// </summary>
        public string TotalG1425 { get; set; }

        /// <summary>
        /// Total requerimientos Gastos Preoperativos
        /// </summary>
        public string TotalG1426 { get; set; }


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
                        Formulario = RequerimientosNegocio.getFormulario(Encabezado.CodigoProyecto);

                        if (Formulario != null)
                        {
                            IdPrimario = Formulario.IdRequerimiento;
                            CargarFormulario();
                        }
                        else
                        {
                            IdPrimario = 0;
                        }
                    }

                    //Se realiza la carga de los requerimientos ingresados anteriormente
                    CargarRequerimientos();
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }
        
        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_Paso4Requerimientos;
        }

        protected void gwGrillas_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView gv = (GridView)sender;

            PaginarGrillas(gv, e);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //Se valida que hayan ingresado por lo menos un requerimiento
                if (ListRequerimientos.Count > 0)
                {
                    bool esNuevo = IdPrimario != 0 ? false : true;

                    Formulario = new ProyectoRequerimiento()
                    {
                        IdProyecto = Encabezado.CodigoProyecto,
                        LugarFisico = cke_Pregunta141.Text.Trim(),
                        TieneLugarFisico = ddlPregunta141.SelectedValue == "1" ? true : false
                    };

                    //Si es nuevo se crea el nuevo registro. Si no se actualiza
                    if (!esNuevo)
                    {
                        Formulario.IdRequerimiento = IdPrimario;
                    }

                    //De acuerdo al resultado se presenta el mensaje de la inserción y/o actualización
                    if (RequerimientosNegocio.setDatosFormulario(Formulario, esNuevo))
                    {
                        //Si es nuevo registro se consulta el id creado
                        if (esNuevo)
                        {
                            IdPrimario = RequerimientosNegocio.getFormulario(Encabezado.CodigoProyecto).IdRequerimiento;
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
                else
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(151), this, "Alert");
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnEditarRequerimiento_Click(object sender, EventArgs e)
        {
            try
            {
                int pos = ((int.Parse(HiddenWidth.Value) - 500) / 2) - 20;
                LinkButton btn = (LinkButton)sender;

                Fonade.Proyecto.Proyectos.Redirect(Response, "ReqNegocio.aspx?IdProyectoInfraestructura=" + btn.CommandArgument + "&IdProyecto=" + Encabezado.CodigoProyecto, "_Blank",
                    "width=650,height=650,top=100,left=" + pos);
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        protected void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            cke_Pregunta141.Text = "";
        }

        protected void btnEliminar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ImageButton btn = (ImageButton)sender;

                if (RequerimientosNegocio.delRequerimiento(int.Parse(btn.CommandArgument)))
                {
                    Utilidades.PresentarMsj(Mensajes.GetMensaje(9), this, "Alert");
                    CargarRequerimientos();
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

        protected void btnAddRequerimiento_Click(object sender, EventArgs e)
        {
            try
            {
                string tiporeq = "";
                int pos = ((int.Parse(HiddenWidth.Value) - 500) / 2) - 20;
                LinkButton btn = (LinkButton)sender;

                //Se envia el tipo de requerimiento según la grilla seleccionada

                switch (btn.CommandArgument)
                {
                    case "gwPregunta1421":
                        tiporeq = Constantes.CONST_Infraestructura_Adecuaciones.ToString();
                        break;
                    case "gwPregunta1422":
                        tiporeq = Constantes.CONST_MaquinariayEquipo.ToString();
                        break;
                    case "gwPregunta1423":
                        tiporeq = Constantes.CONST_EquiposComuniCompu.ToString();
                        break;
                    case "gwPregunta1424":
                        tiporeq = Constantes.CONST_MueblesEnseresOtros.ToString();
                        break;
                    case "gwPregunta1425":
                        tiporeq = Constantes.CONST_Otros.ToString();
                        break;
                    default:
                        tiporeq = Constantes.CONST_GastoPreoperativos.ToString();
                        break;
                }



                Fonade.Proyecto.Proyectos.Redirect(Response, "ReqNegocio.aspx?TipoReq=" + tiporeq + "&IdProyecto="+ Encabezado.CodigoProyecto, "_Blank",
                    "width=650,height=650,top=100,left=" + pos);
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Carga la información del formulario en caso de actualización o consulta
        /// </summary>
        private void CargarFormulario()
        {
            ddlPregunta141.SelectedValue = Formulario.TieneLugarFisico ? "1" : "0";
            cke_Pregunta141.Text = Formulario.LugarFisico;
        }

        /// <summary>
        /// Carga los requerimientos de un plan de negocio
        /// </summary>
        private void CargarRequerimientos()
        {
            ListRequerimientos = RequerimientosNegocio.getRequerimientos(Encabezado.CodigoProyecto, Constantes.CONST_PlanV2);

            GridView[] listagrillas = (UpdatePanel.Controls[0].Controls
                                        .OfType<GridView>()).ToArray();
            
            foreach (GridView item in listagrillas)
            {
                EnlazarGrillas(item);
            }

        }

        /// <summary>
        /// Página la grilla seleccionada
        /// </summary>
        /// <param name="gv">Grilla seleccionada</param>
        /// <param name="e">Evento paginación</param>
        private void PaginarGrillas(GridView gv, GridViewPageEventArgs e)
        {
            gv.PageIndex = e.NewPageIndex;
            EnlazarGrillas(gv);
        }

        /// <summary>
        /// Enlaza los datos a la grilla seleccionada
        /// </summary>
        /// <param name="gv">Grilla a enlazar</param>
        private void EnlazarGrillas(GridView gv)
        {
            gv.DataSource = SepararInfrestructura(gv.ID);
            gv.DataBind();
            gv.Columns[0].Visible = AllowUpdate;

        }

        /// <summary>
        /// De acuerdo al tipo de infraestructura retorna el listado de requerimientos
        /// </summary>
        /// <param name="nombreGrilla">Nombre de la grilla a presentar</param>
        /// <returns></returns>
        private List<RequerimientosNeg> SepararInfrestructura(string nombreGrilla)
        {
            List<RequerimientosNeg> lst = null;

            switch (nombreGrilla)
            {
                case "gwPregunta1421":
                    lst = (ListRequerimientos.Where(
                          reg => reg.CodTipoInfraestructura == Constantes.CONST_Infraestructura_Adecuaciones)
                          .Select(y => new RequerimientosNeg {
                              IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                              Cantidad = y.Cantidad,
                              CodTipoInfraestructura = y.CodTipoInfraestructura,
                              FuenteFinanciacion = y.FuenteFinanciacion,
                              IdFuente = y.IdFuente,
                              NomInfraestructura = y.NomInfraestructura,
                              RequisitosTecnicos = y.RequisitosTecnicos,
                              ValorUnidadCadena = y.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorUnidad = y.ValorUnidad
                          })).ToList();

                    TotalG1421 = "Total " + lst.Select(x => ((decimal)x.Cantidad * (decimal)x.ValorUnidad)).Sum().ToString("0,0.00", CultureInfo.InvariantCulture);

                    break;
                case "gwPregunta1422":
                    lst = (ListRequerimientos.Where(
                          reg => reg.CodTipoInfraestructura == Constantes.CONST_MaquinariayEquipo)
                          .Select(y => new RequerimientosNeg {
                              IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                              Cantidad = y.Cantidad,
                              CodTipoInfraestructura = y.CodTipoInfraestructura,
                              FuenteFinanciacion = y.FuenteFinanciacion,
                              IdFuente = y.IdFuente,
                              NomInfraestructura = y.NomInfraestructura,
                              RequisitosTecnicos = y.RequisitosTecnicos,
                              ValorUnidadCadena = y.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorUnidad = y.ValorUnidad

                          })).ToList();

                    TotalG1422 = "Total " + lst.Select(x => ((decimal)x.Cantidad * (decimal)x.ValorUnidad)).Sum().ToString("0,0.00", CultureInfo.InvariantCulture);
                    break;
                case "gwPregunta1423":
                    lst = (ListRequerimientos.Where(
                          reg => reg.CodTipoInfraestructura == Constantes.CONST_EquiposComuniCompu)
                          .Select(y => new RequerimientosNeg
                          {
                              IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                              Cantidad = y.Cantidad,
                              CodTipoInfraestructura = y.CodTipoInfraestructura,
                              FuenteFinanciacion = y.FuenteFinanciacion,
                              IdFuente = y.IdFuente,
                              NomInfraestructura = y.NomInfraestructura,
                              RequisitosTecnicos = y.RequisitosTecnicos,
                              ValorUnidadCadena = y.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorUnidad = y.ValorUnidad

                          })).ToList();

                    TotalG1423 = "Total " + lst.Select(x => ((decimal)x.Cantidad * (decimal)x.ValorUnidad)).Sum().ToString("0,0.00", CultureInfo.InvariantCulture);
                    break;
                case "gwPregunta1424":
                    lst = (ListRequerimientos.Where(
                          reg => reg.CodTipoInfraestructura == Constantes.CONST_MueblesEnseresOtros)
                          .Select(y => new RequerimientosNeg
                          {
                              IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                              Cantidad = y.Cantidad,
                              CodTipoInfraestructura = y.CodTipoInfraestructura,
                              FuenteFinanciacion = y.FuenteFinanciacion,
                              IdFuente = y.IdFuente,
                              NomInfraestructura = y.NomInfraestructura,
                              RequisitosTecnicos = y.RequisitosTecnicos,
                              ValorUnidadCadena = y.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorUnidad = y.ValorUnidad

                          })).ToList();

                    TotalG1424 = "Total " + lst.Select(x => ((decimal)x.Cantidad * (decimal)x.ValorUnidad)).Sum().ToString("0,0.00", CultureInfo.InvariantCulture);
                    break;
                case "gwPregunta1425":
                    lst = (ListRequerimientos.Where(
                          reg => reg.CodTipoInfraestructura == Constantes.CONST_Otros)
                          .Select(y => new RequerimientosNeg
                          {
                              IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                              Cantidad = y.Cantidad,
                              CodTipoInfraestructura = y.CodTipoInfraestructura,
                              FuenteFinanciacion = y.FuenteFinanciacion,
                              IdFuente = y.IdFuente,
                              NomInfraestructura = y.NomInfraestructura,
                              RequisitosTecnicos = y.RequisitosTecnicos,
                              ValorUnidadCadena = y.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorUnidad = y.ValorUnidad

                          })).ToList();

                    TotalG1425 = "Total " + lst.Select(x => ((decimal)x.Cantidad * (decimal)x.ValorUnidad)).Sum().ToString("0,0.00", CultureInfo.InvariantCulture);
                    break;
                default:
                    lst = (ListRequerimientos.Where(
                          reg => reg.CodTipoInfraestructura == Constantes.CONST_GastoPreoperativos)
                          .Select(y => new RequerimientosNeg
                          {
                              IdProyectoInfraestructura = y.IdProyectoInfraestructura,
                              Cantidad = y.Cantidad,
                              CodTipoInfraestructura = y.CodTipoInfraestructura,
                              FuenteFinanciacion = y.FuenteFinanciacion,
                              IdFuente = y.IdFuente,
                              NomInfraestructura = y.NomInfraestructura,
                              RequisitosTecnicos = y.RequisitosTecnicos,
                              ValorUnidadCadena = y.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture),
                              ValorUnidad = y.ValorUnidad

                          })).ToList();
                          
                    TotalG1426 =  "Total " + lst.Select(x => ((decimal)x.Cantidad * (decimal)x.ValorUnidad)).Sum().ToString("0,0.00", CultureInfo.InvariantCulture);
                    break;
            }

            return lst;
        }

        #endregion                    
    }
}