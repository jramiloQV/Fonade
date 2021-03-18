using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.FONADE.interventoria
{
    public partial class AdicionarInformeVisitaProyecto : Negocio.Base_Page
    {
        #region Variables globales.

        String txtSQL;
        Int32 CodProyecto;
        Int32 CodInforme;
        String txtSQLAux;
        String IdEmpresa;
        DateTime fecha;
        String txtSQLMedio;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los valores de las variables de sesión.
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
            CodInforme = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodInforme"].ToString()) : 0;
            IdEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

            if (!IsPostBack)
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    this.Page.Title = "FONDO EMPRENDER - AGENDA";
                else
                    this.Page.Title = "Ver Planes de Negocio";

                if (CodInforme == 0)
                    btn_IngresarInforme.Visible = true;
                else
                    CargarInformeVisita();
            }
        }

        #region Métodos generales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar los Departamentos.
        /// </summary>
        private void CargarDepartamentos()
        {
            //Inicializar variables.
            ListItem item = new ListItem();
            DataTable depto = new DataTable();

            try
            {
                depto = consultas.ObtenerDataTable("SELECT  id_Departamento, NomDepartamento FROM Departamento", "text");
                SelDpto2.Items.Clear();
                SelDpto1.Items.Clear();

                //Ítem por defecto.
                item = new ListItem();
                item.Value = "";
                item.Text = "Seleccione Departamento";

                //Añadirlos a los DropDownLists de departamentos.
                SelDpto2.Items.Add(item);
                SelDpto1.Items.Add(item);

                foreach (DataRow row in depto.Rows)
                {
                    item = new ListItem();
                    item.Value = row["Id_Departamento"].ToString();
                    item.Text = row["NomDepartamento"].ToString();
                    SelDpto2.Items.Add(item);
                    SelDpto1.Items.Add(item);
                }
                depto = null;
            }
            catch { depto = null; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar las ciudades en "SelMun1" de acuerdo al departamento seleccionado en el DropDownList "SelDpto1".
        /// <param name="p_departamento">Departamento seleccionado.</param>
        /// </summary>
        private void CargarCiudad_SelDpto1(string p_departamento)
        {
            //Inicializar variables.
            ListItem item = new ListItem();
            DataTable DT_ciudad = new DataTable();

            try
            {
                DT_ciudad = consultas.ObtenerDataTable("SELECT Id_Ciudad, NomCiudad FROM Ciudad WHERE CodDepartamento = " + p_departamento, "text");

                SelMun1.Items.Clear();

                foreach (DataRow row in DT_ciudad.Rows)
                {
                    item = new ListItem();
                    item.Value = row["Id_Ciudad"].ToString();
                    item.Text = row["NomCiudad"].ToString();
                    SelMun1.Items.Add(item);
                }

                DT_ciudad = null;
            }
            catch { DT_ciudad = null; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar las ciudades en "SelMun" de acuerdo al departamento seleccionado en el DropDownList "SelDpto2".
        /// <param name="p_departamento">Departamento seleccionado.</param>
        /// </summary>
        private void CargarCiudad_SelDpto2(string p_departamento)
        {
            //Inicializar variables.
            ListItem item = new ListItem();
            DataTable DT_ciudad = new DataTable();

            try
            {
                DT_ciudad = consultas.ObtenerDataTable("SELECT Id_Ciudad, NomCiudad FROM Ciudad WHERE CodDepartamento = " + p_departamento, "text");

                SelMun.Items.Clear();

                foreach (DataRow row in DT_ciudad.Rows)
                {
                    item = new ListItem();
                    item.Value = row["Id_Ciudad"].ToString();
                    item.Text = row["NomCiudad"].ToString();
                    SelMun.Items.Add(item);
                }

                DT_ciudad = null;
            }
            catch { DT_ciudad = null; }
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Cargar el informe de visita seleccioando en "InformeVisitaProyecto.aspx".
        /// </summary>
        private void CargarInformeVisita()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            DataTable RSInf = new DataTable();
            DataTable RSInforme = new DataTable();
            DataTable RSMedio = new DataTable();

            try
            {
                #region Construir consulta SQL.

                txtSQL = " SELECT DISTINCT e.id_empresa, e.razonsocial, e.codproyecto, e.CodCiudad, e.nit" +
                                 " FROM Empresa e, Proyecto p, EmpresaInterventor ei" +
                                 " WHERE e.codproyecto = p.Id_Proyecto" +
                                 " AND e.id_empresa = ei.CodEmpresa";

                if (CodInforme != 0)
                {
                    txtSQLAux = "Select CodEmpresa from informevisitainterventoria where id_informe = " + CodInforme;
                    RSInf = consultas.ObtenerDataTable(txtSQLAux, "text");

                    if (RSInf.Rows.Count > 0)
                    {
                        if (RSInf.Rows[0]["CodEmpresa"].ToString() != "")
                        { txtSQL = txtSQL + " AND e.id_empresa = " + RSInf.Rows[0]["CodEmpresa"].ToString(); }
                    }
                }
                else
                {
                    if (IdEmpresa != "0")
                    { txtSQL = txtSQL + " AND e.id_empresa = " + IdEmpresa; }
                }

                txtSQL = txtSQL + " ORDER BY e.razonsocial";

                #endregion

                //Ejecutar la consulta obtenida de la construcción anterior.
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Cargar el "informe" y el "medio".
                if (CodInforme != 0)
                {
                    txtSQL = "Select * from InformeVisitaInterventoria i, Empresa e where i.CodEmpresa = e.Id_Empresa and id_informe = " + CodInforme;
                    RSInforme = consultas.ObtenerDataTable(txtSQL, "text");

                    txtSQLMedio = " Select Id_MedioDeTransporte, Valor from InformeMedioTransporte, MedioDeTransporte" +
                                  " Where Id_MedioDeTransporte = CodMedioTransporte" +
                                  " and CodInforme = " + CodInforme;
                    RSMedio = consultas.ObtenerDataTable(txtSQLMedio, "text");
                }

                //Cargar los departamentos.
                CargarDepartamentos();

                #region Cargar los campos.

                #region Nombre del Informe.
                if (CodInforme != 0)
                {
                    if (RSInforme.Rows.Count > 0)
                    {
                        NombreInforme.Text = RSInforme.Rows[0]["NombreInforme"].ToString();

                        if (RSInforme.Rows[0]["Estado"].ToString() == "1")
                        { NombreInforme.Enabled = true; }
                    }
                }
                #endregion

                //Empresa:
                if (RS.Rows.Count > 0)
                {
                    Empresa.Value = RS.Rows[0]["NIT"].ToString();
                    NomEmpresa.Text = RS.Rows[0]["razonsocial"].ToString();
                }

                #region Ciudades de origen y destino.

                #region Origen.
                if (CodInforme != 0)
                {
                    if (RSInforme.Rows.Count > 0)
                    {
                        txtSQL = " Select c1.CodDepartamento as dpto_o, c2.CodDepartamento as dpto_d from Ciudad c1, Ciudad c2" +
                                 " Where c1.Id_Ciudad = " + RSInforme.Rows[0]["CodCiudadOrigen"].ToString() +
                                 " and c2.Id_Ciudad = " + RSInforme.Rows[0]["CodCiudadDestino"].ToString();
                        RS = consultas.ObtenerDataTable(txtSQL, "text");
                    }

                    foreach (ListItem item in SelDpto2.Items)
                    {
                        if (item.Value == RS.Rows[0]["dpto_o"].ToString())
                        { item.Selected = true; CargarCiudad_SelDpto2(item.Value); break; }
                    }
                }
                #endregion

                #region Destino.

                if (CodInforme != 0)
                {
                    foreach (ListItem item in SelDpto1.Items)
                    {
                        if (item.Value == RS.Rows[0]["dpto_d"].ToString())
                        { item.Selected = true; CargarCiudad_SelDpto1(item.Value); break; }
                    }
                }

                #endregion

                #endregion

                #region Medios de transporte.

                txtSQL = "select * from MedioDeTransporte";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                int flag = 0;

                if (CodInforme != 0)
                {
                    //Busca los campos agregados "dinámicamente" y les coloca su valor.
                    if (RSMedio.Rows.Count > 0)
                    {
                        try
                        {
                            foreach (DataRow row in RSMedio.Rows)
                            {
                                var valor = this.Page.FindControl("Valor" + row["Id_MedioDeTransporte"].ToString()) as TextBox;
                                valor.Text = decimal.Parse(row["Valor"].ToString()).ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                flag = 1;
                            }
                        }
                        catch { }
                    }

                    if (RSInforme.Rows[0]["Estado"].ToString() == "1")
                    {
                        Valor1.ReadOnly = true;
                        Valor2.ReadOnly = true;
                        Valor3.ReadOnly = true;
                        Valor4.ReadOnly = true;
                    }
                    if (flag == 0)
                    {
                        Valor1.Text = "0";
                        Valor2.Text = "0";
                        Valor3.Text = "0";
                        Valor4.Text = "0";
                    }
                    RSMedio = null;
                }
                else
                {
                    if (flag == 0)
                    {
                        Valor1.Text = "0";
                        Valor2.Text = "0";
                        Valor3.Text = "0";
                        Valor4.Text = "0";
                    }
                }
                #endregion

                //Fecha de salida.
                if (DateTime.TryParse(RSInforme.Rows[0]["FechaSalida"].ToString(), out fecha))
                { FechaSalida.Text = DateTime.Parse(RSInforme.Rows[0]["FechaSalida"].ToString()).ToString("dd/MM/yyyy"); }
                else { FechaSalida.Text = "01/01/2004"; }

                //Fecha de regreso.
                if (DateTime.TryParse(RSInforme.Rows[0]["FechaRegreso"].ToString(), out fecha))
                { FechaRegreso.Text = DateTime.Parse(RSInforme.Rows[0]["FechaRegreso"].ToString()).ToString("dd/MM/yyyy"); }
                else { FechaRegreso.Text = "01/01/2004"; }

                imgPopup.Enabled = false;
                imgRegreso.Enabled = false;

                //Información técnica.
                InformacionTecnica.Text = RSInforme.Rows[0]["InformacionTecnica"].ToString();

                //Información financiera.
                InformacionFinanciera.Text = RSInforme.Rows[0]["InformacionFinanciera"].ToString();

                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + "')", true);
                return;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Según el valor del parámetro, se ejecutarán las acciones correspondientes
        /// al informe.
        /// </summary>
        /// <param name="paramentro">Valor a evaluarpara ejecutar acción.</param>
        /// <returns>string</returns>
        private string EjecutarAccion(string paramentro)
        {
            string msg = "";
            DataTable RS = new DataTable();
            string intEmpresa = "";
            decimal intViatico = 0;
            DateTime dateFechaSalida;
            DateTime dateFechaRegreso;
            decimal intDiferencia = 0;
            decimal intInforme = 0;
            ///Valor del campo de texto de los medios de transporte.
            decimal valorConvertido = 0;
            DataTable RSMedio = new DataTable();
            decimal intValorMedios = 0;

            try
            {
                switch (paramentro)
                {
                    case "Ingresar Informe":
                        #region Crear el informe.

                        //Consulta el Id de la empresa basdo en su razón social.
                        txtSQL = "SELECT Id_Empresa FROM Empresa WHERE nit = '" + Empresa.Value + "'";
                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        //Asigna el Id de la empresa consultada.
                        intEmpresa = RS.Rows[0]["Id_Empresa"].ToString();

                        //Consulta el valor del viático.
                        txtSQL = " SELECT Valor FROM Viatico" +
                                 " WHERE ((SELECT Salario FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + "))" +
                                 " BETWEEN LimiteInferior AND LimiteSuperior and LimiteSuperior != -1)";

                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        if (RS.Rows.Count > 0)
                        { intViatico = decimal.Parse(RS.Rows[0]["Valor"].ToString()); }
                        else
                        {
                            txtSQL = " SELECT Valor FROM Viatico" +
                                     " WHERE ((SELECT Salario FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + "))" +
                                     " >= LimiteInferior and LimiteSuperior = -1)";
                            RS = consultas.ObtenerDataTable(txtSQL, "text");
                            if (RS.Rows.Count > 0) { intViatico = decimal.Parse(RS.Rows[0]["Valor"].ToString()); }
                        }

                        #region Formatear las fechas.

                        //Fecha de salida.
                        if (DateTime.TryParse(FechaSalida.Text, out dateFechaSalida))
                        { dateFechaSalida = DateTime.Parse(FechaSalida.Text); }
                        else { msg = "La fecha de salida seleccionada no es válida."; }

                        //Fecha de regreso.
                        if (DateTime.TryParse(FechaRegreso.Text, out dateFechaRegreso))
                        { dateFechaRegreso = DateTime.Parse(FechaRegreso.Text); }
                        else { msg = "La fecha de regreso seleccionada no es válida."; }

                        #endregion

                        //Si la variable de mensaje NO contiene datos, puede continuar con el registro, de
                        //lo contrario, terminará el flujo, mostrando al final el mensaje.
                        if (msg == "")
                        {
                            #region Inserción.
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            try
                            {
                                
                                SqlCommand cmd = new SqlCommand("MD_CrearInformeVisita", con);

                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@PARAMETRO", "Nuevo");
                                cmd.Parameters.AddWithValue("@NombreInforme", NombreInforme.Text);
                                cmd.Parameters.AddWithValue("@CodCiudadOrigen", SelMun.SelectedValue);
                                cmd.Parameters.AddWithValue("@CodCiudadDestino", SelMun1.SelectedValue);
                                cmd.Parameters.AddWithValue("@FechaSalida", dateFechaSalida);
                                cmd.Parameters.AddWithValue("@FechaRegreso", dateFechaRegreso);
                                cmd.Parameters.AddWithValue("@CodEmpresa", intEmpresa);
                                cmd.Parameters.AddWithValue("@CodInterventor", usuario.IdContacto);
                                cmd.Parameters.AddWithValue("@InformacionTecnica", InformacionTecnica.Text);
                                cmd.Parameters.AddWithValue("@InformacionFinanciera", InformacionFinanciera.Text);
                                cmd.ExecuteNonQuery();
                                //con.Close();
                                //con.Dispose();
                                cmd.Dispose();
                            }
                            catch (Exception ex) { msg = "No se pudo crear el informe de visita. Error: " + ex.Message; }
                            finally {
                                con.Close();
                                con.Dispose();
                            
                            }
                            #endregion

                            //Si hizo bien la inserción, sigue con el flujo.
                            if (msg == "")
                            {
                                txtSQL = " SELECT Id_Informe, CONVERT(int, FechaRegreso - FechaSalida) + 1 AS Diferencia" +
                                         " FROM InformeVisitaInterventoria" +
                                         " WHERE (Id_Informe =" +
                                         " (SELECT MAX(Id_Informe)" +
                                         " FROM InformeVisitaInterventoria))";

                                RS = consultas.ObtenerDataTable(txtSQL, "text");
                                if (RS.Rows.Count > 0)
                                {
                                    intDiferencia = decimal.Parse(RS.Rows[0]["Diferencia"].ToString());
                                    intInforme = decimal.Parse(RS.Rows[0]["Id_Informe"].ToString());
                                }

                                //Siguiente paso...
                                txtSQL = "SELECT Id_MedioDeTransporte, NomMedioDeTransporte FROM MedioDeTransporte";
                                RS = consultas.ObtenerDataTable(txtSQL, "text");
                                intValorMedios = 0;

                                #region Busca los campos agregados "dinámicamente" para obtener su valor e inserta datos.

                                try
                                {
                                    foreach (DataRow row in RS.Rows)
                                    {
                                        var valor = this.Page.FindControl("Valor" + row["Id_MedioDeTransporte"].ToString()) as TextBox;

                                        if (valor.Text != "" || valor.Text != "0")
                                        {
                                            if (decimal.TryParse(valor.Text, out valorConvertido))
                                            { valorConvertido = decimal.Parse(valor.Text); }

                                            //Agregar a la variable de medios.
                                            intValorMedios = intValorMedios + valorConvertido;
                                            txtSQL = " INSERT INTO InformeMedioTransporte(CodInforme, CodMedioTransporte,Valor)" +
                                                     " VALUES(" + intInforme + ", " + row["Id_MedioDeTransporte"].ToString() + ", " + valor.Text + ")";

                                            //Ejecutar inserción.
                                            ejecutaReader(txtSQL, 2);
                                        }
                                    }
                                }
                                catch { msg = "Error al insertar registros en los medios de transporte."; }

                                #endregion

                                //A este punto TIENE que haber procesado la información correctamente.
                                if (msg == "")
                                {
                                    txtSQL = " Update InformeVisitaInterventoria set CostoVisita = " + (intDiferencia * intViatico) + intValorMedios +
                                             " Where (Id_Informe = (SELECT MAX(Id_Informe) FROM InformeVisitaInterventoria))";

                                    //Ejecutar actualización.
                                    ejecutaReader(txtSQL, 2);
                                }
                            }
                        }

                        #endregion
                        break;
                    case "Borrar":
                        #region Borrar (pero NO es claro cómo o quién elimina estos informes).

                        txtSQL = "Delete InformeMedioTransporte where CodInforme = " + CodInforme;
                        ejecutaReader(txtSQL, 2);

                        txtSQL = "Delete InformeVisitaInterventoria where Id_Informe = " + CodInforme;
                        ejecutaReader(txtSQL, 2);

                        #endregion
                        break;
                    case "Modificar":
                        #region Modificar el informe.

                        #region Formatear las fechas.

                        //Fecha de salida.
                        if (DateTime.TryParse(FechaSalida.Text, out dateFechaSalida))
                        { dateFechaSalida = DateTime.Parse(FechaSalida.Text); }
                        else { msg = "La fecha de salida seleccionada no es válida."; }

                        //Fecha de regreso.
                        if (DateTime.TryParse(FechaRegreso.Text, out dateFechaRegreso))
                        { dateFechaRegreso = DateTime.Parse(FechaRegreso.Text); }
                        else { msg = "La fecha de regreso seleccionada no es válida."; }

                        #endregion

                        //Consulta.
                        txtSQL = "SELECT Id_MedioDeTransporte, NomMedioDeTransporte FROM MedioDeTransporte";
                        RS = consultas.ObtenerDataTable(txtSQL, "text");
                        intValorMedios = 0;

                        //Si las fechas fueron formateadas correctamente, puede continuar.
                        if (msg == "")
                        {
                            #region Busca los campos agregados "dinámicamente" para obtener su valor e inserta datos.

                            try
                            {
                                foreach (DataRow row in RS.Rows)
                                {
                                    var valor = this.Page.FindControl("Valor" + row["Id_MedioDeTransporte"].ToString()) as TextBox;

                                    if (valor.Text != "" || valor.Text != "0")
                                    {
                                        if (decimal.TryParse(valor.Text, out valorConvertido))
                                        { valorConvertido = decimal.Parse(valor.Text); }

                                        //Agregar a la variable de medios.
                                        intValorMedios = intValorMedios + valorConvertido;

                                        txtSQL = " Select count(1) as Cuantos from InformeMedioTransporte " +
                                                 " Where CodInforme = " + CodInforme + " and CodMedioTransporte = " + row["Id_MedioDeTransporte"].ToString();
                                        RSMedio = consultas.ObtenerDataTable(txtSQL, "text");

                                        if (Int32.Parse(RSMedio.Rows[0]["Cuantos"].ToString()) > 0)
                                        {
                                            txtSQL = "UPDATE InformeMedioTransporte" +
                                                    " SET Valor = " + valor.Text + " WHERE Codinforme = " + CodInforme +
                                                    " AND CodMedioTransporte = " + row["Id_MedioDeTransporte"].ToString();
                                        }
                                        else
                                        {
                                            txtSQL = " INSERT INTO InformeMedioTransporte(CodInforme, CodMedioTransporte,Valor) " +
                                                     " VALUES(" + CodInforme + ", " + row["Id_MedioDeTransporte"].ToString() + ", " + valor.Text + ")";
                                        }

                                        //Ejecutar inserción.
                                        ejecutaReader(txtSQL, 2);
                                    }
                                    else
                                    {
                                        txtSQL = " Delete InformeMedioTransporte Where CodInforme = " + CodInforme +
                                                 " And CodMedioTransporte = " + row["Id_MedioDeTransporte"].ToString();
                                    }

                                    //Ejecutar la consulta SQL obtenida en las condiciones.
                                    ejecutaReader(txtSQL, 2);
                                }
                            }
                            catch { msg = "Error al insertar registros en los medios de transporte."; }

                            #endregion

                            //Después de procesar los medios de transporte, si la variable de mensaje NO tiene datos, 
                            //podrá continuar.
                            if (msg == "")
                            {
                                #region Actualización.
                                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                try
                                {

                                    SqlCommand cmd = new SqlCommand("MD_CrearInformeVisita", con);

                                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    cmd.Parameters.AddWithValue("@PARAMETRO", "Actualizar");
                                    cmd.Parameters.AddWithValue("@NombreInforme", NombreInforme.Text);
                                    cmd.Parameters.AddWithValue("@CodCiudadOrigen", SelMun.SelectedValue);
                                    cmd.Parameters.AddWithValue("@CodCiudadDestino", SelMun1.SelectedValue);
                                    cmd.Parameters.AddWithValue("@FechaSalida", dateFechaSalida);
                                    cmd.Parameters.AddWithValue("@FechaRegreso", dateFechaRegreso);
                                    cmd.Parameters.AddWithValue("@CostoVisita", intValorMedios);
                                    cmd.Parameters.AddWithValue("@InformacionTecnica", InformacionTecnica.Text);
                                    cmd.Parameters.AddWithValue("@InformacionFinanciera", InformacionFinanciera.Text);
                                    cmd.Parameters.AddWithValue("@Id_Informe", CodInforme);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                    //con.Dispose();
                                    cmd.Dispose();
                                }
                                catch (Exception ex) { msg = "No se pudo actualizar el informe de visita. Error: " + ex.Message; }
                                finally {

                                    con.Close();
                                    con.Dispose();
                                }
                                #endregion
                            }
                        }

                        #endregion
                        break;
                    default:
                        break;
                }

                //Retornar el mensaje.
                return msg;
            }
            catch (Exception ex)
            { msg = "Error: " + ex.Message; return msg; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Validar el formulario antes de ejeuctar la acción en el mismo.
        /// </summary>
        /// <returns>string vació  = puede continuar. // error.</returns>
        private string ValidaFormulario()
        {
            string msg = "";

            try
            {
                switch (msg)
                {
                    case "":
                        break;
                    default:
                        break;
                }

                return msg;
            }
            catch (Exception ex)
            { msg = "Error: " + ex.Message; return msg; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// SelectedIndeChanged para cargar las ciudades del departamento seleccionado en "SelDpto2".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelDpto2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelDpto2.SelectedValue != "") { CargarCiudad_SelDpto2(SelDpto2.SelectedValue); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// SelectedIndeChanged para cargar las ciudades del departamento seleccionado en "SelDpto1".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelDpto1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelDpto1.SelectedValue != "") { CargarCiudad_SelDpto1(SelDpto1.SelectedValue); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2004.
        /// Ingresar informe visita.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_IngresarInforme_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            string msg = "";

            msg = EjecutarAccion(btn_IngresarInforme.Text);

            if (msg != "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + msg + "')", true);
                return;
            }
            else
            {
                //Redirigir al usuario a la página "InformeVisitaProyecto.aspx" (según comportamiento de FONADE clásico).
                Response.Redirect("InformeVisitaProyecto.aspx");
            }
        }
    }
}