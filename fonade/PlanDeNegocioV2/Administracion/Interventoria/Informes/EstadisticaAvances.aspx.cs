using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reportes;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Informes
{
    public partial class EstadisticaAvances : Negocio.Base_Page
    {
        public const int PAGE_SIZE = 10;
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
            var interventores = estadisticasPagosBLL.getListadoInterventores(codContacto, usuario.CodOperador, Datos.Constantes.const_EstadisticaAvance);
            interventores.Find(x => x.codInterventor == 0).nomInterventor = "Seleccione un Interventor";
            llenarDropInterventores(interventores);
        }

        EstadisticasPagosBLL estadisticasPagosBLL = new EstadisticasPagosBLL();

        

        private void llenarDropInterventores(List<InterventoresActivosModel> _listInter)
        {
            ddlInterventores.DataSource = _listInter;
            ddlInterventores.DataTextField = "nomInterventor";
            ddlInterventores.DataValueField = "codInterventor";
            ddlInterventores.DataBind();
        }

        private void cargarGrilla(int _codGrupo, int numIndex, int _codInterventor, bool _nuevaBusqueda)
        {
            int codContacto = 0;
            List<EstadisticasAvancesModel> estadistica = new List<EstadisticasAvancesModel>();

            if (_codGrupo == Datos.Constantes.CONST_CoordinadorInterventor) //Si es coordinador solo cargan los interventores de el
            {
                codContacto = usuario.IdContacto;
            }

            if (_nuevaBusqueda)
            {
                estadistica = estadisticasPagosBLL.GetEstadisticasAvances(_codInterventor);

                Session["Estadistica"] = estadistica;
            }
            else
            {
                estadistica = (List<EstadisticasAvancesModel>)Session["Estadistica"];
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
            cargarGrilla(usuario.CodGrupo, _index, Convert.ToInt32(ddlInterventores.SelectedValue), _nuevaBusqueda);
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            List<EstadisticasAvancesModel> estadistica = new List<EstadisticasAvancesModel>();
            if (Session["Estadistica"] == null)
            {
                Alert("No se ha realizado la busqueda.");
            }
            else
            {
                estadistica = (List<EstadisticasAvancesModel>)Session["Estadistica"];
                //ExportToExcel(estadistica);
                ExportarExcel(estadistica);
            }
        }

        private void ExportarExcel(List<EstadisticasAvancesModel> estadisticasAvances)
        {
            using (var excelPackage = new ExcelPackage())
            {
                //Propiedades del archivo
                excelPackage.Workbook.Properties.Author = "FondoEmprender";
                excelPackage.Workbook.Properties.Title = "Estadistica Avances";
                //Propiedades Hoja de excel
                var sheet = excelPackage.Workbook.Worksheets.Add("Estadistica Avances");
                sheet.Name = "Estadistica Avances";
                //Empezamos a escribir sobre ella.
                var rowindex = 1;
                //Hago un Merge de primeras 4 columnas para poner el titulo.
                sheet.Cells[1, 1].Value = "Estadistica Avances";
                //sheet.Cells[1, 1, 1, 4].Merge = true;
                //Se puede poner un comentario en una celda
                //sheet.Cells[1, 1].AddComment("Listado de recibos", "Masanasa");
                //rowindex = 3;
                //Pongo los encabezados del excel
                var col = 1;
                sheet.Cells[rowindex, col++].Value = "ID Proyecto";
                sheet.Cells[rowindex, col++].Value = "Nombre Actividad";
                sheet.Cells[rowindex, col++].Value = "Item";
                sheet.Cells[rowindex, col++].Value = "Mes";
                sheet.Cells[rowindex, col++].Value = "Fecha Avance";
                sheet.Cells[rowindex, col++].Value = "Observaciones Emprendedor";
                sheet.Cells[rowindex, col++].Value = "Fecha Aprobacion";
                sheet.Cells[rowindex, col++].Value = "Observaciones Interventor";
                sheet.Cells[rowindex, col++].Value = "Aprobada";
                sheet.Cells[rowindex, col++].Value = "Interventor";
                sheet.Cells[rowindex, col++].Value = "Entidad";
                sheet.Cells[rowindex, col++].Value = "Operador";
                rowindex = 2;

                //Recorro los recibos y los ponemos en el Excel
                foreach (var r in estadisticasAvances)
                {
                    col = 1;
                    sheet.Cells[rowindex, col++].Value = r.idProyecto;
                    sheet.Cells[rowindex, col++].Value = r.nomActividad;
                    sheet.Cells[rowindex, col++].Value = r.item;
                    sheet.Cells[rowindex, col++].Value = r.mes;
                    sheet.Cells[rowindex, col++].Value = r.fechaAvanceEmprendedor.HasValue ?
                                                          r.fechaAvanceEmprendedor.Value.Date.ToShortDateString()
                                                          : "";
                    sheet.Cells[rowindex, col++].Value = r.observacionesEmprendedor;
                    sheet.Cells[rowindex, col++].Value = r.fechaAprobacionInterventor.HasValue ?
                                                          r.fechaAprobacionInterventor.Value.Date.ToShortDateString()
                                                          : "";                    
                    sheet.Cells[rowindex, col++].Value = r.observacionesInterventor;
                    sheet.Cells[rowindex, col++].Value = r.Aprobada;
                    sheet.Cells[rowindex, col++].Value = r.nomInterventor;
                    sheet.Cells[rowindex, col++].Value = r.nomEntidad;
                    sheet.Cells[rowindex, col++].Value = r.nomOperador;
                    rowindex++;
                }

                // Ancho de celdas
                //sheet.Cells.AutoFitColumns();

                //Establezco diseño al excel utilizando un diseño predefinido
                var range = sheet.Cells[1, 1, rowindex, 12];
                var table = sheet.Tables.Add(range, "tabla");
                table.TableStyle = TableStyles.Dark9;

                //Ya lo tengo ahora lo devuelvo utilizo el Response porque es Web, sino puedes guardarlo directamente
                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment;filename=EstadisticaAvances.xlsx");
                Response.ContentType = "application/excel";
                Response.Flush();
                Response.End();
            }
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }
    }
}