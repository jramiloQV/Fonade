using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoOperacionCostos : Negocio.Base_Page
    {
        public string codProyecto;
        public int txtTab = Constantes.CONST_CostosInsumos;
        public string codConvocatoria;
        /// <summary>
        /// Variable string que contiene las consultas SQL.
        /// </summary>
        String txtSQL;
        /// <summary>
        /// Tiempo de proyección "numAnios".
        /// </summary>
        Int32 txtTiempoProyeccion;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

        public bool visibleGuardar { get; set; }

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";

            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            //Obtenemos el ProyectoMercadoProyeccionVenta //po = getProyectoOperacion();

            if (!IsPostBack)
            {
                GenerarTablaDeCostosDeProduccion();
                GenerarTablaProyeccionCompras_Unidades();
                GenerarTablaProyeccionCompras_Pesos();
                ObtenerDatosUltimaActualizacion(); /*definirCampos();*/
            }
        }

        #region Método "definirCampos()" comentado.
        //private void definirCampos()
        //{
        //    var query = (from tp in consultas.Db.ProyectoMercadoProyeccionVentas
        //                 where tp.CodProyecto == Convert.ToInt32(codProyecto)
        //                 select tp).FirstOrDefault();

        //    int anios = 0;
        //    if (query != null)
        //    {
        //        anios = query.TiempoProyeccion.HasValue ? query.TiempoProyeccion.Value : 0;
        //    }

        //    string table = "";
        //    table += "<table width='98%' cellpadding='4' cellspacing='1' border='0'>";
        //    table += "<tr class='Titulo' bgcolor='#3D5A87'><td colspan='" + (anios + 1) + "' align='center' class='tituloTabla'>Tabla de costos de producci&oacute;n en pesos(incluido IVA)</td></tr>";

        //    table += "<tr class='Titulo' bgcolor='#3D5A87'><td class='tituloTabla'>Tipo de Insumo</td>";


        //    var qry = from ti in consultas.Db.TipoInsumos
        //              from pi in consultas.Db.ProyectoInsumos
        //              from ppi in consultas.Db.ProyectoProductoInsumos
        //              from ppuv in consultas.Db.ProyectoProductoUnidadesVentas
        //              from pip in consultas.Db.ProyectoInsumoPrecios
        //              where
        //              pi.codTipoInsumo == ti.Id_TipoInsumo
        //              && ppi.CodInsumo == pi.Id_Insumo
        //              && ppuv.CodProducto == ppi.CodProducto
        //              && pip.CodInsumo == pi.Id_Insumo
        //              && pip.Periodo == ppuv.Ano
        //              && ppuv.Unidades != 0 && pi.CodProyecto == Convert.ToInt32(codProyecto)
        //              orderby ti.Id_TipoInsumo ascending
        //              group new { ti, ppuv, pi, ppi, pip } by new { ti.Id_TipoInsumo, ti.NomTipoInsumo, ppuv.Ano } into g
        //              select new costosdeCompras()
        //              {
        //                  IdInsumo = g.Key.Id_TipoInsumo,
        //                  NomInsumo = g.Key.NomTipoInsumo,
        //                  Anio = g.Key.Ano,
        //                  Total = g.Sum(t => (t.ppuv.Unidades.HasValue ? t.ppuv.Unidades.Value : 0) *
        //                       (t.ppi.Cantidad.HasValue ? t.ppi.Cantidad.Value : 0) *
        //                       (((Double)t.ppi.Desperdicio + 100) / 100) *
        //                       (t.pip.Precio.HasValue ? (Double)t.pip.Precio.Value : 0) *
        //                       ((100 + t.pi.IVA) / 100)
        //                       )
        //              };

        //    for (int i = 1; i <= anios; i++)
        //    {
        //        table += "<td align='center' class='tituloTabla'>Año " + i + "</td>";
        //    }

        //    table += "</tr>";

        //    //Totales
        //    double[] totalData = new double[anios];

        //    List<costosdeCompras> result = qry.ToList();
        //    foreach (var fi in result.GroupBy(t => t.NomInsumo))
        //    {
        //        table += "<tr><td>" + fi.Key + "</td>";
        //        for (int i = 1; i <= anios; i++)
        //        {
        //            costosdeCompras ctc = result.First(t => t.Anio == i && t.NomInsumo == fi.Key);
        //            totalData[i - 1] += ctc.Total;
        //            table += "<td>" + ctc.Total.ToString("N2") + "</td>";
        //        }
        //        table += "</tr>";
        //    }

        //    table += "<tr><td><b>total</b></td>";
        //    foreach (double total in totalData)
        //    {
        //        table += "<td>" + total.ToString("N2") + "</td>";
        //    }
        //    table += "</tr></table>";

        //    // Primera Tabla
        //    tabla_pc.InnerHtml = table;

        //    //// Segunda tabla 

        //    table = "<table width='98%' cellpadding='4' cellspacing='1' border='0'>";
        //    table += "<tr class='Titulo' bgcolor='#3D5A87'><td colspan='" + (anios + 1) + "' align='center' class='tituloTabla'>Proyecci&oacute;n de Compras (Unidades)</td></tr>";

        //    table += "<tr class='Titulo' bgcolor='#3D5A87'><td class='tituloTabla'>Tipo de Insumo</td>";

        //    for (int i = 1; i <= anios; i++)
        //    {
        //        table += "<td align='center' class='tituloTabla'>Año " + i + "</td>";
        //    }

        //    table += "</tr>";
        //    foreach (var fi in result.GroupBy(t => new { t.IdInsumo, t.NomInsumo }))
        //    {
        //        var qry2 = from pi in consultas.Db.ProyectoInsumos
        //                   from ppi in consultas.Db.ProyectoProductoInsumos
        //                   from ppuv in consultas.Db.ProyectoProductoUnidadesVentas
        //                   where ppi.CodInsumo == pi.Id_Insumo && ppuv.CodProducto == ppi.CodProducto
        //                       && pi.CodProyecto == Convert.ToInt32(codProyecto)
        //                       && pi.codTipoInsumo == fi.Key.IdInsumo
        //                   group new { ppuv, pi, ppi } by new { pi.codTipoInsumo, pi.nomInsumo, ppuv.Ano } into g
        //                   select new costosdeCompras()
        //                   {
        //                       IdInsumo = g.Key.codTipoInsumo,
        //                       NomInsumo = g.Key.nomInsumo,
        //                       Anio = g.Key.Ano,
        //                       Total = g.Sum(t => (t.ppuv.Unidades.HasValue ? t.ppuv.Unidades.Value : 0) *
        //                            (t.ppi.Cantidad.HasValue ? t.ppi.Cantidad.Value : 0) *
        //                            (((Double)t.ppi.Desperdicio / 100) + 1)
        //                            )
        //                   };
        //        table += "<tr><td colspan='" + (anios + 1) + "' class='TituloDestacados'>" + fi.Key.NomInsumo + "</td></tr>";


        //        List<costosdeCompras> result2 = qry2.ToList();
        //        foreach (var ni in result2.GroupBy(t => new { t.IdInsumo, t.NomInsumo }))
        //        {
        //            table += "<tr><td>" + ni.Key.NomInsumo + "</td>";
        //            for (int i = 1; i <= anios; i++)
        //            {
        //                costosdeCompras ctc = result2.First(t => t.Anio == i && t.IdInsumo == ni.Key.IdInsumo);
        //                table += "<td align='right'>" + ctc.Total.ToString("N2") + "</td>";
        //            }
        //            table += "</tr>";

        //        }
        //        //table += "</tr>";
        //    }
        //    table += "</table>";
        //    tabla_pc2.InnerHtml = table;

        //    // Tercera tabla 
        //    //Totales
        //    double[] totalPesos = new double[anios];
        //    double[] totalIva = new double[anios];


        //    table = "<table width='98%' cellpadding='4' cellspacing='1' border='0'>";
        //    table += "<tr class='Titulo' bgcolor='#3D5A87'><td colspan='" + (anios + 1) + "' align='center' class='tituloTabla'>Proyecci&oacute;n de Compras (Pesos)</td></tr>";

        //    table += "<tr class='Titulo' bgcolor='#3D5A87'><td class='tituloTabla'>Tipo de Insumo</td>";
        //    for (int i = 1; i <= anios; i++)
        //    {
        //        table += "<td align='center' class='tituloTabla'>Año " + i + "</td>";
        //    }

        //    table += "</tr>";
        //    foreach (var fi in result.GroupBy(t => new { t.IdInsumo, t.NomInsumo }))
        //    {
        //        var qry3 = from pi in consultas.Db.ProyectoInsumos
        //                   from ppi in consultas.Db.ProyectoProductoInsumos
        //                   from ppuv in consultas.Db.ProyectoProductoUnidadesVentas
        //                   from pip in consultas.Db.ProyectoInsumoPrecios
        //                   where ppi.CodInsumo == pi.Id_Insumo && ppuv.CodProducto == ppi.CodProducto
        //                   && ppuv.Unidades != 0 && pi.CodProyecto == Convert.ToInt32(codProyecto)
        //                   && pip.CodInsumo == pi.Id_Insumo
        //                   && pi.codTipoInsumo == fi.Key.IdInsumo && pip.Periodo == ppuv.Ano
        //                   group new { pip, ppuv, pi, ppi } by new { pi.codTipoInsumo, pi.nomInsumo, ppuv.Ano } into g
        //                   select new costosdeCompras()
        //                   {
        //                       IdInsumo = g.Key.codTipoInsumo,
        //                       NomInsumo = g.Key.nomInsumo,
        //                       Anio = g.Key.Ano,
        //                       TotalIva = g.Sum(t => (t.ppuv.Unidades.HasValue ? t.ppuv.Unidades.Value : 0) *
        //                           (t.ppi.Cantidad.HasValue ? t.ppi.Cantidad.Value : 0) *
        //                           (((Double)t.ppi.Desperdicio + 100) / 100) *
        //                           (t.pip.Precio.HasValue ? (Double)t.pip.Precio.Value : 0) *
        //                           (t.pi.IVA / 100)
        //                           ),

        //                       Total = g.Sum(t => (t.ppuv.Unidades.HasValue ? t.ppuv.Unidades.Value : 0) *
        //                       (t.ppi.Cantidad.HasValue ? t.ppi.Cantidad.Value : 0) *
        //                       (((Double)t.ppi.Desperdicio + 100) / 100) *
        //                       (t.pip.Precio.HasValue ? (Double)t.pip.Precio.Value : 0)
        //                       )
        //                   };
        //        table += "<tr><td colspan='" + (anios + 1) + "' class='TituloDestacados'>" + fi.Key.NomInsumo + "</td></tr>";



        //        List<costosdeCompras> result3 = qry3.ToList();
        //        foreach (var mi in result3.GroupBy(t => new { t.IdInsumo, t.NomInsumo }))
        //        {
        //            table += "<tr><td>" + mi.Key.NomInsumo + "</td>";
        //            for (int i = 1; i <= anios; i++)
        //            {
        //                costosdeCompras ctc = result3.First(t => t.Anio == i && t.NomInsumo == mi.Key.NomInsumo);
        //                totalPesos[i - 1] += ctc.Total;
        //                totalIva[i - 1] += ctc.TotalIva;
        //                table += "<td align='right'>" + ctc.Total.ToString("N2") + "</td>";
        //            }
        //            table += "</tr>";
        //        }
        //    }

        //    table += "<tr><td><b>total</b></td>";
        //    foreach (double total in totalPesos)
        //    {
        //        table += "<td align='right'>" + total + "</td>";
        //    }
        //    table += "</tr>";

        //    table += "<tr><td><b>total Iva</b></td>";
        //    foreach (double total in totalIva)
        //    {
        //        table += "<td align='right'>" + total + "</td>";
        //    }
        //    table += "</tr>";

        //    table += "<tr><td><b>total</b></td>";
        //    for (int i = 0; i < anios; i++)
        //    {
        //        table += "<td align='right'>" + (totalPesos[i] + totalIva[i]) + "</td>";
        //    }
        //    table += "</tr>";

        //    table += "</table>";
        //    tabla_pc3.InnerHtml = table;

        //} 
        #endregion

        #region Métodos de .

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
        
        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, codConvocatoria);

                #region Obtener el rol.

                //Consulta.
                txtSQL = " SELECT CodContacto, CodRol From ProyectoContacto " +
                         " Where CodProyecto = " + codProyecto + " And CodContacto = " + usuario.IdContacto +
                         " and inactivo=0 and FechaInicio<=getdate() and FechaFin is null ";

                //Asignar variables a DataTable.
                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                //Crear la variable de sesión.
                if (rs.Rows.Count > 0) { HttpContext.Current.Session["CodRol"] = rs.Rows[0]["CodRol"].ToString(); }
                else { HttpContext.Current.Session["CodRol"] = ""; }

                //Destruir la variable.
                rs = null;

                #endregion

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(int.Parse(codProyecto), txtTab).FirstOrDefault();

                if (usuActualizo != null)
                {
                    lbl_nombre_user_ult_act.Text = usuActualizo.nombres.ToUpper();

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(usuActualizo.fecha); }
                    catch { fecha = DateTime.Today; }
                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    //Realizado
                    bRealizado = usuActualizo.realizado;
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                var codRole = HttpContext.Current.Session["CodRol"].ToString();

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && ( (codRole == Constantes.CONST_RolAsesorLider.ToString() || codRole == Constantes.CONST_RolAsesor.ToString()) && CodigoEstado == Constantes.CONST_Inscripcion) || (codRole == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                {
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                    {
                        visibleGuardar = false;
                    }
                    else
                    {
                        visibleGuardar = true;
                        btn_guardar_ultima_actualizacion.Visible = true;
                    }
                }

                if (EsMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {
                    btm_guardarCambios.Visible = true;
                }
                else
                {
                    btm_guardarCambios.Visible = false;
                }

                //Mostrar los enlaces para adjuntar documentos.
                if (EsMiembro && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    tabla_docs.Visible = true;
                }
                visibleGuardar = Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo && !realizado;
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                //tabla = null;
                //txtSQL = null;
                //return;
            }
        }

        /// <summary>
        
        /// 06/06/2014.
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".
        /// El código se encuentra en "Base_Page" línea "116", método "inicioEncabezado".
        /// Ya se le están enviado por parámetro en el método el código del proyecto y la constante "CONST_PostIt".
        /// </summary>
        /// <returns>numPostIt.</returns>
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(codProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
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
                txtSQL = " SELECT TiempoProyeccion FROM ProyectoMercadoProyeccionVentas WHERE codProyecto = " + codProyecto;

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

        /// <summary>
        
        /// 09/06/2014.
        /// Generar la primera tabla llamada "Tabla de costos de producción en pesos(incluido IVA)".
        /// </summary>
        private void GenerarTablaDeCostosDeProduccion()
        {
            //Inicilizar variables.
            Table Tabla_Costos = new Table();
            Tabla_Costos.CssClass = "Grilla";
            Tabla_Costos.Attributes.Add("width", "95%");
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

            try
            {
                //Generar SQL que será concatenado a la consulta principal.
                for (int i = 1; i < Cargar_numAnios() + 1; i++)
                { 
                    strSelect = strSelect + ", sum(case when Ano=" + i.ToString() + " then V.unidades*cantidad*((100+desperdicio)/100.0)*(Precio*((100+IVA)/100.0)) else 0 end) AS [Año " + i.ToString() + "] "; 
                }
            }
            catch (Exception)
            { throw; }

            try{
                //Consulta completa "con la concatenación de la variable (strSelect) generada anteriormente".
                txtSQL = " SELECT T.id_TipoInsumo, T.nomTipoInsumo AS [Tipo de Insumo] " + strSelect +
                         " FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI," +
                         " ProyectoProductoUnidadesVentas V, ProyectoInsumoPrecio P" +
                         " WHERE I.codTipoInsumo = T.id_TipoInsumo AND PI.codInsumo = I.id_Insumo " +
                         " AND V.codProducto = PI.codProducto " +
                         " AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto " +
                         " AND P.periodo = V.ano AND Unidades <> 0 AND I.codProyecto =" + codProyecto +
                         " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo";

                //Asignar resultados a la variable DataTable.
                    tabla_costosProduccion = consultas.ObtenerDataTable(txtSQL, "text");

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

            try{
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
                        celdaEspecial.Text = Double.Parse(row_tabla["Año " + i.ToString()].ToString()).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
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
            try{

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
                                celdaEspecial.Text = "<b>" + Convert.ToDouble(numTotPesos[incr].ToString()).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</b>";
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

                //Añadir al panel la tabla generada y bindear la tabla.
                pnl_tablas_costos.Controls.Add(Tabla_Costos);
                Tabla_Costos.DataBind();

               /* 
                * #region Destruir variables.

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
                * **/

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
            Table Tabla_Proyecciones_1 = new Table();
            Tabla_Proyecciones_1.CssClass = "Grilla";
            Tabla_Proyecciones_1.Attributes.Add("width", "95%");
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
            Label lbl = new Label();
            lbl.ID = "lbl_0";
            lbl.Text = "<br/><br/>";
            pnl_tablas_costos.Controls.Add(lbl);
            lbl = null;

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
                celdaEncabezado.Text = "Proyección de Compras (Unidades)";
                fila1.Cells.Add(celdaEncabezado);
                Tabla_Proyecciones_1.Rows.Add(fila1);

                #endregion

                #region Consulta con la información inicial de los encabezados.

                //Generar SQL que será concatenado a la consulta principal.
                for (int i = 1; i < Cargar_numAnios() + 1; i++)
                { strSelect = strSelect + ", SUM(case when ano=" + i.ToString() + " then V.Unidades*PI.Cantidad*(1+(desperdicio/100.0)) else 0 end) AS [Año " + i.ToString() + "] "; }

                //Consultar los tipos de insumos a generar en la tabla.
                txtSQL = " SELECT T.id_TipoInsumo, T.nomTipoInsumo  AS [Tipo de Insumo] " + strSelect +
                         " FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V,  " +
                         " ProyectoInsumoPrecio P  " +
                         " WHERE I.codTipoInsumo = T.id_TipoInsumo AND PI.codInsumo = I.id_Insumo  " +
                         " AND V.codProducto = PI.codProducto AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto  " +
                         " AND P.periodo=V.ano AND Unidades <> 0 AND I.codProyecto = " + codProyecto +
                         " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo ";

                //Asignar resultados a la variable DataTable.
                rsTipo = consultas.ObtenerDataTable(txtSQL, "text");

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
                             " AND I.codProyecto = " + codProyecto + " AND codTipoInsumo = " + row_rsTipo.ItemArray[0].ToString() +
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
                    celdaEspecial.Text = "<strong style=\"color:#174680;\">" + row_rsTipo["Tipo de Insumo"].ToString() + "</strong>";
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
                                celdaEspecial.Text = Double.Parse(row_dt.ItemArray[i].ToString()).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
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

                //Añadir al panel la tabla generada y bindear la tabla.
                pnl_tablas_costos.Controls.Add(Tabla_Proyecciones_1);
                Tabla_Proyecciones_1.DataBind();

                /*
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
                */
            }
            catch   (Exception)
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
            Table Tabla_Proyecciones_2 = new Table();
            Tabla_Proyecciones_2.CssClass = "Grilla";
            Tabla_Proyecciones_2.Attributes.Add("width", "95%");
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
            Label lbl = new Label();
            lbl.ID = "lbl_0";
            lbl.Text = "<br/><br/>";
            pnl_tablas_costos.Controls.Add(lbl);
            lbl = null;
            String[] arr_totales = { "&nbsp;", "Total", "IVA", "Total mas IVA" };

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

                //Consultar los tipos de insumos a generar en la tabla.
                txtSQL = " SELECT T.id_TipoInsumo, T.nomTipoInsumo AS [Tipo de Insumo] " + strSelect +
                         " FROM TipoInsumo T, ProyectoInsumo I, ProyectoProductoInsumo PI, ProyectoProductoUnidadesVentas V,  " +
                         " ProyectoInsumoPrecio P  " +
                         " WHERE I.codTipoInsumo = T.id_TipoInsumo AND PI.codInsumo = I.id_Insumo  " +
                         " AND V.codProducto = PI.codProducto AND P.codInsumo = I.id_Insumo AND V.codProducto = PI.codProducto  " +
                         " AND P.periodo=V.ano AND Unidades <> 0 AND I.codProyecto = " + codProyecto +
                         " GROUP BY T.id_TipoInsumo, T.nomTipoInsumo ORDER BY nomTipoInsumo ";

                //Asignar resultados a la variable DataTable.
                rsTipo = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in rsTipo.Rows)
                {
                    #region Generar la fila única de "Tipo de Insumo".

                    fila = new TableRow();
                    celdaEspecial = new TableCell();
                    celdaEspecial.Attributes.Add("colspan", "6");
                    celdaEspecial.Text = "<strong style=\"color:#174680;\">" + row["Tipo de Insumo"].ToString() + "</strong>";
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
                                         " AND Unidades <> 0 AND I.codProyecto = " + codProyecto +
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
                            celdaEspecial.Text = Double.Parse(row_dt["Año " + i.ToString()].ToString()).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
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
                            celdaEspecial.Text = "<strong>" + numTotPesos[j].ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";
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
                            celdaEspecial.Text = "<strong>" + numIVA[j].ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";
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
                            celdaEspecial.Text = "<strong>" + (numIVA[j] + numTotPesos[j]).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</strong>";
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

                #endregion

                //Añadir al panel la tabla generada y bindear la tabla.
                pnl_tablas_costos.Controls.Add(Tabla_Proyecciones_2);
                Tabla_Proyecciones_2.DataBind();

                /*
                #region Destruir variables.
                lbl = null;
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
                */
            }
            catch   (Exception)
            { throw; }

            #region Destruir variables.
            lbl = null;
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

        /// <summary>
        
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {            
            int flag = 0;
            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
            flag = Marcar(txtTab.ToString(), codProyecto, "", chk_realizado.Checked);
            GenerarTablaDeCostosDeProduccion();
            GenerarTablaProyeccionCompras_Unidades();
            GenerarTablaProyeccionCompras_Pesos();
            ObtenerDatosUltimaActualizacion();
            
            if (flag == 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }   
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperCosto";
            GenerarTablaDeCostosDeProduccion();
            GenerarTablaProyeccionCompras_Unidades();
            GenerarTablaProyeccionCompras_Pesos();
            ObtenerDatosUltimaActualizacion();
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperCosto";
            GenerarTablaDeCostosDeProduccion();
            GenerarTablaProyeccionCompras_Unidades();
            GenerarTablaProyeccionCompras_Pesos();
            ObtenerDatosUltimaActualizacion();
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
            Response.Redirect(Request.RawUrl);
        }
    }

    #region Clase "costosdeCompras" completadas.
    //public class costosdeCompras
    //{
    //    private int idInsumo;

    //    public int IdInsumo
    //    {
    //        get { return idInsumo; }
    //        set { idInsumo = value; }
    //    }
    //    private string nomInsumo;

    //    public string NomInsumo
    //    {
    //        get { return nomInsumo; }
    //        set { nomInsumo = value; }
    //    }
    //    private int anio;

    //    public int Anio
    //    {
    //        get { return anio; }
    //        set { anio = value; }
    //    }
    //    private double total;

    //    public double Total
    //    {
    //        get { return total; }
    //        set { total = value; }
    //    }
    //    private double totaliva;

    //    public double TotalIva
    //    {
    //        get { return totaliva; }
    //        set { totaliva = value; }
    //    }
    //} 
    #endregion
}