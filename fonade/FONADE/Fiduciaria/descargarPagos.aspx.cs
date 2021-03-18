using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Datos;
using System.IO;
using Ionic.Zip;
using System.Globalization;

namespace Fonade.FONADE.Fiduciaria
{
    /// <summary>
    /// Description : Formulario para descargar los archivos de pago de fiduciaria con aprobación de token y generación de archivos de Terceros, Pagos y adjuntos en zip.
    ///               Se crearon clases que representan la estructura de datos de  las solicitudes de pago, los terceros que son beneficiarios y los pagos. 
    /// Author : marztres@gmail.com
    /// Date : 04/11/2015
    /// </summary>
    public partial class descargarPagos : Negocio.Base_Page
    {
        protected Int64? codigoActa;
        protected DateTime? fechaActa;
        protected string directorioWebArchivosFiduciaria;
        protected string nombreArchivoTerceros;
        protected string nombreArchivoPagos;
        /// <summary>
        /// Datos firmados por token
        /// </summary>
        protected string firmaDigital;
        /// <summary>
        /// Datos del firmante extraidos del token
        /// </summary>
        protected string datosFirmate;
        /// <summary>
        /// Xml de pagos para firmar
        /// </summary>
        protected string xmlParaFirmar;
        /// <summary>
        /// Fecha desde que se inicia la migración, la ruta de los archivos cambia desde esa fecha.
        /// </summary>
        protected DateTime fechaInicioMigracion = new DateTime(2016, 07, 18);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var codigoActaString = FieldValidate.GetSessionString("CodActaFonade");

                //string codigoActaString = "";
                //if (PreviousPage != null)
                //{
                //    string myValue = ((SolicitudesDePagoSinDescargar)PreviousPage).CodigoActaPago;
                //    codigoActaString = myValue;
                //}

                ///Si no existe la variable de sessión CodActaFonade se
                ///se genera error
                if (codigoActaString.Equals(null) || codigoActaString.Equals(String.Empty))
                {
                    throw new ApplicationException("No fue posible obtener el codigo del acta.");
                }

                codigoActa = Convert.ToInt64(codigoActaString);
                lblNumeroDeSolicitud.Text = "Numero de solicitud : " + codigoActaString;

                List<SolicitudDePago> solicitudesDePago = getSolicitudesDePago(codigoActa);

                directorioWebArchivosFiduciaria = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + "Fiduciaria/" + codigoActa.ToString() + "/";

                if (solicitudesDePago.Any())
                {
                    //Si la fecha del pago es menor a la fecha de la migración la ruta es distinta en su estructura.
                    if (solicitudesDePago.First().fechaCoordinador <= fechaInicioMigracion)
                        directorioWebArchivosFiduciaria = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + "Fiduciaria/";
                }

                cargarGridConSolicitudes(solicitudesDePago);
                verificarEstadoActa(codigoActa, solicitudesDePago);



                if (IsPostBack)
                {
                    //Cuando el personal de fiducia descarga los pagos y realiza la firma del Xml con los pagos
                    //se hace un postback desde javasript con los datos de la firma y los datos del firmante
                    //se capturan los parametros y se ejecuta el evento del botón que procesa los pagos.
                    if (Request["__EVENTTARGET"].ToString().Equals("firmaDigital"))
                    {
                        string parameter = Request["__EVENTARGUMENT"];
                        if (!String.IsNullOrEmpty(parameter))
                        {
                            string[] firmaYDatosFirmante = parameter.Split(new[] { "[FirmaSplitter]" }, StringSplitOptions.None);
                            if (firmaYDatosFirmante.Length == 2)
                            {
                                firmaDigital = firmaYDatosFirmante[0];
                                datosFirmate = firmaYDatosFirmante[1];
                                btnDescargarPagos_Click(btnDescargarPagos, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Cargar grid con solicitudes de pago.
        /// </summary>
        /// <param name="solicitudesDePago"></param>
        protected void cargarGridConSolicitudes(List<SolicitudDePago> solicitudesDePago)
        {
            gvSolicitudesDePago.DataSource = solicitudesDePago;
            gvSolicitudesDePago.DataBind();
        }

        /// <summary>
        /// Obtiene las solicitudes de pago.
        /// </summary>
        /// <param name="codigoActa"> Codigo de acta </param>
        protected List<SolicitudDePago> getSolicitudesDePago(Int64? codigoActa)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener las solicitudes de pago
                var solicitudesDePago = (from pagosActaSolicitudesPagos in db.PagosActaSolicitudPagos
                                         join pagosActaSolicitudes in db.PagosActaSolicitudes on pagosActaSolicitudesPagos.CodPagosActaSolicitudes equals pagosActaSolicitudes.Id_Acta
                                         join pagoActividad in db.PagoActividad on pagosActaSolicitudesPagos.CodPagoActividad equals pagoActividad.Id_PagoActividad
                                         join empresa in db.Empresas on pagoActividad.CodProyecto equals empresa.codproyecto
                                         where pagosActaSolicitudesPagos.CodPagosActaSolicitudes == codigoActa && pagosActaSolicitudesPagos.Aprobado == true
                                         select new SolicitudDePago
                                         (
                                           pagoActividad.Id_PagoActividad,
                                           pagoActividad.FechaCoordinador,
                                           empresa.razonsocial,
                                           pagoActividad.CantidadDinero,
                                           pagosActaSolicitudes.ArchivoPagosFA,
                                           pagosActaSolicitudes.ArchivoTercerosFA,
                                           pagoActividad.RutaArchivoZIP
                                         )).ToList();
                return solicitudesDePago;
            }
        }

        /// <summary>
        /// Descargar archivos planos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDescargarPagos_Click(object sender, EventArgs e)
        {
            try
            {
                List<SolicitudDePago> solicitudesDePago = getSolicitudesDePago(codigoActa);

                nombreArchivoTerceros = generarArchivoPlanoTerceros(codigoActa);
                nombreArchivoPagos = generarArchivoPlanoPagos(solicitudesDePago);
                actualizarPagoDescargado(codigoActa, nombreArchivoTerceros, nombreArchivoPagos, solicitudesDePago);
                pnlVerificarToken.Visible = false;

                solicitudesDePago = getSolicitudesDePago(codigoActa);
                verArchivosParaDescargar(solicitudesDePago);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, intentelo de nuevo. detalle : " + ex.Message.Replace("'", string.Empty) + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message.Replace("'", string.Empty) + " ');", true);
            }
        }

        /// <summary>
        /// Generar archivo plano de pagos
        /// </summary>
        protected string generarArchivoPlanoTerceros(Int64? codigoActa)
        {
            string separador = ";";
            string fecha = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
            string nombreArchivo = nombreUnicoArchivoPlano("Terceros", codigoActa, ".txt");
            string directorioArchivosFiduciaria = rutaGuardarArchivos();
            List<string> datoArchivoPlano = new List<string>();

            if (Directory.Exists(directorioArchivosFiduciaria))
            {
                Array.ForEach(Directory.GetFiles(directorioArchivosFiduciaria), File.Delete);
            }
            else
            {
                Directory.CreateDirectory(directorioArchivosFiduciaria);
            }

            List<TerceroBeneficiario> tercerosBeneficiarios = getTercerosBeneficiarios(codigoActa);

            //Add linea inicial
            datoArchivoPlano.Add("1" + separador + fecha + separador + tercerosBeneficiarios.Count().ToString());

            //Add Datos de cada tercero beneficiario.
            int indice = 1;
            foreach (TerceroBeneficiario terceroBeneficiario in tercerosBeneficiarios)
            {
                datoArchivoPlano.Add(terceroBeneficiario.getRegistroArchivoPlano(indice));
                indice++;
            }
            //Add linea final 
            datoArchivoPlano.Add("5");

            File.WriteAllLines(directorioArchivosFiduciaria + nombreArchivo, datoArchivoPlano);

            return nombreArchivo;
        }

        /// <summary>
        /// Obtiene los terceros beneficiarios.
        /// </summary>
        /// <param name="codigoActa"> Codigo de acta </param>
        protected List<TerceroBeneficiario> getTercerosBeneficiarios(Int64? codigoActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener los terceros beneficiarios
                var beneficiarios = (from pagoBeneficiario in db.PagoBeneficiarios
                                     join pagoActividad in db.PagoActividad on pagoBeneficiario.Id_PagoBeneficiario equals pagoActividad.CodPagoBeneficiario
                                     join pagoActaSolicitudPago in db.PagosActaSolicitudPagos on pagoActividad.Id_PagoActividad equals pagoActaSolicitudPago.CodPagoActividad
                                     where pagoBeneficiario.RegistradoFA == false &&
                                           pagoActaSolicitudPago.Aprobado == true &&
                                           pagoActaSolicitudPago.CodPagosActaSolicitudes == codigoActa
                                     select new TerceroBeneficiario(
                                        pagoBeneficiario.NumIdentificacion,
                                        pagoBeneficiario.CodTipoIdentificacion,
                                        codigoActa)
                                      ).Distinct().ToList();
                return beneficiarios;
            }
        }

        protected string generarArchivoPlanoPagos(List<SolicitudDePago> solicitudesDePago)
        {
            string separador = ";";
            string fecha = fechaActa.Value.Day.ToString() + fechaActa.Value.Month.ToString() + fechaActa.Value.Year.ToString();
            string nombreArchivo = nombreUnicoArchivoPlano("Pagos", codigoActa, ".txt");
            string directorioArchivosFiduciaria = rutaGuardarArchivos();
            List<string> datoArchivoPlano = new List<string>();

            List<Pago> pagos = getPagos(codigoActa, fecha, directorioArchivosFiduciaria);

            int indice = 1;
            //Add linea inicial
            datoArchivoPlano.Add(indice + separador + fecha + separador + Constantes.CONST_Fideicomiso + separador + pagos.Count().ToString());
            decimal? totalReportado = 0;
            foreach (Pago pago in pagos)
            {
                pago.generarArchivoZip();

                datoArchivoPlano.Add(pago.getRegistroArchivoPlano(indice));
                indice++;
                totalReportado += pago.cantidadDinero;
            }
            //Add linea final 
            datoArchivoPlano.Add("5" + separador + moneyFormat(totalReportado));

            File.WriteAllLines(directorioArchivosFiduciaria + nombreArchivo, datoArchivoPlano);

            return nombreArchivo;
        }

        protected string moneyFormat(Decimal? valor)
        {
            String valorFormateado = valor.Value.ToString("00.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' });

            return !String.IsNullOrEmpty(valorFormateado) ? valorFormateado : "0";
        }

        /// <summary>
        /// Metodo para obtener los pagos
        /// </summary>
        /// <param name="codigoActa"> Codigo de acta </param>
        /// <param name="fechaActa"> Fecha del acta</param>
        /// <param name="directorioArchivosFiduciaria"> Directorio donde se guardaran los archivos </param>
        /// <returns></returns>
        protected List<Pago> getPagos(Int64? codigoActa, string fechaActa, string directorioArchivosFiduciaria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var solicitudesPagos = (from solicitudPago in db.PagosActaSolicitudPagos
                                        where solicitudPago.CodPagosActaSolicitudes == codigoActa &&
                                        solicitudPago.Aprobado == true
                                        select solicitudPago.CodPagoActividad).ToList();

                //Consulta para obtener los pagos
                var pagos = (from pagoActividad in db.PagoActividad
                             join pagoConcepto in db.PagoConcepto on pagoActividad.CodPagoConcepto equals pagoConcepto.Id_PagoConcepto
                             where solicitudesPagos.Contains(pagoActividad.Id_PagoActividad)
                             select new Pago(
                                pagoActividad.Id_PagoActividad,
                                pagoConcepto.CodigoPagoConcepto,
                                pagoActividad.CodProyecto,
                                pagoActividad.CodPagoBeneficiario,
                                pagoActividad.CantidadDinero,
                                codigoActa,
                                fechaActa,
                                directorioArchivosFiduciaria
                             )
                                      ).ToList();
                return pagos;
            }
        }

        /// <summary>
        /// Ver archivos de pago, terceros y zip
        /// </summary>
        protected void verArchivosParaDescargar(List<SolicitudDePago> solicitudesDePago)
        {
            //limpiamos los paneles de enlaces de pago y terceros y archivos adjuntos.
            pnlDescargarArchivosPagosYTerceros.Controls.Clear();
            pnlDescargarArchivosAdjuntos.Controls.Clear();
            pnlDescargarArchivosPagosYTerceros.Visible = true;
            pnlDescargarArchivosAdjuntos.Visible = true;

            Label lblDescargarArchivosPlanos = new Label();
            lblDescargarArchivosPlanos.ID = "lblDescargarArchivosPlanos";
            lblDescargarArchivosPlanos.Text = " Haga clic en los siguientes enlaces para descargar los archivos : <br/>";

            Label lblDescargarArchivosZip = new Label();
            lblDescargarArchivosZip.ID = "lblDescargarArchivosZip";
            lblDescargarArchivosZip.Text = " Archivos de Soporte de los Pagos : <br/> ";

            if (solicitudesDePago.Any())
            {
                pnlDescargarArchivosPagosYTerceros.Controls.Add(lblDescargarArchivosPlanos);
                pnlDescargarArchivosAdjuntos.Controls.Add(lblDescargarArchivosZip);

                verEnlacePlanoTercero(solicitudesDePago.First());
                verEnlacePlanoPagos(solicitudesDePago.First());
            }

            foreach (SolicitudDePago solicitudPago in solicitudesDePago.OrderBy(filter => filter.idPagoActividad))
            {
                verEnlaceArchivoZip(solicitudPago);
            }
        }

        protected void detalleSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("verDetallePago"))
            {
                if (e.CommandArgument != null)
                {
                    string idPagoActividad = e.CommandArgument.ToString();

                    Session["Id_PagoActividad"] = idPagoActividad;

                    string url = "CoordinadorPago.aspx";
                    string popupDetallePago = "window.open('" + url + "', 'popup_window', 'width=500,height=500,left=100,top=100,resizable=yes');";
                    ClientScript.RegisterStartupScript(this.GetType(), "script", popupDetallePago, true);
                }
            }
        }

        /// <summary>
        /// Ver enlace de archivo de terceros
        /// </summary>
        protected void verEnlacePlanoTercero(SolicitudDePago solicitudPago)
        {
            if (!String.IsNullOrEmpty(solicitudPago.archivoTercero))
            {

                HyperLink linkArchivoPlanoTerceros = new HyperLink();
                linkArchivoPlanoTerceros.ID = "ArchivoTercero" + solicitudPago.idPagoActividad.ToString();
                linkArchivoPlanoTerceros.Text = "<b>Archivo plano de terceros</b> <br/>";
                linkArchivoPlanoTerceros.NavigateUrl = directorioWebArchivosFiduciaria + solicitudPago.archivoTercero;
                linkArchivoPlanoTerceros.Target = "_new";
                pnlDescargarArchivosPagosYTerceros.Controls.Add(linkArchivoPlanoTerceros);

            }
        }

        /// <summary>
        /// Ver enlace de archivo de pagos
        /// </summary>
        protected void verEnlacePlanoPagos(SolicitudDePago solicitudPago)
        {
            if (!String.IsNullOrEmpty(solicitudPago.archivoTercero))
            {

                HyperLink linkArchivoPlanoPago = new HyperLink();
                linkArchivoPlanoPago.ID = "ArchivoPago" + solicitudPago.idPagoActividad.ToString();
                linkArchivoPlanoPago.Text = "<b>Archivos Pago </b> <br/>";
                linkArchivoPlanoPago.NavigateUrl = directorioWebArchivosFiduciaria + solicitudPago.archivoPago;
                linkArchivoPlanoPago.Target = "_new";
                pnlDescargarArchivosPagosYTerceros.Controls.Add(linkArchivoPlanoPago);

            }
        }

        /// <summary>
        /// Ver enlaces de archivos adjuntos en Zip
        /// </summary>
        protected void verEnlaceArchivoZip(SolicitudDePago solicitudPago)
        {
            if (!String.IsNullOrEmpty(solicitudPago.archivoZip))
            {
                HyperLink linkArchivoPagoZip = new HyperLink();
                linkArchivoPagoZip.ID = "ArchivoZip" + solicitudPago.idPagoActividad.ToString();
                linkArchivoPagoZip.Text = "<b>Archivos Pago No. " + solicitudPago.idPagoActividad.ToString() + "</b> <br/>";
                linkArchivoPagoZip.NavigateUrl = directorioWebArchivosFiduciaria + solicitudPago.archivoZip;
                linkArchivoPagoZip.Target = "_new";
                pnlDescargarArchivosAdjuntos.Controls.Add(linkArchivoPagoZip);
            }
        }

        /// <summary>
        /// Verificamos si el acta fue descargada, no se muestra botón
        /// de verificar token y se muestran los enlaces
        /// </summary>
        /// <param name="codigoActa"></param>
        protected void verificarEstadoActa(Int64? codigoActa, List<SolicitudDePago> solicitudesDePago)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                PagosActaSolicitudes solicitudPago = db.PagosActaSolicitudes.SingleOrDefault(ById => ById.Id_Acta == codigoActa);
                if (solicitudPago != null)
                {
                    if (solicitudPago.DescargadoFA == true)
                    {
                        pnlVerificarToken.Visible = false;
                        verArchivosParaDescargar(solicitudesDePago);
                    }
                    fechaActa = solicitudPago.Fecha;
                }
                else
                {
                    throw new ApplicationException("No se encontro el acta de solicitud de pago.");
                }

            }
        }

        /// <summary>
        /// Metodo para actualizar el acta de fonade a descargado y crear una nueva acta para fiduciaria donde
        /// se guarden la firma de los datos y los datos del firmante.
        /// </summary>
        /// <param name="codigoActa"> Codigo del acta de fonade </param>
        /// <param name="nombreArchivoTerceros"> Nombre del archivo de terceros  </param>
        /// <param name="nombreArchivoPagos">Nombre del archivo de pagos </param>
        protected void actualizarPagoDescargado(Int64? codigoActa, string nombreArchivoTerceros, string nombreArchivoPagos, List<SolicitudDePago> solicitudesDePago)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                PagosActaSolicitudes solicitudPagoFonade = db.PagosActaSolicitudes.Single(ById => ById.Id_Acta == codigoActa);

                solicitudPagoFonade.DescargadoFA = true;
                solicitudPagoFonade.ArchivoTercerosFA = nombreArchivoTerceros;
                solicitudPagoFonade.ArchivoPagosFA = nombreArchivoPagos;

                PagosActaSolicitudes solicitudPagoFiduciaria = db.PagosActaSolicitudes.SingleOrDefault(ById => ById.CodActaFonade == codigoActa && ById.Tipo.ToLower().Equals("fiduciaria"));

                if (solicitudPagoFiduciaria != null)
                {
                    solicitudPagoFiduciaria.DescargadoFA = true;
                    solicitudPagoFiduciaria.Fecha = DateTime.Now;
                    solicitudPagoFiduciaria.ArchivoTercerosFA = nombreArchivoTerceros;
                    solicitudPagoFiduciaria.ArchivoPagosFA = nombreArchivoPagos;
                }
                else
                {
                    PagosActaSolicitudes nuevaSolicitudPagoFiduciaria = new PagosActaSolicitudes
                    {
                        Fecha = DateTime.Now,
                        NumSolicitudes = db.PagosActaSolicitudPagos.Count(pagos => pagos.Aprobado == true && pagos.CodPagosActaSolicitudes == codigoActa),
                        Datos = getXmlParaFirmar(solicitudesDePago, codigoActa),
                        Firma = firmaDigital,
                        CodContacto = solicitudPagoFonade.CodContacto,
                        CodRechazoFirmaDigital = null,
                        Tipo = "Fiduciaria",
                        DatosFirma = datosFirmate,
                        DescargadoFA = true,
                        ArchivoTercerosFA = nombreArchivoTerceros,
                        ArchivoPagosFA = nombreArchivoPagos,
                        CodActaFonade = Convert.ToInt32(codigoActa),
                        CodContactoFiduciaria = solicitudPagoFonade.CodContactoFiduciaria
                    };

                    db.PagosActaSolicitudes.InsertOnSubmit(nuevaSolicitudPagoFiduciaria);
                }

                db.SubmitChanges();
            }

        }

        protected string rutaGuardarArchivos()
        {

            string directorioDestino = "\\Fiduciaria\\" + codigoActa.ToString() + "\\";
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");

            return directorioBase + directorioDestino;
        }

        protected string nombreUnicoArchivoPlano(string nombreArchivo, Int64? codigoActa, string extensionArchivo)
        {
            string anio = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.Day.ToString();
            string hora = DateTime.Now.Hour.ToString();
            string minuto = DateTime.Now.Minute.ToString();
            string segundo = DateTime.Now.Second.ToString();

            return dia + mes + anio + hora + minuto + segundo + nombreArchivo + codigoActa + extensionArchivo;
        }

        protected void ObtenerXml_Click(object sender, EventArgs e)
        {
            List<SolicitudDePago> solicitudesDePago = getSolicitudesDePago(codigoActa);

            xmlParaFirmar = getXmlParaFirmar(solicitudesDePago, codigoActa);

            Page.ClientScript.RegisterStartupScript(this.GetType(), "FirmarXml", "GenerarDatos_OnClick('" + xmlParaFirmar + "');", true);
        }

        /// <summary>
        /// Metodo para obtener el xml que sera firmado.
        /// </summary>
        /// <returns>Xml a firmar con formato string.</returns>
        protected string getXmlParaFirmar(List<SolicitudDePago> solicitudesDePago, long? codigoActa)
        {
            string nombreCoordinadorInterventor = usuario.Nombres + " " + usuario.Apellidos;
            string xmlConPagos = string.Empty;
            xmlConPagos = xmlConPagos + @"<?xml version=""1.0"" encoding=""windows-1252""?>";
            xmlConPagos = xmlConPagos + "<Xml_PAGOS_FIDUCIARIA>";

            int contador = 1;
            foreach (var pago in solicitudesDePago)
            {
                xmlConPagos = xmlConPagos + "		 <Xml_Solicitud" + contador + "> ";
                xmlConPagos = xmlConPagos + "		    <xml_CodSolicitudPago" + contador + "> " + pago.idPagoActividad.ToString() + "   </xml_CodSolicitudPago" + contador + "> ";
                xmlConPagos = xmlConPagos + "		    <xml_fechaCodSolicitud> " + pago.fechaCoordinador.ToString() + "   </xml_fechaCodSolicitud> ";
                xmlConPagos = xmlConPagos + "		    <xml_razonSocial> " + pago.razonSocial + "   </xml_razonSocial> ";
                xmlConPagos = xmlConPagos + "		    <xml_valorSolicitado> " + pago.cantidadDineroConFormato + "   </xml_valorSolicitado> ";
                xmlConPagos = xmlConPagos + "		 </Xml_Solicitud" + contador + "> ";
                contador++;
            }
            xmlConPagos = xmlConPagos + "	<xml_codigoActa>" + codigoActa.ToString() + "</xml_codigoActa>";
            xmlConPagos = xmlConPagos + "	<xml_FechaSolicitudes>" + DateTime.Now.ToString("dd/MM/yyyy") + "</xml_FechaSolicitudes>";
            xmlConPagos = xmlConPagos + "	<xml_NumeroSolicitudes>" + (solicitudesDePago.Count).ToString() + "</xml_NumeroSolicitudes>";
            xmlConPagos = xmlConPagos + "	<xml_UsuarioFonade>" + nombreCoordinadorInterventor + "</xml_UsuarioFonade>";
            xmlConPagos = xmlConPagos + "</Xml_PAGOS_FIDUCIARIA>";

            return xmlConPagos.Replace("'", string.Empty);
        }

    }
    public class SolicitudDePago
    {
        public int idPagoActividad { get; set; }
        public DateTime? fechaCoordinador { get; set; }
        public string razonSocial { get; set; }
        public decimal? cantidadDinero { get; set; }
        public string archivoPago { get; set; }
        public string archivoTercero { get; set; }
        public string archivoZip { get; set; }
        public string cantidadDineroConFormato { get; set; }
        public string fechaConFormato { get; set; }

        public SolicitudDePago(int idPagoActividad, DateTime? fechaCoordinador, string razonSocial, decimal? cantidadDinero, string archivoPago, string archivoTercero, string archivoZip)
        {
            this.idPagoActividad = idPagoActividad;
            this.fechaCoordinador = fechaCoordinador;
            this.razonSocial = razonSocial;
            this.cantidadDinero = cantidadDinero;
            this.archivoPago = archivoPago;
            this.archivoTercero = archivoTercero;
            this.archivoZip = archivoZip;
            this.cantidadDineroConFormato = FieldValidate.moneyFormat(cantidadDinero.Value, true);
            this.fechaConFormato = FieldValidate.getFechaConFormato(fechaCoordinador.Value);
        }
    }
    public class Pago
    {
        public Int64? codigoActa { get; set; }
        public int idPagoActividad { get; set; }
        public string codigoPagoConcepto { get; set; }
        public int? codigoProyecto { get; set; }
        public int? codigoPagoBeneficiario { get; set; }
        public decimal? cantidadDinero { get; set; }
        public int codigoConvocatoria { get; set; }
        public string observaciones { get; set; }
        public string numeroIdentificacion { get; set; }
        public int? codigoPagoBanco { get; set; }
        public int? codigoPagoSucursal { get; set; }
        public string numeroCuenta { get; set; }
        public string tipoIdentificacionSigla { get; set; }
        public string fechaActa { get; set; }
        public string encargoFiducia { get; set; }
        public string nombreArchivoPago { get; set; }
        public string directorioArchivosFiduciaria { get; set; }

        private string separador = ";";

        public List<PagoActividadarchivo> archivosPagos = new List<PagoActividadarchivo>();

        public Pago(int idPagoActividad, string codigoPagoConcepto, int? codigoProyecto, int? codigoPagoBeneficiario, decimal? cantidadDinero, Int64? codigoActa, string fechaActa, string directorioArchivosFiduciaria)
        {
            this.idPagoActividad = idPagoActividad;
            this.codigoPagoConcepto = codigoPagoConcepto;
            this.codigoProyecto = codigoProyecto;
            this.codigoPagoBeneficiario = codigoPagoBeneficiario;
            this.cantidadDinero = cantidadDinero;
            this.codigoActa = codigoActa;
            this.archivosPagos = getArchivosPagos(this.idPagoActividad);
            this.fechaActa = fechaActa;
            this.directorioArchivosFiduciaria = directorioArchivosFiduciaria;

            getDetallePago();
        }

        /// <summary>
        /// Obtiene la lista de archivos de pago y sus archivos adjuntos.
        /// </summary>
        public List<PagoActividadarchivo> getArchivosPagos(int idPagoActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var archivosPagos = (from archivoPago in db.PagoActividadarchivo
                                     where archivoPago.CodPagoActividad == idPagoActividad
                                     select archivoPago
                                    ).ToList();

                return archivosPagos;
            }
        }
        /// <summary>
        /// Genenar archivo zip con los adjuntos correspondientes.
        /// </summary>
        public void generarArchivoZip()
        {
            string directorioDocumentos = ConfigurationManager.AppSettings.Get("RutaDocumentos");
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            Boolean anyFileToZip = false; //Variable para saber si al menos un archivo adjunto fue encontrado.            
            string nombreArchivoPago = null;
            if (archivosPagos.Any())
            {
                using (var archivoZip = new ZipFile())
                {
                    var fileIndex = 1;
                    foreach (var archivoPago in archivosPagos)
                    {
                        string rutaArchivo = archivoPago.RutaArchivo.Replace(directorioDocumentos, directorioBase);

                        if (File.Exists(rutaArchivo))
                        {
                            anyFileToZip = true;
                            archivoZip.AddFile(rutaArchivo, string.Empty).FileName = fileIndex + Path.GetFileName(rutaArchivo);
                            fileIndex++;
                        }
                    }
                    //Si se genera el archivo Zip, guardamos el nombre del archivo.
                    //Sino vacio.
                    if (anyFileToZip)
                    {
                        nombreArchivoPago = nombreUnicoArchivoPlano("Pg", this.idPagoActividad, ".zip");
                        archivoZip.Save(directorioArchivosFiduciaria + nombreArchivoPago);
                    }
                    else
                    {
                        nombreArchivoPago = null;
                    }
                }
            }
            actualizarRutaArchivo(nombreArchivoPago);
        }

        /// <summary>
        /// Actualizamos la ruta del archizo Zip generado.
        /// </summary>
        /// <param name="nombreArchivoPago"></param>
        public void actualizarRutaArchivo(string nombreArchivoPago)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                PagoActividad pagoActividad = db.PagoActividad.Single(ById => ById.Id_PagoActividad == this.idPagoActividad);

                pagoActividad.RutaArchivoZIP = nombreArchivoPago;

                db.SubmitChanges();
                this.nombreArchivoPago = nombreArchivoPago;
            }
        }

        public string getRegistroArchivoPlano(int indice)
        {
            string primeraColumna = "2" + separador + indice.ToString() + separador;
            string idPagoActividad = this.idPagoActividad.ToString() + separador;
            string fechaActa = this.fechaActa + separador + "95" + separador + "E" + separador;
            string codigoPagoConcepto = this.codigoPagoConcepto.Replace(" ", string.Empty) + separador + this.nombreArchivoPago + separador + separador;
            string codigoConvocatoria = (this.codigoConvocatoria.ToString().Length < 3 ? "0" + this.codigoConvocatoria.ToString() : this.codigoConvocatoria.ToString());
            string codigoProyecto = (this.codigoProyecto.ToString().Length < 6 ? "0" + this.codigoProyecto.ToString() : this.codigoProyecto.ToString()) + separador + "0101" + separador + "101" + separador;
            string numeroIdentificacion = this.numeroIdentificacion.ToString() + separador;
            string cantidadDinero = moneyFormat(this.cantidadDinero) + separador;
            string columnaFecha = this.fechaActa + separador + "15" + separador + separador + separador + separador + "2" + separador;
            string encargoFiduciaria = (!String.IsNullOrEmpty(this.encargoFiducia) ? this.encargoFiducia : Constantes.CONST_EncargoFiduciario1.ToString()) + separador + this.codigoPagoBanco + separador + separador + this.numeroCuenta.Replace("-", "").Replace(" ", "") + separador;
            string observacion = (!string.IsNullOrEmpty(this.observaciones) ? this.observaciones.Substring(0, Math.Min(190, this.observaciones.Length)).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty) : "fonade");

            return ((primeraColumna + idPagoActividad + fechaActa).Replace(" ", string.Empty).ToUpper() + codigoPagoConcepto + ((codigoConvocatoria + codigoProyecto + numeroIdentificacion + cantidadDinero + columnaFecha + encargoFiduciaria).Replace(" ", string.Empty) + observacion).ToUpper());
        }

        protected string moneyFormat(Decimal? valor)
        {
            String valorFormateado = valor.Value.ToString("00.00", CultureInfo.InvariantCulture).TrimStart(new Char[] { '0' });

            return !String.IsNullOrEmpty(valorFormateado) ? valorFormateado : "0";
        }

        public void getDetallePago()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var convocatoriaProyectos = (from convocatoriaProyecto in db.ConvocatoriaProyectos
                                             where convocatoriaProyecto.Viable == true && convocatoriaProyecto.CodProyecto == this.codigoProyecto
                                             select convocatoriaProyecto).FirstOrDefault();

                var pagoBeneficiario = (from pagoBeneficiaros in db.PagoBeneficiarios
                                        join tipoIdentificacion in db.TipoIdentificacions on pagoBeneficiaros.CodTipoIdentificacion equals tipoIdentificacion.Id_TipoIdentificacion
                                        where pagoBeneficiaros.Id_PagoBeneficiario == this.codigoPagoBeneficiario
                                        select new
                                        {
                                            numeroIdentificacion = pagoBeneficiaros.NumIdentificacion,
                                            codigoPagoBanco = pagoBeneficiaros.CodPagoBanco,
                                            codigoPagoSucursal = pagoBeneficiaros.CodPagoSucursal,
                                            numeroCuenta = pagoBeneficiaros.NumCuenta,
                                            tipoIdentificacionSigla = tipoIdentificacion.Sigla
                                        }).FirstOrDefault();

                var actaPago = (from actaSolicitudPago in db.PagosActaSolicitudPagos
                                where actaSolicitudPago.CodPagoActividad == this.idPagoActividad &&
                                      actaSolicitudPago.CodPagosActaSolicitudes == this.codigoActa
                                select new
                                {
                                    observacion = actaSolicitudPago.Observaciones
                                }
                               ).FirstOrDefault();

                if (convocatoriaProyectos == null)
                    throw new ApplicationException("No se pudo obtener el codigo de la convocatoria.");
                if (pagoBeneficiario == null)
                    throw new ApplicationException("No se pudo obtener la información del pago del beneficiario.");
                if (actaPago == null)
                    throw new ApplicationException("No se pudo obtener la información del acta de pago.");

                this.codigoConvocatoria = convocatoriaProyectos.CodConvocatoria;
                this.observaciones = actaPago.observacion;
                this.numeroIdentificacion = pagoBeneficiario.numeroIdentificacion;
                this.codigoPagoBanco = pagoBeneficiario.codigoPagoBanco;
                this.codigoPagoSucursal = pagoBeneficiario.codigoPagoSucursal;
                this.numeroCuenta = pagoBeneficiario.numeroCuenta;
                this.tipoIdentificacionSigla = pagoBeneficiario.tipoIdentificacionSigla;

                var convocatoria = (from convoctatoriaNueva in db.Convocatoria
                                    where convoctatoriaNueva.Id_Convocatoria == this.codigoConvocatoria
                                    select convoctatoriaNueva).FirstOrDefault();
                if (convocatoria == null)
                    throw new ApplicationException("No se pudo obtener la información del encargo fiduciario.");
                this.encargoFiducia = convocatoria.encargofiduciario;
            }
        }
        /// <summary>
        /// Metodo para generar nombres unicos a archivos de pago.
        /// </summary>
        /// <param name="nombreArchivo"></param>
        /// <param name="codigoActa"></param>
        /// <param name="extensionArchivo"></param>
        /// <returns></returns>
        protected string nombreUnicoArchivoPlano(string nombreArchivo, Int64? codigoActa, string extensionArchivo)
        {
            string anio = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.Day.ToString();
            string hora = DateTime.Now.Hour.ToString();
            string minuto = DateTime.Now.Minute.ToString();
            string segundo = DateTime.Now.Second.ToString();
            string milesima = DateTime.Now.Millisecond.ToString();

            return dia + mes + anio + hora + minuto + segundo + milesima + nombreArchivo + codigoActa + extensionArchivo;
        }

    }
    public class TerceroBeneficiario
    {
        public Int64? codigoActa { get; set; }
        public int? codigoTipoIdentificacion { get; set; }
        public string numeroIdentificacion { get; set; }
        public string sigla { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nombres { get; set; }
        public string razonSocial { get; set; }
        public string tipoNaturaleza { get; set; }
        public string siglaTipoRetencion { get; set; }
        public string codigoCiudadDANE { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public int? codigoPagoBanco { get; set; }
        public int? codidgoPagoSucursal { get; set; }
        public int? tipoCuentaBancaria { get; set; }
        public string numeroCuentaBancaria { get; set; }
        private string separador = ";";

        public TerceroBeneficiario(string numeroIdentificacion, int? codigoTipoIdentificacion, Int64? codigoActa)
        {
            this.codigoTipoIdentificacion = codigoTipoIdentificacion;
            this.numeroIdentificacion = numeroIdentificacion;
            this.codigoActa = codigoActa;

            getDetalleTerceroBeneficiario(this.codigoTipoIdentificacion, this.numeroIdentificacion, this.codigoActa);
        }

        /// <summary>
        /// Obtiene el detalle de un tercero beneficiario.
        /// </summary>
        /// <param name="codigoActa"> Codigo de acta </param>
        protected void getDetalleTerceroBeneficiario(int? codigoTipoIdentificacion, string numeroIdentificacion, Int64? codigoActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener los terceros beneficiarios
                var beneficiario = (from pagoBeneficiario in db.PagoBeneficiarios
                                    join tipoIdentificacion in db.TipoIdentificacions on pagoBeneficiario.CodTipoIdentificacion equals tipoIdentificacion.Id_TipoIdentificacion
                                    join pagoTipoRetencion in db.PagoTipoRetencions on pagoBeneficiario.CodPagoTipoRetencion equals pagoTipoRetencion.Id_PagoTipoRetencion
                                    join ciudad in db.Ciudad on pagoBeneficiario.CodCiudad equals ciudad.Id_Ciudad
                                    join pagoActividad in db.PagoActividad on pagoBeneficiario.Id_PagoBeneficiario equals pagoActividad.CodPagoBeneficiario
                                    join pagoActaSolicitudPago in db.PagosActaSolicitudPagos on pagoActividad.Id_PagoActividad equals pagoActaSolicitudPago.CodPagoActividad
                                    where pagoBeneficiario.CodTipoIdentificacion == codigoTipoIdentificacion &&
                                          pagoBeneficiario.NumIdentificacion == numeroIdentificacion &&
                                          pagoActaSolicitudPago.CodPagosActaSolicitudes == codigoActa
                                    select new
                                    {
                                        sigla = tipoIdentificacion.Sigla,
                                        numIdentificacion = pagoBeneficiario.NumIdentificacion,
                                        nombre = pagoBeneficiario.Nombre,
                                        apellido = pagoBeneficiario.Apellido,
                                        razonSocial = pagoBeneficiario.RazonSocial,
                                        tipoNaturaleza = tipoIdentificacion.TipoNaturaleza,
                                        siglaTipoRetencion = pagoTipoRetencion.Sigla,
                                        codigoCiudadDANE = ciudad.CodigoDANE,
                                        telefono = pagoBeneficiario.Telefono,
                                        direccion = pagoBeneficiario.Direccion,
                                        fax = pagoBeneficiario.Fax,
                                        email = pagoBeneficiario.Email,
                                        codigoPagoBanco = pagoBeneficiario.CodPagoBanco,
                                        codidgoPagoSucursal = pagoBeneficiario.CodPagoSucursal ?? 0,
                                        tipoCuentaBancaria = pagoBeneficiario.TipoCuenta,
                                        numeroCuentaBancaria = pagoBeneficiario.NumCuenta
                                    }).FirstOrDefault();
                if (beneficiario == null)
                    throw new ApplicationException("Error al obtener tercero beneficiario.");

                this.sigla = beneficiario.sigla;
                this.nombre = beneficiario.nombre;
                this.apellido = beneficiario.apellido;
                this.razonSocial = beneficiario.razonSocial;
                this.tipoNaturaleza = beneficiario.tipoNaturaleza;
                this.siglaTipoRetencion = beneficiario.siglaTipoRetencion;
                this.codigoCiudadDANE = beneficiario.codigoCiudadDANE;
                this.telefono = beneficiario.telefono;
                this.direccion = beneficiario.direccion;
                this.fax = beneficiario.fax;
                this.email = beneficiario.email;
                this.codigoPagoBanco = beneficiario.codigoPagoBanco;
                this.codidgoPagoSucursal = beneficiario.codidgoPagoSucursal;
                this.tipoCuentaBancaria = beneficiario.tipoCuentaBancaria;
                this.numeroCuentaBancaria = beneficiario.numeroCuentaBancaria;
                this.nombres = this.apellido + " " + this.nombre;
            }
        }

        public string getRegistroArchivoPlano(int indice)
        {

            string primeraColumna = "2" + separador + indice.ToString() + separador;
            string tipoIdentificacion = this.sigla + separador;
            string numeroIdentificacion = this.numeroIdentificacion + separador;
            string razonSocial = (sigla.ToLower().Trim() == "a" ? this.razonSocial : this.nombres) + separador;
            string tipoNaturaleza = this.tipoNaturaleza + separador + "OT" + separador;
            string retencion = this.siglaTipoRetencion + separador;
            string codigoCiudad = this.codigoCiudadDANE + separador;
            string telefono = this.telefono.Replace("/", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + separador;
            string direccion = this.direccion + separador;
            string fax = this.fax.Replace("/", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + separador;
            string email = this.email + separador;
            string codigoPagobanco = this.codigoPagoBanco + separador;
            string columnaBlanco = separador;
            string tipoCuentaBancaria = (this.tipoCuentaBancaria == 1 ? "04" : "01") + separador;
            string numeroCuentaBancaria = this.numeroCuentaBancaria.Replace("-", "").Replace(" ", "") + separador;
            string ultimaColumna = "P" + separador + "A";

            return (primeraColumna + tipoIdentificacion + numeroIdentificacion + razonSocial + tipoNaturaleza + retencion + codigoCiudad + telefono + direccion + fax + email + codigoPagobanco + columnaBlanco + tipoCuentaBancaria + numeroCuentaBancaria + ultimaColumna).ToUpper();
        }

    }
}