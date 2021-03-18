#region using

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Configuration;
using System.Web;
using Fonade.Clases;

#endregion

namespace Fonade.FONADE.interventoria
{
    //modificado por diana zapata
    public partial class FrameNominaInter : Negocio.Base_Page
    {
        #region Propiedades.
        public int prorroga;
        public int prorrogaTotal;
        private int CodCargo
        {
            get { return (int)ViewState["cargo"]; }
            set { ViewState["cargo"] = value; }
        }

        private int CodProyecto;

        //private int CodProyecto
        //{
        //    get { return (int)ViewState["proyecto"]; }
        //    set { ViewState["proyecto"] = value; }
        //}

        private int CodNomina
        {
            get { return (int)ViewState["nomina"]; }
            set { ViewState["nomina"] = value; }
        }

        /// <summary>
        /// Mes obtenido de la depuración del código.
        /// </summary>
        private int Mes_Obtenido;

        /// <summary>
        /// Valor que define el estado de la página actual, así como el tipo de pago "variable"
        /// a enviar a las páginas "PagosActividad.aspx" y "PagosActividadInter.aspx".
        /// </summary>
        public int Estado;
        /// <summary>
        /// Obtiene el valor establecido según la pestaña activa.
        /// </summary>
        public int txtTab;

        double totalFEGlobal = 0;
        double totalEmpGlobal = 0;

        #endregion

        #region Propiedades de grilla "gv_nomina" (para detectar los campos con ID "fondo...")

        private int CodEmpresa
        {
            get { return (int)ViewState["empresa"]; }
            set { ViewState["empresa"] = value; }
        }
        private int Prorroga
        {
            get { return (int)ViewState["prorroga"]; }
            set { ViewState["prorroga"] = value; }
        }
        private int CodProyecto_PRM
        {
            get { return (int)ViewState["proyecto"]; }
            set { ViewState["proyecto"] = value; }
        }
        public string Sfondo1F
        {
            get { return ViewState["sfondo1f"].ToString(); }
            set { ViewState["sfondo1f"] = value; }
        }
        public string Sfondo2F
        {
            get { return ViewState["sfondo2F"].ToString(); }
            set { ViewState["sfondo2F"] = value; }
        }
        public string Sfondo3F
        {
            get { return ViewState["sfondo3F"].ToString(); }
            set { ViewState["sfondo3F"] = value; }
        }
        public string Sfondo4F
        {
            get { return ViewState["sfondo4F"].ToString(); }
            set { ViewState["sfondo4F"] = value; }
        }
        public string Sfondo5F
        {
            get { return ViewState["sfondo5F"].ToString(); }
            set { ViewState["sfondo5F"] = value; }
        }
        public string Sfondo6F
        {
            get { return ViewState["sfondo6F"].ToString(); }
            set { ViewState["sfondo6F"] = value; }
        }
        public string Sfondo7F
        {
            get { return ViewState["sfondo7F"].ToString(); }
            set { ViewState["sfondo7F"] = value; }
        }
        public string Sfondo8F
        {
            get { return ViewState["sfondo8F"].ToString(); }
            set { ViewState["sfondo8F"] = value; }
        }
        public string Sfondo9F
        {
            get { return ViewState["sfondo9F"].ToString(); }
            set { ViewState["sfondo9F"] = value; }
        }
        public string Sfondo10F
        {
            get { return ViewState["sfondo10F"].ToString(); }
            set { ViewState["sfondo10F"] = value; }
        }
        public string Sfondo11F
        {
            get { return ViewState["sfondo11F"].ToString(); }
            set { ViewState["sfondo11F"] = value; }
        }
        public string Sfondo12F
        {
            get { return ViewState["sfondo12F"].ToString(); }
            set { ViewState["sfondo12F"] = value; }
        }
        public string Sfondo13F
        {
            get { return ViewState["sfondo13F"].ToString(); }
            set { ViewState["sfondo13F"] = value; }
        }
        public string Sfondo14F
        {
            get { return ViewState["sfondo14F"].ToString(); }
            set { ViewState["sfondo14F"] = value; }
        }
        private int Mostrar { get; set; }

        #endregion

        public bool perfilVisibilidad { get{ return usuario.CodGrupo == Constantes.CONST_AdministradorSistema; } }

        /// <summary>
        /// Page_Load.
        /// Mauricio Arias Olave.
        /// 24/05/2014
        /// Cargar la grilla dinámica.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Inicializar variables para generar tabla dinámica.
            CodProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]??0);
            if (CodProyecto.ToString() == "0" || CodProyecto.ToString() == null) { CodProyecto = 0; }

            #region GENERAR GRILLA DINÁMICA DESCOMENTADO.
            //Nómina seleccionada.
            string interno_CodNomina = Convert.ToString(HttpContext.Current.Session["interno_CodNomina"]??"0");
            if (interno_CodNomina != "0")
            {
                //t_anexos = new Table();
                //Llamar a método para activar botón.

                GenerarTabla(interno_CodNomina, CodProyecto.ToString(), HttpContext.Current.Session["EsCargo"].ToString());
            }
            #endregion

            #region Establecer el "Estado" y el "txtTab" de la ventana actual.

            switch (2) //Nómina / Default.
            {
                case 2:
                    txtTab = Constantes.CONST_NominaInter;
                    Estado = 2;
                    break;
                case 3:
                    txtTab = Constantes.CONST_ProduccionInter;
                    Estado = 3;
                    break;
                case 4:
                    txtTab = Constantes.CONST_VentasInter;
                    Estado = 4;
                    break;
                default:
                    txtTab = Constantes.CONST_PlanOperativoInter2;
                    Estado = 1;
                    break;
            }

            #endregion

            if (!IsPostBack)
            {
                t_anexos.Rows.Clear();
                try
                {
                    CargarGridPersonalCalificado();
                    CargarGridManoObra();
                    CargarGridManoDeObra_CargosEnAprobacion();

                    //Evaluar si ciertos campos se mostrarán.
                    EvaluarCampos(usuario.CodGrupo);

                    //Obtener conteo de puestos pendientes por aprobar.
                    if (GrvActividadesNoAprobadas.Rows.Count > 0)
                    { lblpuestosPendientesConteo.Text = "Puestos Pendientes de Aprobar: " + GrvActividadesNoAprobadas.Rows.Count; }
                    else { lblpuestosPendientesConteo.Text = "Puestos Pendientes de Aprobar: 0"; }

                    if (usuario.CodGrupo != Constantes.CONST_Interventor)
                    {
                        gv_personalCalificado.Columns[0].Visible = false; tb_add.Visible = false;
                        lbl_cargosApr.Text = "Mano de Obra Directa";
                        lbl_cargosApr.Style.Clear();
                        lbl_cargosApr.Style.Add("background-color", "#FFFFFF");
                        lbl_cargosApr.Style.Add("font-weight", "bold");
                        lbl_cargosApr.Style.Add("color", "#00468f");
                    }
                    else
                    {
                        lbl_cargosApr.Text = "Cargos en Aprobación";
                        lbl_cargosApr.Style.Clear(); 
                        lbl_cargosApr.Style.Add("background-color", "#C00000");
                        lbl_cargosApr.Style.Add("color", "#ffffff");

                    }
                }
                catch { Response.Redirect("~/Account/Login.aspx"); }
            }
            else { /*Evaluar si ciertos campos se mostrarán.*/                EvaluarCampos(usuario.CodGrupo); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/04/2014.
        /// Cargar la grilla lateral izquierda.
        /// </summary>
        protected void CargarGridPersonalCalificado()
        {
            //Inicializar variables.
            DataTable respuestaDetalle = new DataTable();
            String txtSQL = "";

            #region consulta a la BD. COMENTADO.

            //consultas.Parameters = new[]
            //                               {
            //                                   new SqlParameter
            //                                       {
            //                                           ParameterName = "@CodProyecto" ,Value = CodProyecto //CodCargo
            //                                       }
            //                               };

            //respuestaDetalle = consultas.ObtenerDataTable("MD_ListaDePersonalCalificado_Nomina");

            //if (respuestaDetalle.Rows.Count != 0)
            //{
            //    gv_personalCalificado.DataSource = respuestaDetalle;
            //    gv_personalCalificado.DataBind();
            //}

            #endregion

            #region Bindear a la grilla de "Cargo" (grilla de arriba).

            txtSQL = " SELECT * FROM InterventorNomina WHERE Tipo='Cargo' AND codproyecto = " + CodProyecto +
                     " ORDER BY id_nomina ";

            respuestaDetalle = consultas.ObtenerDataTable(txtSQL, "text");

            gv_personalCalificado.DataSource = respuestaDetalle;
            gv_personalCalificado.DataBind();

            respuestaDetalle = null;

            #endregion
        }

        /// <summary>
        /// Paginación de la grilla de personal calificado "grilla ubicada a la izquierda de la pantalla".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_personalCalificado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            #region nueva pagina personal calificado
            gv_personalCalificado.PageIndex = e.NewPageIndex;
            CargarGridPersonalCalificado();
            #endregion
        }

        private void CargarGridManoObra()
        {
            var datos = (from n in consultas.Db.InterventorNominas
                         where n.Tipo == "Insumo" && n.CodProyecto == CodProyecto
                         select n).ToList();
            grvManoObraDirecta.DataSource = datos;
            grvManoObraDirecta.DataBind();
        }

        /// <summary>
        /// Método que genera una ventana emergente de la llamada "CatalogoInterventorTMP.aspx".
        /// </summary>
        void Cargar()
        {
            #region CatalogoInterventorTMP.aspx
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["pagina"] = "Nomina";
            HttpContext.Current.Session["v_Tipo"] = "Cargo";
            //Versión incompleta.
            //Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
            //             "menubar=0,scrollbars=1,width=710,height=400,top=100");
            //Nueva redirección a la página que es la correcta:
            Redirect(null, "../evaluacion/CatalogoInterventorTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=980,height=400,top=100");
            #endregion
        }

        /// <summary>
        /// Método que recibe el codigo del elemento a procesar yel mes del elemento seleccionado
        /// para devolver un valor que será usado para habilitar el acceso a los avances del elemento.
        /// </summary>
        /// <param name="codactividad">Código del elemento.</param>
        /// <param name="mes">Mes del elemento seleccionado.</param>
        /// <returns>Número decimal.</returns>
        public decimal Avance(int codactividad, int mes)
        {
            #region
            //Inicializar variable.
            decimal dtAvance = 0;

            try
            {
                var query = consultas.Db.AvanceCargoPOMes.FirstOrDefault(am => am.CodCargo == codactividad && am.Mes == mes);

                if (query != null && query.CodCargo != 0)
                { dtAvance = query.Valor; }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dtAvance;
            #endregion
        }

        #region Crear Cargos.

        /// <summary>
        /// Adicionar Actividad al Plan Operativo/Nómina
        /// Generar registro de tipo "Cargo".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Cargar();
        }

        /// <summary>
        /// Adicionar Actividad al Plan Operativo/Nómina
        /// Generar registro de tipo "Cargo".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Adicionar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["v_Tipo"] = "Cargo";
            Redirect(null, "../evaluacion/CatalogoInterventorTMP.aspx", "_blank", "menubar=0,scrollbars=1,width=980,height=400,top=50");
        }

        #endregion

        #region Crear Insumos.

        /// <summary>
        /// Adicionar Mano de Obra al Plan Operativo/Nómina
        /// Generar registro de tipo "Insumo".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["v_Tipo"] = "Insumo";
            Redirect(null, "../evaluacion/CatalogoInterventorTMP.aspx", "_blank", "menubar=0,scrollbars=1,width=980,height=400,top=50");
        }

        /// <summary>
        /// Adicionar Actividad al Plan Operativo/Nómina
        /// Generar registro de tipo "Insumo".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Adicionar2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["v_Tipo"] = "Insumo";
            Redirect(null, "../evaluacion/CatalogoInterventorTMP.aspx", "_blank", "menubar=0,scrollbars=1,width=980,height=400,top=50");
        }

        #endregion

        /// <summary>
        /// Seleccionar el ítem y llamar a "CargarDetalles".
        /// 26/04/2014: Modificación para enviarle por variables de sesión, el código de la nómina y el nombre 
        /// de la nómina seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_personalCalificado_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                HttpContext.Current.Session["Accion"] = "actualizar";
                HttpContext.Current.Session["proyecto"] = CodProyecto;
                HttpContext.Current.Session["CodNomina"] = e.CommandArgument.ToString(); //Estaba "Session["CodProducto"]".
                HttpContext.Current.Session["pagina"] = "Nomina";
                HttpContext.Current.Session["v_Tipo"] = "Cargo";
                ///Se estaba redireccionando erróneamente a esta página, porque se había unificado la funcionalidad.
                //Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
                //         "menubar=0,scrollbars=1,width=710,height=400,top=100");
                //Redirección correcta.
                Redirect(null, "../evaluacion/CatalogoInterventorTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=980,height=400,top=100");
            }
            if (e.CommandName == "eliminar")
            {
                EliminarNominaSeleccionada(Convert.ToInt32(e.CommandArgument.ToString()));

            }
            if (e.CommandName == "mostrar")
            {
                #region Código que separa el código de la nómina y su nombre para añadir los valores a variables de sesión.
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Nueva línea de código para almacenar nombre del cargo seleccionado en sesión.
                HttpContext.Current.Session["CodNomina"] = valores_command[0];
                HttpContext.Current.Session["NombreDelCargo"] = valores_command[1];
                HttpContext.Current.Session["EsCargo"] = "Cargo";
                #endregion

                CargarDetalle(e.CommandArgument.ToString());
            }
        }

        protected void grvManoObra_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                HttpContext.Current.Session["Accion"] = "actualizar";
                HttpContext.Current.Session["proyecto"] = CodProyecto;
                HttpContext.Current.Session["CodNomina"] = e.CommandArgument.ToString(); //Estaba "Session["CodProducto"]".
                HttpContext.Current.Session["pagina"] = "Nomina";
                HttpContext.Current.Session["v_Tipo"] = "Insumo";
                ///Se estaba redireccionando erróneamente a esta página, porque se había unificado la funcionalidad.
                //Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
                //         "menubar=0,scrollbars=1,width=710,height=400,top=100");
                //Redirección correcta.
                Redirect(null, "../evaluacion/CatalogoInterventorTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=980,height=400,top=100");
            }
            if (e.CommandName == "eliminar")
            {
                EliminarNominaSeleccionada(Convert.ToInt32(e.CommandArgument.ToString()));

            }
            if (e.CommandName == "mostrar")
            {
                #region Código que separa el código de la nómina y su nombre para añadir los valores a variables de sesión.
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Nueva línea de código para almacenar nombre del cargo seleccionado en sesión.
                HttpContext.Current.Session["CodNomina"] = valores_command[0];
                HttpContext.Current.Session["NombreDelCargo"] = valores_command[1];
                HttpContext.Current.Session["EsCargo"] = "Insumo";
                #endregion

                CargarDetalle(e.CommandArgument.ToString());
            }
        }

        /// <summary>
        /// Cargar los detalles en la grilla "gv_Nomina".
        /// Mauricio Arias Olave "15/04/2014": Se modifica el código fuente para generar los avances totales.
        /// </summary>
        /// <param name="parametros"></param>
        private void CargarDetalle(string parametros)
        {
            var variables = new string[] { };

            if (!string.IsNullOrEmpty(parametros))
            {
                variables = parametros.Split(';');
            }

            if (!string.IsNullOrEmpty(variables.ToString()))
            {
                var respuestaDetalle = new DataTable();

                #region GENERAR GRILLA DINÁMICA DESCOMENTADO.
                //Llamar a método que generará la tabla.
                //Se generan las variables en sesion para llamar a este método 
                HttpContext.Current.Session["interno_CodNomina"] = variables[0].ToString();
                GenerarTabla(variables[0], CodProyecto.ToString(), HttpContext.Current.Session["EsCargo"].ToString());
                #endregion*/
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 08/04/2014.
        /// Cargar la grilla que se encuentra debajo del "Personal Calificado".
        /// </summary>
        protected void CargarGridManoDeObra_CargosEnAprobacion()
        {
            //Inicializar variables.
            DataTable respuestaDetalle = new DataTable();
            String txtSQL = "";

            #region Código anterior.
            ////Inicializar variables.
            //var respuestaDetalle = new DataTable();

            //consultas.Parameters = new[]
            //                               {
            //                                   new SqlParameter
            //                                       {
            //                                           ParameterName = "@CodProyecto" ,Value = CodProyecto //CodCargo
            //                                       }
            //                               };

            //respuestaDetalle = consultas.ObtenerDataTable("MD_ListaDeCargosEnAprobacion_Nomina");

            //if (respuestaDetalle.Rows.Count != 0)
            //{
            //    GrvActividadesNoAprobadas.DataSource = respuestaDetalle;
            //    GrvActividadesNoAprobadas.DataBind();
            //} 
            #endregion

            #region Bindear a la grilla de "Insumo" (grilla de abajo).

            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            { txtSQL = " select * from InterventorNominaTMP where codproyecto = " + CodProyecto + " order by tipo "; }
            else
            { txtSQL = " SELECT * FROM InterventorNomina WHERE Tipo='Insumo' AND codproyecto = " + CodProyecto + " ORDER BY id_nomina "; }

            respuestaDetalle = consultas.ObtenerDataTable(txtSQL, "text");

            GrvActividadesNoAprobadas.DataSource = respuestaDetalle;
            GrvActividadesNoAprobadas.DataBind();

            respuestaDetalle = null;

            #endregion
        }

        /// <summary>
        /// Seleccionar el ítem y llamar a "CargarDetalle".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvActividadesNoAprobadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                HttpContext.Current.Session["Accion"] = "actualizar";
                HttpContext.Current.Session["CodNomina"] = e.CommandArgument.ToString();
                Redirect(null, "../evaluacion/CatalogoInterventorAprobar_TMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=980,height=400,top=100");
            }
            if (e.CommandName == "consultar")
            {
                HttpContext.Current.Session["Accion"] = "consultar";
                HttpContext.Current.Session["CodNomina"] = e.CommandArgument.ToString();
                Redirect(null, "../evaluacion/CatalogoInterventorAprobar_TMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=980,height=400,top=100");
            }
            if (e.CommandName == "mostrar")
            {
                #region Código que separa el código de la nómina y su nombre para añadir los valores a variables de sesión.
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Nueva línea de código para almacenar nombre del cargo seleccionado en sesión.
                HttpContext.Current.Session["CodNomina"] = valores_command[0];
                //Session["NombreDelCargo"] = valores_command[1];
                HttpContext.Current.Session["EsCargo"] = "Insumo";
                HttpContext.Current.Session["NombreDelCargo"] = valores_command[1];
                #endregion

                CargarDetalle(e.CommandArgument.ToString());
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 11/04/2014.
        /// Generar botón de eliminación "visible" sólo cuando las condiciones se cumplan:
        /// La condición es que el valor "NominaID" sea cero, para obtener este valor, debe consultar 
        /// el procedimiento almacenado "MD_ListaDePersonalCalificado_Nomina" o en su defecto
        /// el archivo .sql "MasConsultas.sql".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_personalCalificado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var labelActividadPo = e.Row.FindControl("lblactividaPOI") as Label;
                var imgEditar = e.Row.FindControl("lnkeliminar") as LinkButton;
                var lnk_ver = e.Row.FindControl("lbl_nombreCargo") as LinkButton;

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar para el caso de que el usuario sea un "Interventor".
                    if (labelActividadPo != null)
                    {
                        string txtsql = "select * from AvanceCargoPOMes where codcargo = " + labelActividadPo.Text;
                        var dt = consultas.ObtenerDataTable(txtsql, "text");

                        if (dt.Rows.Count == 0)
                        {
                            imgEditar.Visible = true;
                            imgEditar.ToolTip = "Eliminar Actividad.";
                        }
                        else { imgEditar.Visible = false; }
                    }
                    #endregion
                }
                else
                {
                    #region Inhaibilitar LinkButton.

                    lnk_ver.Enabled = false;
                    lnk_ver.Style.Add("text-decoration", "none");
                    lnk_ver.ForeColor = System.Drawing.Color.Black;

                    #endregion

                    #region Procesar para cualquier otro "Rol". = Se debe dejar invisible el botón de eliminación.

                    if (labelActividadPo != null)
                    {
                        if (imgEditar != null)
                        {
                            imgEditar.Visible = false;
                        }
                    }

                    #endregion

                    #region Comentarios NO BORRAR!.
                    //if (labelActividadPo != null)
                    //{
                    //    if (labelActividadPo.Text.Equals("0"))
                    //    {
                    //        if (imgEditar != null)
                    //        {
                    //            if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    //            {
                    //                imgEditar.Visible = false;
                    //            }
                    //            else
                    //            {
                    //                imgEditar.Visible = true;
                    //                imgEditar.ToolTip = "Eliminar Actividad.";
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        if (imgEditar != null)
                    //        {
                    //            imgEditar.Visible = false;
                    //        }
                    //    }
                    //} 
                    #endregion
                }
            }
        }

        #region Listado de métodos para cargar avances.

        /// <summary>
        /// Mauricio Arias Olave.
        /// Obtener el listado de nóminas para consultar los totales de avances.
        /// </summary>
        /// <param name="P_CodProyecto">Código del proyecto.</param>
        /// <returns>Listado de la nóminas.</returns>
        private List<Int32> ListadoDeNominas(int P_CodProyecto)
        {
            //Inicializar variables.
            DataTable tabla = new DataTable();
            List<Int32> listado_nominas = new List<int>();

            consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodProyecto" ,Value = CodProyecto //CodCargo
                                                   }
                                           };

            tabla = consultas.ObtenerDataTable("MD_ListaDePersonalCalificado_Nomina");

            if (tabla.Rows.Count > 0)
            {
                for (int i = 0; i < tabla.Rows.Count; i++)
                {
                    Int32 valor = Int32.Parse(tabla.Rows[i]["Id_Nomina"].ToString());
                    listado_nominas.Add(valor);
                }
            }

            return listado_nominas;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/04/2014.
        /// Cargar el total dependiendo de la nómina y el tipo de financiación, los valores devueltos deben
        /// ser agregados al GridView "gv_nomina".
        /// </summary>
        /// <param name="L_Nominas">Listado de nóminas.</param>
        /// <param name="LCodTipoFinanciacion">Listado de código de financiaciones obtenidos.</param>
        /// <returns>Listado de Totales de Avances (colocarlos con rojo).</returns>
        private List<Double> Listado_TotalAvances(List<Int32> L_Nominas, List<Int32> LCodTipoFinanciacion)
        {
            #region Inicializar variables.
            //Listado de meses:
            List<Int32> L_meses = new List<int>();

            //Generar 12 meses.
            for (int m = 1; m < 13; m++) { L_meses.Add(m); }

            //Obtiene la conexión
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            //Inicializa la variable para generar la consulta.
            String sqlConsulta;

            //SqlCommand
            SqlCommand cmd = new SqlCommand();

            //Listado de Doubles a devolver.
            List<Double> listado_ = new List<double>();
            #endregion

            #region Ejecutar la información.
            try
            {
                for (int i = 0; i < L_Nominas.Count; i++)
                {
                    sqlConsulta = "SELECT * FROM [AvanceCargoPOMes] WHERE [CodCargo] = " + L_Nominas[i] +
                          " AND Mes = " + L_meses[i] + " AND CodTipoFinanciacion = " + LCodTipoFinanciacion[i] + " ";

                    conn.Open();
                    cmd = new SqlCommand(sqlConsulta, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    //Se leen los datos, son convertidos a Double para asignarlos a un 
                    //listado de Doubles y usarlos luego en la pantalla.
                    while (reader.Read())
                    {
                        double valor_avance = 0;
                        if (String.IsNullOrEmpty(reader["Valor"].ToString()))
                        { valor_avance = 0; listado_.Add(valor_avance); }
                        else { valor_avance = Double.Parse(reader["Valor"].ToString()); }
                        listado_.Add(valor_avance); //Agregar!
                    }

                    //Cerrar conexión.
                    //conn.Close();
                }
            }
            finally {
                conn.Close();
                conn.Dispose();
            }
            #endregion

            //Listado
            return listado_;
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 15/04/2014: Eliminar la nómina seleccionada ya hacer las funciones que dicta FONADE clásico.
        /// </summary>
        /// <param name="P_CodActividad">Código de la actividad seleccionada.</param>
        private void EliminarNominaSeleccionada(Int32 P_CodNomina)
        {
            //string hola = "";
            //return;

            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            bool procesado = false;

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Crea registros en tablas temporales "para la aprobación del coordinador y el gerente".
                    var txtSql = string.Empty;
                    //Cod. coordinador
                    var result = (from t in consultas.Db.Interventors
                                  where t.CodContacto == usuario.IdContacto
                                  select new { t.CodCoordinador }).FirstOrDefault();

                    if(result.CodCoordinador != 0)
                    {
                        var nominaEliminar = (from n in consultas.Db.InterventorNominas where n.Id_Nomina == P_CodNomina
                                                  select n).FirstOrDefault();

                        txtSql = "INSERT INTO InterventorNominaTMP (id_nomina, CodProyecto, cargo,  Tipo, Tarea) " +
                                      "VALUES (" + nominaEliminar.Id_Nomina + ", " + nominaEliminar.CodProyecto + ", '"+ nominaEliminar.Cargo + "', 'Cargo' ,'Borrar') ";
                        ejecutaReader(txtSql, 2);

                        txtSql = "Insert into InterventorNominaMesTMP (CodCargo) values (" + nominaEliminar.Id_Nomina + ")";
                        ejecutaReader(txtSql, 2);

                        var proyecto = (from p in consultas.Db.Proyecto1s
                                        where p.Id_Proyecto == CodProyecto
                                        select p).FirstOrDefault();

                        //Agendar tarea
                        var txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                        var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                        var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());

                        AgendarTarea agenda = new AgendarTarea
                        (idCoord,
                        "Eliminar cargo de Nomina creada por Interventor",
                        "Revisar cargo de nómina " + proyecto.NomProyecto + " - Cargo --> " + nominaEliminar.Cargo + "Observaciones:"  ,
                        CodProyecto.ToString(),
                        2,
                        "0",
                        true,
                        1,
                        true,
                        false,
                        usuario.IdContacto,
                        "CodProyecto=" + CodProyecto,
                        "",
                        "Catálogo Nómina");

                        //agenda.Agendar();

                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                        //CargarGridPersonalCalificado();
                        Response.Redirect("FrameNominaInter.aspx");
                        return;
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true);
                        return;
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
                //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: No se pudo eliminar el personal seleccionado.')", true);
                //return;
            }
        }

        #region Métodos varios.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/05/2014.
        /// Evaluar "dependiendo del rol del usuario logueado" la habilitación de ciertos
        /// componentes visuales "controles" para acceder a las funcionalidades del sistema.
        /// </summary>
        /// <param name="CodGrupo_Contacto">CodGrupo del contacto = "Rol del usuario logueado".</param>
        private void EvaluarCampos(Int32 CodGrupo_Contacto)
        {
            try
            {
                if (CodGrupo_Contacto == Constantes.CONST_Interventor)
                {
                    #region Habilitar campos que el Interventor puede operar.

                    //Controles para "Adicionar Cargo al Plan Operativo".
                    lblvalidador.Visible = true;
                    Adicionar.Visible = true;
                    LinkButton1.Visible = true;
                    Pagos_nomina.Visible = true;

                    //Controles para "Adicionar Mano de obra al Plan Operativo".
                    //img_help_add_manoDeObra.Visible = true;
                    LinkButton2.Visible = true;
                    tb_add.Visible = true;
                    grvManoObraDirecta.Visible = true;
                    lbl_cargosApr.Visible = true;
                    lblCargosPorAprobar.Visible = true;

                    #endregion
                }
                else
                {
                    #region Deshabilitar/Ocultar campos que el usuario logueado NO puede operar.

                    //Controles para "Adicionar Cargo al Plan Operativo".
                    lblvalidador.Visible = false;
                    Adicionar.Visible = false;
                    LinkButton1.Visible = false;
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                        || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                        || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                    {
                        Pagos_nomina.Visible = true;
                    }

                    //Control "Puestos Pendientes de Aprobar:".
                    lblpuestosPendientesConteo.Visible = false;

                    //Controles para "Adicionar Mano de obra al Plan Operativo".
                    //img_help_add_manoDeObra.Visible = false;
                    //LinkButton2.Visible = false;
                    tb_add.Visible = false;

                    #endregion
                }
            }
            catch { }
        }

        #endregion

        #region Generar grilla dinámicamente. FUNCIONAL.

        /// <summary>
        /// Intento #2 de generar grilla dinámica "según diseño de Table realizado por Diego Quiñonez" en informes.
        /// </summary>
        /// <param name="Cod_Nomina">Nómina seleccionada.</param>
        /// <param name="Cod_Proyecto">Proyecto seleccionado.</param>
        /// <param name="EsCargo">La variable debe contener "Cargo" o "Insumo".</param>
        private void GenerarTabla(String Cod_Nomina, String Cod_Proyecto, String EsCargo)
        {
            //Inicializar variables.
            String txtSQL = "";
            String nomCargo = "";
            Double TotalFE = 0;
            Double TotalEmp = 0;
            DataTable rsCargo = new DataTable();
            DataTable contador = new DataTable();
            DataTable rsTipo1 = new DataTable();
            DataTable rsTipo2 = new DataTable();
            DataTable rsPagoActividad = new DataTable();
            Double TotalTipo1 = 0;
            Double TotalTipo2 = 0;
            Double Valor = 0;
            String Valor_FE = "&nbsp;";
            String Valor_Emp = "&nbsp;";
            Int32 ejecutar = 0;
            #region Obtener el valor de la prórroga para sumarla a la constante de meses generar la tabla.
            int prorroga = 0;
            prorroga = ObtenerProrroga(CodProyecto.ToString());
            int prorrogaTotal = prorroga + Constantes.CONST_Meses;
            #endregion

            try
            {
                //Inicializar tabla.
                t_anexos.Rows.Clear();

                //Inicializar la fila.
                TableRow fila = new TableRow();
                fila.Style.Add("text-align", "center");

                #region Generar la primera fila con los meses que tiene la nómina seleccionada.
                for (int i = 1; i <= prorrogaTotal; i++)
                {
                    TableHeaderCell celda = new TableHeaderCell();
                    celda.Style.Add("text-align", "center");
                    celda.ColumnSpan = 2;
                    celda.Text = "Mes " + i;
                    fila.Cells.Add(celda);
                    t_anexos.Rows.Add(fila);
                    celda = null;
                }
                #endregion

                #region Crear una nueva celda que contiene el valor "Costo Total".
                TableHeaderCell celdaCostoTotal = new TableHeaderCell();
                celdaCostoTotal.Text = "Costo Total";
                celdaCostoTotal.Style.Add("text-align", "center");
                celdaCostoTotal.ColumnSpan = 2;
                fila.Cells.Add(celdaCostoTotal);
                t_anexos.Rows.Add(fila);
                celdaCostoTotal = null;
                #endregion

                #region Agregar nueva fila (para adicionar las celdas "Sueldo" y "Prestaciones").
                //Se obtiene la cantidad de celdas que tiene la primera fila para generar los Sueldos y las Prestaciones.
                int conteo_celdas = fila.Cells.Count + 1; //El +1 es para contar también la celda "Costo Total".
                //Se inicializa la variable para generar una nueva fila.
                fila = new TableRow();

                //Generar las celdas "Sueldo" y "Prestaciones".
                for (int i = 1; i < conteo_celdas; i++)
                {
                    //Celdas "Sueldo" y "Prestaciones Sociales".
                    TableHeaderCell celdaSueldo = new TableHeaderCell();
                    celdaSueldo.Style.Add("text-align", "left");
                    TableHeaderCell celdaPrestaciones = new TableHeaderCell();
                    celdaPrestaciones.Style.Add("text-align", "left");

                    //Agregar datos a la celda de Sueldo.
                    celdaSueldo.Text = "Sueldo";
                    fila.Cells.Add(celdaSueldo);
                    t_anexos.Rows.Add(fila);
                    celdaSueldo = null;

                    //Agregar datos a la celda de Prestaciones Sociales.
                    celdaPrestaciones.Text = "Prestaciones";
                    fila.Cells.Add(celdaPrestaciones);
                    t_anexos.Rows.Add(fila);
                    celdaPrestaciones = null;
                }
                #endregion

                #region Personal calificado - Cargos / Insumos.

                txtSQL = " SELECT DISTINCT * " +
                         " FROM InterventorNomina a,InterventorNominaMes b " +
                         " WHERE a.tipo='" + EsCargo + "' AND id_nomina = codcargo AND codproyecto= " + Cod_Proyecto +
                         " AND mes <> 0 and Id_Nomina = " + Cod_Nomina + " " +
                         " ORDER BY id_nomina, mes, b.tipo ";

                //Asignar resultados de la consulta anterior a variable DataTable.
                rsCargo = consultas.ObtenerDataTable(txtSQL, "text");

                #endregion

                #region Generar tres filas con sus respectivas celdas.

                //Conteo de las celdas anteriores DEBEN ser obtenidas de nuevo para generar correctamente las celdas.
                conteo_celdas = fila.Cells.Count + 1;
                int mes_data = 1;

                //Generar tres celdas.
                for (int i = 0; i < 4; i++)
                {
                    //Si es cero, es la primera fila, que por defecto es una fila vacía.
                    if (i == 0)
                    {
                        #region Agregar nueva fila con espacio separador "igual como lo deja FONADE clásico.

                        //Inicializar la fila.
                        fila = new TableRow();
                        TableCell celdaEspacio = new TableCell();
                        celdaEspacio.Text = "&nbsp;";
                        fila.Cells.Add(celdaEspacio);
                        t_anexos.Rows.Add(fila);

                        #endregion
                    }
                    if (i == 1)
                    {
                        #region Agregar la fila "con los valores de meses (Cargos)".

                        //Inicializar la fila.
                        fila = new TableRow();
                        //Recorrer las celdas.
                        for (int j = 1; j < conteo_celdas; j++)
                        {
                            //Si el mes es menor o igual a los meses totales (mes+prorroga) + 1 (fila Costo Total).
                            if (mes_data <= prorrogaTotal + 1) //+ 1 indicando la "fila" "Costo Total".
                            {
                                //Inicializar variable para obtener el valor de acuerdo al mes.
                                DataRow[] result = rsCargo.Select("Mes = " + mes_data);

                                //Si encuentra datos.
                                if (result.Count() != 0)
                                {
                                    #region Consultar el campo "Valor" y operarlo en las variables correspondientes.

                                    foreach (DataRow row in result)
                                    {
                                        //Obtener el campos "Valor".
                                        Valor = Double.Parse(row["Valor"].ToString());

                                        //Si es de tipo "1", lo añade a variable "FE", si es "2", lo agrega a variable "Emp".
                                        if (row["Tipo1"].ToString() == "1")
                                        {
                                            TotalFE = TotalFE + Valor;
                                            Valor_FE = Valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        }
                                        if (row["Tipo1"].ToString() == "2")
                                        {
                                            TotalEmp = TotalEmp + Valor;
                                            Valor_Emp = Valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        }
                                    }

                                    #endregion

                                    if (mes_data == prorrogaTotal + 1)
                                    {
                                        TableCell celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalFE.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);
                                        totalFEGlobal = TotalFE;

                                        celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalEmp.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);
                                        totalEmpGlobal = TotalEmp;
                                    }
                                    else
                                    {
                                        TableCell celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = Valor_FE;
                                        fila.Cells.Add(celdaEspacio);

                                        celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = Valor_Emp; //"&nbsp;";
                                        fila.Cells.Add(celdaEspacio);
                                    }
                                }
                                else
                                {
                                    if (mes_data == prorrogaTotal + 1)
                                    {
                                        TableCell celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalFE.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);
                                        totalFEGlobal = TotalFE;

                                        celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalEmp.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);
                                        totalEmpGlobal = TotalEmp;
                                    }
                                    else
                                    {
                                        #region Añadir espacios.

                                        TableCell celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = "&nbsp;";
                                        fila.Cells.Add(celdaEspacio);

                                        celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = "&nbsp;";
                                        fila.Cells.Add(celdaEspacio);

                                        #endregion
                                    }
                                }
                                //Incrementar variable mes.
                                mes_data++;
                            }

                        }
                        //Añadir la fila a la tabla.
                        t_anexos.Rows.Add(fila);

                        #endregion
                    }
                    if (i == 2)
                    {
                        #region Agregar la fila "con valores en (rojo)".

                        fila = new TableRow();
                        mes_data = 1;
                        for (int j = 1; j < conteo_celdas; j++)
                        {
                            //Si el mes es menor o igual a los meses totales (mes+prorroga) + 1 (fila Costo Total).
                            if (mes_data <= prorrogaTotal + 1) //+ 1 indicando la "fila" "Costo Total".
                            {
                                #region Depuración de datos de tipo 1.

                                //Inicializar variables de Tipo 1.
                                string tipo1 = "&nbsp;";
                                Double valor_tipo1 = 0;

                                //Consulta SQL de Tipo 1.
                                txtSQL = " select * " +
                                         " from AvanceCargoPOMes " +
                                         " where CodCargo = " + Cod_Nomina +
                                         " and Mes = " + mes_data + " and codtipofinanciacion = 1 ";

                                //Asignar resultados a variable DataTable.
                                rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");

                                //Si la consulta tiene datos, formatea el campo "Valor" y lo agrega a las variables correspondientes.
                                if (rsTipo1.Rows.Count > 0)
                                {
                                    valor_tipo1 = Double.Parse(rsTipo1.Rows[0]["Valor"].ToString());
                                    tipo1 = valor_tipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                    TotalTipo1 = TotalTipo1 + valor_tipo1;
                                }

                                if (mes_data == prorrogaTotal + 1) //Penúltima celda.
                                {
                                    #region Si esta condición se cumple, se debe a que es el Total de tipo 1, por lo tanto se agrega en "Costo Total".

                                    //Variable que contiene el valor "TotalTipo1" formateado.
                                    string sTotalTipo1 = TotalTipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                                    //Agregar celda con el valor formateado.
                                    TableCell celdaEspacio = new TableCell();
                                    celdaEspacio.Attributes.Add("align", "right");
                                    celdaEspacio.Text = "<font color='#CC0000'>" + sTotalTipo1 + "</font>";
                                    fila.Cells.Add(celdaEspacio);

                                    #endregion
                                }
                                else
                                {
                                    //Agregar celda con su valor.
                                    TableCell celdaEspacio = new TableCell();
                                    celdaEspacio.Attributes.Add("align", "right");
                                    celdaEspacio.Text = "<font color='#CC0000'>" + tipo1 + "</font>";
                                    fila.Cells.Add(celdaEspacio);
                                }

                                #endregion

                                #region Depuración de datos de tipo 2.

                                //Inicializar variables de Tipo 2.
                                string tipo2 = "&nbsp;";
                                Double valor_tipo2 = 0;

                                //Consulta SQL de Tipo 2.
                                txtSQL = " select * " +
                                         " from AvanceCargoPOMes " +
                                         " where CodCargo=" + Cod_Nomina +
                                         " and Mes=" + j + " and codtipofinanciacion = 2 ";

                                //Asignar resultados a variable DataTable.
                                rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");

                                //Si contiene datos, formatea el campo "Valor" y lo agrega a las variables correspondientes.
                                if (rsTipo2.Rows.Count > 0)
                                {
                                    valor_tipo2 = Double.Parse(rsTipo2.Rows[0]["Valor"].ToString());
                                    tipo2 = valor_tipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                    TotalTipo2 = TotalTipo2 + valor_tipo2;
                                }

                                if (mes_data == prorrogaTotal + 1) //Última celda.
                                {
                                    #region Si esta condición se cumple, se debe a que es el Total de tipo 2, por lo tanto se agrega en "Costo Total".

                                    //Variable que contiene el valor "TotalTipo2" formateado.
                                    string sTotalTipo2 = TotalTipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                                    //Agregar celda con el valor formateado.
                                    TableCell celdaEspacio = new TableCell();
                                    celdaEspacio.Attributes.Add("align", "right");
                                    celdaEspacio.Text = "<font color='#CC0000'>" + sTotalTipo2 + "</font>";
                                    fila.Cells.Add(celdaEspacio);

                                    #endregion
                                }
                                else
                                {
                                    //Agregar celda con su valor.
                                    TableCell celdaEspacio = new TableCell();
                                    celdaEspacio.Attributes.Add("align", "right");
                                    celdaEspacio.Text = "<font color='#CC0000'>" + tipo2 + "</font>";
                                    fila.Cells.Add(celdaEspacio);
                                }

                                #endregion
                            }

                            //Incrementar variable mes.
                            mes_data++;
                        }
                        //Añadir la fila con sus celdas a la tabla.
                        t_anexos.Rows.Add(fila);

                        #endregion
                    }
                    if (i == 3)
                    {
                        #region Agregar la fila "con los controles dinámicos".

                        //Inicializar celda.
                        fila = new TableRow();
                        //Re-inicializar la variable de "Mes" a 1.
                        mes_data = 1;
                        //for (int m = 1; m <= prorrogaTotal + 1; m++)
                        for (int j = 1; j < conteo_celdas; j++)
                        {
                            //Generar celda.
                            TableCell celda = new TableCell();
                            celda.Style.Add("text-align", "center");
                            celda.ColumnSpan = 2;

                            //Si el mes es menor o igual a los meses totales (mes+prorroga) + 1 (fila Costo Total).
                            if (mes_data <= prorrogaTotal + 1) //+ 1 indicando la "fila" "Costo Total".
                            {
                                //Consulta SQL.
                                txtSQL = " SELECT * FROM AvanceCargoPOMes " +
                                         " WHERE CodCargo = " + Cod_Nomina + " AND mes = " + mes_data;

                                //Asignar resultados a la variable DataTable.
                                rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");

                                //Determinar el valor de la vairiable "ejecutar" de acuerdo a la tabla.
                                if (rsTipo2.Rows.Count > 0)
                                { ejecutar = 1; /*Si existe se coloca la opción de editar y borrar*/ }
                                else
                                { ejecutar = 2; /*Si NO existe se coloca la opción de adicionar*/ }

                                try { nomCargo = Cod_Nomina.Replace("+", "$"); }
                                catch { }

                                if (ejecutar == 1)
                                {
                                    if (EsCargo == "Cargo")
                                    {
                                        #region Tratar como "Cargo" y ejecutar == 1.

                                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor 
                                            || usuario.CodGrupo == Constantes.CONST_Interventor 
                                            || usuario.CodGrupo == Constantes.CONST_Asesor 
                                            || usuario.CodGrupo == Constantes.CONST_LiderRegional
                                            || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                                            || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                                            || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                                        {
                                            rsPagoActividad = new DataTable();

                                            if (!validarPermiso(usuario.CodGrupo))
                                            {
                                                if (String.IsNullOrEmpty(rsTipo2.Rows[0]["ObservacionesInterventor"].ToString()))
                                                {
                                                    #region Eliminar avance.


                                                    if (mes_data <= prorrogaTotal)
                                                    {
                                                        ImageButton img_VerAvance_Elim = new ImageButton();

                                                        //ImageButton.
                                                        img_VerAvance_Elim.ID = "img_VerAvance_Elim" + mes_data.ToString();
                                                        img_VerAvance_Elim.ImageUrl = "~/Images/icoBorrar.gif";
                                                        img_VerAvance_Elim.AlternateText = "Avance";
                                                        img_VerAvance_Elim.CommandName = "borrar";
                                                        img_VerAvance_Elim.CommandArgument = "borrar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                        img_VerAvance_Elim.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                        img_VerAvance_Elim.OnClientClick = "return borrar();";
                                                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                                                            img_VerAvance_Elim.Visible = false;
                                                        celda.Controls.Add(img_VerAvance_Elim);
                                                    }

                                                    #endregion
                                                }
                                            }
                                                                                       

                                            txtSQL = " select * from PagoActividad  where CodProyecto = " + Cod_Proyecto +
                                                     " and codActividad = " + Cod_Nomina + " and TipoPago = 2 " +
                                                     " AND (PagoActividad.Mes = " + mes_data + ") AND (PagoActividad.Estado <> 0) ";

                                            rsPagoActividad = consultas.ObtenerDataTable(txtSQL, "text");

                                            //if (rsTipo2.Rows[0]["Aprobada"].ToString() == "False" || rsTipo2.Rows[0]["Aprobada"].ToString() == "0")
                                            if (rsTipo2.Rows[0]["Aprobada"].ToString() == "True" || rsTipo2.Rows[0]["Aprobada"].ToString() == "1")
                                            {
                                                #region Ver avance.

                                                if (mes_data <= prorrogaTotal)
                                                {
                                                    ImageButton img_VerAvance_Ver1 = new ImageButton();
                                                    LinkButton lnk_VerAvance_Ver1 = new LinkButton();

                                                    //ImageButton.
                                                    img_VerAvance_Ver1.ID = "img_VerAvance_Ver1" + mes_data.ToString();
                                                    img_VerAvance_Ver1.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                    img_VerAvance_Ver1.AlternateText = "Avance";
                                                    img_VerAvance_Ver1.CommandName = "actualizar";
                                                    img_VerAvance_Ver1.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                    img_VerAvance_Ver1.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                    //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                    //{ img_VerAvance = null; }
                                                    //else
                                                    //{ }
                                                    celda.Controls.Add(img_VerAvance_Ver1);

                                                    //LinkButton.
                                                    lnk_VerAvance_Ver1.ID = "lnk_VerAvance_Ver1" + mes_data.ToString();
                                                    lnk_VerAvance_Ver1.Text = "<b>&nbsp;Ver Avance</b>";
                                                    lnk_VerAvance_Ver1.Style.Add("text-decoration", "none");
                                                    lnk_VerAvance_Ver1.CommandName = "actualizar";
                                                    lnk_VerAvance_Ver1.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                    lnk_VerAvance_Ver1.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                    //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                    //{ lnk_VerAvance = null; }
                                                    //else
                                                    //{ }
                                                    celda.Controls.Add(lnk_VerAvance_Ver1);
                                                }

                                                #endregion
                                            }
                                            else
                                            {
                                                if (rsPagoActividad.Rows.Count == 0)
                                                {
                                                    #region Editar avance.

                                                    if (mes_data <= prorrogaTotal)
                                                    {
                                                        ImageButton img_EdtAvance = new ImageButton();
                                                        LinkButton lnk_EdtAvance = new LinkButton();

                                                        //ImageButton.
                                                        img_EdtAvance.ID = "img_EdtAvance_" + mes_data.ToString();
                                                        img_EdtAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                        img_EdtAvance.AlternateText = "Avance";
                                                        if(usuario.CodGrupo == Constantes.CONST_Asesor)
                                                        {
                                                            img_EdtAvance.CommandName = "ver";
                                                        }
                                                        else
                                                        {
                                                            img_EdtAvance.CommandName = "actualizar";
                                                        }
                                                        img_EdtAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                        img_EdtAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                        //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                        //{ img_VerAvance = null; }
                                                        //else
                                                        //{ }
                                                        celda.Controls.Add(img_EdtAvance);

                                                        //LinkButton.
                                                        lnk_EdtAvance.ID = "lnk_EdtAvance_" + mes_data.ToString();
                                                        if(usuario.CodGrupo == Constantes.CONST_Asesor || usuario.CodGrupo == Constantes.CONST_LiderRegional)
                                                        {
                                                            lnk_EdtAvance.Text = "<b>&nbsp;Ver Avance</b>";
                                                        }
                                                        else
                                                        {
                                                            lnk_EdtAvance.Text = "<b>&nbsp;Editar Avance</b>";
                                                        }
                                                        lnk_EdtAvance.Style.Add("text-decoration", "none");
                                                        lnk_EdtAvance.CommandName = "actualizar";
                                                        lnk_EdtAvance.CommandArgument = "Modificar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                        lnk_EdtAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                        //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                        //{ lnk_VerAvance = null; }
                                                        //else
                                                        //{ }
                                                        celda.Controls.Add(lnk_EdtAvance);
                                                    }

                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region Editar avance...¿otra vez? = si, según FONADE clásico.

                                                    if (mes_data <= prorrogaTotal)
                                                    {
                                                        ImageButton img_VerAvance_Again = new ImageButton();
                                                        LinkButton lnk_VerAvance_Again = new LinkButton();

                                                        //ImageButton.
                                                        img_VerAvance_Again.ID = "img_VerAvance_Again" + mes_data.ToString();
                                                        img_VerAvance_Again.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                        img_VerAvance_Again.AlternateText = "Avance";
                                                        if(usuario.CodGrupo == Constantes.CONST_Asesor)
                                                        {
                                                            img_VerAvance_Again.CommandName = "ver";
                                                        }
                                                        else
                                                        {
                                                            img_VerAvance_Again.CommandName = "actualizar";
                                                        }
                                                        img_VerAvance_Again.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                        img_VerAvance_Again.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                        //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                        //{ img_VerAvance = null; }
                                                        //else
                                                        //{ }
                                                        celda.Controls.Add(img_VerAvance_Again);

                                                        //LinkButton.
                                                        lnk_VerAvance_Again.ID = "lnk_VerAvance_Again" + mes_data.ToString();
                                                        if(usuario.CodGrupo == Constantes.CONST_Asesor)
                                                        {
                                                            lnk_VerAvance_Again.Text = "<b>&nbsp;Ver Avance</b>";
                                                        }
                                                        else
                                                        {
                                                            lnk_VerAvance_Again.Text = "<b>&nbsp;Editar Avance</b>";
                                                        }
                                                        lnk_VerAvance_Again.Style.Add("text-decoration", "none");
                                                        lnk_VerAvance_Again.CommandName = "actualizar";
                                                        lnk_VerAvance_Again.CommandArgument = "Modificar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                        lnk_VerAvance_Again.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                        //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                        //{ lnk_VerAvance = null; }
                                                        //else
                                                        //{ }
                                                        celda.Controls.Add(lnk_VerAvance_Again);
                                                    }

                                                    #endregion
                                                }
                                            }
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Tratar como "Insumo" y ejecutar == 1.

                                        if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                        {
                                            if (String.IsNullOrEmpty(rsTipo2.Rows[0]["ObservacionesInterventor"].ToString()))
                                            {
                                                #region Eliminar avance.

                                                if (mes_data <= prorrogaTotal)
                                                {
                                                    ImageButton img_VerAvance_INSM = new ImageButton();
                                                    LinkButton lnk_VerAvance_INSM = new LinkButton();

                                                    //ImageButton.
                                                    img_VerAvance_INSM.ID = "img_VerAvance_INSM" + mes_data.ToString();
                                                    img_VerAvance_INSM.ImageUrl = "~/Images/icoBorrar.gif";
                                                    img_VerAvance_INSM.AlternateText = "Avance";
                                                    img_VerAvance_INSM.CommandName = "borrar";
                                                    img_VerAvance_INSM.CommandArgument = "borrar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                    img_VerAvance_INSM.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                    celda.Controls.Add(img_VerAvance_INSM);

                                                    //LinkButton.
                                                    lnk_VerAvance_INSM.ID = "lnk_VerAvance_INSM" + mes_data.ToString();
                                                    lnk_VerAvance_INSM.Text = "<b>&nbsp;Eliminar Avance</b>";
                                                    lnk_VerAvance_INSM.Style.Add("text-decoration", "none");
                                                    lnk_VerAvance_INSM.CommandName = "borrar";
                                                    lnk_VerAvance_INSM.CommandArgument = "borrar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                    lnk_VerAvance_INSM.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                    celda.Controls.Add(lnk_VerAvance_INSM);
                                                }

                                                #endregion
                                            }

                                            #region Editar avance.

                                            if (mes_data <= prorrogaTotal)
                                            {
                                                ImageButton img_VerAvance = new ImageButton();
                                                LinkButton lnk_VerAvance = new LinkButton();

                                                //ImageButton.
                                                img_VerAvance.ID = "img_VerAvance_" + mes_data.ToString();
                                                img_VerAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                img_VerAvance.AlternateText = "Avance";
                                                img_VerAvance.CommandName = "actualizar";
                                                img_VerAvance.CommandArgument = "Modificar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                img_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(img_VerAvance);

                                                //LinkButton.
                                                lnk_VerAvance.ID = "lnk_VerAvance_" + mes_data.ToString();
                                                lnk_VerAvance.Text = "<b>&nbsp;Editar Avance</b>";
                                                lnk_VerAvance.Style.Add("text-decoration", "none");
                                                lnk_VerAvance.CommandName = "actualizar";
                                                lnk_VerAvance.CommandArgument = "Modificar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                lnk_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(lnk_VerAvance);
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            #region Ver avance.

                                            if (mes_data <= prorrogaTotal)
                                            {
                                                ImageButton img_VerAvance_INSM_Va = new ImageButton();
                                                LinkButton lnk_VerAvance_INSM_Va = new LinkButton();

                                                //ImageButton.
                                                img_VerAvance_INSM_Va.ID = "img_VerAvance_INSM_Va" + mes_data.ToString();
                                                img_VerAvance_INSM_Va.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                img_VerAvance_INSM_Va.AlternateText = "Avance";
                                                img_VerAvance_INSM_Va.CommandName = "actualizar";
                                                img_VerAvance_INSM_Va.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                img_VerAvance_INSM_Va.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(img_VerAvance_INSM_Va);

                                                //LinkButton.
                                                lnk_VerAvance_INSM_Va.ID = "lnk_VerAvance_INSM_Va" + mes_data.ToString();
                                                lnk_VerAvance_INSM_Va.Text = "<b>&nbsp;Ver Avance</b>";
                                                lnk_VerAvance_INSM_Va.Style.Add("text-decoration", "none");
                                                lnk_VerAvance_INSM_Va.CommandName = "actualizar";
                                                lnk_VerAvance_INSM_Va.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                lnk_VerAvance_INSM_Va.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(lnk_VerAvance_INSM_Va);
                                            }

                                            #endregion
                                        }

                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (EsCargo == "Cargo")
                                    {
                                        #region Tratar como "Cargo" y ejecutar == 2.

                                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                                        {
                                            #region Reportar avance.

                                            if (mes_data <= prorrogaTotal)
                                            {
                                                ImageButton img_ra_2_ReportarAvance = new ImageButton();
                                                LinkButton lnk_ra_2_ReportarAvance = new LinkButton();

                                                //ImageButton.
                                                img_ra_2_ReportarAvance.ID = "img_ra_2_ReportarAvance_ra_2_" + mes_data.ToString();
                                                img_ra_2_ReportarAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                img_ra_2_ReportarAvance.AlternateText = "Reportar";
                                                img_ra_2_ReportarAvance.CommandName = "Reportar";
                                                img_ra_2_ReportarAvance.CommandArgument = "Reportar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                img_ra_2_ReportarAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(img_ra_2_ReportarAvance);

                                                //LinkButton.
                                                lnk_ra_2_ReportarAvance.ID = "lnk_ra_2_ReportarAvance_ra_2_" + mes_data.ToString();
                                                lnk_ra_2_ReportarAvance.Text = "<b>&nbsp;Reportar Avance</b>";
                                                lnk_ra_2_ReportarAvance.Style.Add("text-decoration", "none");
                                                lnk_ra_2_ReportarAvance.CommandName = "Reportar";
                                                lnk_ra_2_ReportarAvance.CommandArgument = "Reportar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                lnk_ra_2_ReportarAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(lnk_ra_2_ReportarAvance);
                                            }

                                            #endregion
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Tratar como "Insumo" y ejecutar == 2.

                                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                                        {
                                            #region Reportar avance.

                                            if (mes_data <= prorrogaTotal)
                                            {
                                                ImageButton img_Repo_Avance = new ImageButton();
                                                LinkButton lnk_Repo_Avance = new LinkButton();

                                                //ImageButton.
                                                img_Repo_Avance.ID = "img_Repo_Avance_" + mes_data.ToString();
                                                img_Repo_Avance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                img_Repo_Avance.AlternateText = "Avance";
                                                img_Repo_Avance.CommandName = "crear";
                                                img_Repo_Avance.CommandArgument = "Adicionar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                img_Repo_Avance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                if(totalFEGlobal == 0 && totalEmpGlobal == 0)
                                                {
                                                    img_Repo_Avance.Visible = true;
                                                }
                                                celda.Controls.Add(img_Repo_Avance);

                                                //LinkButton.
                                                lnk_Repo_Avance.ID = "lnk_Repo_Avance_" + mes_data.ToString();
                                                lnk_Repo_Avance.Text = "<b>&nbsp;Reportar Avance</b>";
                                                lnk_Repo_Avance.Style.Add("text-decoration", "none");
                                                lnk_Repo_Avance.CommandName = "VerAvance";
                                                lnk_Repo_Avance.CommandArgument = "Adicionar" + ";" + Cod_Proyecto + ";" + Cod_Nomina + ";" + mes_data + ";" + nomCargo;
                                                lnk_Repo_Avance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                if (totalFEGlobal == 0 && totalEmpGlobal == 0)
                                                {
                                                    lnk_Repo_Avance.Visible = true;
                                                }
                                                celda.Controls.Add(lnk_Repo_Avance);
                                            }

                                            #endregion
                                        }

                                        #endregion
                                    }
                                }
                            }

                            //Incrementa el mes 
                            mes_data++;

                            //Añadir la celda a la fila y la fila a la tabla.
                            fila.Cells.Add(celda);
                            t_anexos.Rows.Add(fila);
                            celda = null;
                        }
                        t_anexos.Rows.Add(fila);

                        #endregion
                    }
                }

                #endregion

                //Bindear finalmente la grilla.
                t_anexos.DataBind();
            }
            catch (Exception ex)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //t_anexos.Visible = false;
            }
        }

        #region Métodos dinámicos de los controles generados en el método "GenerarTabla".

        /// <summary>
        /// Método asignado a los controles dinámicos para ver los avances.
        /// FUNCIONAL!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_VerAvance(Object sender, CommandEventArgs e)
        {
            try
            {
                //Inicializar variables.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                Session["datosNomina"] = null;

                if (e.CommandName != "borrar")
                {
                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        if (e.CommandName != "Reportar")
                        {
                            HttpContext.Current.Session["Accion"] = "editar";
                        }
                        else
                        {
                            HttpContext.Current.Session["Accion"] = e.CommandName;
                        }
                    }
                    else
                    {
                        HttpContext.Current.Session["Accion"] = e.CommandName;
                    }
                    HttpContext.Current.Session["proyecto"] = valores_command[1].ToString();
                    //Ya se le está enviado la nómina seleccionada en una variable de sesión.
                    HttpContext.Current.Session["MesDeLaNomina"] = valores_command[3].ToString();
                    Session["idAnexoNomina"] = valores_command[2].ToString();
                    //Ya se le está enviando el nombre del cargo seleccionado en una variable de sesión.
                    HttpContext.Current.Session["pagina"] = "Nomina";
                    Session["CodCargo"] = valores_command[2].ToString();
                    Redirect(null, "../evaluacion/CatalogoNominaPOInterventoria.aspx", "_blank",
                               "menubar=0,scrollbars=1,width=710,height=530,top=100");
                }
                else
                {
                    var mes = int.Parse(valores_command[3]);
                    var nominas = (from n in consultas.Db.AvanceCargoPOMes
                                   where n.Mes == mes && n.CodCargo == int.Parse(valores_command[2])
                                   select n).ToList();
                    consultas.Db.AvanceCargoPOMes.DeleteAllOnSubmit(nominas);
                    consultas.Db.SubmitChanges();

                    //var sql = "Delete from AvanceCargoPOAnexos Where codCargo = " + valores_command[2] + " And Mes = " + valores_command[3];
                    //ejecutaReader(sql, 2);

                    var sql = "Update AvanceCargoPOAnexos set Borrado = 1 where CodCargo = " + valores_command[2] + " and mes = " + valores_command[3];
                    ejecutaReader(sql, 2);
                    Response.Redirect("FrameNominaInter.aspx");
                }
            }
            catch { }
        }

        #endregion

        #endregion

        /// <summary>
        /// Botón de impresión.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkImprimir_Click(object sender, EventArgs e)
        {
            Redirect(null, "ImprimirPlanOperativos.aspx", "_blank", "width=640,height=480,scrollbars=yes,resizable=no");
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 02/07/2014.
        /// Revisar y cambiar dinámicamente el estado del LinkButton.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvActividadesNoAprobadas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnk = e.Row.FindControl("lnk_editarValores") as LinkButton;

                if (lnk != null)
                {
                    if (usuario.CodGrupo != Constantes.CONST_Interventor)
                    { lnk.Style.Add("text-decoration", "none"); lnk.Enabled = false; }
                }
            }
        }

        /// <summary>
        /// Botón "Pagos".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkPagos_Click(object sender, EventArgs e)
        {
            if (Estado == 1)
            {
                HttpContext.Current.Session["TipoPago"] = "1";
                HttpContext.Current.Session["CodProyecto"] = CodProyecto;

                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                { Redirect(null, "PagosActividad.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100"); }

                else if (usuario.CodGrupo == Constantes.CONST_Interventor)
                { Redirect(null, "PagosActividadInter.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100"); }
            }
            else if (Estado == 2)
            {
                HttpContext.Current.Session["TipoPago"] = "2";
                HttpContext.Current.Session["CodProyecto"] = CodProyecto;

                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                { Redirect(null, "PagosActividad.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100"); }

                else if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    Redirect(null, "PagosActividad.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                    //Redirect(null, "PagosActividadInter.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                }
                else if(usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                {
                    Redirect(null, "PagosActividad.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                }
            }
        }

        private bool validarPermiso(int codGrupo)
        {
            bool validar = false;

            if (codGrupo == Constantes.CONST_CoordinadorInterventor)
            {
                validar = true;
            }
            if (codGrupo == Constantes.CONST_GerenteInterventor)
            {
                validar = true;
            }
            if (codGrupo == Constantes.CONST_AdministradorSistema)
            {
                validar = true;
            }

            return validar;
        }
    }
}
