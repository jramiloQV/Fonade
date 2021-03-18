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

namespace Fonade.FONADE.interventoria
{
    public partial class FrameVentasInter : Negocio.Base_Page
    {
        public int prorroga;
        public int prorrogaTotal;

        #region Propiedades.

        private int CodCargo
        {
            get { return (int)ViewState["cargo"]; }
            set { ViewState["cargo"] = value; }
        }

        private int CodProyecto
        {
            get { return (int)ViewState["proyecto"]; }
            set { ViewState["proyecto"] = value; }
        }

        private int CodNomina
        {
            get { return (int)ViewState["nomina"]; }
            set { ViewState["nomina"] = value; }
        }

        #endregion

        #region Propiedades de grilla "gv_Detallesproductos" (para detectar los campos con ID "fondo...")

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
        
        int clik_mostrar = 0; // Permite validar que link de mostrar ha clicado el usuario
        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //CodProyecto = (int)(!string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? Convert.ToInt64(HttpContext.Current.Session["CodProyecto"]) : 0);
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? int.Parse(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;

            #region GENERAR GRILLA DINÁMICA DESCOMENTADO.
            //Producto seleccionado.
            string interno_CodVentas = HttpContext.Current.Session["interno_CodVentas"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["interno_CodVentas"].ToString()) ? HttpContext.Current.Session["interno_CodVentas"].ToString() : "0";

            if (interno_CodVentas != "0")
            {
                GenerarTabla(interno_CodVentas, CodProyecto.ToString());
            }
            #endregion

            if (!IsPostBack)
            {
                t_anexos.Rows.Clear();
                try
                {
                    gv_Detallesproductos.Visible = false;
                    //tabla_meses.Visible = false;
                    CargarGridProductosVentasEnProduccion();
                    CargarGridProductosEnAprobacion();

                    //Obtener conteo de puestos productos por aprobar.
                    if (GrvActividadesNoAprobadas.Rows.Count > 0)
                    { 
                        lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: " + GrvActividadesNoAprobadas.Rows.Count;
                    }
                    else {
                        lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: 0";
                    }
                    //Evaluar si ciertos campos se mostrarán.
                    EvaluarCampos(usuario.CodGrupo);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                //Evaluar si ciertos campos se mostrarán.
                EvaluarCampos(usuario.CodGrupo);
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 05/04/2014.
        /// Cargar la grilla lateral izquierda de productos.
        /// </summary>
        protected void CargarGridProductosVentasEnProduccion()
        {
            //Inicializar variables.
            var respuestaDetalle = new DataTable();

            //consultas.Parameters = new[]
            //                               {
            //                                   new SqlParameter
            //                                       {
            //                                           ParameterName = "@CodProyecto" ,Value = CodProyecto //CodCargo
            //                                       }
            //                               };

            //respuestaDetalle = consultas.ObtenerDataTable("MD_ProductosEnVentas");
            respuestaDetalle = consultas.ObtenerDataTable("select *, id_ventas as [VentasID] from InterventorVentas where codproyecto = " + CodProyecto + " order by id_ventas", "text");
            gv_productos.DataSource = respuestaDetalle;
            gv_productos.DataBind();            
        }

        /// <summary>
        /// Paginación de la grilla de personal calificado "grilla ubicada a la izquierda de la pantalla".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_productos.PageIndex = e.NewPageIndex;
            CargarGridProductosVentasEnProduccion();
        }

        protected void Adicionar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Cargar();
        }

        /// <summary>
        /// Método que genera una ventana emergente de la llamada "CatalogoVentasTMP.aspx".
        /// </summary>
        void Cargar()
        {
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["pagina"] = "Ventas";
            //Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
            //             "menubar=0,scrollbars=1,width=710,height=400,top=100");
            //Nueva redirección a la página que es la correcta:
            Redirect(null, "../evaluacion/CatalogoVentasPOInterventoria.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Método que recibe el codigo del elemento a procesar y el mes del elemento seleccionado
        /// para devolver un valor que será usado para habilitar el acceso a los avances del elemento.
        /// </summary>
        /// <param name="codactividad">Código del elemento.</param>
        /// <param name="mes">Mes del elemento seleccionado.</param>
        /// <param name="tipoFinanciacion">Tipo de financiación del producto.</param>
        /// <returns>Número decimal.</returns>
        public decimal Avance(int codactividad, int mes, int tipoFinanciacion)
        {
            //Inicializar variable.
            decimal dtAvance = 0;

            try
            {
                var query = consultas.Db.AvanceVentasPOMes.FirstOrDefault(am => am.CodProducto == codactividad && am.Mes == mes && am.CodTipoFinanciacion == tipoFinanciacion);

                if (query != null && query.CodProducto != 0)
                { dtAvance = query.Valor; }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dtAvance;
        }

        /// <summary>
        /// Seleccionar el ítem y llamar a "CatalogoVentasTMP".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_productos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                HttpContext.Current.Session["Accion"] = "actualizar";
                HttpContext.Current.Session["proyecto"] = CodProyecto;
                HttpContext.Current.Session["CodProducto"] = e.CommandArgument.ToString();
                HttpContext.Current.Session["pagina"] = "Ventas";
                Redirect(null, "../evaluacion/CatalogoVentasTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            if (e.CommandName == "mostrar")
            {
                #region Código que separa el código del producto y su nombre para añadir los valores a variables de sesión.
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Nueva línea de código para almacenar nombre del producto seleccionado en sesión.
                HttpContext.Current.Session["CodVenta"] = valores_command[0];
                HttpContext.Current.Session["NombreVenta"] = valores_command[1];

                #endregion
                clik_mostrar = 0;
                CargarDetalle(e.CommandArgument.ToString());
            }
            // Se crea la validacion de la eliminción 
            // Diciembre 6 de 2014 - Alex Flautero
            if (e.CommandName == "eliminar")
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                EliminarProductoSeleccionado(valores_command[0], valores_command[1]);
            }
        }

        /// <summary>
        /// Cargar los detalles del producto seleccionado.
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
                //Inicializar variables.
                var respuestaDetalle = new DataTable();
                int codigoProductoVenta = 0;

                #region Region 1.

                try
                {
                    codigoProductoVenta = Int32.Parse(variables[0]);
                    HttpContext.Current.Session["interno_CodVentas"] = variables[0].ToString();
                    GenerarTabla(variables[0], CodProyecto.ToString());

                #endregion
                }
                catch (Exception)
                {
                    gv_Detallesproductos.Visible = false;
                    //tabla_meses.Visible = false;
                    return;
                }
            }
        }

        public void DynamicCommand_VerAvance(Object sender, CommandEventArgs e)
        {
            //DIANA
            try
            {
                //Inicializar variables.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                HttpContext.Current.Session["Accion"] = valores_command[0];
                if (valores_command[0].ToString() == "actualizar")
                    if(usuario.CodGrupo == Constantes.CONST_Emprendedor)
                        Session["Archivos"] = "Actializar";
                HttpContext.Current.Session["proyecto"] = valores_command[1];

                HttpContext.Current.Session["CodVenta"] = valores_command[2].ToString();
                //Ya se le está enviado la nómina seleccionada en una variable de sesión.
                HttpContext.Current.Session["MesVenta"] = valores_command[3].ToString();
                //Ya se le está enviando el nombre del cargo seleccionado en una variable de sesión.
                HttpContext.Current.Session["pagina"] = "Venta";
                Redirect(null, "../evaluacion/CatalogoVentasPOInterventoria.aspx", "_blank",
                           "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            catch (Exception) { throw; }
        }

        private void GenerarTabla(String Cod_Ventas, String Cod_Proyecto)
        {
            //Inicializar variables.
            String txtSQL = "";
            String CodProducto = "";
            Double TotalFE = 0;
            Double TotalEmp = 0;
            DataTable rsProducto = new DataTable();
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
            Double numProductoValor = 0;
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

                #region Agregar nueva fila (para adicionar las celdas "Cantidad" y "Costo").
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
                    celdaSueldo.Text = "Ventas";
                    celdaSueldo.Attributes.Add("title", "Ventas Presupuestadas");
                    fila.Cells.Add(celdaSueldo);
                    t_anexos.Rows.Add(fila);
                    celdaSueldo = null;

                    //Agregar datos a la celda de Prestaciones Sociales.
                    celdaPrestaciones.Text = "Ingreso";
                    celdaPrestaciones.Attributes.Add("title", "Ingreso por Ventas Estimado");
                    fila.Cells.Add(celdaPrestaciones);
                    t_anexos.Rows.Add(fila);
                    celdaPrestaciones = null;
                }
                #endregion

                #region Consulta SQL inicial.
                  

                // La varible dato nos ayuda a verificar que datos va a mostrar cuando hagan clic en el link mostrar.
                if(clik_mostrar == 0){
                    txtSQL = " SELECT DISTINCT * FROM InterventorVentas left join InterventorVentasMes " +
                         " on id_ventas = codproducto WHERE codproyecto = " + Cod_Proyecto +
                         " AND id_ventas = " + Cod_Ventas + " ORDER BY id_ventas, mes, tipo ";
                }
                else if (clik_mostrar == 1)
                {
                txtSQL = " SELECT DISTINCT * FROM InterventorVentasTMP left join InterventorVentasMesTMP " +
                         " on id_ventas = codproducto WHERE codproyecto = " + Cod_Proyecto +
                         " AND id_ventas = " + Cod_Ventas + " ORDER BY id_ventas, mes, tipo ";
                }

                //Asignar resultados de la consulta anterior a variable DataTable.
                rsProducto = consultas.ObtenerDataTable(txtSQL, "text");

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
                        #region Agregar nueva fila con espacio separador "igual como lo deja FONADE clásico".

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
                        #region Agregar la fila "con los valores de meses (Cantidades y multiplicación del costo)".

                        //Inicializar la fila.
                        fila = new TableRow();
                        //Recorrer las celdas.
                        for (int j = 1; j < conteo_celdas; j++)
                        {
                            //Si el mes es menor o igual a los meses totales (mes+prorroga) + 1 (fila Costo Total).
                            if (mes_data <= prorrogaTotal + 1) //+ 1 indicando la "fila" "Costo Total".
                            {
                                //Inicializar variable para obtener el valor de acuerdo al mes.
                                DataRow[] result = rsProducto.Select("Mes = " + mes_data);

                                //Si encuentra datos.
                                if (result.Count() != 0)
                                {
                                    #region Consultar el campo "Valor" y operarlo en las variables correspondientes.

                                    foreach (DataRow row in result)
                                    {
                                        //Obtener el campos "Valor".
                                        Valor = Double.Parse(row["Valor"].ToString());

                                        //Si es de tipo "1", lo añade a variable "FE", si es "2", lo agrega a variable "Emp".
                                        if (row["Tipo"].ToString() == "1")
                                        {
                                            TotalFE = TotalFE + Valor;
                                            Valor_FE = Valor.ToString();
                                            //Variable que contiene el mismo valor de la variable "Valor", la cual será multiplicada mas adelante.
                                            numProductoValor = Valor;
                                        }
                                        if (row["Tipo"].ToString() == "2")
                                        {
                                            TotalEmp = TotalEmp + Double.Parse(row["Valor"].ToString()) * numProductoValor;
                                            Valor_Emp = (Double.Parse(row["Valor"].ToString()) * numProductoValor).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        }
                                    }

                                    #endregion

                                    if (mes_data == prorrogaTotal + 1)
                                    {
                                        TableCell celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalFE.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);

                                        celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalEmp.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);
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
                                        celdaEspacio.Text = TotalFE.ToString();
                                        fila.Cells.Add(celdaEspacio);

                                        celdaEspacio = new TableCell();
                                        celdaEspacio.Attributes.Add("align", "right");
                                        celdaEspacio.Text = TotalEmp.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        fila.Cells.Add(celdaEspacio);
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
                                         " from AvanceVentasPOMes " +
                                         " where CodProducto = " + Cod_Ventas +
                                         " and Mes = " + mes_data + " and codtipofinanciacion = 1 ";

                                //Asignar resultados a variable DataTable.
                                rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");

                                //Si la consulta tiene datos, formatea el campo "Valor" y lo agrega a las variables correspondientes.
                                if (rsTipo1.Rows.Count > 0)
                                {
                                    valor_tipo1 = Double.Parse(rsTipo1.Rows[0]["Valor"].ToString());
                                    tipo1 = valor_tipo1.ToString();
                                    TotalTipo1 = TotalTipo1 + valor_tipo1;
                                }

                                if (mes_data == prorrogaTotal + 1) //Penúltima celda.
                                {
                                    #region Si esta condición se cumple, se debe a que es el Total de tipo 1, por lo tanto se agrega en "Costo Total".

                                    //Variable que contiene el valor "TotalTipo1" formateado.
                                    string sTotalTipo1 = TotalTipo1.ToString();

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
                                         " from AvanceVentasPOMes " +
                                         " where CodProducto = " + Cod_Ventas +
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
                                txtSQL = " SELECT * FROM AvanceVentasPOMes " +
                                         " WHERE CodProducto = " + Cod_Ventas + " AND mes = " + mes_data;

                                //Asignar resultados a la variable DataTable.
                                rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");

                                //Determinar el valor de la vairiable "ejecutar" de acuerdo a la tabla.
                                if (rsTipo2.Rows.Count > 0)
                                { ejecutar = 1; /*Si existe se coloca la opción de editar y borrar*/ }
                                else
                                { ejecutar = 2; /*Si NO existe se coloca la opción de adicionar*/ }

                                try { CodProducto = Cod_Ventas.Replace("+", "$"); }
                                catch { }

                                if (ejecutar == 1)
                                {
                                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                                    {
                                        if (rsTipo2.Rows[0]["Aprobada"].ToString() == "False" || rsTipo2.Rows[0]["Aprobada"].ToString() == "0")
                                        {
                                            #region Ver avance.

                                            ImageButton img_VerAvance = new ImageButton();
                                            LinkButton lnk_VerAvance = new LinkButton();

                                            //ImageButton.
                                            img_VerAvance.ID = "img_VerAvance_" + mes_data.ToString();
                                            img_VerAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                            img_VerAvance.AlternateText = "Avance";
                                            img_VerAvance.CommandName = "actualizar";
                                            img_VerAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            img_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                            //{ img_VerAvance = null; }
                                            //else
                                            //{ }
                                            celda.Controls.Add(img_VerAvance);

                                            //LinkButton.
                                            lnk_VerAvance.ID = "lnk_VerAvance_" + mes_data.ToString();
                                            lnk_VerAvance.Text = "<b>Ver Avance</b>";
                                            lnk_VerAvance.Style.Add("text-decoration", "none");
                                            lnk_VerAvance.Style.Add("margin-left", "2px");
                                            lnk_VerAvance.CommandName = "VerAvance";
                                            lnk_VerAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            lnk_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                            //{ lnk_VerAvance = null; }
                                            //else
                                            //{ } 
                                            celda.Controls.Add(lnk_VerAvance);

                                            #endregion
                                        }
                                        else
                                        {
                                            if (String.IsNullOrEmpty(rsTipo2.Rows[0]["ObservacionesInterventor"].ToString()))
                                            {
                                                #region Eliminar avance.

                                                ImageButton img_VerAvance_Elim = new ImageButton();
                                                LinkButton lnk_VerAvance_Elim = new LinkButton();

                                                //ImageButton.
                                                img_VerAvance_Elim.ID = "img_VerAvance_Elim" + mes_data.ToString();
                                                img_VerAvance_Elim.ImageUrl = "~/Images/icoBorrar.gif";
                                                img_VerAvance_Elim.AlternateText = "Avance";
                                                img_VerAvance_Elim.CommandName = "borrar";
                                                img_VerAvance_Elim.CommandArgument = "borrar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                                img_VerAvance_Elim.OnClientClick = "return borrar();";
                                                img_VerAvance_Elim.Visible = true;
                                                img_VerAvance_Elim.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                //if (usuario.CodGrupo != Constantes.CONST_Interventor)
                                                //{ img_VerAvance = null; }
                                                //else
                                                //{ }
                                                celda.Controls.Add(img_VerAvance_Elim);

                                                #endregion
                                            }

                                            #region Editar

                                            ImageButton img_Edt_VerAvance = new ImageButton();
                                            LinkButton lnk_Edt_VerAvance = new LinkButton();

                                            //ImageButton.
                                            img_Edt_VerAvance.ID = "img_Edt_VerAvance_" + mes_data.ToString();
                                            img_Edt_VerAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                            img_Edt_VerAvance.AlternateText = "Editar Avance";
                                            img_Edt_VerAvance.CommandName = "actualizar";
                                            img_Edt_VerAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            img_Edt_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(img_Edt_VerAvance);

                                            //LinkButton.
                                            lnk_Edt_VerAvance.ID = "lnk_VerAvance_" + mes_data.ToString();
                                            lnk_Edt_VerAvance.Text = "<b>Ver Avance</b>";
                                            if (String.IsNullOrEmpty(rsTipo2.Rows[0]["ObservacionesInterventor"].ToString()))
                                                lnk_Edt_VerAvance.Text = "<b>Editar Avance</b>";
                                            lnk_Edt_VerAvance.Style.Add("text-decoration", "none");
                                            lnk_Edt_VerAvance.Style.Add("margin-left", "2px");
                                            lnk_Edt_VerAvance.CommandName = "VerAvance";
                                            lnk_Edt_VerAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            lnk_Edt_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(lnk_Edt_VerAvance);


                                            #endregion
                                        }
                                    }
                                    else if (usuario.CodGrupo == Constantes.CONST_Interventor 
                                        || usuario.CodGrupo == Constantes.CONST_Asesor 
                                        || usuario.CodGrupo == Constantes.CONST_JefeUnidad 
                                        || usuario.CodGrupo == Constantes.CONST_CallCenter
                                        || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                                        || usuario.CodGrupo == Constantes.CONST_LiderRegional 
                                        || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                                        || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                                        || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                                    {
                                        #region Ver avance.

                                        if (mes_data <= prorrogaTotal)
                                        {
                                            ImageButton img_Ver_Avance_EI = new ImageButton();
                                            LinkButton lnk_Ver_Avance_EI = new LinkButton();

                                            //ImageButton.
                                            img_Ver_Avance_EI.ID = "img_Ver_Avance_EI" + mes_data.ToString();
                                            img_Ver_Avance_EI.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                            img_Ver_Avance_EI.AlternateText = "Ver Avance";
                                            img_Ver_Avance_EI.CommandName = "actualizar";
                                            img_Ver_Avance_EI.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            img_Ver_Avance_EI.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(img_Ver_Avance_EI);

                                            //LinkButton.
                                            lnk_Ver_Avance_EI.ID = "lnk_Ver_Avance_EI" + mes_data.ToString();
                                            lnk_Ver_Avance_EI.Text = "<b>Ver Avance</b>";
                                            lnk_Ver_Avance_EI.Style.Add("text-decoration", "none");
                                            lnk_Ver_Avance_EI.Style.Add("margin-left", "2px");
                                            lnk_Ver_Avance_EI.CommandName = "VerAvance";
                                            lnk_Ver_Avance_EI.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            lnk_Ver_Avance_EI.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(lnk_Ver_Avance_EI);
                                        }

                                        #endregion
                                    }
                                }
                                else
                                {
                                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                                    {
                                        #region Avance.

                                        if (mes_data <= prorrogaTotal)
                                        {
                                            ImageButton img_Avance_DOS = new ImageButton();
                                            LinkButton lnk_Avance_DOS = new LinkButton();

                                            //ImageButton.
                                            img_Avance_DOS.ID = "img_Avance_DOS" + mes_data.ToString();
                                            img_Avance_DOS.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                            img_Avance_DOS.CommandName = "Nuevo";
                                            img_Avance_DOS.CommandArgument = "Nuevo" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            img_Avance_DOS.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(img_Avance_DOS);

                                            //LinkButton.
                                            lnk_Avance_DOS.ID = "lnk_Ver_Avance_EI" + mes_data.ToString();
                                            lnk_Avance_DOS.Text = "<b>Avance</b>";
                                            lnk_Avance_DOS.Style.Add("text-decoration", "none");
                                            lnk_Avance_DOS.Style.Add("margin-left", "2px");
                                            lnk_Avance_DOS.CommandName = "Nuevo";
                                            lnk_Avance_DOS.CommandArgument = "Nuevo" + ";" + Cod_Proyecto + ";" + Cod_Ventas + ";" + mes_data + ";" + CodProducto;
                                            lnk_Avance_DOS.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(lnk_Avance_DOS);
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

        /// <summary>
        /// Adicionar Actividad al Plan Operativo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            //Cargar();
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["pagina"] = "Ventas";
            Redirect(null, "../evaluacion/CatalogoVentasTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Adicionar Mano de Obra al Plan Operativo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       /* protected void LinkButton2_Click(object sender, EventArgs e)
        {
            return;
        }*/

        /// <summary>
        /// Mauricio Arias Olave.
        /// 07/04/2014.
        /// Generación de las filas de nómina.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_Detallesproductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex == 1)
                {
                    //Colocar los valores de avances de color rojo.
                    e.Row.ForeColor = System.Drawing.Color.Red;

                    #region Label
                    var fondo1F = e.Row.FindControl("fondo1F") as Label;
                    var fondo2F = e.Row.FindControl("fondo2F") as Label;
                    var fondo3F = e.Row.FindControl("fondo3F") as Label;
                    var fondo4F = e.Row.FindControl("fondo4F") as Label;
                    var fondo5F = e.Row.FindControl("fondo5F") as Label;
                    var fondo6F = e.Row.FindControl("fondo6F") as Label;
                    var fondo7F = e.Row.FindControl("fondo7F") as Label;
                    var fondo8F = e.Row.FindControl("fondo8F") as Label;
                    var fondo9F = e.Row.FindControl("fondo9F") as Label;
                    var fondo10F = e.Row.FindControl("fondo10F") as Label;
                    var fondo11F = e.Row.FindControl("fondo11F") as Label;
                    var fondo12F = e.Row.FindControl("fondo12F") as Label;
                    var fondo13F = e.Row.FindControl("fondo13F") as Label;
                    var fondo14F = e.Row.FindControl("fondo14F") as Label;
                    #endregion

                    #region Validacion Mostrar Avances

                    if (fondo1F != null)
                    {
                        Sfondo1F = fondo1F.Text;
                    }
                    if (fondo2F != null)
                    {
                        Sfondo2F = fondo2F.Text;
                    }
                    if (fondo3F != null)
                    {
                        Sfondo3F = fondo3F.Text;
                    }
                    if (fondo4F != null)
                    {
                        Sfondo4F = fondo4F.Text;
                    }
                    if (fondo5F != null)
                    {
                        Sfondo5F = fondo5F.Text;
                    }
                    if (fondo6F != null)
                    {
                        Sfondo6F = fondo6F.Text;
                    }
                    if (fondo7F != null)
                    {
                        Sfondo7F = fondo7F.Text;
                    }
                    if (fondo8F != null)
                    {
                        Sfondo8F = fondo8F.Text;
                    }
                    if (fondo9F != null)
                    {
                        Sfondo9F = fondo9F.Text;
                    }

                    if (fondo10F != null)
                    {
                        Sfondo10F = fondo10F.Text;
                    }

                    if (fondo11F != null)
                    {
                        Sfondo11F = fondo11F.Text;
                    }

                    if (fondo12F != null)
                    {
                        Sfondo12F = fondo12F.Text;
                    }
                    if (fondo13F != null)
                    {
                        Sfondo13F = fondo13F.Text;
                    }
                    if (fondo14F != null)
                    {
                        Sfondo14F = fondo14F.Text;
                    }

                    #endregion
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (e.Row.RowIndex == -1)
                {
                    #region imagenes

                    var imgAvance1F = e.Row.FindControl("imgAvance1") as Image;
                    var imgAvance2F = e.Row.FindControl("imgAvance2") as Image;
                    var imgAvance3F = e.Row.FindControl("imgAvance3") as Image;
                    var imgAvance4F = e.Row.FindControl("imgAvance4") as Image;
                    var imgAvance5F = e.Row.FindControl("imgAvance5") as Image;
                    var imgAvance6F = e.Row.FindControl("imgAvance6") as Image;
                    var imgAvance7F = e.Row.FindControl("imgAvance7") as Image;
                    var imgAvance8F = e.Row.FindControl("imgAvance8") as Image;
                    var imgAvance9F = e.Row.FindControl("imgAvance9") as Image;
                    var imgAvance10F = e.Row.FindControl("imgAvance10") as Image;
                    var imgAvance11F = e.Row.FindControl("imgAvance11") as Image;
                    var imgAvance12F = e.Row.FindControl("imgAvance12") as Image;
                    var imgAvance13F = e.Row.FindControl("imgAvance13") as Image;
                    var imgAvance14F = e.Row.FindControl("imgAvance14") as Image;

                    #endregion

                    #region link

                    var lnkactividad1F = e.Row.FindControl("lnkactividad1") as LinkButton;
                    var lnkactividad2F = e.Row.FindControl("lnkactividad2") as LinkButton;
                    var lnkactividad3F = e.Row.FindControl("lnkactividad3") as LinkButton;
                    var lnkactividad4F = e.Row.FindControl("lnkactividad4") as LinkButton;
                    var lnkactividad5F = e.Row.FindControl("lnkactividad5") as LinkButton;
                    var lnkactividad6F = e.Row.FindControl("lnkactividad6") as LinkButton;
                    var lnkactividad7F = e.Row.FindControl("lnkactividad7") as LinkButton;
                    var lnkactividad8F = e.Row.FindControl("lnkactividad8") as LinkButton;
                    var lnkactividad9F = e.Row.FindControl("lnkactividad9") as LinkButton;
                    var lnkactividad10F = e.Row.FindControl("lnkactividad10") as LinkButton;
                    var lnkactividad11F = e.Row.FindControl("lnkactividad11") as LinkButton;
                    var lnkactividad12F = e.Row.FindControl("lnkactividad12") as LinkButton;
                    var lnkactividad13F = e.Row.FindControl("lnkactividad13") as LinkButton;
                    var lnkactividad14F = e.Row.FindControl("lnkactividad14") as LinkButton;

                    #endregion

                    #region Si los valores empiezan con "$0" ó están vacíos, el enlace e imagen ligados a los datos estarán ocultos.

                    if (string.IsNullOrEmpty(Sfondo1F) || Sfondo1F.StartsWith("$0"))
                    {
                        imgAvance1F.Visible = false;
                        lnkactividad1F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo2F) || Sfondo2F.StartsWith("$0"))
                    {
                        imgAvance2F.Visible = false;
                        lnkactividad2F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo3F) || Sfondo3F.StartsWith("$0"))
                    {
                        imgAvance3F.Visible = false;
                        lnkactividad3F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo4F) || Sfondo4F.StartsWith("$0"))
                    {
                        imgAvance4F.Visible = false;
                        lnkactividad4F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo5F) || Sfondo5F.StartsWith("$0"))
                    {
                        imgAvance5F.Visible = false;
                        lnkactividad5F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo6F) || Sfondo6F.StartsWith("$0"))
                    {
                        imgAvance6F.Visible = false;
                        lnkactividad6F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo7F) || Sfondo7F.StartsWith("$0"))
                    {
                        imgAvance7F.Visible = false;
                        lnkactividad7F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo8F) || Sfondo8F.StartsWith("$0"))
                    {
                        imgAvance8F.Visible = false;
                        lnkactividad8F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo9F) || Sfondo9F.StartsWith("$0"))
                    {
                        imgAvance9F.Visible = false;
                        lnkactividad9F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo10F) || Sfondo10F.StartsWith("$0"))
                    {
                        imgAvance10F.Visible = false;
                        lnkactividad10F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo11F) || Sfondo11F.StartsWith("$0"))
                    {
                        imgAvance11F.Visible = false;
                        lnkactividad11F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo12F) || Sfondo12F.StartsWith("$0"))
                    {
                        imgAvance12F.Visible = false;
                        lnkactividad12F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo13F) || Sfondo13F.StartsWith("$0"))
                    {
                        imgAvance13F.Visible = false;
                        lnkactividad13F.Visible = false;
                    }
                    if (string.IsNullOrEmpty(Sfondo14F) || Sfondo14F.StartsWith("$0"))
                    {
                        imgAvance14F.Visible = false;
                        lnkactividad14F.Visible = false;
                    }

                    #endregion

                    #region Habilitar enlaces de "Ver Avances". (Mauricio Arias Olave "": Si es "Interventor" hace la consulta. si no, no.)

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {

                        //Recorrer los 14 meses.
                        for (int i = 1; i <= 14; i++)
                        {
                            //Instanciar strings.
                            string objeto = "lnkactividad" + i;
                            string obj_img = "imgAvance" + i;

                            //Generar nuevos valores.
                            var link = e.Row.FindControl(objeto) as LinkButton;
                            var imga = e.Row.FindControl(obj_img) as Image;

                            //Si los valores instanciados NO son null, entra en el flujo. 
                            if (link != null && imga != null)
                            {
                                string txtSQL = "SELECT * FROM AvanceVentasPOMes WHERE CodProducto=" + Convert.ToInt32(HttpContext.Current.Session["CodProduccion"].ToString()) + " AND mes=" + i + "";
                                var reader = consultas.ObtenerDataTable(txtSQL, "text"); // ejecutaReader(txtSQL, 1);

                                if (reader.Rows.Count == 0)
                                {
                                    //Si NO devuelve datos, los elementos instanciados estarán invisibles.
                                    imga.Visible = false;
                                    imga.Enabled = false;
                                    link.Visible = false;
                                    link.Enabled = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        imgAvance1F.Visible = false;
                        lnkactividad1F.Visible = false;
                        imgAvance2F.Visible = false;
                        lnkactividad2F.Visible = false;
                        imgAvance3F.Visible = false;
                        lnkactividad3F.Visible = false;
                        imgAvance4F.Visible = false;
                        lnkactividad4F.Visible = false;
                        imgAvance5F.Visible = false;
                        lnkactividad5F.Visible = false;
                        imgAvance6F.Visible = false;
                        lnkactividad6F.Visible = false;
                        imgAvance7F.Visible = false;
                        lnkactividad7F.Visible = false;
                        imgAvance8F.Visible = false;
                        lnkactividad8F.Visible = false;
                        imgAvance9F.Visible = false;
                        lnkactividad9F.Visible = false;
                        imgAvance10F.Visible = false;
                        lnkactividad10F.Visible = false;
                        imgAvance11F.Visible = false;
                        lnkactividad11F.Visible = false;
                        imgAvance12F.Visible = false;
                        lnkactividad12F.Visible = false;
                        imgAvance13F.Visible = false;
                        lnkactividad13F.Visible = false;
                        imgAvance14F.Visible = false;
                        lnkactividad14F.Visible = false;
                    }

                    #endregion
                }
            }
        }

        /// <summary>
        /// Alex Falutero
        /// 2014/12/06
        /// Metodo que se encarga de enviar la solicitud de eliminar  un producto. La solicitud llega al interventor
        /// para que la apruebe y sea enviada a eliminar por parte del gerente interventor.
        public void EliminarProductoSeleccionado(String id_ventas,String NomProducto)
        {
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            //int consulta = 0;
            //bool procesado = true;
            try
            {
                if (NomProducto == "VENTAS TOTALES")
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No es posible eliminar los Totales.')", true);
                    return;
                }
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Crea registros en tablas temporales "para la aprobación de borrado por parte del coordinador y el gerente"
                    sqlConsulta = "select top 1 CodContacto from TabInterventorProyecto where codProyecto = " + CodProyecto;

                    var idInterventor = consultas.ObtenerDataTable(sqlConsulta, "text").Rows[0].ItemArray[0].ToString();
                    if (!string.IsNullOrEmpty(idInterventor))
                    {
                        //Ejecutar la inserción de los datos que se  envían a Borrar
                        sqlConsulta = "INSERT INTO InterventorVentasTMP (id_ventas,CodProyecto,NomProducto,ChequeoCoordinador,Tarea,ChequeoGerente) " +
                                          "VALUES (" + id_ventas + ", " + CodProyecto + ",'" + NomProducto + "',null,'Borrar',null) ";

                        ejecutaReader(sqlConsulta, 2);

                        var venta = (from v in consultas.Db.InterventorVentasTMPs
                                     where v.CodProyecto == CodProyecto && v.id_ventas == int.Parse(id_ventas)
                                     select v).FirstOrDefault();

                        //Agendar tarea.
                        var txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                        var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                        var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());

                        var proyecto = (from p in consultas.Db.Proyecto1s
                                        where p.Id_Proyecto == CodProyecto
                                        select p).FirstOrDefault();

                        AgendarTarea agenda = new AgendarTarea
                        (idCoord,
                        "Eliminación de Producto en Ventas enviada por Interventor",
                        "Revisar actividad del plan operativo " + proyecto.NomProyecto + " - Actividad --> " + venta.NomProducto + " Observaciones: ",
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
                        "Catálogo Actividad Plan Operativo");

                        agenda.Agendar();

                        CargarGridProductosVentasEnProduccion();
                        CargarGridProductosEnAprobacion();

                        if (GrvActividadesNoAprobadas.Rows.Count > 0)
                        {
                            lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: " + GrvActividadesNoAprobadas.Rows.Count;
                        }
                        else
                        {
                            lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: 0";
                        }
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto seleccionado enviado a eliminar.');window.opener.location = window.opener.location;", true);
                        
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto sin Interventor asignado.')", true);
                    }
                    #endregion
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 11/04/2014.
        /// Generar botón de eliminación "visible" sólo cuando las condiciones se cumplan:
        /// La condición es que el valor "VentasID" sea cero, para obtener este valor, debe consultar 
        /// el procedimiento almacenado "MD_ProductosEnVentas" o en su defecto
        /// el archivo .sql "MasConsultas.sql".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //VentasID
                var labelActividadPo = e.Row.FindControl("lblactividaPOI") as Label;
                var imgEditar = e.Row.FindControl("imgeditar") as Image;
                var lnk_ver = e.Row.FindControl("lbl_nombreProducto") as LinkButton;
                String TxtSQL;
                

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar para el caso de que el usuario sea un "Interventor".
                    if (labelActividadPo != null)
                    {   
                        
                        // Se debe mostrar el icono de borrar solo cuando no hay dineros ejecutados 
                        //TxtSQL = " SELECT SUM(Valor) as Ejecucion FROM AvanceVentasPOMes WHERE CodTipoFinanciacion = 1 AND  CodProducto = " + labelActividadPo.Text + " GROUP BY CodTipoFinanciacion";
                        TxtSQL = "select * from AvanceVentasPOMes	where CodProducto = " + labelActividadPo.Text;
                        var c_1 = consultas.ObtenerDataTable(TxtSQL, "text");

                        if (c_1.Rows.Count > 0)// Si hay registros => evaluamos la cantidad de unidades
                        {
                            ////int CantVentas = Convert.ToInt32(c_1.Rows[0]["Ventas"]);
                            //int CantVentas = Convert.ToInt32(c_1.Rows[0]["Ejecucion"]);
                            //if (CantVentas == 0 && imgEditar != null)// Si cantidad de unidades es cero (0) => se puede borrar
                            //{
                            //    imgEditar.Visible = true;
                            //    imgEditar.ToolTip = "Eliminar Actividad.";
                            //}
                            //else
                            //{
                            //    imgEditar.Visible = false;
                            //}

                            imgEditar.Visible = false;
                            imgEditar.ToolTip = "Eliminar Actividad.";
                        }
                        else
                        {
                            imgEditar.Visible = true;
                            imgEditar.ToolTip = "Eliminar Actividad.";
                        }
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
                }
            }
        }

        #region Productos en Ventas en Aprobación - Métodos -

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/04/2014.
        /// Cargar los productos en aprobación.
        /// </summary>
        protected void CargarGridProductosEnAprobacion()
        {
            //Inicializar variables.
            var respuestaDetalle = new DataTable();

            consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodProyecto" ,Value = CodProyecto //CodCargo
                                                   }
                                           };

            respuestaDetalle = consultas.ObtenerDataTable("MD_ListaDeCargosEnAprobacion_Ventas");

            if (respuestaDetalle.Rows.Count != 0)
            {
                GrvActividadesNoAprobadas.DataSource = respuestaDetalle;
                GrvActividadesNoAprobadas.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/04/2014.
        /// RowCommand para mostrar resultados en los meses del producto en aprobación seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvActividadesNoAprobadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "editar")
            {
                HttpContext.Current.Session["Accion"] = "actualizar";
                HttpContext.Current.Session["proyecto"] = CodProyecto;
                HttpContext.Current.Session["CodProducto"] = e.CommandArgument.ToString();
                Redirect(null, "../evaluacion/CatalogoVentasAprobar_TMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            if (e.CommandName == "mostrar1")
            {
                clik_mostrar = 1;
                CargarDetalle(e.CommandArgument.ToString());
            }
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 22/04/2014: Se modifica el diseño y el código para enviarle los valores a consultar y
        /// procesar en la página que se cargará.
        /// 26/04/2014: Modificación para enviarle el mes seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_Detallesproductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Versión de Mauricio Arias Olave.
            if (e.CommandName == "editar")
            {
                HttpContext.Current.Session["Accion"] = "actualizar";
                HttpContext.Current.Session["proyecto"] = CodProyecto;
                //Ya se le está enviado el producto seleccionado en una variable de sesión.
                HttpContext.Current.Session["MesDelProducto_Venta_Seleccionado"] = e.CommandArgument;
                //Ya se le está enviando el nombre del producto seleccionado en una variable de sesión.
                HttpContext.Current.Session["pagina"] = "Ventas";
                //Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
                //         "menubar=0,scrollbars=1,width=710,height=400,top=100");
                Redirect(null, "../evaluacion/CatalogoVentasPOInterventoria.aspx", "_blank",
                           "menubar=0,scrollbars=1,width=710,height=400,top=100");
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

                    //Controles para "Adicionar Producto al Plan Operativo".
                    lblvalidador.Visible = true;
                    Adicionar.Visible = true;
                    LinkButton1.Visible = true;

                    #endregion
                }
                else
                {
                    #region Deshabilitar/Ocultar campos que el usuario logueado NO puede operar.

                    //Controles para "Adicionar Producto al Plan Operativo".
                    lblvalidador.Visible = false;
                    Adicionar.Visible = false;
                    LinkButton1.Visible = false;

                    //Control "Productos Pendientes de Aprobar:".
                    lblpuestosPendientesConteo.Visible = false;

                    #endregion
                }
            }
            catch (Exception) { throw; }
        }

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
    }
}