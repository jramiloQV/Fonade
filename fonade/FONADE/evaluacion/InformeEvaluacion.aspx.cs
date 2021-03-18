#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>12 - 03 - 2014</Fecha>
// <Archivo>InformeEvaluacion.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using Fonade.Clases;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class InformeEvaluacion : Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Código del proyecto seleccionado.
        /// </summary>
        String CodProyecto;

        /// <summary>
        /// Código de la convocatoria del proyecto seleccionado.
        /// </summary>
        String CodConvocatoria;

        /// <summary>
        /// Variable para redactar las consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Contiene la información obtenida de la consulta en el método "ObservacionEvaluacion".
        /// </summary>
        String txtGenerales;

        /// <summary>
        /// Contiene la información obtenida de la consulta en el método "ObservacionEvaluacion".
        /// </summary>
        String txtValorRecomendado;

        /// <summary>
        /// Contiene la información obtenida de la consulta en el método "ObservacionEvaluacion".
        /// </summary>
        String txtConclusionesFinancieras;

        /// <summary>
        /// Contiene la información obtenida de la consulta en el método "ObservacionEvaluacion".
        /// </summary>
        Int32 TiempoProyeccion;

        /// <summary>
        /// Tabla que contendrá los resultados depurados en el método "ObservacionEvaluacion".
        /// </summary>
        DataTable tabla_resultado;

        /// <summary>
        /// Valor que se declara en el método "GenerarTerceraTabla" y métodos subsecuentes.
        /// </summary>
        String txtVariable;

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/06/2014.
        /// Variable que contiene el valor "Todos" en caso de que se haya seleccionado así, de lo contrario, estará
        /// vacío.
        /// </summary>
        String Var_Accion;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 29/05/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CodProyecto = HttpContext.Current.Session["CodProyecto"] != null ? CodProyecto = HttpContext.Current.Session["CodProyecto"].ToString() : "0";
                CodConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null ? CodConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";
                Var_Accion = HttpContext.Current.Session["Var_Accion"] != null ? Var_Accion = HttpContext.Current.Session["Var_Accion"].ToString() : "";

                if (CodProyecto == "0" && CodConvocatoria == "0")
                { /*Cerrar ventana.*/ ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

                //Establecer fecha.
                DateTime fecha = DateTime.Now;
                string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                lblfecha.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                lusuario.Text = usuario.Nombres + " " + usuario.Apellidos;

                #region Llamada a métodos que generan uno por uno las partes que componen el informe de evaluación.
                //Generar encabezado.
                CargarEncabezado();

                //Generar tablas.
                GenerarSegundaTabla();
                GenerarTerceraTabla();
                GenerarCuartaTabla();
                GenerarQuintaTabla();
                GenerarSextaTabla();
                GenerarSeptimaTabla();
                GenerarOctavaTabla();
                GenerarNovenaTabla();
                GenerarDecimaTabla();
                GenerarUndecimaTabla();
                GenerarDuodecimaTabla();
                #endregion

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Por favor, oprima click sobre la tabla que desea imprimir.')", true);
            }
        }

        #region Métodos generales.

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/04/2014.
        /// Método usado en "DeclaraVariables.inc" de FONADE Clásico.
        /// Usado para obtener el valor "Texto" de la tabla "Texto", este valor será usado en la creación
        /// de mensajes cuando el CheckBox "chk_actualizarInfo" esté chequeado; Si el resultado de la consulta
        /// NO trae datos, según FONADE Clásico, crea un registro con la información dada.
        /// </summary>
        /// <param name="NomTexto">Nombre del texto a consultar.</param>
        /// <returns>NomTexto consultado.</returns>
        private string Texto(String NomTexto)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String RSTexto;
            String txtSQL;
            bool correcto = false;

            //Consulta
            txtSQL = "SELECT Texto FROM Texto WHERE NomTexto='" + NomTexto + "'";

            var resultado = consultas.ObtenerDataTable(txtSQL, "text");

            if (resultado.Rows.Count > 0)
                return resultado.Rows[0]["Texto"].ToString();
            else
            {
                #region Si no existe la palabra "consultada", la crea.

                txtSQL = "INSERT INTO Texto (NomTexto, Texto) VALUES ('" + NomTexto + "','" + NomTexto + "')";

                //Asignar SqlCommand para su ejecución.
                cmd = new SqlCommand(txtSQL, conn);

                //Ejecutar SQL.
                correcto = EjecutarSQL(conn, cmd);

                if (correcto == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de TEXTO.')", true);
                    return NomTexto; //""; //Debería retornar vacío y validar en el método donde se llame si esté validado.
                }
                else
                { return NomTexto; }

                #endregion
            }
        }

        #endregion

        #region Métodos para generar información complementaria al informe.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 29/05/2014.
        /// Obtener las observaciones de evaluación.
        /// </summary>
        /// <returns>DataTable con datos.</returns>
        private DataTable ObservacionEvaluacion()
        {
            try
            {
                //Inicializar variables.
                DataTable rs = new DataTable();
                String[] arr_final = new String[6];

                //Consulta.
                txtSQL = " SELECT * FROM evaluacionobservacion WHERE codConvocatoria = " + CodConvocatoria +
                         " AND CodProyecto = " + CodProyecto;

                //Asignar resultados de la consulta a variable DataTable.
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos, creará la tabla compuesta.
                if (rs.Rows.Count > 0)
                {
                    //Crear arreglo final de encabezado (resultado de la consulta).
                    arr_final[0] = rs.Rows[0]["Actividades"].ToString();
                    arr_final[1] = rs.Rows[0]["ProductosServicios"].ToString();
                    arr_final[2] = rs.Rows[0]["EstrategiaMercado"].ToString();
                    arr_final[3] = rs.Rows[0]["ProcesoProduccion"].ToString();
                    arr_final[4] = rs.Rows[0]["EstructuraOrganizacional"].ToString();
                    arr_final[5] = rs.Rows[0]["TamanioLocalizacion"].ToString();

                    //Crear tabla.
                    tabla_resultado = new DataTable();

                    tabla_resultado.Columns.Add("Actividades a las que se dedicará la empresa", typeof(System.String));
                    tabla_resultado.Columns.Add("Productos y Servicios que Ofrecerá", typeof(System.String));
                    tabla_resultado.Columns.Add("Canales de distribución, estrategias de mercado", typeof(System.String));
                    tabla_resultado.Columns.Add("Proceso de producción", typeof(System.String));
                    tabla_resultado.Columns.Add("Análisis Estructura Organizacional", typeof(System.String));
                    tabla_resultado.Columns.Add("Análisis Tamaño propuesto y Localización", typeof(System.String));

                    //Añadir los valores a la tabla.
                    tabla_resultado.Rows.Add(arr_final);

                    #region Asignar los valores a variables globales.
                    txtGenerales = rs.Rows[0]["Generales"].ToString();
                    txtValorRecomendado = rs.Rows[0]["ValorRecomendado"].ToString();
                    txtConclusionesFinancieras = rs.Rows[0]["ConclusionesFinancieras"].ToString();
                    TiempoProyeccion = Int32.Parse(rs.Rows[0]["TiempoProyeccion"].ToString());
                    #endregion

                    //Destruir la variable que tenía los resultados de la consulta.
                    rs = null;

                    //Retornar la tabla.
                    return tabla_resultado;
                }
                else { tabla_resultado = new DataTable(); return tabla_resultado; }
            }
            catch
            { tabla_resultado = new DataTable(); return tabla_resultado; }
        }

        #endregion

        #region Métodos que generan el informe.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 29/05/2014.
        /// Cargar el encabezado.
        /// </summary>
        private void CargarEncabezado()
        {
            //Inicializar variables.
            DataTable rs = new DataTable();
            DataTable rsAux = new DataTable();
            Double AporteDinero = 0;
            Double AporteEspecie = 0;
            Double Suma_Aportes = 0;
            String Suma_Aportes_Formateado = "0,0";
            Double Total1 = 0;
            Double Total2 = 0;
            Double Suma_Total1_Total2 = 0;
            String Total1_Formateado = "0,0";
            String Total2_Formateado = "0,0";
            String Suma_Total1_Total2_Formateado = "0,0";
            String sEquipoTrabajo = "";
            DateTime txtAux = DateTime.Today;
            String txtAux2 = "";
            Double ValorCartera = 0;
            Double ValorOtrasCarteras = 0;
            String ValorCartera_Formateado = "0,0";
            String ValorOtrasCarteras_Formateado = "0,0";
            String sEvaluador = "";
            String sAsesor = "";

            try
            {
                //Generar primera consulta.
                txtSQL = " SELECT nomProyecto, Sumario, nomCiudad + ' - ' + nomDepartamento Ciudad, " +
                         " nomUnidad + ' - ' + nomInstitucion Unidad " +
                         " FROM Proyecto P, Ciudad C, Departamento D, institucion I " +
                         " WHERE P.codCiudad = C.id_Ciudad AND " +
                         " C.codDepartamento = D.id_Departamento AND P.codInstitucion=I.id_Institucion AND " +
                         " id_Proyecto = " + CodProyecto;

                //Asignar resultados a variable DataTable.
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Si la consulta tiene datos, puede continuar con el flujo.
                if (rs.Rows.Count > 0)
                {
                    #region Como tiene datos, empezará a dibujar la tabla y sub-tablas que forman el encabezado.

                    #region Inicio de todo el bloque de encabezado.

                    lbl_tr_emprendedores.Text = "<div style=\"font-size: 8px; font-family: Arial;\">" +
                                                "<table style='font-size: 10px' width='100%' border='1' cellpadding='4' cellspacing='0' bordercolor='#000000'>" +
                                                "  <tr><td>" +
                                                "  <table style='font-size: 10px' width='100%' border='0' cellpadding='4' cellspacing='1'>				" +
                                                "      <tr><td colspan='4'><b>Nombre del plan de negocio: " + CodProyecto + " - </b>" + rs.Rows[0]["nomProyecto"].ToString() + "</tr>" +
                                                "      <tr><td colspan='4'>" + rs.Rows[0]["Sumario"].ToString() + "</td></tr>" +
                                                "      <tr>" +
                                                "          <td><b>Ciudad Sede:</b></td>" +
                                                "          <td>" + rs.Rows[0]["Ciudad"].ToString() + "</td>" +
                                                "          <td><b>Unidad Emprendedora:</b></td>" +
                                                "          <td>" + rs.Rows[0]["unidad"].ToString() + "</td>" +
                                                "      </tr>" +
                                                "      <tr>" +
                                                "          <td><b>Fecha Solicitud:</b></td>" +
                                                "          <td>&nbsp;</td>" +
                                                "          <td><b>Evaluador:</b> </td>" +
                                                "          <td>";

                    #endregion

                    #region Consultar el evaluador.

                    //Consulta.
                    txtSQL = " SELECT  A.nombres + ' ' + A.Apellidos as Evaluador " +
                             " FROM ProyectoContacto P, Contacto A " +
                             " WHERE P.codContacto = A.id_Contacto AND P.Inactivo = 0 " +
                             " AND codRol = " + Constantes.CONST_RolEvaluador +
                             " AND P.codProyecto = " + CodProyecto;

                    //Asignar resultados a la variable DataTable.
                    rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                    //Si tiene datos, asigna el valor en la variable.
                    if (rsAux.Rows.Count > 0)
                    { sEvaluador = rsAux.Rows[0]["Evaluador"].ToString(); rsAux = null; }

                    #endregion

                    #region Consultar el asesor.

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + sEvaluador + "</td>" +
                                                                            "</tr>" +
                                                                            "<tr>" +
                                                                            "	<td><b>Fecha Evaluacion:</b></td>" +
                                                                            "	<td>&nbsp;</td>" +
                                                                            "	<td><b>Asesor:</b></td>" +
                                                                            "	<td>";

                    //Consultar asesor.
                    txtSQL = " SELECT A.nombres + ' ' + A.Apellidos as Asesor " +
                             " FROM ProyectoContacto P, Contacto A " +
                             " WHERE P.codContacto = A.id_Contacto AND P.Inactivo = 0 " +
                             " AND codRol = " + Constantes.CONST_RolAsesorLider +
                             " AND P.codProyecto = " + CodProyecto;

                    //Asignar resultados de la consulta a variable DataTable.
                    rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                    //Si tiene datos, asigna el valor en la variable.
                    if (rsAux.Rows.Count > 0)
                    { sAsesor = rsAux.Rows[0]["Asesor"].ToString(); rsAux = null; }

                    #endregion

                    #region Integrantes de la iniciativa empresarial.

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + sAsesor + "</td>" +
                                   "			</tr>" +
                                   "		</table>" +
                                   "		</td></tr>" +
                                   "	</table>" +
                                   "	<br/>		" +
                                   "	<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                   "		<tr><td>" +
                                   "		<table style='font-size: 10px' width='100%' border='0' cellpadding='4' cellspacing='1'>" +
                                   "			<tr bgcolor='#000000'>" +
                                   "				<td class='Blanca' rowspan='2' align='center'><span style='color:#ffffff'>Integrantes de la iniciativa Empresarial</span></td>" +
                                   "				<td class='Blanca' colspan='2' align='center'><span style='color:#ffffff'>Tipo</span></td>" +
                                   "				<td class='Blanca' colspan='4' align='center'><span style='color:#ffffff'>Valor del aporte (miles de pesos)</span></td>" +
                                   "			</tr>				" +
                                   "			<tr bgcolor='#000000'>" +
                                   "				<td class='Blanca' align='center'><span style='color:#ffffff'>Emprendedor</span></td>" +
                                   "				<td class='Blanca' align='center'><span style='color:#ffffff'>Otro</span></td>" +
                                   "				<td class='Blanca' align='center'><span style='color:#ffffff'>Aporte Total</span></td>" +
                                   "				<td class='Blanca' align='center'><span style='color:#ffffff'>Aporte en Dinero</span></td>" +
                                   "				<td class='Blanca' align='center'><span style='color:#ffffff'>Aporte en especie</span></td>" +
                                   "				<td class='Blanca' align='center'><span style='color:#ffffff'>Clase de especie</span></td>" +
                                   "			</tr>";

                    #endregion

                    #region Consultar emprendedor y aportes.
                    //Generar consulta.
                    txtSQL = " SELECT C.Nombres + ' ' + C.Apellidos Emprendedor, P.Beneficiario, AporteDinero, AporteEspecie, " +
                             " DetalleEspecie " +
                             " FROM EvaluacionContacto E, Contacto C, ProyectoContacto P " +
                             " WHERE E.codContacto = C.id_contacto AND " +
                             " E.codProyecto = P.codProyecto AND " +
                             " P.inactivo = 0 AND P.codRol = " + Constantes.CONST_RolEmprendedor +
                             " AND P.codContacto = C.id_Contacto AND " +
                             " E.codProyecto=" + CodProyecto + " AND E.codConvocatoria=" + CodConvocatoria;

                    //Asignar resultados a variable DataTable.
                    rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                    //Si tiene datos.
                    if (rsAux.Rows.Count > 0)
                    {
                        #region Inicia ciclo para generar valores de los valores aportados.
                        for (int i = 0; i < rsAux.Rows.Count; i++)
                         {
                            #region Generar la tabla en el Label con la demás información recopilada hasta el momento.
                            //Iniciar con la estructura de la tabla.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>" +
                                                      "	 <td align='left'><b>" + rsAux.Rows[i]["Emprendedor"].ToString() + "</b></td>" +
                                                      "	 <td align='center'><!--Abre la celda para establecer la imagen.-->";

                            #region Valores de la celda 1.
                            //Si el valor "Beneficiario" tiene datos, se coloca la imagen con el "chulo", de lo contrario, el gif.
                            if (rsAux.Rows[i]["Beneficiario"].ToString() == "1" || rsAux.Rows[i]["Beneficiario"].ToString().ToLower() == "true")
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<img src='../../Images/chulo.gif' border=0>"; }
                            else
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<br/>"; }

                            //Cierra la celda.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td>";
                            #endregion

                            #region Valores de la celda 2.
                            //Abre la celda #2.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td>";

                            //Si el valor "Beneficiario" tiene datos, se coloca la imagen con el "chulo", de lo contrario, el gif.
                            if (rsAux.Rows[i]["Beneficiario"].ToString() == "0" || rsAux.Rows[i]["Beneficiario"].ToString().ToLower() == "false")
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<img src='../../Images/chulo.gif' border=0>"; }
                            else
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<br/>"; }

                            //Cierra la celda #2
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td>";
                            #endregion

                            #region Valores de la celda 3.

                            //Obetener los valores de la consulta, operarlos y formatear el resultado.
                            try
                            {
                                AporteDinero = Double.Parse(rsAux.Rows[i]["AporteDinero"].ToString());
                                AporteEspecie = Double.Parse(rsAux.Rows[i]["AporteEspecie"].ToString());
                                Suma_Aportes = (AporteDinero + AporteEspecie) / 1000;
                                //Suma_Aportes_Formateado = Suma_Aportes.ToString("0,0.00", CultureInfo.InvariantCulture);
                                Suma_Aportes_Formateado = FieldValidate.moneyFormat(Suma_Aportes); //string.Format("{0:0.00}", Suma_Aportes);
                            }
                            catch { }

                            //Abre y cierra la celda #3.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + Suma_Aportes_Formateado + "</td>";

                            #endregion

                            #region Valores de la celda 4.

                            //Obetener los valores de la consulta, operarlos y formatear el resultado.
                            try
                            {
                                AporteDinero = Double.Parse(rsAux.Rows[i]["AporteDinero"].ToString());
                                AporteDinero = AporteDinero / 1000;
                                //Suma_Aportes_Formateado = AporteDinero.ToString("0,0.00", CultureInfo.InvariantCulture);
                                Suma_Aportes_Formateado = FieldValidate.moneyFormat(AporteDinero);// string.Format("{0:0.00}", AporteDinero);
                            }
                            catch { }

                            //Abre y cierra la celda #4.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + Suma_Aportes_Formateado + "</td>";

                            #endregion

                            #region Valores de la celda 5.

                            //Obetener los valores de la consulta, operarlos y formatear el resultado.
                            try
                            {
                                AporteEspecie = Double.Parse(rsAux.Rows[i]["AporteEspecie"].ToString());
                                AporteEspecie = AporteEspecie / 1000;
                                //Suma_Aportes_Formateado = AporteDinero.ToString("0,0.00", CultureInfo.InvariantCulture);
                                Suma_Aportes_Formateado = FieldValidate.moneyFormat(AporteEspecie);// string.Format("{0:0.00}", AporteEspecie);
                            }
                            catch { }

                            //Abre y cierra la celda #5.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + Suma_Aportes_Formateado + "</td>";

                            #endregion

                            #region Valores de la celda 6.

                            //Abre y cierra la celda #6.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td>" + rsAux.Rows[i]["DetalleEspecie"].ToString() + "</td>";

                            #endregion

                            //Finalmente se cierra la fila.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>";
                            #endregion

                            #region Operar valores para obtener acumulados.

                            Total1 = Total1 + (AporteDinero);
                            Total2 = Total2 + (AporteEspecie);

                            #endregion
                        }
                        #endregion
                    }
                    else
                    { /*Como NO tiene datos, solo se bindean ciertos datos de la tabla.*/ }
                    #endregion

                    #region Al salir del ciclo, abre otra fila para mostrar los totales aportados y dibujar nueva fila.

                    //Vaciar la tabla "se ejecutará otra consulta que hace uso de esta misma variable".
                    rsAux = null;

                    try
                    {
                        //Sumar los totales "Total1" y "Total2" obtenidos.
                        Suma_Total1_Total2 = Total1 + Total2;

                        //Formatear la variable "Suma_Total1_Total2".
                        Suma_Total1_Total2_Formateado = FieldValidate.moneyFormat(Suma_Total1_Total2);

                        //Formatear la variable "Total1".
                        Total1_Formateado = FieldValidate.moneyFormat(Total1);// Total1.ToString("0.00", CultureInfo.InvariantCulture);

                        //Formatear la variable "Total2".
                        Total2_Formateado = FieldValidate.moneyFormat(Total2); // Total2.ToString("0.00", CultureInfo.InvariantCulture);

                    }
                    catch { /*Hubo un error.*/ }

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr bgcolor=\"#DDDDDD\">" +
                                                                                "<td align=\"right\" class=\"Titulo\" colspan=\"3\">" +
                                                                                "    Total Aportes" +
                                                                                "</td>" +
                                                                                "<td align=\"right\" class=\"Titulo\">" +
                                                                                Suma_Total1_Total2_Formateado +
                                                                                "</td>" +
                                                                                "<td align=\"right\" class=\"Titulo\">" +
                                                                                Total1_Formateado +
                                                                                "</td>" +
                                                                                "<td align=\"right\" class=\"Titulo\">" +
                                                                                Total2_Formateado +
                                                                                "</td>" +
                                                                                "<td>" +
                                                                                "    &nbsp;" +
                                                                                "</td>" +
                                                                            "</tr>";

                    #endregion

                    #region Nueva fila para agregar las observaciones.

                    //Consulta de observaciones.
                    txtSQL = " SELECT * FROM EvaluacionObservacion " +
                             " WHERE codProyecto = " + CodProyecto +
                             " AND codConvocatoria = " + CodConvocatoria;

                    //Asignar resultados a variable DataTable.
                    rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                    //Si tiene datos, asigna los valores consultados a variables internas para su visualización mas adelante.
                    if (rsAux.Rows.Count > 0)
                    {
                        sEquipoTrabajo = rsAux.Rows[0]["EquipoTrabajo"].ToString();
                        try { txtAux = Convert.ToDateTime(rsAux.Rows[0]["FechaCentralesRiesgo"].ToString()); }
                        catch { txtAux = new DateTime(); }
                        txtAux2 = rsAux.Rows[0]["CentralesRiesgo"].ToString();
                        //Destruir la variable.
                        rsAux = null;
                    }

                    //Tabla con las observaciones.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "  <tr><td colspan=\"7\">" +
                        " <table style='font-size: 10px' border=0 cellpadding='4' cellspacing='1'>" +
                        "     <tr>" +
                        "       <td>" +
                                    "<p>" +
                                    "<b>Observaciones Composición Grupo de Socios - Equipo de Trabajo:</b><br/>" +
                                    sEquipoTrabajo + "<br/>" +
                                    //" " + "<br/> " + //txtAux
                                    " " + //txtAux2
                        "         </p>" +
                        "     </td></tr>" +
                        " </table>" +
                    " </td></tr>";

                    #endregion

                    #region Generar nueva tabla "Informe centrales de riesgo".

                    //Concatenación.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</table><br/>" +
                    " <table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                    "     <tr>" +
                    "         <td>" +
                    "              <table style='font-size: 10px' width='100%' border='0' cellpadding='4' cellspacing='1'>" +
                    "              	<tr bgcolor='#000000'>		" +
                    "              		<td class='Blanca' colspan='2' align='center'><span style='color:#ffffff'>Informe Centrales De Riesgo</span></td>" +
                    "              		<td class='Blanca' colspan='2' align='right'><span style='color:#ffffff'>Fecha Reporte:</span></td>	" +
                    "              		<td class='Blanca' colspan='2' align='center'><span style='color:#ffffff'>" + txtAux.ToString("dd / " + UppercaseFirst(txtAux.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"))) + " / yyyy") + "</span></td>" +
                    "              	</tr>				" +
                    "              	<tr bgcolor='#000000'>" +
                    "              		<td class='Blanca' align='center'><span style='color:#ffffff'>Integrantes de la iniciativa Empresarial</span></td>	" +
                    "              		<td class='Blanca' align='center'><span style='color:#ffffff'>Entidades Fin. CarteraComercial, Consumo y Tarjeta de crédito</span></td>" +
                    "              		<td class='Blanca' align='center'><span style='color:#ffffff'>Valor Cartera Total (miles)</span></td>" +
                    "              		<td class='Blanca' align='center'><span style='color:#ffffff'>Peor Calificación</span></td>" +
                    "              		<td class='Blanca' align='center'><span style='color:#ffffff'>Ctas. Corrientes</span></td>" +
                    "              		<td class='Blanca' align='center'><span style='color:#ffffff'>Otras Carteras (miles)</span></td>" +
                    " </tr>";

                    #endregion

                    #region Continuación de la tabla mas generación de tabla interna.

                    //Consulta.
                    txtSQL = " SELECT C.Nombres + ' ' + C.Apellidos Emprendedor, Entidades, ValorCartera, ValorOtrasCarteras, " +
                             " PeorCalificacion, CuentasCorrientes " +
                             " FROM EvaluacionContacto E, Contacto C, ProyectoContacto P " +
                             " WHERE E.codContacto = C.id_contacto AND " +
                             " E.codProyecto = P.codProyecto AND " +
                             " P.inactivo = 0 AND P.codRol = " + Constantes.CONST_RolEmprendedor + " AND P.codContacto = C.id_Contacto AND " +
                             " E.codProyecto=" + CodProyecto + " AND E.codConvocatoria = " + CodConvocatoria;

                    //Asignar resultados de la consulta a variable DataTable.
                    rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                    //Recorrer resultados almacenados en la tabla para generar filas de la tabla HTML.
                    for (int j = 0; j < rsAux.Rows.Count; j++)
                    {
                        //Obtener "ValorCartera".
                        ValorCartera = Double.Parse(rsAux.Rows[j]["ValorCartera"].ToString());
                        //Operar el valor obtenido.
                        ValorCartera = ValorCartera / 1000;
                        //Formatear el valor obtenido.
                        //ValorCartera_Formateado = ValorCartera.ToString("0,0.00", CultureInfo.InvariantCulture);
                        ValorCartera_Formateado = FieldValidate.moneyFormat(ValorCartera); // string.Format("{0:0.00}", ValorCartera);

                        //Obtener "ValorOtrasCarteras".
                        ValorOtrasCarteras = Double.Parse(rsAux.Rows[j]["ValorCartera"].ToString());
                        //Operar el valor obtenido.
                        ValorOtrasCarteras = ValorOtrasCarteras / 1000;
                        //Formatear el valor obtenido.
                        //ValorOtrasCarteras_Formateado = ValorOtrasCarteras.ToString("0,0.00", CultureInfo.InvariantCulture);
                        ValorOtrasCarteras_Formateado = FieldValidate.moneyFormat(ValorOtrasCarteras); // string.Format("{0:0.00}", ValorOtrasCarteras);


                        //Generar tabla.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + " <tr>" +
                                                                                " 	<td align='left'>" + rsAux.Rows[j]["Emprendedor"].ToString() + "</td>" +
                                                                                " 	<td align='left'>" + rsAux.Rows[j]["Entidades"].ToString() + "</td>	" +
                                                                                " 	<td align='right'>" + ValorCartera_Formateado + "</td>" +
                                                                                " 	<td align='center'>" + rsAux.Rows[j]["PeorCalificacion"].ToString() + "</td>" +
                                                                                " 	<td align='center'>" + rsAux.Rows[j]["CuentasCorrientes"].ToString() + "</td>" +
                                                                                " 	<td align='right'>" + ValorOtrasCarteras_Formateado + "</td>" +
                                                                                " </tr>";
                    }

                    #endregion

                    //Cerrar toda la tabla = FIN.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</table>" +
                                                                            " </td></tr>" +
                                                                            " <tr><td>" +
                                                                            " 	<table style='font-size: 10px' border=0 cellpadding='4' cellspacing='1'>" +
                                                                            " 		<tr><td height='50' valign='top'><p><b>Observaciones Reporte Centrales de Riesgo:</b><br>" + txtAux2 + "</p></td></tr>" +
                                                                            " 	</table>" +
                                                                            " </td></tr>" +
                                                                            "</table>";

                    //Destruir tabla inicial.
                    rs = null;

                    #endregion
                }
                else
                {
                    //De lo contrario, no mostrará nada "según FONADE clásico".
                    lbl_tr_emprendedores.Text = "";
                }
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error en el encabezado: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la segunda tabla.
        /// </summary>
        private void GenerarSegundaTabla()
        {
            try
            {
                //Obtener tabla.
                tabla_resultado = ObservacionEvaluacion();

                //Recorrer la tabla para dibujar resultados en HTML.
                for (int i = 0; i < 6; i++)
                {
                    //Dibujando la tabla de acuerdo a la posición en fila y columna respectivamente.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + " <table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                                                            "     <tr><td colspan='7'>" +
                                                                            "         <table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>" +
                                                                            "             <tr><td bgcolor='#000000' height='30'><span style='color:#ffffff'>" + tabla_resultado.Columns[i].ColumnName + "</span></td></tr>" +
                                                                            "             <tr><td height='30'>" + tabla_resultado.Rows[0][i].ToString() + "</td></tr>" +
                                                                            "         </table>" +
                                                                            "     </td></tr>		" +
                                                                            " </table>		" +
                                                                            " <br/> ";
                }

                //Destruir DataTable "para no mantener valores en memoria".
                tabla_resultado = null;
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #2: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la tercera tabla.
        /// </summary>
        private void GenerarTerceraTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();

            try
            {
                //Reiniciar variable para generar las consultas en SQL.
                txtSQL = "";

                //Iniciando con la generación de la tabla.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                                                            "<tr><td colspan='7'>" +
                                                                            "	<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1' class='zebra'>" +
                                                                            "<tr height='30'><td bgcolor='#000000' colspan=" + (TiempoProyeccion + 2) + " class='TituloTabla' height='30'><span style='color:#ffffff'>Evaluación del proyecto</span></td></tr>" +
                                                                            "<tr height='30'bgcolor='#000000'><td width='200' class='TituloTabla'><span style='color:#ffffff'>Variable</span></td>";

                //Generar celdas de encabezado de la tabla interna por cada año obtenido en "TiempoProyeccion".
                for (int i = 1; i < TiempoProyeccion + 1; i++) //+1 para que encaje con los resultados de los años.
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td class='TituloTabla' align='center'><span style='color:#ffffff'>Año " + i + "</span></td>";
                    //"Concatenar" las partes que formarán enteramente la consulta SQL mas adelante a ejecutar.
                    txtSQL = txtSQL + ", " + "SUM( CASE isnull(periodo, 0) when " + i + " then isnull(V.Valor, 0) else 0 end) Valor" + i;
                }

                //Después del ciclo, se cierran las celdas.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td class='TituloTabla' align='center'><span style='color:#ffffff'>Promedio<br>sector</span></td>" +
                                                                        "</tr>";

                #region Establecer consulta SQL, asignar valores a la variable "rsAux" y ejecutar el código.
                txtSQL = " SELECT T.nomTipoSupuesto tipo, S.nomEvaluacionProyectoSupuesto Nombre "
                         + txtSQL + ", SUM( CASE isnull(periodo, 0) when 0 then isnull(V.Valor, 0) else 0 end) Promedio " +
                         " FROM evaluacionProyectoSupuesto S " +
                         " INNER JOIN TipoSupuesto T ON T.id_TipoSupuesto = S.codTipoSupuesto " +
                         " LEFT JOIN  evaluacionProyectoSupuestoValor V ON S.Id_EvaluacionProyectoSupuesto = V.codSupuesto " +
                         " WHERE S.codProyecto=" + CodProyecto + " and codConvocatoria = " + CodConvocatoria + " " +
                         " GROUP BY T.nomTipoSupuesto, S.nomEvaluacionProyectoSupuesto " +
                         " ORDER BY T.nomTipoSupuesto, S.nomEvaluacionProyectoSupuesto";

                //Declarar variable.
                txtVariable = "";

                //Asignar resultados de la consulta a variable DataTable.
                rsAux = new DataTable();
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                #region Recorrer la tabla.
                foreach (DataRow row in rsAux.Rows)
                {
                    if (txtVariable != row["Tipo"].ToString())
                    {
                        if (txtVariable.Trim() != "")
                        {
                            lbl_tr_emprendedores.Text =
                                lbl_tr_emprendedores.Text + "<tr><td colspan='" + (TiempoProyeccion + 1) + "'><br/></td></tr>";
                        }

                        //Concatenar mas HTML.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr><td colspan='" + (TiempoProyeccion + 1) + "' class='titulo'>Supuestos " + row["Tipo"].ToString() + "</td></tr>";
                        //Asignar valor.
                        txtVariable = row["Tipo"].ToString();
                    }

                    //Al salir del if, sigue concatenando mas HTML.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>" +
                                "<td>" + row["Nombre"].ToString() + "</td>";

                    //Generar segundo recorrido.
                    for (int k = 1; k < TiempoProyeccion + 1; k++)
                    { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + row["Valor" + k].ToString() + "</td>"; }

                    //Antes de terminar el foreach, cierra las celdas y filas generadas.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + row["Promedio"].ToString() + "</td>" +
                                                                            "</tr>";
                }
                #endregion

                //Cierra la tercera tabla generada.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</table>" +
                    "<br/>";

                //Destruir variables.
                rsAux = null;
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #3: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la cuarta tabla.
        /// </summary>
        private void GenerarCuartaTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();

            try
            {
                //Iniciar con la generación de la cuarta tabla interna.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1' class='zebra' >" +
                                                                            "<tr height='30' bgcolor='#000000'><td bgcolor='#000000' width='230' class='TituloTabla'><span style='color:#ffffff'>Indicadores Financieros proyectados</span></td>";

                //Inicializar la variable de consultas SQL.
                txtSQL = "";

                //Generar celdas de encabezado de la tabla interna por cada año obtenido en "TiempoProyeccion".
                for (int i = 1; i < TiempoProyeccion + 1; i++) //+1 para que encaje con los resultados de los años.
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td bgcolor='#000000' class='TituloTabla' align='center'><span style='color:#ffffff'>Año " + i + "</span></td>";
                    //"Concatenar" las partes que formarán enteramente la consulta SQL mas adelante a ejecutar.
                    txtSQL = txtSQL + ", " + "SUM( CASE isnull(periodo, 0) when " + i + " then isnull(V.Valor, 0) else 0 end) Valor" + i;
                }

                //Al salir, cierra el <tr>.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>";

                #region Generar consulta y asignar resultados a la variable "rsAux".

                txtSQL = "SELECT S.Descripcion Nombre " + txtSQL +
                        " FROM EvaluacionIndicadorFinancieroProyecto S  " +
                        " LEFT JOIN  EvaluacionIndicadorFinancieroValor V " +
                        " ON S.Id_EvaluacionIndicadorFinancieroProyecto = V.codEvaluacionIndicadorFinancieroProyecto " +
                        " WHERE S.codProyecto=" + CodProyecto + " and codConvocatoria = " + CodConvocatoria +
                        " GROUP BY S.Descripcion " +
                        " ORDER BY S.Descripcion";
                txtVariable = "";

                //Inicializar variables.
                rsAux = new DataTable();
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                #endregion

                #region Recorrer tabla.
                foreach (DataRow row in rsAux.Rows)
                {
                    //Generar fila y celdas de la fila.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>" +
                                                                                "<td>" + row["Nombre"].ToString() + "</td>";

                    //Generar segundo recorrido.
                    for (int k = 1; k < TiempoProyeccion + 1; k++)
                    { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + row["Valor" + k].ToString() + "</td>"; }

                    //Al salir del for anterior, cierra la fila.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>";
                }
                #endregion

                //Al salir del recorrido de la tabla, cierra la tabla interna.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + " </table>					" +
                                                                        " 	</td></tr>			" +
                                                                        " </table>		" +
                                                                        " <br/>";
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #4: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la quinta tabla.
        /// </summary>
        private void GenerarQuintaTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();

            try
            {
                //Iniciar con la generación de la tabla interna.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                " <tr><td colspan='7'>" +
                " 	<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1' class='zebra'>" +
                " 		<tr height='30'><td bgcolor='#000000' colspan='" + (TiempoProyeccion + 1) + "'class='TituloTabla' height='30'><span style='color:#ffffff'>Flujo de caja Proyectado y rentabilidad Cifras en miles de pesos</span></td></tr>" +
                " 		<tr height='30' bgcolor='#000000'><td width='230' class='TituloTabla'><span style='color:#ffffff'>Rubro</span></td>";

                //Inicializar la variable de consultas SQL.
                txtSQL = "";

                //Generar celdas de encabezado de la tabla interna por cada año obtenido en "TiempoProyeccion".
                for (int i = 1; i < TiempoProyeccion + 1; i++) //+1 para que encaje con los resultados de los años.
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td class='TituloTabla' align='center'><span style='color:#ffffff'>Año " + i + "</span></td>";
                    //"Concatenar" las partes que formarán enteramente la consulta SQL mas adelante a ejecutar.
                    txtSQL = txtSQL + ", " + "SUM( CASE isnull(periodo, 0) when " + i + " then isnull(V.Valor, 0) else 0 end) Valor" + i;
                }

                //Al salir del ciclo, cierra la fila.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>";

                #region Generar consulta y asignar resultados a la variable "rsAux".
                txtSQL = " SELECT S.Descripcion Nombre " + txtSQL +
                                 " FROM EvaluacionRubroProyecto S " +
                                 " LEFT JOIN EvaluacionRubroValor V ON S.Id_EvaluacionRubroProyecto = V.codEvaluacionRubroProyecto " +
                                 " WHERE S.codProyecto=" + CodProyecto + " and codConvocatoria = " + CodConvocatoria +
                                 " GROUP BY S.Descripcion " +
                                 " ORDER BY S.Descripcion";

                //Inicializar la variable "txtVariable".
                txtVariable = "";

                //Asignar resultados de la consulta a variable DataTable.
                rsAux = new DataTable();
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                #region Recorrer la tabla.
                foreach (DataRow row in rsAux.Rows)
                {
                    //Generar HTML.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>" +
                                                                                "<td>" + row["Nombre"].ToString() + "</td>";

                    //Generar segundo recorrido.
                    for (int k = 1; k < TiempoProyeccion + 1; k++)
                    { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='right'>" + row["Valor" + k].ToString() + "</td>"; }

                    //Antes de salir del ciclo, se cierra la fila.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>";
                }
                #endregion

                //Al salir del recorrido de la tabla, cierra la tabla interna.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</table>" +
                                                                        "	</td></tr>			" +
                                                                        "</table>		" +
                                                                        "<br/>";
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #5: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la sexta tabla.
        /// </summary>
        private void GenerarSextaTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();
            Double Valor = 0;
            String Valor_Formateado = "0";

            try
            {
                //Iniciar con la generación de la tabla.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                                                            "<tr><td>" +
                                                                            "	<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1' class='zebra'>" +
                                                                            "		<tr height='30'><td bgcolor='#000000' colspan='3' class='TituloTabla' height='30'><span style='color:#ffffff'>Indicadores de Gestión</span></td></tr>" +
                                                                            "		<tr bgcolor='#000000' colspan='2' height='30'>" +
                                                                            "			<td width='13'>&nbsp;</td>" +
                                                                            "			<td class='TituloTabla'><span style='color:#ffffff'>Descripción</span></td>" +
                                                                            "			<td class='TituloTabla' align='Center'><span style='color:#ffffff'>Valor</span></td>" +
                                                                            "		</tr>";

                #region Generar consulta y asignar resultados a la variable "rsAux".
                txtSQL = " SELECT id_indicador, Descripcion, Tipo, Valor, Protegido " +
                         " FROM EvaluacionProyectoIndicador WHERE codProyecto = " + CodProyecto +
                         " AND codConvocatoria = " + CodConvocatoria;

                //Asignar resultados de la consulta a variable DataTable.
                rsAux = new DataTable();
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                #region Recorrer la tabla.
                foreach (DataRow row in rsAux.Rows)
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>" +
                            "<td>&nbsp;</td>" +
                            "<td>" + row["Descripcion"].ToString() + "</td>" +
                            "<td align='right'>";

                    //Formatear el campo "Valor".
                    Valor = Double.Parse(row["Valor"].ToString());

                    #region Condicionar la información a dibujar en HTML.
                    if (row["Tipo"].ToString() == "$")
                    {
                        //Valor_Formateado = "<b>$</b> " + Valor.ToString("0,0.00", CultureInfo.InvariantCulture);
                        Valor_Formateado = "<b>$</b> " + FieldValidate.moneyFormat( Valor,false);
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + Valor_Formateado;
                    }
                    if (row["Tipo"].ToString() == "%")
                    {
                        //Valor_Formateado = Valor.ToString("0,0.00", CultureInfo.InvariantCulture) + " <b>%</b>";
                        Valor_Formateado = FieldValidate.moneyFormat(Valor,false) + " <b>%</b>";
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + Valor_Formateado;
                    }
                    if (row["Tipo"].ToString() == "#")
                    {
                        //Valor_Formateado = Valor.ToString("0,0.00", CultureInfo.InvariantCulture);
                        Valor_Formateado = FieldValidate.moneyFormat(Valor,false);
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + Valor_Formateado;
                    }
                    #endregion
                }
                #endregion

                //Al salir del recorrido, cierra la tabla y genera una sub-tabla con mas datos.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</table>" +
                                                                        "	</td></tr>" +
                                                                        "	<tr><td>" +
                                                                        "		<table style='font-size: 10px' border=0 cellpadding='4' cellspacing='1'>" +
                                                                        "			<tr><td height='50' valign='top'><p><b>Conclusiones Flujo de caja Proyectado y Rentabilidad Esperada del proyecto:</b><br/>" + txtConclusionesFinancieras + "</p></td></tr>" +
                                                                        "		</table>				" +
                                                                        "	</td></tr>" +
                                                                        "</table>		" +
                                                                        "<br/>";

            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #6: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la séptima tabla "contiene dos tablas".
        /// </summary>
        private void GenerarSeptimaTabla()
        {
            //Inicializar variables.
            DataTable rs = new DataTable();

            try
            {
                #region Tabla 1.
                //Iniciar con la generación de la tabla interna.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                "<tr><td>" +
                "	<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>" +
                "		<tr height='30'><td bgcolor='#000000' colspan='3' class='TituloTabla' height='30'><span style='color:#ffffff'>Riesgos Identificados y mitigaci&oacute;n</span></td></tr>" +
                "		<tr class='Titulo' bgcolor='#000000'>" +
                "			<td width='15'>&nbsp;</td>" +
                "			<td width='49%' align='left' class='tituloTabla'><span style='color:#ffffff'>Riesgo</span></td>" +
                "			<td width='49%' align='left' class='tituloTabla'><span style='color:#ffffff'>Mitigaci&oacute;n</span></td>" +
                "		</tr>";

                //Generar consulta.
                txtSQL = " SELECT id_Riesgo, Riesgo, Mitigacion " +
                         " FROM EvaluacionRiesgo " +
                         " WHERE codProyecto = " + CodProyecto + " and codConvocatoria = " + CodConvocatoria +
                         " ORDER BY id_Riesgo";

                //Asignar resultados a la variable DataTable.
                rs = new DataTable();
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Recorrer tabla.
                foreach (DataRow row in rs.Rows)
                {
                    //Generar fila y celdas respectivamente.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>			" +
                        "		<td>&nbsp;</td>" +
                        "		<td>" + row["Riesgo"].ToString() + "</td>" +
                        "		<td>" + row["Mitigacion"].ToString() + "</td>" +
                        "	</tr>";
                }

                //Al terminar de recorrer la tabla, se cierra la tabla.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "   </table>				" +
                                                                        " 	</td></tr>" +
                                                                        " </table>" +
                                                                        " <br/>";
                #endregion

                #region Tabla 2.

                //Generar tabla menor.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                                                      "	<tr><td colspan='7'>" +
                                                                      "		<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>" +
                                                                      "			<tr><td bgcolor='#000000' class='TituloTabla' height='30'><span style='color:#ffffff'>Resumen concepto General - Compromisos y Condiciones</span></td></tr>" +
                                                                      "			<tr><td height='30'>" + txtGenerales + "</td></tr>" +
                                                                      "		</table>" +
                                                                      "	</td></tr>		" +
                                                                      "</table>			" +
                                                                      "<br/>";

                #endregion
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #7: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 30/05/2014.
        /// Cargar la octava tabla "contiene varias sub-tablas".
        /// Ésta tabla genera una tabla de tamaño, contiene observaciones y puntajes de aprobación
        /// por cada "Campo".
        /// </summary>
        private void GenerarOctavaTabla()
        {
            //Inicializar variables.
            Double numGranTotal = 0;
            DataTable rsAux = new DataTable();
            DataTable rs = new DataTable();
            String codTempAspecto = "";
            long numTotal = 0;
            DataTable rsJust = new DataTable();
            String txtMensaje;

            try
            {
                //Inicializar la variable "".
                numGranTotal = 0;

                //Generar consulta sql.
                txtSQL = " SELECT id_campo, Campo, Puntaje " +
                         " FROM Campo C, ConvocatoriaCampo CC " +
                         " WHERE C.id_Campo = CC.codcampo and C.codCampo is null AND codConvocatoria = " + CodConvocatoria;

                //Asignar resultados a la variable DataTable.
                rsAux = new DataTable();
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                #region Iniciar con el ciclo grande.

                //Recorrer la tabla.
                for (int i = 0; i < rsAux.Rows.Count; i++)
                {
                    //Inicio de la tabla.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                                                                "<tr><td>" +
                                                                                    "<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>			" +
                                                                                            "<tr height='30' bgcolor='#000000'><td class='tituloTabla' colspan='4'><span style='color:#ffffff'>" + rsAux.Rows[i]["Campo"].ToString() + "</span></td></tr>";

                    //Obtener el código temporal del aspecto.
                    codTempAspecto = rsAux.Rows[i]["id_campo"].ToString();

                    //Ejecutar consulta con el "codTempAspecto" cargado.
                    txtSQL = " SELECT C.id_campo, c.campo, p.id_campo as idVariable, case when cc.Puntaje is null then c.campo else p.campo end orden, cc.Puntaje Maximo, isnull(ec.Puntaje, 0) Asignado " +
                             " FROM CAMPO C " +
                             " LEFT JOIN CAMPO P ON c.codcampo=p.id_campo " +
                             " INNER JOIN ConvocatoriaCampo CC ON C.id_campo = CC.codCampo AND " +
                             " C.Inactivo = 0 AND ( p.codcampo=" + codTempAspecto + " OR c.codcampo=" + codTempAspecto + ") AND " +
                             " CC.codConvocatoria=" + CodConvocatoria +
                             " LEFT JOIN EvaluacionCampo EC ON C.id_campo = EC.codCampo AND " +
                             " CC.codConvocatoria=EC.codConvocatoria  AND " +
                             " EC.codProyecto=" + CodProyecto + " " +
                             " ORDER BY orden, maximo ";

                    //Asignar resultados de la consulta a DataTable "rs".
                    rs = consultas.ObtenerDataTable(txtSQL, "text");

                    #region Generar fila con celdas de "Puntaje" y/o "Máximo".
                    //Generar esta fila con las siguientes celdas.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr bgcolor='#000000'>" +
                            "<td width='3%'  class='tituloTabla'>&nbsp;</td>" +
                            "<td width='77%' class='tituloTabla'><span style='color:#ffffff'>Campo</span></td>";

                    //Si es diferente a "Medio Ambiente", generará las celdas "Puntaje" y "Máximo".
                    if (rsAux.Rows[i]["Campo"].ToString() != "Medio Ambiente")
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td width='10%' class='tituloTabla' align='center'><span style='color:#ffffff'>Puntaje</span></td>" +
                            "<td width='10%' class='tituloTabla' align='center'><span style='color:#ffffff'>Máximo</span></td>";
                    }
                    else /*De lo contrario sólo generará la celda de "Puntaje".*/
                    { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td width='10%' class='tituloTabla' align='center' colspan='2'><span style='color:#ffffff'>Puntaje</span></td>"; }

                    //Cerrar fila con celdas.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td>" +
                        "</tr>";
                    #endregion

                    #region Recorrer "rs".

                    //Inicializar la variable que contendrá el puntaje de aprobación.
                    numTotal = 0;

                    for (int j = 0; j < rs.Rows.Count; j++)
                    {
                        //Sumar el valor "Asignado"
                        numTotal = numTotal + long.Parse(rs.Rows[j]["Asignado"].ToString());

                        //Si el campo "txtVariable" es diferente al contenido del campo "Orden".
                        if (txtVariable != rs.Rows[j]["Orden"].ToString())
                        {
                            //Asignar el campo "Nombre" a la variable "txtVariable".
                            txtVariable = rs.Rows[j]["Orden"].ToString();

                            //1.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                                "<tr><td colspan='4'class='titulo'>" + rs.Rows[j]["Orden"].ToString() + "</td></tr>" +
                                "<tr><td class='titulo' height='8'>&nbsp;</td>" +
                                "	<td colspan='4'class='titulo'>" +
                                "	<table style='font-size: 10px' width='100%' border='1' cellpadding='4' cellspacing='0' bordercolor='#000000'>" +
                                "		<tr><td width='100%' valign='top'><b>Observaciones:</b><br/>";

                            //Generar Justificación.
                            txtSQL = " SELECT Justificacion " +
                                    " FROM EvaluacionCampoJustificacion " +
                                    " WHERE codProyecto = " + CodProyecto + " and codconvocatoria = " + CodConvocatoria + " and codCampo = " + rs.Rows[j]["idVariable"].ToString();

                            rsJust = consultas.ObtenerDataTable(txtSQL, "text");
                            if (rsJust.Rows.Count > 0)
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + rsJust.Rows[0]["Justificacion"].ToString(); }
                            rsJust = null;

                            //Antes de cerrar el if, cierra las filas columnas y tabla interna de observaciones.
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td></tr>" +
                                    "</table>" +
                                "</td></tr>";
                        }

                        //Al salir del IF, genera una nueva fila.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr>" +
                                "<td>&nbsp;</td>" +
                                "<td>" + rs.Rows[j]["Campo"].ToString() + "</td>";

                        //Si el "Campo" es diferente a "Medio Ambiente".
                        if (rsAux.Rows[i]["Campo"].ToString() != "Medio Ambiente")
                        {
                            //Mostrará las celdas "Asignado y Máximo" y "Puntaje".
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                                "<td align='center'>" + rs.Rows[j]["Asignado"].ToString() + "</td>" +
                                "<td align='center'>" + rs.Rows[j]["Maximo"].ToString() + "</td>";
                        }
                        else
                        {
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='center' colspan='2'>";
                            if (rs.Rows[j]["Asignado"].ToString() == "0")
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "NO"; }
                            else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "SI"; }
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td>";
                        }
                    }

                    #endregion

                    #region Generar demás líneas de código.

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<tr><td colspan='4' height='1'><HR noshade size='1'></td></tr>" +
                        "<tr>";

                    //Se vacía la variable "txtMensaje".
                    txtMensaje = "";

                    //Si el número totals es menor al puntaje, la variable "txtMensaje" quedará con el valor "No ".
                    if (numTotal < long.Parse(rsAux.Rows[i]["Puntaje"].ToString())) { txtMensaje = "No "; }

                    //Generar nueva fila.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<td class='Titulo' align='right' colspan='2' > " + txtMensaje + "Aprobado</td>";
                    if (rsAux.Rows[i]["Campo"].ToString() != "Medio Ambiente")
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<td class='Titulo' align='center'>" + numTotal + "</td>" +
                                "<td>&nbsp;</td>";
                    }
                    else
                    {
                        try
                        {
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='center' colspan='2'>";
                            if (numTotal == 0)
                            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "NO"; }
                            else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "SI"; }
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td>";
                        }
                        catch { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + ""; }
                    }

                    #region Al salir de la condición "if", genera estas líneas.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "</tr><tr><td colspan='2' align='right'>M&iacute;nimo aprobatorio: </td>";
                    if (rsAux.Rows[i]["Campo"].ToString() != "Medio Ambiente")
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<td align='center'>" + rsAux.Rows[i]["Puntaje"].ToString() + "</td>" +
                                "<td>&nbsp;</td>";
                    }
                    else
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='center'>";
                        if (rsAux.Rows[i]["Puntaje"].ToString() == "0")
                        { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "NO"; }
                        else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "SI"; }
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</td>";
                    }
                    #endregion

                    #region Al salir del "if" anterior, genera estas líneas.

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>" +
                        "<tr><td colspan='4'>&nbsp;</td></tr>" +
                    "</table>" +
                "</tr>" +
            "</table><br/>";

                    #endregion

                    //Agregar los valores obtenidos a la variable "numGranTotal".
                    numGranTotal = numGranTotal + numTotal;

                    #endregion
                }

                #endregion
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error #8: " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/06/2014.
        /// Genera la tabla que en ejecución tiene por título "Resumen Concepto de Evaluación".
        /// </summary>
        private void GenerarNovenaTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();
            long numTotal = 0;
            Double numGranTotal = 0;

            try
            {
                #region Generar inicio de la tabla:
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                                "<tr><td>" +
                                    "<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1' class='zebra'>			" +
                                        "<tr height='30' align='center' bgcolor='#000000'><td bgcolor='#000000' class='tituloTabla' colspan='4'><span style='color:#ffffff'>Resumen Concepto de Evaluaci&oacute;n</span></td></tr>" +
                                        "<tr bgcolor='#000000'>" +
                                        "	<td width='3%'  class='tituloTabla'>&nbsp;</td>" +
                                        "	<td width='77%' class='tituloTabla'><span style='color:#ffffff'>aspecto</span></td>" +
                                        "	<td width='10%' class='tituloTabla' align='center'><span style='color:#ffffff'>Puntaje Obtenido</span></td>" +
                                        "	<td width='10%' class='tituloTabla' align='center'><span style='color:#ffffff'>Mínimo Aceptable</span></td>" +
                                        "</tr>						";
                #endregion

                #region Generar consulta SQL y asignar resultados a la variable "rsAux".
                txtSQL = " SELECT X.idAspecto, X.Campo, D.puntaje as Minimo, isnull(sum(EC.Puntaje), 0) Total " +
                         " FROM ConvocatoriaCampo CC " +
                         " INNER JOIN (	SELECT P.Campo, P.id_campo idAspecto, V.id_Campo idVariable, C.id_Campo idCampo " +
                         " FROM Campo P " +
                         " LEFT  JOIN Campo V ON P.id_Campo = V.codCampo " +
                         " LEFT  JOIN Campo C ON V.id_Campo = C.codCampo " +
                         " WHERE C.id_Campo is not null and V.id_Campo is not null ) X " +
                         " ON CC.codCampo = X.idCampo and " +
                         " CC.codConvocatoria = " + CodConvocatoria + " " +
                         " INNER JOIN ConvocatoriaCampo D ON X.idAspecto = D.codCampo and CC.codConvocatoria = D.codConvocatoria " +
                         " LEFT JOIN EvaluacionCampo EC " +
                         " ON CC.codConvocatoria = EC.codConvocatoria and " +
                         " EC.codProyecto = " + CodProyecto + " and " +
                         " X.idCampo = EC.codCampo " +
                         " GROUP BY X.idAspecto, X.Campo, D.puntaje " +
                         " ORDER BY X.idAspecto ";

                //Inicializar las variables.
                numTotal = 0;
                numGranTotal = 0;

                //Asignar resultados de la consulta a variable DataTable.
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                #region Recorrer variable "rsAux".

                foreach (DataRow row_rsAux in rsAux.Rows)
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<tr><td width='3%' class='tituloTabla'>&nbsp;</td>" +
                                "<td>" + row_rsAux["Campo"].ToString() + "</td>";

                    if (row_rsAux["Campo"].ToString() != "Medio Ambiente")
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<td align='center'>" + row_rsAux["Total"].ToString() + "</td>" +
                                "<td align='center'>" + row_rsAux["Minimo"].ToString() + "</td>";
                    }
                    else
                    {
                        //Total.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='center'>";
                        if (row_rsAux["Total"].ToString() == "0") { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "NO"; }
                        else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "SI"; }

                        //Mínimo.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<td align='center'>";
                        if (row_rsAux["Minimo"].ToString() == "0") { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "NO"; }
                        else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "SI"; }
                    }

                    //Agregar valores obtenidos a la variable "numTotal".
                    numTotal = numTotal + long.Parse(row_rsAux["Total"].ToString());
                    numGranTotal = numGranTotal + Double.Parse(row_rsAux["Minimo"].ToString());
                }

                #endregion

                #region Generar líneas de código que cierran la tabla interna.

                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<tr bgcolor='#000000' ><td bgcolor='#000000' width='3%' class='tituloTabla'>&nbsp;</td>" +
                            "<td bgcolor='#000000' align='right'  class='titulo'><span style='color:#ffffff'>Resultado General</span> </td>" +
                            "<td bgcolor='#000000' align='center' class='titulo'><span style='color:#ffffff'>" + numTotal + "</span></td>" +
                            "<td bgcolor='#000000' align='center' class='titulo'><span style='color:#ffffff'>" + numGranTotal + "</span></td>" +
                        "</tr>" +
                    "</table>" +
                "</tr>" +
            "</table>";

                #endregion
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error en la generación de las tabla ''Resumen Concepto de Evaluación'': " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/06/2014.
        /// Generar la décima tabla con título "¿Se recomienda su aprobación?".
        /// </summary>
        private void GenerarDecimaTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();

            try
            {
                #region Crear consulta SQL y asignar resultados a la variable "rsAux".
                txtSQL = " SELECT isnull(Justificacion, ' ') Justificacion, " +
                         " case when isnull(Viable, 0) = 1 then 'SI' else 'NO' end Aprobado " +
                         " FROM ConvocatoriaProyecto WHERE codConvocatoria = " + CodConvocatoria +
                         " AND codProyecto = " + CodProyecto;

                //Asignar resultados de la consulta a la variable "rsAux".
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                foreach (DataRow row_rsAux in rsAux.Rows)
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<br/>" +
                        "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                        "	<tr><td>" +
                        "		<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1' class='zebra'>" +
                        "			<tr><td bgcolor='#000000' class='TituloTabla' height='30'><span style='color:#ffffff'>¿Se recomienda su aprobación? " + row_rsAux["Aprobado"].ToString() + "</span></td></tr>" +
                        "			<tr><td height='20'><b>Justificaci&oacute;n: </b><br/>" + row_rsAux["Justificacion"].ToString() + "</td></tr>" +
                        "		</table>			" +
                        "	</tr>" +
                        "</table>";
                }
                //Destruir variables.
                rsAux = null;
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error en la generación de la tabla ''¿Se recomienda su aprobación?'': " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/06/2014.
        /// Generar undécima tabla.
        /// </summary>
        private void GenerarUndecimaTabla()
        {
            //Inicializar variables.
            DataTable rsAux = new DataTable();

            try
            {
                #region Iniciar con la estructura de la tabla.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<br/>" +
                    "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                        "<tr><td colspan='7'>" +
                            "<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>" +
                                "<tr><td colspan='7' bgcolor='#000000' class='TituloTabla' height='30'><span style='color:#ffffff'>Indicadores de Gestión</span></td></tr>" +
                                "<tr class='Titulo' bgcolor='#000000'>" +
                                "	<td width='3%'>&nbsp;</td>		" +
                                "	<td width='30%' align='left' class='tituloTabla'><span style='color:#ffffff'>Aspecto</span></td>" +
                                "	<td width='8%' align='center' class='tituloTabla'><span style='color:#ffffff'>Fecha de Seguimiento</span></td>" +
                                "	<td width='25%' align='Center' class='tituloTabla'><span style='color:#ffffff'>Indicador</span></td>" +
                                "	<td width='25%' align='left' class='tituloTabla'><span style='color:#ffffff'>Descripción del Indicador</span></td>" +
                                "	<td width='9%' align='Center' class='tituloTabla'><span style='color:#ffffff'>Rango aceptable</span></td>" +
                                "</tr>			";
                #endregion

                #region Consulta SQL y asignar los resultados a la variable "rsAux".
                txtSQL = " SELECT * FROM EvaluacionIndicadorGestion " +
                         " WHERE codProyecto = " + CodProyecto +
                         " AND codConvocatoria = " + CodConvocatoria;

                //Asignar los resultados de la consulta a variable "rsAux".
                rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                #region Recorrer variable "rsAux".
                foreach (DataRow row_rsAux in rsAux.Rows)
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<tr>" +
                                "	<td></td>" +
                                "	<td>" + row_rsAux["Aspecto"].ToString() + "</td>" +
                                "	<td align='center'>" + row_rsAux["FechaSeguimiento"].ToString() + "</td>" +
                                "	<td align='center'>" + row_rsAux["Numerador"].ToString() + "<hr noshade color='#000000'>" + row_rsAux["Denominador"].ToString() + "</td>" +
                                "	<td>" + row_rsAux["Descripcion"].ToString() + "</td>" +
                                "	<td align='center'>" + row_rsAux["RangoAceptable"].ToString() + " %</td>" +
                                "</tr>";
                }
                #endregion

                //Al salir del ciclo, añade las siguientes líneas.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "		</table>" +
                    "	</td></tr>	" +
                    "</table>" +
                    "<br/>";

                //Destruir variables.
                rsAux = null;
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error en la generación de la tabla: ''Indicadores de Gestión'': " + ex.Message + "</strong>"; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/06/2014.
        /// Generar duodécima (y última(s)) tabla(s).
        /// </summary>
        private void GenerarDuodecimaTabla()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            String txtSolicitado = "";
            Double txtRecomendado = 0;
            String txtEquipoTrabajo = "";
            bool EsMiembro = false;
            bool bRealizado = false;
            String stxtClass;
            String sReadOnly;
            Double Total = 0;
            Double numtotal = 0;
            DataTable rsAux = new DataTable();
            DataTable tabla_SMLV = new DataTable();
            Double SMLV_Obtenido = 0;
            Double d_AporteDinero = 0;
            Double d_AporteEspecie = 0;

            try
            {
                #region Consultar el SMLV del año actual.

                txtSQL = " SELECT SalarioMinimo FROM SalariosMinimos WHERE [AñoSalario] = " + DateTime.Today.Year;
                tabla_SMLV = consultas.ObtenerDataTable(txtSQL, "text");
                if (tabla_SMLV.Rows.Count > 0) { SMLV_Obtenido = Double.Parse(tabla_SMLV.Rows[0]["SalarioMinimo"].ToString()); }
                else { SMLV_Obtenido = 616027; }
                tabla_SMLV = null;
                #endregion

                #region Generar el inicio de la tabla.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                    "<tr>" +
                        "<td colspan='7'>" +
                            "<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>" +
                            "	<tr>" +
                            "		<td colspan='7' bgcolor='#000000' class='TituloTabla' height='30'><span style='color:#ffffff'>Aportes</span></td>" +
                            "	</tr>" +
                            "</table>" +
                            "<table width='100%' border='0' cellspacing='0' cellpadding='0' >" +
                              "<tr> " +
                                "<td width='0'></td>" +
                                "<td width='100%' align='center'>";
                #endregion

                #region Consulta SQL y obtener los valores de las variables "txtSolicitado", "txtRecomendado" y "txtEquipoTrabajo"

                txtSQL = " SELECT P.Recursos, C.valorRecomendado, C.EquipoTrabajo " +
                         " FROM ProyectoFinanzasIngresos P LEFT JOIN EvaluacionObservacion C " +
                         " ON P.codProyecto = C.codProyecto and C.codConvocatoria = " + CodConvocatoria +
                         " WHERE P.codproyecto=" + CodProyecto;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    txtSolicitado = RS.Rows[0]["Recursos"].ToString();
                    try { txtRecomendado = Double.Parse(RS.Rows[0]["valorRecomendado"].ToString()); }
                    catch { txtRecomendado = 0; }
                    txtEquipoTrabajo = RS.Rows[0]["EquipoTrabajo"].ToString();
                }

                RS = null;

                #endregion

                #region Establecer los valores de las variables "stxtClass" y "sReadOnly".
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodProyecto);

                if (EsMiembro == true && bRealizado == false && usuario.CodGrupo == Constantes.CONST_Evaluador && Var_Accion != "Todos")
                {
                    stxtClass = "boxes";
                    sReadOnly = "";
                }
                else
                {
                    stxtClass = "SoloLectura";
                    sReadOnly = "ReadOnly";
                }
                #endregion

                #region Tabla #2.

                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<table style='font-size: 10px' width='100%' border='0' cellpadding='1' cellspacing='4' >";

                #region Generar la segunda consulta SQL a la variable "RS".
                txtSQL = " SELECT nomTipoIndicador, id_TipoIndicador, sum(solicitado) as TotalSolicitado, " +
                                 " isnull(sum(Recomendado),0) as TotalRecomendado " +
                                 " FROM TipoIndicadorGestion T, EvaluacionProyectoAporte E " +
                                 " WHERE E.codProyecto = " + CodProyecto +
                                 " AND E.codConvocatoria = " + CodConvocatoria +
                                 " AND codTipoIndicador = id_tipoindicador " +
                                 " GROUP BY nomTipoIndicador, id_TipoIndicador " +
                                 " ORDER BY id_TipoIndicador";

                RS = consultas.ObtenerDataTable(txtSQL, "text");
                #endregion

                //Inicializar variables.
                Total = 0;

                #region Recorrer tabla "RS".
                foreach (DataRow row_RS in RS.Rows)
                {
                    #region Generar líneas iniciales de las filas, celdas y tabla interna.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                                    "<tr><td width='0%'></td>" +
                                                "	<td width='100%' align='left' class='Titulo'>" + row_RS["nomtipoIndicador"].ToString() + "</td></tr>" +
                                                "<tr><td colspan='3'>" +
                                                    "<table style='font-size: 10px' width='100%' border='0' cellpadding='4' cellspacing='1' class='zebra'>			" +
                                                        "<tr bgcolor='#000000'>" +
                                                        "	<td bgcolor='#000000' class='tituloTabla' width='0%'>&nbsp;</td>" +
                                                        "	<td bgcolor='#000000' width='30%' class='tituloTabla'><span style='color:#ffffff'>Nombre</span></td>" +
                                                        "	<td bgcolor='#000000' width='25%' class='tituloTabla'><span style='color:#ffffff'>Detalle</span></td>						" +
                                                        "	<td bgcolor='#000000' width='15%' class='tituloTabla' align='center'><span style='color:#ffffff'>Total Solicitado</span></td>" +
                                                        "	<td bgcolor='#000000' width='5%' class='tituloTabla' align='center'><span style='color:#ffffff'>%</span></td>" +
                                                        "	<td bgcolor='#000000' width='18%' class='tituloTabla' align='center'><span style='color:#ffffff'>Total Recomendado</span></td>" +
                                                        "	<td bgcolor='#000000' width='5%' class='tituloTabla' align='center'><span style='color:#ffffff'>%</span></td>" +
                                                        "</tr>";
                    #endregion

                    //Inicializar variable "numtotal".
                    numtotal = 0;

                    #region Generar nueva consulta SQL y asignar los resultados a la variable "rsAux".
                    txtSQL = " SELECT  id_Aporte, Nombre, Detalle, Solicitado, isnull(Recomendado,0) as recomendado, Protegido " +
                             " FROM EvaluacionProyectoAporte E " +
                             " WHERE E.codProyecto = " + CodProyecto +
                             " AND E.codConvocatoria = " + CodConvocatoria +
                             " AND codTipoIndicador= " + row_RS["id_TipoIndicador"].ToString() +
                             " ORDER BY id_Aporte";

                    //Asignar resultados de la nueva consulta a la variable "rsAux".
                    rsAux = consultas.ObtenerDataTable(txtSQL, "text");
                    #endregion

                    #region Recorrer la variable "rsAux".
                    foreach (DataRow row_rsAux in rsAux.Rows)
                    {
                        //Agregar el valor "Solicitado" a la variable "numTotal".
                        numtotal = numtotal + Double.Parse(row_rsAux["Solicitado"].ToString());

                        //Generar inicio de fila y celda.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<tr>" +
                                "<td>";

                        //A pesar de la gran "condición", no dibuja nada.
                        if (EsMiembro == true && bRealizado == false && usuario.CodGrupo == Constantes.CONST_Evaluador && Var_Accion != "Todos" && (row_rsAux["Protegido"].ToString() == "0" || row_rsAux["Protegido"].ToString() == "False")) { }

                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "</td>" +
                            "	<td>" +
                            "	" + row_rsAux["Nombre"].ToString() + "" +
                            "	</td>" +
                            "	<td>" + row_rsAux["Detalle"].ToString() + "</td>" +
                            "	<td align='right'>" + FieldValidate.moneyFormat(Double.Parse(row_rsAux["Solicitado"].ToString())) + "</td>" +////
                            "	<td align='right'>";

                        #region Solicitado.
                        if (row_RS["TotalSolicitado"].ToString() != "0")
                        {
                            Double val_Solicitado = Double.Parse(row_rsAux["Solicitado"].ToString());
                            Double val_TotalSolicitado = Double.Parse(row_RS["TotalSolicitado"].ToString());
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                                FieldValidate.moneyFormat((val_Solicitado * 100 / val_TotalSolicitado),false);
                        }
                        else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "0"; }
                        #endregion

                        #region Recomendado.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "	<td align='right'>" + FieldValidate.moneyFormat(Double.Parse(row_rsAux["Recomendado"].ToString())) + "</td>" +//
                                            "	<td align='right'>";

                        if (row_RS["TotalRecomendado"].ToString() != "0")
                        {
                            Double val_Recomendado = Double.Parse(row_rsAux["Recomendado"].ToString());
                            Double val_TotalRecomendado = Double.Parse(row_RS["TotalRecomendado"].ToString());
                            lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                                FieldValidate.moneyFormat((val_Recomendado * 100 / val_TotalRecomendado),false);
                        }
                        else { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "0"; }
                        #endregion

                        //Cerrar fila.
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "</tr>";
                    }
                    #endregion

                    //Destruir variable.
                    rsAux = null;

                    //Generar mas líneas de código.
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<tr>" +
                            "	<td>&nbsp;</td>" +
                            "	<td>&nbsp;</td>" +
                            "	<td Align='right' class='Titulo'>Totales</td>" +
                            "	<td align='right' class='Titulo'>" + FieldValidate.moneyFormat(Double.Parse(row_RS["TotalSolicitado"].ToString())) + "</td>" +
                            "	<td align='right' class='Titulo'>100</td>" +
                            "	<td align='right' class='Titulo'>" + FieldValidate.moneyFormat(Double.Parse(row_RS["TotalRecomendado"].ToString())) + "</td>" +
                            "	<td align='right' class='Titulo'>100</td>" +
                            "</tr>					" +
                        "</table>" +
                        "</td></tr>" +
                        "<tr><td colspan='2' class='tituloTabla'>&nbsp;</td></tr>";

                    //Agregar valor total.
                    Total = Total + Double.Parse(row_RS["TotalRecomendado"].ToString());
                }
                #endregion

                //Destruir variable.
                RS = null;

                #region Condicionar código de la convocatoria.
                if (Int32.Parse(CodConvocatoria) == 1) { Total = txtRecomendado * SMLV_Obtenido; }
                else
                {
                    if (txtRecomendado != Total)
                    { if (Total > (224 * SMLV_Obtenido)) { Total = 224 * SMLV_Obtenido; } }
                }
                #endregion

                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "</table>" +
                    "<br/><br/>";

                #endregion

                #region Tabla #3.

                #region Inicio de la tabla #3.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<table style='font-size: 10px' border='0' width='100%' cellspacing='1' cellpadding='4'>			" +
                        "<tr class='Titulo' bgcolor='#000000'>" +
                        "	<td rowspan='2' width='0'>&nbsp;</td>		" +
                        "	<td rowspan='2' align='left' class='tituloTabla'><span style='color:#ffffff'>Integrantes de la iniciativa empresarial</span></td>" +
                        "	<td colspan='2' align='center' class='tituloTabla'><span style='color:#ffffff'>Tipo</span></td>" +
                        "	<td colspan='4' align='Center' class='tituloTabla'><span style='color:#ffffff'>Valor Aporte (miles de pesos)</span></td>" +
                        "</tr>" +
                        "<tr class='Titulo' bgcolor='#000000'>" +
                        "	<td width='1%'  align='center' class='tituloTabla'><span style='color:#ffffff'>Emprendedor</span></td>" +
                        "	<td width='1%'  align='center' class='tituloTabla'><span style='color:#ffffff'>otro</span></td>" +
                        "	<td align='Center' class='tituloTabla'><span style='color:#ffffff'>Aporte Total</span></td>" +
                        "	<td align='Center' class='tituloTabla'><span style='color:#ffffff'>Aporte en Dinero</span></td>" +
                        "	<td align='center' class='tituloTabla'><span style='color:#ffffff'>Aporte en Especie</span></td>" +
                        "	<td width='10%' align='left' class='tituloTabla'><span style='color:#ffffff'>Clase de Especie</span></td>" +
                        "</tr>";
                #endregion

                #region Generar consulta para la tabla #3 y asignar los resultados de la misma a la variable "RS".

                //Tercera consulta.
                txtSQL = " SELECT distinct C.id_contacto, C.Nombres + ' ' + C.apellidos NomCompleto, PC.Beneficiario, " +
                         " isnull(EC.AporteDinero,0) as AporteDinero, isnull(EC.AporteEspecie,0) as AporteEspecie ," +
                         " EC.DetalleEspecie " +
                         " FROM Contacto C INNER JOIN ProyectoContacto PC ON C.id_contacto = PC.codContacto " +
                         " and Pc.inactivo=0 and C.Inactivo = 0 and PC.codProyecto = " + CodProyecto +
                         " and codRol = " + Constantes.CONST_RolEmprendedor +
                         " LEFT JOIN EvaluacionContacto EC ON PC.codContacto = EC.codContacto " +
                         " and PC.codProyecto = EC.codProyecto and EC.codConvocatoria = " + CodConvocatoria +
                         " ORDER BY C.Nombres + ' ' + C.apellidos";

                //Asignar resultados de la tercera consulta a la variable "RS".
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                #endregion

                #region Recorrer la tabla "RS".
                foreach (DataRow row_3_RS in RS.Rows)
                {
                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<tr>" +
                            "<td></td>" +
                            "<td>" + row_3_RS["NomCompleto"].ToString() + "</td>" +
                            "<td align='Center'>";

                    if (row_3_RS["Beneficiario"].ToString() == "1" || row_3_RS["Beneficiario"].ToString() == "True")
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<img src='../../Images/chulo.gif'></td>";
                    }
                    else
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "&nbsp;</td>";
                    }

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                        "<td align='Center'>";

                    if (row_3_RS["Beneficiario"].ToString() == "0" || row_3_RS["Beneficiario"].ToString() == "False")
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "<img src='../../Images/chulo.gif'></td>";
                    }
                    else
                    {
                        lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                            "&nbsp;</td>";
                    }

                    d_AporteDinero = Double.Parse(row_3_RS["AporteDinero"].ToString());
                    d_AporteEspecie = Double.Parse(row_3_RS["AporteEspecie"].ToString());

                    lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<td align='right'>" + FieldValidate.moneyFormat(((d_AporteDinero + d_AporteEspecie) / 1000)) + "</td>" +
                    "<td align='right'>" + FieldValidate.moneyFormat((d_AporteDinero / 1000)) + "</td>" +
                    "<td align='right'>" + FieldValidate.moneyFormat((d_AporteEspecie / 1000))+ "</td>" +
                    "<td width='10%' align='left'>" + row_3_RS["DetalleEspecie"].ToString() + "</td>";
                }
                #endregion

                //Cerrar tabla interna.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "</table>" +
                    "<br/><br/><br/><br/>";

                #endregion

                #region Tabla #4.

                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<table style='font-size: 10px' border=0>" +
                    "	<tr><td colspan='4' class='Titulo'>Observaciones composición grupo de socios - Equipo de trabajo  </td></tr>" +
                    "	<tr><td colspan='4'>" +
                    "		" + txtEquipoTrabajo + "<br/><br/>" +
                    "	</td></tr>" +
                    "	<tr><td colspan='2'>" +
                    "		<table style='font-size: 10px' border='0' WIDTH='100%'>" +
                    "			<tr><td class='Titulo'>Recursos solicitados al fondo emprender en (smlv) </td>" +
                    "				<td COLSPAN='2' class='Titulo' align='left'><input type='text' class='sololectura' size='4' ReadOnly value='" + txtSolicitado + "' id='text'1 name='text'1></td></tr>" +
                    "			</tr><td class='Titulo'>Valor recomendado (smlv) </td>" +
                    "				<td class='Titulo' WIDTH='120'><input type='text' id='Recomendado' name='Recomendado' class='sololectura' size='7' maxlength='3' ReadOnly value='" + txtRecomendado + "'></TD>" +
                    "				<TD ALIGN='RIGHT'>&nbsp;</TD>						" +
                    "			</tr>" +
                    "			</TABLE>" +
                    "		</TD>" +
                    "	</TR>		" +
                    "</table>";

                #endregion

                //Líneas finales.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "</td>" +
                        "		  <tr>" +
                        "		</table>" +

                        "	</td>" +
                        "</tr>" +
                    "</table>";

                //Mas líneas.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "<br/>" +
                    "<table style='font-size: 10px' width='100%' border='1' cellpadding='0' cellspacing='0' bordercolor='#000000'>" +
                    "	<tr><td colspan='7'>" +
                    "		<table style='font-size: 10px' width='100%' border='0' cellpadding='3' cellspacing='1'>" +
                    "			<tr><td bgcolor='#000000' class='TituloTabla' height='30'><span style='color:#ffffff'>Nota</span></td></tr>" +
                    "			<tr><td height='30'>" + Texto("TXT_PIE_EVALUACION") + "</td></tr>" +
                    "		</table>" +
                    "	</td></tr>		" +
                    "</table>";

                //Final line.
                lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text +
                    "</td>" +
            "	</tr>" +
            "</table>" +
                "</div>";
                //"</div>";
            }
            catch (Exception ex)
            { lbl_tr_emprendedores.Text = lbl_tr_emprendedores.Text + "<strong style='color:#FA0B0B;'>Error en la generación de las tablas finales: ''Indicadores de Gestión'': " + ex.Message + "</strong>"; }
        }

        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}