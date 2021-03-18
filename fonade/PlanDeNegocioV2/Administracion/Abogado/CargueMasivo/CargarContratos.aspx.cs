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

namespace Fonade.PlanDeNegocioV2.Administracion.Abogado.CargueMasivo
{
    public partial class CargarContratos : Negocio.Base_Page
    {
        protected FonadeUser Usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnsubir_Click(object sender, EventArgs e)
        {
            try
            {
                var rutaArchivo = SubirRespuestaDePagos();
                var excelPago = ProcesarArchivoExcel(rutaArchivo);

                if (excelPago.Columns.Count < 15)
                    throw new ApplicationException("El archivo excel no tiene las columnas necesarias para continuar, debe tener 14 columnas");

                var respuestasPago = new List<RespuestaPago>();
                var index = 1;
                var hasErrors = false;
                var hasSuccess = false;
                foreach (DataRow excelRow in excelPago.Rows)
                {
                    int? codigoProyecto;
                    RespuestaPago respuestaPago = new RespuestaPago(excelRow, index, out codigoProyecto, Usuario.IdContacto);

                    var countDuplicados = (from myRow in excelPago.AsEnumerable()
                                           where myRow.Field<Double?>("CodigoProyecto") == (double)codigoProyecto.GetValueOrDefault(0)
                                           select myRow).Count();

                    //var countDuplicados = (from myRow in excelPago.AsEnumerable()
                    //               where myRow.Field<Double?>("idProyecto") == (double)codigoProyecto.GetValueOrDefault(0)
                    //               select myRow).Count();

                    respuestaPago.Procesar(countDuplicados, Usuario.CodOperador);

                    respuestasPago.Add(respuestaPago);

                    if (respuestaPago.Color.Equals("#FFCCCC") || respuestaPago.Color.Equals("#FFFF66"))
                        hasErrors = true;
                    else if (respuestaPago.Color.Equals("#99FFFF"))
                        hasSuccess = true;

                    index++;
                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.InsertLog(respuestaPago.ToString());
                }

                if (hasErrors)
                {
                    if (hasSuccess)
                        partialUpload.Visible = true;
                    else
                        errorUpload.Visible = true;
                }
                else if (hasSuccess)
                {
                    successUpload.Visible = true;
                }

                gvMain.DataSource = respuestasPago;
                gvMain.DataBind();

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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al procesar el archivi, verifique si estan todas las columnas completas e intentelo de nuevo. detalle : " + ex.Message.Replace("'", string.Empty) + " ');", true);
            }
        }

        protected void grvResumen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var color = gvMain.DataKeys[e.Row.RowIndex].Values[0].ToString();
                e.Row.BackColor = Color.FromName(color);
            }
        }

        protected string SubirRespuestaDePagos()
        {
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string directorioDestino = "CargueInfoContratos\\";
            StringBuilder querySql = new StringBuilder();
            string nombreArchivo = "RespuestaPagos".GetNombreUnicoArchivo(System.IO.Path.GetExtension(archivoPagos.PostedFile.FileName));

            // ¿ Es valido el archivo ?
            if (!archivoPagos.HasFile)
                throw new ApplicationException("Archivo invalido");
            // ¿ Es Excel Valido ? 
            if (archivoPagos.PostedFile.ContentType != "application/vnd.ms-excel" && archivoPagos.PostedFile.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                throw new ApplicationException("Adjunte un archivo Excel con extensión .xls, los demas tipos son invalidos.");
            //¿ Nombre archivo empty?
            if (string.IsNullOrEmpty(archivoPagos.FileName) || !System.IO.Path.GetExtension(archivoPagos.PostedFile.FileName).Contains(".xls"))
                throw new ApplicationException("Nombre de archivo invalido' ");

            return UploadFileToServer(archivoPagos, directorioBase, directorioDestino, nombreArchivo);
        }

        protected DataTable ProcesarArchivoExcel(string rutaArchivo)
        {
            string extencion = System.IO.Path.GetExtension(rutaArchivo);
            string proveedorExcel = extencion == ".xlsx" ? @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + rutaArchivo + "; Extended Properties=Excel 12.0;" : @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + rutaArchivo + "; Extended Properties='Excel 8.0;HDR=YES;IMEX=1;ImportMixedTypes=Text'";

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
            //        using (var oleDbCommand = new OleDbCommand("select * from [Hoja1$]", oleDbConnection))
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
        public string Indice { get; set; }
        public int? CodigoProyecto { get; set; }
        public string NumeroContrato { get; set; }
        public string ObjetoContrato { get; set; }
        public string PlazoContratoMeses { get; set; }
        public string NumeroApPresupuestal { get; set; }
        public string NumeroActaConsejoDirectivo { get; set; }
        public decimal? ValorInicialPesos { get; set; }
        public decimal? ValorEnte { get; set; }
        public decimal? ValorSena { get; set; }
        public DateTime? FechaAP { get; set; }
        public string EstadoContrato { get; set; }
        public string TipoContrato { get; set; }
        public DateTime? FechaConsejoDirectivo { get; set; }
        public DateTime? FechaCertificadoDisponibilidad { get; set; }
        public string CertificadoDisponibilidad { get; set; }
        public int IdUsuario { get; set; }
        private string _color;
        public string Color
        {
            get
            {
                if (_color.Equals("amarillo"))
                    return "#FFFF66";
                if (_color.Equals("rojo"))
                    return "#FFCCCC";
                if (_color.Equals("azul"))
                    return "#99FFFF";
                return "#99FFFF";
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
        public RespuestaPago(DataRow rowExcel, int index, out int? idProyecto, int idUsuario)
        {
            Indice = index.ToString();
            CodigoProyecto = rowExcel.GetInt(0);
            NumeroContrato = rowExcel.GetString(1);
            ObjetoContrato = rowExcel.GetString(2);
            FechaAP = rowExcel.GetDate(3);
            ValorInicialPesos = rowExcel.GetDecimal(4);
            PlazoContratoMeses = rowExcel.GetString(5);
            NumeroApPresupuestal = rowExcel.GetString(6);
            NumeroActaConsejoDirectivo = rowExcel.GetString(7);
            FechaConsejoDirectivo = rowExcel.GetDate(8);
            ValorEnte = rowExcel.GetDecimal(9);
            ValorSena = rowExcel.GetDecimal(10);
            CertificadoDisponibilidad = rowExcel.GetString(11);
            FechaCertificadoDisponibilidad = rowExcel.GetDate(12);
            EstadoContrato = rowExcel.GetString(13);
            TipoContrato = rowExcel.GetString(14);
            idProyecto = rowExcel.GetInt(0);
            IdUsuario = idUsuario;
        }

        public void Procesar(int countDuplicados, int? codOperador)
        {
            try
            {
                if (countDuplicados > 1)
                    throw new Exception("Registro duplicado, no se guardo información, rectifique el archivo excel.");

                CheckNull();

                if (CodigoProyecto == 0)
                    throw new Exception("El código de proyecto no puede ser 0.");

                FieldValidate.ValidateNumeric("Número de contrato", NumeroContrato, true, int.MaxValue);
                FieldValidate.ValidateString("Número de contrato", NumeroContrato, true, 10);
                FieldValidate.ValidateString("Objeto de contrato", ObjetoContrato, true, 255);
                FieldValidate.ValidateNumeric("Plazo de contrato en meses", PlazoContratoMeses, true, int.MaxValue);
                FieldValidate.ValidateString("Plazo de contrato en meses", PlazoContratoMeses, true, 2);
                FieldValidate.ValidateNumeric("Número AP presupuestal", NumeroApPresupuestal, true, int.MaxValue);
                FieldValidate.ValidateString("Número AP presupuestal", NumeroApPresupuestal, true, 10);
                FieldValidate.ValidateNumeric("Número de acta de consejo directivo", NumeroActaConsejoDirectivo, true, int.MaxValue);
                FieldValidate.ValidateString("Número de acta de consejo directivo", NumeroActaConsejoDirectivo, true, 10);
                FieldValidate.ValidateNumeric("Certificado de disponibilidad", CertificadoDisponibilidad, true, int.MaxValue);
                FieldValidate.ValidateString("Certificado de disponibilidad", CertificadoDisponibilidad, true, 5);
                FieldValidate.ValidateString("Estado del contrato", EstadoContrato, true);
                FieldValidate.ValidateString("Valor inicial en pesos", ValorInicialPesos.ToString(), true, 15);
                FieldValidate.ValidateNumeric("Valor inicial en pesos", ValorInicialPesos.ToString(), true, Int64.MaxValue);
                FieldValidate.ValidateString("Valor Ente", ValorEnte.ToString(), true, 15);
                FieldValidate.ValidateNumeric("Valor Ente", ValorEnte.ToString(), true, Int64.MaxValue);
                FieldValidate.ValidateString("Valor Sena", ValorSena.ToString(), true, 15);
                FieldValidate.ValidateNumeric("Valor Sena", ValorSena.ToString(), true, Int64.MaxValue);
                FieldValidate.ValidateString("Tipo contrato", TipoContrato, true, 150);

                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ProyectoExist(CodigoProyecto.GetValueOrDefault()))
                    throw new ApplicationException("El proyecto no existe en el sistema.");

                var estado = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getEstadoProyecto(CodigoProyecto.GetValueOrDefault());
                if (estado != Constantes.CONST_Ejecucion)
                    throw new ApplicationException("El proyecto no esta en estado ejecución.");

                string[] estadosContrato = new string[] { "Condonado", "Terminado", "No condonado", "En evaluación de indicadores", "Liquidados", "Con ejecución de recursos", "Sin ejecución de Recursos", "Legalización" };

                if (!estadosContrato.Contains(EstadoContrato.Trim(), StringComparer.InvariantCultureIgnoreCase))
                    throw new ApplicationException("Estado invalido, solo es permitido: Condonado,Terminado,No condonado,En evaluación de indicadores,Liquidados,Con ejecución de recursos,Sin ejecución de Recursos,Legalización");

                if (codOperador != null)
                {
                    if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ProyectoOperador(CodigoProyecto.GetValueOrDefault(), codOperador))
                        throw new ApplicationException("El proyecto a cargar no está asociado a su operador.");
                }

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.ExistContrato(NumeroContrato, CodigoProyecto.GetValueOrDefault()))
                    throw new ApplicationException("El número contrato ya existe para otro proyecto.");

                Save();
            }
            catch (ApplicationException ex)
            {
                Color = "amarillo";
                MensajeSistema = ex.Message;
            }
            catch (Exception ex)
            {
                Color = "rojo";
                MensajeSistema = "Error : " + ex.Message;
            }
        }

        public void CheckNull()
        {
            if (CodigoProyecto == null)
                throw new Exception("Código de proyecto invalido.");
            if (NumeroContrato == null)
                throw new Exception("Numero de contrato invalido.");
            if (ObjetoContrato == null)
                throw new Exception("Objeto de contrato invalido.");
            if (FechaAP == null)
                throw new Exception("Fecha AP invalido");
            if (ValorInicialPesos == null)
                throw new Exception("Valor inicial en pesos invalido.");
            if (PlazoContratoMeses == null)
                throw new Exception("Plazo de contrato en meses invalido.");
            if (NumeroApPresupuestal == null)
                throw new Exception("Número AP presupuestal invalido.");
            if (NumeroActaConsejoDirectivo == null)
                throw new Exception("Número de acta de consejo directivo invalida");
            if (FechaConsejoDirectivo == null)
                throw new Exception("Fecha consejo directivo invalido.");
            if (ValorEnte == null)
                throw new Exception("Valor ente invalido.");
            if (ValorSena == null)
                throw new Exception("Valor Sena invalido.");
            if (CertificadoDisponibilidad == null)
                throw new Exception("Certificado de disponibilidad invalido.");
            if (FechaCertificadoDisponibilidad == null)
                throw new Exception("Fecha de certificado de disponibilidad invalido.");
            if (EstadoContrato == null)
                throw new Exception("Estado invalido.");
            if (TipoContrato == null)
                throw new Exception("Tipo de contrato invalido.");
        }

        public void Save()
        {
            var currentEntity = new Datos.ContratoEmpresa
            {
                NumeroContrato = NumeroContrato,
                ObjetoContrato = ObjetoContrato,
                FechaAP = FechaAP,
                ValorInicialEnPesos = ValorInicialPesos,
                PlazoContratoMeses = Convert.ToByte(PlazoContratoMeses),
                NumeroAPContrato = Convert.ToInt32(NumeroApPresupuestal),
                NumeroActaConcejoDirectivo = Convert.ToInt32(NumeroActaConsejoDirectivo),
                FechaActaConcejoDirectivo = FechaConsejoDirectivo,
                ValorEnte = ValorEnte,
                Valorsena = ValorSena,
                CertificadoDisponibilidad = Convert.ToInt32(CertificadoDisponibilidad),
                FechaCertificadoDisponibilidad = FechaCertificadoDisponibilidad,
                Estado = EstadoContrato,
                CodEmpresa = CodigoProyecto.GetValueOrDefault(0), //CodEmpresa = CodigoProyecto - En la consulta del insert se busca por proyecto no por empresa.
                TipoContrato = TipoContrato
            };

            Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.Insert(currentEntity);
            Color = "azul";
            MensajeSistema = "Actualizado correctamente.";
        }

        public override string ToString()
        {
            return String.Format("CodigoProyecto:{0},NumeroContrato:{1},ObjetoContrato:{2},FechaAP:{3},ValorInicialPesos:{4},PlazoContratoMeses:{5},NumeroApPresupuestal:{6},NumeroActaConsejoDirectivo:{7},FechaConsejoDirectivo:{8},ValorEnte:{8},ValorSena:{9},CertificadoDisponibilidad:{10},FechaCertificadoDisponibilidad:{11},EstadoContrato:{12},TipoContrato:{13},IdUsuario:{14}", CodigoProyecto, NumeroContrato, ObjetoContrato, FechaAP, ValorInicialPesos, PlazoContratoMeses, NumeroApPresupuestal, NumeroActaConsejoDirectivo, FechaConsejoDirectivo, ValorEnte, ValorSena, CertificadoDisponibilidad, FechaCertificadoDisponibilidad, EstadoContrato, TipoContrato, IdUsuario);
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
            try
            {
                if ((index + 1) > item.ItemArray.Count())
                    return string.Empty;
                if (item.ItemArray[index] != DBNull.Value)
                    return item.ItemArray[index].ToString();
                else
                    return string.Empty;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static int? GetInt(this DataRow item, int index)
        {
            try
            {
                if ((index + 1) > item.ItemArray.Count())
                    return null;
                if (item.ItemArray[index] != DBNull.Value)
                    return Convert.ToInt32(item.ItemArray[index]);
                else
                    return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public static DateTime? GetDate(this DataRow item, int index)
        {
            try
            {
                if ((index + 1) > item.ItemArray.Count())
                    return null;
                if (item.ItemArray[index] != DBNull.Value)
                    return Convert.ToDateTime(item.ItemArray[index]);
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Decimal? GetDecimal(this DataRow item, int index)
        {
            try
            {
                if ((index + 1) > item.ItemArray.Count())
                    return null;
                if (item.ItemArray[index] != DBNull.Value)
                    return Convert.ToDecimal(item.ItemArray[index].ToString().Replace(",", "").Replace(".", ","));
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
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