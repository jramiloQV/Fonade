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
    public partial class CatalogoVentasTMP : Base_Page
    {
        #region Variables globales.
        public String CodProyecto;
        public int txtTab = Constantes.CONST_ProyeccionesVentas;
        public String codConvocatoria;
        String CodUsuario;
        String CodGrupo;
        string pagina;

        /// <summary>
        /// Código del producto (producción) seleccionado.
        /// </summary>
        public string codProduccion;

        String txtNomProyecto;

        String Detalles_CambiosPO_VO;

        String txtSQL;
        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarCombo();
            }
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
                pagina = HttpContext.Current.Session["pagina"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["pagina"].ToString()) ? HttpContext.Current.Session["pagina"].ToString() : "0";
                HttpContext.Current.Session["Accion"] = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : string.Empty;

                #region Consulta para traer el nombre del proyecto
                txtSQL = "select NomProyecto from Proyecto WHERE id_proyecto=" + CodProyecto;
                var rr = consultas.ObtenerDataTable(txtSQL, "text");
                if (rr.Rows.Count > 0) { txtNomProyecto = rr.Rows[0]["NomProyecto"].ToString(); rr = null; }
                #endregion

                #region Variables de sesión creadas en "CambiosPO.aspx".
                //Sesión inicial que indica que la información a procesar proviene de "CambiosPO.aspx".
                Detalles_CambiosPO_VO = HttpContext.Current.Session["Detalles_CambiosPO_VO"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Detalles_CambiosPO_VO"].ToString()) ? HttpContext.Current.Session["Detalles_CambiosPO_VO"].ToString() : "";
                #endregion

                if (Detalles_CambiosPO_VO != "")
                {
                    #region CambiosPO.aspx.
                    //Mostrar campos.
                    lbl_inv_aprobar.Visible = true;
                    dd_inv_aprobar.Visible = true;
                    lbl_inv_obvservaciones.Visible = true;
                    txt_inv_observaciones.Visible = true;

                    //Inhabilitar panel que contiene la tabla dinámica.
                    P_Meses.Enabled = false;

                    //Evaluar la acción a tomar.
                    if (HttpContext.Current.Session["Accion"].ToString().Equals("Adicionar") || HttpContext.Current.Session["Accion"].ToString().Equals("crear"))
                    {
                        B_Acion.Text = "Crear";
                        lbl_enunciado.Text = "Adicionar";
                        TB_Item.Visible = true;
                        L_Item.Visible = true;
                        TB_Item.Visible = true;
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
                    llenarpanel(); //Llenar panel
                    BuscarDatos_Ventas(true); //Buscar datos.
                    #endregion
                }
                else
                {
                    #region Cargar desde "FrameVentasInter.aspx".
                    //Aplicar para producción.
                    llenarpanel();
                    if (HttpContext.Current.Session["Accion"].ToString().Equals("Adicionar") || HttpContext.Current.Session["Accion"].ToString().Equals("crear"))
                    {
                        B_Acion.Text = "Crear";
                        lbl_enunciado.Text = "Adicionar";
                        if (TB_Item.Text == "")
                        {
                            TB_Item.Text = "";
                        }
                        TB_Item.Visible = true;
                        L_Item.Visible = true;
                        TB_Item.Enabled = true;
                    }

                    if (HttpContext.Current.Session["Accion"].ToString().Equals("actualizar") || HttpContext.Current.Session["Accion"].ToString().Equals("Modificar"))
                    {
                        B_Acion.Text = "Actualizar";
                        lbl_enunciado.Text = "MODIFICAR";

                        //TB_Item.Text = "null";
                        //TB_Item.Visible = false;
                        //L_Item.Visible = false;
                        //TB_Item.Enabled = false;
                        TB_Item.Enabled = false;

                        BuscarDatos_Ventas(false);
                    }
                    if (HttpContext.Current.Session["Accion"].ToString().Equals("Borrar") || HttpContext.Current.Session["Accion"].ToString().Equals("borrar"))
                    {
                        B_Acion.Text = "Borrar";
                    }
                    #endregion
                }
            }
            catch (Exception)
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
                    labelFondo.Text = "Ventas";//Cantidad
                    labelAportes.ID = "labelaportes";
                    labelAportes.Text = "Ingreso";//Costo
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
                    textboxFondo.MaxLength = 10;
                    TextBox textboxAporte = new TextBox();
                    textboxAporte.ID = "Aporte" + i;
                    textboxAporte.Width = 50;
                    textboxAporte.TextChanged += new EventHandler(TextBox_TextChanged);
                    textboxAporte.AutoPostBack = true;
                    textboxAporte.MaxLength = 10;
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

        ValidarActividades validarActividades = new ValidarActividades();
        /// <summary>
        /// Guardar y/o actualizar la información.
        /// </summary>
        /// <param name="acion">Si el valor es 1, guardará la información, si es 2, actualizará dicha información.</param>
        private void alamcenar(int acion)
        {
            //Inicializar variables.            
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            DataTable RsActividad = new DataTable();
            String Valor = "";
            DataTable Rs = new DataTable();
            DataTable RsAux = new DataTable();
            String correcto = "";
            String NomActividad = "";

            if (CodProyecto != "0" || codProduccion != "0")
            {
                if (acion == 1)
                {
                    #region Guardar la información del producto en venta.
                    #region Si es Interventor.
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        string txtSQL = "select CodCoordinador from interventor where codcontacto=" + usuario.IdContacto;
                        var dt = consultas.ObtenerDataTable(txtSQL, "text");
                        if (dt.Rows.Count > 0)
                        {
                            correcto = dt.Rows[0].ItemArray[0].ToString();
                        }
                        if (correcto == "")
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                            return;
                        }
                        int ActividadTmp;
                        txtSQL = "select id_Ventas from InterventorVentasTMP ORDER BY id_Ventas DESC";
                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                        if (Rs.Rows.Count == 0)
                        {
                            ActividadTmp = 1;
                        }
                        else
                        {
                            ActividadTmp = Convert.ToInt32(Rs.Rows[0]["id_Ventas"]) + 1;
                        }

                        int cantRegVentas = validarActividades.validarVentas(Convert.ToInt32(CodProyecto), TB_Item.Text);

                        int cantRegVentasTMP = validarActividades.validarVentasTMP(Convert.ToInt32(CodProyecto), TB_Item.Text);

                        if (cantRegVentasTMP > 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una actividad de Venta registrada con la misma informacion.');window.close();", true);
                        }
                        else
                        {

                            txtSQL = "Insert into InterventorVentasTMP (id_Ventas,CodProyecto,NomProducto) values (" + ActividadTmp + "," + CodProyecto + ",'" + TB_Item.Text + "')";
                            ejecutaReader(txtSQL, 2);

                            LogActivitdades.WriteError(4, txtSQL); //4 -> Ventas

                            string mes = "0";
                            string valor = "0";
                            string tipo = "0";

                            #region Cargue de valores de meses 
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

                                            txtSQL = "INSERT INTO InterventorVentasMesTMP(CodProducto,Mes,Valor,Tipo) " +
                                             "VALUES(" + ActividadTmp + "," + mes + "," + valor + "," + tipo + ")";

                                            ejecutaReader(txtSQL, 2);
                                        }
                                    }
                                }
                            }
                            #endregion


                            // Esta consulta trae el Coordinador interventor del proyecto 
                            txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                            var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                            var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());

                            if (dt2.Rows.Count > 0)
                            {
                                //Agendar tarea.
                                AgendarTarea agenda = new AgendarTarea
                                (idCoord,
                                "Producto en ventas enviado a Aprobacion por Interventor",
                                "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + NomActividad + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Ventas");
                                //agenda.Agendar();
                            }

                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                        }
                    }
                    #endregion

                    #region Si es Gerente Interventor.

                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1")//Si
                        {
                            #region Aprobado.

                            #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL.

                            var ventasTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                             where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                             select vt).FirstOrDefault();

                            #endregion

                            int cantRegVentas = validarActividades.validarVentas(ventasTmp.CodProyecto, ventasTmp.NomProducto);

                            if (cantRegVentas > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ya existe una actividad de Venta registrada con la misma informacion.');window.close();", true);
                            }
                            else
                            {

                                #region INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA.
                                // Obtengo  el ultimo  id de la tabla

                                txtSQL = "Insert InterventorVentas Values(" + ventasTmp.CodProyecto + ",'" + ventasTmp.NomProducto + "')";
                                ejecutaReader(txtSQL, 2);

                                LogActivitdades.WriteError(4, txtSQL); //4 -> Ventas

                                txtSQL = "Select Max(id_ventas) from InterventorVentas";
                                var idMax = int.Parse(consultas.ObtenerDataTable(txtSQL, "text").Rows[0].ItemArray[0].ToString());

                                #endregion

                                #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.
                                var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                                where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                                select vt).FirstOrDefault();
                                txtSQL = " DELETE FROM InterventorVentasTMP " +
                                         " where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL POR MESES.
                                var ventasMesTemp = (from vtm in consultas.Db.InterventorVentasMesTMPs
                                                     where vtm.CodProducto == ventasTmp.id_ventas
                                                     select vtm).ToList();

                                #endregion

                                #region INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA POR MESES.
                                foreach (var fila in ventasMesTemp)
                                {
                                    var val = fila.Valor.ToString().Split(',');
                                    if (fila.Tipo == 1)
                                    {
                                        txtSQL = " Insert into InterventorVentasMes (CodProducto, Mes, Valor, Tipo) ";
                                        txtSQL += " values (" + idMax + ", " + fila.Mes + ", " + val[0] + ", 1) ";
                                    }
                                    else
                                    {
                                        txtSQL = " Insert into InterventorVentasMes (CodProducto, Mes, Valor, Tipo) ";
                                        txtSQL += " values (" + idMax + ", " + fila.Mes + ", " + val[0] + ", 2) ";
                                    }

                                    ejecutaReader(txtSQL, 2);
                                }

                                //ELIMINA LOS REGISTROS DE LA TABLA TEMPPORAL POR MESES.
                                txtSQL = "Delete From InterventorVentasMesTMP where CodProducto = " + ventasTmp.id_ventas;
                                ejecutaReader(txtSQL, 2);

                                #endregion

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
                                    "Producto en Ventas Aprobado por Gerente Interventor",
                                    "Revisar Productos en Ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                    "Catálogo Ventas");

                                    agenda.Agendar();
                                }

                                //Destruir variables.
                                CodProyecto = "0";
                                codProduccion = "0";
                                HttpContext.Current.Session["CodProducto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);

                                #endregion
                            }

                        }
                        else
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
                            var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                            where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                            select vt).FirstOrDefault();

                            txtSQL = " DELETE FROM InterventorVentasTMP " +
                                     " where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);


                            #endregion

                            #region Eliminación #2.

                            txtSQL = " DELETE FROM InterventorVentasMesTMP " +
                                     " where CodProducto = " + codProduccion;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            #endregion



                            #region Generar tarea pendiente.

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
                                "Producto en Ventas Rechazado por Gerente Interventor",
                                "Revisar Productos en Ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Ventas");

                                agenda.Agendar();
                            }

                            #endregion

                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);

                            #endregion
                        }
                    }

                    #endregion

                    #region  Si es Cooordinador Interventor
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        #region Aprobado.
                        if (dd_inv_aprobar.SelectedValue == "1")//Si
                        {
                            txtSQL = "UPDATE InterventorVentasTMP SET ChequeoCoordinador = 1 where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;
                            ejecutaReader(txtSQL, 2);

                            var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                            where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                            select vt).FirstOrDefault();

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
                                "Producto en ventas Aprobado por Coordinador Interventor",
                                "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
                                CodProyecto,
                                19,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + CodProyecto,
                                "",
                                "Catálogo Ventas");

                                agenda.Agendar();
                            }
                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                        }
                        #endregion

                        // Pendiente saber bien que se debe hacer cuando el coordinador no aprueba
                        #region No Aprobado.
                        if (dd_inv_aprobar.SelectedValue == "0")//No
                        {
                            var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                            where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                            select vt).FirstOrDefault();

                            #region Se devuelve al interventor.
                            txtSQL = "Delete from InterventorVentasTMP Where CodProyecto = " + ventaTmp.CodProyecto + " and id_ventas = " + ventaTmp.id_ventas;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = "Delete From InterventorVentasMesTMP where CodProducto = " + ventaTmp.id_ventas;
                            ejecutaReader(txtSQL, 2);

                            txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                     " FROM EmpresaInterventor " +
                                     " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                     " WHERE EmpresaInterventor.Inactivo = 0 " +
                                     " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                     " AND Empresa.codproyecto = " + CodProyecto;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion


                            NomActividad = ventaTmp.NomProducto;


                            #region Generar tarea pendiente.

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
                            "Producto en ventas Rechazado para revisión por Coordinador Interventor",
                            "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + NomActividad + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                            "Catálogo Ventas");

                            agenda.Agendar();

                            #endregion

                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                        }
                        #endregion
                    }
                    #endregion

                    #endregion
                }
                if (acion == 2)
                {
                    #region Editar el producto en venta seleccionado.

                    //Comprobar si el usuario tiene el código grupo de "Coordinador Interventor" ó "Gerente Interventor".
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        #region Si es Gerente Interventor.
                        if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            if (dd_inv_aprobar.SelectedValue == "1")//Si
                            {
                                #region Aprobado.

                                #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL.

                                var ventaTMP = (from vt in consultas.Db.InterventorVentasTMPs
                                                where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                                select vt).FirstOrDefault();
                                var venta = (from v in consultas.Db.InterventorVentas
                                             where v.id_ventas == int.Parse(codProduccion) && v.CodProyecto == int.Parse(CodProyecto)
                                             select v).FirstOrDefault();

                                #endregion

                                #region ACTUALIZA LOS REGISTROS EN LA TABLA DEFINITIVA.

                                txtSQL = " Update InterventorVentas set CodProyecto = " + ventaTMP.CodProyecto +
                                         ", NomProducto = '" + ventaTMP.NomProducto + "' WHERE CodProyecto = " + ventaTMP.CodProyecto +
                                         " and id_Ventas = " + ventaTMP.id_ventas;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region BORRAR EL REGISTRO YA ACTUALIZADO DE LA TABLA TEMPORAL.

                                txtSQL = " DELETE FROM InterventorVentasTMP " +
                                         " where CodProyecto = " + ventaTMP.CodProyecto + " and id_Ventas = " + ventaTMP.id_ventas;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL POR MESES.

                                var ventaTmpMes = (from vtm in consultas.Db.InterventorVentasMesTMPs
                                                   where vtm.CodProducto == ventaTMP.id_ventas
                                                   select vtm).ToList();

                                #endregion

                                #region BORRAR TODOS LOS REGISTROS DE LA TABLA InterventorVentasMes CORRESPONDIENTES AL CODIGO DE ACTIVIDAD.

                                txtSQL = " DELETE FROM InterventorVentasMes " +
                                         " where CodProducto = " + venta.id_ventas;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA POR MESES.

                                foreach (var fila in ventaTmpMes)
                                {
                                    var val = fila.Valor.ToString().Split(',');
                                    if (fila.Tipo == 1)
                                    {
                                        txtSQL = " Insert into InterventorVentasMes (CodProducto,Mes,Valor,Tipo) ";
                                        txtSQL += " values (" + venta.id_ventas + ", " + fila.Mes + ", " + val[0] + ", 1) ";
                                    }
                                    else
                                    {
                                        txtSQL = " Insert into InterventorVentasMes (CodProducto,Mes,Valor,Tipo) ";
                                        txtSQL += " values (" + venta.id_ventas + ", " + fila.Mes + ", " + val[0] + ", 2) ";
                                    }
                                    ejecutaReader(txtSQL, 2);
                                }

                                #endregion

                                #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.

                                txtSQL = " DELETE FROM InterventorVentasMesTMP " +
                                         " where CodProducto = " + ventaTMP.id_ventas;

                                //Ejecutar consulta.
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
                                    "Modificación de Producto en Ventas Aprobado por Gerente Interventor",
                                    "Revisar Productos en Ventas " + txtNomProyecto + " - Actividad --> " + ventaTMP.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                    "Catálogo Ventas");

                                    agenda.Agendar();
                                }

                                #endregion

                                //Destruir variables.
                                CodProyecto = "0";
                                codProduccion = "0";
                                HttpContext.Current.Session["CodProducto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);

                                #endregion
                            }
                            if (dd_inv_aprobar.SelectedValue == "0")//No
                            {
                                #region No Aprobado.

                                #region Consulta #1.

                                txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                         " FROM EmpresaInterventor " +
                                         " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                         " WHERE EmpresaInterventor.Inactivo = 0 " +
                                         " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                         " AND Empresa.codproyecto = " + CodProyecto;

                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                                #endregion

                                #region Eliminación #1.

                                var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                                where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                                select vt).FirstOrDefault();

                                txtSQL = " DELETE FROM InterventorVentasTMP " +
                                         " where CodProyecto = " + CodProyecto +
                                         " and id_Ventas = " + codProduccion;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);


                                #endregion

                                // Pendinte saber si al eliminar la tarea pendiente el interventor puede editar 
                                // los valores para que cree nuevamente la tarea corrigiendo la obsevacion registrada
                                // por el Gerente 

                                #region Eliminación #2.

                                txtSQL = " DELETE FROM InterventorVentasMesTMP " +
                                         " where CodProducto = " + codProduccion;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region Generar tarea pendiente.
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
                                    "Modificación de Producto en Ventas NO Aprobado por Gerente Interventor",
                                    "Revisar Productos en Ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                    "Catálogo Ventas");

                                    agenda.Agendar();
                                }

                                #endregion

                                //Destruir variables.
                                CodProyecto = "0";
                                codProduccion = "0";
                                HttpContext.Current.Session["CodProducto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                                #endregion
                            }
                        }
                        #endregion

                        #region Si es Coordinador Interventor.

                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            if (dd_inv_aprobar.SelectedValue == "1") //Si
                            {
                                #region Aprobado.

                                #region Actualización.
                                txtSQL = " UPDATE InterventorVentasTMP SET ChequeoCoordinador = 1 " +
                                                             " where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;
                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region Consulta #1.
                                txtSQL = " select Id_grupo from Grupo where NomGrupo = 'Coordinar de Interventor' ";
                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");
                                #endregion

                                #region Consulta #2.

                                txtSQL = " select CodContacto from GrupoContacto " +
                                         " where CodGrupo = " + RsActividad.Rows[0]["Id_grupo"].ToString();

                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                                var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                                where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                                select vt).FirstOrDefault();

                                #endregion

                                #region Asigna tarea a Gerente Interventor Ventas
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
                                    "Modificación de Producto en ventas Aprobado por Coordinador Interventor",
                                    "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
                                    CodProyecto,
                                    19,
                                    "0",
                                    true,
                                    1,
                                    true,
                                    false,
                                    usuario.IdContacto,
                                    "CodProyecto=" + CodProyecto,
                                    "",
                                    "Catálogo Ventas");

                                    agenda.Agendar();

                                    #endregion

                                    //Destruir variables.
                                    CodProyecto = "0";
                                    codProduccion = "0";
                                    HttpContext.Current.Session["CodProducto"] = null;
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                                    #endregion
                                }
                            }

                            else
                            {
                                #region No Aprobado.

                                #region Consulta #1.

                                txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                         " FROM EmpresaInterventor " +
                                         " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                         " WHERE EmpresaInterventor.Inactivo = 0 " +
                                         " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                         " AND Empresa.codproyecto = " + CodProyecto;

                                RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                                #endregion

                                #region Eliminación #1.

                                var ventaTmp = (from vt in consultas.Db.InterventorVentasTMPs
                                                where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                                select vt).FirstOrDefault();

                                txtSQL = " DELETE FROM InterventorVentasTMP " +
                                         " where CodProyecto = " + CodProyecto +
                                         " and id_Ventas = " + codProduccion;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                // Pendinte saber si al eliminar la tarea pendiente el interventor puede editar 
                                // los valores para que cree nuevamente la tarea corrigiendo la obsevacion registrada por el Coordinador 

                                #region Eliminación #2.

                                txtSQL = " DELETE FROM InterventorVentasMesTMP " +
                                         " where CodProducto = " + codProduccion;

                                //Ejecutar consulta.
                                ejecutaReader(txtSQL, 2);

                                #endregion

                                #region Generar tarea pendiente.

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
                                "Modiciación de Producto en Ventas Rechazado por Coordinador Interventor",
                                "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + NomActividad + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Ventas");

                                agenda.Agendar();

                                #endregion

                                //Destruir variables.
                                CodProyecto = "0";
                                codProduccion = "0";
                                HttpContext.Current.Session["CodProducto"] = null;
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);

                                #endregion
                            }
                        }
                        #endregion
                    }

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        #region Si es Interventor.
                        string txtSQL = "select CodCoordinador from interventor where codcontacto=" + usuario.IdContacto;
                        //cmd = new SqlCommand(txtSQL, conn);
                        correcto = consultas.ObtenerDataTable(txtSQL, "text").Rows[0].ItemArray[0].ToString(); // String_EjecutarSQL(conn, cmd);
                        if (string.IsNullOrEmpty(correcto))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.');window.close();", true);
                            return;
                        }

                        string NomProducto;
                        txtSQL = "select NomProducto from InterventorVentas where CodProyecto=" + CodProyecto + " and id_ventas=" + codProduccion;
                        NomProducto = consultas.ObtenerDataTable(txtSQL, "text").Rows[0].ItemArray[0].ToString();
                        if (string.IsNullOrEmpty(NomProducto))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No pudo ser consultado el nombre del producto');window.close();", true);
                            return;
                        }

                        txtSQL = "Insert into InterventorVentasTMP (id_Ventas,CodProyecto,NomProducto,Tarea) values (" + codProduccion + "," + CodProyecto + ",'" + NomProducto + "','Modificar')";
                        ejecutaReader(txtSQL, 2);
                        string mes = "0";
                        string valor = "0";
                        string tipo = "0";

                        var modificarVenta = (from v in consultas.Db.InterventorVentasTMPs
                                              where v.CodProyecto == int.Parse(CodProyecto) && v.id_ventas == int.Parse(codProduccion)
                                              select v).FirstOrDefault();

                        int CantidadDatos = 0;
                        txtSQL = "SELECT count(*) as Cantidad FROM InterventorVentasMesTMP WHERE CodProducto = " + codProduccion;
                        Rs = consultas.ObtenerDataTable(txtSQL, "text");
                        //if (Rs.Rows.Count > 0) 
                        if (Convert.ToInt32(Rs.Rows[0]["Cantidad"]) >= 1)
                        {
                            CantidadDatos = 1;
                        }

                        #region Grabar datos meses
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
                                            txtSQL = "INSERT INTO InterventorVentasMesTMP(CodProducto,Mes,Valor,Tipo) " +
                                            "VALUES(" + codProduccion + "," + mes + "," + valor + "," + tipo + ")";
                                        }
                                        if (CantidadDatos == 1)
                                        {
                                            // Se actualiza el valor del dato
                                            txtSQL = "UPDATE InterventorVentasMesTMP SET Valor = " + valor +
                                                " WHERE CodProducto = " + codProduccion +
                                                " AND MES = " + mes +
                                                " AND TIPO = " + tipo;
                                        }

                                        ejecutaReader(txtSQL, 2);
                                    }
                                }
                            }
                        }
                        #endregion 

                        // Esta consulta trae el Coordinador interventor del proyecto 
                        txtSQL = "select CodCoordinador  from interventor where codcontacto=" + usuario.IdContacto;
                        var dt2 = consultas.ObtenerDataTable(txtSQL, "text");
                        var idCoord = int.Parse(dt2.Rows[0].ItemArray[0].ToString());

                        if (dt2.Rows.Count > 0)
                        {
                            //Agendar tarea.
                            AgendarTarea agenda = new AgendarTarea
                            (idCoord,
                            "Modificación de Producto en Ventas enviado a Aprobacion por Interventor",
                            "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + modificarVenta.NomProducto + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
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
                            "Catálogo Ventas");
                            //agenda.Agendar();
                        }

                        //Destruir variables.
                        CodProyecto = "0";
                        codProduccion = "0";
                        HttpContext.Current.Session["CodProducto"] = null;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                        #endregion
                    }

                    #endregion
                }
                if (acion == 3) //Eliminar
                {
                    #region Si es Gerente Interventor.
                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        var ventaTmp = (from v in consultas.Db.InterventorVentas
                                        where v.CodProyecto == int.Parse(CodProyecto) && v.id_ventas == int.Parse(codProduccion)
                                        select v).FirstOrDefault();
                        if (dd_inv_aprobar.SelectedValue == "1") //Si
                        {
                            #region Aprobado

                            #region BORRA LOS REGISTROS EN LA TABLA DEFINITIVA.

                            txtSQL = " DELETE FROM InterventorVentas " +
                                     " where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            #endregion

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.


                            txtSQL = " DELETE FROM InterventorVentasTMP " +
                                     " where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);


                            #endregion

                            #region BORRA LOS REGISTROS EN LA TABLA DEFINITIVA POR MESES.

                            txtSQL = " DELETE FROM InterventorVentasMes " +
                                     " where CodProducto = " + codProduccion;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);


                            #endregion

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.

                            txtSQL = " DELETE FROM InterventorVentasMesTMP " +
                                     " where CodProducto = " + codProduccion;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);

                            #endregion

                            //Agendar tarea
                            // Seleccional id del Interventor
                            //mensaje
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
                                "Eliminación de Producto en Ventas Aprobado por Gerente Interventor",
                                "Revisar Productos en Ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Ventas");

                                agenda.Agendar();
                            }

                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);

                            #endregion
                        }
                        if (dd_inv_aprobar.SelectedValue == "0") //No
                        {
                            #region No Aprobado.
                            #region Consulta #1.
                            txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                     " FROM EmpresaInterventor " +
                                     " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                     " WHERE EmpresaInterventor.Inactivo = 0 " +
                                     " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                     " AND Empresa.codproyecto = " + CodProyecto;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.
                            txtSQL = " DELETE FROM InterventorVentasTMP where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;
                            ejecutaReader(txtSQL, 2);
                            #endregion

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.
                            txtSQL = " DELETE FROM InterventorVentasMesTMP where CodProducto = " + codProduccion;
                            cmd = new SqlCommand(txtSQL, conn);
                            correcto = String_EjecutarSQL(conn, cmd);
                            if (correcto != "")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar registros de la tabla temporal por mes: " + correcto + " \n LA CONSULTA ESTA ASÍ: " + txtSQL + "');", true);
                                return;
                            }
                            #endregion

                            #region Informar a Interventor la NO aprobación
                            //Agendar tarea
                            // Seleccional id del Interventor
                            //mensaje
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
                                "Eliminación de Producto en Ventas Rechazado por Gerente Interventor",
                                "Revisar Productos en Ventas " + txtNomProyecto + " - Actividad --> " + ventaTmp.NomProducto + " Observaciones: " + txt_inv_observaciones.Text.Trim(),
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
                                "Catálogo Ventas");

                                agenda.Agendar();
                            }

                            #endregion

                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                            #endregion
                        }
                    }
                    #endregion

                    #region Si es Coordinador Interventor
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        if (dd_inv_aprobar.SelectedValue == "1") //Si
                        {
                            #region Aprobado.
                            var venta = (from vt in consultas.Db.InterventorVentas
                                         where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                         select vt).FirstOrDefault();

                            txtSQL = "UPDATE InterventorVentasTMP SET ChequeoCoordinador = 1 where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;
                            cmd = new SqlCommand(txtSQL, conn);
                            correcto = String_EjecutarSQL(conn, cmd);

                            if (correcto != "")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al aprobar la creación de la venta del producto);", true);
                                return;
                            }

                            txtSQL = " SELECT NomProducto FROM InterventorVentasTMP " +
                                        " WHERE CodProyecto = " + CodProyecto + " and id_Ventas= " + codProduccion;
                            Rs = consultas.ObtenerDataTable(txtSQL, "text");

                            if (Rs.Rows.Count > 0) { NomActividad = Rs.Rows[0]["NomProducto"].ToString(); }

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
                                "Producto en ventas Aprobado para Eliminación por Coordinador Interventor",
                                "Revisar productos en ventas " + txtNomProyecto + " - Actividad --> " + venta.NomProducto + "<br>Observaciones:</br>" + txt_inv_observaciones.Text.Trim(),
                                CodProyecto,
                                19,
                                "0",
                                true,
                                1,
                                true,
                                false,
                                usuario.IdContacto,
                                "CodProyecto=" + CodProyecto,
                                "",
                                "Catálogo Ventas");

                                agenda.Agendar();
                            }
                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
                            #endregion
                        }
                        if (dd_inv_aprobar.SelectedValue == "0") //No
                        {
                            #region No Aprobado.

                            var ventatmp = (from vt in consultas.Db.InterventorVentasTMPs
                                            where vt.CodProyecto == int.Parse(CodProyecto) && vt.id_ventas == int.Parse(codProduccion)
                                            select vt).FirstOrDefault();

                            #region Consulta #1.
                            txtSQL = " SELECT EmpresaInterventor.CodContacto " +
                                     " FROM EmpresaInterventor " +
                                     " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                     " WHERE EmpresaInterventor.Inactivo = 0 " +
                                     " AND EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider +
                                     " AND Empresa.codproyecto = " + CodProyecto;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.
                            txtSQL = " DELETE FROM InterventorVentasTMP where CodProyecto = " + CodProyecto + " and id_Ventas = " + codProduccion;
                            ejecutaReader(txtSQL, 2);
                            #endregion

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL POR MESES.
                            txtSQL = " DELETE FROM InterventorVentasMesTMP where CodProducto = " + codProduccion;
                            ejecutaReader(txtSQL, 2);
                            #endregion

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

                            #region Informar a Interventor la NO aprobación
                            AgendarTarea agenda = new AgendarTarea
                            (datos[0].idContacto,
                            "Producto en Ventas Rechazado para eliminación por Coordinador de Interventoria",
                            "Revisar producto en ventas " + txtNomProyecto + " - Actividad --> " + ventatmp.NomProducto + "<BR><BR>Observaciones:<BR>" + txt_inv_observaciones.Text.Trim(),
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
                            "Catálogo Ventas");

                            agenda.Agendar();

                            #endregion

                            //Destruir variables.
                            CodProyecto = "0";
                            codProduccion = "0";
                            HttpContext.Current.Session["CodProducto"] = null;
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();window.close();", true);
                            #endregion
                        }

                    }
                    #endregion
                }
                else //No es un dato válido.
                {
                    //Destruir variables.
                    CodProyecto = "0";
                    codProduccion = "0";
                    HttpContext.Current.Session["CodProducto"] = null;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location = window.opener.location;window.close();", true);
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
            HttpContext.Current.Session["CodProducto"] = null;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 09/04/2014.
        /// Modificar la información del valor seleccionado en la grilla de "Producción".
        /// </summary>
        /// <param name="VieneDeCambiosPO">TRUE = Viene de "CambiosPO.aspx". // FALSE = Viene de "FrameVentasInter.aspx".</param>
        private void BuscarDatos_Ventas(bool VieneDeCambiosPO)
        {
            //Obtiene la conexión
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());

            //Inicializa la variable para generar la consulta.
            String sqlConsulta = "";
            if (VieneDeCambiosPO == true)
            {
                #region Cargar la información de "CambiosPO.aspx".

                bool ChequeoCoordinador;
                bool ChequeoGerente;

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Los campos del formulario se bloquean.
                    TB_Item.Enabled = false;
                    P_Meses.Enabled = false;
                    #endregion
                    sqlConsulta = "SELECT * FROM InterventorVentasTMP where id_Ventas = " + codProduccion;
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        sqlConsulta = "SELECT * FROM InterventorVentas where id_Produccion =" + codProduccion;
                    }
                }

                var dt = consultas.ObtenerDataTable(sqlConsulta, "text");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        Session["Tarea"] = fila["Tarea"].ToString();
                        //if (fila["ChequeoCoordinador"].ToString() == "1")
                        //{
                        //    ChequeoCoordinador = true;
                        //}
                        //else
                        //{
                        //    ChequeoCoordinador = false;
                        //}

                        //if (fila["ChequeoGerente"].ToString() == "2")
                        //{
                        //    ChequeoGerente = true;
                        //}
                        //else
                        //{
                        //    ChequeoGerente = false;
                        //}

                        //if(ChequeoCoordinador)
                        //{
                        //    dd_inv_aprobar.SelectedValue = "1";
                        //}
                        //else
                        //{
                        //    dd_inv_aprobar.SelectedValue = "0";
                        //}

                        //if(ChequeoGerente)
                        //{
                        //    dd_inv_aprobar.SelectedValue = "1";
                        //}
                        //else
                        //{
                        //    dd_inv_aprobar.SelectedValue = "0";
                        //}
                    }
                    TB_Item.Text = dt.Rows[0].ItemArray[2].ToString();
                }

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    if (HttpContext.Current.Session["Tarea"].ToString() == "Borrar")
                    {
                        sqlConsulta = " select * from InterventorVentasMes where CodProducto = " + codProduccion;
                    }
                    else
                    {
                        sqlConsulta = "select * from InterventorVentasMesTMP where CodProducto = " + codProduccion;
                    }
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        sqlConsulta = " select * from InterventorVentasMes where CodProducto = " + codProduccion + " ORDER BY tipo, mes";
                    }
                }

                dt = consultas.ObtenerDataTable(sqlConsulta, "text");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow fila in dt.Rows)
                    {
                        TextBox controltext;
                        var valor_Obtenido = fila["Tipo"].ToString();
                        if (valor_Obtenido.Equals("1"))
                        {
                            controltext = (TextBox)this.FindControl("Fondoo" + fila["Mes"].ToString());
                        }
                        else
                        {
                            controltext = (TextBox)this.FindControl("Aporte" + fila["Mes"].ToString());
                        }

                        if (controltext != null)
                        {
                            if (String.IsNullOrEmpty(fila["Valor"].ToString()))
                            {
                                controltext.Text = "0";
                            }
                            else
                            {
                                var valor = Double.Parse(fila["Valor"].ToString());
                                controltext.Text = valor.ToString();
                            }

                            sumar(controltext);
                        }
                    }
                }

                #endregion
            }
            else
            {
                #region Viene de "FrameVentasInter.aspx".

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    if (pagina.Equals("Produccion"))
                        sqlConsulta = "select * from InterventorProduccionMesTMP where CodProducto = " + codProduccion;
                    if (pagina.Equals("Ventas"))
                        sqlConsulta = "select * from InterventorVentasMesTMP where CodProducto = " + codProduccion;
                    if (pagina.Equals("Nomina"))
                        sqlConsulta = "select * from InterventorNominaMesTMP where CodCargo = " + codProduccion + " order by mes";
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        if (pagina.Equals("Produccion"))
                            sqlConsulta = "select * from InterventorProduccionMes where CodProducto = " + codProduccion + " ORDER BY tipo, mes";
                        if (pagina.Equals("Ventas"))
                            sqlConsulta = "select * from InterventorVentasMes where CodProducto = " + codProduccion + " ORDER BY tipo, mes";
                        if (pagina.Equals("Nomina"))
                            sqlConsulta = "select * from InterventorNominaMes where CodCargo = " + codProduccion + " order by mes";
                    }
                }

                //SqlCommand cmd = new SqlCommand(sqlConsulta, conn);
                try
                {
                    var ventaTmp = (from vt in consultas.Db.InterventorVentas
                                    where vt.id_ventas == int.Parse(codProduccion)
                                    select vt).FirstOrDefault();
                    TB_Item.Text = ventaTmp.NomProducto;
                    conn.Open();
                    var reader = consultas.ObtenerDataTable(sqlConsulta, "text");
                    if (reader.Rows.Count > 0)
                    {
                        foreach (DataRow fila in reader.Rows)
                        {
                            TextBox controltext;
                            var valor_Obtenido = fila.ItemArray[2].ToString();
                            if (valor_Obtenido.Equals("1"))
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
                    //while (reader.Read())
                    //{
                    //    TextBox controltext;
                    //    string valor_Obtenido = reader["Tipo"].ToString();//.Equals("1");

                    //    if (valor_Obtenido.Equals("1"))
                    //        controltext = (TextBox)this.FindControl("Fondoo" + reader["Mes"].ToString());
                    //    else
                    //        controltext = (TextBox)this.FindControl("Aporte" + reader["Mes"].ToString());

                    //    if (String.IsNullOrEmpty(reader["Valor"].ToString()))
                    //        controltext.Text = "0";
                    //    else
                    //    {
                    //        Double valor = Double.Parse(reader["Valor"].ToString());
                    //        controltext.Text = valor.ToString();
                    //    }

                    //    sumar(controltext);
                    //}
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
                    conn.Dispose();
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