using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Xml;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Clases;
//using CAPICOM;

namespace Fonade.FONADE.interventoria
{
    public partial class PagosActividadCoord : Negocio.Base_Page
    {
        SignedXml signedXml = new SignedXml();

        /// <summary>
        /// Contiene el valor de la firma digital...
        /// </summary>
        string firmaDigital;
        string datosFirmate;
        string errorMessageDetail;


        /// <summary>
        /// Contiene el Xml generado con la información a almacenar.
        /// </summary>
        String Datos;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "FONDO EMPRENDER - Aprobacion de Solicitudes de Pago";

            if (!IsPostBack)
            {
                
                if (PreviousPage != null)
                {
                    string myValue = ((interventoria.PagosActividadCoordFiduciaria)PreviousPage).codContactoFiduciariaDDL;
                    HttpContext.Current.Session["CodContatoFiduciaria"] = myValue;
                }
                else
                {
                    Response.Redirect("PagosActividadCoordFiduciaria.aspx");
                }

                CargarInformacionTablaDinamica();
            }

            if (IsPostBack) {
                //Cuando el coordinador de interventoria envia los pagos a fiducia y realiza la firma del Xml con los pagos
                //se hace un postback desde javasript con los datos de la firma y los datos del firmante
                //se capturan los parametros y se ejecuta el evento del botón que procesa los pagos.
                if (Request["__EVENTTARGET"].ToString().Equals("firmaDigital"))
                {
                    string parameter = Request["__EVENTARGUMENT"];
                    if (!String.IsNullOrEmpty(parameter)){
                        string[] firmaYDatosFirmante = parameter.Split(new[] { "[FirmaSplitter]" }, StringSplitOptions.None);

                        if (firmaYDatosFirmante.Length == 2) 
                        {
                            firmaDigital = firmaYDatosFirmante[0];
                            datosFirmate = firmaYDatosFirmante[1];
                            btnverficaCertficado_Click(btnverficaCertficado, null);
                        }                        
                    }   
                }
            }
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

        /// <summary>
        /// Cargar la información de la grilla.
        /// Este método internamente evalúa si hay valores en la variable de sesión.
        /// </summary>
        private void CargarInformacionTablaDinamica()
        {
            String txtSQL = "";            
            DataTable tabla_sql = new DataTable();

            try
            {                
                txtSQL = " SELECT DISTINCT * FROM (SELECT Empresa.Id_Empresa, PagoActividad.Id_PagoActividad, Empresa.razonsocial, UPPER(Contacto.Nombres +' '+Contacto.Apellidos) Agendo, PagoActividad.FechaInterventor AS Fecha, " +
                         " PagoActividad.CantidadDinero AS Valor, PagoBeneficiario.NumIdentificacion, Empresa.codproyecto,PagosActaSolicitudPagos.CodPagoActividad as codactafonade, PagoActividad.ObservaInterventor, " +
                         " (SELECT TOP 1 CodContactoFiduciaria FROM convocatoria, Convocatoriaproyecto, convenio WHERE id_convocatoria = codconvocatoria AND id_convenio = codconvenio AND id_convocatoria IN " +
                         " (SELECT top 1 MAX(CodConvocatoria) AS CodConvocatoria FROM Convocatoriaproyecto WHERE viable = 1 AND Codproyecto = Empresa.codproyecto)  " +
                         " AND Codproyecto = Empresa.codproyecto ) AS codcontactofiduciaria " +
                         " FROM PagoActividad " +
                         " INNER JOIN Empresa ON PagoActividad.CodProyecto = Empresa.codproyecto " +
                         " Inner Join EmpresaInterventor on EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                         " Inner Join Contacto on Contacto.Id_Contacto = (Select top 1 ei.CodContacto from EmpresaInterventor ei Where ei.CodEmpresa = Empresa.id_empresa AND ei.FechaFin IS NULL) " +
                         " INNER JOIN PagoBeneficiario ON PagoActividad.CodPagoBeneficiario = PagoBeneficiario.Id_PagoBeneficiario " +
                         " LEFT JOIN PagosActaSolicitudPagos on PagosActaSolicitudPagos.CodPagoActividad = PagoActividad.Id_PagoActividad " +
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
                                  " WHERE (EmpresaInterventor.Rol in(" + Constantes.CONST_RolInterventorLider + ","+ Constantes.CONST_RolInterventor + ")) " +
                                  " AND (EmpresaInterventor.Inactivo = 0) " +
                                  " AND (Interventor.CodCoordinador = " + usuario.IdContacto + ")" +
                                  " AND (EmpresaInterventor.CodEmpresa = " + tabla_sql.Rows[i]["Id_Empresa"].ToString() + ")";

                    DataTable_TMP = consultas.ObtenerDataTable(mini_txtSQL, "text");

                    if (DataTable_TMP.Rows.Count == 0)
                    { tabla_sql.Rows[i].Delete(); }
                }

                HttpContext.Current.Session["dtEmpresas"] = tabla_sql;
                gvsolicitudes.DataSource = tabla_sql;
                gvsolicitudes.DataBind();
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        /// <summary>
        /// RowCommand para la grilla "gvsolicitudes".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvsolicitudes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "mostrar_coordinadorPago")
            {
                LinkButton lnkBtn = e.CommandSource as LinkButton;
                HttpContext.Current.Session["Id_PagoActividad"] = lnkBtn.Text;
                Redirect(null, "CoodinadorPago.aspx", "_blank",
                    "menubar=0,scrollbars=1,width=380,height=220,top=100");
            }
        }

        /// <summary>
        /// RowDataBound que ejecuta determinadas consultas para establecer valores sobre los controles
        /// internos del GridView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvsolicitudes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnk_Id = e.Row.FindControl("lnk_btn_Id_PagoActividad") as LinkButton;
                var lblFecha = e.Row.FindControl("lbl_fecha") as Label;
                var hdf = e.Row.FindControl("hdf_codactafonade") as HiddenField;
                var rbLst = e.Row.FindControl("rb_lst_aprobado") as RadioButtonList;
                var lbl_Display = e.Row.FindControl("lbl_displayText") as Label;
                var lbl_Inter = e.Row.FindControl("lbl_Intervemtor") as Label;
                var lbl_valor_formateado = e.Row.FindControl("lbl_valor") as Label;
                var hdf_idEmpresa = e.Row.FindControl("hdf_empresa") as HiddenField;
              
                if (lnk_Id != null && lblFecha != null && hdf != null && rbLst != null && lbl_Display != null
                    && lbl_Inter != null && lbl_valor_formateado != null && hdf_idEmpresa != null)
                {
                    
                    try
                    {                      
                        DateTime fecha = Convert.ToDateTime(lblFecha.Text);
                        string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        lblFecha.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                    }
                    catch (Exception ex) { errorMessageDetail = ex.Message; }
                   
                    if (hdf.Value.Trim() == "" || hdf.Value == null)
                    {                       
                        rbLst.SelectedValue = "opcion_Pendiente";
                    }
                    else
                    {                        
                        lbl_Display.Visible = true;
                        lbl_Display.Text = "Este Registro No puede ser enviado A pagos, está incluído en el Acta " + hdf.Value;                       
                        rbLst.Items[0].Enabled = false;
                        rbLst.Items[1].Enabled = false;
                        rbLst.SelectedValue = "opcion_Pendiente";
                    }
                 
                    try
                    {
                        Double valor = Convert.ToDouble(lbl_valor_formateado.Text);
                        lbl_valor_formateado.Text = valor.ToString("C0", CultureInfo.CreateSpecificCulture("es-CO"));
                    }
                    catch (Exception ex) { errorMessageDetail = ex.Message; }
                }
            }
        }

        /// <summary>
        /// Paginación de la grilla "gvsolicitudes".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvsolicitudes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvsolicitudes.PageIndex = e.NewPageIndex;
            CargarInformacionTablaDinamica();
        }

        /// <summary>
        /// Generar XML y "Enviar Datos" que es, generar el archivo plan, el registro en la base de datos
        /// y firmar el documento generado con la certificación digital.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnenviardatos_Click(object sender, EventArgs e)
        {
            //crearXML();
            btnenviardatos.Enabled = false;
            GenerarXml();
        }

        private void crearXML()
        {
            try
            {
                CspParameters cspParams = new CspParameters();
                cspParams.KeyContainerName = "XML_DSIG_RSA_KEY";
                RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider(cspParams);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load("test.xml");
                byte[] hashSignature = rsaKey.SignHash(Convert.FromBase64String("e7jQRU4xmLaQmWVO9pVovhWSeGU="), CryptoConfig.MapNameToOID("SHA1"));
                SignXml(xmlDoc, rsaKey);

                signedXml.SigningKey = rsaKey;
                Console.WriteLine("XML file signed.");
                xmlDoc.Save("test.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void GenerarSoloXml()
        {
            try
            {
                //Inicializar variables.

                String opcion = "No";
                RadioButtonList rbList = new RadioButtonList();
                LinkButton lnk = new LinkButton();
                HiddenField hdf = new HiddenField();
                HiddenField hdf_proyecto = new HiddenField();
                Label lbl = new Label();
                HiddenField hdf_beneficiario = new HiddenField();
                TextBox txtObservaciones = new TextBox();
                String PerfilFonade = usuario.Nombres + " " + usuario.Apellidos;

                Datos = Datos + @"<?xml version=""1.0"" encoding=""windows-1252""?>";
                Datos = Datos + "<Xml_PAGOS>";
      
                var cont = 1;

                for (int k = 0; k < gvsolicitudes.Rows.Count; k++)
                {
                    opcion = "";
                    GridViewRow row = gvsolicitudes.Rows[k];
                    rbList = (RadioButtonList)row.FindControl("rb_lst_aprobado");
                    lnk = (LinkButton)row.FindControl("lnk_btn_Id_PagoActividad");
                    hdf = (HiddenField)row.FindControl("hdf_RazonSocial");
                    hdf_proyecto = (HiddenField)row.FindControl("hdf_codProyecto");
                    lbl = (Label)row.FindControl("lbl_valor");
                    hdf_beneficiario = (HiddenField)row.FindControl("hdf_CodBeneficiario");
                    txtObservaciones = (TextBox)row.FindControl("txt_observ");

                    if (rbList.SelectedItem.Text == "Si" || rbList.SelectedItem.Text == "No")
                    {
                        opcion = rbList.SelectedItem.Text;

                        Datos = Datos + "		 <Xml_Solicitud" + cont + "> ";
                        Datos = Datos + "		    <xml_CodSolicitud> " + lnk.Text + "   </xml_CodSolicitud> ";
                        Datos = Datos + "		    <xml_NomEmpresa> " + hdf.Value + " </xml_NomEmpresa>";
                        Datos = Datos + "		    <xml_CodProyecto>" + hdf_proyecto.Value + " </xml_CodProyecto>";
                        Datos = Datos + "		    <xml_Valor>" + lbl.Text + "</xml_Valor>" + " ";
                        Datos = Datos + "		    <xml_CodBeneficiario> " + hdf_beneficiario.Value + "</xml_CodBeneficiario> ";
                        Datos = Datos + "		    <xml_opcion>" + opcion + "</xml_opcion> ";
                        Datos = Datos + "		    <xml_Observaciones> " + txtObservaciones.Text + " </xml_Observaciones> ";
                        Datos = Datos + "		 </Xml_Solicitud" + cont + "> ";
                        cont++;
                    }
                }

                Datos = Datos + "	<xml_FechaSolicitudes>" + DateTime.Now.ToString("dd/MM/yyyy") + "</xml_FechaSolicitudes>";
                Datos = Datos + "	<xml_NumeroSolicitudes>" + (cont - 1) + "</xml_NumeroSolicitudes>";
                Datos = Datos + "	<xml_UsuarioFonade>" + PerfilFonade + "</xml_UsuarioFonade>";
                Datos = Datos + "</Xml_PAGOS>";

                Session["Datos"] = Datos;

                var ValidaCRL = true;

                if (ValidaCRL)
                {
                   
                    Datos = Convert.ToString(HttpContext.Current.Session["Datos"]);
                    
                    HttpContext.Current.Session["numSolicitudes"] = (cont - 1); 

                    if (Datos != "")
                    {
                        int numSolicitudes = (cont - 1); 
                        var CodActa = string.Empty;
                        var txtmensajeNoInsert = string.Empty;

                        var txtSQL = " INSERT INTO PagosActaSolicitudes (Fecha, NumSolicitudes, Datos, Firma, DatosFirma, CodContacto,Tipo,CodContactoFiduciaria) " +
                                 " VALUES(GETDATE()," + numSolicitudes + ",'" + Datos + "','" + firmaDigital + "','" + datosFirmate + "'," + usuario.IdContacto + ",'Fonade'," + HttpContext.Current.Session["CodContatoFiduciaria"].ToString() + ")";

                        ejecutaReader(txtSQL, 2);
                        
                        var txt = " SELECT MAX(Id_Acta) AS Id_Acta FROM PagosActaSolicitudes WHERE CodContacto = " + usuario.IdContacto + " AND Tipo = 'Fonade'";
                        var dt = consultas.ObtenerDataTable(txt, "text");

                        if (dt.Rows.Count > 0) { CodActa = dt.Rows[0]["Id_Acta"].ToString(); }

                        for (int k = 0; k < gvsolicitudes.Rows.Count; k++)
                        {
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
                                    opcion = "0";

                                txtObservaciones.Text = txtObservaciones.Text.TrimEnd(new char[] { '\r', '\n', '\t' });

                                txtSQL = " select count(*) as cuantos,codpagosActaSolicitudes " +
                                         " from PagosActaSolicitudPagos where codpagoactividad = " + lnk.Text +
                                         " group by codpagosActaSolicitudes ";

                                var rscount = consultas.ObtenerDataTable(txtSQL, "text");

                                if (rscount.Rows.Count > 0)
                                {
                                    txtmensajeNoInsert = txtmensajeNoInsert + " Pago No : " + lnk.Text + " - Acta No: " + rscount.Rows[0]["codpagosActaSolicitudes"].ToString() + "</br>";
                                }
                                else
                                {
                          
                                    txtSQL = " INSERT INTO PagosActaSolicitudPagos (CodPagosActaSolicitudes,CodPagoActividad,Aprobado,Observaciones) " +
                                             " VALUES(" + CodActa + "," + lnk.Text + "," + opcion + ",'" + txtObservaciones.Text + "')";

                                    ejecutaReader(txtSQL, 2);
                              
                                    

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
                                            txtSQL = " UPDATE PagoActividad SET Estado = " + opcion + ", FechaCoordinador = GETDATE()" +
                                                     " WHERE Id_PagoActividad = " + lnk.Text;

                                            ejecutaReader(txtSQL, 2);
                                        }
                                    }
                                }
                            }
                        }

                        //Validar que la solicitud del pago esta en la fiducia correcta.
                        validarEnvioAFiducia(Convert.ToInt32(CodActa));

                        if (txtmensajeNoInsert != "")
                        { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Los siguientes pagos no fueron procesados porque se encontraban registrados en las actas relacionadas <br/>" + txtmensajeNoInsert + "')", true); }

                        if (ValidaCRL)
                        {
                            var numAprobadas = 0;
                            for (int m = 0; m < gvsolicitudes.Rows.Count; m++)
                            {
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

                                if(opcion == "No")
                                {
                                    //Envia Notificacion a Interventor
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

                                    //Envia NOtificacion a Emprendedor
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
                                }
                            }

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Datos se han procesado con éxito.')", true);

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "env_vis()", true);

                            if (numAprobadas > 0)
                            {
                                if (HttpContext.Current.Session["CodContatoFiduciaria"].ToString() != "")
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                        (Int32.Parse(HttpContext.Current.Session["CodContatoFiduciaria"].ToString()),
                                        "Descargar Archivos de Solicitudes de Pago",
                                        "Se han generado nuevas solicitudes de pago, debe descargarlas para procesarlas en el sistema de la Fiduciaria.",
                                        "",
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
                           if (HttpContext.Current.Session["ErrorCapicom"].ToString() != "")
                            {
                          
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + HttpContext.Current.Session["MensajeErrorCapicom"].ToString() + "')", true);
                                Redirect(null, "PagosActividadCoordFiduciaria.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                                return;
                            }
                        }
                    }
                    Redirect(null, "PagosActividadCoordFiduciaria.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error al generar XML de pagos");
            }
        }

        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private void validarEnvioAFiducia(int _idActa)
        {
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = db.SP_ValidarPagoEnviadoAFiducia(_idActa);                               
            }
        }

        public static void SignXml(XmlDocument xmlDoc, RSA Key)
        {
            string errorMessageDetail;
            try
            {
                RSAKeyValue rsakey = new RSAKeyValue();
                if (xmlDoc == null)
                    throw new ArgumentException("xmlDoc");
                if (Key == null)
                    throw new ArgumentException("Key");

                SignedXml signedXml = new SignedXml(xmlDoc);
                Reference reference = new Reference();
                reference.Uri = "";
                XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(env);
                signedXml.AddReference(reference);
                signedXml.ComputeSignature();
                XmlElement xmlDigitalSignature = signedXml.GetXml();
                xmlDoc.DocumentElement.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        /// <summary>
        /// Generar Xml "es un archvo plano que se crea como registro en base de datos 
        /// Contiene TODO el código de las páginas "PagosActivdadCoord.aspx" y "FirmaCoordinador.asp".
        /// </summary>
        private void GenerarXml()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            String opcion = "No";
            RadioButtonList rbList = new RadioButtonList();
            LinkButton lnk = new LinkButton();
            HiddenField hdf = new HiddenField();
            HiddenField hdf_proyecto = new HiddenField();
            Label lbl = new Label();
            HiddenField hdf_beneficiario = new HiddenField();
            TextBox txtObservaciones = new TextBox();
            String PerfilFonade = usuario.Nombres + " " + usuario.Apellidos;
            String Firma = "";
            String txtSQL = "";
            String ResultadoEjecutable = "";
            Boolean ValidaCRL = false;

            SqlCommand cmd = new SqlCommand();
            String CodActa = "";
            DataTable RS = new DataTable();
            DataTable rscount = new DataTable();
            String txtmensajeNoInsert = "";
            Int32 numAprobadas = 0;
            string mensageErrorDetail;

            try
            {
                
                
                ValidaCRL = true; 

                if (ValidaCRL == false)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ResultadoEjecutable + "')", true);
                    Redirect(null, "PagosActividadCoordFiduciaria.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                    return;
                }

                if (ValidaCRL)
                {
                    Datos = Convert.ToString(HttpContext.Current.Session["Datos"]);

                    Firma = Convert.ToString(HttpContext.Current.Session["Firma"]);
                    HttpContext.Current.Session["numSolicitudes"] = gvsolicitudes.Rows.Count.ToString();
                    
                    if (Datos != "")
                    {
                        int numSolicitudes = gvsolicitudes.Rows.Count;

                        txtSQL = " INSERT INTO PagosActaSolicitudes (Fecha, NumSolicitudes, Datos, Firma, CodContacto,Tipo,CodContactoFiduciaria) " +
                                 " VALUES(GETDATE()," + gvsolicitudes.Rows.Count.ToString() + ",'" + Datos + "','" + Firma + "'," + usuario.IdContacto + ",'Fonade'," + HttpContext.Current.Session["CodContatoFiduciaria"].ToString() + ")";

                        try
                        {
                            cmd = new SqlCommand(txtSQL, con);

                            if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                            cmd.CommandType = CommandType.Text;
                            cmd.ExecuteNonQuery();

                            cmd.Dispose();
                        }
                        catch(Exception ex) { mensageErrorDetail = ex.Message; }
                        finally {
                            con.Close();
                            con.Dispose();                        
                        }

                        txtSQL = " SELECT MAX(Id_Acta) AS Id_Acta FROM PagosActaSolicitudes WHERE CodContacto = " + usuario.IdContacto + " AND Tipo = 'Fonade'";
                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        if (RS.Rows.Count > 0) { CodActa = RS.Rows[0]["Id_Acta"].ToString(); }

                        for (int k = 0; k < gvsolicitudes.Rows.Count; k++)
                        {
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

                                txtObservaciones.Text = txtObservaciones.Text.TrimEnd(new char[] { '\r', '\n', '\t' });

                                txtSQL = " select count(*) as cuantos,codpagosActaSolicitudes " +
                                         " from PagosActaSolicitudPagos where codpagoactividad = " + lnk.Text +
                                         " group by codpagosActaSolicitudes ";

                                rscount = consultas.ObtenerDataTable(txtSQL, "text");

                                if (rscount.Rows.Count > 0)
                                {
                                    txtmensajeNoInsert = txtmensajeNoInsert + " Pago No : " + lnk.Text + " - Acta No: " + rscount.Rows[0]["codpagosActaSolicitudes"].ToString() + "</br>";
                                }
                                else
                                {                                    
                                    txtSQL = " INSERT INTO PagosActaSolicitudPagos (CodPagosActaSolicitudes,CodPagoActividad,Aprobado,Observaciones) " +
                                             " VALUES(" + CodActa + "," + lnk.Text + "," + opcion + ",'" + txtObservaciones.Text + "')";

                                    try
                                    {
                                        cmd = new SqlCommand(txtSQL, con);

                                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                        cmd.CommandType = CommandType.Text;
                                        cmd.ExecuteNonQuery();
                                        cmd.Dispose();
                                    }
                                    catch (Exception ex) {  mensageErrorDetail = ex.Message; }
                                    finally {

                                        con.Close();
                                        con.Dispose();
                                    }
                                   
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
                                            
                                            txtSQL = " UPDATE PagoActividad SET Estado = " + opcion + ", FechaCoordinador = GETDATE()" +
                                                     " WHERE Id_PagoActividad = " + lnk.Text;

                                            try
                                            {
                                                 cmd = new SqlCommand(txtSQL, con);

                                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                                cmd.CommandType = CommandType.Text;
                                                cmd.ExecuteNonQuery();
                                                cmd.Dispose();
                                            }
                                            catch (Exception ex){ mensageErrorDetail = ex.Message; }
                                            finally {

                                                con.Close();
                                                con.Dispose();
                                            }                                           
                                        }
                                    }
                                }
                            }                            
                        }

                        if (txtmensajeNoInsert != "")
                        { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Los siguientes pagos no fueron procesados porque se encontraban registrados en las actas relacionadas <br/>" + txtmensajeNoInsert + "')", true); }
                   
                        if (ValidaCRL)
                        {
                            for (int m = 0; m < gvsolicitudes.Rows.Count; m++)
                            {                                
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
                            }

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La Datos se han procesado con éxito.')", true);

                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "env_vis()", true);

                            if (numAprobadas > 0)
                            {
                                if (HttpContext.Current.Session["CodContatoFiduciaria"].ToString() != "")
                                {
                                    AgendarTarea agenda = new AgendarTarea
                                        (Int32.Parse(HttpContext.Current.Session["CodContatoFiduciaria"].ToString()),
                                        "Descargar Archivos de Solicitudes de Pago",
                                        "Se han generado nuevas solicitudes de pago, debe descargarlas para procesarlas en el sistema de la Fiduciaria.",
                                        "",                                       
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
                            if (HttpContext.Current.Session["ErrorCapicom"].ToString() != "")
                            {               
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + HttpContext.Current.Session["MensajeErrorCapicom"].ToString() + "')", true);
                                Redirect(null, "PagosActividadCoordFiduciaria.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                                return;
                            }                          
                        }                        
                    }
                    
                    Redirect(null, "PagosActividadCoordFiduciaria.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                }

                         }
            catch (Exception ex) { mensageErrorDetail = ex.Message; }
        }

        protected void btnverficaCertficado_Click(object sender, EventArgs e)
        {
            GenerarSoloXml();
            if (HttpContext.Current.Session["Datos"] == null)
            {
                Session.Add("Datos", "");
            }
            HttpContext.Current.Session["Datos"] = Datos;
        }

        protected void ObtenerXml_Click(object sender, EventArgs e)
        {
            string xmlParaFirmar = getXmlParaEncriptar();

            Page.ClientScript.RegisterStartupScript(this.GetType(), "FirmarXml", "GenerarDatos_OnClick('" + xmlParaFirmar + "');", true);
        }

        /// <summary>
        /// Metodo para obtener el xml que sera firmado.
        /// </summary>
        /// <returns>Xml a firmar con formato string.</returns>
        protected string getXmlParaEncriptar() 
        {                                                        
            string nombreCoordinadorInterventor = usuario.Nombres + " " + usuario.Apellidos;
            string xmlConPagos = string.Empty;
            xmlConPagos = xmlConPagos + @"<?xml version=""1.0"" encoding=""windows-1252""?>";
            xmlConPagos = xmlConPagos + "<Xml_PAGOS>";

            int contador = 1;
            foreach (GridViewRow actividad in gvsolicitudes.Rows)
            {                                
                RadioButtonList radioButtonList = (RadioButtonList)actividad.FindControl("rb_lst_aprobado");
                LinkButton linkButtonCodigoActividad = (LinkButton)actividad.FindControl("lnk_btn_Id_PagoActividad");
                HiddenField hiddenFieldRazonSocial = (HiddenField)actividad.FindControl("hdf_RazonSocial");
                HiddenField hiddenFieldCodigoProyecto = (HiddenField)actividad.FindControl("hdf_codProyecto");
                Label lblValorPago = (Label)actividad.FindControl("lbl_valor");
                HiddenField hiddenFieldCodigoBeneficiario = (HiddenField)actividad.FindControl("hdf_CodBeneficiario");
                TextBox txtObservacion = (TextBox)actividad.FindControl("txt_observ");
                string opcionSeleccionada = radioButtonList.SelectedItem.Text.ToLower();

                if (opcionSeleccionada == "si" || opcionSeleccionada == "no")
                {                    
                    xmlConPagos = xmlConPagos + "		 <Xml_Solicitud" + contador + "> ";
                    xmlConPagos = xmlConPagos + "		    <xml_CodSolicitud> " + linkButtonCodigoActividad.Text + "   </xml_CodSolicitud> ";
                    xmlConPagos = xmlConPagos + "		    <xml_NomEmpresa> " + hiddenFieldRazonSocial.Value + " </xml_NomEmpresa>";
                    xmlConPagos = xmlConPagos + "		    <xml_CodProyecto>" + hiddenFieldCodigoProyecto.Value + " </xml_CodProyecto>";
                    xmlConPagos = xmlConPagos + "		    <xml_Valor>" + lblValorPago.Text + "</xml_Valor>" + " ";
                    xmlConPagos = xmlConPagos + "		    <xml_CodBeneficiario> " + hiddenFieldCodigoBeneficiario.Value + "</xml_CodBeneficiario> ";
                    xmlConPagos = xmlConPagos + "		    <xml_opcion>" + opcionSeleccionada + "</xml_opcion> ";
                    xmlConPagos = xmlConPagos + "		    <xml_Observaciones> " + txtObservacion.Text + " </xml_Observaciones> ";
                    xmlConPagos = xmlConPagos + "		 </Xml_Solicitud" + contador + "> ";
                }
                contador++;
            }                                                   
            xmlConPagos = xmlConPagos + "	<xml_FechaSolicitudes>" + DateTime.Now.ToString("dd/MM/yyyy") + "</xml_FechaSolicitudes>";
            xmlConPagos = xmlConPagos + "	<xml_NumeroSolicitudes>" + (gvsolicitudes.Rows.Count).ToString() + "</xml_NumeroSolicitudes>";
            xmlConPagos = xmlConPagos + "	<xml_UsuarioFonade>" + nombreCoordinadorInterventor + "</xml_UsuarioFonade>";
            xmlConPagos = xmlConPagos + "</Xml_PAGOS>";

            return xmlConPagos.Replace("'",string.Empty);
        }
    }
}