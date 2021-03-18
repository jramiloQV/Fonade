using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using Fonade.Clases;
using System.Configuration;
using System.IO;

namespace Fonade.FONADE.interventoria
{
    public partial class AgregarInformeFinalInterventoria : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Id del informe seleccionado.
        /// </summary>
        string InformeIdInformeFinal;

        /// <summary>
        /// Id de la empresa del informe seleccionado.
        /// </summary>
        string idEmpresa;

        string txtSQL;

        string txtNomEmpresa;
        string CodProyecto;

        /// <summary>
        /// Variable que determina la acción sobre la empresa seleccionada.
        /// Viene de "InformeConsolidadoInter.aspx".
        /// </summary>
        String accion_nuevo;

        /// <summary>
        /// Valor "estado obtenido en el método "OcultarCampos(string);
        /// </summary>
        Int32 Estado;

        string accion;

        #endregion

        /*
         * 
         * Estados de un reporte final de interventoria
	     * 0 - Ingresado
	     * 1 - Enviado
	     * 2 - Aprobado Coordinador
	     * 3 - Aprobado Gerente
         * 
         */

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "0";
            //Obtener valores de las variables de sesión.
            InformeIdInformeFinal = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            idEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

            //Cargar el estado y establacer si se ven o no ciertos controles "botones".
            TraerIdInformeFinal(InformeIdInformeFinal);

            if (!IsPostBack)
            {
                //Cargar el nombre del interventor en sesión:
                L_TituloNombre.Text = "Interventor " + usuario.Nombres + " " + usuario.Apellidos;

                //Cargar el nombre del coordinador:
                lblnomcoordinador.Text = "Coordinador: " + CargarCoordinadorDelInterventor();

                //Establecer fecha actual en los campos correspondientes.
                DateTime fecha = DateTime.Today;
                c_fecha_s.SelectedDate = fecha;
                txtDate.Text = fecha.ToString("dd/MM/yyyy");

                if(accion != "Nuevo")
                {
                    if (InformeIdInformeFinal != "0" && idEmpresa != "0")
                    {
                        //Consultar estado.
                        ConsultarEstado(InformeIdInformeFinal);
                        //var rol = HttpContext.Current.Session["cod_rol"].ToString();
                        var rol = usuario.CodGrupo;
                        if (Estado == 0 && (usuario.CodGrupo == Constantes.CONST_Interventor || rol == Constantes.CONST_RolInterventorLider))
                        {
                            //Mostrar botones "Guardar, Borrar y Enviar".
                            btn_guardar.Visible = true;
                            btn_borrar.Visible = true;
                            btn_enviar.Visible = true;
                        }
                        else
                        {
                            //Mostrar botón "Imprimir".
                            btn_imprimir.Visible = true;
                        }

                        #region Consultar datos.

                        //Ocultar ciertos campos.
                        txtDate.Visible = false;
                        imgPopup.Visible = false;
                        hdf_CodInforme.Value = InformeIdInformeFinal;


                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            espacio_aprobar.Visible = true;
                            lbl_texto_aprobar.Visible = true;
                            Aprobar.Visible = true;
                            btn_imprimir.Visible = true;
                        }

                        #endregion
                        InformeIdInformeFinal = "0";
                        idEmpresa = "0";
                    }
                }
                else
                {
                    #region Crear datos.

                    //Luego se adiciona el informe.
                    accion_nuevo = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "0";
                    EvaluarAccion(accion_nuevo);

                    #endregion
                }

                recogerDatos();
                llenarTabla();
                comleeeGrid();
                anexos();
            }
            else
            {
                if (InformeIdInformeFinal != "0" && idEmpresa != "0")
                {
                    //Consultar estado.
                    ConsultarEstado(InformeIdInformeFinal);
                    recogerDatos();
                    llenarTabla();
                    comleeeGrid();
                    anexos();
                }
            }
        }

        #region Métodos para consultar datos.

        private void recogerDatos()
        {
            InformeIdInformeFinal = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            idEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

            if (!string.IsNullOrEmpty(idEmpresa))
            {
                txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa=" + idEmpresa;

                var infoEmoresa = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoEmoresa.Rows.Count > 0)
                {
                    txtNomEmpresa = infoEmoresa.Rows[0]["RazonSocial"].ToString();
                    CodProyecto = infoEmoresa.Rows[0]["CodProyecto"].ToString();
                }
            }
            else
            { Response.Redirect("InformeConsolidadoInter.aspx"); }
        }

        private void llenarTabla()
        {
            var infoTanlaAux = new DataTable();

            txtSQL = "SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + idEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                lblEmpresa.Text = infoTanlaAux.Rows[0]["RazonSocial"].ToString();
                lblTelefono.Text = infoTanlaAux.Rows[0]["Telefono"].ToString();
                lblDireccion.Text = infoTanlaAux.Rows[0]["DomicilioEmpresa"].ToString() + "-" + infoTanlaAux.Rows[0]["NomCiudad"].ToString();
                //lblCiudad.Text = infoTanlaAux.Rows[0]["NomCiudad"].ToString();

                #region Código que estaba por fuera de la condición IF.
                lblinforme.Text = "Informe Interventoría Final - " + infoTanlaAux.Rows[0]["RazonSocial"].ToString();

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InterventorInformeFinal WHERE (id_InterventorInformeFinal =" + InformeIdInformeFinal + ")))";

                    infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                    if (infoTanlaAux.Rows.Count > 0)
                        L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0][0].ToString();

                    txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InterventorInformeFinal WHERE id_InterventorInformeFinal =" + InformeIdInformeFinal + "))))";

                    infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                    if (infoTanlaAux.Rows.Count > 0)
                        lblnomcoordinador.Text = "Coordinador: " + infoTanlaAux.Rows[0][0].ToString();
                }

                txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + idEmpresa;

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    lblnumContrato.Text = infoTanlaAux.Rows[0][0].ToString();

                txtSQL = "SELECT Id_InterventorInformeFinal, FechaInforme, Estado FROM InterventorInformeFinal WHERE  Id_InterventorInformeFinal= " + InformeIdInformeFinal;

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    lblfechainforme.Text = infoTanlaAux.Rows[0]["FechaInforme"].ToString();

                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + idEmpresa + "))";
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow fila in infoTanlaAux.Rows)
                {
                    TableRow filat = new TableRow();
                    TableCell celdat = new TableCell();
                    celdat.Text = "" + fila["Nombre"].ToString() + " Identificación: " + fila["Identificacion"].ToString();
                    filat.Cells.Add(celdat);
                    t_table.Rows.Add(filat);
                    t_table.DataBind();
                }
                #endregion
            }
        }

        private void comleeeGrid()
        {
            //Instacia de la tabla principal.
            var infoTanlaAux = new DataTable();

            //Cargar el rol del usuario.
            CargarRol();

            //Consulta.
            txtSQL = "SELECT Id_InterventorInformeFinalCriterio, NomInterventorInformeFinalCriterio FROM InterventorInformeFinalCriterio WHERE CodEmpresa IN (0," + idEmpresa + ") ORDER BY CodEmpresa, Id_InterventorInformeFinalCriterio";

            //Asignar resultados de la consulta.
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            //Variables inicializadoras para establacer ID's a controles dinámicos.
            int inicial = 0;
            int inicial2 = 0;

            //Variable que determina si se agrega el cumplimiento.
            bool agregarBotonEliminar = false;

            //Si hay datos...
            if (infoTanlaAux.Rows.Count > 0)
            {
                //Recorre las filas de la tabla.
                foreach (DataRow fil in infoTanlaAux.Rows)
                {
                    //Inicializar variables internas
                    TableHeaderRow filatablava = new TableHeaderRow();
                    LinkButton lnk_titulo = new LinkButton();
                    ImageButton img = new ImageButton();

                    #region Establecer valores del LinkButton.

                    lnk_titulo.ID = "lnk_titulo_0_" + inicial2.ToString();
                    lnk_titulo.Text = "Agregar";
                    lnk_titulo.CausesValidation = false;
                    lnk_titulo.CommandName = lnk_titulo.ID;
                    lnk_titulo.Visible = false;

                    //try { lnk_titulo.CommandArgument = fil["Id_InterventorInformeFinalCriterio"].ToString(); }
                    //catch { lnk_titulo.CommandArgument = "0"; }

                    try
                    {
                        //Infome, Empresa e ítem.
                        lnk_titulo.CommandArgument = fil["Id_InterventorInformeFinalCriterio"].ToString() + ";" + idEmpresa + ";" + InformeIdInformeFinal;
                    }
                    catch
                    { lnk_titulo.CommandArgument = "0" + ";" + idEmpresa + ";" + InformeIdInformeFinal; }

                    //lnk_titulo.Click += new EventHandler(lnk_titulo_Click);
                    lnk_titulo.Command += new CommandEventHandler(DynamicCommand_1);

                    if (Estado == 0 && usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["cod_rol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())
                    {
                        //Determina si este valor debe estar visible.
                        //El botón sólo debe estar visible si el botón "Imprimir" está visible.
                        //if (hdf_Estado.Value == "0") { lnk_titulo.Visible = false; }
                        lnk_titulo.Visible = true;
                    }

                    #endregion

                    #region Establecer valores del ImageButton.

                    img.ID = "img_" + inicial2.ToString();
                    img.ImageUrl = "../../Images/icoAdicionar.gif";
                    img.Command += new CommandEventHandler(DynamicCommand_3);
                    img.CommandName = "crear";
                    img.BackColor = System.Drawing.Color.White;
                    img.CausesValidation = false;
                    img.Visible = false;

                    try
                    {
                        //Infome, Empresa e ítem.
                        img.CommandArgument = fil["Id_InterventorInformeFinalCriterio"].ToString() + ";" + idEmpresa + ";" + InformeIdInformeFinal;
                    }
                    catch
                    { img.CommandArgument = "0" + ";" + idEmpresa + ";" + InformeIdInformeFinal; }

                    if (Estado == 0 && usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["cod_rol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())
                    {
                        //Determina si este valor debe estar visible.
                        //El botón sólo debe estar visible si el botón "Imprimir" está visible.
                        //if (hdf_Estado.Value == "0") { img.Visible = false; }
                        img.Visible = true;
                    }

                    #endregion

                    //Añadir el TableHeaderCell.
                    filatablava.Cells.Add(crearceladtitulo(fil["NomInterventorInformeFinalCriterio"].ToString(), 3, 1, "", lnk_titulo, img));
                    //Añadir el TableHeaderRow a la tabla en diseño.
                    t_variable.Rows.Add(filatablava);

                    //Siguiente consulta.
                    txtSQL = "SELECT Id_InterventorInformeFinalItem, NomInterventorInformeFinalItem, CodEmpresa FROM InterventorInformeFinalItem WHERE CodEmpresa IN (0," + idEmpresa + ") AND CodInformeFinalCriterio = " + fil["Id_InterventorInformeFinalCriterio"].ToString() + " ORDER BY CodEmpresa, Id_InterventorInformeFinalItem";

                    //Asignar resultados de la consulta a variable temporal.
                    var laAux = consultas.ObtenerDataTable(txtSQL, "text");

                    //Si la variable temporal contiene datos..
                    if (laAux.Rows.Count > 0)
                    {
                        //Recorrer filas de la variable temporal.
                        foreach (DataRow compl in laAux.Rows)
                        {
                            //Siguiente consulta.
                            txtSQL = "SELECT CodInformeFinal, Observaciones FROM InterventorInformeFinalCumplimiento WHERE CodInformeFinal = " + InformeIdInformeFinal + " AND CodInformeFinalItem = " + compl["Id_InterventorInformeFinalItem"].ToString() + " AND CodEmpresa = " + idEmpresa;

                            //Generar nueva variable temporal.
                            var laAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                            //Instancias de los controles.
                            LinkButton lnk_btn = new LinkButton();
                            Label txtObservacion = new Label();
                            ImageButton img_action = new ImageButton();

                            #region Establecer valores del LinkButton interno.

                            lnk_btn.ID = "lnk_btn" + inicial.ToString();
                            lnk_btn.Text = "Editar Texto";
                            //lnk_btn.Click += new EventHandler(lnk_Click);
                            lnk_btn.CausesValidation = false;
                            lnk_btn.CommandName = lnk_titulo.ID;
                            lnk_btn.Command += new CommandEventHandler(DynamicCommand_2);
                            lnk_btn.Visible = false;

                            //try { lnk_btn.CommandArgument = laAux2.Rows[0]["CodInformeFinal"].ToString(); }
                            try
                            {
                                //Infome, Empresa e ítem.
                                lnk_btn.CommandArgument = laAux2.Rows[0]["CodInformeFinal"].ToString() + ";" + idEmpresa + ";" + compl["Id_InterventorInformeFinalItem"].ToString();
                            }
                            catch
                            { lnk_btn.CommandArgument = InformeIdInformeFinal + ";" + idEmpresa + ";" + compl["Id_InterventorInformeFinalItem"].ToString(); }

                            if (Estado == 0 && usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["cod_rol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())
                            {
                                //Determina si este valor debe estar visible.
                                //El botón sólo debe estar visible si el botón "Imprimir" está visible.
                                //if (hdf_Estado.Value == "0") { lnk_btn.Visible = false; }
                                lnk_btn.Visible = true;
                            }

                            #endregion

                            #region Establecer valores del TextBox.

                            txtObservacion.ID = "txtObservacion" + inicial.ToString();

                            //Asignar texto al textbox.
                            if (laAux2.Rows.Count > 0)
                            { txtObservacion.Text = laAux2.Rows[0]["Observaciones"].ToString(); }

                            #endregion

                            #region Valores del ImageButton interno "para eliminar" (en caso contrario, NO agrega el ImageButton).

                            img_action.ID = "img_action" + inicial.ToString();
                            img_action.CausesValidation = false;
                            img_action.CommandName = "eliminar"; //lnk_titulo.ID;
                            img_action.Command += new CommandEventHandler(DynamicCommand_3);

                            try
                            {
                                //Infome, Empresa e ítem.
                                img_action.CommandArgument = laAux2.Rows[0]["CodInformeFinal"].ToString() + ";" + idEmpresa + ";" + compl["Id_InterventorInformeFinalItem"].ToString();
                            }
                            catch
                            { img_action.CommandArgument = InformeIdInformeFinal + ";" + idEmpresa + ";" + compl["Id_InterventorInformeFinalItem"].ToString(); }

                            //compl = laAux
                            #region Validar si se muestra y adiciona el botón de eliminación.

                            if (compl["CodEmpresa"].ToString() != "0")
                            {
                                //Si el estado es igual a cero y el usuario es del grupo "Interventor" y es de rol "RolInterventorLider".
                                if (Estado == 0 && usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["cod_rol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())
                                {
                                    //Mostrar y agregar el ImageButton de eliminación "Así como su propiedad de eliminación".
                                    img_action.ImageUrl = "../../Images/icoEliminar.gif";
                                    img_action.ToolTip = "Eliminar Cumplimiento";
                                    img_action.CommandName = "eliminar";
                                    img_action.OnClientClick = "return alerta()";
                                    //Agregar control.
                                    agregarBotonEliminar = true;

                                    //Determina si este valor debe estar visible.
                                    //El botón sólo debe estar visible si el botón "Imprimir" está visible.
                                    //if (hdf_Estado.Value == "0") { img_action.Visible = false; }
                                    img_action.Visible = true;
                                }
                                else { agregarBotonEliminar = false; img_action = null; }
                            }
                            #endregion

                            #endregion

                            //Crear instancia de TableRow.
                            TableRow filaNeA2 = new TableRow();

                            //Añadir elementos a la instacia generada anteriormente.
                            if (agregarBotonEliminar)
                            {
                                //filaNeA2.Cells.Add(celdaNormalCONTROL(img_action, null, 1, 1, ""));
                                filaNeA2.Cells.Add(celdaNormal("", 1, 1, "", img_action));
                                //Diego Quiñonez: establecer nuevamente la variable a FALSE, para evitar que
                                //se añada un ImageButton nuevo.
                                agregarBotonEliminar = false;
                            }
                            else
                            { img_action = null; filaNeA2.Cells.Add(celdaNormal("", 1, 1, "", null)); }

                            filaNeA2.Cells.Add(celdaNormal(compl["NomInterventorInformeFinalItem"].ToString() + ":", 1, 1, "", null));
                            filaNeA2.Cells.Add(celdaNormalCONTROL(lnk_btn, txtObservacion, 1, 1, ""));

                            //Añadir la instancia completa a la tabla en diseño.
                            t_variable.Rows.Add(filaNeA2);

                            //Incrementar valor de la variable "inicial".
                            inicial++;
                        }
                    }

                    //Incrementar valor de la variable "inicial2".
                    inicial2++;
                }
            }

            //Bindear la tabla.
            t_variable.DataBind();
        }

        private void anexos()
        {
            var infoTanlaAux = new DataTable();

            txtSQL = @"SELECT InterventorInformeFinalAnexos.Id_InterventorInformeFinalAnexos, " +
                     " InterventorInformeFinalAnexos.NomInterventorInformeFinalAnexos, InterventorInformeFinalAnexos.RutaArchivo, " +
                     " DocumentoFormato.Icono FROM InterventorInformeFinalAnexos INNER JOIN DocumentoFormato " +
                     " ON InterventorInformeFinalAnexos.CodDocumentoFormato = DocumentoFormato.Id_DocumentoFormato " +
                     " WHERE (InterventorInformeFinalAnexos.Borrado = 0) AND (InterventorInformeFinalAnexos.CodInformeFinal = " + InformeIdInformeFinal + ")";

            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                foreach (DataRow fil in infoTanlaAux.Rows)
                {
                    TableRow filaNeA2 = new TableRow();

                    filaNeA2.Cells.Add(celdaNormalCONTROL((new Image
                    {
                        ImageUrl = "~/Images/" + fil["Icono"].ToString()
                    }), null, 1, 1, ""));
                    filaNeA2.Cells.Add(celdaNormalCONTROL((new HyperLink
                    {
                        Text = "" + fil["NomInterventorInformeFinalAnexos"].ToString(),
                        Target = "_blank",
                        NavigateUrl = "" + fil["RutaArchivo"].ToString()
                    }),
                                                   null, 1, 1, ""));

                    t_anexos.Rows.Add(filaNeA2);
                }
                t_anexos.DataBind();
            }
            else
            {
                p_Anexos.Visible = false;
                p_Anexos.Enabled = false;
            }
        }

        private TableHeaderCell crearceladtitulo(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo, Control control, Control control2)
        {
            Label lbr = new Label();
            lbr.Text = "<br/>";
            var celda1 = new TableHeaderCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            var titulo1 = new Label { Text = mensaje };
            celda1.Controls.Add(titulo1);
            celda1.Controls.Add(lbr);
            if (control2 != null) { celda1.Controls.Add(control2); }
            if (control == null) { lbr = null; }
            else { celda1.Controls.Add(control); }

            return celda1;
        }

        /// <summary>
        /// Agregar celda normal.
        /// </summary>
        /// <param name="mensaje">String.</param>
        /// <param name="colspan">colspan.</param>
        /// <param name="rowspan">rowspan.</param>
        /// <param name="cssestilo">cssestilo.</param>
        /// <param name="control_img">Control ImageButton = Debe ser NULL si NO se desea agregar este u otro control.</param>
        /// <returns>TableCell.</returns>
        private TableCell celdaNormal(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo, Control control_img)
        {
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            var titulo1 = new Label { Text = mensaje };

            if (control_img != null)
            {
                celda1.Controls.Add(control_img);
                celda1.Controls.Add(titulo1);
            }
            else
            { celda1.Controls.Add(titulo1); }

            return celda1;
        }

        /// <summary>
        /// Método que acepta "entre otros parámetros" dos controles para añadirlos a la celda de la tabla en construcción.
        /// </summary>
        /// <param name="mensaje">Control 1.</param>
        /// <param name="mensaje2">Control 2 "si NO se requiere un segundo control, este valor DEBE ir NULL".</param>
        /// <param name="colspan">colspan</param>
        /// <param name="rowspan">rowspan</param>
        /// <param name="cssestilo">cssestilo</param>
        /// <returns>TableCell</returns>
        private TableCell celdaNormalCONTROL(Control mensaje, Control mensaje2, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            Label literalBreak = new Label();
            literalBreak.Text = "<br/>";
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            if (mensaje2 == null)
                literalBreak = null;
            else
            {
                celda1.Controls.Add(mensaje2);
                celda1.Controls.Add(literalBreak);
                celda1.Controls.Add(mensaje);
            }
            return celda1;
        }

        #endregion

        #region Métodos generales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Cargar el nombre del coordinador interventor quien es el "usuario creador del usuario en sesión".
        /// </summary>
        /// <returns></returns>
        private string CargarCoordinadorDelInterventor()
        {
            try
            {
                txtSQL = " SELECT Nombres + ' ' + Apellidos AS Nombre  " +
                         " FROM Contacto WHERE (Id_Contacto IN (SELECT codcoordinador FROM Interventor " +
                         " WHERE (CodContacto = " + usuario.IdContacto + ")))";

                var t = consultas.ObtenerDataTable(txtSQL, "text");

                if (t.Rows.Count > 0)
                {
                    string NombreCoordinador = t.Rows[0]["Nombre"].ToString();
                    t = null;
                    return NombreCoordinador;
                }
                else
                    return "";
            }
            catch { return ""; }
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 20/05/2014.
        /// Establecer acción a ejecutar, de acuerdo al contenido del parámetro.
        /// </summary>
        /// <param name="accion">Parámetro "Acción".</param>
        private void EvaluarAccion(string accion)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlConnection conn_ = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            bool correcto = false;
            DataTable tabla = new DataTable();
            var txtSQL = string.Empty;
            //IMPORTANTES!
            int Id_InterventorInformeFinalAnexos = 0;
            int Id_InterventorInformeFinalCriterio = 0; //Este valor cambia dependiendo de varios factores...

            try
            {
                switch (accion)
                {
                    case "Borrar":
                        //Borrar de la tabla InterventorInformeFinalCumplimiento
                        txtSQL = "DELETE FROM InterventorInformeFinalCumplimiento WHERE CodInformeFinal = " + InformeIdInformeFinal;
                        ejecutaReader(txtSQL, 2);

                        //Eliminar el archivo Físicamente
                        txtSQL = "select distinct RutaArchivo from InterventorInformeFinalAnexos WHERE CodInformeFinal = " + InformeIdInformeFinal;
                        var dt = consultas.ObtenerDataTable(txtSQL, "text");
                        var path = ConfigurationManager.AppSettings["RutaIP"];
                        if(dt.Rows.Count > 0)
                        {
                            foreach(DataRow f in dt.Rows)
                            {
                                try
                                {
                                    path += f.ItemArray[0].ToString();
                                    if (File.Exists(path))
                                    {
                                        File.Delete(path);
                                    }
                                }
                                catch(Exception e)
                                {

                                }
                            }
                        }

                        //Borrar de la tabla InterventorInformeFinalAnexos
                        txtSQL = "DELETE FROM InterventorInformeFinalAnexos WHERE CodInformeFinal = " + InformeIdInformeFinal;
                        ejecutaReader(txtSQL, 2);

                        //Borrar de la tabla InterventorInformeFinal
                        txtSQL = "DELETE FROM InterventorInformeFinal WHERE Id_InterventorInformeFinal = " + InformeIdInformeFinal;
                        ejecutaReader(txtSQL, 2);

                        //FInaliza
                        Session["Accion"] = null;
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Informe borrado correctamente!'); window.location = 'InformeConsolidadoInter.aspx';", true);
                        break;

                    case "BorrarAnexos":
                        #region Borrar archivos anexos...

                        //Modificacion para Eliminar el archivo Físicamente - Vladimir Delgado Barbosa 15 Mayo de 2007.
                        txtSQL = " select distinct RutaArchivo FROM InterventorInformeFinalAnexos " +
                                 " WHERE Id_InterventorInformeFinalAnexos = " + Id_InterventorInformeFinalAnexos; //& fnRequest("CodAnexo");

                        tabla = null;
                        tabla = consultas.ObtenerDataTable(txtSQL, "text");

                        for (int i = 0; i < tabla.Rows.Count; i++)
                        {
                            //En cada ciclo active el método "BorrarArchivo".
                            //ESTE MÉTODO NO EXISTE AQUÍ!
                            //BorrarArchivo(tabla.Rows[i]["RutaArchivo"].ToString()); //(RSB2("RutaArchivo")
                        }

                        //Borrar de la tabla de anexos.
                        txtSQL = " DELETE FROM InterventorInformeFinalAnexos " +
                                 " WHERE Id_InterventorInformeFinalAnexos = " + Id_InterventorInformeFinalAnexos; //& fnRequest("CodAnexo")

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(txtSQL, conn);

                        //Ejecutar SQL.
                        // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                        //correcto = EjecutarSQL(conn, cmd);
                        correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;
                        //Terminar y salir.
                        txtSQL = ""; tabla = null;

                        #endregion
                        Session["Accion"] = null;
                        break;

                    case "BorrarItem":
                        #region BorrarItem "Se DEBE USAR el evento "DynamicCommand_3".

                        #region Borrar de la tabla de InterventorInformeFinalCumplimiento.

                        txtSQL = " DELETE FROM InterventorInformeFinalCumplimiento " +
                                 " WHERE CodInformeFinal = " + InformeIdInformeFinal +
                                 " AND CodInformeFinalItem = " + Id_InterventorInformeFinalCriterio + //+ fnRequest("CodItem") + 
                                 " AND CodEmpresa = " + idEmpresa;

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(txtSQL, conn);

                        //Ejecutar SQL.
                        // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                        //correcto = EjecutarSQL(conn, cmd);
                        correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;

                        #endregion

                        #region Borrar de la tabla de InterventorInformeFinalItem.

                        txtSQL = " DELETE FROM InterventorInformeFinalItem " +
                                 " WHERE Id_InterventorInformeFinalItem = " + Id_InterventorInformeFinalCriterio;// fnRequest("CodItem")

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(txtSQL, conn);

                        //Ejecutar SQL.
                        // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                        //correcto = EjecutarSQL(conn, cmd);
                        correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;
                        Session["Accion"] = null;
                        #endregion

                        #endregion
                        break;

                    case "Enviar":
                        #region Enviar "Evaluando internamente los Grupos/Roles.

                        #region Para Interventor...

                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        {
                            //Verifica si el interventor tiene un coordinador asignado
                            txtSQL = " select CodCoordinador from interventor where codcontacto = " + usuario.IdContacto;

                            //Asignar resultados de la consulta a variable DataTable.
                            tabla = null;
                            tabla = consultas.ObtenerDataTable(txtSQL, "text");

                            //Si tiene datos...
                            if (tabla.Rows.Count > 0)
                            {
                                //Actualiza el estado lo pone en 1.
                                txtSQL = " UPDATE InterventorInformeFinal SET Estado = 1 " +
                                         " WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                //Asignar SqlCommand para su ejecución.
                                //cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                try
                                {
                                    ejecutaReader(txtSQL, 2);
                                    correcto = true;
                                }
                                catch(Exception ex)
                                {
                                    correcto = false;
                                }

                                //Si es correcto, hace la notificación de tareas...
                                if (correcto == true)
                                {
                                    Session["Accion"] = null;
                                    //Generar tarea...
                                    //prTareaAsignarCoordinadorInfAnual = COMENTADO EN FONADE CLÁSICO.
                                    AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Anual",
                                        "Revisión del Informe Anual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 28, "0", false, 1,
                                        true, false, usuario.IdContacto,
                                        "Accion=Editar&CodEmpresa=" + idEmpresa + "&CodInforme=" + InformeIdInformeFinal, "", "Asignar Coordinador Inf Anual");
                                    tarea.Agendar();
                                    //Response.Redirect("InformeConsolidadoInter.aspx");
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.location='InformeConsolidadoInter.aspx'; ", true);
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Final.')", true);
                                    return;
                                }
                            }
                        }

                        #endregion

                        #region Para Coordinador Interventor...

                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            if (Aprobar.Items[0].Selected)
                            {
                                #region Si fue aprobado, hace una atualización y genera tareas.

                                #region Actualización y continuación del flujo.

                                txtSQL = " UPDATE InterventorInformeFinal SET Estado = 2" +
                                                                 " WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                                //correcto = EjecutarSQL(conn, cmd);
                                correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;

                                if (correcto == true)
                                {
                                    //Consulta #2.
                                    txtSQL = "select Id_grupo from Grupo where NomGrupo = 'Gerente Interventor' ";
                                    tabla = null;
                                    tabla = consultas.ObtenerDataTable(txtSQL, "text");

                                    //Consulta #3.
                                    txtSQL = "select CodContacto from GrupoContacto where CodGrupo =" + tabla.Rows[0]["Id_grupo"].ToString();

                                    //Generar tarea:
                                    //prTareaAsignarGerenteInfAnual //COMENTADO EN FONADE CLÁSICO.
                                    Session["Accion"] = null;
                                    AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Anual",
                                            "Revisión del Informe Anual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 29, "0", false, 1,
                                            true, false, usuario.IdContacto,
                                            "Accion=Editar&CodEmpresa=" + idEmpresa + "&CodInforme=" + InformeIdInformeFinal, "", "Asignar Gerente Inf Anual");
                                    tarea.Agendar();
                                    //Response.Redirect("InformeConsolidadoInter.aspx");
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.location.reload(false); ", true);
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Consolidado.')", true);
                                    return;
                                }

                                #endregion

                                #endregion
                            }
                            else
                            {
                                #region Si NO fue aprobado...

                                //Actualización.
                                txtSQL = "UPDATE InterventorInformeFinal SET Estado = 0 WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                                //correcto = EjecutarSQL(conn, cmd);
                                correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;

                                if (correcto == true)
                                {
                                    Session["Accion"] = null;
                                    //Se debe generar tarea para el interventor para corregir el informe.
                                    txtSQL = " SELECT nomInterventorInformeFinal, CodInterventor FROM InterventorInformeFinal " +
                                             " WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                    //Asignar resultados de la consulta.
                                    tabla = null;
                                    tabla = consultas.ObtenerDataTable(txtSQL, "text");

                                    //Si tiene datos, genera tarea.
                                    if (tabla.Rows.Count > 0)
                                    {
                                        //Se genera tarea pendiente para el interventor para que vuelva a ingresar el informe.
                                        AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisar Informe Consolidado",
                                            "El coordinador de Interventoria no aprueba el informe consolidado por los siguientes motivos: ", //+ txt_ComentariosAprobacion.Text,
                                            hdf_cod_proyecto.Value, 28, "0", false, 1,
                                            true, false, usuario.IdContacto,
                                            "Accion=Editar&CodEmpresa=" + idEmpresa + "&CodInforme=" + InformeIdInformeFinal, "", "Informe Interventoria");
                                        tarea.Agendar();
                                        //Response.Redirect("InformeConsolidadoInter.aspx");
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.location.reload(false); ", true);
                                    }
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Consolidado.')", true);
                                    return;
                                }

                                #endregion
                            }
                        }

                        #endregion

                        #region Para Gerente Interventor...

                        if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            if (Aprobar.Items[0].Selected)
                            {
                                #region Si está aprobado...

                                //Actualización.
                                txtSQL = " UPDATE InterventorInformeFinal SET Estado = 3 WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                                //correcto = EjecutarSQL(conn, cmd);
                                correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;

                                //Verificar.
                                if (correcto == false)
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Consolidado.')", true);
                                    return;
                                }
                                else
                                {
                                    Session["Accion"] = null;
                                    //Response.Redirect("InformeConsolidadoInter.aspx");
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.location.reload(false); ", true);
                                }

                                #endregion
                            }
                            else
                            {
                                #region Si NO está aprobado...

                                //Actualización.
                                txtSQL = " UPDATE InterventorInformeFinal SET Estado = 0 " +
                                         " WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                // 2014/12/30 RAlvaradoT no se ejecutaba por eso siempre da false y no crea la tarea
                                //correcto = EjecutarSQL(conn, cmd);
                                correcto = (cmd.ExecuteNonQuery() > 0) ? true : false;

                                if (correcto == true)
                                {
                                    Session["Accion"] = null;
                                    #region Continuar con el flujo y generar tareas.

                                    //Se debe generar tarea para el interventor para corregir el informe
                                    txtSQL = "SELECT nomInterventorInformeFinal, CodInterventor FROM InterventorInformeFinal WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                                    //Asignar resultados de la consulta a variable DataTable.
                                    tabla = null;
                                    tabla = consultas.ObtenerDataTable(txtSQL, "text");

                                    //Si tiene datos, genera tarea.
                                    if (tabla.Rows.Count > 0)
                                    {
                                        //Se genera tarea pendiente para el interventor para que vuelva a ingresar el informe.
                                        AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisar Informe Consolidado",
                                            "El Gerente de Interventoria no aprueba el informe consolidado por los siguientes motivos:  " /*+txt_ComentariosAprobacion.Text*/, hdf_cod_proyecto.Value, 28, "0", false, 1,
                                            true, false, usuario.IdContacto,
                                            "Accion=Editar&CodInforme=" + InformeIdInformeFinal + "&CodEmpresa=" + idEmpresa, "", "Informe Interventoria");
                                        tarea.Agendar();

                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Informe se ha devuelto al Interventor!!!');window.opener.location.reload();window.close();", true);
                                    }

                                    #endregion
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Consolidado.')", true);
                                    return;
                                }

                                #endregion
                            }
                        }

                        #endregion

                        #endregion
                        break;

                    case "Guardar":
                        #region Actualiza la fecha de realización del informe.
                        DateTime fecha = new DateTime();
                        try
                        {
                            fecha = Convert.ToDateTime(txtDate.Text);
                        }
                        catch
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('hubo un error al depurar la fecha.')", true);
                            return;
                        }

                        //txtSQL = "UPDATE InterventorInformeFinal SET FechaInforme = '" + fecha.ToString("dd/MM/yyyy") + "' WHERE Id_InterventorInformeFinal = " + InformeIdInformeFinal;
                        try
                        {
                            var infoFinalInter = (from ii in consultas.Db.InterventorInformeFinal
                                                  where ii.Id_InterventorInformeFinal == int.Parse(InformeIdInformeFinal)
                                                  select ii).FirstOrDefault();
                            infoFinalInter.FechaInforme = fecha;
                            consultas.Db.SubmitChanges();
                            //ejecutaReader(txtSQL, 2);
                            correcto = true;
                        }
                        catch(Exception ex)
                        {
                            correcto = false;
                        }

                        if (correcto == true)
                        {
                            //Terrminar y salir.
                            Session["Accion"] = null;
                            Response.Redirect("InformeConsolidadoInter.aspx");
                            //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Informe guardado satisfactoriamente!'); window.location('InformeConsolidadoInter.aspx'); ", true);
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo guardar el Informe Final.')", true);
                            return;
                        }
                        #endregion
                        break;

                    case "Nuevo":
                        #region Antes de editar el reporte se debe agregar a la tabla.
                        var empresa = (from e in consultas.Db.Empresas where e.id_empresa == int.Parse(Session["CodEmpresa"].ToString())
                                           select e).FirstOrDefault();

                        var obj = new Datos.InterventorInformeFinal
                        {
                            NomInterventorInformeFinal = "Informe Interventoría Final - " + empresa.razonsocial,
                            CodInterventor = usuario.IdContacto,
                            FechaInforme = DateTime.Now,
                            Estado = 0,
                            CodEmpresa = empresa.id_empresa
                        };

                        try
                        {
                            consultas.Db.InterventorInformeFinal.InsertOnSubmit(obj);
                            consultas.Db.SubmitChanges();
                            correcto = true;
                        }
                        catch(Exception ex)
                        {
                            correcto = false;
                        }

                        if (correcto == true)
                        {
                            HttpContext.Current.Session["CodInforme"] = obj.Id_InterventorInformeFinal.ToString();
                            InformeIdInformeFinal = obj.Id_InterventorInformeFinal.ToString();
                            Session["Accion"] = null;
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al crear un nuevo Informe Final.')", true);
                            return;
                        }

                        #endregion
                        break;

                    default:
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/05/2014.
        /// Verificar si la tarea ya fue aprobada.
        /// </summary>
        private void VerificarTareaAprobada()
        {
            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            {
                //Consulta.
                txtSQL = " SELECT Estado FROM InterventorInformeFinal WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos...
                if (RS.Rows.Count > 0)
                {
                    //Si el estado es igual a 2 y está aprobado, la tarea ya fué asignada.
                    if ((RS.Rows[0]["Estado"].ToString() == "2") && (Aprobar.Items[0].Selected)) //1
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');window.opener.location.reload();window.close();", true);
                    }
                }
            }
            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                //Consulta.
                txtSQL = " SELECT Estado FROM InterventorInformeFinal WHERE id_InterventorInformeFinal = " + InformeIdInformeFinal;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos...
                if (RS.Rows.Count > 0)
                {
                    //Si el estado es igual a 3 y está aprobado, la tarea ya fué asignada.
                    if ((RS.Rows[0]["Estado"].ToString() == "3") && (Aprobar.Items[0].Selected)) //1
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');window.opener.location.reload();window.close();", true);
                    }
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 20/05/2014.
        /// Traer el id del informe final
        /// </summary>
        /// <param name="codInforme">Código de Informe.</param>
        private void TraerIdInformeFinal(String codInforme)
        {
            try
            {
                //Trae el id del informe final
                txtSQL = " SELECT Id_InterventorInformeFinal, FechaInforme, Estado " +
                         " FROM InterventorInformeFinal WHERE Id_InterventorInformeFinal = " + codInforme;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Asignar los siguientes valores a variables ocultas.
                hdf_CodInforme.Value = RS.Rows[0]["Id_InterventorInformeFinal"].ToString();
                hdf_FechaInforme.Value = RS.Rows[0]["FechaInforme"].ToString();
                hdf_Estado.Value = RS.Rows[0]["Estado"].ToString();

                //Ocultar botones y sólo mostrar el botón "Imprimir".
                if (hdf_Estado.Value != "0")
                {
                    btn_guardar.Visible = false;
                    btn_borrar.Visible = false;
                    btn_enviar.Visible = false;
                    btn_imprimir.Visible = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 20/05/2014.
        /// De acuerdo a la variable oculta (en este caso), el contenido de la variable de sesión "Accion", realiza
        /// la acción evaluada a su vez en el método "EvaluarAccion".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_Click(object sender, EventArgs e)
        {
            accion = "Guardar";
            try { EvaluarAccion(accion); }
            catch { return; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 20/05/2014.
        /// Borrar el informe...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_borrar_Click(object sender, EventArgs e)
        {
            try { EvaluarAccion("Borrar"); }
            catch { return; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 20/05/2014.
        /// Ebviar el informe final.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_enviar_Click(object sender, EventArgs e)
        {
            try { EvaluarAccion("Enviar"); }
            catch { return; }
        }

        /// <summary>
        /// Consultar el estado del informe.
        /// </summary>
        /// <param name="codInforme">Código del informe</param>
        private void ConsultarEstado(String codInforme)
        {
            try
            {
                //Trae el id del informe final
                txtSQL = " SELECT Estado FROM InterventorInformeFinal WHERE Id_InterventorInformeFinal = " + codInforme;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                { Estado = Int32.Parse(RS.Rows[0]["Estado"].ToString()); }

                //Destruir variable.
                RS = null;
            }
            catch { }
        }

        #region Métodos dinámicos. "http://stackoverflow.com/questions/4041137/setting-linkbuttons-onclick-event-to-method-in-codebehind".

        /// <summary>
        /// Evento RowCommand para establecer funcionalidad al seleccionar el control ubicado en el título/encabezado/TableHeader.
        /// Invoca ventana emergente para crear cumplimientos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_1(Object sender, CommandEventArgs e)
        {
            try
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["s_hdf_CodInforme"] = valores_command[2];
                HttpContext.Current.Session["s_hdf_CodItem"] = valores_command[0];
                HttpContext.Current.Session["s_hdf_CodEmpresa"] = valores_command[1];
                Redirect(null, "CatalogoCumpliInformeInter.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            catch
            { return; }
        }

        /// <summary>
        /// Evento RowCommand para establecer funcionalidad al seleccionar el control ubicado 
        /// en el detalle del ámbito/encabezado/TableCell.
        /// Invoca ventana emergente para crear o actualizar item informe final.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_2(Object sender, CommandEventArgs e)
        {
            try
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["s_hdf_CodInforme"] = valores_command[0];
                HttpContext.Current.Session["s_hdf_CodItem"] = valores_command[2];
                HttpContext.Current.Session["s_hdf_CodEmpresa"] = valores_command[1];
                Redirect(null, "CatalogoInformeFinalEditarItem.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            catch
            { return; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/05/2014.
        /// Dependiendo del CommandName "si es Eliminar o Crear", eliminará o creará el cumplimiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_3(Object sender, CommandEventArgs e)
        {
            try
            {
                //Inicializar variables.
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand();
                bool correcto = false;

                //Inicializar variables para obtener por separado la información almacenada en el CommandArgument.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Si es "Eliminar"...
                if (e.CommandName.Equals("eliminar"))
                {
                    #region Eliminar cumplimiento.

                    #region Comentarios.
                    //Eliminar cumplimiento..
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Item: " + valores_command[2] + ", Informe: " + valores_command[0] + " , Empresa: " + valores_command[1] + "');", true);
                    //return; 
                    #endregion

                    #region PASO 1: Borrar de la tabla de InterventorInformeFinalCumplimiento.
                    
                    txtSQL = " DELETE FROM InterventorInformeFinalCumplimiento " +
                             " WHERE CodInformeFinal = " + valores_command[0] +
                             " AND CodInformeFinalItem = " + valores_command[2] +
                             " AND CodEmpresa = " + valores_command[1];

                    ////Asignar SqlCommand para su ejecución.
                    //cmd = new SqlCommand(txtSQL, conn);

                    ////Ejecutar SQL.
                    //correcto = EjecutarSQL(conn, cmd);
                    try
                    {
                        ejecutaReader(txtSQL, 2);
                        correcto = true;
                    }
                    catch(Exception ex)
                    {
                        correcto = false;
                    }

                    if (correcto == true)
                    {
                        #region PASO 2: Borrar de la tabla de InterventorInformeFinalItem.

                        txtSQL = " DELETE FROM InterventorInformeFinalItem " +
                                 " WHERE Id_InterventorInformeFinalItem = " + valores_command[2];

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(txtSQL, conn);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(conn, cmd);

                        if (correcto == true)
                        {
                            //Refrescar la ventana para observar los cambios.
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Cumplimiento eliminado correctamente.');window.location.reload(false);", true);
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hubo un error al eliminar el cumplimiento (2).')", true);
                            return;
                        }

                        #endregion
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hubo un error al eliminar el cumplimiento (1).')", true);
                        return;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    //Crear Cumplimiento.
                    HttpContext.Current.Session["s_hdf_CodInforme"] = valores_command[2];
                    HttpContext.Current.Session["s_hdf_CodItem"] = valores_command[0];
                    HttpContext.Current.Session["s_hdf_CodEmpresa"] = valores_command[1];
                    Redirect(null, "CatalogoCumpliInformeInter.aspx", "_blank",
                             "menubar=0,scrollbars=1,width=710,height=400,top=100");
                }
            }
            catch { return; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 22/05/2014.
        /// Cargar en variable de sesión "Session["cod_rol"]" el rol del usuario, al ejecutar una consulta SQL.
        /// </summary>
        private void CargarRol()
        {
            try
            {
                txtSQL = " SELECT TOP 1 CodContacto,Rol From EmpresaInterventor,Empresa " +
                       " WHERE id_empresa = codempresa AND CodContacto =" + usuario.IdContacto +
                       " AND inactivo = 0 and FechaFin IS NULL ORDER BY rol DESC";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                    HttpContext.Current.Session["cod_rol"] = dt.Rows[0]["Rol"];
                else
                    HttpContext.Current.Session["cod_rol"] = "0";
            }
            catch
            { HttpContext.Current.Session["cod_rol"] = "0"; }
        }

        #endregion
    }
}