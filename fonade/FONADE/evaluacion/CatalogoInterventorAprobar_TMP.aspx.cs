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
    /// CatalogoInterventorAprobar_TMP
    /// </summary>
    
    public partial class CatalogoInterventorAprobar_TMP : Base_Page
    {
        public String codProyecto;
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        String CodUsuario;
        String CodGrupo;

        /// <summary>
        /// Código de la nómina seleccionada.
        /// </summary>
        public string codNomina;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
                //Obtener la información almacenada en las variables de sesión.
                CodUsuario = usuario.IdContacto.ToString();
                CodGrupo = HttpContext.Current.Session["CodGrupo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodGrupo"].ToString()) ? HttpContext.Current.Session["CodGrupo"].ToString() : "0";
                codNomina = HttpContext.Current.Session["CodNomina"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodNomina"].ToString()) ? HttpContext.Current.Session["CodNomina"].ToString() : "0";
                codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

                //Aplicar para producción.
                llenarpanel();

                if (HttpContext.Current.Session["Accion"].ToString().Equals("crear"))
                {
                    B_Acion.Text = "Crear";
                    lbl_enunciado.Text = "Adicionar";
                }
                if (HttpContext.Current.Session["Accion"].ToString().Equals("actualizar") || HttpContext.Current.Session["Accion"].ToString().Equals("editar") || HttpContext.Current.Session["Accion"].ToString().Equals("consultar"))
                {
                    B_Acion.Text = "Actualizar";
                    lbl_enunciado.Text = "Consultar";
                    L_Item.Visible = false;
                    TB_Item.Visible = false;
                    TB_Item.Text = "Default";
                    BuscarDatos_Nomina();
                    B_Acion.Visible = false;
                }
                if (HttpContext.Current.Session["Accion"].ToString().Equals("borrar"))
                {
                    B_Acion.Text = "Borrar";
                }
            //}
            //catch (Exception) { throw; }
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


            for (int i = 1; i <= 15; i++) //for (int i = 1; i <= 13; i++) = Son 14 meses según el FONADE clásico.
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
                    labelFondo.Text = "Sueldo";
                    labelAportes.ID = "labelaportes";
                    labelAportes.Text = "Prestaciones";
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


                if (i < 15)
                {
                    celdaMeses = new TableCell();
                    celdaFondo = new TableCell();
                    celdaAporte = new TableCell();
                    celdaTotal = new TableCell();


                    celdaMeses.Width = 50;
                    celdaFondo.Width = 50;
                    celdaAporte.Width = 50;
                    celdaTotal.Width = 50;

                    //String variable = "Mes" + i;
                    Label labelMeses = new Label();
                    labelMeses.ID = "Mes" + i;
                    labelMeses.Text = "Mes " + i;
                    labelMeses.Width = 50;
                    TextBox textboxFondo = new TextBox();
                    textboxFondo.ID = "Fondoo" + i;
                    textboxFondo.Width = 50;
                    textboxFondo.TextChanged += new System.EventHandler(TextBox_TextChanged);
                    textboxFondo.AutoPostBack = true;
                    TextBox textboxAporte = new TextBox();
                    textboxAporte.ID = "Aporte" + i;
                    textboxAporte.Width = 50;
                    textboxAporte.TextChanged += new EventHandler(TextBoxAportes_TextChanged);
                    textboxAporte.AutoPostBack = true;
                    Label totalMeses = new Label();
                    totalMeses.ID = "TotalMes" + i;
                    totalMeses.Text = "0.0";
                    totalMeses.Width = 50;

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
                if (i == 15)
                {

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
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
            //if (B_Acion.Text.Equals("Crear")) alamcenar(1);
            //if (B_Acion.Text.Equals("Actualizar")) alamcenar(2);
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
            Int32 valor_suma2 = 0;

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

            if (textAporte.Text.Trim() == "")
            { suma2 = 0; textAporte.Text = suma2.ToString(); }
            else { suma2 = Double.Parse(textAporte.Text); valor_suma2 = Convert.ToInt32(suma2); textAporte.Text = valor_suma2.ToString(); }

            #region Comentarios anteriores NO BORRAR.

            //if (textboxID.Length == 7)
            //{
            //    objeto = "TotalMes" + (textboxID[textboxID.Length - 2]) + (textboxID[textboxID.Length - 1]);

            //    controltext = (Label)this.FindControl(objeto);
            //    string a = controltext.ID;

            //    textFondo = (TextBox)this.FindControl("Sueldo" + (textboxID[textboxID.Length - 2]) + (textboxID[textboxID.Length - 1]));
            //    textAporte = (TextBox)this.FindControl("Prestaciones" + (textboxID[textboxID.Length - 2]) + (textboxID[textboxID.Length - 1]));

            //    if (textAporte.Text.Trim() == "")
            //    { suma2 = 0; textAporte.Text = suma2.ToString(); }
            //    else { suma2 = Double.Parse(textAporte.Text); valor_suma2 = Convert.ToInt32(suma2); textAporte.Text = valor_suma2.ToString(); }
            //}
            //else
            //{
            //    controltext = (Label)this.FindControl(objeto);
            //    string a = controltext.ID;

            //    textFondo = (TextBox)this.FindControl("Sueldo" + (textboxID[textboxID.Length - 1]));
            //    textAporte = (TextBox)this.FindControl("Prestaciones" + (textboxID[textboxID.Length - 1]));

            //    if (textAporte.Text.Trim() == "")
            //    { suma2 = 0; textAporte.Text = suma2.ToString(); }
            //    else { suma2 = Double.Parse(textAporte.Text); valor_suma2 = Convert.ToInt32(suma2); textAporte.Text = valor_suma2.ToString(); }
            //} 

            #endregion


            try
            {
                if (String.IsNullOrEmpty(textFondo.Text))
                { suma1 = 0; textFondo.Text = suma1.ToString(); }
                else
                { suma1 = Double.Parse(textFondo.Text); textFondo.Text = suma1.ToString(); }

                if (String.IsNullOrEmpty(textAporte.Text))
                { suma2 = 0; textAporte.Text = suma2.ToString(); }
                else { suma2 = Double.Parse(textAporte.Text); valor_suma2 = Convert.ToInt32(suma2); textAporte.Text = valor_suma2.ToString(); }

                //Con formato
                //controltext.Text = "$" + (suma1 + suma2).ToString("0,0.00", CultureInfo.InvariantCulture);
                controltext.Text = "" + (suma1 + valor_suma2);

                labelfondocosto.Text = "0";


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
                                    { if (text.Text.Trim() == "") { text.Text = "0"; } labelfondocosto.Text = (Convert.ToDouble(labelfondocosto.Text) + Convert.ToDouble(text.Text)).ToString(); }
                                    if (L_SumaTotalescosto != null)
                                        L_SumaTotalescosto.Text = (Convert.ToDouble(labelfondocosto.Text)).ToString();
                                }
                            }
                        }
                    }
                }


            }
            catch (FormatException) { }
            catch (NullReferenceException) { }


        }

        private void sumarAporte(TextBox textbox, string param_opcional = null)
        {
            String textboxID = textbox.ID;


            var labelfondocosto = this.FindControl("labelfondocosto") as Label;
            var labelaportescosto = this.FindControl("labelaportescosto") as Label;
            var L_SumaTotalescosto = this.FindControl("L_SumaTotalescosto") as Label;

            int limit = 0;
            if (textboxID.Length == 7)
                limit = 1;
            else
                limit = 2;

            String objeto = "TotalMes" + textboxID.Substring(6, limit);

            Label controltext;
            controltext = (Label)this.FindControl(objeto);

            TextBox textFondo;
            textFondo = (TextBox)this.FindControl("Fondoo" + textboxID.Substring(6, limit)); //Sueldo
            TextBox textAporte;
            textAporte = (TextBox)this.FindControl("Aporte" + textboxID.Substring(6, limit)); //Prestaciones
            try
            {
                Double suma1;
                Double suma2;

                if (String.IsNullOrEmpty(textFondo.Text))
                    suma1 = 0;
                else
                    suma1 = Double.Parse(textFondo.Text);

                if (String.IsNullOrEmpty(textAporte.Text))
                    suma2 = 0;
                else
                    suma2 = Double.Parse(textAporte.Text);



                if (!String.IsNullOrEmpty(param_opcional.Trim()))
                {
                    //Tratamiento para los Productos en "Ventas".
                    Int32 suma_convertida_1 = Convert.ToInt32(suma1);
                    double suma_convertida_2 = Math.Floor(suma2);
                    double valor = suma_convertida_1 + suma_convertida_2;
                    controltext.Text = "" + valor;
                }
                else
                {
                    controltext.Text = "" + (suma1 + suma2);
                }

                labelaportescosto.Text = "0";
                //Int32 cantidades = 0;

                foreach (TableRow fila in T_Meses.Rows)
                {
                    //cantidades = 0;
                    foreach (TableCell celda in fila.Cells)
                    {
                        foreach (Control control in celda.Controls)
                        {
                            try
                            {

                            }
                            catch (Exception) { }
                        }
                    }
                }

                //foreach (Control miControl in T_Meses.Controls)
                //{
                //    var tablerow = miControl.Controls;

                //    foreach (Control micontrolrows in tablerow)
                //    {
                //        var hijos = micontrolrows.Controls;

                //        foreach (Control chijos in hijos)
                //        {
                //            if (chijos.GetType() == typeof(TextBox))
                //            {
                //                var text = chijos as TextBox;

                //                if (text.ID.StartsWith(Prestaciones))
                //                {
                //                    if (labelaportescosto != null)
                //                        labelaportescosto.Text = (Convert.ToDouble(labelaportescosto.Text) + Convert.ToDouble(text.Text)).ToString();

                //                    if (L_SumaTotalescosto != null)
                //                        L_SumaTotalescosto.Text = (Convert.ToDouble(L_SumaTotalescosto.Text) + (Convert.ToDouble(labelaportescosto.Text))).ToString();
                //                }
                //            }
                //        }
                //    }
                //}


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
        /// TextboxChanged para Aportes/Prestaciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TextBoxAportes_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = (TextBox)sender;
            sumarAporte(textbox, "0");
        }

        /// <summary>
        /// Cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Cancelar_Click(object sender, EventArgs e)
        {
            RedirectPage(false, string.Empty, "cerrar");
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 09/04/2014.
        /// Modificar la información del valor seleccionado en la grilla de "Nómina".
        /// La consulta que contiene este método es aplicable sólo a los elemento de nómina por aprobar.
        /// </summary>
        private void BuscarDatos_Nomina()
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            String sqlConsulta;
            //String _tarea = "";
            String _ChequeoCoordinador = "";
            String _ChequeoGerente = "";
            DataTable tabla_sql = new DataTable();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor)
            {
                #region Cargar el nombre del elemento seleccionado.

                sqlConsulta = "SELECT * FROM InterventorNominaTMP WHERE id_Nomina =" + codNomina;

                tabla_sql = consultas.ObtenerDataTable(sqlConsulta, "text");

                if (tabla_sql.Rows.Count > 0)
                {
                    if (tabla_sql.Rows.Count > 0)
                    {
                        #region Aprobación de cambios del coordinador.
                        if (!String.IsNullOrEmpty(_ChequeoCoordinador))
                        {
                            if (_ChequeoCoordinador == "True" || _ChequeoCoordinador == "1")
                            { /* dd_inv_aprobar.SelectedValue = "1";*/ }
                        }
                        else
                        { /*dd_inv_aprobar.SelectedValue = "0";*/ }
                        #endregion

                        #region Aprobación de cambios del gerente.
                        try
                        {
                            if (!String.IsNullOrEmpty(_ChequeoCoordinador))
                            {
                                if (_ChequeoCoordinador == "True" || _ChequeoCoordinador == "1")
                                { AprobarGerente.Checked = Convert.ToBoolean(_ChequeoGerente); }
                            }
                            else
                            { AprobarGerente.Checked = false; }
                        }
                        catch { AprobarGerente.Checked = false; }
                        #endregion

                        //Cargar la información en los controles del formulario.
                        TB_Item.Text = tabla_sql.Rows[0]["Cargo"].ToString();
                    }
                }
                else
                {
                    //Tarea ya aprobada (tendría que salir un botón para cerrar la ventana actual...)...
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');this.window.close();", true);
                    return;
                }

                tabla_sql = null;

                #endregion

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    sqlConsulta = "select * from InterventorNominaMesTMP where CodCargo = " + codNomina + " order by Mes";

                    var reader = consultas.ObtenerDataTable(sqlConsulta, "text"); // cmd.ExecuteReader();
                    if (reader.Rows.Count > 0)
                    {
                        foreach (DataRow fila in reader.Rows)
                        {
                            TextBox controltext;
                            var valor_Obtenido = fila.ItemArray[3].ToString();

                            if (valor_Obtenido.Trim() == "1")
                            {
                                controltext = (TextBox)this.FindControl("Fondoo" + fila.ItemArray[1].ToString());
                                if (controltext != null)
                                {
                                    controltext.Text = fila.ItemArray[2].ToString();
                                    sumar(controltext);
                                }
                                
                            }
                            else
                            {
                                controltext = (TextBox)this.FindControl("Aporte" + fila.ItemArray[1].ToString());

                                if (controltext != null)
                                {
                                    if (String.IsNullOrEmpty(fila.ItemArray[2].ToString()))
                                    {
                                        controltext.Text = "0";
                                        sumarAporte(controltext);//aaa
                                    }
                                    else
                                    {
                                        Double valor = Double.Parse(fila.ItemArray[2].ToString());
                                        if (controltext != null)
                                        {
                                            controltext.Text = valor.ToString();
                                            sumarAporte(controltext, codNomina);
                                        }

                                    }
                                }
                                
                            }
                        }
                    }

                    //SqlCommand cmd = new SqlCommand(sqlConsulta, conn);
                    //try
                    //{
                    //    conn.Open();
                        
                    //    while (reader.Read())
                    //    {
                    //        TextBox controltext;
                    //        //TextBox costoTotal;
                    //        string valor_Obtenido = reader["Tipo"].ToString();//.Equals("1");

                    //        if (valor_Obtenido.Trim() == "1")
                    //        {
                    //            controltext = (TextBox)this.FindControl("Fondoo" + reader["Mes"].ToString());
                    //            controltext.Text = reader["Valor"].ToString();
                    //            sumar(controltext);
                    //        }
                    //        else
                    //        {
                    //            controltext = (TextBox)this.FindControl("Aporte" + reader["Mes"].ToString());

                    //            if (String.IsNullOrEmpty(reader["Valor"].ToString()))
                    //            {
                    //                controltext.Text = "0";
                    //                sumarAporte(controltext);//aaa
                    //            }
                    //            else
                    //            {
                    //                Double valor = Double.Parse(reader["Valor"].ToString());
                    //                controltext.Text = valor.ToString();
                    //                sumarAporte(controltext, codNomina);
                    //            }
                    //        }
                    //    }
                    //}
                    //catch (SqlException se)
                    //catch (Exception se)
                    //{
                    //    string h = se.Message;
                    //    throw se;
                    //}
                    //finally
                    //{
                    //    conn.Close();
                    //    conn.Dispose();
                    //    cmd.Dispose();
                    //}
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/04/2014. Se crea este método para llamar por separado a las consultas.
        /// </summary>
        /// <param name="sqlConsulta">Consukta SQL.</param>
        /// <param name="connection">Conexión.</param>
        /// <summary>      
        private void Ejecutar(string sqlConsulta, SqlConnection connection)
        {
            //Obtiene la conexión
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            SqlCommand cmd = new SqlCommand(sqlConsulta, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TextBox controltext;
                    //TextBox costoTotal;
                    if (reader["Tipo"].ToString().Equals("1")) //CodTipoFinanciacion
                    {
                        controltext = (TextBox)this.FindControl("Fondoo" + reader["Mes"].ToString());
                    }
                    else
                    {
                        controltext = (TextBox)this.FindControl("Aporte" + reader["Valor"].ToString());
                    }
                    controltext.Text = reader["Valor"].ToString();
                    sumar(controltext);
                }
                //connection.Close();
            }
            catch (Exception se) //SqlException
            {
                string h = se.Message;
                throw se;
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                cmd.Dispose();
            }
        }
    }
}