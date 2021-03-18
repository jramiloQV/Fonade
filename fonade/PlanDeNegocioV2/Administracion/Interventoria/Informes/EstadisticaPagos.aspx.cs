using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reportes;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Informes
{
    public partial class EstadisticaPagos : Negocio.Base_Page
    {

        public void ValidateUsers()
        {
            try
            {
                if (!(usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema
                    || usuario.CodGrupo == Datos.Constantes.CONST_CoordinadorInterventor
                    || usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExportar);
            if (!IsPostBack)
            {
                ValidateUsers();
                cargarComboInterventores(usuario.CodGrupo);
                //cargarGrilla(usuario.CodGrupo);
            }
        }

        private void cargarComboInterventores(int _codGrupo)
        {
            int codContacto = 0;
            if (_codGrupo == Datos.Constantes.CONST_CoordinadorInterventor) //Si es coordinador solo cargan los interventores de el
            {
                codContacto = usuario.IdContacto;
            }
            var interventores = estadisticasPagosBLL.getListadoInterventores(codContacto, usuario.CodOperador, Datos.Constantes.const_EstadisticaPagos);
            llenarDropInterventores(interventores);
        }

        private void llenarDropInterventores(List<InterventoresActivosModel> _listInter)
        {
            ddlInterventores.DataSource = _listInter;
            ddlInterventores.DataTextField = "nomInterventor";
            ddlInterventores.DataValueField = "codInterventor";
            ddlInterventores.DataBind();
        }

        EstadisticasPagosBLL estadisticasPagosBLL = new EstadisticasPagosBLL();

        public const int PAGE_SIZE = 10;

        private void cargarGrilla(int _codGrupo, int numIndex, int _codInterventor
            , DateTime? _fechaInic, DateTime? _fechaFin, bool _nuevaBusqueda)
        {
            int codContacto = 0;
            List<EstadisticasPagosModel> estadistica = new List<EstadisticasPagosModel>();

            if (_codGrupo == Datos.Constantes.CONST_CoordinadorInterventor) //Si es coordinador solo cargan los interventores de el
            {
                codContacto = usuario.IdContacto;
            }

            if (_nuevaBusqueda)
            {
                estadistica = estadisticasPagosBLL.GetEstadisticasPagos(codContacto, _codInterventor
                                                                            , _fechaInic, _fechaFin, usuario.CodOperador);

                Session["Estadistica"] = estadistica;
            }
            else
            {
                estadistica = (List<EstadisticasPagosModel>)Session["Estadistica"];
            }



            lblCantReg.Text = "Registros Encontrados: " + estadistica.Count;

            gvResultado.DataSource = estadistica;
            gvResultado.PageIndex = numIndex;
            gvResultado.DataBind();
        }


        protected void gvResultado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ejecutarCargaGrilla(e.NewPageIndex, false); //False no hay nueva busqueda
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            ejecutarCargaGrilla(0, true); //True Nueva busqueda
        }

        private void ejecutarCargaGrilla(int _index, bool _nuevaBusqueda)
        {
            DateTime? FechaInicial = null;
            DateTime? FechaFinal = null;

            if (txtFechaIni.Text != "")
            {
                FechaInicial = Convert.ToDateTime(txtFechaIni.Text);
            }
            if (txtFechaFin.Text != "")
            {
                FechaFinal = Convert.ToDateTime(txtFechaFin.Text);
            }

            cargarGrilla(usuario.CodGrupo, _index, Convert.ToInt32(ddlInterventores.SelectedValue)
               , FechaInicial, FechaFinal, _nuevaBusqueda);
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            List<EstadisticasPagosModel> estadistica = new List<EstadisticasPagosModel>();
            if (Session["Estadistica"] == null)
            {
                Alert("No se ha realizado la busqueda.");
            }
            else
            {
                estadistica = (List<EstadisticasPagosModel>)Session["Estadistica"];
                if (usuario.CodOperador != null)
                {
                    estadistica = estadistica.Where(x => x.codOperador == usuario.CodOperador).ToList();
                }
                                
                //ExportToExcel(estadistica);
                ExportarExcel(estadistica);
            }
        }

        private void ExportarExcel(List<EstadisticasPagosModel> estadisticasPagos)
        {
            using (var excelPackage = new ExcelPackage())
            {
                //Propiedades del archivo
                excelPackage.Workbook.Properties.Author = "FondoEmprender";
                excelPackage.Workbook.Properties.Title = "Estadistica Pagos";
                //Propiedades Hoja de excel
                var sheet = excelPackage.Workbook.Worksheets.Add("Estadistica Pagos");
                sheet.Name = "Estadistica Pagos";
                //Empezamos a escribir sobre ella.
                var rowindex = 1;
                //Hago un Merge de primeras 4 columnas para poner el titulo.
                sheet.Cells[1, 1].Value = "Estadistica Pagos";
                //sheet.Cells[1, 1, 1, 4].Merge = true;
                //Se puede poner un comentario en una celda
                //sheet.Cells[1, 1].AddComment("Listado de recibos", "Masanasa");
                //rowindex = 3;
                //Pongo los encabezados del excel
                var col = 1;
                sheet.Cells[rowindex, col++].Value = "ID Proyecto";
                sheet.Cells[rowindex, col++].Value = "Nombre Proyecto";
                sheet.Cells[rowindex, col++].Value = "Nombre Interventor";
                sheet.Cells[rowindex, col++].Value = "Fecha Aprob Interventor";
                sheet.Cells[rowindex, col++].Value = "Fecha Aprob Coordinador Inter";
                sheet.Cells[rowindex, col++].Value = "Fecha Respuesta Fiduciaria";
                sheet.Cells[rowindex, col++].Value = "Cod Solicitud";
                sheet.Cells[rowindex, col++].Value = "Nombre Pago";
                sheet.Cells[rowindex, col++].Value = "Cantidad Dinero";
                sheet.Cells[rowindex, col++].Value = "Estado";
                sheet.Cells[rowindex, col++].Value = "Observacion Fiduciaria o Coordinacion";
                sheet.Cells[rowindex, col++].Value = "Operador";
                rowindex = 2;

                //Recorro los recibos y los ponemos en el Excel
                foreach (var r in estadisticasPagos)
                {
                    col = 1;
                    sheet.Cells[rowindex, col++].Value = r.idProyecto;
                    sheet.Cells[rowindex, col++].Value = r.nomProyecto;
                    sheet.Cells[rowindex, col++].Value = r.nombreInterventor;
                    sheet.Cells[rowindex, col++].Value = r.fechaAprobInterventor.HasValue ?
                                                          r.fechaAprobInterventor.Value.Date.ToShortDateString()
                                                          : "";
                    sheet.Cells[rowindex, col++].Value = r.fechaAprobORechaCoordinador.HasValue ?
                                                          r.fechaAprobORechaCoordinador.Value.Date.ToShortDateString()
                                                          : "";
                    sheet.Cells[rowindex, col++].Value = r.fechaRespuestaFiducia.HasValue ?
                                                          r.fechaRespuestaFiducia.Value.Date.ToShortDateString()
                                                          : "";
                    sheet.Cells[rowindex, col++].Value = r.idPagoActividad;
                    sheet.Cells[rowindex, col++].Value = r.nomPagoActividad;
                    sheet.Cells[rowindex, col++].Value = r.cantidadDinero;
                    sheet.Cells[rowindex, col++].Value = r.estado;
                    sheet.Cells[rowindex, col++].Value = r.observacionFiduciaOCoordinador;
                    sheet.Cells[rowindex, col++].Value = r.Operador;
                    rowindex++;
                }

                // Ancho de celdas
                sheet.Cells.AutoFitColumns();

                //Establezco diseño al excel utilizando un diseño predefinido
                var range = sheet.Cells[1, 1, rowindex,12];
                var table = sheet.Tables.Add(range, "tabla");
                table.TableStyle = TableStyles.Dark9;

                //Ya lo tengo ahora lo devuelvo utilizo el Response porque es Web, sino puedes guardarlo directamente
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment;filename=EstadisticaPagos.xlsx");
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();
            }
        }

        public void ExportToExcel<T>(List<T> myList)
        {
            string fileName = "MyFilename.xlsx";

            DataGrid dg = new DataGrid();
            dg.AllowPaging = false;
            dg.DataSource = myList;
            dg.DataBind();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.AddHeader("Content-Disposition",
              "attachment; filename=" + fileName);

            HttpContext.Current.Response.ContentType =
              "application/vnd.ms-excel";
            System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlTextWriter =
              new System.Web.UI.HtmlTextWriter(stringWriter);
            dg.RenderControl(htmlTextWriter);
            System.Web.HttpContext.Current.Response.Write(stringWriter.ToString());
            System.Web.HttpContext.Current.Response.End();
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }
    }
}