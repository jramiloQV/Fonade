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
    public partial class CatalogoVentasAprobar_TMP : Base_Page
    {
        public String codProyecto;
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        String CodUsuario;
        String CodGrupo;

        /// <summary>
        /// Código del producto (producción) seleccionado.
        /// </summary>
        public string codProduccion;

        /// <summary>
        /// Usado para determinar si hace la búsqueda en la tabla TMP o normal.
        /// Ej: si este valor es != null, hace la consulta en "InterventorVentasMesTMP", de lo contrario
        /// consultará en "InterventorVentasMes".
        /// </summary>
        public string ValorTMP;

        /// <summary>
        /// Variable que DEBE pasarse por sesión para revisar si el valor está o no aprobado.
        /// Por defecto, en el Page_Load, se dejará 0 si NO ENCUENTRA NADA.
        /// </summary>
        public int s_valorAprobado;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Obtener la información almacenada en las variables de sesión.
                CodUsuario = usuario.IdContacto.ToString();
                CodGrupo = HttpContext.Current.Session["CodGrupo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodGrupo"].ToString()) ? HttpContext.Current.Session["CodGrupo"].ToString() : "0";
                codProduccion = HttpContext.Current.Session["CodProducto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProducto"].ToString()) ? HttpContext.Current.Session["CodProducto"].ToString() : "0";
                ValorTMP = HttpContext.Current.Session["ValorTMP"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ValorTMP"].ToString()) ? HttpContext.Current.Session["ValorTMP"].ToString() : "0";
                codProyecto = HttpContext.Current.Session["proyecto"].ToString() != null && !string.IsNullOrEmpty(HttpContext.Current.Session["proyecto"].ToString()) ? HttpContext.Current.Session["proyecto"].ToString() : "0"; ;
                s_valorAprobado = HttpContext.Current.Session["s_valorAprobado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["s_valorAprobado"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["s_valorAprobado"].ToString()) : 0;
            }
            catch (Exception) { throw; }
            try{
                //Aplicar para producción.
                llenarpanel();
            }
            catch (Exception) { throw; }
            try { 
                if (HttpContext.Current.Session["Accion"].ToString().Equals("crear")) {B_Acion.Text = "Crear"; lbl_enunciado.Text = "Adicionar"; }
                else if (HttpContext.Current.Session["Accion"].ToString().Equals("actualizar")) { B_Acion.Text = "Actualizar";  lbl_enunciado.Text = "Consultar"; L_Item.Visible = false; TB_Item.Visible = false; TB_Item.Text = "Default"; BuscarDatos_Ventas(); B_Acion.Visible = false; }
                else if (HttpContext.Current.Session["Accion"].ToString().Equals("borrar")) {B_Acion.Text = "Borrar"; }
            }
            catch (Exception) { throw; }
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
                    labelFondo.Text = "Ventas";
                    labelAportes.ID = "labelaportes";
                    labelAportes.Text = "Ingreso";
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
            if (B_Acion.Text.Equals("Crear")) alamcenar(1);
            if (B_Acion.Text.Equals("Actualizar") || B_Acion.Text.Equals("actualizar")) alamcenar(2);
        }

        /// <summary>
        /// Guardar y/o actualizar la información.
        /// </summary>
        /// <param name="acion">Si el valor es 1, guardará la información, si es 2, actualizará dicha información.</param>
        private void alamcenar(int acion)
        {
            //Inicializar variables.            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();

            if (acion == 1)
            {
                #region Guardar la información de Nómina. = SIN CONSULTAS.



                #endregion
            }
            if (acion == 2)
            {
                #region Editar la nómina seleccionada.

                //Comprobar si el usuario tiene el código grupo de "Coordinador Interventor" ó "Gerente Interventor".
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    //Consulta los valores temporales.
                    cmd = new SqlCommand("SELECT * FROM [InterventorVentasTMP] WHERE [id_Produccion] = " + codProduccion, conn);

                    //Consulta los valores reales.
                    //cmd = new SqlCommand("SELECT * FROM [InterventorVentas] WHERE [id_Produccion] = " + codProduccion, conn);

                    //Consulta los "costos".
                    //cmd = new SqlCommand("SELECT * FROM [InterventorVentasMesTMP] WHERE [id_Produccion] = " + codProduccion + " ORDER BY [Mes] ", conn);

                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor) //Si el usuario tiene el código grupo "Interventor".
                {
                    //Consulta los valores reales.
                    cmd = new SqlCommand("SELECT * FROM [InterventorVentas] WHERE [id_Produccion] = " + codProduccion, conn);

                    //Consulta los "costos".
                    //cmd = new SqlCommand("SELECT * FROM [InterventorVentasMes] WHERE [id_Produccion] = " + codProduccion + " ORDER BY [Mes] ", conn);
                }

                #endregion
            }
            else //No es un dato válido.
            { return; }

            #region Código anterior comentado. NO BORRAR.

            //String item = TB_Item.Text;

            //String idGuardar = "";
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            //if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //{
            //    SqlCommand cmd = new SqlCommand("SELECT [CodCoordinador] FROM [Interventor] WHERE [CodContacto] = " + CodUsuario, conn);

            //    try
            //    {
            //        conn.Open();
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        reader.Read();
            //        String codCordinador = reader["CodCoordinador"].ToString();
            //        reader.Close();
            //        conn.Close();

            //        if (String.IsNullOrEmpty(codCordinador))
            //        {
            //            System.Windows.Forms.MessageBox.Show("No tiene ningún coordinador asignado.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            //            return;
            //        }
            //        else
            //        {
            //            conn.Open();
            //            cmd = new SqlCommand("SELECT MAX([Id_Actividad]) Id_Actividad FROM [ProyectoActividadPOInterventorTMP]", conn);
            //            reader = cmd.ExecuteReader();
            //            reader.Read();
            //            if (reader.FieldCount != 0)
            //            {
            //                if (String.IsNullOrEmpty(reader["Id_Actividad"].ToString())) idGuardar = "" + 1;
            //                else idGuardar = "" + (Int64.Parse(reader["Id_Actividad"].ToString()) + 1 + Int64.Parse(codProyecto));
            //            }

            //        }
            //    }
            //    catch (SqlException se)
            //    {
            //        throw se;
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //}

            //string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            //using (var con = new SqlConnection(conexionStr))
            //{
            //    using (var com = con.CreateCommand())
            //    {
            //        com.CommandText = "MD_Insertar_Actualizar_ProyectoActividadPO";
            //        com.CommandType = System.Data.CommandType.StoredProcedure;

            //        if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //        {
            //            if (acion == 1) com.Parameters.AddWithValue("@_Id_Actividad", 0); //idGuardar);
            //            if (acion == 2) com.Parameters.AddWithValue("@_Id_Actividad", CodActividad);
            //        }
            //        else
            //        {
            //            if (acion == 1) com.Parameters.AddWithValue("@_Id_Actividad", 0);
            //            if (acion == 2) com.Parameters.AddWithValue("@_Id_Actividad", CodActividad);
            //        }

            //        //com.Parameters.AddWithValue("@_NomActividad", actividad);
            //        com.Parameters.AddWithValue("@_CodProyecto", codProyecto);
            //        com.Parameters.AddWithValue("@_CodGrupo", usuario.CodGrupo);

            //        if (acion == 1)
            //        {
            //            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //            {
            //                com.Parameters.AddWithValue("@_caso", "CREATEINTERVENTOR");
            //            }
            //            else
            //            {
            //                com.Parameters.AddWithValue("@_caso", "CREATE");
            //            }
            //        }
            //        if (acion == 2)
            //        {
            //            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //            {
            //                com.Parameters.AddWithValue("@_caso", "UPDATEINTERVENTOR");
            //            }
            //            else
            //            {
            //                com.Parameters.AddWithValue("@_caso", "UPDATE");
            //            }
            //        }
            //        try
            //        {
            //            con.Open();
            //            com.ExecuteReader();
            //        }
            //        catch (Exception ex)
            //        {
            //            throw ex;
            //        }
            //        finally
            //        {
            //            con.Close();
            //        }
            //    }
            //}

            //String RsActividad = "";
            ////textbox
            //if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //{
            //    RsActividad = idGuardar;
            //}
            //else
            //{
            //    //SqlCommand cmd = new SqlCommand("SELECT [Id_Actividad] FROM [ProyectoActividadPO] WHERE [NomActividad] = " + actividad + " AND [CodProyecto] = " + codProyecto, conn);
            //    SqlCommand cmd = new SqlCommand("SELECT [Id_Actividad] FROM [ProyectoActividadPO] WHERE [CodProyecto] = " + codProyecto + " ");

            //    try
            //    {
            //        conn.Open();
            //        SqlDataReader reader = cmd.ExecuteReader();
            //        reader.Read();
            //        RsActividad = reader["CodCoordinador"].ToString();
            //        reader.Close();
            //    }
            //    catch (SqlException se)
            //    {
            //        throw se;
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }
            //}

            #endregion

            #region MD_Insertar_Actualizar_ProyectoActividadPOMes COMENTADO.

            //using (var con = new SqlConnection(conexionStr))
            //{
            //    using (var com = con.CreateCommand())
            //    {
            //        com.CommandText = "MD_Insertar_Actualizar_ProyectoActividadPOMes";
            //        com.CommandType = System.Data.CommandType.StoredProcedure;

            //        for (int j = 1; j <= 2; j++)
            //        {
            //            for (int i = 1; i <= 12; i++)
            //            {
            //                Label controltext;
            //                controltext = (Label)this.FindControl("TotalMes" + i);


            //                if (acion == 1) com.Parameters.AddWithValue("@CodActividad", RsActividad);
            //                if (acion == 2) com.Parameters.AddWithValue("@CodActividad", CodActividad);

            //                com.Parameters.AddWithValue("@Mes", i);
            //                com.Parameters.AddWithValue("@CodTipoFinanciacion", j);
            //                com.Parameters.AddWithValue("@Valor", controltext.Text);

            //                if (acion == 1)
            //                {
            //                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //                    {
            //                        com.Parameters.AddWithValue("@_caso", "CREATEINTERVENTOR");
            //                    }
            //                    else
            //                    {
            //                        com.Parameters.AddWithValue("@_caso", "CREATE");
            //                    }
            //                }
            //                if (acion == 2)
            //                {
            //                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
            //                    {
            //                        com.Parameters.AddWithValue("@_caso", "UPDATEINTERVENTOR");
            //                    }
            //                    else
            //                    {
            //                        com.Parameters.AddWithValue("@_caso", "UPDATE");
            //                    }
            //                }

            //                try
            //                {
            //                    con.Open();
            //                    com.ExecuteReader();
            //                }
            //                catch (Exception ex)
            //                {
            //                    throw ex;
            //                }
            //                finally
            //                {
            //                    con.Close();
            //                }
            //            }
            //        }
            //    }
            //}

            #endregion
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
                Int32 cantidades = 0;

                foreach (TableRow fila in T_Meses.Rows)
                {
                    cantidades = 0;
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
        /// Modificar la información del valor seleccionado en la grilla de "Ventas".
        /// La consulta que contiene este método es aplicable sólo a los productos en ventas por aprobar.
        /// </summary>
        private void BuscarDatos_Ventas()
        {
            //Obtiene la conexión
            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            //Inicializa la variable para generar la consulta.
            String sqlConsulta;

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                sqlConsulta = " SELECT * FROM [InterventorVentasMesTMP] " +
                              " WHERE [InterventorVentasMesTMP].[CodProducto] = " + codProduccion + " " +
                              " ORDER BY [InterventorVentasMesTMP].[Tipo], " +
                              " [InterventorVentasMesTMP].[Mes] ";
            }
            else
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    sqlConsulta = " SELECT * FROM [InterventorVentasMesTMP] " +
                              " WHERE [InterventorVentasMesTMP].[CodProducto] = " + codProduccion + " " +
                              " ORDER BY [InterventorVentasMesTMP].[Tipo], " +
                              " [InterventorVentasMesTMP].[Mes] ";

                    var dt = consultas.ObtenerDataTable(sqlConsulta, "text");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow fila in dt.Rows)
                        {
                            TextBox controltext;
                            //TextBox costoTotal;

                            var valor_Obtenido = fila["Tipo"].ToString();

                            if (valor_Obtenido == "1")
                            {
                                controltext = (TextBox)this.FindControl("Fondoo" + fila["Mes"].ToString());
                                if (controltext != null)
                                {
                                    controltext.Text = fila["Valor"].ToString();
                                    sumar(controltext);
                                }
                            }
                            else
                            {
                                controltext = (TextBox)this.FindControl("Aporte" + fila["Mes"].ToString());

                                if(controltext != null)
                                {
                                    if (String.IsNullOrEmpty(fila["Valor"].ToString()))
                                    {
                                        controltext.Text = "0";
                                        sumarAporte(controltext);//aaa
                                    }
                                    else
                                    {
                                        Double valor = Double.Parse(fila["Valor"].ToString());
                                        controltext.Text = valor.ToString();
                                        sumarAporte(controltext, codProduccion);
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        #region Comentarios del método anterior NO BORRAR.
        //{
        //    //Obtiene la conexión
        //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

        //    //Inicializa la variable para generar la consulta.
        //    String sqlConsulta;

        //    if (ValorTMP.Trim() != codProduccion)
        //    {
        //        #region Consultas si los registros están aprobados.

        //        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
        //        {
        //            sqlConsulta = "SELECT * FROM [InterventorVentasMes] WHERE [CodProducto] = " + codProduccion + " ORDER BY Mes";

        //            //Ejecutar consulta SQL.
        //            Ejecutar(sqlConsulta, conn);
        //        }
        //        if (usuario.CodGrupo == Constantes.CONST_Interventor && ValorTMP != null)
        //        {
        //            sqlConsulta = "SELECT * FROM [InterventorVentasMes] WHERE [CodProducto] = " + codProduccion + " ORDER BY Mes";

        //            //Ejecutar consulta SQL.
        //            Ejecutar(sqlConsulta, conn);
        //        }

        //        #endregion
        //    }
        //    else
        //    {
        //        #region Consultas de registros NO están aprobados "según CatalogoProduccionAprobar_TMP.asp".

        //        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
        //        {
        //            sqlConsulta = "SELECT * FROM [InterventorVentasMesTMP] WHERE [CodProducto] = " + codProduccion + " ";

        //            //Ejecutar consulta SQL.
        //            Ejecutar(sqlConsulta, conn);
        //        }
        //        if (usuario.CodGrupo == Constantes.CONST_Interventor)
        //        {
        //            sqlConsulta = "SELECT * FROM [InterventorVentasMesTMP] WHERE [CodProducto] = " + codProduccion + " ";

        //            //Ejecutar consulta SQL.
        //            Ejecutar(sqlConsulta, conn);
        //        }

        //        #endregion
        //    }
        //} 
        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/04/2014. Se crea este método para llamar por separado a las consultas.
        /// </summary>
        /// <param name="sqlConsulta">Consukta SQL.</param>
        /// <param name="connection">Conexión.</param>
        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/04/2014. Se crea este método para llamar por separado a las consultas.
        /// </summary>
        /// <param name="sqlConsulta">Consukta SQL.</param>
        /// <param name="connection">Conexión.</param>
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
                   // TextBox costoTotal;
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
                connection.Close();
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
            }
        }

        #region Métodos creados el 16/04/2014.

        /// <summary>
        /// Crear el producto, que lea los valores de la tabla.
        /// </summary>
        private void CrearProductos()
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            bool procesado = false;
            int codProduccion_Autoincr = 1;
            int i = 1;

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar la información.

                    //Consultar si tiene Coordinador asignado.
                    var result = (from t in consultas.Db.Interventors
                                  where t.CodContacto == usuario.IdContacto
                                  select new { t.CodCoordinador }).FirstOrDefault();

                    if (result.CodCoordinador == 0) //No existe
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true);
                        return;
                    }
                    else
                    {
                        #region Ejecutar sentencias de inserción SQL.

                        //Consultar el id_produccion para autoincrementarlo y usarlo en la inserción de datos.
                        var result_produccion_autoincremental = (from t in consultas.Db.InterventorVentasTMPs
                                                                 select new { t.id_ventas }).OrderByDescending(x => x.id_ventas);

                        if (result_produccion_autoincremental.Count() == 0)
                        {
                            ///No hay valores, por lo que se debe usar un 1 como valor autoincremental (Id_del nuevo producto.)
                            ///Es decir, se emplea la variable "codProduccion_Autoincr".
                        }
                        else
                        {
                            //Incrementar valor.
                            codProduccion_Autoincr = result_produccion_autoincremental.First().id_ventas + 1;

                            //Ejecutar Insert #1.
                            sqlConsulta = "INSERT INTO InterventorVentasTMP (id_Produccion, CodProyecto, NomProducto) " +
                                          "VALUES (" + codProduccion_Autoincr + ", " + codProyecto + ", 'NOMBREDELPRODUCTO') "; //Cambiar por el verdadero Nombre del producto.

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            if (procesado) //Si es TRUE, el proceso debe seguir normal, si no sale nada, toca revisar el código.
                            {
                                #region Recorrer la tabla para agregarle los valores, mientras se hace esta inserción.

                                if (i == 1)//Si en el ciclo, el valor es == 1 se hace esta inserción
                                {
                                    //Ejecutar Insert de tipo 1.
                                    sqlConsulta = "INSERT INTO InterventorVentasMesTMP(CodProducto, Mes, Valor, Tipo) " +
                                                  "VALUES (" + codProduccion_Autoincr + ", " + 1 + ", " + 0 + ", 1) ";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);
                                }
                                else
                                {
                                    //Ejecutar Insert de tipo 2.
                                    sqlConsulta = "INSERT INTO InterventorVentasMesTMP(CodProducto, Mes, Valor, Tipo) " +
                                                  "VALUES (" + codProduccion_Autoincr + ", " + 1 + ", " + 0 + ", 2) ";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);
                                }

                                #endregion

                                //prTareaAsignarCoordinadorProduccion = En FONADE Clásico está comentado, por lo tanto no está implementado aquí.
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el producto en InterventorVentasTMP.')", true);
                                return;
                            }
                        }

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    //Según CatalogoProduccionTMP.asp, sólo aplica para el grupo "Interventor" (cerrar ventana).
                    //Cerrar la ventana.
                    ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                    return;
                }
            }
            catch (Exception)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: No se pudo crear el producto.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/04/2014.
        /// Adicionar el producto seleccionado.
        /// Adaptando exactamente el código fuente de CatalogoProduccionTMP.asp.
        /// </summary>
        private void AdicionarProductos(int Valor_AprobadoGerente)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            bool procesado = false;
            int ProductoSeleccionado = 0;
            int codProyectoConvertido = 0;

            try { ProductoSeleccionado = Convert.ToInt32(codProduccion.ToString()); }
            catch { return; }

            try { codProyectoConvertido = Convert.ToInt32(codProyecto.ToString()); }
            catch { return; }

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Procesar como Gerente Interventor.

                    if (Valor_AprobadoGerente == 1) // Si está aprobado por el gerente
                    {
                        #region Sí fue aprobado por el gerente, por esto, continúa el flujo.

                        #region Trae los registros de la tabla temporal. (Obtiene los datos NomProducto // id_produccion).

                        var cod_cargo_linq = (from t in consultas.Db.InterventorVentasTMPs
                                              where t.CodProyecto == codProyectoConvertido
                                              && t.id_ventas == ProductoSeleccionado
                                              select new
                                              {
                                                  t.NomProducto,
                                                  t.id_ventas
                                              }).FirstOrDefault();

                        #endregion

                        if (cod_cargo_linq.id_ventas > 0)
                        {
                            //Generación de datos.
                            string nom_producto_obtenido = cod_cargo_linq.NomProducto;
                            int Id_Produccion_Seleccionado = cod_cargo_linq.id_ventas;

                            #region Insertar los nuevos registros en la tabla definitiva.

                            sqlConsulta = "INSERT INTO InterventorVentas (CodProyecto, NomProducto) " +
                                          "VALUES (" + codProyectoConvertido + ", '" + nom_producto_obtenido + "') ";

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #region Borrar el registro ingresado en la tabla temporal.

                            sqlConsulta = "DELETE FROM InterventorVentasTMP" +
                                          "WHERE CodProyecto = " + codProyectoConvertido + " AND id_Produccion = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #region Traer el código de la actividad para adicionarlo a la tabla definitiva por mes.

                            var cods_int_produccion = (from s in consultas.Db.InterventorVentas
                                                       orderby s.id_ventas descending
                                                       select new
                                                       {
                                                           s.id_ventas
                                                       }).FirstOrDefault();

                            int cod_produccion_sql2 = cods_int_produccion.id_ventas;

                            #endregion

                            #region Trae los registros de la tabla temporal por meses.

                            var sql_3 = (from v in consultas.Db.InterventorVentasMesTMPs
                                         where v.CodProducto == ProductoSeleccionado
                                         select new
                                         {
                                             v.Mes,
                                             v.Valor,
                                             v.Tipo
                                         }).FirstOrDefault();

                            int? Mes_sql_3 = sql_3.Mes;
                            decimal? Valor_sql_3 = sql_3.Valor;
                            int? Tipo_sql_3 = sql_3.Tipo;

                            #endregion

                            #region Inserta los nuevos registros en la tabla definitiva por meses

                            for (int i = 0; i < 14; i++)
                            {
                                if (Tipo_sql_3 == 1) // Si el tipo es 1, genera este flujo.
                                {
                                    //Registra con tipo 1.
                                    sqlConsulta = " INSERT INTO InterventorVentasMes (CodProducto, Mes, Valor, Tipo) " +
                                                  " VALUES (" + ProductoSeleccionado + ", " + Mes_sql_3 + ", " + Valor_sql_3 + ", 1)";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);
                                }
                                else
                                {

                                    //Registra con tipo 2.
                                    sqlConsulta = " INSERT INTO InterventorVentasMes (CodProducto, Mes, Valor, Tipo) " +
                                                  " VALUES (" + ProductoSeleccionado + ", " + Mes_sql_3 + ", " + Valor_sql_3 + ", 2)";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);
                                }
                            }

                            #endregion

                            #region Borrar el registro de la tabla temporal por meses.

                            sqlConsulta = "DELETE FROM InterventorVentasMesTMP" +
                                          "WHERE CodProducto = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion
                        }
                        else
                        {
                            //No se obtuvieron datos.
                            return;
                        }

                        #endregion
                    }
                    if (Valor_AprobadoGerente == 0) //No está aprobado.
                    {
                        #region Se procesa la información según el archivo "CatalogoProduccionTMP.asp" de FONADE clasico.

                        //Se devuelve al interventor, se le avisa al coordinador.
                        var return_interventor = (from emp_int in consultas.Db.EmpresaInterventors
                                                  join emp in consultas.Db.Empresas
                                                  on emp_int.CodEmpresa equals emp.id_empresa
                                                  where emp_int.Inactivo.Equals(0) && emp_int.Rol.Equals(Constantes.CONST_RolInterventorLider)
                                                  && emp.codproyecto.Equals(codProyectoConvertido)
                                                  select new { emp_int.CodContacto }).FirstOrDefault();

                        int? codContacto_sql4 = return_interventor.CodContacto;

                        #region Eliminación #1.

                        sqlConsulta = "DELETE FROM InterventorVentasTMP" +
                                      "WHERE CodProyecto = " + codProyectoConvertido + " AND id_Produccion = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Eliminación #2.

                        sqlConsulta = "DELETE FROM InterventorVentasMesTMP" +
                                      "WHERE CodProducto = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Consultar cargo de la siguiente sentencia SQL y generar TareaPendiente.

                        #region Obtener el nombre del proyecto "para ser usado en la creación de la tarea pendiente".

                        var nmb_proyecto_linq = (from t in consultas.Db.Proyecto
                                                 where t.Id_Proyecto.Equals(codProyectoConvertido)
                                                 select new { t.NomProyecto }).FirstOrDefault();

                        string NmbProyecto = nmb_proyecto_linq.NomProyecto;

                        #endregion

                        #region Consultar nombre del producto de la siguiente sentencia SQL.

                        var codcargo_sql5 = (from t in consultas.Db.InterventorVentasTMPs
                                             where t.CodProyecto == codProyectoConvertido
                                             && t.id_ventas == ProductoSeleccionado
                                             select new { t.NomProducto }).FirstOrDefault();

                        string nmb_producto_obtenido_sql5 = codcargo_sql5.NomProducto;

                        #endregion

                        #region Generar tareas pendientes.

                        TareaUsuario datoNuevo = new TareaUsuario();
                        datoNuevo.CodContacto = (int)codContacto_sql4;
                        datoNuevo.CodProyecto = codProyectoConvertido;
                        datoNuevo.NomTareaUsuario = "Producto en Producción Rechazado por Gerente Interventor";
                        datoNuevo.Descripcion = "Revisar productos en Producción " + NmbProyecto + " - Actividad --> " + nmb_producto_obtenido_sql5 + "<BR><BR>Observaciones:<BR>" + "OBSERVACIONES DEL INTERVENTOR"; //& fnRequest("ObservaInter")
                        datoNuevo.CodTareaPrograma = 2;
                        datoNuevo.Recurrente = "0"; //"false";
                        datoNuevo.RecordatorioEmail = false;
                        datoNuevo.NivelUrgencia = 1;
                        datoNuevo.RecordatorioPantalla = true;
                        datoNuevo.RequiereRespuesta = false;
                        datoNuevo.CodContactoAgendo = usuario.IdContacto;
                        datoNuevo.DocumentoRelacionado = "";

                        try
                        {
                            Consultas consulta = new Consultas();
                            consulta.Db.TareaUsuarios.InsertOnSubmit(datoNuevo);
                        }
                        catch { string msg_err = "Error en generar tareas."; }

                        #endregion

                        #endregion

                        #endregion
                    }
                    else
                    {
                        //Cerrar la ventana.
                        ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                        return;
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar como Interventor.

                    //Consultar si tiene Coordinador asignado.
                    var result = (from t in consultas.Db.Interventors
                                  where t.CodContacto == usuario.IdContacto
                                  select new { t.CodCoordinador }).FirstOrDefault();

                    if (result.CodCoordinador == 0) //No existe
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true);
                        return;
                    }
                    else
                    {
                        #region Ejecutar sentencias SQL para asignar la tarea al coordinador.

                        #region Obtener el nombre del producto para usarlo en la inserción siguiente.

                        var NomProducto_linq = (from t in consultas.Db.InterventorVentas
                                                where t.CodProyecto == codProyectoConvertido
                                                && t.id_ventas.Equals(ProductoSeleccionado)
                                                select new { t.NomProducto }).FirstOrDefault();

                        string Nombre_producto_Obtenido = NomProducto_linq.NomProducto;

                        #endregion

                        #region Inserción.

                        sqlConsulta = "INSERT INTO InterventorVentasTMP (id_Produccion, CodProyecto, NomProducto, Tarea) " +
                                      "VALUES (" + ProductoSeleccionado + ", " + codProyectoConvertido + ", '" + Nombre_producto_Obtenido + "', 'Modificar') ";

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Recorrer los campos de la tabla con 14 meses y crear registros según las condiciones señaladas en código.

                        for (int i = 0; i < 15; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                //Si el valor de la caja de texto es != de vacío y es diferente de 0 hace la inserción.
                                //if ((i && j) && (i!= 0)) //A2
                                if (i == 0)
                                {
                                    if (i == 1)
                                    {
                                        sqlConsulta = "INSERT INTO InterventorVentasMesTMP(CodProducto, Mes, Valor, Tipo) " +
                                                      "VALUES (" + ProductoSeleccionado + ", " + j + ", '" + j + 1 /*Valor de la caja de texto*/ + ", 1) ";

                                        cmd = new SqlCommand(sqlConsulta, connection);
                                        procesado = EjecutarSQL(connection, cmd);
                                    }
                                    else
                                    {
                                        sqlConsulta = "INSERT INTO InterventorVentasMesTMP(CodProducto, Mes, Valor, Tipo) " +
                                                      "VALUES (" + ProductoSeleccionado + ", " + j + ", '" + j + 1 /*Valor de la caja de texto*/ + ", 2) ";

                                        cmd = new SqlCommand(sqlConsulta, connection);
                                        procesado = EjecutarSQL(connection, cmd);
                                    }
                                }
                            }
                        }

                        #endregion

                        //prTareaAsignarCoordinadorProduccion = No implementado en FONADE clásico, comentado.

                        #endregion
                    }

                    #endregion
                }
            }
            catch (Exception)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: No se pudo adicionar el producto seleccionado.')", true);
                return;
            }
        }

        /// <summary>
        /// Actualizar el producto "en venta".
        /// Recibe por Session el codProduccion seleccionado, pero se debe validar que si lo tenga, de lo
        /// contrario lo retorna.
        /// 16/04/2014: Se tiene que enviar un valor llamado "AprobadoGerente" (al parecer es sólo 1 = Si // 0 = No).
        /// </summary>
        private void ActualizarProductos(int Valor_AprobadoGerente)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            bool procesado = false;
            int ProductoSeleccionado = 0;
            int codProyectoConvertido = 0;

            try { ProductoSeleccionado = Convert.ToInt32(codProduccion.ToString()); }
            catch { return; }

            try { codProyectoConvertido = Convert.ToInt32(codProyecto.ToString()); }
            catch { return; }

            try
            {
                ///Comprobar que si el usuario en sesión es un Gerente Interventor, si lo es puede
                ///ejecutar el flujo, de lo contrario no (no devuelve nada).

                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Procesar la actualización como Gerente Interventor.

                    if (Valor_AprobadoGerente == 1) // Si está aprobado por el gerente
                    {
                        #region Sí fue aprobado por el gerente, por esto, continúa el flujo.

                        #region Trae los registros de la tabla temporal. (Obtiene los datos NomProducto // id_produccion).

                        var cod_cargo_linq = (from t in consultas.Db.InterventorVentasTMPs
                                              where t.CodProyecto == codProyectoConvertido
                                              && t.id_ventas == ProductoSeleccionado
                                              select new
                                              {
                                                  t.id_ventas,
                                                  t.NomProducto
                                              }).FirstOrDefault();

                        #endregion

                        if (cod_cargo_linq.id_ventas > 0)
                        {
                            //Generación de datos.
                            string NomProducto_Obtenido = cod_cargo_linq.NomProducto;
                            int Id_Producto_Venta_Seleccionado = cod_cargo_linq.id_ventas;

                            #region Insertar los nuevos registros en la tabla definitiva.

                            sqlConsulta = "INSERT INTO InterventorVentas (CodProyecto, NomProducto) " +
                                          "VALUES (" + codProyectoConvertido + ", '" + NomProducto_Obtenido + "') ";

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #region Borrar el registro ingresado en la tabla temporal.

                            sqlConsulta = "DELETE FROM InterventorVentasTMP" +
                                          "WHERE CodProyecto = " + codProyectoConvertido + " AND id_Produccion = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #region Traer el código de la actividad para adicionarlo a la tabla definitiva por mes.

                            var cods_int_produccion = (from s in consultas.Db.InterventorVentas
                                                       orderby s.id_ventas descending
                                                       select new
                                                       {
                                                           s.id_ventas
                                                       }).FirstOrDefault();

                            int cod_produccion_sql2 = cods_int_produccion.id_ventas;

                            #endregion

                            #region Trae los registros de la tabla temporal por meses.

                            var sql_3 = (from v in consultas.Db.InterventorVentasMesTMPs
                                         where v.CodProducto == ProductoSeleccionado
                                         select new
                                         {
                                             v.Mes,
                                             v.Tipo,
                                             v.Valor
                                         }).FirstOrDefault();

                            int? Mes_sql_3 = sql_3.Mes;
                            decimal? Valor_sql_3 = sql_3.Valor;
                            int? Tipo_sql_3 = sql_3.Tipo;

                            #endregion

                            #region Inserta los nuevos registros en la tabla definitiva por meses

                            for (int i = 0; i < 14; i++)
                            {
                                if (Tipo_sql_3 == 1) // Si el tipo es 1, genera este flujo.
                                {
                                    //Registrar como tipo 1.
                                    sqlConsulta = " INSERT INTO InterventorVentasMes (CodProducto, Mes, Valor, Tipo) " +
                                                  " VALUES (" + ProductoSeleccionado + ", " + Mes_sql_3 + ", " + Valor_sql_3 + ", 1)";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);
                                }
                                else
                                {
                                    //Registrar como tipo 2.
                                    sqlConsulta = " INSERT INTO InterventorVentasMes (CodProducto, Mes, Valor, Tipo) " +
                                                  " VALUES (" + ProductoSeleccionado + ", " + Mes_sql_3 + ", " + Valor_sql_3 + ", 2)";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);
                                }
                            }

                            #endregion

                            #region Borrar el registro de la tabla temporal por meses.

                            sqlConsulta = "DELETE FROM InterventorVentasMesTMP" +
                                          "WHERE CodProducto = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion
                        }
                        else
                        {
                            //No se obtuvieron datos.
                            return;
                        }

                        #endregion
                    }
                    if (Valor_AprobadoGerente == 0) //No está aprobado.
                    {
                        #region Se procesa la información según el archivo "CatalogoProduccionTMP.asp" de FONADE clasico.

                        //Se devuelve al interventor, se le avisa al coordinador.
                        var return_interventor = (from emp_int in consultas.Db.EmpresaInterventors
                                                  join emp in consultas.Db.Empresas
                                                  on emp_int.CodEmpresa equals emp.id_empresa
                                                  where emp_int.Inactivo.Equals(0) && emp_int.Rol.Equals(Constantes.CONST_RolInterventorLider)
                                                  && emp.codproyecto.Equals(codProyectoConvertido)
                                                  select new { emp_int.CodContacto }).FirstOrDefault();

                        int? codContacto_sql4 = return_interventor.CodContacto;

                        #region Eliminación #1.

                        sqlConsulta = "DELETE FROM InterventorVentasTMP" +
                                      "WHERE CodProyecto = " + codProyectoConvertido + " AND id_produccion = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Eliminación #2.

                        sqlConsulta = "DELETE FROM InterventorVentasMesTMP" +
                                      "WHERE CodProducto = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Consultar cargo de la siguiente sentencia SQL y generar TareaPendiente.

                        #region Obtener el nombre del proyecto "para ser usado en la creación de la tarea pendiente".

                        var nmb_proyecto_linq = (from t in consultas.Db.Proyecto
                                                 where t.Id_Proyecto.Equals(codProyectoConvertido)
                                                 select new { t.NomProyecto }).FirstOrDefault();

                        string NmbProyecto = nmb_proyecto_linq.NomProyecto;

                        #endregion

                        #region Consultar nombre del producto de la siguiente sentencia SQL.

                        var codcargo_sql5 = (from t in consultas.Db.InterventorVentasTMPs
                                             where t.CodProyecto == codProyectoConvertido
                                             && t.id_ventas == ProductoSeleccionado
                                             select new { t.NomProducto }).FirstOrDefault();

                        string nmb_producto_obtenido_sql5 = codcargo_sql5.NomProducto;

                        #endregion

                        #region Generar tareas pendientes.

                        TareaUsuario datoNuevo = new TareaUsuario();
                        datoNuevo.CodContacto = (int)codContacto_sql4;
                        datoNuevo.CodProyecto = codProyectoConvertido;
                        datoNuevo.NomTareaUsuario = "Producto en Producción Rechazado por Gerente Interventor";
                        datoNuevo.Descripcion = "Revisar productos en Producción " + NmbProyecto + " - Actividad --> " + nmb_producto_obtenido_sql5 + "<BR><BR>Observaciones:<BR>" + "OBSERVACIONES DEL INTERVENTOR"; //& fnRequest("ObservaInter")
                        datoNuevo.CodTareaPrograma = 2;
                        datoNuevo.Recurrente = "0"; //"false";
                        datoNuevo.RecordatorioEmail = false;
                        datoNuevo.NivelUrgencia = 1;
                        datoNuevo.RecordatorioPantalla = true;
                        datoNuevo.RequiereRespuesta = false;
                        datoNuevo.CodContactoAgendo = usuario.IdContacto;
                        datoNuevo.DocumentoRelacionado = "";

                        try
                        {
                            Consultas consulta = new Consultas();
                            consulta.Db.TareaUsuarios.InsertOnSubmit(datoNuevo);
                        }
                        catch { string msg_err = "Error en generar tareas."; }

                        #endregion

                        #endregion

                        #endregion
                    }
                    else
                    {
                        //Cerrar la ventana.
                        ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                        return;
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    #region Procesar la actualización como Coordinador Interventor.

                    if (Valor_AprobadoGerente == 1) //Se usa la misma variable, pero para Coordinador se llama "Aprobado".
                    {
                        #region Update.

                        sqlConsulta = " UPDATE InterventorVentasTMP " +
                                      " SET ChequeoCoordinador = 1 " +
                                      " WHERE CodProyecto = " + codProyectoConvertido +
                                      " AND id_Produccion = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Obtener Id_Grupo de la consulta.

                        var IdGrupo_sql1 = (from t in consultas.Db.Grupos
                                            where t.NomGrupo.Equals("Gerente Interventor")
                                            select new { t.Id_Grupo }).FirstOrDefault();

                        int IdGrupo_Obtenido = IdGrupo_sql1.Id_Grupo;

                        #endregion

                        #region Con el Id_Grupo obtenido de la consulta anterior, se consulta el CodContacto de GrupoContacto.

                        //Es una tabla relacional.
                        var CodContacto_Rel_sql2 = (from t in consultas.Db.GrupoContactos
                                                    where t.CodGrupo.Equals(IdGrupo_Obtenido)
                                                    select new { t.CodContacto }).FirstOrDefault();

                        int CodContacto_Obtenido = CodContacto_Rel_sql2.CodContacto;

                        #endregion

                        //Asignar tarea = NO IMPLEMENTADO = código comentado en el clásico. prTareaAsignarGerenteProduccion
                    }
                    if (Valor_AprobadoGerente == 0) //No está aprobado.
                    {
                        #region Se procesa la información según el archivo "CatalogoProduccionTMP.asp" de FONADE clasico.

                        //Se devuelve al interventor, se le avisa al coordinador.
                        var return_interventor = (from emp_int in consultas.Db.EmpresaInterventors
                                                  join emp in consultas.Db.Empresas
                                                  on emp_int.CodEmpresa equals emp.id_empresa
                                                  where emp_int.Inactivo.Equals(0) && emp_int.Rol.Equals(Constantes.CONST_RolInterventorLider)
                                                  && emp.codproyecto.Equals(codProyectoConvertido)
                                                  select new { emp_int.CodContacto }).FirstOrDefault();

                        int? codContacto_sql4 = return_interventor.CodContacto;

                        #region Eliminación #1.

                        sqlConsulta = "DELETE FROM InterventorVentasTMP" +
                                      "WHERE CodProyecto = " + codProyectoConvertido + " AND id_Produccion = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Eliminación #2.

                        sqlConsulta = "DELETE FROM InterventorVentasMesTMP" +
                                      "WHERE CodProducto = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Consultar cargo de la siguiente sentencia SQL y generar TareaPendiente.

                        #region Obtener el nombre del proyecto "para ser usado en la creación de la tarea pendiente".

                        var nmb_proyecto_linq = (from t in consultas.Db.Proyecto
                                                 where t.Id_Proyecto.Equals(codProyectoConvertido)
                                                 select new { t.NomProyecto }).FirstOrDefault();

                        string NmbProyecto = nmb_proyecto_linq.NomProyecto;

                        #endregion

                        #region Consultar nombre del producto de la siguiente sentencia SQL.

                        var nmb_producto_sql5 = (from t in consultas.Db.InterventorVentasTMPs
                                                 where t.CodProyecto == codProyectoConvertido
                                                 && t.id_ventas == ProductoSeleccionado
                                                 select new { t.NomProducto }).FirstOrDefault();

                        string nmb_producto_obtenido_sql5 = nmb_producto_sql5.NomProducto;

                        #endregion

                        #region Generar tareas pendientes.

                        TareaUsuario datoNuevo = new TareaUsuario();
                        datoNuevo.CodContacto = (int)codContacto_sql4;
                        datoNuevo.CodProyecto = codProyectoConvertido;
                        datoNuevo.NomTareaUsuario = "Producto en Producción Rechazado por Coordinador de Interventoria";
                        datoNuevo.Descripcion = "Revisar productos en Producción " + NmbProyecto + " - Actividad --> " + nmb_producto_obtenido_sql5 + "<BR><BR>Observaciones:<BR>" + "OBSERVACIONES DEL INTERVENTOR"; //& fnRequest("ObservaInter")
                        datoNuevo.CodTareaPrograma = 2;
                        datoNuevo.Recurrente = "0"; //"false";
                        datoNuevo.RecordatorioEmail = false;
                        datoNuevo.NivelUrgencia = 1;
                        datoNuevo.RecordatorioPantalla = true;
                        datoNuevo.RequiereRespuesta = false;
                        datoNuevo.CodContactoAgendo = usuario.IdContacto;
                        datoNuevo.DocumentoRelacionado = "";

                        try
                        {
                            Consultas consulta = new Consultas();
                            consulta.Db.TareaUsuarios.InsertOnSubmit(datoNuevo);
                        }
                        catch { string msg_err = "Error en generar tareas."; }

                        #endregion

                        #endregion

                        #endregion
                    }
                    else
                    {
                        //Cerrar la ventana.
                        ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                        return;
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar la actualización como Interventor.

                    //Consultar si tiene Coordinador asignado.
                    var result = (from t in consultas.Db.Interventors
                                  where t.CodContacto == usuario.IdContacto
                                  select new { t.CodCoordinador }).FirstOrDefault();

                    if (result.CodCoordinador == 0) //No existe
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true);
                        return;
                    }
                    else
                    {
                        #region Ejecuta sentencias de actualización SQL.

                        #region Obtener el nombre del producto para usarlo en la inserción siguiente.

                        var NomProducto_linq = (from t in consultas.Db.InterventorVentas
                                                where t.CodProyecto == codProyectoConvertido
                                                && t.id_ventas.Equals(ProductoSeleccionado)
                                                select new { t.NomProducto }).FirstOrDefault();

                        string Nombre_producto_Obtenido = NomProducto_linq.NomProducto;

                        #endregion

                        #region Inserción.

                        sqlConsulta = "INSERT INTO InterventorVentasTMP (id_Produccion, CodProyecto, NomProducto, Tarea) " +
                                      "VALUES (" + ProductoSeleccionado + ", " + codProyectoConvertido + ", '" + Nombre_producto_Obtenido + "', 'Modificar') ";

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Recorrer los campos de la tabla con 14 meses y crear registros según las condiciones señaladas en código.

                        for (int i = 0; i < 15; i++)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                //Si el valor de la caja de texto es != de vacío y es diferente de 0 hace la inserción.
                                //if ((i && j) && (i!= 0)) //A2
                                if (i == 0)
                                {
                                    if (i == 1)
                                    {
                                        sqlConsulta = "INSERT INTO InterventorVentasMesTMP(CodProducto, Mes, Valor, Tipo) " +
                                                      "VALUES (" + ProductoSeleccionado + ", " + j + ", '" + j + 1 /*Valor de la caja de texto*/ + ", 1) ";

                                        cmd = new SqlCommand(sqlConsulta, connection);
                                        procesado = EjecutarSQL(connection, cmd);
                                    }
                                    else
                                    {
                                        sqlConsulta = "INSERT INTO InterventorVentasMesTMP(CodProducto, Mes, Valor, Tipo) " +
                                                      "VALUES (" + ProductoSeleccionado + ", " + j + ", '" + j + 1 /*Valor de la caja de texto*/ + ", 2) ";

                                        cmd = new SqlCommand(sqlConsulta, connection);
                                        procesado = EjecutarSQL(connection, cmd);
                                    }
                                }
                            }
                        }

                        #endregion

                        //prTareaAsignarCoordinadorProduccion = No implementado en FONADE clásico, comentado.

                        #endregion
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    //Cerrar la ventana.
                    ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                    return;
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene permisos para realizar esta acción.')", true);
                    return;
                }
            }
            catch (Exception)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: No se pudo actualizar el producto seleccionado.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/04/2014.
        /// Modificar el producto seleccionado.
        /// Adaptando exactamente el código fuente de CatalogoProduccionTMP.asp.
        /// </summary>
        /// <param name="Valor_AprobadoGerente">Valor Aprobado, debe ser 1 para ejecutar el proceso, de lo contrario, se cerrará la ventana.</param>
        private void ModificarProductos(int Valor_AprobadoGerente)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            bool procesado = false;
            int ProductoSeleccionado = 0;
            int codProyectoConvertido = 0;

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    if (Valor_AprobadoGerente == 1) //Sí está aprobado.
                    {
                        #region Procesa la informacion, ya que está aprobada.

                        #region Trae los registros de la tabla temporal. (Obtiene el dato id_produccion).

                        var cod_cargo_linq = (from t in consultas.Db.InterventorVentasTMPs
                                              where t.CodProyecto == codProyectoConvertido
                                              && t.id_ventas == ProductoSeleccionado
                                              select new
                                              {
                                                  t.id_ventas
                                              }).FirstOrDefault();

                        #endregion

                        if (cod_cargo_linq.id_ventas > 0)
                        {
                            //Generación de datos.
                            int Id_Producto_Seleccionado = cod_cargo_linq.id_ventas;
                            
                            #region Actualiza los registros en la tabla definitiva.

                            sqlConsulta = " UPDATE InterventorVentas " +
                                          " SET CodProyecto = " + codProyectoConvertido +
                                          " WHERE CodProyecto = " + codProyectoConvertido +
                                          " AND id_Produccion = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #region Borrar el registro ya actualizado en la tabla temporal.

                            sqlConsulta = "DELETE FROM InterventorVentasTMP" +
                                          "WHERE id_Produccion = " + codProyectoConvertido;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #region Traer el código de la actividad para adicionarlo a la tabla definitiva por mes.

                            var cods_int_produccion = (from s in consultas.Db.InterventorVentas
                                                       orderby s.id_ventas descending
                                                       select new
                                                       {
                                                           s.id_ventas
                                                       }).FirstOrDefault();

                            int cod_produccion_sql2 = cods_int_produccion.id_ventas;

                            #endregion

                            #region Trae los registros de la tabla temporal por meses.

                            //Consulta SQL. CodProducto, Mes, Tipo, valor
                            sqlConsulta = " SELECT CodProducto, Mes, Tipo, valor FROM InterventorVentasMesTMP " +
                                          " WHERE CodProducto = " + ProductoSeleccionado;

                            #region Borrar todos los registros de la tabla "InterventorVentasMes" correspondientes al código de la actividad...

                            sqlConsulta = "DELETE FROM InterventorVentasMes" +
                                          "WHERE CodProducto = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            //Asignar el resultado de la consulta en una tabla para luego recorrerla.
                            DataTable tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                            #region Hace el recorrido de la tabla que contiene los resultados, evaluando si es de tipo 1 ó 2 y hace la inserción.

                            for (int i = 0; i < tabla.Rows.Count; i++)
                            {
                                if (tabla.Rows[i]["Tipo"].ToString() == "1")
                                {
                                    #region Inserción de tipo 1.

                                    sqlConsulta = " INSERT INTO InterventorVentasMes (CodProducto, Mes, Valor, Tipo) " +
                                                  " VALUES (" + ProductoSeleccionado + ", " + Convert.ToInt32(tabla.Rows[i]["Mes"].ToString()) + ", " + Convert.ToInt32(tabla.Rows[i]["Valor"].ToString()) + ", 1)";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);

                                    #endregion
                                }
                                else
                                {
                                    #region Inserción de tipo 2.

                                    sqlConsulta = " INSERT INTO InterventorVentasMes (CodProducto, Mes, Valor, Tipo) " +
                                                  " VALUES (" + ProductoSeleccionado + ", " + Convert.ToInt32(tabla.Rows[i]["Mes"].ToString()) + ", " + Convert.ToInt32(tabla.Rows[i]["Valor"].ToString()) + ", 2)";

                                    cmd = new SqlCommand(sqlConsulta, connection);
                                    procesado = EjecutarSQL(connection, cmd);

                                    #endregion
                                }
                            }

                            #endregion

                            #region Borrar el registro ya ingresado en la tabla temporal por meses.

                            sqlConsulta = "DELETE FROM InterventorVentasMesTMP" +
                                          "WHERE CodProducto = " + ProductoSeleccionado;

                            cmd = new SqlCommand(sqlConsulta, connection);
                            procesado = EjecutarSQL(connection, cmd);

                            #endregion

                            #endregion
                        }
                        else
                        {
                            //No se obtuvieron datos.
                            return;
                        }

                        #endregion
                    }
                    else
                    {
                        //Cerrar la ventana.
                        ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                        return;
                    }
                }
                else
                {
                    //No puede acceder a esta funcionalidad según "CatalogoProduccionTMP.asp".
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene permisos para realizar esta acción.')", true);
                    return;
                }
            }
            catch (Exception)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: No se pudo modificar el producto seleccionado.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/04/2014.
        /// Eliminar el producto en venta, de acuerdo a los permisos que tenga el usuario en sesión, es decir, 
        /// de acuerdo a su grupo = rol.
        /// <param name="Valor_Aprobado">Valor aprobado, debe ser 1 = SI // 0 = NO o saldrá y no hará nada.</param>
        /// </summary>
        private void EliminarProductos(int Valor_Aprobado)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            bool procesado = false;
            int ProductoSeleccionado = 0;
            int codProyectoConvertido = 0;

            try { ProductoSeleccionado = Convert.ToInt32(codProduccion.ToString()); }
            catch { return; }

            try { codProyectoConvertido = Convert.ToInt32(codProyecto.ToString()); }
            catch { return; }

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar el flujo como Interventor.

                    //Consultar si tiene Coordinador asignado.
                    var result = (from t in consultas.Db.Interventors
                                  where t.CodContacto == usuario.IdContacto
                                  select new { t.CodCoordinador }).FirstOrDefault();

                    if (result.CodCoordinador == 0) //No existe
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true);
                        return;
                    }
                    else
                    {
                        #region Asignar la tarea al coordinador.

                        sqlConsulta = "INSERT INTO InterventorVentasTMP (id_Produccion, CodProyecto, Tarea) " +
                                      "VALUES (" + ProductoSeleccionado + ", " + codProyectoConvertido + " , 'Borrar') ";

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Hace una segunda inserción a otra tabla...

                        sqlConsulta = "INSERT INTO InterventorVentasMesTMP (CodProducto) " +
                                      "VALUES ( " + ProductoSeleccionado + ") ";

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        //Asignar tarea = NO IMPLEMENTADO = código comentado en el clásico. prTareaAsignarCoordinadorProduccion
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Procesar el flujo como Gerente Interventor.

                    if (Valor_Aprobado == 1) // Si está aprobado
                    {
                        #region Borrar los registros de la tabla definitiva.

                        sqlConsulta = " DELETE FROM InterventorVentas " +
                                      " WHERE CodProyecto = " + codProyectoConvertido + " AND id_Produccion = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Borrar el registro ya ingresado en la tabla temporal.

                        sqlConsulta = " DELETE FROM InterventorVentasTMP " +
                                      " WHERE CodProyecto = " + codProyectoConvertido + " AND id_Produccion = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Borra los registros de la tabla definitiva por meses.

                        sqlConsulta = " DELETE FROM InterventorVentasMes " +
                                      " WHERE CodProducto = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion

                        #region Borra el registro de la tabla temporal por meses.

                        sqlConsulta = " DELETE FROM InterventorVentasMesTMP " +
                                      " WHERE CodProducto = " + ProductoSeleccionado;

                        cmd = new SqlCommand(sqlConsulta, connection);
                        procesado = EjecutarSQL(connection, cmd);

                        #endregion
                    }
                    else
                    {
                        //No está aprobado.
                        //Cerrar la ventana.
                        ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                        return;
                    }

                    #endregion
                }
                ///Inquietud: Según el FONADE clásico, valida PRIMERO si el grupo(rol) es Interventor, si es así, ejecuta el código, pero después 
                ///hace la misma validación de que si el grupo(rol) es Interventor, si es así, se cierra la ventana pero le envía otros datos a otra página...
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    //Cerrar la ventana.
                    ClientScriptManager cm = this.ClientScript; cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.reload();window.close();</script>");
                    return;
                }
                else
                {
                    //No puede acceder a esta funcionalidad según "CatalogoProduccionTMP.asp".
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene permisos para realizar esta acción.')", true);
                    return;
                }
            }
            catch (Exception)
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: No se pudo eliinar el producto seleccionado.')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// Ejecutar SQL.
        /// Método que recibe la conexión y la consulta SQL y la ejecuta.
        /// </summary>
        /// <param name="p_connection">Conexión.</param>
        /// <param name="p_cmd">SqlCommand.</param>
        /// <returns>TRUE = Consulta ejecutada correctamente. // FALSE = Error.</returns>
        private bool EjecutarSQL(SqlConnection p_connection, SqlCommand p_cmd)
        {
            //Ejecutar controladamente la consulta SQL.
            try
            {
                p_connection.Open();
                p_cmd.ExecuteReader();
                p_connection.Close();
                return true;
            }
            catch
            { return false; }
            finally
            { p_connection.Close(); p_connection.Dispose(); }
        }

        #endregion
    }
}