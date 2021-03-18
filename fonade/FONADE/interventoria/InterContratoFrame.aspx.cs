using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;
using Ionic.Zip;
using Fonade.Negocio.Interventoria;
using static Fonade.Negocio.Interventoria.CorreosNotificacionBLL;
using Fonade.PlanDeNegocioV2.Formulacion.Report;
using static Fonade.PlanDeNegocioV2.Formulacion.Report.dsAceptarTerminosCargaInformacion;
using Microsoft.Reporting.WebForms;

namespace Fonade.FONADE.interventoria
{
    public partial class InterContratoFrame : Negocio.Base_Page
    {
        string CodProyecto;
        string CodEmpresa;
        string CodConvocatoria;
        string anioConvocatoria;
        string[] arr_meses = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        string txtSQL;
        string _cadenaConex = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public bool perfilVisibilidad
        {
            get
            {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_GerenteAdministrador)
                    return true;
                else
                    return false;
            }
            set { }
        }

        public bool mostrarEliminar
        {
            get
            {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    return true;
                else
                    return false;
            }
            set { }
        }

        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["CodProyecto"]);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

            if (CodProyecto == "0" || CodProyecto == null)
            {
                CodProyecto = Convert.ToString(Request.QueryString["codproyecto"]);
            }

            if (CodProyecto == null)
            {
                Response.Redirect("~/Fonade/MiPerfil/Home.aspx");
            }

            if (CodEmpresa == "0")
            {
                CodEmpresa = (from em in consultas.Db.Empresas
                              where em.codproyecto == int.Parse(CodProyecto)
                              select em.id_empresa).FirstOrDefault().ToString();
            }

            //codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodConvocatoria = Convert.ToString(HttpContext.Current.Session["CodConvocatoria"] ?? "0");


            if (!IsPostBack)
            {
                panelCargaArchivosEmprendedor.Visible = (usuario.CodGrupo == Constantes.CONST_Emprendedor);

                txtSQL = "SELECT Max(CodConvocatoria) AS CodConvocatoria FROM ConvocatoriaProyecto WHERE CodProyecto = " + CodProyecto;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                    CodConvocatoria = dt.Rows[0]["CodConvocatoria"].ToString();

                if (!string.IsNullOrEmpty(CodConvocatoria))
                {
                    txtSQL = "select year(fechainicio) from convocatoria where id_Convocatoria=" + CodConvocatoria;

                    dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt.Rows.Count > 0)
                        anioConvocatoria = dt.Rows[0][0].ToString();
                }


                lenarInfo();

                cargarDatosGridArchivos(Convert.ToInt32(CodProyecto));
                cargarDDLTipoArchivo(usuario.CodGrupo);
            }

        }

        private void cargarDDLTipoArchivo(int codGrupo)
        {
            var tiposDeArchivo = CargueMasivoZIPBLL.tipoCargaArchivo();

            var tipos = tiposDeArchivo
                .Where(x => x.idtipo != tipoDeCargaArchivo.ContratosDeCooperacionFirmados.ToString())
                .ToList();

            if(codGrupo == Datos.Constantes.CONST_Emprendedor)
                tipos = tipos.Where(x => x.idtipo != tipoDeCargaArchivo.ActaDeInicio.ToString()).ToList();

            ddlTipoArchivo.DataSource = tipos;
            ddlTipoArchivo.DataBind();
        }

        private void cargarDatosGridArchivos(int _codProyecto)
        {
            CargueMasivoZIPBLL cargueMasivoZIPBLL = new CargueMasivoZIPBLL();
            gvArchivosEspeciales.DataSource = cargueMasivoZIPBLL.getArchivosEspeciales(_codProyecto);
            gvArchivosEspeciales.DataBind();
        }

        private void lenarInfo()
        {
            //Inicializar variables.
            DateTime fecha = new DateTime();
            string sMes;
            string[] palabras;

            if (!string.IsNullOrEmpty(CodEmpresa) && !string.IsNullOrEmpty(CodProyecto))
            {
                txtSQL = "SELECT a.* FROM ContratoEmpresa as a, Empresa as b where a.CodEmpresa = b.Id_Empresa and b.CodProyecto = " + CodProyecto + " and a.CodEmpresa=" + CodEmpresa;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    lblNumContrato.Text = dt.Rows[0]["NumeroContrato"].ToString();
                    lblplazoMeses.Text = dt.Rows[0]["PlazoContratoMeses"].ToString();

                    #region Fecha del Acta.
                    //Obtener fecha "del acta".
                    bool tieneFechaInicioActa = false;
                    try
                    {
                        fecha = Convert.ToDateTime(dt.Rows[0]["FechaDeInicioContrato"].ToString());
                        tieneFechaInicioActa = true;
                    }
                    catch
                    {
                        fecha = DateTime.Today;
                        tieneFechaInicioActa = false;
                    }

                    //Cambiar fecha según FONADE clásico.
                    palabras = fecha.ToString("dd/MM/yyyy").Split('/');
                    string a = arr_meses[Int32.Parse(palabras[1])]; //Prueba, estaba en "la posición 0" y saliá error interno.
                    lblFechaActa.Text = tieneFechaInicioActa ? palabras[0] + "/" + arr_meses[Int32.Parse(palabras[1])] + "/" + palabras[2] : "";
                    #endregion

                    lblNumAppresupuestal.Text = dt.Rows[0]["NumeroAPContrato"].ToString();
                    lblObjeto.Text = dt.Rows[0]["ObjetoContrato"].ToString();

                    #region Fecha del ap.
                    //Obtener fecha "del ap".
                    try { fecha = Convert.ToDateTime(dt.Rows[0]["fechaap"].ToString()); }
                    catch { fecha = DateTime.Today; }

                    //Cambiar fecha según FONADE clásico.
                    //palabras = fecha.ToString("dd/MM/yyyy").Split('/');
                    //lblFechaAp.Text = palabras[1] + "/" + arr_meses[Int32.Parse(palabras[0])] + "/" + palabras[2];
                    sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.InvariantCulture);
                    lblFechaAp.Text = fecha.Day + "/" + sMes + "/" + fecha.Year;
                    #endregion

                    #region Fecha de firma del contrato.
                    //Obtener fecha "de firma del contrato".
                    bool tieneFechaFirmaActa = false;
                    try
                    {
                        fecha = Convert.ToDateTime(dt.Rows[0]["FechaFirmaDelContrato"].ToString());
                        tieneFechaFirmaActa = true;
                    }
                    catch
                    {
                        fecha = DateTime.Today;
                        tieneFechaFirmaActa = false;
                    }

                    //Cambiar fecha según FONADE clásico.
                    palabras = fecha.ToString("dd/MM/yyyy").Split('/');
                    lblFechaFirmaContrato.Text = tieneFechaFirmaActa ? palabras[0] + "/" + arr_meses[Int32.Parse(palabras[1])] + "/" + palabras[2] : "";
                    #endregion

                    lblPolizaSeguro.Text = dt.Rows[0]["numeropoliza"].ToString();
                    lblCompaniaSeguroVida.Text = dt.Rows[0]["companiaseguros"].ToString();
                    lblValorInicial.Text = Double.Parse(dt.Rows[0]["ValorInicialEnPesos"].ToString()).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                }
            }

            cargarGridAnexos(CodProyecto);

            if (validarPermisoPerfil(usuario.CodGrupo))
            {
                lnkSuberArchivo.Visible = true;
                lnkSuberArchivo.Enabled = true;
            }
            else
            {
                lnkSuberArchivo.Visible = false;
                lnkSuberArchivo.Enabled = false;
            }

            Adjunto.Visible = false;
            Adjunto.Enabled = false;
        }

        private bool validarPermisoPerfil(int _codGrupo)
        {
            bool validado = false;

            if (usuario.CodGrupo == Constantes.CONST_Interventor ||
                usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor ||
                usuario.CodGrupo == Constantes.CONST_GerenteInterventor ||
                usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
            {
                validado = true;
            }

            return validado;
        }

        private void cargarGridAnexos(string _codProyecto)
        {
            if (!string.IsNullOrEmpty(_codProyecto))
            {
                txtSQL = "select distinct * from " +
                    " ContratosArchivosAnexos where CodProyecto=" + _codProyecto +
                    " order by NombreArchivo";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("filePath"));
                    //dt.Columns.Add(new DataColumn("filePathIndividual"));

                    //var rutaIndividual = System.Configuration.ConfigurationManager.AppSettings["FServerNuevo"].Replace("{0}", string.Empty);
                    //dt.Columns["filePathIndividual"].Expression = string.Format("'{0}'+ruta", rutaIndividual);

                    var xpr = System.Configuration.ConfigurationManager.AppSettings.Get("RutaIP").Replace("{0}", string.Empty);
                    dt.Columns["filePath"].Expression = string.Format("'{0}'+ruta", xpr);
                    //dt.Columns["filePath"].Expression = ConfigurationManager.AppSettings.Get("RutaIP") + "ruta";

                    //DataList1.DataSource = dt;
                    //DataList1.DataBind();

                    gvDescargaArchivos.DataSource = dt;
                    gvDescargaArchivos.DataBind();
                }
            }
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            var ygv = ((Button)sender);
            var ujm = ygv.Text;
            var wsx = ygv.CommandArgument;
            var edc = this.CodProyecto;
            txtSQL = string.Format("delete from ContratosArchivosAnexos where Ruta = '{0}' and Nombrearchivo = '{1}' and CodProyecto = {2}", wsx, ujm, edc);
            ejecutaReader(txtSQL, 2);
        }

        protected void lnkSuberArchivo_Click1(object sender, EventArgs e)
        {
            if (validarPermisoPerfil(usuario.CodGrupo))
            {
                panexosagre.Visible = false;
                panexosagre.Enabled = false;

                Adjunto.Visible = true;
                Adjunto.Enabled = true;
            }
            else
            {
                Adjunto.Visible = false;
                Adjunto.Enabled = false;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Adjunto.Visible = false;
            Adjunto.Enabled = false;

            panexosagre.Visible = true;
            panexosagre.Enabled = true;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/05/2014.
        /// Limitado por tamaño la carga de archivos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubirDocumento_Click(object sender, EventArgs e)
        {
            if (fuArchivo.HasFile)
            {
                if (fuArchivo.PostedFile.ContentLength > 10485760) // = 10)
                {
                    lblErrorDocumento.Visible = true;
                    lblErrorDocumento.Text = "El tamaño del archivo debe ser menor a 10 Mb.";
                    return;
                }
                else
                {
                    #region Procesar el archivo seleccionado.

                    String NombreArchivo = System.IO.Path.GetFileName(fuArchivo.PostedFile.FileName);
                    String extension = System.IO.Path.GetExtension(fuArchivo.PostedFile.FileName);

                    string saveLocation = string.Format("Documentos/Proyecto/Proyecto_{0}/{1}", CodProyecto, NombreArchivo);

                    var folder = Server.MapPath("~\\Documentos\\Proyecto\\Proyecto_" + CodProyecto);
                    if (!System.IO.Directory.Exists(folder))
                    {
                        System.IO.Directory.CreateDirectory(folder);
                    }

                    fuArchivo.SaveAs(Server.MapPath("~\\Documentos\\Proyecto\\Proyecto_" + CodProyecto + "\\") + NombreArchivo);

                    txtSQL = "insert into ContratosArchivosAnexos (CodProyecto,ruta,NombreArchivo) values (" + CodProyecto + ",'" + saveLocation + "','" + NombreArchivo + "')";

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    SqlCommand cmd = new SqlCommand(txtSQL, conn);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteReader();
                    }
                    catch (SqlException) { }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }

                    Adjunto.Visible = false;
                    Adjunto.Enabled = false;

                    panexosagre.Visible = true;
                    panexosagre.Enabled = true;

                    Response.Redirect("InterContratoFrame.aspx");

                    #endregion
                }
            }
            else
            {
                lblErrorDocumento.Visible = true;
                lblErrorDocumento.Text = "No ha seleccionado un archivo.";
                return;
            }
        }

        #region Métodos de Mauricio Arias Olave.

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

        #endregion

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {

            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("Contrato");
                foreach (GridViewRow row in gvDescargaArchivos.Rows)
                {
                    if ((row.FindControl("chkSelect") as CheckBox).Checked)
                    {
                        string filePath = (row.FindControl("lblFilePath") as Label).Text;
                        zip.AddFile(filePath, "Contrato");
                    }
                }
                Response.Clear();
                Response.BufferOutput = false;
                string zipName = String.Format("Contrato_{0}_{1}.zip", CodProyecto.ToString(), DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                zip.Save(Response.OutputStream);
                Response.End();
            }


        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvDescargaArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Borrar")
            {
                if (e.CommandArgument != null)
                {
                    string id = e.CommandArgument.ToString();
                    lblIdArchivoContrato.Text = id;
                    cargarDatosEnModal(Convert.ToInt32(id));
                    ModalEliminarArchivo.Show();
                }
            }
        }

        private void cargarDatosEnModal(int _idArchivo)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
            {
                var archivo = (from arc in db.ContratosArchivosAnexos
                               where arc.IdContratoArchivoAnexo == _idArchivo
                               select arc).FirstOrDefault();

                if (archivo != null)
                {
                    lblNombreArchivo.Text = archivo.NombreArchivo;
                }

            }
        }

        private bool ingresarRegistroEliminado(int _idArchivo, string _motivo, ContratoArchivoDTO _contratoArchivoDTO)
        {
            bool ingresado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadenaConex))
            {
                ContratosArchivosEliminado contratosArchivos = new ContratosArchivosEliminado
                {
                    CodProyecto = _contratoArchivoDTO.codProyecto,
                    ruta = _contratoArchivoDTO.ruta,
                    NombreArchivo = _contratoArchivoDTO.NombreArchivo,
                    FechaIngresoEnAnexos = _contratoArchivoDTO.fechaIngreso,
                    CodContactoEnAnexos = _contratoArchivoDTO.codContactoArchivo,
                    CodContactoEliminaArchivo = usuario.IdContacto,
                    MotivoEliminacion = _motivo,
                    FechaEliminacion = DateTime.Now
                };

                db.ContratosArchivosEliminados.InsertOnSubmit(contratosArchivos);
                db.SubmitChanges();

                ingresado = true;
            }

            return ingresado;
        }

        private bool EliminarArchivo(int _idArchivo, string _motivo)
        {
            bool eliminado = false;

            ContratoArchivoDTO archivoDTO = new ContratoArchivoDTO();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
            {
                var query = (from ca in db.ContratosArchivosAnexos
                             where ca.IdContratoArchivoAnexo == _idArchivo
                             select ca).FirstOrDefault();

                archivoDTO.codContactoArchivo = query.CodContacto;
                archivoDTO.codProyecto = query.CodProyecto;
                archivoDTO.fechaIngreso = query.FechaIngreso;
                archivoDTO.NombreArchivo = query.NombreArchivo;
                archivoDTO.ruta = query.ruta;

                if (ingresarRegistroEliminado(_idArchivo, _motivo, archivoDTO))
                {
                    //Eliminar en FonadeDB
                    db.ContratosArchivosAnexos.DeleteOnSubmit(query);
                    db.SubmitChanges();

                    cargarGridAnexos(archivoDTO.codProyecto.ToString());

                    eliminado = true;
                }
            }
            return eliminado;
        }

        protected void btnEliminarArchivo_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblIdArchivoContrato.Text);
            string motivo = txtMotivoEliminar.Text;
            if (motivo != "")
            {
                if (EliminarArchivo(id, motivo))
                {
                    txtMotivoEliminar.Text = "";
                    Alert("Se eliminó el archivo correctamente.");
                }
                else
                {
                    txtMotivoEliminar.Text = motivo;
                    Alert("No se logró eliminar el archivo.");
                }
            }
            else
            {
                Alert("El campo motivo es obligatorio.");
            }

        }

        protected void gvArchivosEspeciales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("VerArchivo"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros;
                    parametros = e.CommandArgument.ToString().Split(';');

                    var nombreArchivo = parametros[0];
                    var urlArchivo = ConfigurationManager.AppSettings.Get("RutaIP") + parametros[1];
                    string id = parametros[2];

                    Response.Clear();
                    //Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + CodigoProyecto + "_" + nombreArchivo);
                    Response.TransmitFile(urlArchivo);
                    Response.End();
                }
            }
            if (e.CommandName.Equals("Borrar"))
            {
                if (e.CommandArgument != null)
                {
                    int idArchivoActa = Convert.ToInt32(e.CommandArgument.ToString());

                    CargueMasivoZIPBLL cargueMasivoZIPBLL = new CargueMasivoZIPBLL();

                    if (cargueMasivoZIPBLL.eliminarArchivosEspecialContrato(idArchivoActa, usuario.IdContacto))
                    {
                        Alert("Archivo eliminado!");
                        cargarDatosGridArchivos(Convert.ToInt32(CodProyecto));
                    }
                }
            }
        }

        protected void btnSubirFirmados_Click(object sender, EventArgs e)
        {
            CargueMasivoZIPBLL cargueMasivoZIPBLL = new CargueMasivoZIPBLL();
            string tipoArchivo = ddlTipoArchivo.SelectedValue;
            string error = "";
            if (cargueMasivoZIPBLL.cargarArchivoAdicionalEmprendedor(FUArchivoFirmado
                                                                    , tipoArchivo, CodigoProyecto, usuario.IdContacto
                                                                    , ref error))
            {
                int _codigoProyecto = Convert.ToInt32(CodProyecto);
                cargarDatosGridArchivos(_codigoProyecto);
                string archivoAceptacionPDF = "";

                var tipoContratoGarantiaMobiliaria = CargueMasivoZIPBLL.tipoCargaArchivo();

                //Excepcion de generar archivo de aceptacion de terminos ContratoGarantiasMobiliarias y Contrapartidas
                if (!(tipoArchivo == tipoContratoGarantiaMobiliaria.Where(x => x.idtipo == tipoDeCargaArchivo.ContratoGarantiasMobiliarias.ToString())
                                                                .Select(x => x.idtipo).FirstOrDefault())
                && !(tipoArchivo == tipoContratoGarantiaMobiliaria.Where(x => x.idtipo == tipoDeCargaArchivo.Contrapartidas.ToString())
                                                                .Select(x => x.idtipo).FirstOrDefault()))
                {
                    if (generarPDFAceptacion(usuario.Nombres + " " + usuario.Apellidos
                                     , usuario.Email, _codigoProyecto, tipoArchivo
                                     , ref archivoAceptacionPDF))
                    {
                        //Insertar en BD la ruta del archivo
                        Insert(_codigoProyecto, archivoAceptacionPDF);
                    }
                }

                cargarGridAnexos(CodProyecto);
                Alert("Se cargó el archivo exitosamente");
            }
            else
            {
                Alert(error);
            }

        }

        protected void Insert(int codigoProyecto, string fileName)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var archivoContrato = new ContratosArchivosAnexo
                {
                    CodProyecto = codigoProyecto,
                    NombreArchivo = fileName,
                    ruta = "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + fileName,
                    CodContacto = usuario.IdContacto,
                    FechaIngreso = DateTime.Now
                };
                db.ContratosArchivosAnexos.InsertOnSubmit(archivoContrato);
                db.SubmitChanges();
            }
        }

        private bool generarPDFAceptacion(string nombre, string email
                                        , int codigoProyecto, string tipoArchivo
                                        , ref string archivoNotificacion)
        {
            bool generado = false;

            try
            {
                string OutPutDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "Proyecto\\";
                var customOutPutDirectory = OutPutDirectory + "Proyecto_" + codigoProyecto + "\\";

                dsAceptarTerminosCargaInformacion dsAceptarTerminosCarga = new dsAceptarTerminosCargaInformacion();

                dtCargaInfoRow dtCargaInfoRow = dsAceptarTerminosCarga.dtCargaInfo.NewdtCargaInfoRow();

                dtCargaInfoRow.EmailAcepta = email;
                dtCargaInfoRow.FechaHoraAceptacion = DateTime.Now.ToString();
                dtCargaInfoRow.NombreAcepta = nombre;
                dtCargaInfoRow.Mensaje = "En mi calidad de Emprendedor beneficiario del " +
                    "Fondo Emprender, " +
                    "declaro bajo juramento y certifico con mi firma digitalizada, " +
                    "de manera libre y voluntaria que cada uno de los hechos, " +
                    "comentarios o precisiones sobre la ejecución del plan de negocios " +
                    "y los documentos de soporte presentados durante la visita de seguimiento virtual, " +
                    "en el marco del contrato suscrito con ENTerritorio y el SENA, " +
                    "se ajustan a la verdad, son ciertos y " +
                    "veraces frente a la realidad empresarial y mi actuar como empresario";

                dsAceptarTerminosCarga.dtCargaInfo.AdddtCargaInfoRow(dtCargaInfoRow);

                ReportDataSource reportDataCargaInfo = new ReportDataSource();
                reportDataCargaInfo.Value = dsAceptarTerminosCarga.dtCargaInfo;
                reportDataCargaInfo.Name = "dsDatos";

                LocalReport report = new LocalReport();

                report.DataSources.Add(reportDataCargaInfo);

                report.ReportPath = @"PlanDeNegocioV2\Formulacion\Report\aceptarTerminosCargaInformacion.rdlc";

                byte[] fileBytes = report.Render("PDF");

                if (fileBytes != null)
                {
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", fileBytes.Length.ToString());
                    //Response.BinaryWrite(fileBytes);
                    string strFilePath = customOutPutDirectory;
                    string strFileName = codigoProyecto + "-AceptaTerminos_" + tipoArchivo + ".pdf";
                    string filename = Path.Combine(strFilePath, strFileName);
                    using (FileStream fs = new FileStream(filename, FileMode.Create))
                    {
                        fs.Write(fileBytes, 0, fileBytes.Length);
                    }

                    archivoNotificacion = strFileName;
                }
                generado = true;
            }
            catch (Exception ex)
            {
                generado = false;
            }

            return generado;
        }
    }

    public class ContratoArchivoDTO
    {
        public int? codProyecto { get; set; }
        public string ruta { get; set; }
        public string NombreArchivo { get; set; }
        public DateTime? fechaIngreso { get; set; }
        public int? codContactoArchivo { get; set; }
    }
}