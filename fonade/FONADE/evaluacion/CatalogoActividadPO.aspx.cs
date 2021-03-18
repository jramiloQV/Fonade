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
using System.Text;
using Fonade.Status;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoActividadPO
    /// </summary>
    
    public partial class CatalogoActividadPO : Base_Page
    {
        /// <summary>
        /// Codigo del proyecto
        /// </summary>
        public String CodProyecto;
        /// <summary>
        /// txtTab
        /// </summary>
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        /// <summary>
        /// verifica
        /// </summary>
        public int verifica = 0;

        // Se crean las instancias tipo (proyectoactividadPOInterventor y CatalogoActividadPO) para administrar
        // la administracion de lass actividades creadas por cada perfil de usuario        
        /// <summary>
        /// ObjProActPOInt
        /// </summary>
        public Negocio.Entidades.ProyectoActividadPOInterventorEntity ObjProActPOInt;

        /// <summary>
        /// ObjCatActPO
        /// </summary>
        public Negocio.Evaluacion.CatalogoActividadPONegocio ObjCatActPO = new Negocio.Evaluacion.CatalogoActividadPONegocio();

        /// <summary>
        /// ObjProActPOMesInt
        /// </summary>
        public Negocio.Entidades.ProyectoActividadPOMesInterventorEntity ObjProActPOMesInt;

        /// <summary>
        /// LST3
        /// </summary>
        public List<Negocio.Entidades.ProyectoActividadPOMesInterventorEntity> lst3;

        /// <summary>
        /// LST
        /// </summary>
        public List<Negocio.Entidades.ProyectoActividadPOInterventorEntity> lst = new List<Negocio.Entidades.ProyectoActividadPOInterventorEntity>();

        /// <summary>
        /// LST1
        /// </summary>
        public List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity> lst1 = new List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity>();

        /// <summary>
        /// LST2
        /// </summary>
        public List<Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity> lst2 = new List<Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity>();

        /// <summary>
        /// ObjCambiosPo
        /// </summary>
        public Negocio.Interventoria.CambiosPONegocio ObjCambiosPo = new Negocio.Interventoria.CambiosPONegocio();

        Int32 CodActividad;
        /// <summary>
        /// Indica que si el valor contiene alguna información, significa que proviene
        /// probablemente de un valor seleccionado en "CambiosPO.aspx", por lo tanto, DEBE mostrar ciertos campos que se
        /// mantienen invisibles.
        /// </summary>
        String Detalles_CambiosPO_PO;
        /// <summary>
        /// Valor creado en "FramePlanOperativoInterventoria.aspx" que dicta que el valor seleccionado
        /// es de "Actividades en Aprobación", por lo tanto consulta las tablas temporales.
        /// </summary>
        String dato_TMP;
        /// <summary>
        /// Nombre del proyecto.
        /// </summary>
        String txtNomProyecto;
        String txtSQL;

        public string _Item { get; set; }
        public string _NomActividad { get; set; }
        public string _Metas { get; set; }


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Establecer en variable de sesión "txtObjeto" el valor correspondiente de acuerdo a su rol.
            //Si es coordinador o gerente, se crea la sesión "txtObjeto" con el valor "Agendar Tarea";
            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            { HttpContext.Current.Session["txtObjeto"] = "Agendar Tarea"; }
            //Si es Interventor, lo hace con otro dato.
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            { HttpContext.Current.Session["txtObjeto"] = "Mis Proyectos para Seguimiento de Interventoría"; }
            //Si es Evaluador...
            if (usuario.CodGrupo == Constantes.CONST_Evaluador)
            { HttpContext.Current.Session["txtObjeto"] = "Mis planes de negocio a evaluar"; }

            //Código del proyecto:
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            //Código de la actividad seleccionada.
            CodActividad = HttpContext.Current.Session["CodActividad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodActividad"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodActividad"].ToString()) : 0;

            //
            var accionActual = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "";

            // Consulta para traer el nombre del proyecto
            txtSQL = "select NomProyecto from Proyecto WHERE id_proyecto=" + CodProyecto;
            var rr = consultas.ObtenerDataTable(txtSQL, "text");
            if (rr.Rows.Count > 0) { txtNomProyecto = rr.Rows[0]["NomProyecto"].ToString(); rr = null; }


            try
            {
                if (!IsPostBack)
                {
                    //Asignar evento JavaScript a TextBox.
                    TB_item.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");

                    // Variables de sesión creadas en "CambiosPO.aspx".
                    //Sesión inicial que indica que la información a procesar proviene de "CambiosPO.aspx".
                    Detalles_CambiosPO_PO = HttpContext.Current.Session["Detalles_CambiosPO_PO"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Detalles_CambiosPO_PO"].ToString()) ? HttpContext.Current.Session["Detalles_CambiosPO_PO"].ToString() : "";


                    //Revisar si la variable contiene datos "debe ser así para volver visibles ciertos campos".
                    if (Detalles_CambiosPO_PO.Trim() != "" && CodActividad != 0)
                    {
                        // Procesar la información que proviene de "CambiosPO.aspx".

                        //Mostrar campos.
                        lbl_inv_aprobar.Visible = true;
                        dd_inv_aprobar.Visible = true;
                        lbl_inv_obvservaciones.Visible = true;
                        txt_inv_observaciones.Visible = true;
                        TB_Actividad.Visible = true;

                        //Inhabilitar panel que contiene la tabla dinámica.
                        P_Meses.Enabled = false;

                        //Evaluar la acción a tomar. #SANTIAGO SANCHEZ CAMBIOS
                        if (accionActual.Equals("Adicionar"))
                        {
                            B_Acion.Width = 100;
                            CargarTitulo(accionActual);

                        }
                        if (accionActual.Equals("actualizar") || accionActual.Equals("Modificar"))
                        {
                            B_Acion.Text = "Actualizar";
                            CargarTitulo(accionActual);
                        }
                        if (accionActual.Equals("Borrar"))
                        {
                            B_Acion.Text = "Borrar";
                            CargarTitulo(accionActual);
                        }
                        // FIN CAMBIOS SANTIAGO


                        //Llenar el panel.
                        llenarpanel();

                        if (CodActividad != 0)
                        {
                            //Buscar los datos.
                            buscarDatos(CodActividad, true);
                        }

                    }
                    else
                    {
                        // Procesar la información que proviene de "FramePlanOperativoInterventoria.aspx".

                        //Si ya aquí NO contiene datos, es porque es una NUEVA ACTIVIDAD!
                        if (CodActividad == 0) { lbl_titulo_PO.Text = "NUEVA ACTIVIDAD"; }

                        //Evaluar la acción a tomar. 
                        //CAMBIOS SANTIAGO (se cambio HttpContext.Current.Session["Accion"].ToString por accionActual y el aprametro de cargar titulo es el mismo accionActual)
                        if (accionActual.Equals("Adicionar"))
                        {
                            B_Acion.Width = 100;
                            CargarTitulo(accionActual);

                        }
                        if (accionActual.Equals("actualizar") || accionActual.Equals("Modificar"))
                        {
                            B_Acion.Text = "Actualizar";
                            lbl_titulo_PO.Text = "Editar";
                        }
                        if (accionActual.Equals("Borrar"))
                        {
                            B_Acion.Text = "Borrar";
                            CargarTitulo(accionActual);
                        }

                        //Llenar el panel.
                        llenarpanel();

                        if (CodActividad != 0)
                        {
                            //Buscar los datos.
                            buscarDatos(CodActividad, false);
                        }


                    }
                    CargarGrid();
                }
                else
                {
                    //Llenar el panel.
                    llenarpanel();
                }
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true); }
        }

        private void CargarGrid()
        {
            dd_inv_aprobar.Items.Insert(0, new ListItem("No", "0"));
            dd_inv_aprobar.Items.Insert(1, new ListItem("Si", "1"));
        }

        /// <summary>
        ///  código fuente para generar 14 meses.
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

            // Nuevas líneas, con el valor obtenido de la prórrgoa se suma a la constante de meses y se genera la tabla.
            int prorroga = 0;
            prorroga = ObtenerProrroga(CodProyecto);
            int prorrogaTotal = prorroga + Constantes.CONST_Meses + 1;


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
                    labelFondo.Text = "Fondo Emprender"; //Cantidad
                    labelAportes.ID = "labelaportes";
                    labelAportes.Text = "Aporte Emprendedor"; //Costo
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


                if (i < prorrogaTotal) //15
                {
                    celdaMeses = new TableCell();
                    celdaFondo = new TableCell();
                    celdaAporte = new TableCell();
                    celdaTotal = new TableCell();


                    celdaMeses.Width = 70;
                    celdaFondo.Width = 70;
                    celdaAporte.Width = 70;
                    celdaTotal.Width = 70;

                    Label labelMeses = new Label();
                    labelMeses.ID = "Mes" + i;
                    labelMeses.Text = "Mes " + i;
                    labelMeses.Width = 70;
                    labelMeses.Font.Size = 8;
                    TextBox textboxFondo = new TextBox();
                    textboxFondo.ID = "Fondoo" + i;
                    textboxFondo.Width = 70;
                    textboxFondo.TextChanged += new System.EventHandler(TextBox_TextChanged);
                    textboxFondo.AutoPostBack = true;
                    textboxFondo.MaxLength = 10;
                    textboxFondo.Font.Size = 8;
                    textboxFondo.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                    TextBox textboxAporte = new TextBox();
                    textboxAporte.ID = "Aporte" + i;
                    textboxAporte.Width = 70;
                    textboxAporte.TextChanged += new EventHandler(TextBox_TextChanged);
                    textboxAporte.AutoPostBack = true;
                    textboxAporte.MaxLength = 10;
                    textboxAporte.Font.Size = 8;
                    textboxAporte.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                    Label totalMeses = new Label();
                    totalMeses.ID = "TotalMes" + i;
                    totalMeses.Text = "0.0";
                    totalMeses.Width = 70;
                    totalMeses.Font.Bold = true;
                    totalMeses.Font.Size = 8;

                    celdaMeses.Controls.Add(labelMeses);


                    celdaFondo.Controls.Add(textboxFondo);
                    celdaAporte.Controls.Add(textboxAporte);
                    celdaTotal.Controls.Add(totalMeses);

                    filaTablaMeses.Cells.Add(celdaMeses);
                    filaTablaFondo.Cells.Add(celdaFondo);
                    filaTablaAporte.Cells.Add(celdaAporte);
                    filaTotal.Cells.Add(celdaTotal);
                }
                if (i == prorrogaTotal) //15
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
                    labelFondo.Font.Size = 8;
                    labelFondo.Font.Bold = true;
                    labelAportes.ID = "labelaportescosto";
                    labelAportes.Text = "0";
                    labelAportes.Font.Size = 8;
                    labelAportes.Font.Bold = true;
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

                    //}
                }
            }
        }

        /// <summary>
        /// Buscar los datos de la actividad seleccionada.
        /// </summary>
        /// <param name="actividad">Actividad a consultar</param>
        /// <param name="VieneDeCambiosPO">TRUE = Cargar la información de "CambiosPO.aspx". // FALSE = Cargar la información de "FramePlanOperativoInterventoria.aspx".</param>
        private void buscarDatos(Int32 actividad, bool VieneDeCambiosPO)
        {
            dato_TMP = HttpContext.Current.Session["dato_TMP"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["dato_TMP"].ToString()) ? HttpContext.Current.Session["dato_TMP"].ToString() : "";

            if (VieneDeCambiosPO == true)
            {
                //Cargar la información según la página "CambiosPO.aspx".

                String ChequeoCoordinador;
                String ChequeoGerente;

                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                String sqlConsulta;
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    // Los campos del formulario se bloquean.

                    TB_item.Enabled = false;
                    TB_Actividad.Enabled = false;
                    TB_metas.Enabled = false;
                    P_Meses.Enabled = false;


                    sqlConsulta = "SELECT * FROM [ProyectoActividadPOInterventorTMP] WHERE [CodProyecto] = " + CodProyecto + "AND [Id_Actividad] = " + actividad;
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOInterventor] WHERE [Id_Actividad] = " + actividad;
                    }
                    else
                    {
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPO] WHERE [Id_Actividad] = " + actividad;
                    }
                }
                try
                {

                    var reader = consultas.ObtenerDataTable(sqlConsulta, "text");
                    if (reader.Rows.Count > 0)
                    {
                        try
                        {
                            HttpContext.Current.Session["Tarea"] = reader.Rows[0].ItemArray[6].ToString();
                            ChequeoCoordinador = reader.Rows[0].ItemArray[5].ToString();
                            ChequeoGerente = reader.Rows[0].ItemArray[7].ToString();

                            // Aprobación de cambios del coordinador.
                            if (!String.IsNullOrEmpty(ChequeoCoordinador))
                            {
                                if (ChequeoCoordinador == "True" || ChequeoCoordinador == "1")
                                { dd_inv_aprobar.SelectedValue = "1"; }
                            }
                            else
                            { dd_inv_aprobar.SelectedValue = "0"; }

                            // Aprobación de cambios del gerente.
                            if (!String.IsNullOrEmpty(ChequeoGerente))
                            {
                                if (ChequeoGerente == "True" || ChequeoGerente == "1")
                                { dd_inv_aprobar.SelectedValue = "1"; }
                            }
                            else
                            { dd_inv_aprobar.SelectedValue = "0"; }


                        }
                        catch (NullReferenceException) { }
                        catch (Exception) { }

                        _Item = reader.Rows[0].ItemArray[3].ToString();
                        _NomActividad = reader.Rows[0].ItemArray[1].ToString();
                        Session["NombActividad"] = reader.Rows[0].ItemArray[1].ToString();
                        _Metas = reader.Rows[0].ItemArray[4].ToString();
                    }
                }
                catch (SqlException se)
                {
                    throw se;
                }
                finally
                {
                    conn.Close();
                    conn.Dispose();
                }

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    sqlConsulta = "SELECT * FROM [ProyectoActividadPOMesInterventorTMP] WHERE [CodActividad] = " + actividad;
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOMesInterventor] WHERE [CodActividad] = " + actividad;
                    }
                    else
                    {
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOMes] WHERE [CodActividad] = " + actividad;
                    }
                }

                try
                {
                    var reader = consultas.ObtenerDataTable(sqlConsulta, "text");
                    if (reader.Rows.Count > 0)
                    {
                        foreach (DataRow fila in reader.Rows)
                        {
                            TextBox controltext = new TextBox();
                            string valor_Obtenido = Convert.IsDBNull(fila.ItemArray[2]) ? "1" : fila.ItemArray[2].ToString();
                            if (valor_Obtenido.Equals("1"))
                            {
                                controltext = (TextBox)this.FindControl("Fondoo" + (Convert.IsDBNull(fila.ItemArray[1]) ? "0" : fila.ItemArray[1].ToString())) ?? new TextBox() { ID = "Fondoo1" };
                            }
                            else
                            {
                                controltext = (TextBox)this.FindControl("Aporte" + (Convert.IsDBNull(fila.ItemArray[1]) ? "0" : fila.ItemArray[1].ToString())) ?? new TextBox() { ID = "Aporte1" };
                            }
                            controltext.Text = Convert.IsDBNull(fila.ItemArray[3]) ? "0" : Convert.ToDecimal(fila.ItemArray[3].ToString()).ToString("C");
                            sumar(controltext);
                        }
                    }
                }
                catch (Exception se)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error " + se.Message + "')", true);
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                    conn.Dispose();
                }
            }
            else
            {
                //Cargar la información según la página "FramePlanOperativoInterventoria.aspx".

                if (dato_TMP == "")
                {
                    // Información de la grilla "Actividades".
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    String sqlConsulta;
                    DataTable tabla = new DataTable();

                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        // Consultar la información si es Coordinador o Gerente Interventor.
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOInterventorTMP] WHERE [CodProyecto] = " + CodProyecto + "AND [Id_Actividad] = " + actividad;

                        tabla = null;
                        tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                        if (tabla.Rows.Count > 0)
                        {
                            _Item = tabla.Rows[0]["Item"].ToString();
                            _NomActividad = tabla.Rows[0]["NomActividad"].ToString();
                            _Metas = tabla.Rows[0]["Metas"].ToString();
                        }

                    }
                    else
                    {
                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        {
                            // Consultar la información si es Interventor.
                            sqlConsulta = "SELECT * FROM [ProyectoActividadPOInterventor] WHERE [Id_Actividad] = " + actividad;

                            tabla = null;
                            tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                            if (tabla.Rows.Count > 0)
                            {
                                _Item = tabla.Rows[0]["Item"].ToString();
                                _NomActividad = tabla.Rows[0]["NomActividad"].ToString();
                                Session["NombActividad"] = _NomActividad;
                                _Metas = tabla.Rows[0]["Metas"].ToString();
                            }

                        }
                        else
                        {
                            //Consultar "si es otro tipo de usuario - grupo"...
                            sqlConsulta = "SELECT * FROM [ProyectoActividadPO] WHERE [Id_Actividad] = " + actividad;

                            tabla = null;
                            tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                            if (tabla.Rows.Count > 0)
                            {
                                _Item = tabla.Rows[0]["Item"].ToString();
                                _NomActividad = tabla.Rows[0]["NomActividad"].ToString();
                                _Metas = tabla.Rows[0]["Metas"].ToString();
                            }
                        }
                    }
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOMesInterventorTMP] WHERE [CodActividad] = " + actividad;
                    }
                    else
                    {
                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        {
                            sqlConsulta = "SELECT * FROM [ProyectoactividadPOMesInterventor] WHERE [CodActividad] = " + actividad;
                        }
                        else
                        {
                            sqlConsulta = "SELECT * FROM [ProyectoActividadPOMes] WHERE [CodActividad] = " + actividad;
                        }
                    }
                    try
                    {
                        var reader = consultas.ObtenerDataTable(sqlConsulta, "text");
                        if (reader.Rows.Count > 0)
                        {
                            foreach (DataRow fila in reader.Rows)
                            {
                                TextBox controltext;
                                string valor_Obtenido = fila.ItemArray[2].ToString();

                                if (valor_Obtenido.Equals("1"))
                                {
                                    controltext = (TextBox)this.FindControl("Fondoo" + fila.ItemArray[1].ToString()); // reader["Mes"].ToString());
                                }
                                else
                                {
                                    controltext = (TextBox)this.FindControl("Aporte" + fila.ItemArray[1].ToString()); // reader["Mes"].ToString());
                                }

                                if (String.IsNullOrEmpty(fila.ItemArray[3].ToString()))
                                    controltext.Text = "0";
                                else
                                {
                                    var valor = Double.Parse(fila.ItemArray[3].ToString());
                                    controltext.Text = valor.ToString("0", CultureInfo.InvariantCulture);
                                }

                                sumar(controltext);

                            }
                        }

                    }
                    catch (Exception se)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error " + se.Message + "')", true);
                    }
                    finally
                    {
                        if (conn != null)
                            conn.Close();
                    }

                }
                else
                {
                    // Información de la grilla "Actividades en Aprobación".

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    String sqlConsulta;
                    DataTable tabla = new DataTable();

                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        // Consultar la información si es Coordinador o Gerente Interventor.
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOInterventorTMP] WHERE [CodProyecto] = " + CodProyecto + "AND [Id_Actividad] = " + actividad;

                        tabla = null;
                        tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                        if (tabla.Rows.Count > 0)
                        {
                            _Item = tabla.Rows[0]["Item"].ToString();
                            _NomActividad = tabla.Rows[0]["NomActividad"].ToString();
                            _Metas = tabla.Rows[0]["Metas"].ToString();
                        }

                    }
                    else
                    {
                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        {
                            // Consultar la información si es Interventor.
                            sqlConsulta = "SELECT * FROM ProyectoActividadPOInterventorTMP WHERE Id_Actividad = " + actividad;

                            tabla = null;
                            tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                            if (tabla.Rows.Count > 0)
                            {
                                _Item = tabla.Rows[0]["Item"].ToString();
                                _NomActividad = tabla.Rows[0]["NomActividad"].ToString();
                                _Metas = tabla.Rows[0]["Metas"].ToString();
                            }

                        }
                        else
                        {
                            // Consultar "si es otro tipo de usuario - grupo"...
                            sqlConsulta = "SELECT * FROM [ProyectoActividadPO] WHERE [Id_Actividad] = " + actividad;

                            tabla = null;
                            tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                            if (tabla.Rows.Count > 0)
                            {
                                _Item = tabla.Rows[0]["Item"].ToString();
                                _NomActividad = tabla.Rows[0]["NomActividad"].ToString();
                                _Metas = tabla.Rows[0]["Metas"].ToString();
                            }

                        }
                    }
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        sqlConsulta = "SELECT * FROM [ProyectoActividadPOMesInterventorTMP] WHERE [CodActividad] = " + actividad;
                    }
                    else
                    {
                        if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        {
                            sqlConsulta = "SELECT * FROM ProyectoActividadPOMesInterventorTMP WHERE CodActividad = " + actividad;
                        }
                        else
                        {
                            sqlConsulta = "SELECT * FROM [ProyectoActividadPOMes] WHERE [CodActividad] = " + actividad;
                        }
                    }

                    SqlCommand cmd = new SqlCommand(sqlConsulta, conn);
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            TextBox controltext;
                            string valor_Obtenido = reader["CodTipoFinanciacion"].ToString();//.Equals("1");

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
                    }

                }
            }
        }

        private string _cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        ValidarActividades validarActividades = new ValidarActividades();
        //alex

        /// <summary>
        /// Handles the Click event of the B_Acion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Acion_Click(object sender, EventArgs e)
        {
            if (B_Acion.Text.Equals("Crear") || B_Acion.Text.Equals("Adicionar")) { alamcenar(1); }
            if (B_Acion.Text.Equals("Actualizar") || B_Acion.Text.Equals("Modificar")) { alamcenar(2); }
            if (B_Acion.Text.Equals("Eliminar") || B_Acion.Text.Equals("Borrar")) { alamcenar(3); }
        }

        private void alamcenar(int acion)
        {
            //Inicializar variables.            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String NomActividad = "";
            DataTable RsActividad = new DataTable();
            DataTable Rs = new DataTable();
            DataTable RsRevisa = new DataTable();
            String correcto = "";

            if (CodProyecto != "0" || CodActividad != 0)
            {
                if (acion == 1)
                {
                    // Guardar la información de plan operativo "guarda en tablas temporales".

                    //Si es Interventor.
                    //Si es interventor inserta los registros en tablas temporales para la aprobación del coordinador y el gerente.
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        //Asigna la tarea al coordinador.
                        string txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;

                        var dt = consultas.ObtenerDataTable(txtSQL, "text");
                        correcto = dt.Rows[0].ItemArray[0].ToString();
                        if (string.IsNullOrEmpty(correcto))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                            return;
                        }
                        int ActividadTmp = 1;
                        txtSQL = "select Id_Actividad  from proyectoactividadPOInterventorTMP ORDER BY Id_Actividad DESC";
                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                        if (Rs.Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningúna actividada asociada.');window.close();", true);
                            return;
                        }
                        ActividadTmp = Convert.ToInt32(Rs.Rows[0]["Id_Actividad"]) + 1;

                        int cantRegPlanOperativo = validarActividades
                                                        .validarPlanOperativo(Convert.ToInt32(CodProyecto), Convert.ToInt32(TB_item.Text), TB_Actividad.Text.ToUpper());

                        int cantRegPlanOperativoTMP = validarActividades
                                                         .validarPlanOperativoTMP(Convert.ToInt32(CodProyecto), Convert.ToInt32(TB_item.Text), TB_Actividad.Text.ToUpper());

                        if (cantRegPlanOperativoTMP > 0 || cantRegPlanOperativo > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una actividad de Plan Operativo registrada con la misma informacion.');window.close();", true);
                        }
                        else
                        {

                            txtSQL = "Insert into proyectoactividadPOInterventorTMP (id_actividad,CodProyecto,Item,NomActividad,Metas) " +
                                    "values (" + ActividadTmp + "," + CodProyecto + "," + TB_item.Text + ",'" + TB_Actividad.Text + "','" + TB_metas.Text + "')";
                            ejecutaReader(txtSQL, 2);

                            //1 -> Guarda Log Plan Operativo
                            LogActivitdades.WriteError(1, txtSQL);

                            string mes = "0";
                            string valor = "0";
                            string tipo = "0";

                            // Leer los valores de los TextBox para insertarlos en la tabla temporal.
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

                                            //Insertar los costos por mes y tipo de financiacion.
                                            txtSQL = "INSERT INTO ProyectoactividadPOMesInterventorTMP(CodActividad,Mes,CodTipoFinanciacion,Valor) " +
                                            "VALUES(" + ActividadTmp + "," + mes + "," + tipo + ", Cast('" + valor + "' as money))";
                                            if (!string.IsNullOrEmpty(txtSQL))
                                            {
                                                ejecutaReader(txtSQL, 2);
                                            }
                                            txtSQL = null;
                                        }
                                    }
                                }
                            }


                            //Agendar tarea.
                            txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                            var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                            var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());

                            AgendarTarea agenda = new AgendarTarea
                            (idCoord,
                            "Actividad del Plan Operativo creada por Interventor",
                            "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + TB_Actividad.Text + "<br>Observaciones:</br>" + txt_inv_observaciones.Text.Trim(),
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

                            //agenda.Agendar();


                            //Destruir variables.
                            CodProyecto = "0";
                            CodActividad = 0;
                            HttpContext.Current.Session["CodActividad"] = null;
                            HttpContext.Current.Session["NomActividad"] = null;

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                        }

                    }


                    //Si es Coordinado inserta los registros en tablas temporales para la aprobación del gerente.
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        var grupo = (from g in consultas.Db.Grupos
                                     where g.NomGrupo == "Gerente Interventor"
                                     select g).FirstOrDefault();
                        var contacto = (from c in consultas.Db.GrupoContactos
                                        where c.CodGrupo == grupo.Id_Grupo
                                        select c).FirstOrDefault();

                        if (dd_inv_aprobar.SelectedValue == "1")
                        {
                            txtSQL = "UPDATE proyectoactividadPOInterventorTMP SET ChequeoCoordinador=1 Where CodProyecto=" + CodProyecto + " and Id_Actividad=" + CodActividad + ";";

                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea.
                            AgendarTarea agenda = new AgendarTarea
                            (contacto.CodContacto,
                            "Actividad del Plan Operativo creada por Coord. de  Interventoria",
                            "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + NomActividad + "Observaciones:" + txt_inv_observaciones.Text.Trim(),
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
                        }
                        else
                        {
                            var planOperTmp = (from pot in consultas.Db.ProyectoActividadPOInterventorTMPs
                                               where pot.CodProyecto == int.Parse(CodProyecto) && pot.Id_Actividad == CodActividad
                                               select pot).FirstOrDefault();

                            txtSQL = "DELETE FROM proyectoactividadPOInterventorTMP where CodProyecto=" + CodProyecto + " and Id_Actividad=" + CodActividad;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "DELETE FROM ProyectoactividadPOMesInterventorTMP where CodActividad=" + CodActividad;
                            ejecutaReader(txtSQL, 2);

                            // Seleccional id del Interventor
                            var datos = (from ei in consultas.Db.EmpresaInterventors
                                         join ee in consultas.Db.Empresas on ei.CodEmpresa equals ee.id_empresa
                                         join p in consultas.Db.Proyecto
                                         on ee.codproyecto equals p.Id_Proyecto
                                         where ee.codproyecto == int.Parse(Session["CodProyecto"].ToString()) && ei.Inactivo == false
                                         select new datosAgendar
                                         {
                                             idContacto = (int)ei.CodContacto,
                                             idProyecto = (int)p.Id_Proyecto,
                                             nombre = p.NomProyecto
                                         }).ToList();
                            AgendarTarea agenda = new AgendarTarea
                            (datos[0].idContacto,
                            "Actividad del Plan Operativo Rechazada por el Coordniador de Interventoria",
                            "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + planOperTmp.NomActividad + "Observaciones:" + txt_inv_observaciones.Text.Trim(),
                            CodProyecto,
                            12,
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
                        }
                    }

                    //Si es Gerente Interventor.
                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")//Si
                        {
                            // Aprobado.
                            // TRAE LOS REGISTROS DE LA TABLA TEMPORAL.

                            txtSQL = " select * from proyectoactividadPOInterventorTMP " +
                                     " where CodProyecto = " + CodProyecto +
                                     " and Id_Actividad = " + CodActividad;
                            var prjActividadInter = (from p in consultas.Db.ProyectoActividadPOInterventorTMPs
                                                     where p.CodProyecto == int.Parse(CodProyecto)
                                                         && p.Id_Actividad == CodActividad
                                                     select p).FirstOrDefault();

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            int cantRegPlanOperativo = validarActividades
                                                        .validarPlanOperativo(Convert.ToInt32(CodProyecto), Convert.ToInt32(prjActividadInter.Item), prjActividadInter.NomActividad.ToUpper());

                            if (cantRegPlanOperativo > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una actividad de Plan Operativo registrada con la misma informacion.');window.close();", true);
                            }
                            else
                            {
                                // INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA.
                                var acvidadInterventor = new Datos.ProyectoActividadPOInterventor
                                {
                                    CodProyecto = int.Parse(CodProyecto),
                                    Item = (Int16)prjActividadInter.Item,
                                    NomActividad = prjActividadInter.NomActividad,
                                    Metas = prjActividadInter.Metas
                                };

                                var flag = (from a in consultas.Db.ProyectoActividadPOInterventors
                                            where a.CodProyecto == acvidadInterventor.CodProyecto && a.Item == acvidadInterventor.Item && a.NomActividad == acvidadInterventor.NomActividad
                                            select a).FirstOrDefault();
                                if (flag == null)
                                {
                                    consultas.Db.ProyectoActividadPOInterventors.InsertOnSubmit(acvidadInterventor);
                                    consultas.Db.SubmitChanges();
                                }

                                // BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.
                                //1 -> Plan Operativo
                                txtSQL = " var acvidadInterventor = new Datos.ProyectoActividadPOInterventor" +
                                "{" +
                                    "CodProyecto = int.Parse(CodProyecto)," +
                                    "Item = (Int16)prjActividadInter.Item," +
                                    "NomActividad = prjActividadInter.NomActividad," +
                                    "Metas = prjActividadInter.Metas" +
                                "};";
                                //1 -> Guarda Log Plan Operativo
                                LogActivitdades.WriteError(1, txtSQL);

                                txtSQL = " DELETE proyectoactividadPOInterventorTMP " +
                                         " where CodProyecto = " + CodProyecto +
                                         " and Id_Actividad = " + CodActividad + "; ";
                                txtSQL += "Select @@rowcount;";

                                //Ejecutar consulta.
                                correcto = consultas.RetornarEscalar(txtSQL, "text").ToString();

                                if (string.IsNullOrEmpty(correcto))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar registros de la tabla temporal: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                    return;
                                }


                                // TRAE EL CODIGO DE ACTIVIDAD PARA ADICIONARLO A LA TABLA DEFINITIVA POR MES.


                                // TRAE LOS REGISTROS DE LA TABLA TEMPORAL POR MESES.

                                txtSQL = " select * from ProyectoActividadPOMesInterventorTMP " +
                                         " where CodActividad = " + CodActividad;
                                Rs = consultas.ObtenerDataTable(txtSQL, "text");
                                var filas = (from am in consultas.Db.ProyectoActividadPOMesInterventorTMPs
                                             where am.CodActividad == CodActividad
                                             select am).ToList();


                                // INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA POR MESES.



                                foreach (DataRow row_Rs in Rs.Rows)
                                {
                                    txtSQL = " SELECT * FROM ProyectoActividadPOMesInterventor " +
                                             " where codactividad = " + acvidadInterventor.Id_Actividad + " and mes = " + row_Rs["Mes"].ToString() + " and codtipoFinanciacion = " + row_Rs["codtipoFinanciacion"].ToString();
                                    var val = row_Rs["Valor"].ToString().Split(',');
                                    RsRevisa = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (RsRevisa.Rows.Count > 0)
                                    {
                                        // Actualizar.

                                        txtSQL = " update ProyectoActividadPOMesInterventor set CodTipoFinanciacion = " + row_Rs["CodTipoFinanciacion"].ToString() + ", Valor = " + val[0] +
                                                 " where codactividad = " + acvidadInterventor.Id_Actividad + " and mes = " + row_Rs["Mes"].ToString() + ";";

                                    }
                                    else
                                    {
                                        // Ingresar.

                                        txtSQL = " Insert into ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor) " +
                                                 " values (" + acvidadInterventor.Id_Actividad + ", " + row_Rs["Mes"].ToString() + ", " + row_Rs["CodTipoFinanciacion"].ToString() + ", " + val[0] + "); ";

                                    }

                                    //Ejecutar consulta.
                                    txtSQL += " Select @@rowcount;";
                                    correcto = consultas.RetornarEscalar(txtSQL, "text").ToString();

                                    if (string.IsNullOrEmpty(correcto))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                                        return;
                                    }
                                }



                                // BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.

                                txtSQL = " DELETE ProyectoActividadPOMesInterventorTMP " +
                                         " where CodActividad = " + CodActividad + "; Select @@rowcount;";

                                //Ejecutar consulta.

                                correcto = consultas.RetornarEscalar(txtSQL, "text").ToString();

                                if (string.IsNullOrEmpty(correcto))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar registros de la tabla temporal por mes: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                    return;
                                }

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
                                    "Revisar Actividad del Plan Operativo. Se ha Aprobado una actividad.",
                                    "Revisar actividad del plan operativo " + datos[0].nombre + " - Actividad --> " + acvidadInterventor.NomActividad + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                    "Catálogo Actividad Plan Operativo");

                                    agenda.Agendar();
                                }

                                //Destruir variables.
                                CodProyecto = "0";
                                //Session["CodProyecto"] = null;
                                CodActividad = 0;
                                HttpContext.Current.Session["CodActividad"] = null;
                                HttpContext.Current.Session["NomActividad"] = null;
                                //RedirectPage(false, string.Empty, "cerrar");
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                            }
                        }
                        else
                        {
                            // No Aprobado.

                            // Se devuelve al interventor, se le avisa al coordinador.

                            txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                     " FROM EmpresaInterventor " +
                                     " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                     " WHERE EmpresaInterventor.Inactivo = 0 " +
                                     " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                     " AND Empresa.codproyecto = " + CodProyecto;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");


                            // Eliminación #1.

                            txtSQL = " DELETE FROM proyectoactividadPOInterventorTMP " +
                                     " where CodProyecto = " + CodProyecto +
                                     " and Id_Actividad = " + CodActividad;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);



                            // Eliminación #2.
                            txtSQL = " DELETE FROM ProyectoactividadPOMesInterventorTMP where CodActividad = " + CodActividad;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);


                            // Generar tarea pendiente.
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
                                "Actividad del Plan Operativo Rechazada por Gerente Interventor.",
                                "Revisar actividad del plan operativo " + datos[0].nombre + " - Actividad --> " + Session["NombActividad"].ToString() + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Actividad Plan Operativo");

                                agenda.Agendar();
                            }


                            //Destruir variables.
                            CodProyecto = "0";
                            //Session["CodProyecto"] = null;
                            CodActividad = 0;
                            HttpContext.Current.Session["CodActividad"] = null;
                            HttpContext.Current.Session["NomActividad"] = null;
                            //RedirectPage(false, string.Empty, "cerrar");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                        }
                    }


                }
                if (acion == 2)
                {
                    // Editar el plan operativo seleccionado.

                    //Comprobar si el usuario tiene el código grupo de "Coordinador Interventor" ó "Gerente Interventor".
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        //Ejecutar como Coordinador Interventor.

                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            if (dd_inv_aprobar.SelectedValue == "1") //Si
                            {
                                // Aprobado.

                                // Actualización.

                                txtSQL = " UPDATE proyectoactividadPOInterventorTMP SET ChequeoCoordinador = 1 " +
                                         " where CodProyecto = " + CodProyecto + " and Id_Actividad = " + CodActividad + "; ";
                                txtSQL += "Select @@ROWCOUNT;";

                                //Ejecutar consulta.

                                correcto = consultas.RetornarEscalar(txtSQL, "text").ToString();

                                //Agendar tarea.
                                var grupo = (from g in consultas.Db.Grupos
                                             where g.NomGrupo == "Gerente Interventor"
                                             select g).FirstOrDefault();
                                var contacto = (from c in consultas.Db.GrupoContactos
                                                where c.CodGrupo == grupo.Id_Grupo
                                                select c).FirstOrDefault();

                                AgendarTarea agenda = new AgendarTarea
                                (contacto.CodContacto,
                                "Revisar Actividad del plan operativo. Se ha modificado una actividad.",
                                "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + Session["NombActividad"].ToString() + "<br>Observaciones:</br>" + txt_inv_observaciones.Text.Trim(),
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

                                if (string.IsNullOrEmpty(correcto))
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al actualizar la actividad aprobada: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                    return;
                                }

                                //COMENTADO EN FONADE CLÁSICO "prTareaAsignarGerente".

                                //Finalmente, destruye las variables.
                                correcto = "";
                                CodProyecto = "0";
                                CodActividad = 0;
                                HttpContext.Current.Session["CodActividad"] = null;
                                HttpContext.Current.Session["NomActividad"] = null;
                                HttpContext.Current.Session["CodProyecto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);

                            }
                            else
                            {
                                //No Aprobado.

                                // Consultar y asignar valores a "RsActividad".
                                //Consulta.
                                txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                         " FROM EmpresaInterventor " +
                                         " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                         " WHERE EmpresaInterventor.Inactivo = 0 " +
                                         " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                         " AND Empresa.CodProyecto = " + CodProyecto;

                                //Asignar resultados de la consulta.
                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");


                                // Eliminación #1.
                                var planOperTmp = (from pot in consultas.Db.ProyectoActividadPOInterventorTMPs
                                                   where pot.CodProyecto == int.Parse(CodProyecto) && pot.Id_Actividad == CodActividad
                                                   select pot).FirstOrDefault();

                                txtSQL = " DELETE FROM proyectoactividadPOInterventorTMP " +
                                         " where CodProyecto=" + CodProyecto + " and Id_Actividad = " + CodActividad;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                //Eliminación #2.
                                txtSQL = " DELETE FROM ProyectoactividadPOMesInterventorTMP " +
                                         " where CodActividad=" + CodActividad;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                //Agendar tarea.
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
                                "Modificacion a actividad del Plan Operativo Rechazada por Coordinador de Interventoria",
                                "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + planOperTmp.NomActividad + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
                                CodProyecto,
                                2,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "",
                                "",
                                "Traslado Planes");

                                agenda.Agendar();

                                //Finalmente, destruye las variables.
                                correcto = "";
                                CodProyecto = "0";
                                CodActividad = 0;
                                HttpContext.Current.Session["CodActividad"] = null;
                                HttpContext.Current.Session["NomActividad"] = null;
                                HttpContext.Current.Session["CodProyecto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                            }
                        }

                        if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            if (dd_inv_aprobar.SelectedValue == "1") //Si
                            {
                                // trae la info de la tabla temporal
                                var actividadesTMP = (from actTmp in consultas.Db.ProyectoActividadPOInterventorTMPs
                                                      where actTmp.CodProyecto == int.Parse(CodProyecto) && actTmp.Id_Actividad == Convert.ToInt32(CodActividad)
                                                      select actTmp).FirstOrDefault();

                                //Consulta que exista la info en la tabla definitiva 
                                var actividad = (from a in consultas.Db.ProyectoActividadPOInterventors
                                                 where a.Id_Actividad == actividadesTMP.Id_Actividad
                                                 select a).FirstOrDefault();
                                if (actividad != null)
                                {
                                    //Se actualiza la actividad en tabla definitiva y se elimina la info de la temporal
                                    actividad.Item = (short)actividadesTMP.Item;
                                    actividad.NomActividad = actividadesTMP.NomActividad;
                                    actividad.Metas = actividadesTMP.Metas;

                                    //Se elimina la info de la tabla actividad temporal
                                    txtSQL = "DELETE FROM proyectoactividadPOInterventorTMP Where CodProyecto=" + actividadesTMP.CodProyecto + " and Id_Actividad=" + actividadesTMP.Id_Actividad;
                                    ejecutaReader(txtSQL, 2);
                                    consultas.Db.SubmitChanges();
                                }

                                //Lista de actividadMes en la temporal
                                var actividadMesTMP = (from amt in consultas.Db.ProyectoActividadPOMesInterventorTMPs
                                                       where amt.CodActividad == actividad.Id_Actividad
                                                             && amt.Mes != null
                                                       select amt).ToList();

                                //Lista de actividadMes en la definitiva
                                var actividadMes = (from am in consultas.Db.ProyectoActividadPOMesInterventors
                                                    where am.CodActividad == actividad.Id_Actividad
                                                    select am).ToList();

                                //Se eliminan los datos de actividadMes en la definitiva
                                if (actividadMes.Count > 0)
                                {
                                    txtSQL = "DELETE ProyectoActividadPOMesInterventor where CodActividad=" + actividad.Id_Actividad;
                                    ejecutaReader(txtSQL, 2);
                                }

                                if (actividadMesTMP.Count > 0)
                                {
                                    //Se insertan los nuevos datos a la tabla definitiva desde la temporal
                                    foreach (var amt in actividadMesTMP)
                                    {
                                        var val = amt.Valor.ToString().Split(',');
                                        txtSQL = "Insert into ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor) ";
                                        txtSQL += "values (" + amt.CodActividad + "," + amt.Mes + "," + amt.CodTipoFinanciacion + "," + val[0] + ")";
                                        ejecutaReader(txtSQL, 2);
                                    }

                                    //Se eliminan los registros de la tabla mes temporal
                                    txtSQL = "DELETE ProyectoActividadPOMesInterventorTMP where CodActividad=" + actividadesTMP.Id_Actividad;
                                    ejecutaReader(txtSQL, 2);
                                }

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

                                foreach (var id in idsAgendar)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                    (id,
                                    "Revisar Actividad del Plan Operativo. Se aprobó la modificacion de una actividad.",
                                    "Revisar actividad del plan operativo " + datos[0].nombre + " - Actividad --> " + Session["NombActividad"].ToString() + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                    "Catálogo Actividad Plan Operativo");

                                    agenda.Agendar();
                                }

                                //Finalmente, destruye las variables.
                                correcto = "";
                                CodProyecto = "0";
                                CodActividad = 0;
                                HttpContext.Current.Session["CodActividad"] = null;
                                HttpContext.Current.Session["NomActividad"] = null;
                                HttpContext.Current.Session["CodProyecto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                            }
                            else
                            {
                                var planOperTmp = (from pot in consultas.Db.ProyectoActividadPOInterventorTMPs
                                                   where pot.CodProyecto == int.Parse(CodProyecto) && pot.Id_Actividad == CodActividad
                                                   select pot).FirstOrDefault();
                                txtSQL = "Delete from proyectoactividadPOInterventorTMP where CodProyecto = " + planOperTmp.CodProyecto + " And Id_Actividad = " + planOperTmp.Id_Actividad;
                                ejecutaReader(txtSQL, 2);

                                txtSQL = "Delete from ProyectoActividadPOMesInterventorTMP where CodActividad = " + planOperTmp.Id_Actividad;
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

                                foreach (var id in idsAgendar)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                    (id,
                                    "Revisar Actividad del Plan Operativo. Modificacion de actividad NO aprobada por el Gerente Interventor.",
                                    "Revisar actividad del plan operativo " + datos[0].nombre + " - Actividad --> " + Session["NombActividad"].ToString() + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                    "Catálogo Actividad Plan Operativo");

                                    agenda.Agendar();
                                }

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                            }
                        }
                    }
                    if (usuario.CodGrupo == Constantes.CONST_Interventor) //Si el usuario tiene el código grupo "Interventor".
                    {
                        // Ejecutar como Interventor.
                        string txtSQL = "SELECT CodCoordinador FROM interventor WHERE codcontacto=" + usuario.IdContacto;

                        var dt = consultas.ObtenerDataTable(txtSQL, "text");
                        correcto = dt.Rows[0].ItemArray[0].ToString();
                        if (string.IsNullOrEmpty(correcto))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                            return;
                        }
                        var query = "Select * from proyectoactividadPOInterventorTMP where Id_Actividad = " + CodActividad + " and CodProyecto = " + CodProyecto + " and NomActividad = '" + TB_Actividad.Text + "'";
                        var dt2 = consultas.ObtenerDataTable(query, "text");
                        if (dt2.Rows.Count == 0)
                        {
                            txtSQL = "INSERT INTO proyectoactividadPOInterventorTMP (id_actividad,CodProyecto,Item,NomActividad,Metas,Tarea) " +
                                "values (" + CodActividad + "," + CodProyecto + "," + TB_item.Text + ",'" + TB_Actividad.Text + "','" + TB_metas.Text + "','Modificar')";
                            ejecutaReader(txtSQL, 2);
                        }

                        string mes = "0";
                        string valor = "0";
                        string tipo = "0";

                        int CantidadDatos = 0;

                        // Grabar datos meses
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

                                        txtSQL = "SELECT count(*) as Cantidad FROM ProyectoactividadPOMesInterventorTMP WHERE CodActividad = " + CodActividad + " and mes = " + mes + " and CodTipoFinanciacion = " + tipo;
                                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                                        if (int.Parse(Rs.Rows[0].ItemArray[0].ToString()) > 0)
                                        {
                                            CantidadDatos = 1;
                                        }
                                        if (CantidadDatos == 0)
                                        {
                                            txtSQL = "INSERT INTO ProyectoactividadPOMesInterventorTMP(CodActividad,Mes,CodTipoFinanciacion,Valor) VALUES(" + CodActividad + "," + mes + "," + tipo + ",Cast('" + valor + "' as money))";
                                        }
                                        else
                                        {
                                            txtSQL = "UPDATE ProyectoactividadPOMesInterventorTMP SET Valor =  Cast('" + valor + "' as money) WHERE CodActividad = " + CodActividad + " AND Mes = " + mes + " AND CodTipoFinanciacion = " + tipo;
                                        }
                                        ejecutaReader(txtSQL, 2);
                                        CantidadDatos = 0;
                                    }
                                }
                            }
                        }

                        //Agendar tarea.
                        txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                        var dt3 = consultas.ObtenerDataTable(txtSQL, "text");
                        var idCoord = int.Parse(dt3.Rows[0].ItemArray[0].ToString());

                        AgendarTarea agenda = new AgendarTarea
                        (idCoord,
                        "Actividad del Plan Operativo modificada por Interventor",
                        "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + TB_Actividad.Text + "<br>Observaciones:</br>" + txt_inv_observaciones.Text.Trim(),
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

                        //agenda.Agendar();

                        //Destruir variables.
                        CodProyecto = "0";
                        CodActividad = 0;
                        HttpContext.Current.Session["CodActividad"] = null;
                        HttpContext.Current.Session["NomActividad"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);

                    }

                }
                if (acion == 3) /*Eliminar*/
                {
                    // Si es Gerente Interventor
                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        //Si está aprobado...
                        if (dd_inv_aprobar.SelectedValue == "1") //Si
                        {
                            // Aprobado

                            //BORRA LOS REGISTROS EN LA TABLA DEFINITIVA
                            txtSQL = " DELETE proyectoactividadPOInterventor " +
                                     " where CodProyecto = " + CodProyecto + " and Id_Actividad = " + CodActividad;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            //BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL
                            txtSQL = " DELETE proyectoactividadPOInterventorTMP " +
                                     " where CodProyecto = " + CodProyecto + " and Id_Actividad = " + CodActividad;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            //BORRA LOS REGISTROS EN LA TABLA DEFINITIVA POR MESES
                            txtSQL = " DELETE ProyectoActividadPOMesInterventor " +
                                     " where CodActividad = " + CodActividad;
                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);


                            // BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.
                            txtSQL = " DELETE ProyectoActividadPOMesInterventorTMP " +
                                     " where CodActividad = " + CodActividad;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            //ESTA PENDIENTE PREGUNTAR QUE MÁS DEBE BORRAR ESTANDOO EN EL POPUP Y CON EL GERENTE INTERVENTOR

                            /*
                            * ELIMINA LOS REGISTROS EN LAS TABLAS TEMPORALES
                            */

                            var plan_operativo =
                            string.Format("DELETE [dbo].[ProyectoActividadPOInterventorTMP] WHERE [ChequeoGerente] IS NULL " +
                            "AND [ChequeoCoordinador] = 'true' AND [Tarea]='Borrar' AND [Id_Actividad] = {0} AND [CodProyecto] = {1}",
                            HttpContext.Current.Session["CodActividad"] ?? 0, HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(plan_operativo, 2);

                            var plan_operativo_actividad =
                            string.Format("DELETE FROM [dbo].[ProyectoActividadPOInterventor] WHERE [Id_Actividad] = {0} AND [CodProyecto]={1}",
                            HttpContext.Current.Session["CodActividad"] ?? 0, HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(plan_operativo_actividad, 2);

                            var nomina = string.Format("DELETE [dbo].[InterventorNominaTMP] WHERE [Tarea] = 'Borrar' AND [ChequeoGerente] IS NULL " +
                                "AND [ChequeoCoordinador]='true' AND [Id_Nomina]={0} AND  [CodProyecto] = {1}", HttpContext.Current.Session["CodNomina"] ?? 0,
                                HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(nomina, 2);

                            var produccion = string.Format("DELETE [dbo].[InterventorProduccionTMP] WHERE [ChequeoCoordinador] = 'true' " +
                            "AND [ChequeoGerente] IS NULL AND [ChequeoCoordinador]='true' AND [Tarea]='Borrar' AND [id_produccion] = {0} " +
                            "AND [CodProyecto] = {1} ", HttpContext.Current.Session["CodProducto"] ?? 0, HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(produccion, 2);

                            var ventas = string.Format("DELETE [dbo].[InterventorVentasTMP] WHERE [ChequeoCoordinador]= 1 AND [ChequeoGerente] IS NULL AND " +
                            "[Tarea]='Borrar' AND [ChequeoCoordinador]='true' AND [id_ventas] = {0} AND [CodProyecto] = {1}", HttpContext.Current.Session["CodProducto"] ?? 0,
                            HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(ventas, 2);

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

                            foreach (var id in idsAgendar)
                            {
                                AgendarTarea agenda = new AgendarTarea
                                (id,
                                "Actividad del Plan Operativo Eliminada por Gerente Interventor.",
                                "Revisar actividad del plan operativo " + datos[0].nombre + " - Actividad --> " + Session["NombActividad"].ToString() + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Actividad Plan Operativo");

                                agenda.Agendar();
                            }

                            //Destruir variables.
                            CodProyecto = "0";
                            CodActividad = 0;
                            HttpContext.Current.Session["CodActividad"] = null;
                            HttpContext.Current.Session["NomActividad"] = null;

                            //Actualizar la fecha de modificación del tab.
                            prActualizarTab(txtTab.ToString(), CodProyecto);

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                        }
                        else
                        {
                            var planOperTmp = (from pot in consultas.Db.ProyectoActividadPOInterventorTMPs
                                               where pot.CodProyecto == int.Parse(CodProyecto) && pot.Id_Actividad == CodActividad
                                               select pot).FirstOrDefault();
                            txtSQL = "Delete from ProyectoActividadPOInterventorTMP where CodProyecto =" + planOperTmp.CodProyecto + " And Id_Actividad = " + planOperTmp.Id_Actividad;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "Delete from ProyectoActividadPOMesInterventor Where CodActividad = " + planOperTmp.Id_Actividad;
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

                            foreach (var id in idsAgendar)
                            {
                                AgendarTarea agenda = new AgendarTarea
                                (id,
                                "Actividad del Plan Operativo. Actividad NO Eliminada por Gerente Interventor.",
                                "Revisar actividad del plan operativo " + datos[0].nombre + " - Actividad --> " + Session["NombActividad"].ToString() + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Actividad Plan Operativo");

                                agenda.Agendar();
                            }

                            //Destruir variables.
                            CodProyecto = "0";
                            CodActividad = 0;
                            HttpContext.Current.Session["CodActividad"] = null;
                            HttpContext.Current.Session["NomActividad"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                        }
                    }

                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")
                        {
                            /*
                            * ACTUALIZAR LOS DATOS PARA QUE EL USUARIO GERENTE INTERVENTOR PUEDA ELIMINAR EL CAMBIO
                            */
                            txtSQL = "UPDATE ProyectoActividadPOInterventorTMP SET ChequeoCoordinador = 'true', Tarea= 'Borrar' WHERE Id_Actividad = " + Session["CodActividad"] + " AND CodProyecto = " + Session["CodProyecto"];
                            ejecutaReader(txtSQL, 2);
                            var nomina = string.Format("UPDATE [dbo].[InterventorNominaTMP] SET [Tarea] = 'Borrar', [ChequeoGerente] = NULL, " +
                                "[ChequeoCoordinador]='true' WHERE [Id_Nomina]={0} AND  [CodProyecto] = {1}", HttpContext.Current.Session["CodNomina"] ?? 0, HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(nomina, 2);
                            var produccion = string.Format("UPDATE [dbo].[InterventorProduccionTMP] SET [ChequeoGerente] = NULL," + "[ChequeoCoordinador]='true', [Tarea]='Borrar' WHERE [id_produccion] = {0} AND [CodProyecto] = {1} ", HttpContext.Current.Session["CodProducto"] ?? 0, HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(produccion, 2);
                            var ventas = string.Format("UPDATE [dbo].[InterventorVentasTMP] SET [ChequeoGerente] = NULL, " +
                            "[Tarea]='Borrar', [ChequeoCoordinador]='true' WHERE [id_ventas] = {0} AND [CodProyecto] = {1}", HttpContext.Current.Session["CodProducto"] ?? 0, HttpContext.Current.Session["CodProyecto"] ?? 0);
                            ejecutaReader(ventas, 2);

                            // Agendar Tarea
                            var grupo = (from g in consultas.Db.Grupos
                                         where g.NomGrupo == "Gerente Interventor"
                                         select g).FirstOrDefault();
                            var contacto = (from c in consultas.Db.GrupoContactos
                                            where c.CodGrupo == grupo.Id_Grupo
                                            select c).FirstOrDefault();

                            AgendarTarea agenda = new AgendarTarea
                            (contacto.CodContacto,
                            "Revisar Actividad del plan operativo. Se ha Autorizado la Eliminacion de una actividad.",
                            "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + Session["NombActividad"].ToString() + "<br>Observaciones:</br>" + txt_inv_observaciones.Text.Trim(),
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
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "window.opener.location.reload(); window.close();", true);
                        }
                        else
                        {
                            var planOperTmp = (from poT in consultas.Db.ProyectoActividadPOInterventorTMPs
                                               where poT.CodProyecto == int.Parse(CodProyecto) && poT.Id_Actividad == int.Parse(Session["CodActividad"].ToString())
                                               select poT).FirstOrDefault();

                            txtSQL = "Delete from ProyectoActividadPOInterventorTMP where Id_Actividad = " + planOperTmp.Id_Actividad + " And CodProyecto = " + planOperTmp.CodProyecto;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "Delete from ProyectoActividadPOMesInterventorTMP Where CodActividad = " + planOperTmp.Id_Actividad;
                            ejecutaReader(txtSQL, 2);

                            //Agendar tarea.
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
                            "Revisar Actividad del plan operativo. No se Autoriza la Eliminacion de una actividad.",
                            "Revisar actividad del plan operativo " + txtNomProyecto + " - Actividad --> " + planOperTmp.NomActividad + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
                            CodProyecto,
                            2,
                            "0",
                            true,
                            1,
                            true,
                            false,
                            usuario.IdContacto,
                            "",
                            "",
                            "Traslado Planes");

                            agenda.Agendar();

                            //Destruir variables.
                            CodProyecto = "0";
                            CodActividad = 0;
                            HttpContext.Current.Session["CodActividad"] = null;
                            HttpContext.Current.Session["NomActividad"] = null;

                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                        }
                    }
                }
                else //No es un dato válido.
                {
                    //Destruir variables.
                    CodProyecto = "0";
                    CodActividad = 0;
                    HttpContext.Current.Session["CodActividad"] = null;
                    HttpContext.Current.Session["NomActividad"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                }
            }
            else
            {
                return;
            }
        }

        private void sumar(TextBox textbox)
        {
            //Inicializar variables.
            String textboxID = textbox.ID ?? string.Empty;
            TextBox textFondo;
            TextBox textAporte;
            Label controltext;

            //Se movieron las variables del try para la suma.
            Double suma1 = 0;
            Double suma2 = 0; //Según el FONADE clásico, el valor COSTO lo toma como ENTERO al SUMARLO.

            var labelfondocosto = this.FindControl("labelfondocosto") as Label;
            var labelaportescosto = this.FindControl("labelaportescosto") as Label;
            var L_SumaTotalescosto = this.FindControl("L_SumaTotalescosto") as Label;

            //Details
            int limit = 0;
            if (textboxID.Length == 7)
                limit = 1;
            else
                limit = 2;

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
                    suma1 = Double.Parse(textFondo.Text.Replace("$", "").Replace(".", ""));
                    textFondo.Text = suma1.ToString();
                }

                if (String.IsNullOrEmpty(textAporte.Text.Replace("$", "").Replace(".", "")))
                {
                    suma2 = 0;
                    textAporte.Text = suma2.ToString();
                }
                else
                {
                    suma2 = Double.Parse(textAporte.Text.Replace("$", "").Replace(".", ""));
                    textAporte.Text = suma2.ToString();
                }

                //Con formato
                controltext.Text = "" + (suma1 + suma2).ToString("C");

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
                                            text.Text = int.Parse("0").ToString("C");
                                        }
                                        labelfondocosto.Text = (Convert.ToDouble(labelfondocosto.Text.Replace("$", "").Replace(".", "")) + Convert.ToDouble(text.Text.Replace("$", "").Replace(".", ""))).ToString("C");

                                    }
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
                                            labelaportescosto.Text = (Convert.ToDouble(labelaportescosto.Text.Replace("$", "").Replace(".", "")) + Convert.ToDouble(text.Text.Replace("$", "").Replace(".", ""))).ToString("C");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (L_SumaTotalescosto != null)
                {
                    L_SumaTotalescosto.Text = (Convert.ToDouble(labelaportescosto.Text.Replace("$", "").Replace(".", "")) + Convert.ToDouble(labelfondocosto.Text.Replace("$", "").Replace(".", ""))).ToString("C");
                }
            }
            catch (FormatException) { }
            catch (NullReferenceException) { }
        }

        /// <summary>
        /// Handles the TextChanged event of the TextBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TB_item.Text))
            {
                Session["item"] = TB_item.Text;
            }
            if (!string.IsNullOrEmpty(TB_Actividad.Text))
            {
                Session["actividad"] = TB_Actividad.Text;
            }
            if (!string.IsNullOrEmpty(TB_metas.Text))
            {
                Session["metas"] = TB_metas.Text;
            }
            TextBox textbox = (TextBox)sender;
            sumar(textbox);
        }

        /// <summary>
        /// Handles the Click event of the B_Cancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            //Destruir variables.
            CodProyecto = "0";
            CodActividad = 0;
            HttpContext.Current.Session["CodActividad"] = null;
            HttpContext.Current.Session["NomActividad"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();window.opener.location.reload();", true);
        }

        /// <summary>
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
                        lbl_titulo_PO.Text = "ADICIONAR ACTIVIDAD";
                    }
                }
                if (accion == "Modificar")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        lbl_titulo_PO.Text = "MODIFICAR ACTIVIDAD";
                    }
                }
                if (accion == "Borrar")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        lbl_titulo_PO.Text = "BORRAR ACTIVIDAD";
                    }
                }
            }
            catch { lbl_titulo_PO.Text = "ADICIONAR ACTIVIDAD"; }
        }


        /// <summary>
        /// Eliminars this instance.
        /// </summary>
        /// Eliminar (FONADE clásico como tal no muestra la ejecución de sentencia DELETE)

        private void Eliminar()
        {
            //Inicializar variables.
            String sqlConsulta = "";

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    sqlConsulta = " SELECT DISTINCT CodTipoFinanciacion, Mes, Valor " +
                                  " FROM proyectoactividadPOmesInterventor WHERE CodActividad = " + CodActividad +
                                  " ORDER BY codtipofinanciacion, mes ";
                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    sqlConsulta = " SELECT DISTINCT CodTipoFinanciacion, Mes, Valor " +
                                  " FROM proyectoactividadPOmesInterventor WHERE CodActividad = " + CodActividad +
                                  " ORDER BY codtipofinanciacion, mes";
                }

                //Si ha pasado por la variable y tiene consulta.
                if (sqlConsulta != "")
                {
                    //Cargar la información de los meses...
                }
            }
            catch { }
        }

    }
}