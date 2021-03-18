using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionCostosProd : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        String txtSQL;
        Int32 txtTiempoProyeccion;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (CodigoProyecto != 0)
                {
                    GenerarTablaDeCostosDeProduccion();
                    GenerarTablaProyeccionCompras_Unidades();
                    GenerarTablaProyeccionCompras_Pesos();
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        /// <summary>

        /// 09/06/2014.
        /// Cargar el valor "numAnios" en la variable global y de sesión llamadas "int_txtTiempoProyeccion".
        /// </summary>
        /// <returns>int_txtTiempoProyeccion / numAnios</returns>
        private Int32 Cargar_numAnios()
        {
            //Inicializar variables.
            Int32 int_txtTiempoProyeccion = 0;
            try
            {
                Datos.Consultas consultas = new Datos.Consultas();

                txtSQL = " SELECT TiempoProyeccion FROM ProyectoMercadoProyeccionVentas WHERE codProyecto = " + CodigoProyecto;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    int_txtTiempoProyeccion = Int32.Parse(dt.Rows[0]["TiempoProyeccion"].ToString());
                    dt = null;
                    txtSQL = null;
                    HttpContext.Current.Session["int_txtTiempoProyeccion"] = int_txtTiempoProyeccion;
                    txtTiempoProyeccion = int_txtTiempoProyeccion;
                    return int_txtTiempoProyeccion;
                }
                else
                {
                    dt = null;
                    txtSQL = null;
                    HttpContext.Current.Session["int_txtTiempoProyeccion"] = int_txtTiempoProyeccion;
                    txtTiempoProyeccion = int_txtTiempoProyeccion;
                    return int_txtTiempoProyeccion;
                }
            }
            catch { HttpContext.Current.Session["int_txtTiempoProyeccion"] = int_txtTiempoProyeccion; txtTiempoProyeccion = int_txtTiempoProyeccion; return int_txtTiempoProyeccion; }
        }

        /// 09/06/2014.
        /// Generar la primera tabla llamada "Tabla de costos de producción en pesos(incluido IVA)".
        /// </summary>
        private void GenerarTablaDeCostosDeProduccion()
        {
            //Inicilizar variables.

            String strSelect = "";
            DataTable tabla_costosProduccion = new DataTable();
            TableHeaderCell celdaEncabezado = new TableHeaderCell();
            TableHeaderCell celdaDatos = new TableHeaderCell();
            TableCell celdaEspecial = new TableCell();
            TableHeaderRow fila1 = new TableHeaderRow();
            TableRow fila = new TableRow();
            TableCell celdaDatosda = new TableCell();
            object[] numTotPesos = new object[12];
            Int32 incr = 1;
            Datos.Consultas consultas = new Datos.Consultas();
            int aux = 0;

            try
            {
                aux = Cargar_numAnios();

                //Generar SQL que será concatenado a la consulta principal.
                for (int i = 1; i < aux + 1; i++)
                {
                    strSelect = strSelect + ", sum(case when Ano=" + i.ToString() + " then V.unidades*cantidad*((100+desperdicio)/100.0)*(Precio*((100+IVA)/100.0)) else 0 end) AS [Año " + i.ToString() + "] ";
                }
            }
            catch (Exception)
            { throw; }

            try
            {
                if (aux > 0)
                {
                    //Consulta completa "con la concatenación de la variable (strSelect) generada anteriormente".
                    txtSQL = " SELECT T.id_TipoInsumo, T.nomTipoInsumo AS [Tipo de Insumo] " + strSelect +
                             " FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI," +
                             " ProyectoProductoUnidadesVentas V, ProyectoInsumoPrecio P" +
                             " WHERE I.codTipoInsumo = T.id_TipoInsumo AND PI.codInsumo = I.id_Insumo " +
                             " AND V.codProducto = PI.codProducto " +
                             " AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto " +
                             " AND P.periodo = V.ano AND Unidades <> 0 AND I.codProyecto =" + CodigoProyecto +
                             " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo";

                    //Asignar resultados a la variable DataTable.
                    tabla_costosProduccion = consultas.ObtenerDataTable(txtSQL, "text");
                }

                    #region Generar fila principal.

                    fila1 = new TableHeaderRow();
                    fila1.Attributes.Add("bgcolor", "#3D5A87");
                    celdaEncabezado = new TableHeaderCell();
                    celdaEncabezado.Attributes.Add("colspan", "6");
                    celdaEncabezado.Attributes.Add("color", "white");
                    celdaEncabezado.Attributes.Add("align", "center");
                    celdaEncabezado.Style.Add("text-align", "center");
                    celdaEncabezado.Text = "Tabla de costos de producción en pesos (incluido IVA)";
                    fila1.Cells.Add(celdaEncabezado);
                    Tabla_Costos.Rows.Add(fila1);

                    #endregion

                    #region Generar segunda fila con los encabezados correctos.

                    fila = new TableHeaderRow();
                    fila.Attributes.Add("bgcolor", "#3D5A87");
                    foreach (DataColumn item_string in tabla_costosProduccion.Columns)
                    {
                        //Inicializar la celda del encabezado.
                        celdaEncabezado = new TableHeaderCell();

                        //Si es "id_TipoInsumo" no muestra el título.
                        if (item_string.ColumnName != "id_TipoInsumo")
                        {
                            //Añadir atributo adicional si es año.
                            if (item_string.ColumnName.Contains("Año"))
                            {
                                celdaEncabezado.Attributes.Add("align", "center");
                                celdaEncabezado.Style.Add("text-align", "center");
                            }

                            //Continuar con la generación de la celda.
                            celdaEncabezado.Text = item_string.ColumnName;
                            fila.Cells.Add(celdaEncabezado);
                        }

                    }
                    Tabla_Costos.Rows.Add(fila);

                    #endregion
                
            }
            catch (Exception)
            { throw; }

            try
            {
                #region Generar siguientes dos filas "Insumos" y "Materia Prima" y sus datos.
                foreach (DataRow row_tabla in tabla_costosProduccion.Rows)
                {
                    //Inicializar fila.
                    fila = new TableRow();

                    #region Inicializar celda "Insumo" ó "Materia Prima".
                    celdaEspecial = new TableCell();
                    celdaEspecial.Text = row_tabla["Tipo de Insumo"].ToString();
                    fila.Cells.Add(celdaEspecial);
                    #endregion

                    #region Generar las celdas con valores de "Años".
                    for (int i = 1; i < txtTiempoProyeccion + 1; i++)
                    {
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "right");
                        celdaEspecial.Text = Double.Parse(row_tabla["Año " + i.ToString()].ToString()).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                        fila.Cells.Add(celdaEspecial);
                    }
                    #endregion

                    //Se añade la fila a la tabla.
                    Tabla_Costos.Rows.Add(fila);
                }

                #endregion
            }
            catch (Exception)
            { throw; }
            try
            {

                #region Obtener los totales "para luego mostrarlos en la grilla.

                //Fuente: http://stackoverflow.com/questions/5892993/how-to-calculate-the-sum-of-the-datatable-column-in-asp-net
                if (tabla_costosProduccion.Rows.Count > 0)
                {
                    for (int i = 1; i < txtTiempoProyeccion + 1; i++)
                    {
                        double valn = Double.Parse(tabla_costosProduccion.Compute("Sum([Año " + i.ToString() + "])", "").ToString());
                        //var valn = tabla_costosProduccion.Compute("Sum([Año " + i.ToString() + "])", "");
                        numTotPesos[i] = valn;
                    }

                #endregion

                    #region Generar tabla de totales.

                    //Inicializar fila.
                    fila = new TableRow();

                    for (int i = 0; i < tabla_costosProduccion.Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            //Inicializar nueva celda.
                            celdaEspecial = new TableCell();
                            celdaEspecial.Text = "<b>Totales</b>";
                            fila.Cells.Add(celdaEspecial);
                        }
                        else
                        {
                            if (incr <= txtTiempoProyeccion)
                            {
                                //Generar lista de totales.
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<b>" + Convert.ToDouble(numTotPesos[incr].ToString()).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                                fila.Cells.Add(celdaEspecial);
                                incr++;
                            }
                        }
                    }
                }
                //Se devuelve la variable a su estado original.
                incr = 1;

                //Se añade la fila a la tabla.
                Tabla_Costos.Rows.Add(fila);

                    #endregion
            }
            catch (Exception)
            { throw; }

            #region Destruir variables.

            Tabla_Costos = new Table();
            strSelect = "";
            tabla_costosProduccion = null;
            celdaEncabezado = null;
            celdaDatos = null;
            celdaEspecial = null;
            fila1 = null;
            fila = null;
            celdaDatosda = null;
            numTotPesos = null;
            incr = 0;

            #endregion
        }

        /// <summary>

        /// 10/06/2014.
        /// Generar la segunda tabla llamada "Proyección de Compras (Unidades)".
        /// </summary>
        private void GenerarTablaProyeccionCompras_Unidades()
        {
            //Inicializar variables.
            
            String strSelect = "";
            DataTable rsTipo = new DataTable();
            TableHeaderCell celdaEncabezado = new TableHeaderCell();
            TableHeaderCell celdaDatos = new TableHeaderCell();
            TableCell celdaEspecial = new TableCell();
            TableHeaderRow fila1 = new TableHeaderRow();
            TableRow fila = new TableRow();
            TableCell celdaDatosda = new TableCell();
            List<double> numTotPesos = new List<double>();
            List<double> Rows1 = new List<double>();
            List<double> Rows2 = new List<double>();
            List<double> RowsResults = new List<double>();
            Datos.Consultas consultas = new Datos.Consultas();
            int aux = 0;


            try
            {
                aux = Cargar_numAnios();

                #region Generar fila principal.

                fila1 = new TableHeaderRow();
                fila1.Attributes.Add("bgcolor", "#3D5A87");
                celdaEncabezado = new TableHeaderCell();
                celdaEncabezado.Attributes.Add("colspan", "6");
                celdaEncabezado.Attributes.Add("color", "white");
                celdaEncabezado.Attributes.Add("align", "center");
                celdaEncabezado.Style.Add("text-align", "center");
                celdaEncabezado.Text = "Proyección de Compras (Unidades)";
                fila1.Cells.Add(celdaEncabezado);
                Tabla_Proyecciones_1.Rows.Add(fila1);

                #endregion

                #region Consulta con la información inicial de los encabezados.

                if (aux > 0)
                {

                    //Generar SQL que será concatenado a la consulta principal.
                    for (int i = 1; i < aux + 1; i++)
                    { strSelect = strSelect + ", SUM(case when ano=" + i.ToString() + " then V.Unidades*PI.Cantidad*(1+(desperdicio/100.0)) else 0 end) AS [Año " + i.ToString() + "] "; }

                    //Consultar los tipos de insumos a generar en la tabla.
                    txtSQL = " SELECT T.id_TipoInsumo, T.nomTipoInsumo  AS [Tipo de Insumo] " + strSelect +
                             " FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V,  " +
                             " ProyectoInsumoPrecio P  " +
                             " WHERE I.codTipoInsumo = T.id_TipoInsumo AND PI.codInsumo = I.id_Insumo  " +
                             " AND V.codProducto = PI.codProducto AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto  " +
                             " AND P.periodo=V.ano AND Unidades <> 0 AND I.codProyecto = " + CodigoProyecto +
                             " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo ";

                    //Asignar resultados a la variable DataTable.
                    rsTipo = consultas.ObtenerDataTable(txtSQL, "text");
                }

                #endregion

                #region Generar segunda fila con los encabezados correctos.

                fila = new TableHeaderRow();
                fila.Attributes.Add("bgcolor", "#3D5A87");
                foreach (DataColumn item_string in rsTipo.Columns)
                {
                    //Inicializar la celda del encabezado.
                    celdaEncabezado = new TableHeaderCell();

                    //Si es "id_TipoInsumo" no muestra el título.
                    if (item_string.ColumnName != "id_TipoInsumo")
                    {
                        //Añadir atributo adicional si es año.
                        if (item_string.ColumnName.Contains("Año"))
                        {
                            celdaEncabezado.Attributes.Add("align", "center");
                            celdaEncabezado.Style.Add("text-align", "center");
                        }

                        //Continuar con la generación de la celda.
                        celdaEncabezado.Text = item_string.ColumnName;
                        fila.Cells.Add(celdaEncabezado);
                    }

                }
                Tabla_Proyecciones_1.Rows.Add(fila);

                #endregion

                //Recorrer la tabla de la consulta principal para generar la información.
                foreach (DataRow row_rsTipo in rsTipo.Rows)
                {
                    #region Region 1. "Contiene la información de las proyecciones de compras en unidades".

                    txtSQL = " SELECT I.nomInsumo AS [Tipo de Insumo] " + strSelect +
                             " FROM ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V  " +
                             " WHERE PI.codInsumo = I.id_insumo AND V.codProducto = PI.codProducto  " +
                             " AND I.codProyecto = " + CodigoProyecto + " AND codTipoInsumo = " + row_rsTipo.ItemArray[0].ToString() +
                             " GROUP BY I.nomInsumo " +
                             " HAVING Sum(V.Unidades) > 0 ";

                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    #endregion

                    #region Generar fila única "Insumos" o "Materia Prima".

                    //Inicializar la nueva fila.
                    fila = new TableRow();
                    //Inicializar nueva celda.
                    celdaEspecial = new TableCell();
                    celdaEspecial.Attributes.Add("colspan", "6");
                    celdaEspecial.Text = "<strong>" + row_rsTipo["Tipo de Insumo"].ToString() + "</strong>";
                    //Añadir la celda a la fila de datos obtenidos.
                    fila.Cells.Add(celdaEspecial);
                    Tabla_Proyecciones_1.Rows.Add(fila);

                    #endregion

                    #region Generar filas de acuerdo a la hecha en el region llamado "Region 1".

                    foreach (DataRow row_dt in dt.Rows)
                    {
                        //Inicializar fila.
                        fila = new TableRow();

                        for (int i = 0; i < row_rsTipo.ItemArray.Count() - 1; i++)
                        {
                            //Inicializar la celda.
                            celdaEspecial = new TableCell();
                            //Si la posición es igual a cero "la cual contiene el nombre de la materia prima,
                            //ésta se agrega directamente", de lo contrario, sele añade la 
                            //propiedad de alineación del texto, se formatea el dato y se añade a la
                            //fila.
                            if (i == 0) { celdaEspecial.Text = row_dt.ItemArray[i].ToString(); }
                            else
                            {
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = Double.Parse(row_dt.ItemArray[i].ToString()).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                            }
                            //Añadir la celda generada a la fila.
                            fila.Cells.Add(celdaEspecial);
                        }

                        //Agregar la fila generada a la tabla.
                        Tabla_Proyecciones_1.Rows.Add(fila);
                    }

                    dt = null;

                    #endregion
                }

            }
            catch (Exception)
            { throw; }

            #region Destruir variables.

            Tabla_Proyecciones_1 = new Table();
            strSelect = "";
            rsTipo = null;
            celdaEncabezado = null;
            celdaDatos = null;
            celdaEspecial = null;
            fila1 = null;
            fila = null;
            celdaDatosda = null;
            numTotPesos = new List<double>();
            Rows1 = new List<double>();
            Rows2 = new List<double>();
            RowsResults = new List<double>();

            #endregion
        }

        /// <summary>

        /// 10/06/2014.
        /// Generar la tercera tabla llamada "Proyección de Compras (Pesos)".
        /// </summary>
        private void GenerarTablaProyeccionCompras_Pesos()
        {
            //Inicializar variables.
            String strSelect = "";
            DataTable rsTipo = new DataTable();
            TableHeaderCell celdaEncabezado = new TableHeaderCell();
            TableHeaderCell celdaDatos = new TableHeaderCell();
            TableCell celdaEspecial = new TableCell();
            TableHeaderRow fila1 = new TableHeaderRow();
            TableRow fila = new TableRow();
            TableCell celdaDatosda = new TableCell();
            Double[] numTotPesos = new Double[12];
            Double[] numIVA = new Double[12];
            List<double> ListaYears = new List<double>();
            List<double> ListaIVAs = new List<double>();
            String[] arr_totales = { "&nbsp;", "Total", "IVA", "Total mas IVA" };
            Datos.Consultas consultas = new Datos.Consultas();

            try
            {
                #region Generar fila principal.

                fila1 = new TableHeaderRow();
                fila1.Attributes.Add("bgcolor", "#3D5A87");
                celdaEncabezado = new TableHeaderCell();
                celdaEncabezado.Attributes.Add("colspan", "6");
                celdaEncabezado.Attributes.Add("color", "white");
                celdaEncabezado.Attributes.Add("align", "center");
                celdaEncabezado.Style.Add("text-align", "center");
                celdaEncabezado.Text = "Proyección de Compras (Pesos)";
                fila1.Cells.Add(celdaEncabezado);
                Tabla_Proyecciones_2.Rows.Add(fila1);

                #endregion

                #region Generar encabezados y string SQL para ser usado en la siguiente consulta.

                //Instanciar la fila.
                fila = new TableHeaderRow();
                fila.Attributes.Add("bgcolor", "#3D5A87");

                #region Añadir la celda "Tipos de Insumo".

                //Inicializar la celda.
                celdaEncabezado = new TableHeaderCell();

                //Agregar valores a la celda.
                celdaEncabezado.Text = "Tipos de Insumo";

                //Añadir la celda a la fila.
                fila.Cells.Add(celdaEncabezado);

                #endregion

                //Generar encabezados de "Años" y string SQL que será concatenado a la consulta principal.
                for (int i = 1; i < Cargar_numAnios() + 1; i++)
                {
                    //Inicializar la celda del encabezado.
                    celdaEncabezado = new TableHeaderCell();

                    //Agregar valores a la celda.
                    celdaEncabezado.Attributes.Add("align", "center");
                    celdaEncabezado.Style.Add("text-align", "center");
                    celdaEncabezado.Text = "Año " + i.ToString();

                    //Añadir la celda a la fila.
                    fila.Cells.Add(celdaEncabezado);

                    //Generar SQL en variable "strSelect".
                    strSelect = strSelect + " , sum(case when Ano=" + i.ToString() + " then V.unidades*cantidad*((100+desperdicio)/100.0)*precio else 0 end) AS [Año " + i.ToString() + "] " +
                                            " , sum(case when Ano=" + i.ToString() + " then V.unidades*cantidad*((100+desperdicio)/100.0)*precio*IVA/100.0 else 0 end) IVA" + i.ToString();
                }

                //Añadir la fila a la tabla.
                Tabla_Proyecciones_2.Rows.Add(fila);

                #endregion

                #region Consultar los valores por "Insumo" o "Materia Prima" y generar las filas.

                int aux = Cargar_numAnios();

                if (aux > 0)
                {

                    //Consultar los tipos de insumos a generar en la tabla.
                    txtSQL = " SELECT T.id_TipoInsumo, T.nomTipoInsumo AS [Tipo de Insumo] " + strSelect +
                             " FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V,  " +
                             " ProyectoInsumoPrecio P  " +
                             " WHERE I.codTipoInsumo = T.id_TipoInsumo AND PI.codInsumo = I.id_Insumo  " +
                             " AND V.codProducto = PI.codProducto AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto  " +
                             " AND P.periodo=V.ano AND Unidades <> 0 AND I.codProyecto = " + CodigoProyecto +
                             " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo ";

                    //Asignar resultados a la variable DataTable.
                    rsTipo = consultas.ObtenerDataTable(txtSQL, "text");
                }

                foreach (DataRow row in rsTipo.Rows)
                {
                    #region Generar la fila única de "Tipo de Insumo".

                    fila = new TableRow();
                    celdaEspecial = new TableCell();
                    celdaEspecial.Attributes.Add("colspan", "6");
                    celdaEspecial.Text = "<strong >" + row["Tipo de Insumo"].ToString() + "</strong>";
                    fila.Cells.Add(celdaEspecial);
                    //Agregar fila.
                    Tabla_Proyecciones_2.Rows.Add(fila);

                    #endregion

                    #region Consulta SQL y resultados añadidos a la tabla temporal "dt".

                    txtSQL = " SELECT I.nomInsumo " + strSelect +
                                         " FROM ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V, " +
                                         " ProyectoInsumoPrecio P " +
                                         " WHERE PI.codInsumo = I.id_Insumo AND V.codProducto=PI.codProducto  " +
                                         " AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto AND P.periodo = V.ano  " +
                                         " AND Unidades <> 0 AND I.codProyecto = " + CodigoProyecto +
                                         " AND I.codTipoInsumo=" + row["id_tipoInsumo"].ToString() +
                                         " GROUP BY I.nomInsumo ";

                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                    #endregion

                    #region Por cada fila en "dt", generará los resultados en nueva fila con nuevas celdas.

                    foreach (DataRow row_dt in dt.Rows)
                    {
                        //Inicializar nueva fila.
                        fila = new TableRow();
                        //Inicializar celda.
                        celdaDatosda = new TableCell();

                        //Generar la celda del nombre.
                        celdaDatosda.Text = row_dt["nomInsumo"].ToString();
                        fila.Cells.Add(celdaDatosda);

                        //Recorrer los años para generar los valores de las columnas "Años" y
                        //obtener los valores de "IVA's" en la lista de doubles, "así como los valores de años".
                        for (int i = 1; i < txtTiempoProyeccion + 1; i++)
                        {
                            //Generar las siguientes celdas.
                            celdaEspecial = new TableCell();
                            celdaEspecial.Attributes.Add("align", "right");
                            celdaEspecial.Text = Double.Parse(row_dt["Año " + i.ToString()].ToString()).ToString("$ 0,0.00", CultureInfo.InvariantCulture);
                            numTotPesos[i] = numTotPesos[i] + Double.Parse(row_dt["Año " + i.ToString()].ToString());
                            numIVA[i] = numIVA[i] + Double.Parse(row_dt["IVA" + i.ToString()].ToString());
                            fila.Cells.Add(celdaEspecial);
                        }
                        //Añadir la fila generada con las celdas a la tabla.
                        Tabla_Proyecciones_2.Rows.Add(fila);
                    }

                    //Destruir variable
                    dt = null;

                    #endregion
                }

                #endregion

                #region Generar filas de totales.

                if (aux > 0)
                {
                    //Recorrer la variable "arr_totates" que contiene los títulos pre-definidos.
                    for (int i = 0; i < arr_totales.Count(); i++)
                    {
                        #region Generar la fila única de acuerdo al valor almacenado en el arreglo "arr_totales".

                        #region Generar la primera celda de la fila.
                        fila = new TableRow();
                        celdaEspecial = new TableCell();
                        celdaEspecial.Text = "<strong>" + arr_totales[i] + "</strong>";
                        fila.Cells.Add(celdaEspecial);
                        #endregion

                        #region Establecer iteración de datos.

                        if (i == 1)
                        {
                            #region Iterar la variable "numTotPesos" = Años.
                            for (int j = 1; j < txtTiempoProyeccion + 1; j++)
                            {
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<strong>" + numTotPesos[j].ToString("$ 0,0.00", CultureInfo.InvariantCulture) + "</strong>";
                                fila.Cells.Add(celdaEspecial);
                            }
                            #endregion
                        }
                        if (i == 2)
                        {
                            #region Iterar la variable "numIVA" = IVA's.
                            for (int j = 1; j < txtTiempoProyeccion + 1; j++)
                            {
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<strong>" + numIVA[j].ToString("$ 0,0.00", CultureInfo.InvariantCulture) + "</strong>";
                                fila.Cells.Add(celdaEspecial);
                            }
                            #endregion
                        }
                        if (i == 3)
                        {
                            #region Iterar para sumar los valores de las variables "numTotPesos" y "numIVA".
                            for (int j = 1; j < txtTiempoProyeccion + 1; j++)
                            {
                                celdaEspecial = new TableCell();
                                celdaEspecial.Attributes.Add("align", "right");
                                celdaEspecial.Text = "<strong>" + (numIVA[j] + numTotPesos[j]).ToString("$ 0,0.00", CultureInfo.InvariantCulture) + "</strong>";
                                fila.Cells.Add(celdaEspecial);
                            }
                            #endregion
                        }

                        #endregion

                        //Añadir la celda generada a la fila.
                        fila.Cells.Add(celdaEspecial);

                        //Agregar fila a la tabla.
                        Tabla_Proyecciones_2.Rows.Add(fila);

                        #endregion
                    }
                }
                #endregion


            }
            catch (Exception)
            { throw; }

            #region Destruir variables.
            Tabla_Proyecciones_2 = null;
            strSelect = null;
            rsTipo = null;
            celdaEncabezado = null;
            celdaDatos = null;
            celdaEspecial = null;
            fila1 = null;
            fila = null;
            celdaDatosda = null;
            numTotPesos = null;
            numIVA = null;
            ListaYears = null;
            ListaIVAs = null;
            arr_totales = null;
            #endregion
        }
    }
}