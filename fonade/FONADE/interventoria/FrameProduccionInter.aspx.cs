using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Web.UI;
using System.Configuration;
using System.Web;
using Fonade.Clases;

namespace Fonade.FONADE.interventoria
{
    public partial class FrameProduccionInter : Negocio.Base_Page
    {
        #region Propiedades.
        public int prorroga;
        public int prorrogaTotal;
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

            CodProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"] ?? "0");

            #region GENERAR GRILLA DINÁMICA DESCOMENTADO.
            //Producto seleccionado.
            string interno_CodProduccion = Convert.ToString(HttpContext.Current.Session["interno_CodProduccion"]??"0");
            if (interno_CodProduccion != "0")
            {
                GenerarTabla(interno_CodProduccion, CodProyecto.ToString());
            }
            #endregion

            if (!IsPostBack)
            {
                t_anexos.Rows.Clear();
                try
                {
                    CargarGridProductosEnProduccion();
                    CargarGridProductosEnAprobacion();

                    //Obtener conteo de productos pendientes por aprobar.
                    if (GrvActividadesNoAprobadas.Rows.Count > 0)
                    { lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: " + GrvActividadesNoAprobadas.Rows.Count; }
                    else { lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: 0"; }

                    //Evaluar si ciertos campos se mostrarán.
                    EvaluarCampos(usuario.CodGrupo);

                    //Mostrar columna de insumos "sólo si el usuario en sesión NO es un Interventor.
                    if (usuario.CodGrupo != Constantes.CONST_Interventor)
                    {
                        gv_productos.Columns[0].Visible = false;

                        //Controlar...
                        gv_productos.Columns[6].Visible = true;
                        gv_productos.Columns[5].Visible = true; 
                    }
                }
                catch (Exception)
                {
                    throw;
                    // Response.Redirect("~/Account/Login.aspx");
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
        protected void CargarGridProductosEnProduccion()
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

            respuestaDetalle = consultas.ObtenerDataTable("MD_ProductosEnProduccion");

            if (respuestaDetalle.Rows.Count != 0)
            {
                gv_productos.DataSource = respuestaDetalle;
                gv_productos.DataBind();
            }
        }

        /// <summary>
        /// Paginación de la grilla de personal calificado "grilla ubicada a la izquierda de la pantalla".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gv_productos.PageIndex = e.NewPageIndex;
            CargarGridProductosEnProduccion();
        }

        protected void Adicionar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["pagina"] = "Produccion";
            Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Método que recibe el codigo del elemento a procesar yel mes del elemento seleccionado
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
                var query = consultas.Db.AvanceProduccionPOMes.FirstOrDefault(am => am.CodProducto == codactividad && am.Mes == mes && am.CodTipoFinanciacion == tipoFinanciacion);

                if (query != null && query.CodProducto != 0)
                { dtAvance = query.Valor; }
            }
            catch (Exception)
            {
                //throw new Exception(ex.Message);
                throw;
            }

            return dtAvance;
        }

        /// <summary>
        /// Seleccionar el ítem y llamar a "CatalogoProduccionTMP".
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
                HttpContext.Current.Session["pagina"] = "Produccion";
                Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            if (e.CommandName == "mostrar")
            {
                #region Código que separa el código del producto y su nombre para añadir los valores a variables de sesión.
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Nueva línea de código para almacenar nombre del producto seleccionado en sesión.
                HttpContext.Current.Session["CodProduccion"] = valores_command[0];
                HttpContext.Current.Session["NombreDelProductoSeleccionado"] = valores_command[1];
                #endregion

                CargarDetalle(e.CommandArgument.ToString());
            }
            if (e.CommandName == "insumos")
            {
                #region Invocar ventana modal para mostrar los insumos del producto seleccionado.
                //Separar los valores.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Crear variables de sesión para implementar en página a invocar.
                HttpContext.Current.Session["s_CodProduccion"] = valores_command[0];
                HttpContext.Current.Session["s_NombreProducto"] = valores_command[1];
                HttpContext.Current.Session["s_CodProyecto"] = valores_command[2];

                //Enviar información procesada a ventana informativa.
                Redirect(null, "InsumoProductoInterventor.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=460,height=317,top=100");
                #endregion
            }

            #region Borrar los valores de la proyección mensual de ventas
            // Diciembre 9 de 2014
            // Alex Flautero
            if (e.CommandName == "eliminar")
            {
                //Session["CodProducto"] = e.CommandArgument.ToString();
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                EliminarProductoSeleccionado(valores_command[0], valores_command[1]);
            }

            #endregion
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
                int codigoProducto = 0;

                try
                {
                    codigoProducto = Int32.Parse(variables[0]);
                    HttpContext.Current.Session["interno_CodProduccion"] = variables[0].ToString();
                    GenerarTabla(variables[0], CodProyecto.ToString());
                }
                catch (Exception) { return; }
            }

        }

        /// <summary>
        /// Método dinámico que genera una ventana emergente para mostrar los detalles/avances del
        /// producto seleccionado.
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

                HttpContext.Current.Session["Accion"] = valores_command[0];
                HttpContext.Current.Session["proyecto"] = valores_command[1];
                HttpContext.Current.Session["CodProduccion"] = valores_command[2];
                HttpContext.Current.Session["CodProyecto"] = CodProyecto.ToString();
                //Ya se le está enviado la nómina seleccionada en una variable de sesión.
                HttpContext.Current.Session["MesDelProductoSeleccionado"] = valores_command[3].ToString();
                //Ya se le está enviando el nombre del cargo seleccionado en una variable de sesión.
                HttpContext.Current.Session["pagina"] = "Producción";
                Redirect(null, "../evaluacion/CatalogoProduccionPOInterventoria.aspx", "_blank",
                           "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Intento #2 de generar grilla dinámica "según diseño de Table realizado por Diego Quiñonez" en informes.
        /// </summary>
        /// <param name="Cod_Produccion">Código de la producción seleccionada.</param>
        /// <param name="Cod_Proyecto">Código del proyecto.</param>
        private void GenerarTabla(String Cod_Produccion, String Cod_Proyecto)
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
            double valor1 = 0;
            double totalTot = 0;
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
                    celdaSueldo.Text = "Cantidad";
                    celdaSueldo.Attributes.Add("title", "Cantidad Presupuestada");
                    fila.Cells.Add(celdaSueldo);
                    t_anexos.Rows.Add(fila);
                    celdaSueldo = null;

                    //Agregar datos a la celda de Prestaciones Sociales.
                    celdaPrestaciones.Text = "Costo";
                    celdaPrestaciones.Attributes.Add("title", "Costo de Producción Estimado");
                    fila.Cells.Add(celdaPrestaciones);
                    t_anexos.Rows.Add(fila);
                    celdaPrestaciones = null;
                }
                #endregion

                #region Consulta SQL inicial.
                if (clik_mostrar == 0)
                {
                    txtSQL = " SELECT DISTINCT * FROM InterventorProduccion left join InterventorProduccionMes " +
                         " on id_produccion = codproducto WHERE codproyecto = " + Cod_Proyecto +
                         " AND id_produccion = " + Cod_Produccion + " ORDER BY id_produccion, mes, tipo ";
                }
                else if (clik_mostrar == 1)
                {
                    txtSQL = " SELECT DISTINCT * FROM InterventorProduccionTMP left join InterventorProduccionMesTMP " +
                         " on id_produccion = codproducto WHERE codproyecto = " + Cod_Proyecto +
                         " AND id_produccion = " + Cod_Produccion + " ORDER BY id_produccion, mes, tipo ";
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
                        
                        //numProductoValor = 1;
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
                                            valor1 = Valor;
                                            TotalFE = TotalFE + Valor;
                                            Valor_FE = Valor.ToString();
                                            //Variable que contiene el mismo valor de la variable "Valor", la cual será multiplicada mas adelante.
                                            numProductoValor = Valor;
                                        }
                                        if (row["Tipo"].ToString() == "2")
                                        {
                                            numProductoValor = Valor;
                                            TotalEmp = TotalEmp + Double.Parse(row["Valor"].ToString()); // *numProductoValor;
                                            Valor_Emp = (Double.Parse(row["Valor"].ToString()) + numProductoValor).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                            Valor_Emp = (Valor * valor1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                            totalTot += Valor * valor1;
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
                                        //celdaEspacio.Text = TotalEmp.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                        celdaEspacio.Text = totalTot.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
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
                                         " from AvanceProduccionPOMes " +
                                         " where CodProducto = " + Cod_Produccion +
                                         " and Mes = " + mes_data +" and codtipofinanciacion = 1  ";

                                //Asignar resultados a variable DataTable.
                                rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");
                                var producto = (from p in consultas.Db.AvanceProduccionPOMes
                                                where p.CodProducto == int.Parse(Cod_Produccion) && p.Mes == mes_data
                                                select p).ToList();

                                if(producto.Count > 0)
                                {
                                    //valor_tipo1 = 
                                }

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
                                         " from AvanceProduccionPOMes " +
                                         " where CodProducto = " + Cod_Produccion +
                                         " and Mes=" + j +" and codtipofinanciacion = 2 ";

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
                                txtSQL = " SELECT * FROM AvanceProduccionPOMes " +
                                         " WHERE CodProducto = " + Cod_Produccion + " AND mes = " + mes_data;

                                //Asignar resultados a la variable DataTable.
                                rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");

                                //Determinar el valor de la vairiable "ejecutar" de acuerdo a la tabla.
                                if (rsTipo2.Rows.Count > 0)
                                { ejecutar = 1; /*Si existe se coloca la opción de editar y borrar*/ }
                                else
                                { ejecutar = 2; /*Si NO existe se coloca la opción de adicionar*/ }

                                try { CodProducto = Cod_Produccion.Replace("+", "$"); }
                                catch { }

                                if (ejecutar == 1)
                                {
                                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                                    {
                                        //False
                                        if (rsTipo2.Rows[0]["Aprobada"].ToString() == "True" || rsTipo2.Rows[0]["Aprobada"].ToString() == "1")
                                        {
                                            #region Ver avance.

                                            if (mes_data <= prorrogaTotal)
                                            {
                                                ImageButton img_VerAvance = new ImageButton();
                                                LinkButton lnk_VerAvance = new LinkButton();

                                                //ImageButton.
                                                img_VerAvance.ID = "img_VerAvance_" + mes_data.ToString();
                                                img_VerAvance.ImageUrl = "~/Images/icoAdicionarUsuario.gif";
                                                img_VerAvance.AlternateText = "Avance";
                                                img_VerAvance.CommandName = "actualizar";
                                                img_VerAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                                img_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                                                { img_VerAvance = null; }
                                                else
                                                { celda.Controls.Add(img_VerAvance); }

                                                //LinkButton.
                                                lnk_VerAvance.ID = "lnk_VerAvance_" + mes_data.ToString();
                                                lnk_VerAvance.Text = "<b>Ver Avance</b>";
                                                lnk_VerAvance.Style.Add("text-decoration", "none");
                                                lnk_VerAvance.Style.Add("margin-left", "2px");
                                                lnk_VerAvance.CommandName = "VerAvance";
                                                lnk_VerAvance.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                                lnk_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                                                { lnk_VerAvance = null; }
                                                else
                                                { celda.Controls.Add(lnk_VerAvance); }
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            if (String.IsNullOrEmpty(rsTipo2.Rows[0]["ObservacionesInterventor"].ToString()))
                                            {
                                                #region Eliminar avance.

                                                if (mes_data <= prorrogaTotal)
                                                {
                                                    ImageButton img_Elim_VerAvance = new ImageButton();

                                                    //ImageButton.
                                                    img_Elim_VerAvance.ID = "img_Elim_VerAvance_" + mes_data.ToString();
                                                    img_Elim_VerAvance.ImageUrl = "~/Images/icoBorrar.gif";
                                                    img_Elim_VerAvance.AlternateText = "Avance";
                                                    img_Elim_VerAvance.CommandName = "borrar";
                                                    img_Elim_VerAvance.CommandArgument = "borrar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                                    img_Elim_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                    img_Elim_VerAvance.OnClientClick = "return borrar();";
                                                    celda.Controls.Add(img_Elim_VerAvance);
                                                }


                                                #endregion
                                            }

                                            #region Editar

                                            if (mes_data <= prorrogaTotal)
                                            {
                                                ImageButton img_Edt_VerAvance = new ImageButton();
                                                LinkButton lnk_Edt_VerAvance = new LinkButton();

                                                //ImageButton.
                                                img_Edt_VerAvance.ID = "img_Edt_VerAvance_" + mes_data.ToString();
                                                img_Edt_VerAvance.ImageUrl = "~/Images/editar.png";
                                                img_Edt_VerAvance.AlternateText = "Editar Avance";
                                                img_Edt_VerAvance.CommandName = "actualizar";
                                                img_Edt_VerAvance.CommandArgument = "Modificar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                                img_Edt_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(img_Edt_VerAvance);

                                                //LinkButton.
                                                lnk_Edt_VerAvance.ID = "lnk_VerAvance_" + mes_data.ToString();
                                                lnk_Edt_VerAvance.Text = "<b>Editar</b>";
                                                lnk_Edt_VerAvance.Style.Add("text-decoration", "none");
                                                lnk_Edt_VerAvance.Style.Add("margin-left", "2px");
                                                lnk_Edt_VerAvance.CommandName = "VerAvance";
                                                lnk_Edt_VerAvance.CommandArgument = "Modificar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                                lnk_Edt_VerAvance.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                                celda.Controls.Add(lnk_Edt_VerAvance);
                                            }


                                            #endregion
                                        }
                                    }
                                    else if (usuario.CodGrupo == Constantes.CONST_Interventor 
                                            || usuario.CodGrupo == Constantes.CONST_Asesor 
                                            || usuario.CodGrupo == Constantes.CONST_JefeUnidad 
                                            || usuario.CodGrupo == Constantes.CONST_CallCenter
                                            || usuario.CodGrupo == Constantes.CONST_CallCenterOperador
                                            || usuario.CodGrupo == Constantes.CONST_LiderRegional 
                                            || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
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
                                            img_Ver_Avance_EI.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                            img_Ver_Avance_EI.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(img_Ver_Avance_EI);

                                            //LinkButton.
                                            lnk_Ver_Avance_EI.ID = "lnk_Ver_Avance_EI" + mes_data.ToString();
                                            lnk_Ver_Avance_EI.Text = "<b>Ver Avance</b>";
                                            lnk_Ver_Avance_EI.Style.Add("text-decoration", "none");
                                            lnk_Ver_Avance_EI.Style.Add("margin-left", "2px");
                                            lnk_Ver_Avance_EI.CommandName = "VerAvance";
                                            lnk_Ver_Avance_EI.CommandArgument = "actualizar" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
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
                                            img_Avance_DOS.CommandArgument = "Nuevo" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
                                            img_Avance_DOS.Command += new CommandEventHandler(DynamicCommand_VerAvance);
                                            celda.Controls.Add(img_Avance_DOS);

                                            //LinkButton.
                                            lnk_Avance_DOS.ID = "lnk_Ver_Avance_EI" + mes_data.ToString();
                                            lnk_Avance_DOS.Text = "<b>Avance</b>";
                                            lnk_Avance_DOS.Style.Add("text-decoration", "none");
                                            lnk_Avance_DOS.Style.Add("margin-left", "2px");
                                            lnk_Avance_DOS.CommandName = "Nuevo";
                                            lnk_Avance_DOS.CommandArgument = "Nuevo" + ";" + Cod_Proyecto + ";" + Cod_Produccion + ";" + mes_data + ";" + CodProducto;
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
            HttpContext.Current.Session["Accion"] = "Adicionar";
            HttpContext.Current.Session["codProyecto"] = CodProyecto;
            HttpContext.Current.Session["pagina"] = "Produccion";
            Redirect(null, "../evaluacion/CatalogoProduccionTMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=950,height=450,top=100");
        }

        /// <summary>
        /// Adicionar Mano de Obra al Plan Operativo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            return;
        }

        /// <summary>
        /// Alex Flautero
        /// 2014/12/06
        /// Metodo que se encarga de enviar la solicitud de eliminar  un producto. La solicitud llega al interventor
        /// para que la apruebe y sea enviada a eliminar por parte del gerente interventor.
        public void EliminarProductoSeleccionado(String id_produccion, String NomProducto)
        {
            
            //Inicializar variables.
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta;
            int consulta = 0;
            bool procesado = true;
            try
            {
                if(NomProducto == "PRODUCCION TOTAL") {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No es posible eliminar los Totales.')", true);
                    return;
                }

                //&& NomProducto <> 'PRODUCTOS TOTALES'
                if (usuario.CodGrupo == Constantes.CONST_Interventor )
                {
                    #region Crea registros en tablas temporales "para la aprobación de borrado por parte del coordinador y el gerente"
                    sqlConsulta = "select CodContacto from TabInterventorProyecto where codProyecto = " + CodProyecto;
                    // = new SqlCommand(sqlConsulta, connection);
                    //cmd.CommandType = CommandType.Text;
                    var IdContacto = consultas.ObtenerDataTable(sqlConsulta, "text").Rows[0].ItemArray[0].ToString();
                    //procesado = EjecutarSQL(connection, cmd);
                    if (!string.IsNullOrEmpty(IdContacto))
                    {
                        //Ejecutar la inserción de los datos que se  envían a Borrar
                        sqlConsulta = "INSERT INTO InterventorProduccionTMP (id_Produccion,CodProyecto,NomProducto,ChequeoCoordinador,Tarea,ChequeoGerente) " +
                                          "VALUES (" + id_produccion + ", " + CodProyecto + ",'" + NomProducto + "',Null,'Borrar',Null) ";
                        //cmd = new SqlCommand(sqlConsulta, connection);
                        //cmd.CommandType = CommandType.Text;

                        ejecutaReader(sqlConsulta, 2);

                        var producto = (from p in consultas.Db.InterventorProduccionTMPs
                                        where p.CodProyecto == CodProyecto && p.id_produccion == int.Parse(id_produccion)
                                        select p).FirstOrDefault();

                        var proyecto = (from p in consultas.Db.Proyecto1s
                                        where p.Id_Proyecto == CodProyecto
                                        select p).FirstOrDefault();

                        //Agendar tarea. 
                        var txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                        var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                        var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());
                        
                        AgendarTarea agenda = new AgendarTarea
                        (idCoord,
                        "Revisar Productos en Producción. Se ha eliminado un Producto por el Interventor",
                        "Revisar Productos en Producción " + proyecto.NomProyecto + " - Producto --> " + producto.NomProducto + " Observaciones: " ,
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

                        CargarGridProductosEnProduccion();
                        CargarGridProductosEnAprobacion();

                        //Obtener conteo de productos pendientes por aprobar.
                        if (GrvActividadesNoAprobadas.Rows.Count > 0)
                        {
                            lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: " + GrvActividadesNoAprobadas.Rows.Count;
                        }
                        else
                        {
                            lblpuestosPendientesConteo.Text = "Productos Pendientes de Aprobar: 0";
                        }
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Producto seleccionado enviado a eliminar.');window.opener.location=window.opener.location;", true);
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
        /// La condición es que el valor "ProduccionID" sea cero, para obtener este valor, debe consultar 
        /// el procedimiento almacenado "MD_ProductosEnProduccion" o en su defecto
        /// el archivo .sql "MasConsultas.sql".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_productos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ProduccionID
                var labelActividadPo = e.Row.FindControl("lblactividaPOI") as Label;
                var imgEditar = e.Row.FindControl("imgeditar") as Image;
                var lnk_ver = e.Row.FindControl("lbl_nombreProducto") as LinkButton;
                var lnkERliminar = e.Row.FindControl("lnkborrar") as LinkButton;

                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar para el caso de que el usuario sea un "Interventor".
                    if (labelActividadPo != null)
                    {

                        // Se debe mostrar el icono de borrar solo cuando no hay dineros ejecutados 
                        String  TxtSQL = " SELECT SUM(Valor) as Ejecucion FROM AvanceProduccionPOMes WHERE CodTipoFinanciacion = 1 AND  CodProducto = " + labelActividadPo.Text + " GROUP BY CodTipoFinanciacion";

                        var c_1 = consultas.ObtenerDataTable(TxtSQL, "text");

                        if (c_1.Rows.Count > 0)
                        {
                            //int CantVentas = Convert.ToInt32(c_1.Rows[0]["Ventas"]);
                            int CantVentas = Convert.ToInt32(c_1.Rows[0]["Ejecucion"]);
                            if (CantVentas == 0 && imgEditar != null)// Si cantidad de unidades es cero (0) se puede borrar
                            {
                                imgEditar.Visible = true;
                                imgEditar.ToolTip = "Eliminar Producto.";
                            }
                            else
                            {
                                imgEditar.Visible = false;
                            }
                            lnkERliminar.Visible = false;
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

                #region version anterior
                /*if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Procesar para el caso de que el usuario sea un "Interventor".
                    if (labelActividadPo != null)
                    {
                        if (labelActividadPo.Text.Equals("0"))
                        {
                            if (imgEditar != null)
                            {
                                imgEditar.Visible = true;
                                imgEditar.ToolTip = "Eliminar Actividad.";
                            }
                        }
                        else
                        {
                            if (imgEditar != null)
                            {
                                imgEditar.Visible = false;
                            }
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
                }*/
                #endregion
            }
        }

        #region Productos en Aprobación - Métodos -

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

            respuestaDetalle = consultas.ObtenerDataTable("MD_ListaDeCargosEnAprobacion_Produccion");

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
                Redirect(null, "../evaluacion/CatalogoProduccionAprobar_TMP.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            if (e.CommandName == "mostrar1")
            {
                CargarDetalle(e.CommandArgument.ToString());
            }
        }

        #endregion

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
                    
                    lblvalidador.Visible = false;
                    Adicionar.Visible = false;
                    LinkButton1.Visible = false;
                    
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