using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using System.Data.OleDb;
using Datos;
using System.Globalization;
using System.Drawing;
using System.Text;
using Fonade.Account;
using System.Web.Security;
using ExcelDataReader;

namespace Fonade.FONADE.interventoria
{
    /// <summary>
    /// Formulario para subir archivos de respuesta de fiduciaria.
    /// By @marztres  
    /// </summary>
    public partial class SubirRespuestaPagos : Negocio.Base_Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnsubir_Click(object sender, EventArgs e)
        {
            try
            {
                var rutaArchivo = SubirRespuestaDePagos();
                var excelPago = ProcesarArchivoExcel(rutaArchivo);

                if (excelPago.Columns.Count < 22)
                    throw new ApplicationException("El archivo excel no tiene las columnas necesarias para continuar, debe tener 22 columnas minimo o 23 maximo si incluye opción de observaciones de cambio");

                var respuestasPago = new List<RespuestaPago>();

                foreach (DataRow excelRow in excelPago.Rows)
                {
                    RespuestaPago respuestaPago = new RespuestaPago(excelRow);
                    respuestaPago.Procesar();

                    respuestasPago.Add(respuestaPago);
                }

                gvRespuestaPagos.DataSource = respuestasPago;
                gvRespuestaPagos.DataBind();

                //Enviar Correo

                pnlEjemplo.Visible = false;
                pnlResultados.Visible = true;
            }
            catch (ApplicationException ex)
            {
                pnlEjemplo.Visible = true;
                pnlResultados.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message.Replace("'", string.Empty) + "');", true);
            }
            catch (Exception ex)
            {
                pnlEjemplo.Visible = true;
                pnlResultados.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al procesar el archivo, verifique si estan todas las columnas completas e intentelo de nuevo. detalle : " + ex.Message.Replace("'", string.Empty) + " ');", true);
            }
        }

        protected void grvResumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var color = gvRespuestaPagos.DataKeys[e.Row.RowIndex].Values[0].ToString();
                e.Row.BackColor = Color.FromName(color);
            }
        }

        protected string SubirRespuestaDePagos()
        {
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string directorioDestino = "RespuestaPagos\\";
            StringBuilder querySql = new StringBuilder();
            string nombreArchivo = "RespuestaPagos".GetNombreUnicoArchivo(System.IO.Path.GetExtension(archivoPagos.PostedFile.FileName));

            // ¿ Es valido el archivo ?
            if (!archivoPagos.HasFile)
                throw new ApplicationException("Archivo invalido");
            // ¿ Es Excel Valido ? 
            if (archivoPagos.PostedFile.ContentType != "application/vnd.ms-excel" && archivoPagos.PostedFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                throw new ApplicationException("Adjunte un archivo Excel con extensión .xls o .xlsx, los demas tipos son invalidos.");
            //¿ Nombre archivo empty?
            if (string.IsNullOrEmpty(archivoPagos.FileName) || !System.IO.Path.GetExtension(archivoPagos.PostedFile.FileName).Contains(".xls"))
                throw new ApplicationException("Nombre de archivo invalido' ");

            return UploadFileToServer(archivoPagos, directorioBase, directorioDestino, nombreArchivo);
        }



        protected DataTable ProcesarArchivoExcel(string rutaArchivo)
        {
            string extencion = System.IO.Path.GetExtension(rutaArchivo);
            string proveedorExcel = extencion == ".xlsx" ? @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo + "; Extended Properties=Excel 12.0;" : @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + rutaArchivo + "; Extended Properties=Excel 8.0";

            DataTable table = new DataTable();

            //Ruta del fichero Excel
            string filePath = rutaArchivo;

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    // Ejemplos de acceso a datos
                    table = result.Tables[0];
                    //DataRow row = table.Rows[0];
                    //string cell = row[0].ToString();
                }
            }

            return table;

            //using (OleDbConnection oleDbConnection = new OleDbConnection(proveedorExcel))
            //{
            //    using (var excelTable = new DataTable())
            //    {
            //        using (var oleDbCommand = new OleDbCommand("select * from [Pagos$]", oleDbConnection))
            //        {
            //            oleDbConnection.Open();
            //            using (var adapter = new OleDbDataAdapter(oleDbCommand))
            //            {
            //                adapter.Fill(excelTable);

            //                return excelTable;
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Metodo para subir el archivo a un directorio indicado
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="directorioBase"></param>
        /// <param name="directorioDestino"></param>
        protected string UploadFileToServer(FileUpload archivo, string directorioBase, string directorioDestino, string nombreArchivo)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(directorioBase + directorioDestino))
                Directory.CreateDirectory(directorioBase + directorioDestino);
            //¿ Existe el archivo ? si delete
            if (File.Exists(directorioBase + directorioDestino + nombreArchivo))
                File.Delete(directorioBase + directorioDestino + nombreArchivo);

            archivo.SaveAs(directorioBase + directorioDestino + nombreArchivo);

            return directorioBase + directorioDestino + nombreArchivo;
        }
    }

    public class RespuestaPago
    {

        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        public string Indice { get; set; }
        public int? CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string Nit { get; set; }
        public string Beneficiario { get; set; }
        public string DocumentoAPagar { get; set; }
        public string TipoCuentaBancaria { get; set; }
        public string NumeroCuentaBancaria { get; set; }
        public int? SolicitudPago { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public int? ValorBruto { get; set; }
        public decimal? ValorRetefuente { get; set; }
        public decimal? ValorReteIva { get; set; }
        public decimal? ValorReteIca { get; set; }
        public decimal? ValorReteCree { get; set; }
        public decimal? ValorPagado { get; set; }
        public string CodigoAch { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Observaciones { get; set; }
        public string MotivoRechazo { get; set; }
        public DateTime? FechaRechazo { get; set; }
        public DateTime? FechaMovimiento { get; set; }
        public string ObservacionReproceso { get; set; }
        public int IdActividad
        {
            get
            {
                var documentoAPagar = DocumentoAPagar;
                var idActividad = string.Empty;

                try
                {

                    if (String.IsNullOrEmpty(documentoAPagar))
                        return 0;

                    var indiceInicial = documentoAPagar.LastIndexOf("Pg") + "Pg".Length;
                    var tamanoId = documentoAPagar.LastIndexOf(".zip") - indiceInicial;
                    idActividad = documentoAPagar.Substring(indiceInicial, tamanoId);

                    return Convert.ToInt32(idActividad);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
            }
        }
        //Actividad guardada en base de datos.
        public PagoActividad Actividad { get; set; }
        public Boolean Aprobado
        {
            get
            {
                return !string.IsNullOrEmpty(CodigoAch) ? true : false;
            }
            private set { }
        }
        public int Estado
        {
            get
            {
                return Aprobado ? Constantes.CONST_EstadoAprobadoFA : Constantes.CONST_EstadoRechazadoFA;
            }
            private set { }
        }
        private string _color;
        public string Color
        {
            get
            {
                if (_color.Equals("amarillo"))
                    return "#FFFF66";
                if (_color.Equals("rojo"))
                    return "#FFCCCC";
                if (_color.Equals("gris"))
                    return "#DCDCDC";
                if (_color.Equals("azul"))
                    return "#99FFFF";
                return "#DCDCDC";
            }
            set
            {
                _color = value.Trim();
            }
        }
        public string MensajeSistema
        {
            get;
            set;
        }
        //Verifica si es diferente algun dato de la base de datos.
        public Boolean Changed
        {
            get
            {
                return HasChanged();
            }
            set
            {
            }
        }
        public RespuestaPago(DataRow rowExcel)
        {
            Indice = rowExcel.GetString(0);
            CodigoProyecto = rowExcel.GetInt(1);
            NombreProyecto = rowExcel.GetString(2);
            Nit = rowExcel.GetString(3);
            Beneficiario = rowExcel.GetString(4);
            DocumentoAPagar = rowExcel.GetString(5);
            TipoCuentaBancaria = rowExcel.GetString(6);
            NumeroCuentaBancaria = rowExcel.GetString(7);
            SolicitudPago = rowExcel.GetInt(8);
            FechaOperacion = rowExcel.GetDate(9);
            ValorBruto = rowExcel.GetInt(10);
            ValorRetefuente = rowExcel.GetDecimal(11);
            ValorReteIva = rowExcel.GetDecimal(12);
            ValorReteIca = rowExcel.GetDecimal(13);
            ValorReteCree = rowExcel.GetDecimal(14);
            ValorPagado = rowExcel.GetDecimal(15);
            CodigoAch = rowExcel.GetString(16);
            FechaPago = rowExcel.GetDate(17);
            Observaciones = rowExcel.GetString(18);
            MotivoRechazo = rowExcel.GetString(19);
            FechaRechazo = rowExcel.GetDate(20);
            FechaMovimiento = rowExcel.GetDate(21);
            ObservacionReproceso = rowExcel.GetString(22);
            Actividad = GetActividad(IdActividad);
        }

        public Boolean HasChanged()
        {
            var changed = false;

            if (Actividad.Estado != Constantes.CONST_EstadoAprobadoFA && Actividad.Estado != Constantes.CONST_EstadoRechazadoFA)
                return false;
            var estadoChanged = Estado != Actividad.Estado ? true : false;
            var observacionesChanged = Observaciones != Actividad.ObservacionesFA ? true : false;
            var fechaPagoChanged = FechaPago.GetValueOrDefault(DateTime.Now).Date != Actividad.FechaRtaFA.GetValueOrDefault(DateTime.Now).Date ? true : false;
            var valorReteFuenteChanged = ValorRetefuente != Actividad.valorretefuente ? true : false;
            var valorReteIcaChanged = ValorReteIca != Actividad.valorreteica ? true : false;
            var valorReteIvaChanged = ValorReteIva != Actividad.valorreteiva ? true : false;
            var valorReteCreeChanged = ValorReteCree != Actividad.otrosdescuentos ? true : false;
            var valorPagadoChanged = ValorPagado != Actividad.valorpagado ? true : false;
            var codigoAchChanged = CodigoAch != Actividad.codigopago ? true : false;
            var observacionReprocesoChanged = ObservacionReproceso != Actividad.ObservacionCambio ? true : false;

            if (estadoChanged || observacionesChanged || fechaPagoChanged || valorReteFuenteChanged || valorReteIcaChanged || valorReteIvaChanged || valorReteCreeChanged || valorPagadoChanged || codigoAchChanged || observacionReprocesoChanged)
                changed = true;

            return changed;
        }
        public PagoActividad GetActividad(int idActividad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.PagoActividad.SingleOrDefault(filter => filter.Id_PagoActividad.Equals(idActividad));
            }
        }

        public void Procesar()
        {
            try
            {
                if (IdActividad.Equals(0))
                    throw new Exception("No. Documento a pagar no tiene el formato correcto.");
                if (Actividad == null)
                    throw new Exception("No existe el pago en el sistema.");
                if (Changed && string.IsNullOrEmpty(ObservacionReproceso))
                    throw new ApplicationException("No tiene observación de cambio.");
                if (Aprobado && Observaciones.ToLower().Trim().Contains("rechazad"))
                    throw new Exception("Rectifique la información, tiene codigo ach pero esta rechazado en la observación.");
                if (!Aprobado && !Observaciones.ToLower().Trim().Contains("rechazad"))
                    throw new Exception("Rectifique la información, pago no aprobado pero no esta rechazado en la observación.");
                if (Changed)
                    MensajeSistema = ObservacionReproceso;

                //Se valida si el usuario es diferente al administrador
                if (Usuario.CodGrupo != Constantes.CONST_AdministradorSistema)
                {
                    string cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    int? codOperador = 0;
                    using (FonadeDBDataContext db = new FonadeDBDataContext(cadena))
                    {
                        codOperador = (from p in db.Proyecto
                                       where p.Id_Proyecto == CodigoProyecto
                                       select p.codOperador).FirstOrDefault();
                    }

                    if (codOperador != Usuario.CodOperador)
                        throw new ApplicationException("El proyecto no pertenece a su operador.");
                }

                Save();
            }
            catch (ApplicationException ex)
            {
                Color = "rojo";
                MensajeSistema = ex.Message;
            }
            catch (Exception ex)
            {
                Color = "amarillo";
                MensajeSistema = "Error : " + ex.Message;
            }
        }

        public void Save()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.PagoActividad.SingleOrDefault(filter => filter.Id_PagoActividad.Equals(IdActividad));

                string idPagoActividad = IdActividad.ToString();
                string motivoRechazo = "";

                if (Aprobado)
                {
                    entity.FechaRtaFA = FechaPago;
                    entity.ObservacionesFA = Observaciones;
                    entity.Estado = Estado;
                    entity.valorreteiva = ValorReteIva;
                    entity.valorreteica = ValorReteIca;
                    entity.valorretefuente = ValorRetefuente;
                    entity.otrosdescuentos = ValorReteCree;
                    entity.valorpagado = ValorPagado;
                    entity.codigopago = CodigoAch;
                    entity.FechaCargaArchivo = DateTime.Now;
                    if (Changed)
                        entity.ObservacionCambio = ObservacionReproceso;
                }
                else
                {
                    entity.FechaCargaArchivo = DateTime.Now;
                    entity.FechaRtaFA = FechaRechazo;
                    entity.ObservacionesFA = Observaciones + Environment.NewLine + MotivoRechazo;
                    entity.Estado = Estado;
                    if (Changed)
                        entity.ObservacionCambio = ObservacionReproceso;                    
                }

                if (!Aprobado)
                    motivoRechazo = Observaciones + Environment.NewLine + MotivoRechazo;

                if (Changed)
                    SaveHistorico();
                if (Changed)
                    Color = "azul";
                else
                {
                    if (Actividad.Estado != Constantes.CONST_EstadoAprobadoFA && Actividad.Estado != Constantes.CONST_EstadoRechazadoFA) // Si el estado es diferente a aprovado o rechazado por fiduciaria es la primera vez que se da repuesta.
                        Color = "azul";
                    else
                        Color = "gris";
                }

                db.SubmitChanges();

                //Enviar Notificacion si no estan aprobados

                if (!Aprobado)
                {
                    EnviarNotificacion(idPagoActividad, motivoRechazo);
                }
            }
        }

        [ContextStatic]
        protected Consultas consultas;

        private void EnviarNotificacion(string _Id_PagoActividad, string _Observaciones)
        {
            Datos.Consultas consultas = new Consultas();
            string txtSQL = "";
            //Envia Notificacion a Interventor
            txtSQL = "SELECT EmpresaInterventor.CodContacto, Empresa.CodProyecto " +
                             " FROM EmpresaInterventor " +
                             " INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa " +
                             " INNER JOIN PagoActividad ON Empresa.codproyecto = PagoActividad.CodProyecto " +
                             " WHERE (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ") " +
                             " and Inactivo = 0"+
                             " AND (PagoActividad.Id_PagoActividad = " + _Id_PagoActividad + ")";

            var rsInterventores = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow row_rsInterventores in rsInterventores.Rows)
            {
                AgendarTarea agenda = new AgendarTarea
                    (Int32.Parse(row_rsInterventores["CodContacto"].ToString()),
                    "Solicitud de Pago No. " + _Id_PagoActividad + " Rechazada por Fiduciaria",
                    "Se ha rechazado la Solicitud de pago No " + _Id_PagoActividad + ". <BR><BR>Observaciones Fiduciaria: " + _Observaciones,
                    row_rsInterventores["CodProyecto"].ToString(),
                    2,
                    "0",
                    true,
                    1,
                    true,
                    false,
                    Usuario.IdContacto,
                    null,
                    null,
                    ""); //"Firma Coordinador"

                agenda.Agendar();
            }

            rsInterventores = null;

            //Envia NOtificacion a Emprendedor
            txtSQL = " SELECT ProyectoContacto.CodContacto, ProyectoContacto.CodProyecto " +
                     " FROM PagoActividad " +
                     " INNER JOIN ProyectoContacto ON PagoActividad.CodProyecto = ProyectoContacto.CodProyecto " +
                     " WHERE (PagoActividad.Id_PagoActividad = " + _Id_PagoActividad + ") " +
                     " AND (ProyectoContacto.Inactivo = 0) " +
                     " AND (dbo.ProyectoContacto.CodRol = " + Constantes.CONST_RolEmprendedor + ")";

            var rsEmprendedores = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow row_rsEmprendedores in rsEmprendedores.Rows)
            {
                AgendarTarea agenda = new AgendarTarea
                    (Int32.Parse(row_rsEmprendedores["CodContacto"].ToString()),
                    "Solicitud de Pago No. " + _Id_PagoActividad + " Rechazada por Fiduciaria",
                    "Se ha rechazado la Solicitud de pago No " + _Id_PagoActividad + ". </br></br>Observaciones Fiduciaria: " + _Observaciones,
                    row_rsEmprendedores["CodProyecto"].ToString(),
                    2,
                    "0",
                    true,
                    1,
                    true,
                    false,
                    Usuario.IdContacto,
                    null,
                    null,
                    ""); //"Firma Coordinador"

                agenda.Agendar();
            }

            rsEmprendedores = null;

        }

        public void SaveHistorico()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = new PagoActividadHistorico();

                entity.CodPagoActividad = Actividad.Id_PagoActividad;
                entity.Estado = Actividad.Estado.Value;
                entity.ObservacionesFA = Actividad.ObservacionesFA;
                entity.FechaRtaFA = Actividad.FechaRtaFA;
                entity.valorretefuente = Actividad.valorretefuente;
                entity.valorreteiva = Actividad.valorreteiva;
                entity.valorreteica = Actividad.valorreteica;
                entity.otrosdescuentos = Actividad.otrosdescuentos;
                entity.codigopago = Actividad.codigopago;
                entity.valorpagado = Actividad.valorpagado;
                entity.fechapago = Actividad.fechapago;
                entity.fecharechazo = Actividad.fecharechazo;
                entity.ObservacionCambio = Actividad.ObservacionCambio;
                entity.fechaRegistro = DateTime.Now;

                db.PagoActividadHistorico.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }
    }

    /// <summary>
    /// Ayudantes para extraer de manera sencilla con metodos de extención los valores del excel de respuestas de pagos.
    /// By @marztres
    /// </summary>
    public static class HelperPagos
    {
        public static string GetString(this DataRow item, int index)
        {
            if ((index + 1) > item.ItemArray.Count())
                return string.Empty;
            if (item.ItemArray[index] != DBNull.Value)
                return item.ItemArray[index].ToString();
            else
                return string.Empty;
        }

        public static int? GetInt(this DataRow item, int index)
        {
            if ((index + 1) > item.ItemArray.Count())
                return null;
            if (item.ItemArray[index] != DBNull.Value)
                return Convert.ToInt32(item.ItemArray[index]);
            else
                return null;
        }

        public static DateTime? GetDate(this DataRow item, int index)
        {
            if ((index + 1) > item.ItemArray.Count())
                return null;
            if (item.ItemArray[index] != DBNull.Value)
                return Convert.ToDateTime(item.ItemArray[index]);
            else
                return null;
        }

        public static Decimal? GetDecimal(this DataRow item, int index)
        {
            if ((index + 1) > item.ItemArray.Count())
                return null;
            if (item.ItemArray[index] != DBNull.Value)
                return Convert.ToDecimal(item.ItemArray[index]);
            else
                return null;
        }

        public static string GetNombreUnicoArchivo(this string nombreArchivo, string extensionArchivo)
        {
            string anio = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string dia = DateTime.Now.Day.ToString();
            string hora = DateTime.Now.Hour.ToString();
            string minuto = DateTime.Now.Minute.ToString();
            string segundo = DateTime.Now.Second.ToString();
            string milesima = DateTime.Now.Millisecond.ToString();

            return nombreArchivo + dia + mes + anio + hora + minuto + segundo + milesima + extensionArchivo;
        }
    }
}