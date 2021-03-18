using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using System.Globalization;
using System.Data;
using Fonade.Clases;
using Fonade.Status;

namespace Fonade.FONADE.evaluacion
{
    public partial class CatalogoProduccionTMP : Base_Page
    {
        public String CodProyecto;
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        string pagina;

        /// <summary>
        /// Código del producto (producción) seleccionado.
        /// </summary>
        public string codProduccion;

        /// <summary>
        /// Usado para determinar si hace la búsqueda en la tabla TMP o normal.
        /// Ej: si este valor es != null, hace la consulta en "InterventorProduccionMesTMP", de lo contrario
        /// consultará en "InterventorProduccionMes".
        /// </summary>
        public string ValorTMP;

        /// <summary>
        /// Valor que es recibido por sesión y contiene los valores "Cargo" ó "Insumo".
        /// </summary>
        public string v_Tipo;

        /// <summary>
        /// Nombre del proyecto.
        /// </summary>
        String txtNomProyecto;

        /// <summary>
        /// Indica que el valor seleccionado viene de "CambiosPO.aspx".
        /// </summary>
        String Detalles_CambiosPO_PO;

        /// <summary>
        /// Variable que contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Establecer en variable de sesión "txtObjeto" el valor correspondiente de acuerdo a su rol.
                //Si es coordinador o gerente, se crea la sesión "txtObjeto" con el valor "Agendar Tarea";
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { HttpContext.Current.Session["txtObjeto"] = "Agendar Tarea"; }
                //Si es Interventor, lo hace con otro dato.
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                { HttpContext.Current.Session["txtObjeto"] = "Mis Proyectos para Seguimiento de Interventoría"; }
                //Si es Evaluador...
                if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                { HttpContext.Current.Session["txtObjeto"] = "Mis planes de negocio a evaluar"; }
                #endregion

                //Obtener la información almacenada en las variables de sesión.
                CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
                codProduccion = HttpContext.Current.Session["CodProducto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProducto"].ToString()) ? HttpContext.Current.Session["CodProducto"].ToString() : "0";
                ValorTMP = HttpContext.Current.Session["ValorTMP"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ValorTMP"].ToString()) ? HttpContext.Current.Session["ValorTMP"].ToString() : "0";
                pagina = HttpContext.Current.Session["pagina"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["pagina"].ToString()) ? HttpContext.Current.Session["pagina"].ToString() : "0";
                v_Tipo = HttpContext.Current.Session["v_Tipo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["v_Tipo"].ToString()) ? HttpContext.Current.Session["v_Tipo"].ToString() : "";
                HttpContext.Current.Session["Accion"] = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "";
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                #region Consulta para traer el nombre del proyecto
                txtSQL = "select NomProyecto from Proyecto WHERE id_proyecto = " + CodProyecto;
                var rr = consultas.ObtenerDataTable(txtSQL, "text");
                if (rr.Rows.Count > 0) { txtNomProyecto = rr.Rows[0]["NomProyecto"].ToString(); rr = null; }
                #endregion
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                #region Variables de sesión creadas en "CambiosPO.aspx".
                //Sesión inicial que indica que la información a procesar proviene de "CambiosPO.aspx".
                Detalles_CambiosPO_PO = HttpContext.Current.Session["Detalles_CambiosPO_PO"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Detalles_CambiosPO_PO"].ToString()) ? HttpContext.Current.Session["Detalles_CambiosPO_PO"].ToString() : "";
                #endregion

                //Revisar si la variable contiene datos "debe ser así para volver visibles ciertos campos".
                if (Detalles_CambiosPO_PO.Trim() != "" && codProduccion != "0")
                {
                    #region CambiosPO.aspx.
                    //Mostrar campos.
                    lbl_inv_aprobar.Visible = true;
                    dd_inv_aprobar.Visible = true;
                    lbl_inv_obvservaciones.Visible = true;
                    txt_inv_observaciones.Visible = true;

                    //Inhabilitar panel que contiene la tabla dinámica.
                    P_Meses.Enabled = false;
                    if (!IsPostBack)
                    {
                        //Evaluar la acción a tomar.
                        if (HttpContext.Current.Session["Accion"].ToString().Equals("Adicionar") || HttpContext.Current.Session["Accion"].ToString().Equals("crear"))
                        {
                            B_Acion.Text = "Crear";
                            lbl_enunciado.Text = "Adicionar";
                            TB_Item.Visible = true;
                            L_Item.Visible = true;
                            TB_Item.Text = "";
                            //txt_inv_observaciones.Text = "";
                        }
                        else if (HttpContext.Current.Session["Accion"].ToString().Equals("actualizar") || HttpContext.Current.Session["Accion"].ToString().Equals("Modificar"))
                        {
                            B_Acion.Text = "Actualizar";
                            CargarTitulo("Modificar");
                        }
                        else if (HttpContext.Current.Session["Accion"].ToString().Equals("Borrar") || HttpContext.Current.Session["Accion"].ToString().Equals("borrar"))
                        {
                            B_Acion.Text = "Borrar";
                        }
                        else
                        {
                            L_Item.Visible = true;
                            TB_Item.Enabled = true;
                        }

                        var producto = (from ip in consultas.Db.InterventorProduccionTMPs
                                        where ip.id_produccion == int.Parse(Session["CodProducto"].ToString())
                                        select ip).FirstOrDefault();
                        if (producto != null)
                        {
                            TB_Item.Text = producto.NomProducto;
                            TB_Item.Visible = true;
                            TB_Item.Enabled = true;
                        }

                        CargarCombo();
                    }
                    //Llenar el panel.
                    //llenarpanel();
                    //Buscar datos.
                    //BuscarDatos_Produccion(true);
                    #endregion
                }
                else
                {
                    #region FrameProduccionInter.aspx.
                    if (!IsPostBack)
                    {
                        //llenarpanel();
                        if (HttpContext.Current.Session["Accion"].ToString().Equals("Adicionar") || HttpContext.Current.Session["Accion"].ToString().Equals("crear"))
                        {
                            B_Acion.Text = "Crear";
                            lbl_enunciado.Text = "Adicionar";
                            TB_Item.Visible = true;
                            L_Item.Visible = true;
                            TB_Item.Text = "";
                            txt_inv_observaciones.Text = "";
                        }
                        if (HttpContext.Current.Session["Accion"].ToString().Equals("actualizar") || HttpContext.Current.Session["Accion"].ToString().Equals("Modificar"))
                        {
                            B_Acion.Text = "Actualizar";
                            lbl_enunciado.Text = "Editar";

                            //TB_Item.Text = "null";
                            TB_Item.Visible = true;
                            L_Item.Visible = true;
                            TB_Item.Enabled = false;
                            //TB_Item.Text = "";

                            //BuscarDatos_Produccion(false);
                        }

                        if (HttpContext.Current.Session["Accion"].ToString().Equals("Borrar") || HttpContext.Current.Session["Accion"].ToString().Equals("borrar"))
                        {
                            B_Acion.Text = "Borrar";
                        }
                    }
                    //Aplicar para producción.
                    //llenarpanel();
                    #endregion
                }
                llenarpanel();
                BuscarDatos_Produccion(false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CargarCombo()
        {
            dd_inv_aprobar.Items.Insert(0, new ListItem("No", "0"));
            dd_inv_aprobar.Items.Insert(1, new ListItem("Si", "1"));
        }

        /// <summary>
        /// Generar tabla.
        /// Mauricio Arias Olave "15/04/2014": Se cambia el código fuente para generar 14 meses.
        /// </summary>
        private void llenarpanel()
        {
            TableRow filaTablaMeses = new TableRow();
            T_Meses.Rows.Add(filaTablaMeses);
            TableRow filaTablaFondo = new TableRow();
            T_Meses.Rows.Add(filaTablaFondo);
            TableRow filaTablaAporte = new TableRow();
            T_Meses.Rows.Add(filaTablaAporte);
            TableRow filaTotal = new TableRow();
            T_Meses.Rows.Add(filaTotal);

            #region Nuevas líneas, con el valor obtenido de la prórrgoa se suma a la constante de meses y se genera la tabla.
            int prorroga = 0;
            prorroga = ObtenerProrroga(CodProyecto);
            int prorrogaTotal = prorroga + Constantes.CONST_Meses + 1; //El +1 es paar evitar modificar aún mas el for...
            #endregion

            for (int i = 1; i <= prorrogaTotal; i++) //for (int i = 1; i <= 13; i++) = Son 14 meses según el FONADE clásico.
            {
                TableCell celdaMeses;
                TableCell celdaFondo;
                TableCell celdaAporte;
                TableCell celdaTotal;


                if (i == 1)
                {
                    celdaMeses = new TableCell();
                    celdaFondo = new TableCell();
                    celdaAporte = new TableCell();
                    celdaTotal = new TableCell();


                    Label labelMes = new Label();
                    Label labelFondo = new Label();
                    Label labelAportes = new Label();
                    Label labelTotal = new Label();

                    labelMes.ID = "labelMes";
                    labelFondo.ID = "labelfondo";
                    labelFondo.Text = "Cantidad";
                    labelAportes.ID = "labelaportes";
                    labelAportes.Text = "Costo";
                    labelTotal.ID = "L_SumaTotales";
                    labelTotal.Text = "Total";


                    celdaMeses.Controls.Add(labelMes);
                    celdaFondo.Controls.Add(labelFondo);
                    celdaAporte.Controls.Add(labelAportes);
                    celdaTotal.Controls.Add(labelTotal);


                    filaTablaMeses.Cells.Add(celdaMeses);
                    filaTablaFondo.Cells.Add(celdaFondo);
                    filaTablaAporte.Cells.Add(celdaAporte);
                    filaTotal.Cells.Add(celdaTotal);
                }


                if (i < prorrogaTotal)//15
                {
                    celdaMeses = new TableCell();
                    celdaFondo = new TableCell();
                    celdaAporte = new TableCell();
                    celdaTotal = new TableCell();


                    celdaMeses.Width = 45;
                    celdaFondo.Width = 45;
                    celdaAporte.Width = 45;
                    celdaTotal.Width = 45;

                    //String variable = "Mes" + i;
                    Label labelMeses = new Label();
                    labelMeses.ID = "Mes" + i;
                    labelMeses.Text = "Mes " + i;
                    labelMeses.Width = 45;
                    TextBox textboxFondo = new TextBox();
                    textboxFondo.AutoPostBack = true;
                    textboxFondo.ID = "Fondoo" + i;
                    textboxFondo.Width = 45;
                    textboxFondo.TextChanged += new System.EventHandler(TextBox_TextChanged);

                    /*
                     * AsyncPostBackTrigger tgr = new AsyncPostBackTrigger();
                    tgr.ControlID = textboxFondo.ID;
                    tgr.EventName = "TextChanged";
                    updatepanel.Triggers.Add(tgr);
                     */

                    textboxFondo.MaxLength = 10;
                    TextBox textboxAporte = new TextBox();
                    textboxAporte.AutoPostBack = true;
                    textboxAporte.ID = "Aporte" + i;
                    textboxAporte.Width = 45;
                    textboxAporte.TextChanged += new EventHandler(TextBox_TextChanged);

                    textboxAporte.AutoPostBack = true;
                    /*
                     * AsyncPostBackTrigger tgr1 = new AsyncPostBackTrigger();
                    tgr1.ControlID = textboxAporte.ID;
                    tgr1.EventName = "TextChanged";
                    updatepanel.Triggers.Add(tgr1);
                    */
                    textboxAporte.MaxLength = 10;
                    Label totalMeses = new Label();
                    totalMeses.ID = "TotalMes" + i;
                    totalMeses.Text = "0.0";
                    totalMeses.Width = 45;

                    //P_Meses.Controls.Add(label);
                    celdaMeses.Controls.Add(labelMeses);


                    celdaFondo.Controls.Add(textboxFondo);
                    celdaAporte.Controls.Add(textboxAporte);
                    celdaTotal.Controls.Add(totalMeses);

                    filaTablaMeses.Cells.Add(celdaMeses);
                    filaTablaFondo.Cells.Add(celdaFondo);
                    filaTablaAporte.Cells.Add(celdaAporte);
                    filaTotal.Cells.Add(celdaTotal);
                }
                if (i == prorrogaTotal)//15
                {

                    if (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {

                        celdaMeses = new TableCell();
                        celdaFondo = new TableCell();
                        celdaAporte = new TableCell();
                        celdaTotal = new TableCell();


                        Label labelMes = new Label();
                        Label labelFondo = new Label();
                        Label labelAportes = new Label();
                        Label labelTotal = new Label();


                        labelMes.ID = "labelMescosto";
                        labelMes.Text = "Costo Total";
                        labelFondo.ID = "labelfondocosto";
                        labelFondo.Text = "0";
                        labelAportes.ID = "labelaportescosto";
                        labelAportes.Text = "0";
                        labelTotal.ID = "L_SumaTotalescosto";
                        labelTotal.Text = "0";


                        celdaMeses.Controls.Add(labelMes);
                        celdaFondo.Controls.Add(labelFondo);
                        celdaAporte.Controls.Add(labelAportes);
                        celdaTotal.Controls.Add(labelTotal);


                        filaTablaMeses.Cells.Add(celdaMeses);
                        filaTablaFondo.Cells.Add(celdaFondo);
                        filaTablaAporte.Cells.Add(celdaAporte);
                        filaTotal.Cells.Add(celdaTotal);

                    }
                }
            }
        }

        /// <summary>
        /// Dependiendo del valor, si es 1, creará registros, si es 2, los actualizará.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Acion_Click(object sender, EventArgs e)
        {
            if (B_Acion.Text.Equals("Crear")) alamcenar(1);
            if (B_Acion.Text.Equals("Actualizar")) alamcenar(2);
            if (B_Acion.Text.Equals("Borrar")) alamcenar(3);
        }

        // Creado por Alex Flautero - Diciembre 12 de 2014
        protected void dd_inv_aprobar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dd_inv_aprobar.SelectedValue == "0")
            {
                dd_inv_aprobar.SelectedValue = "1";
            }
            else
            {
                dd_inv_aprobar.SelectedValue = "0";
            }
        }

        ValidarActividades validarActividades = new ValidarActividades();

        /// <summary>
        /// Guardar y/o actualizar la información.
        /// </summary>
        /// <param name="acion"> Si el valor es 1, guardará la información, si es 2, actualizará dicha información.</param>
        private void alamcenar(int acion)
        {
            //Inicializar variables.            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String correcto = "";
            String Valor = "";
            DataTable RsActividad = new DataTable();
            DataTable ValidaContacto = new DataTable();
            DataTable Rs = new DataTable();
            String NomActividad = "";

            if (CodProyecto != "0" || codProduccion != "0")
            {
                if (acion == 1)
                {
                    #region Si es Interventor.
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        #region Guardar como Interventor.
                        string txtSQL = "select CodCoordinador from interventor where codcontacto=" + usuario.IdContacto;

                        var idCoordinador = consultas.ObtenerDataTable(txtSQL, "text").Rows[0].ItemArray[0].ToString();

                        int ActividadTmp;
                        txtSQL = "select id_Produccion from InterventorProduccionTMP ORDER BY id_Produccion DESC";
                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                        if (Rs.Rows.Count == 0)
                        {
                            ActividadTmp = 1;
                        }
                        else
                        {
                            ActividadTmp = Convert.ToInt32(Rs.Rows[0]["id_Produccion"]) + 1;
                        }

                        int cantRegProduccion = validarActividades.validarProduccion(Convert.ToInt32(CodProyecto), TB_Item.Text);

                        int cantRegProduccionTMP = validarActividades
                                                        .validarProduccionTMP(Convert.ToInt32(CodProyecto), TB_Item.Text);

                        if (cantRegProduccionTMP > 0 || cantRegProduccion > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una actividad de Produccion registrada con la misma informacion.');window.close();", true);
                        }
                        else
                        {

                            txtSQL = "Insert into InterventorProduccionTMP (id_Produccion,CodProyecto,NomProducto,ChequeoCoordinador,Tarea,ChequeoGerente) values (" + ActividadTmp + "," + CodProyecto + ",'" + TB_Item.Text + "',null,'Adicionar',null)";
                            string mes = "0";
                            string valor = "0";
                            string tipo = "0";
                            ejecutaReader(txtSQL, 2);

                            LogActivitdades.WriteError(3, txtSQL); //3 -> Produccion

                            #region Insercion de valores
                            foreach (Control miControl in T_Meses.Controls)
                            {
                                var tablerow = miControl.Controls;

                                foreach (Control micontrolrows in tablerow)
                                {
                                    var hijos = micontrolrows.Controls;

                                    foreach (Control chijos in hijos)
                                    {
                                        if (chijos.GetType() == typeof(TextBox))
                                        {
                                            var text = chijos as TextBox;

                                            if (text.ID.StartsWith("Fondoo")) //Catidad
                                            {
                                                tipo = "1";
                                            }
                                            else
                                            {
                                                if (text.ID.StartsWith("Aporte")) //Sueldo
                                                {
                                                    tipo = "2";
                                                }
                                            }
                                            if (string.IsNullOrEmpty(text.Text))
                                                valor = "0";
                                            else
                                                valor = text.Text;

                                            int limit = 0;
                                            if (text.ID.Length == 7)
                                                limit = 1;
                                            else
                                                limit = 2;

                                            mes = text.ID.Substring(6, limit);

                                            txtSQL = "INSERT INTO InterventorProduccionMesTMP(CodProducto,Mes,Valor,Tipo) " +
                                            "VALUES(" + ActividadTmp + "," + mes + "," + valor + "," + tipo + ")";

                                            ejecutaReader(txtSQL, 2);
                                        }
                                    }
                                }
                            }
                            #endregion

                            // Esta consulta trae el Coordinador interventor del proyecto 
                            txtSQL = "select codcoordinador as CodContacto from interventor where CodContacto in(" +
                                            " SELECT a.CodContacto" +
                                            " FROM EmpresaInterventor a INNER JOIN Empresa b ON a.CodEmpresa = b.id_empresa " +
                                            " WHERE a.Inactivo = 0 AND a.Rol = " + Constantes.CONST_RolInterventorLider +
                                            " AND b.codproyecto = " + CodProyecto + ")";
                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RsActividad.Rows.Count > 0)
                            {
                                //Agendar tarea.
                                AgendarTarea agenda = new AgendarTarea
                                (Int32.Parse(RsActividad.Rows[0]["CodContacto"].ToString()),
                                "Producto en producción enviado a Aprobacion por Interventor",
                                "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + TB_Item.Text + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                                CodProyecto,
                                18,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + CodProyecto,
                                "",
                                "Catálogo Producción");
                                //agenda.Agendar();
                            }
                            #endregion
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto creado');window.opener.location = window.opener.location;window.close();", true);
                        }

                    }
                    #endregion

                    #region Si es Gerente Interventor o Coordinador Interventor
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1") //Si
                        {
                            #region Aprobado gerente interventor
                            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                            {
                                #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL.

                                txtSQL = " select * from InterventorProduccionTMP " +
                                            " where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;

                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                                #endregion

                                #region INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA.

                                txtSQL = " Insert into InterventorProduccion (CodProyecto, NomProducto) " +
                                            " values (" + CodProyecto + ", '" + RsActividad.Rows[0]["NomProducto"].ToString() + "')";

                                //Ejecutar consulta.
                                cmd = new SqlCommand(txtSQL, conn);
                                correcto = String_EjecutarSQL(conn, cmd);

                                if (correcto != "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al insertar registros en la tabla definitiva: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                    return;
                                }

                                #endregion

                                #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.

                                txtSQL = " DELETE FROM InterventorProduccionTMP " +
                                            " where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;

                                //Ejecutar consulta.
                                cmd = new SqlCommand(txtSQL, conn);
                                correcto = String_EjecutarSQL(conn, cmd);

                                if (correcto != "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar registros de la tabla temporal: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                    return;
                                }

                                #endregion

                                #region TRAE EL CODIGO DE ACTIVIDAD PARA ADICIONARLO A LA TABLA DEFINITIVA POR MES.

                                txtSQL = "select id_Produccion from InterventorProduccion ORDER BY id_Produccion DESC";
                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");
                                Valor = RsActividad.Rows[0]["id_Produccion"].ToString();

                                #endregion

                                #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL POR MESES.

                                txtSQL = " select * from InterventorProduccionMesTMP " +
                                            " where CodProducto = " + codProduccion;
                                Rs = consultas.ObtenerDataTable(txtSQL, "text");

                                #endregion

                                #region INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA POR MESES.

                                foreach (DataRow row_Rs in Rs.Rows)
                                {
                                    if (row_Rs["Tipo"].ToString() == "1")
                                    {
                                        #region Ingresar Tipo 1.

                                        txtSQL = " Insert into InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) " +
                                                    " values (" + Valor + "," + row_Rs["Mes"].ToString() + ", " + row_Rs["Valor"].ToString() + ", 1) ";

                                        #endregion
                                    }
                                    else
                                    {
                                        #region Ingresar Tipo 2.

                                        txtSQL = " Insert into InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) " +
                                                    " values (" + Valor + "," + row_Rs["Mes"].ToString() + ", " + row_Rs["Valor"].ToString() + ", 2) ";

                                        #endregion
                                    }

                                    //Ejecutar consulta.
                                    cmd = new SqlCommand(txtSQL, conn);
                                    correcto = String_EjecutarSQL(conn, cmd);

                                    if (correcto != "")
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al insertar en la tabla definitiva por mes: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                        return;
                                    }
                                }

                                #endregion

                                #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.

                                txtSQL = " DELETE FROM InterventorProduccionMesTMP " +
                                            " where CodProducto = " + codProduccion;

                                //Ejecutar consulta.
                                cmd = new SqlCommand(txtSQL, conn);
                                correcto = String_EjecutarSQL(conn, cmd);

                                if (correcto != "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar registros de la tabla temporal por meses: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                    return;
                                }

                                #endregion

                            }
                            #endregion

                            #region  Aprobacion de tarea Coordinador interventor
                            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                            {
                                txtSQL = "UPDATE InterventorProduccionTMP SET ChequeoCoordinador = 1 where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;
                                cmd = new SqlCommand(txtSQL, conn);
                                correcto = String_EjecutarSQL(conn, cmd);

                                if (correcto != "")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al aprobar la creación del producto);", true);
                                    return;
                                }

                                #region Generar tarea pendiente para el Gerente Interventor
                                //txtSQL = "select Id_grupo from Grupo  where NomGrupo = 'Gerente Interventor'";

                                txtSQL = " SELECT NomProducto FROM InterventorProduccionTMP " +
                                        " WHERE CodProyecto = " + CodProyecto + " and id_Produccion= " + codProduccion;
                                Rs = consultas.ObtenerDataTable(txtSQL, "text");

                                if (Rs.Rows.Count > 0) { NomActividad = Rs.Rows[0]["NomProducto"].ToString(); }


                                // Esta consulta trae el Gerente interventor del proyecto 

                                var grupo = (from g in consultas.Db.Grupos
                                             where g.NomGrupo == "Gerente Interventor"
                                             select g).FirstOrDefault();
                                var contacto = (from c in consultas.Db.GrupoContactos
                                                where c.CodGrupo == grupo.Id_Grupo
                                                select c).FirstOrDefault();

                                //Agendar tarea.
                                AgendarTarea agenda = new AgendarTarea
                                (contacto.CodContacto,
                                "Producto en producción Aprobado por Coordinador Interventor",
                                "Revisar productos en producción " + txtNomProyecto + " - Producto --> " + NomActividad + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                                CodProyecto,
                                17,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + CodProyecto,
                                "",
                                "Catálogo Producción");

                                agenda.Agendar();
                                #endregion
                            }
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto creado correctamente.');window.opener.location = window.opener.location;window.close();", true);
                            #endregion
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto creado');window.opener.location = window.opener.location;window.close();", true);
                        }
                        if (dd_inv_aprobar.SelectedValue == "0") //No
                        {
                            #region No Aprobado.

                            #region Se devuelve al interventor, se le avisa al coordinador.

                            txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                        " FROM EmpresaInterventor " +
                                        " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                        " WHERE EmpresaInterventor.Inactivo = 0 " +
                                        " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                        " AND Empresa.codproyecto = " + CodProyecto;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion

                            #region Eliminación #1.
                            var productoTmp = (from n in consultas.Db.InterventorProduccionTMPs
                                               where n.CodProyecto == int.Parse(CodProyecto) && n.id_produccion == int.Parse(codProduccion)
                                               select n).FirstOrDefault();

                            txtSQL = " DELETE FROM InterventorProduccionTMP where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;
                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            #endregion

                            #region Eliminación #2.

                            txtSQL = " DELETE FROM InterventorProduccionMesTMP where CodProducto = " + codProduccion;
                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            #endregion


                            #region Generar tarea pendiente.
                            //Agendar tarea.
                            AgendarTarea agenda = new AgendarTarea
                            (Int32.Parse(RsActividad.Rows[0]["CodContacto"].ToString()),
                            "Producto en producción Rechazado por Coordinador Interventor",
                            "Revisar productos en producción " + txtNomProyecto + " - Producto --> " + productoTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                            CodProyecto,
                            2,
                            "0",
                            true,
                            1,
                            true,
                            false,
                            usuario.IdContacto,
                            "CodProyecto=" + CodProyecto,
                            "",
                            "Catálogo Productos");
                            agenda.Agendar();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);

                            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                            {

                            }
                            #endregion
                            #endregion
                        }

                    }

                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")
                        {

                            var productoTMP = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();

                            int cantRegProduccion = validarActividades.validarProduccion(productoTMP.CodProyecto, productoTMP.NomProducto);

                            if (cantRegProduccion > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una actividad de Produccion registrada con la misma informacion.');window.close();", true);
                            }
                            else
                            {

                                var producto = new Datos.InterventorProduccion
                                {
                                    CodProyecto = productoTMP.CodProyecto,
                                    NomProducto = productoTMP.NomProducto
                                };

                                consultas.Db.InterventorProduccions.InsertOnSubmit(producto);
                                consultas.Db.SubmitChanges();

                                txtSQL = "var producto = new Datos.InterventorProduccion" +
                                "{" +
                                    "CodProyecto = productoTMP.CodProyecto," +
                                    "NomProducto = productoTMP.NomProducto" +
                                "};" +
                                "consultas.Db.InterventorProduccions.InsertOnSubmit(producto);" +
                                "consultas.Db.SubmitChanges();";
                                LogActivitdades.WriteError(3, txtSQL); //3 -> Produccion

                                txtSQL = "DELETE FROM InterventorProduccionTMP where CodProyecto=" + productoTMP.CodProyecto + " and id_Produccion=" + productoTMP.id_produccion;
                                ejecutaReader(txtSQL, 2);

                                var productoMesTmp = (from pmt in consultas.Db.InterventorProduccionMesTMPs
                                                      where pmt.CodProducto == productoTMP.id_produccion
                                                      select pmt).ToList();

                                foreach (var fila in productoMesTmp)
                                {
                                    var val = fila.Valor.ToString().Split(',');
                                    if (fila.Tipo == 1)
                                    {
                                        txtSQL = "Insert into InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) ";
                                        txtSQL += "Values (" + producto.id_produccion + "," + fila.Mes + "," + val[0] + ",1)";
                                    }
                                    else
                                    {
                                        txtSQL = "Insert into InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) ";
                                        txtSQL += "Values (" + producto.id_produccion + "," + fila.Mes + "," + val[0] + ",2)";
                                    }

                                    ejecutaReader(txtSQL, 2);
                                }

                                txtSQL = "DELETE FROM InterventorProduccionMesTMP where CodProducto=" + productoTMP.id_produccion;
                                ejecutaReader(txtSQL, 2);

                                //Agendar Tarea
                                // Seleccional id del Interventor
                                var datos = (from ei in consultas.Db.EmpresaInterventors
                                             join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                             join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                             where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                             select new datosAgendar
                                             {
                                                 idContacto = (int)ei.CodContacto,
                                                 idProyecto = (int)p.Id_Proyecto,
                                                 nombre = p.NomProyecto
                                             }).ToList();

                                // Seleccional id del Emprendedor
                                var datoE = (from pc in consultas.Db.ProyectoContactos
                                             join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                             where pc.CodProyecto == int.Parse(Session["CodProyecto"].ToString()) && pc.CodRol == 3
                                             select new datosAgendar
                                             {
                                                 idContacto = pc.CodContacto,
                                                 idProyecto = p.Id_Proyecto,
                                                 nombre = p.NomProyecto
                                             }).ToList();

                                int[] idsAgendar = new int[2];
                                idsAgendar[0] = Convert.ToInt32(datos[0].idContacto);
                                idsAgendar[1] = Convert.ToInt32(datoE[0].idContacto);

                                foreach (var id in idsAgendar)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                    (id,
                                    "Producto en producción Aprobado por Gerente Interventor",
                                    "Revisión Adición, Modificación o Borrado de Actividad del interventor al Plan Operativo " + txtNomProyecto,
                                    CodProyecto,
                                    2,
                                    "0",
                                    true,
                                    1,
                                    true,
                                    false,
                                    usuario.IdContacto,
                                    "Accion=Editar&CodProyecto=" + productoTMP.CodProyecto + "&CodProducto=" + productoTMP.id_produccion,
                                    "",
                                    "Catálogo Productos");

                                    agenda.Agendar();
                                }
                            }

                        }
                        else
                        {
                            //Se elimina el producto, se le avisa al coordinador
                            txtSQL = " SELECT EmpresaInterventor.CodContacto FROM EmpresaInterventor ";
                            txtSQL += " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa ";
                            txtSQL += " WHERE EmpresaInterventor.Inactivo = 0 AND EmpresaInterventor.Rol = 8";
                            txtSQL += " AND Empresa.codproyecto = " + CodProyecto;

                            var idCoordinador = consultas.ObtenerDataTable(txtSQL, "text").Rows[0].ItemArray[0].ToString();


                            //Se elemina el producto
                            var productoTmp = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();
                            if (productoTmp != null)
                            {
                                txtSQL = "DELETE FROM InterventorProduccionTMP where CodProyecto=" + productoTmp.CodProyecto + " and id_Produccion=" + productoTmp.id_produccion;
                                ejecutaReader(txtSQL, 2);

                                txtSQL = "DELETE FROM InterventorProduccionMesTMP where CodProducto=" + productoTmp.id_produccion;
                                ejecutaReader(txtSQL, 2);

                                //Agendar Tarea
                                // Seleccional id del Interventor
                                var datos = (from ei in consultas.Db.EmpresaInterventors
                                             join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                             join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                             where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                             select new datosAgendar
                                             {
                                                 idContacto = (int)ei.CodContacto,
                                                 idProyecto = (int)p.Id_Proyecto,
                                                 nombre = p.NomProyecto
                                             }).ToList();

                                // Seleccional id del Emprendedor
                                var datoE = (from pc in consultas.Db.ProyectoContactos
                                             join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                             where pc.CodProyecto == int.Parse(Session["CodProyecto"].ToString()) && pc.CodRol == 3
                                             select new datosAgendar
                                             {
                                                 idContacto = pc.CodContacto,
                                                 idProyecto = p.Id_Proyecto,
                                                 nombre = p.NomProyecto
                                             }).ToList();

                                int[] idsAgendar = new int[2];
                                idsAgendar[0] = Convert.ToInt32(datos[0].idContacto);
                                idsAgendar[1] = Convert.ToInt32(datoE[0].idContacto);

                                foreach (var id in idsAgendar)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                    (id,
                                    "Producto en producción Rechazado por Gerente Interventor",
                                    "Revisión Adición, Modificación o Borrado de Actividad del interventor al Plan Operativo " + txtNomProyecto,
                                    CodProyecto,
                                    2,
                                    "0",
                                    true,
                                    1,
                                    true,
                                    false,
                                    usuario.IdContacto,
                                    "Accion=Editar&CodProyecto=" + productoTmp.CodProyecto + "&CodProducto=" + productoTmp.id_produccion,
                                    "",
                                    "Catálogo Productos");

                                    agenda.Agendar();
                                }


                            }
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                    }
                    #endregion
                }
                if (acion == 2)
                {
                    #region Editar la información previamente seleccionada.

                    //Comprobar si el usuario tiene el código grupo de "Coordinador Interventor.
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")
                        {
                            txtSQL = " UPDATE InterventorProduccionTMP SET ChequeoCoordinador = 1 " +
                                         " where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;
                            ejecutaReader(txtSQL, 2);

                            var grupo = (from g in consultas.Db.Grupos
                                         where g.NomGrupo == "Gerente Interventor"
                                         select g).FirstOrDefault();
                            var gerente = (from gc in consultas.Db.GrupoContactos
                                           where gc.CodGrupo == grupo.Id_Grupo
                                           select gc).FirstOrDefault();

                            //Agendar tarea.
                            AgendarTarea agenda = new AgendarTarea
                            (gerente.CodContacto,
                            "Revisión tarea modificación aprobada por Coordinador Interventor",
                            "Revisión Adición, Modificación o Borrado de Actividad del interventor al Plan Operativo " + txtNomProyecto,
                            CodProyecto,
                            17,
                            "0",
                            true,
                            1,
                            true,
                            false,
                            usuario.IdContacto,
                            "Accion=Editar&CodProyecto=" + CodProyecto + "&CodProducto=" + codProduccion,
                            "",
                            "Catálogo Productos");

                            agenda.Agendar();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificacion deProducto aprobado!');window.opener.location = window.opener.location;window.close();", true);

                        }
                        else
                        {
                            //El coord de Inerventria no aprueba modificacion
                            var productoTmp = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();

                            txtSQL = " SELECT EmpresaInterventor.CodContacto  FROM EmpresaInterventor ";
                            txtSQL += " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa ";
                            txtSQL += " WHERE EmpresaInterventor.Inactivo = 0 AND Empresa.codproyecto = " + CodProyecto;

                            var dt = consultas.ObtenerDataTable(txtSQL, "text");

                            txtSQL = "DELETE FROM InterventorProduccionTMP where CodProyecto=" + productoTmp.CodProyecto + " and id_Produccion=" + productoTmp.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "DELETE FROM InterventorProduccionMesTMP where CodProducto=" + productoTmp.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea.
                            AgendarTarea agenda = new AgendarTarea
                           (int.Parse(dt.Rows[0].ItemArray[0].ToString()),
                           "Revisión tarea modificación no aprobada por Coordinador Interventor",
                           "Revisión Adición, Modificación o Borrado de Actividad del interventor al Plan Operativo " + txtNomProyecto,
                           CodProyecto,
                           17,
                           "0",
                           true,
                           1,
                           true,
                           false,
                           usuario.IdContacto,
                           "Accion=Editar&CodProyecto=" + CodProyecto + "&CodProducto=" + codProduccion,
                           "",
                           "Catálogo Productos");
                            agenda.Agendar();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Modificacion de Producto negada!');window.opener.location = window.opener.location;window.close();", true);
                        }
                    }

                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")
                        {
                            // El gerente aprueba la modificacion
                            var productoTmp = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();
                            var producto = (from pt in consultas.Db.InterventorProduccions
                                            where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                            select pt).FirstOrDefault();

                            producto.NomProducto = productoTmp.NomProducto;
                            consultas.Db.SubmitChanges();

                            txtSQL = " DELETE FROM InterventorProduccionTMP where CodProyecto = " + productoTmp.CodProyecto + " and id_Produccion = " + productoTmp.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            var productoMesTmp = (from ptm in consultas.Db.InterventorProduccionMesTMPs
                                                  where ptm.CodProducto == productoTmp.id_produccion
                                                  select ptm).ToList();

                            txtSQL = " DELETE FROM InterventorProduccionMes where CodProducto = " + producto.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            foreach (var fila in productoMesTmp)
                            {
                                var val = fila.Valor.ToString().Split(',');
                                if (fila.Tipo == 1)
                                {
                                    txtSQL = " Insert into InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) ";
                                    txtSQL += " values (" + producto.id_produccion + "," + fila.Mes + ", " + val[0] + " , 1) ";
                                }
                                else
                                {
                                    txtSQL = " Insert into InterventorProduccionMes (CodProducto,Mes,Valor,Tipo) ";
                                    txtSQL += " values (" + producto.id_produccion + "," + fila.Mes + ", " + val[0] + " , 2) ";
                                }

                                ejecutaReader(txtSQL, 2);

                            }

                            txtSQL = " DELETE InterventorProduccionMesTMP where CodProducto = " + productoTmp.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea
                            // Seleccional id del Interventor
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            // Seleccional id del Emprendedor
                            var datoE = (from pc in consultas.Db.ProyectoContactos
                                         join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                         where pc.CodProyecto == int.Parse(Session["CodProyecto"].ToString()) && pc.CodRol == 3
                                         select new datosAgendar
                                         {
                                             idContacto = pc.CodContacto,
                                             idProyecto = p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            int[] idsAgendar = new int[2];
                            idsAgendar[0] = Convert.ToInt32(datos[0].idContacto);
                            idsAgendar[1] = Convert.ToInt32(datoE[0].idContacto);
                            //Agendar tarea.
                            foreach (var id in idsAgendar)
                            {
                                AgendarTarea agenda = new AgendarTarea
                                (id,
                                "Revisar Productos en Producción. Se ha Aprobado una modificación .",
                                "Revisar Productos en Producción " + datos[0].nombre + " - Producto --> " + productoTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                                datos[0].idProyecto.ToString(),
                                2,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + datos[0].idProyecto,
                                "",
                                "Catálogo Producción");

                                agenda.Agendar();
                            }

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();;window.close();", true);
                        }
                        else
                        {
                            // El gerente no aprueba las modificaciones
                            var productoTmp = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();

                            txtSQL = " DELETE FROM InterventorProduccionTMP where CodProyecto = " + productoTmp.CodProyecto + " and id_Produccion = " + productoTmp.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = " DELETE InterventorProduccionMesTMP where CodProducto = " + productoTmp.id_produccion;
                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea
                            // Seleccional id del Interventor
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            // Seleccional id del Emprendedor
                            var datoE = (from pc in consultas.Db.ProyectoContactos
                                         join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                         where pc.CodProyecto == int.Parse(Session["CodProyecto"].ToString()) && pc.CodRol == 3
                                         select new datosAgendar
                                         {
                                             idContacto = pc.CodContacto,
                                             idProyecto = p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            int[] idsAgendar = new int[2];
                            idsAgendar[0] = Convert.ToInt32(datos[0].idContacto);
                            idsAgendar[1] = Convert.ToInt32(datoE[0].idContacto);
                            //Agendar tarea.
                            foreach (var id in idsAgendar)
                            {
                                AgendarTarea agenda = new AgendarTarea
                                (id,
                                "Revisar Productos en Producción. Se ha Negado una modificación .",
                                "Revisar Productos en Producción " + datos[0].nombre + " - Producto --> " + productoTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                                datos[0].idProyecto.ToString(),
                                2,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + datos[0].idProyecto,
                                "",
                                "Catálogo Producción");

                                agenda.Agendar();
                            }

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();;window.close();", true);


                        }
                    }

                    if (usuario.CodGrupo == Constantes.CONST_Interventor) //Si el usuario tiene el código grupo "Interventor".
                    {
                        #region Si es Interventor.
                        string txtSQL = "select CodCoordinador from interventor where codcontacto=" + usuario.IdContacto;
                        cmd = new SqlCommand(txtSQL, conn);
                        correcto = String_EjecutarSQL(conn, cmd);
                        if (correcto != "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                            return;
                        }
                        string NomProducto;
                        if (pagina.Equals("Produccion"))
                            txtSQL = "select NomProducto from InterventorProduccion where CodProyecto=" + CodProyecto + " and id_Produccion=" + codProduccion;
                        if (pagina.Equals("Ventas"))
                            txtSQL = "select NomProducto from InterventorVentas where CodProyecto=" + CodProyecto + " and id_ventas=" + codProduccion;
                        if (pagina.Equals("Nomina"))
                            txtSQL = "select Cargo as NomProducto from InterventorNomina where CodProyecto=" + CodProyecto + " and Id_Nomina=" + codProduccion;

                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                        if (Rs.Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No pudo ser consultado el nombre del producto');window.close();", true);
                            return;
                        }
                        NomProducto = Rs.Rows[0]["NomProducto"].ToString();


                        if (pagina.Equals("Produccion"))
                            txtSQL = "Insert into InterventorProduccionTMP (id_Produccion,CodProyecto,NomProducto,Tarea) values (" + codProduccion + "," + CodProyecto + ",'" + NomProducto + "','Modificar')";
                        if (pagina.Equals("Ventas"))
                            txtSQL = "Insert into InterventorVentasTMP (id_Ventas,CodProyecto,NomProducto,Tarea) values (" + codProduccion + "," + CodProyecto + ",'" + NomProducto + "','Modificar')";
                        if (pagina.Equals("Nomina"))
                            txtSQL = "Insert into InterventorNominaTMP (Id_Nomina,CodProyecto,cargo,Tipo,Tarea) values (" + codProduccion + "," + CodProyecto + ",'" + NomProducto + "','" + HttpContext.Current.Session["v_Tipo"].ToString() + "','Modificar')";

                        ejecutaReader(txtSQL, 2);

                        string mes = "0";
                        string valor = "0";
                        string tipo = "0";
                        #region for2

                        int CantidadDatos = 0;
                        txtSQL = "SELECT count(*) as Cantidad FROM InterventorProduccionMesTMP WHERE CodProducto = " + codProduccion;
                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                        if (Convert.ToInt32(Rs.Rows[0]["Cantidad"]) >= 1)
                        {
                            CantidadDatos = 1;
                        }

                        foreach (Control miControl in T_Meses.Controls)
                        {
                            var tablerow = miControl.Controls;

                            foreach (Control micontrolrows in tablerow)
                            {
                                var hijos = micontrolrows.Controls;

                                foreach (Control chijos in hijos)
                                {
                                    if (chijos.GetType() == typeof(TextBox))
                                    {
                                        var text = chijos as TextBox;

                                        if (text.ID.StartsWith("Fondoo")) //Sueldo
                                        {
                                            tipo = "1";
                                        }
                                        else
                                        {
                                            if (text.ID.StartsWith("Aporte")) //Sueldo
                                            {
                                                tipo = "2";
                                            }
                                        }
                                        if (string.IsNullOrEmpty(text.Text))
                                            valor = "0";
                                        else
                                            valor = text.Text;

                                        int limit = 0;
                                        if (text.ID.Length == 7)
                                            limit = 1;
                                        else
                                            limit = 2;

                                        mes = text.ID.Substring(6, limit);

                                        if (CantidadDatos == 0)
                                        {

                                            if (pagina.Equals("Produccion"))
                                                txtSQL = "INSERT INTO InterventorProduccionMesTMP(CodProducto,Mes,Valor,Tipo) " +
                                                "VALUES(" + codProduccion + "," + mes + "," + valor + "," + tipo + ")";
                                            if (pagina.Equals("Ventas"))
                                                txtSQL = "INSERT INTO InterventorVentasMesTMP(CodProducto,Mes,Valor,Tipo) " +
                                                "VALUES(" + codProduccion + "," + mes + "," + valor + "," + tipo + ")";
                                            if (pagina.Equals("Nomina"))
                                                txtSQL = "INSERT INTO InterventorNominaMesTMP(CodCargo,Mes,Valor,Tipo) " +
                                                "VALUES(" + codProduccion + "," + mes + "," + valor + "," + tipo + ")";
                                        }
                                        else
                                        {
                                            // Se actualiza el valor del dato
                                            if (pagina.Equals("Produccion"))
                                                txtSQL = "UPDATE InterventorProduccionMesTMP SET Valor = " + valor +
                                                " WHERE CodProducto = " + codProduccion + " AND MES = " + mes + " AND TIPO = " + tipo;
                                            if (pagina.Equals("Ventas"))
                                                txtSQL = "UPDATE InterventorVentasMesTMP SET Valor = " + valor +
                                                " WHERE CodProducto = " + codProduccion + " AND MES = " + mes + " AND TIPO = " + tipo;
                                            if (pagina.Equals("Nomina"))
                                                txtSQL = "UPDATE InterventorNominaMesTMP SET Valor = " + valor +
                                                " WHERE CodProducto = " + codProduccion + " AND MES = " + mes + " AND TIPO = " + tipo;
                                        }
                                        ejecutaReader(txtSQL, 2);
                                    }
                                }
                            }
                        }
                        #endregion

                        //Agendar tarea.
                        txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                        var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                        var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());

                        AgendarTarea agenda = new AgendarTarea
                        (idCoord,
                        "Modificacion a Producto creada por Interventor.",
                        "Revisar Productos en Producción " + txtNomProyecto + " - Actividad --> " + TB_Item.Text + "<br>Observaciones:</br>" + txt_inv_observaciones.Text.Trim(),
                        CodProyecto,
                        2,
                        "0",
                        true,
                        1,
                        true,
                        false,
                        usuario.IdContacto,
                        "CodProyecto=" + CodProyecto,
                        "",
                        "Catálogo Actividad Plan Operativo");

                        agenda.Agendar();

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto modificado');window.opener.location = window.opener.location;window.close();", true);
                        #endregion
                    }
                    #endregion
                }
                if (acion == 3) //Eliminar
                {
                    #region Si es Gerente Interventor.
                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        #region Aprobado.
                        if (dd_inv_aprobar.SelectedValue == "1") //Si
                        {

                            // Se verifica que el coordinador ya aprobo el Borrado
                            txtSQL = " select ChequeoCoordinador from InterventorProduccionTMP " +
                                            " where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");
                            if (Convert.ToInt32(RsActividad.Rows[0]["ChequeoCoordinador"]) == 1)
                            {

                                #region BORRA LOS REGISTROS EN LA TABLA DEFINITIVA.

                                var productoBorrar = (from p in consultas.Db.InterventorProduccions
                                                      where p.id_produccion == int.Parse(codProduccion)
                                                      select p).FirstOrDefault();

                                txtSQL = " DELETE FROM InterventorProduccion " +
                                         " where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;

                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.

                                txtSQL = " DELETE FROM InterventorProduccionTMP " +
                                         " where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;

                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region BORRA LOS REGISTROS EN LA TABLA DEFINITIVA POR MESES.

                                txtSQL = " DELETE FROM InterventorProduccionMes " +
                                         " where CodProducto = " + codProduccion;

                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.

                                txtSQL = " DELETE FROM InterventorProduccionMesTMP " +
                                         " where CodProducto = " + codProduccion;

                                ejecutaReader(txtSQL, 2);

                                #endregion

                                //Agendar tarea
                                // Seleccional id del Interventor
                                var datos = (from ei in consultas.Db.EmpresaInterventors
                                             join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                             join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                             where ee.codproyecto == int.Parse(CodProyecto) && ei.Inactivo == false
                                             select new datosAgendar
                                             {
                                                 idContacto = (int)ei.CodContacto,
                                                 idProyecto = (int)p.Id_Proyecto,
                                                 nombre = p.NomProyecto
                                             }).ToList();

                                // Seleccional id del Emprendedor
                                var datoE = (from pc in consultas.Db.ProyectoContactos
                                             join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                             where pc.CodProyecto == int.Parse(CodProyecto) && pc.CodRol == 3
                                             select new datosAgendar
                                             {
                                                 idContacto = pc.CodContacto,
                                                 idProyecto = p.Id_Proyecto,
                                                 nombre = p.NomProyecto
                                             }).ToList();

                                int[] idsAgendar = new int[2];
                                idsAgendar[0] = Convert.ToInt32(datos[0].idContacto);
                                idsAgendar[1] = Convert.ToInt32(datoE[0].idContacto);
                                //Agendar tarea.
                                foreach (var id in idsAgendar)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                    (id,
                                    "Revisar Productos en Producción. Se ha Eliminado un Producto.",
                                    "Revisar Productos en Producción " + datos[0].nombre + " - Producto --> " + productoBorrar.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                                    datos[0].idProyecto.ToString(),
                                    2,
                                    "0",
                                    true,
                                    1,
                                    true,
                                    false,
                                    usuario.IdContacto,
                                    "CodProyecto=" + datos[0].idProyecto,
                                    "",
                                    "Catálogo Producción");

                                    agenda.Agendar();
                                }

                                //Destruir variables.
                                CodProyecto = "0";
                                codProduccion = "0";
                                ValorTMP = "";
                                v_Tipo = "";
                                HttpContext.Current.Session["CodProduccion"] = null;
                                HttpContext.Current.Session["CodNomina"] = null;
                                HttpContext.Current.Session["v_Tipo"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se ha eliminado el producto con éxito.');window.opener.location=window.opener.location;window.close();", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La tarea Borrar para el producto no ha sido aprobada por el Coordinador Interventor.');window.opener.location=window.opener.location;window.close();", true);
                            }

                        }
                        else
                        {
                            var productoBorrar = (from p in consultas.Db.InterventorProduccionTMPs
                                                  where p.CodProyecto == int.Parse(CodProyecto) && p.id_produccion == int.Parse(codProduccion)
                                                  select p).FirstOrDefault();

                            txtSQL = "Delete from InterventorProduccionTMP where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "Delete from InterventorProduccionMesTMP where CodProducto = " + codProduccion;
                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea
                            // Seleccional id del Interventor
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == int.Parse(CodProyecto) && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            // Seleccional id del Emprendedor
                            var datoE = (from pc in consultas.Db.ProyectoContactos
                                         join p in consultas.Db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                                         where pc.CodProyecto == int.Parse(CodProyecto) && pc.CodRol == 3
                                         select new datosAgendar
                                         {
                                             idContacto = pc.CodContacto,
                                             idProyecto = p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();

                            int[] idsAgendar = new int[2];
                            idsAgendar[0] = Convert.ToInt32(datos[0].idContacto);
                            idsAgendar[1] = Convert.ToInt32(datoE[0].idContacto);
                            //Agendar tarea.
                            foreach (var id in idsAgendar)
                            {
                                AgendarTarea agenda = new AgendarTarea
                                (id,
                                "Revisar Productos en Producción. Producto Rechazado para eliminación.",
                                "Revisar Productos en Producción " + datos[0].nombre + " - Producto --> " + productoBorrar.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
                                datos[0].idProyecto.ToString(),
                                2,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + datos[0].idProyecto,
                                "",
                                "Catálogo Producción");

                                agenda.Agendar();
                            }

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto NO aprobado para Borrar.');window.opener.location=window.opener.location;window.close();", true);
                        }
                        #endregion

                        #region No Aprobado.
                        #endregion

                    }
                    #endregion

                    #region Aprobacion de tarea Borrar si es Coordinador Interventor
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")
                        {
                            txtSQL = "UPDATE InterventorProduccionTMP SET ChequeoCoordinador = 1 where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;

                            ejecutaReader(txtSQL, 2);

                            #region Generar tarea pendiente para el Gerente Interventor
                            txtSQL = "select Id_grupo from Grupo  where NomGrupo = 'Gerente Interventor'";


                            txtSQL = "select CodContacto from GrupoContacto where CodGrupo in(select Id_grupo from Grupo  where NomGrupo = 'Gerente Interventor') ";
                            //cmd = new SqlCommand(txtSQL, conn);
                            Rs = consultas.ObtenerDataTable(txtSQL, "text");
                            var productoTmp = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();

                            if (productoTmp != null) { NomActividad = productoTmp.NomProducto; }

                            // Esta consulta trae el Gerente interventor del proyecto 
                            var grupo = (from g in consultas.Db.Grupos
                                         where g.NomGrupo == "Gerente Interventor"
                                         select g).FirstOrDefault();
                            var contacto = (from c in consultas.Db.GrupoContactos
                                            where c.CodGrupo == grupo.Id_Grupo
                                            select c).FirstOrDefault();

                            if (contacto != null)
                            {
                                //Agendar tarea.
                                AgendarTarea agenda = new AgendarTarea
                                (contacto.CodContacto,
                                "Producto en producción Aprobado para Borrar por Coordinador Interventor",
                                "Revisar productos en producción " + txtNomProyecto + " - Actividad --> " + NomActividad + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
                                CodProyecto,
                                17,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + CodProyecto,
                                "",
                                "Catálogo Producción");

                                agenda.Agendar();
                            }
                            #endregion
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto aprobado para Borrar.');window.opener.location=window.opener.location;window.close();", true);
                        }
                        else
                        {
                            var productoTmp = (from pt in consultas.Db.InterventorProduccionTMPs
                                               where pt.CodProyecto == int.Parse(CodProyecto) && pt.id_produccion == int.Parse(codProduccion)
                                               select pt).FirstOrDefault();

                            if (productoTmp != null) { NomActividad = productoTmp.NomProducto; }

                            txtSQL = "Delete from InterventorProduccionTMP where CodProyecto = " + CodProyecto + " and id_Produccion = " + codProduccion;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "Delete from InterventorProduccionMesTMP where CodProducto = " + codProduccion;
                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea Interventor
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();
                            AgendarTarea agenda = new AgendarTarea
                            (datos[0].idContacto,
                            "Producto en producción Rechazado para Borrar por Coordinador Interventor",
                            "Revisar productos en producción " + txtNomProyecto + " - Actividad --> " + NomActividad + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
                            CodProyecto,
                            17,
                            "0",
                            true,
                            1,
                            true,
                            false,
                            usuario.IdContacto,
                            "",
                            "",
                            "Catálogo Producción");

                            agenda.Agendar();

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto NO aprobado para Borrar.');window.opener.location=window.opener.location;window.close();", true);

                        }

                    }
                    #endregion
                    //Destruir variables.
                    CodProyecto = "0";
                    codProduccion = "0";
                    ValorTMP = "";
                    v_Tipo = "";
                    HttpContext.Current.Session["CodProduccion"] = null;
                    HttpContext.Current.Session["CodNomina"] = null;
                    HttpContext.Current.Session["v_Tipo"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                }
            }
        }

        private void sumar(TextBox textbox)
        {
            //Inicializar variables.
            String textboxID = textbox.ID;
            TextBox textFondo;
            TextBox textAporte;
            Label controltext;

            //Se movieron las variables del try para la suma.
            Double suma1 = 0;
            Double suma2 = 0; //Según el FONADE clásico, el valor COSTO lo toma como ENTERO al SUMARLO.
                              // Int32 valor_suma2 = 0;

            var labelfondocosto = this.FindControl("labelfondocosto") as Label;
            var labelaportescosto = this.FindControl("labelaportescosto") as Label;
            var L_SumaTotalescosto = this.FindControl("L_SumaTotalescosto") as Label;

            //Details
            int limit = 0;
            if (textboxID.Length == 7)
                limit = 1;
            else
                limit = 2;

            //String objeto = "TotalMes" + (textboxID[textboxID.Length - 1]);
            String objeto = "TotalMes" + textboxID.Substring(6, limit);


            controltext = (Label)this.FindControl(objeto);
            textFondo = (TextBox)this.FindControl("Fondoo" + textboxID.Substring(6, limit)); //Sueldo
            textAporte = (TextBox)this.FindControl("Aporte" + textboxID.Substring(6, limit)); //Prestaciones

            try
            {
                if (String.IsNullOrEmpty(textFondo.Text))
                {
                    suma1 = 0;
                    textFondo.Text = suma1.ToString();
                }
                else
                {
                    suma1 = Double.Parse(textFondo.Text);
                    textFondo.Text = suma1.ToString();
                }

                if (String.IsNullOrEmpty(textAporte.Text))
                {
                    suma2 = 0;
                    textAporte.Text = suma2.ToString();
                }
                else
                {
                    suma2 = Double.Parse(textAporte.Text);
                    //valor_suma2 = Convert.ToInt32(suma2);
                    textAporte.Text = suma2.ToString();
                }

                //Con formato
                //controltext.Text = "$" + (suma1 + suma2).ToString("0,0.00", CultureInfo.InvariantCulture);
                //controltext.Text = String.Format("{0:c}", (suma1 + suma2));
                controltext.Text = "" + (suma1 + suma2);
                labelfondocosto.Text = "0";
                labelaportescosto.Text = "0";


                foreach (Control miControl in T_Meses.Controls)
                {
                    var tablerow = miControl.Controls;

                    foreach (Control micontrolrows in tablerow)
                    {
                        var hijos = micontrolrows.Controls;

                        foreach (Control chijos in hijos)
                        {
                            if (chijos.GetType() == typeof(TextBox))
                            {
                                var text = chijos as TextBox;

                                if (text.ID.StartsWith("Fondoo")) //Sueldo
                                {
                                    if (labelfondocosto != null)
                                    {
                                        if (string.IsNullOrEmpty(text.Text.Trim()))
                                        {
                                            text.Text = "0";
                                        }
                                        labelfondocosto.Text = (Convert.ToDouble(labelfondocosto.Text) + Convert.ToDouble(text.Text)).ToString();
                                    }
                                    //if (L_SumaTotalescosto != null)
                                    //    L_SumaTotalescosto.Text = (Convert.ToDouble(labelfondocosto.Text)).ToString();
                                }
                                else
                                {
                                    if (text.ID.StartsWith("Aporte")) //Sueldo
                                    {
                                        if (labelaportescosto != null)
                                        {
                                            if (string.IsNullOrEmpty(text.Text.Trim()))
                                            {
                                                text.Text = "0";
                                            }
                                            labelaportescosto.Text = (Convert.ToDouble(labelaportescosto.Text) + Convert.ToDouble(text.Text)).ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (L_SumaTotalescosto != null)
                {
                    L_SumaTotalescosto.Text = String.Format("{0:c}", (Convert.ToDouble(labelaportescosto.Text)) + (Convert.ToDouble(labelfondocosto.Text)));
                }
            }
            catch (FormatException) { }
            catch (NullReferenceException) { }
        }

        /// <summary>
        /// Textbox Changed para Fondo/Sueldo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            sumar(textbox);
        }

        /// <summary>
        /// Cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            //Destruir variables.
            CodProyecto = "0";
            codProduccion = "0";
            ValorTMP = "";
            v_Tipo = "";
            HttpContext.Current.Session["CodProduccion"] = null;
            HttpContext.Current.Session["CodNomina"] = null;
            HttpContext.Current.Session["v_Tipo"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 09/04/2014.
        /// Modificar la información del valor seleccionado en la grilla de "Producción".
        /// </summary>
        /// <param name="VieneDeCambiosPO">TRUE = Viene de "CambiosPO.aspx". // FALSE = Viene de "FrameProduccionInter.aspx".</param>
        private void BuscarDatos_Produccion(bool VieneDeCambiosPO)
        {
            //Obtiene la conexión
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            //Inicializa la variable para generar la consulta.
            String sqlConsulta = "";
            if (VieneDeCambiosPO == true)
            {
                #region Cargar la información de "CambiosPO.aspx".

                String ChequeoCoordinador;
                String ChequeoGerente;

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Los campos del formulario se bloquean.
                    TB_Item.Enabled = false;
                    P_Meses.Enabled = false;
                    #endregion
                    sqlConsulta = "SELECT * FROM InterventorProduccionTMP where id_Produccion = " + codProduccion;
                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    sqlConsulta = "SELECT * FROM InterventorProduccion where id_Produccion =" + codProduccion;
                }


                SqlCommand cmd = new SqlCommand(sqlConsulta, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        try
                        {
                            HttpContext.Current.Session["Tarea"] = reader["NomProducto"].ToString();
                            ChequeoCoordinador = reader["ChequeoCoordinador"].ToString();
                            ChequeoGerente = reader["ChequeoGerente"].ToString();
                            #region Aprobación de cambios del coordinador.
                            if (!String.IsNullOrEmpty(ChequeoCoordinador))
                            {
                                //if (ChequeoCoordinador == "True" || ChequeoCoordinador == "1")
                                if (ChequeoCoordinador == "1")
                                {
                                    dd_inv_aprobar.SelectedValue = "1";
                                }
                            }
                            else
                            { dd_inv_aprobar.SelectedValue = "0"; }
                            #endregion

                            #region Aprobación de cambios del gerente.
                            if (!String.IsNullOrEmpty(ChequeoGerente))
                            {
                                if (ChequeoGerente == "1")
                                {
                                    dd_inv_aprobar.SelectedValue = "1";
                                }
                            }
                            else
                            { dd_inv_aprobar.SelectedValue = "0"; }
                            #endregion

                            TB_Item.Text = reader["NomProducto"].ToString();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        //Destruir variables.
                        CodProyecto = "0";
                        codProduccion = "0";
                        ValorTMP = "";
                        v_Tipo = "";
                        HttpContext.Current.Session["CodProduccion"] = null;
                        HttpContext.Current.Session["CodNomina"] = null;
                        HttpContext.Current.Session["v_Tipo"] = null;
                        //Tarea ya aprobada (tendría que salir un botón para cerrar la ventana actual...)...
                        reader.Dispose();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');window.opener.location=window.opener.location;window.close();", true);
                    }
                }
                catch (SqlException se)
                {
                    throw se;
                }
                finally
                {
                    conn.Close();
                    cmd.Dispose();
                }

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    if (HttpContext.Current.Session["Tarea"].ToString() == "Borrar")
                    {
                        sqlConsulta = " select * from InterventorProduccionMes where CodProducto = " + codProduccion;
                    }
                    else
                    {
                        sqlConsulta = "select * from InterventorProduccionMesTMP where CodProducto = " + codProduccion;
                    }
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        sqlConsulta = " select * from InterventorProduccionMes where CodProducto = " + codProduccion + " ORDER BY tipo, mes";
                    }
                }

                cmd = new SqlCommand(sqlConsulta, conn);
                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        TextBox controltext;
                        string valor_Obtenido = reader["Tipo"].ToString();//.Equals("1");

                        if (valor_Obtenido.Equals("1"))
                            controltext = (TextBox)this.FindControl("Fondoo" + reader["Mes"].ToString());
                        else
                            controltext = (TextBox)this.FindControl("Aporte" + reader["Mes"].ToString());

                        if (String.IsNullOrEmpty(reader["Valor"].ToString()))
                            controltext.Text = "0";
                        else
                        {
                            Double valor = Double.Parse(reader["Valor"].ToString());
                            controltext.Text = valor.ToString();
                        }

                        sumar(controltext);
                    }
                }
                //catch (SqlException se)
                catch (Exception se)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error " + se.Message + "')", true);
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                    cmd.Dispose();
                }

                #endregion
            }
            else
            {
                #region Carga la información de "FrameProduccionInter.aspx".             

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    sqlConsulta = "select * from InterventorProduccionMesTMP where CodProducto = " + codProduccion;
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        sqlConsulta = "select * from InterventorProduccionMes where CodProducto = " + codProduccion + " ORDER BY tipo, mes";
                    }
                }

                var producto = (from p in consultas.Db.InterventorProduccionTMPs
                                where p.id_produccion == int.Parse(codProduccion)
                                select p).FirstOrDefault();
                if (producto != null)
                {
                    TB_Item.Text = producto.NomProducto;
                    DataTable Rs1 = new DataTable();
                    Rs1 = consultas.ObtenerDataTable(sqlConsulta, "text");
                    if (Rs1.Rows.Count >= 1)
                    {
                        int i = 0;
                        string valor_Obtenido = "";
                        while (i < Rs1.Rows.Count)
                        {
                            TextBox controltext = new TextBox();
                            valor_Obtenido = Rs1.Rows[i]["Tipo"].ToString();

                            if (valor_Obtenido.Equals("1"))
                                controltext = (TextBox)this.FindControl("Fondoo" + Rs1.Rows[i]["Mes"].ToString());
                            else
                                controltext = (TextBox)this.FindControl("Aporte" + Rs1.Rows[i]["Mes"].ToString());

                            if (String.IsNullOrEmpty(Rs1.Rows[i]["Valor"].ToString()))
                                controltext.Text = "0";
                            else
                            {
                                Double valor = Double.Parse(Rs1.Rows[i]["Valor"].ToString());
                                controltext.Text = valor.ToString();
                            }
                            sumar(controltext);
                            i++;
                        }
                    }
                }
                else
                {
                    var producto2 = (from p in consultas.Db.InterventorProduccions
                                     where p.id_produccion == int.Parse(codProduccion)
                                     select p).FirstOrDefault();

                    if (producto2 != null)
                    {
                        TB_Item.Text = producto2.NomProducto;
                        DataTable Rs1 = new DataTable();

                        sqlConsulta = "select * from InterventorProduccionMes where CodProducto = " + producto2.id_produccion + " ORDER BY tipo, mes";
                        Rs1 = consultas.ObtenerDataTable(sqlConsulta, "text");
                        if (Rs1.Rows.Count >= 1)
                        {
                            int i = 0;
                            string valor_Obtenido = "";
                            while (i < Rs1.Rows.Count)
                            {
                                TextBox controltext = new TextBox();
                                valor_Obtenido = Rs1.Rows[i]["Tipo"].ToString();

                                if (valor_Obtenido.Equals("1"))
                                    controltext = (TextBox)this.FindControl("Fondoo" + Rs1.Rows[i]["Mes"].ToString());
                                else
                                    controltext = (TextBox)this.FindControl("Aporte" + Rs1.Rows[i]["Mes"].ToString());

                                if (String.IsNullOrEmpty(Rs1.Rows[i]["Valor"].ToString()))
                                    controltext.Text = "0";
                                else
                                {
                                    Double valor = Double.Parse(Rs1.Rows[i]["Valor"].ToString());
                                    controltext.Text = valor.ToString();
                                }
                                sumar(controltext);
                                i++;
                            }
                        }
                    }
                }




                #endregion
            }
        }


        #region Métodos activados por la selección de un plan operativo en "cambiosPO.aspx".

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/05/2014.
        /// Cargar el título, dependiendo de la acción a realizar.
        /// </summary>
        /// <param name="accion">La acción DEBE SER "Adicionar", "Modificar" o "Eliminar".</param>
        private void CargarTitulo(String accion)
        {
            try
            {
                if (accion == "Adicionar")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        lbl_enunciado.Text = "NUEVO";
                    }
                }
                if (accion == "Modificar")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        lbl_enunciado.Text = "MODIFICAR";
                    }
                }
                if (accion == "Eliminar")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        lbl_enunciado.Text = "ELIMINAR";
                    }
                }
            }
            catch { lbl_enunciado.Text = "ADICIONAR ACTIVIDAD"; }
        }

        #endregion

    }
}