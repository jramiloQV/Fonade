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
//using CAPICOM;
using Fonade.Clases;

namespace Fonade.FONADE.interventoria
{
    public partial class FirmaCoordinador : Negocio.Base_Page //System.Web.UI.Page
    {
        #region Variables globales.

        //Variables de la pantalla "FirmaContrato.aspx".
        String Datos;
        String Firma;
        String numSolicitudes;
        String txtmensaje;
        String txtmensajeNoInsert;
        DateTime fecha = DateTime.Today;
        String txtSQL;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        String opcion = "No";
        RadioButtonList rbList = new RadioButtonList();
        LinkButton lnk = new LinkButton();
        HiddenField hdf = new HiddenField();
        HiddenField hdf_proyecto = new HiddenField();
        Label lbl = new Label();
        HiddenField hdf_beneficiario = new HiddenField();
        TextBox txtObservaciones = new TextBox();

        String CodActa = "";
        DataTable RS = new DataTable();
        DataTable rscount = new DataTable();
        Boolean bolVerifica = false;
        String txtRechazo = "";
        String txtDatosFirma = "";
        String CodRechazoFirmaDigital = "";
        Int32 numAprobadas = 0;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Obtener valores de sesión.
                Datos = HttpContext.Current.Session["Datos"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Datos"].ToString()) ? HttpContext.Current.Session["Datos"].ToString() : "";
                Firma = HttpContext.Current.Session["Firma"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Firma"].ToString()) ? HttpContext.Current.Session["Firma"].ToString() : "";
                numSolicitudes = HttpContext.Current.Session["numSolicitudes"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["numSolicitudes"].ToString()) ? HttpContext.Current.Session["numSolicitudes"].ToString() : "0";

                if (!IsPostBack)
                {
                    #region Ajustar nombre y fecha.

                    #region Nombre.

                    lblNombre.Text = usuario.Nombres + " " + usuario.Apellidos;

                    #endregion

                    #region Fecha.

                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    lblFecha.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;

                    #endregion

                    #endregion

                    if (Datos != "")
                    {
                        CargarInformacionTablaDinamica();

                        //Recibe los datos del formulario que firmo el coordinador de interventoria
                        numSolicitudes = gvsolicitudes.Rows.Count.ToString();

                        #region Se crea el acta.

                        txtSQL = " INSERT INTO PagosActaSolicitudes (Fecha, NumSolicitudes, Datos, Firma, CodContacto,Tipo,CodContactoFiduciaria) " +
                                 " VALUES(GETDATE()," + gvsolicitudes.Rows.Count.ToString() + ",'" + Datos + "','" + Firma + "'," + usuario.IdContacto + ",'Fonade'," + HttpContext.Current.Session["CodContatoFiduciaria"].ToString() + ")";

                        try
                        {
                            //NEW RESULTS:
                            cmd = new SqlCommand(txtSQL, con);

                            if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            //con.Close();
                            //con.Dispose();
                            cmd.Dispose();
                        }
                        catch { }
                        finally {
                            con.Close();
                            con.Dispose();
                        }
                        #endregion

                        #region Se trae el id del acta recien ingresada.

                        txtSQL = " SELECT MAX(Id_Acta) AS Id_Acta FROM PagosActaSolicitudes WHERE CodContacto = " + usuario.IdContacto + " AND Tipo = 'Fonade'";
                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        if (RS.Rows.Count > 0) { CodActa = RS.Rows[0]["Id_Acta"].ToString(); }

                        #endregion

                        for (int k = 0; k < gvsolicitudes.Rows.Count; k++)
                        {
                            #region Recorrer la grilla para hacer otro tipo de inserciones...

                            //Instanciar valores.
                            opcion = "";
                            GridViewRow row = gvsolicitudes.Rows[k];
                            rbList = (RadioButtonList)row.FindControl("rb_lst_aprobado");
                            lnk = (LinkButton)row.FindControl("lnk_btn_Id_PagoActividad");
                            hdf = (HiddenField)row.FindControl("hdf_RazonSocial");
                            hdf_proyecto = (HiddenField)row.FindControl("hdf_codProyecto");
                            lbl = (Label)row.FindControl("lbl_valor");
                            hdf_beneficiario = (HiddenField)row.FindControl("hdf_CodBeneficiario");
                            txtObservaciones = (TextBox)row.FindControl("txt_observ");

                            if (rbList.SelectedItem.Text != "Pendiente")
                            {
                                opcion = "0";

                                if (rbList.SelectedItem.Text == "Si")
                                    opcion = "1";
                                else
                                    opcion = "2";

                                //Se limpia la cadena, se elimina el salto de linea, el retorno de carro y el tabulador.
                                txtObservaciones.Text = txtObservaciones.Text.TrimEnd(new char[] { '\r', '\n', '\t' });

                                //Valido si el Pago Existe en algun acta.
                                txtSQL = " select count(*) as cuantos,codpagosActaSolicitudes " +
                                         " from PagosActaSolicitudPagos where codpagoactividad = " + lnk.Text +
                                         " group by codpagosActaSolicitudes ";

                                rscount = consultas.ObtenerDataTable(txtSQL, "text");

                                if (rscount.Rows.Count > 0)
                                {
                                    txtmensajeNoInsert = txtmensajeNoInsert + " Pago No : " + lnk.Text + " - Acta No: " + rscount.Rows[0]["codpagosActaSolicitudes"].ToString() + "</br>";
                                    ltMensaje.Text = txtmensajeNoInsert;
                                }
                                else
                                {
                                    #region Inserción.

                                    txtSQL = " INSERT INTO PagosActaSolicitudPagos (CodPagosActaSolicitudes,CodPagoActividad,Aprobado,Observaciones) " +
                                             " VALUES(" + CodActa + "," + lnk.Text + "," + opcion + ",'" + txtObservaciones.Text + "')";

                                    try
                                    {
                                        //NEW RESULTS:
                                        cmd = new SqlCommand(txtSQL, con);

                                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                        //con.Close();
                                        //con.Dispose();
                                        cmd.Dispose();
                                    }
                                    catch { }
                                    finally {
                                        con.Close();
                                        con.Dispose();
                                    }
                                    #endregion
                                    ltMensaje.Text = "";
                                    if (rbList.SelectedItem.Text == "Pendiente")
                                    {
                                        opcion = Constantes.CONST_EstadoCoordinador.ToString();
                                    }
                                    else
                                    {
                                        if (rbList.SelectedItem.Text == "Si")
                                        { opcion = Constantes.CONST_EstadoFiduciaria.ToString(); }
                                        else
                                        { opcion = Constantes.CONST_EstadoRechazadoFA.ToString(); }

                                        if (rbList.SelectedItem.Text != "Pendiente")
                                        {
                                            #region Actualización.

                                            txtSQL = " UPDATE PagoActividad SET Estado = " + opcion + ", FechaCoordinador = GETDATE()" +
                                                     " WHERE Id_PagoActividad = " + lnk.Text;

                                            try
                                            {
                                                //NEW RESULTS:
                                                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                                cmd = new SqlCommand(txtSQL, con);

                                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                                cmd.CommandType = CommandType.Text;
                                                cmd.ExecuteNonQuery();
                                                //con.Close();
                                                //con.Dispose();
                                                cmd.Dispose();
                                            }
                                            catch { }
                                            finally {

                                                con.Close();
                                                con.Dispose();
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }

                            #endregion
                        }

                        if (txtmensajeNoInsert != "")
                        { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Los siguientes pagos no fueron procesados porque se encontraban registrados en las actas relacionadas <br/>" + txtmensajeNoInsert + "')", true); }

                        //PART TWO...
                        bolVerifica = false;
                        txtRechazo = "";

                        //Variable de session para capturar el error que pueda detectar CAPICOM.
                        HttpContext.Current.Session["ErrorCapicom"] = "";
                        HttpContext.Current.Session["MensajeErrorCapicom"] = "";

                        #region Verificar que los datos que fueron firmados en el cliente son los mismos datos que se han recibido en el servidor y no han sido adulterados.

                        if (VerifySign(Datos, Firma))
                        {
                            //Verificar que el certificado digital empleado para el proceso de firma no se encuentra en la Lista de Certificados Revocados (CRL) y, por lo tanto, es un certificado valido.
                            if (ValidateRoot(Datos, Firma))
                            {
                                //Verificar que el certificado digital empleado para el proceso de firma se encuentra dentro de su período de vigencia
                                if (Validatetime(Datos, Firma))
                                {
                                    if (ValidaFirmantes(Datos, Firma)) { bolVerifica = true; }
                                }
                                else
                                {
                                    HttpContext.Current.Session["ErrorCapicom"] = "Time";
                                    HttpContext.Current.Session["MensajeErrorCapicom"] = "El Certificado utilizado no esta vigente, por lo tanto no es válido.";
                                }
                            }
                            else
                            {
                                HttpContext.Current.Session["ErrorCapicom"] = "Root";
                                HttpContext.Current.Session["MensajeErrorCapicom"] = "El Certificado utilizado no fue emitido por una entidad certificadora, por lo tanto no es válido.";
                            }
                        }

                        #endregion

                        //Extraer la información de identificación del firmante, se actualiza el acta con estos datos
                        txtDatosFirma = "";
                        //f3l
                        /*
                        //Inicializar el objeto Capicom que gestiona las firmas digitales
                        SignedData Verifier = new SignedData();
                        Verifier.Content = Datos;

                        //Verificar la firma digital
                        Verifier.Verify(Firma, true, 0);

                        foreach (Certificate Certificate in Verifier.Certificates)
                        { txtDatosFirma = txtDatosFirma + Certificate.SubjectName; }

                        #region Actualización.

                        txtSQL = "UPDATE PagosActaSolicitudes SET DatosFirma = '" + txtDatosFirma + "' WHERE Id_Acta = " + CodActa;

                        try
                        {
                            //NEW RESULTS:
                            cmd = new SqlCommand(txtSQL, con);

                            if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();
                            con.Close();
                            con.Dispose();
                            cmd.Dispose();
                        }
                        catch { }

                        #endregion

                        #region Si todas las verificaciones OK, se hacen los cambios de estado de los pagos (se van a fiduciaria o se devuelven a Interventor).

                        if (bolVerifica)
                        {
                            for (int m = 0; m < gvsolicitudes.Rows.Count; m++)
                            {
                                //Instanciar valores.
                                opcion = "";
                                GridViewRow row = gvsolicitudes.Rows[m];
                                rbList = (RadioButtonList)row.FindControl("rb_lst_aprobado");
                                lnk = (LinkButton)row.FindControl("lnk_btn_Id_PagoActividad");
                                hdf = (HiddenField)row.FindControl("hdf_RazonSocial");
                                hdf_proyecto = (HiddenField)row.FindControl("hdf_codProyecto");
                                lbl = (Label)row.FindControl("lbl_valor");
                                hdf_beneficiario = (HiddenField)row.FindControl("hdf_CodBeneficiario");
                                txtObservaciones = (TextBox)row.FindControl("txt_observ");

                                if (rbList.SelectedItem.Text == "Pendiente")
                                { opcion = Constantes.CONST_EstadoCoordinador.ToString(); }
                                else if (rbList.SelectedItem.Text == "Si")
                                {
                                    opcion = Constantes.CONST_EstadoFiduciaria.ToString();
                                    numAprobadas = numAprobadas + 1;
                                }
                                else if (rbList.SelectedItem.Text == "No")
                                {
                                    opcion = Constantes.CONST_EstadoInterventor.ToString();
                                }

                                #region Mensaje 1.

                                //Oct 19 2005, Alejandro Garzon R.
                                //Inicio modificacion
                                //Solicitud rechazada, se genera tarea avisandole al interventor y al emprendedor
                                //Interventor
                                txtSQL = "SELECT EmpresaInterventor.CodContacto, Empresa.CodProyecto " +
                                                 " FROM EmpresaInterventor " +
                                                 " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                                                 " INNER JOIN PagoActividad ON Empresa.codproyecto = PagoActividad.CodProyecto " +
                                                 " WHERE (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                                 " AND (PagoActividad.Id_PagoActividad = " + lnk.Text + ")";

                                var rsInterventores = consultas.ObtenerDataTable(txtSQL, "text");

                                foreach (DataRow row_rsInterventores in rsInterventores.Rows)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                        (Int32.Parse(row_rsInterventores["CodContacto"].ToString()),
                                        "Solicitud de Pago No. " + lnk.Text + " Rechazada por Coordinador de Interventoria",
                                        "Se ha rechazado la Solicitud de pago No " + lnk.Text + ". <BR><BR>Observaciones Coordinador: " + txtObservaciones.Text,
                                        row_rsInterventores["CodProyecto"].ToString(),
                                        2,
                                        "0",
                                        true,
                                        1,
                                        true,
                                        false,
                                        usuario.IdContacto,
                                        null,
                                        null,
                                        "Firma Coordinador");

                                    agenda.Agendar();
                                }

                                rsInterventores = null;

                                #endregion

                                #region Mensaje 2.

                                //Emprendedores
                                txtSQL = " SELECT ProyectoContacto.CodContacto, ProyectoContacto.CodProyecto " +
                                         " FROM PagoActividad " +
                                         " INNER JOIN ProyectoContacto ON PagoActividad.CodProyecto = ProyectoContacto.CodProyecto " +
                                         " WHERE (PagoActividad.Id_PagoActividad = " + lnk.Text + ") " +
                                         " AND (ProyectoContacto.Inactivo = 0) " +
                                         " AND (dbo.ProyectoContacto.CodRol = " + Constantes.CONST_RolEmprendedor + ")";

                                var rsEmprendedores = consultas.ObtenerDataTable(txtSQL, "text");

                                foreach (DataRow row_rsEmprendedores in rsEmprendedores.Rows)
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                        (Int32.Parse(row_rsEmprendedores["CodContacto"].ToString()),
                                        "Solicitud de Pago No. " + lnk.Text + " Rechazada por Coordinador de Interventoria",
                                        "Se ha rechazado la Solicitud de pago No " + lnk.Text + ". </br></br>Observaciones Coordinador: " + txtObservaciones.Text,
                                        row_rsEmprendedores["CodProyecto"].ToString(),
                                        2,
                                        "0",
                                        true,
                                        1,
                                        true,
                                        false,
                                        usuario.IdContacto,
                                        null,
                                        null,
                                        "Firma Coordinador");

                                    agenda.Agendar();
                                }

                                rsEmprendedores = null;
                                //Fin modificacion, Oct 19 2005

                                #endregion
                            }

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Datos se han procesado con éxito.')", true);

                            //Se genera actividad para la fiduciaria, se genera una tarea avisando por email.
                            if (numAprobadas > 0)
                            {
                                if (HttpContext.Current.Session["CodContatoFiduciaria"].ToString() != "")
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                        (Int32.Parse(HttpContext.Current.Session["CodContatoFiduciaria"].ToString()),
                                        "Descargar Archivos de Solicitudes de Pago",
                                        "Se han generado nuevas solicitudes de pago, debe descargarlas para procesarlas en el sistema de la Fiduciaria.",
                                        "",
                                        //row_rsInterventores["CodProyecto"].ToString(),
                                        2,
                                        "0",
                                        true,
                                        1,
                                        true,
                                        false,
                                        usuario.IdContacto,
                                        null,
                                        null,
                                        "Firma Coordinador");

                                    agenda.Agendar();
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            #region Se genero un error en una de las validaciones, se captura el error,

                            if (HttpContext.Current.Session["ErrorCapicom"].ToString() != "")
                            {
                                CodRechazoFirmaDigital = TraerCodRechazoFirmaDigital(HttpContext.Current.Session["ErrorCapicom"].ToString()).ToString();

                                txtSQL = " UPDATE PagosActaSolicitudes SET CodRechazoFirmaDigital = " + CodRechazoFirmaDigital + " WHERE Id_Acta = " + CodActa;

                                try
                                {
                                    //NEW RESULTS:
                                    cmd = new SqlCommand(txtSQL, con);

                                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                    cmd.CommandType = CommandType.Text;
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    con.Dispose();
                                    cmd.Dispose();
                                }
                                catch { }

                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + HttpContext.Current.Session["MensajeErrorCapicom"].ToString() + "')", true);
                                return;
                            }

                            #endregion
                        }

                        #endregion

                        div_clear.Attributes.Add("display", "none");
                        */
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 13/05/2014.
        /// Cargar la información de la grilla.
        /// Este método internamente evalúa si hay valores en la variable de sesión.
        /// </summary>
        private void CargarInformacionTablaDinamica()
        {
            //Inicializar variables.
            String txtSQL = "";
            String txtSQL1 = "";
            DataTable tabla_sql = new DataTable();

            try
            {
                txtSQL1 = " SELECT DISTINCT PagoActividad.Id_PagoActividad AS tablaId_PagoActividad, CodActaFonade " +
                          " FROM PagoActividad " +
                          " INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                          " INNER JOIN PagoBeneficiario ON PagoActividad.CodPagoBeneficiario = PagoBeneficiario.Id_PagoBeneficiario " +
                          " INNER JOIN PagosActaSolicitudPagos ON (PagoActividad.id_PagoActividad = PagosActaSolicitudPagos.codPagoActividad) " +
                          " INNER JOIN PagosActaSolicitudes ON (PagosActaSolicitudPagos.codPagosActaSolicitudes = PagosActaSolicitudes.id_acta AND PagosActaSolicitudes.tipo = 'Fiduciaria') " +
                          " WHERE PagoActividad.Estado = " + Constantes.CONST_EstadoCoordinador;

                txtSQL = " SELECT * FROM (SELECT Empresa.Id_Empresa, PagoActividad.Id_PagoActividad, Empresa.razonsocial, PagoActividad.FechaInterventor AS Fecha, " +
                         " PagoActividad.CantidadDinero AS Valor, PagoBeneficiario.NumIdentificacion, Empresa.codproyecto,codactafonade, PagoActividad.ObservaInterventor, " +
                         " (SELECT TOP 1 CodContactoFiduciaria FROM convocatoria, Convocatoriaproyecto, convenio WHERE id_convocatoria = codconvocatoria AND id_convenio = codconvenio AND id_convocatoria IN " +
                         " (SELECT MAX(CodConvocatoria) AS CodConvocatoria FROM Convocatoriaproyecto WHERE viable = 1 AND Codproyecto = Empresa.codproyecto)  " +
                         " AND Codproyecto = Empresa.codproyecto ) AS codcontactofiduciaria " +
                         " FROM PagoActividad " +
                         " INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                         " INNER JOIN PagoBeneficiario ON PagoActividad.CodPagoBeneficiario = PagoBeneficiario.Id_PagoBeneficiario " +
                         " LEFT JOIN (" + txtSQL1 + ") AS tabla ON (tablaId_PagoActividad = id_PagoActividad) " +
                         " WHERE PagoActividad.Estado = " + Constantes.CONST_EstadoCoordinador +
                         " ) AS tabla ";

                if (!String.IsNullOrEmpty(HttpContext.Current.Session["CodContatoFiduciaria"].ToString()))
                {
                    txtSQL = txtSQL + " where codcontactofiduciaria = " + HttpContext.Current.Session["CodContatoFiduciaria"].ToString();
                }
                //txtSQL = txtSQL + " ORDER BY fecha, Id_PagoActividad	 ";
                // Para mostrar la información en el orden establecido en FONADE clásico.
                txtSQL = txtSQL + " ORDER BY fecha ASC	 ";

                //Asignar resultados de la consulta anterior a variable DataTable.
                tabla_sql = consultas.ObtenerDataTable(txtSQL, "text");

                #region Recorrer listado para eliminar valores que NO corresponden a la grilla (ver notas internas).

                ///Según FONADE clásico, si la consulta que sigue a continuación NO contiene datos, NO debe mostrar
                ///los resultados de la consulta principal, en resumen, sólo se mostrará la información que debe mostrar
                ///en el GridView.

                for (int i = 0; i < tabla_sql.Rows.Count; i++)
                {
                    String mini_txtSQL = "";
                    DataTable DataTable_TMP = new DataTable();
                    mini_txtSQL = " SELECT DISTINCT Contacto.Nombres + ' ' + Contacto.Apellidos AS Intervemtor " +
                                  " FROM Contacto  INNER JOIN EmpresaInterventor " +
                                  " ON Contacto.Id_Contacto = EmpresaInterventor.CodContacto " +
                                  " INNER JOIN Interventor ON EmpresaInterventor.CodContacto = Interventor.CodContacto " +
                                  " WHERE (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                                  " AND (EmpresaInterventor.Inactivo = 0) " +
                                  " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                  " AND (EmpresaInterventor.CodEmpresa = " + tabla_sql.Rows[i]["Id_Empresa"].ToString() + ")";

                    //Asignar valores a DataTable_TMP;
                    DataTable_TMP = consultas.ObtenerDataTable(mini_txtSQL, "text");

                    //Si la consulta NO trajo datos, debe eliminar la información correspondiente en el DataTable de la 
                    //consulta principal.
                    if (DataTable_TMP.Rows.Count == 0)
                    { tabla_sql.Rows[i].Delete(); }
                }

                #endregion

                //Bindear datos.
                HttpContext.Current.Session["dtEmpresas"] = tabla_sql;
                gvsolicitudes.DataSource = tabla_sql;
                gvsolicitudes.DataBind();
            }
            catch { string err = ""; }
        }

        protected void gvsolicitudes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvsolicitudes_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }


        protected void gvsolicitudes_RowDataBound(Object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvsolicitudes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

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
    }
}

