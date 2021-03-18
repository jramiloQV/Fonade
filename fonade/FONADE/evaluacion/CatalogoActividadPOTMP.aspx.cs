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

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// CatalogoActividadPOTMP
    /// </summary>
    
    public partial class CatalogoActividadPOTMP : Base_Page
    {
        public String codProyecto;
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        Int32 CodActividad;
        /// <summary>
        /// Variable para ejecutar consultas SQL.
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
                //Código del proyecto:
                codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

                //Asignar evento JavaScript a TextBox.
                TB_item.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");

                // Procesar la información que proviene de "FramePlanOperativoInterventoria.aspx".

                //Código de la actividad seleccionada:
                CodActividad = HttpContext.Current.Session["CodActividad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodActividad"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodActividad"].ToString()) : 0;

                //En "FramePlanOperativoInterventoria.aspx" se creó otra variable que contiene el Id de la actividad.
                if (CodActividad == 0)
                {
                    //Aquí se obtiene el valor generado en "FramePlanOperativoInterventoria.aspx"
                    CodActividad = HttpContext.Current.Session["NomActividad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NomActividad"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["NomActividad"].ToString()) : 0;

                    //Si ya aquí NO contiene datos, es un error, se recarga la página padre y se cierra esta ventana emergente.
                    if (CodActividad == 0)
                    { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true); }
                }

                //Evaluar la acción a tomar.
                if (HttpContext.Current.Session["Accion"].ToString().Equals("crear"))
                { B_Acion.Width = 100; }
                if (HttpContext.Current.Session["Accion"].ToString().Equals("actualizar") || HttpContext.Current.Session["Accion"].ToString().Equals("Editar"))
                { B_Acion.Text = "Actualizar"; }
                if (HttpContext.Current.Session["Accion"].ToString().Equals("borrar"))
                { B_Acion.Text = "Borrar"; }

                //Llenar el panel.
                llenarpanel();

                //Establecer título.
                CargarTitulo(HttpContext.Current.Session["Accion"].ToString());

                if (CodActividad != 0)
                {
                    // Bloquea los campos dependiendo de la consulta en el campo "ChequeoCoordinador".
                    txtSQL = " SELECT ChequeoCoordinador FROM proyectoactividadPOInterventorTMP " +
                             " where CodProyecto = " + codProyecto + " AND id_Actividad = " + CodActividad;

                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt.Rows.Count > 0)
                    {
                        switch (dt.Rows[0]["ChequeoCoordinador"].ToString())
                        {
                            case "0":
                                break;
                            default:
                                //Inhabilitar campos:
                                TB_item.Enabled = false;
                                TB_Actividad.Enabled = false;
                                TB_metas.Enabled = false;
                                //Inhabilitar panel que contiene la tabla dinámica.
                                P_Meses.Enabled = false;
                                break;
                        }
                        dt = null;
                    }


                    //Buscar los datos.
                    buscarDatos(CodActividad);
                }


            }
            catch (Exception) { }
        }

        /// <summary>
        ///  generar 14 meses.
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
            prorroga = ObtenerProrroga(codProyecto);
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
                    labelFondo.Font.Size = 8;
                    labelFondo.Font.Bold = true;
                    labelAportes.ID = "labelaportes";
                    labelAportes.Text = "Aporte Emprendedor"; //Costo
                    labelAportes.Font.Size = 8;
                    labelAportes.Font.Bold = true;
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

                    //String variable = "Mes" + i;
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
                    totalMeses.Font.Size = 8;

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

        /// <summary>
        /// Buscar los datos de la actividad seleccionada.
        /// </summary>
        /// <param name="actividad">Actividad a consultar</param>
        
        private void buscarDatos(Int32 actividad)
        {
            // Información de la grilla "Actividades en Aprobación".

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            String sqlConsulta;
            DataTable tabla = new DataTable();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                // Consultar la información si es Coordinador o Gerente Interventor.
                sqlConsulta = "SELECT * FROM [ProyectoActividadPOInterventorTMP] WHERE [CodProyecto] = " + codProyecto + "AND [Id_Actividad] = " + actividad;

                tabla = null;
                tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (tabla.Rows.Count > 0)
                {
                    TB_item.Text = tabla.Rows[0]["Item"].ToString();
                    TB_Actividad.Text = tabla.Rows[0]["NomActividad"].ToString();
                    TB_metas.Text = tabla.Rows[0]["Metas"].ToString();
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
                        TB_item.Text = tabla.Rows[0]["Item"].ToString();
                        TB_Actividad.Text = tabla.Rows[0]["NomActividad"].ToString();
                        TB_metas.Text = tabla.Rows[0]["Metas"].ToString();
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
                        TB_item.Text = tabla.Rows[0]["Item"].ToString();
                        TB_Actividad.Text = tabla.Rows[0]["NomActividad"].ToString();
                        TB_metas.Text = tabla.Rows[0]["Metas"].ToString();
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

            try
            {
                var reader = consultas.ObtenerDataTable(sqlConsulta, "text"); 

                if (reader.Rows.Count > 0)
                {
                    foreach (DataRow fila in reader.Rows)
                    {
                        TextBox controltext;
                        var valor_Obtenido = fila.ItemArray[2].ToString();
                        if (valor_Obtenido == "1")
                        {
                            controltext = (TextBox)this.FindControl("Fondoo" + fila.ItemArray[1].ToString());
                        }
                        else
                        {
                            controltext = (TextBox)this.FindControl("Aporte" + fila.ItemArray[1].ToString());
                        }

                        if (controltext != null)
                        {
                            if (string.IsNullOrEmpty(fila.ItemArray[3].ToString()))
                            {
                                controltext.Text = "0";
                            }
                            else
                            {
                                var valor = Double.Parse(fila.ItemArray[3].ToString());
                                controltext.Text = valor.ToString();
                            }
                            sumar(controltext);
                        }
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

        /// <summary>
        /// Handles the Click event of the B_Acion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Acion_Click(object sender, EventArgs e)
        {
            if (B_Acion.Text.Equals("Crear")) alamcenar(1);
            if (B_Acion.Text.Equals("Actualizar")) alamcenar(2);
        }

        private void alamcenar(int acion)
        {
            //Inicializar variables.            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            DataTable Rs = new DataTable();
            String correcto = "";

            if (acion == 1)
            {
                // Guardar la información de plan operativo "guarda en tablas temporales".

                //Si es interventor inserta los registros en tablas temporales para la aprobación del coordinador y el gerente.
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    //Asigna la tarea al coordinador.
                    string txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                    cmd = new SqlCommand(txtSQL, conn);
                    correcto = String_EjecutarSQL(conn, cmd);
                    if (correcto != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                        return;
                    }

                    int ActividadTmp = 1;
                    txtSQL = "select Id_Actividad  from proyectoactividadPOInterventorTMP ORDER BY Id_Actividad DESC";
                    Rs = consultas.ObtenerDataTable(txtSQL, "text");
                    if (Rs.Rows.Count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                        return;
                    }
                    ActividadTmp = Convert.ToInt32(Rs.Rows[0]["Id_Actividad"]) + 1;

                    txtSQL = "Insert into proyectoactividadPOInterventorTMP (id_actividad,CodProyecto,Item,NomActividad,Metas) " +
                        "values (" + ActividadTmp + "," + codProyecto + "," + TB_item.Text + ",'" + TB_Actividad.Text + "','" + TB_metas.Text + "')";

                    ejecutaReader(txtSQL, 2);

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

                                    if (text.ID.StartsWith("Fondo")) //Sueldo
                                    { tipo = "1"; }
                                    if (text.ID.StartsWith("Aporte")) //Sueldo
                                    { tipo = "2"; }

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
                                    "VALUES(" + ActividadTmp + "," + mes + "," + tipo + "," + valor + ")";

                                    ejecutaReader(txtSQL, 2);
                                }
                            }
                        }
                    }


                    //Destruir variables.
                    codProyecto = "0";
                    //Session["CodProyecto"] = null;
                    CodActividad = 0;
                    HttpContext.Current.Session["CodActividad"] = null;
                    HttpContext.Current.Session["NomActividad"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                }


            }
            if (acion == 2)
            {
                // Editar el plan operativo seleccionado.

                //Comprobar si el usuario tiene el código grupo de "Coordinador Interventor" ó "Gerente Interventor".
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {


                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor) //Si el usuario tiene el código grupo "Interventor".
                {
                    string txtSQL = "SELECT CodCoordinador FROM interventor WHERE codcontacto=" + usuario.IdContacto;
                    cmd = new SqlCommand(txtSQL, conn);
                    correcto = String_EjecutarSQL(conn, cmd);
                    if (correcto != "")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                        return;
                    }
                    txtSQL = "INSERT INTO proyectoactividadPOInterventorTMP (id_actividad,CodProyecto,Item,NomActividad,Metas,Tarea) " +
                    "values (" + CodActividad + "," + codProyecto + "," + TB_item.Text + ",'" + TB_Actividad.Text + "','" + TB_metas.Text + "','Modificar')";
                    ejecutaReader(txtSQL, 2);


                    int CantidadDatos = 0;
                    txtSQL = "SELECT count(*) as Cantidad FROM ProyectoactividadPOMesInterventorTMP WHERE CodActividad = " + CodActividad;
                    Rs = consultas.ObtenerDataTable(txtSQL, "text");
                    if (Convert.ToInt32(Rs.Rows[0]["Cantidad"]) >= 1)
                    {
                        CantidadDatos = 1;
                    }

                    string mes = "0";
                    string valor = "0";
                    string tipo = "0";

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
                                        txtSQL = "INSERT INTO ProyectoactividadPOMesInterventorTMP(CodActividad,Mes,CodTipoFinanciacion,Valor) VALUES(" + CodActividad + "," + mes + "," + tipo + "," + valor + ")";
                                    }
                                    if (CantidadDatos == 1)
                                    {
                                        txtSQL = "UPDATE ProyectoactividadPOMesInterventorTMP SET Valor = " + valor + " WHERE CodActividad = " + CodActividad + " Mes = " + mes + " CodTipoFinanciacion = " + tipo;
                                    }
                                    ejecutaReader(txtSQL, 2);
                                }
                            }
                        }
                    }

                    //Destruir variables.
                    codProyecto = "0";
                    //Session["CodProyecto"] = null;
                    CodActividad = 0;
                    HttpContext.Current.Session["CodActividad"] = null;
                    HttpContext.Current.Session["NomActividad"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
                    //                    RedirectPage(false, string.Empty, "cerrar");
                }


            }
            else //No es un dato válido.
            { return; }
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
            //Int32 valor_suma2 = 0;

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
                                    if (L_SumaTotalescosto != null)
                                        L_SumaTotalescosto.Text = (Convert.ToDouble(labelfondocosto.Text)).ToString();
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
                                            //SE HACEN AJUSTES PARA QUE SE SUME Y NO SE CONCATENEN
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
                    //SE HACEN AJUSTES PARA QUE SE SUME Y NO SE CONCATENEN
                    L_SumaTotalescosto.Text = (Convert.ToDouble(labelaportescosto.Text) + Convert.ToDouble(labelfondocosto.Text)).ToString();
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
            codProyecto = "0";
            //Session["CodProyecto"] = null;
            CodActividad = 0;
            HttpContext.Current.Session["CodActividad"] = null;
            HttpContext.Current.Session["NomActividad"] = null;
            //RedirectPage(false, string.Empty, "cerrar");
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location=window.opener.location;window.close();", true);
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
                if (accion == "Eliminar")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        lbl_titulo_PO.Text = "BORRAR ACTIVIDAD";
                    }
                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    lbl_titulo_PO.Text = "Consultar";
                    //Ocultar botón de "actualización".
                    B_Acion.Visible = false;
                    //Inhabilitar campos.
                    TB_item.Enabled = false;
                    TB_Actividad.Enabled = false;
                    TB_metas.Enabled = false;
                    //Inhabilitar panel que contiene la tabla dinámica.
                    P_Meses.Enabled = false;
                }
            }
            catch { lbl_titulo_PO.Text = "ADICIONAR ACTIVIDAD"; }
        }
    }
}