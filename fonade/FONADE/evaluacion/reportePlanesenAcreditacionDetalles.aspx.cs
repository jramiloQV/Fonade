using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.evaluacion
{
    public partial class reportePlanesenAcreditacionDetalles : System.Web.UI.Page
    {
        DataTable datatable;

        String CodProyecto = "";
        String prAcreditado = "";

        String CodProyecto2 = "";
        String NomProyecto = "";

        String Acreditador = "";
        String Fecha = "";
        String Ciudad = "";
        String Departamento = "";
        String Institucion = "";
        String Unidad = "";
        String Sector = "";
        String SubSector = "";
        String Recursos = "";
        String ObservacionFinal = "";

        String Emprendedor = "";
        String ObsPendiente = "";
        String ObsNoAcreditado = "";

        Table tabla;
        TableRow filaN;

        //Table tablaExcel;
        //TableRow filaNExcel;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.ContentType = "";
            //Response.AddHeader("content-disposition", "inline; filename=Consulta.xls");
            try
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Session["idNombreConvocatoriaS"].ToString()))
                {
                    Response.Redirect("reportePlanesenAcreditacion.aspx");
                }
                else
                {
                    L_NomConvocatoria.Text = HttpContext.Current.Session["idNombreConvocatoriaS"].ToString();

                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                    SqlCommand cmd = new SqlCommand("select * from convocatoria where Id_Convocatoria = " + HttpContext.Current.Session["idConvocatoriaEvalS"].ToString(), conn);

                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            L_Recursos.Text = reader["Presupuesto"].ToString();
                        }
                        reader.Close();
                    }
                    catch (SqlException)
                    {
                    }
                    finally
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch(Exception)
            {
                Response.Redirect("reportePlanesenAcreditacion.aspx");
            }

            llenarDatatable();

            crearTabla();
        }

        private void llenarDatatable()
        {
            datatable = new DataTable();

            datatable.Columns.Add("CodConvocatoria");
            datatable.Columns.Add("NomConvocatoria");
            datatable.Columns.Add("CodProyecto");
            datatable.Columns.Add("NomProyecto");
            datatable.Columns.Add("Acreditador");
            datatable.Columns.Add("NombreAcreditador");
            datatable.Columns.Add("ApellidosAcreditado");
            datatable.Columns.Add("FechaAcreditacion");
            datatable.Columns.Add("ObservacionFinal");
            datatable.Columns.Add("Cod_DeEstado");
            datatable.Columns.Add("NombreEmprendedor");
            datatable.Columns.Add("ApellidosEmprendedor");
            datatable.Columns.Add("NomInstitucion");
            datatable.Columns.Add("NomUnidad");
            datatable.Columns.Add("NomCiudad");
            datatable.Columns.Add("NomDepartamento");
            datatable.Columns.Add("NomSector");
            datatable.Columns.Add("NomSubSector");
            datatable.Columns.Add("Pendiente");
            datatable.Columns.Add("FechaPendiente");
            datatable.Columns.Add("ObservacionPendiente");
            datatable.Columns.Add("Subsanado");
            datatable.Columns.Add("ObservacionSubsanado");
            datatable.Columns.Add("FechaSubsanado");
            datatable.Columns.Add("Acreditado");
            datatable.Columns.Add("ObservacionAcreditado");
            datatable.Columns.Add("FechaAcreditado");
            datatable.Columns.Add("NoAcreditado");
            datatable.Columns.Add("ObservacionNoAcreditado");
            datatable.Columns.Add("FechaNoAcreditado");
            datatable.Columns.Add("Recursos");
            datatable.Columns.Add("CodEstado");
            datatable.Columns.Add("NomEstado");
            datatable.Columns.Add("Fecha");

            string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            using (var con = new SqlConnection(conexionStr))
            {
                using (var com = con.CreateCommand())
                {
                    com.CommandText = "MD_BuscarReportePlanesEnAcreditacion";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.AddWithValue("@_ConvocatoriaID", HttpContext.Current.Session["idConvocatoriaEvalS"].ToString());
                    
                    try
                    {
                        con.Open();

                        SqlDataReader reader = com.ExecuteReader();
                        while (reader.Read())
                        {
                            DataRow fila = datatable.NewRow();

                            fila["CodConvocatoria"] = reader["CodConvocatoria"].ToString();
                            fila["NomConvocatoria"] = reader["NomConvocatoria"].ToString();
                            fila["CodProyecto"] = reader["CodProyecto"].ToString();
                            fila["NomProyecto"] = reader["NomProyecto"].ToString();
                            fila["Acreditador"] = reader["Acreditador"].ToString();
                            fila["NombreAcreditador"] = reader["NombreAcreditador"].ToString();
                            fila["ApellidosAcreditado"] = reader["ApellidosAcreditado"].ToString();
                            fila["FechaAcreditacion"] = reader["FechaAcreditacion"].ToString();
                            fila["ObservacionFinal"] = reader["ObservacionFinal"].ToString();
                            fila["Cod_DeEstado"] = reader["Cod_DeEstado"].ToString();
                            fila["NombreEmprendedor"] = reader["NombreEmprendedor"].ToString();
                            fila["ApellidosEmprendedor"] = reader["ApellidosEmprendedor"].ToString();
                            fila["NomInstitucion"] = reader["NomInstitucion"].ToString();
                            fila["NomUnidad"] = reader["NomUnidad"].ToString();
                            fila["NomCiudad"] = reader["NomCiudad"].ToString();
                            fila["NomDepartamento"] = reader["NomDepartamento"].ToString();
                            fila["NomSector"] = reader["NomSector"].ToString();
                            fila["NomSubSector"] = reader["NomSubSector"].ToString();
                            fila["Pendiente"] = reader["Pendiente"].ToString();
                            fila["FechaPendiente"] = reader["FechaPendiente"].ToString();
                            fila["ObservacionPendiente"] = reader["ObservacionPendiente"].ToString();
                            fila["Subsanado"] = reader["Subsanado"].ToString();
                            fila["ObservacionSubsanado"] = reader["ObservacionSubsanado"].ToString();
                            fila["FechaSubsanado"] = reader["FechaSubsanado"].ToString();
                            fila["Acreditado"] = reader["Acreditado"].ToString();
                            fila["ObservacionAcreditado"] = reader["ObservacionAcreditado"].ToString();
                            fila["FechaAcreditado"] = reader["FechaAcreditado"].ToString();
                            fila["NoAcreditado"] = reader["NoAcreditado"].ToString();
                            fila["ObservacionNoAcreditado"] = reader["ObservacionNoAcreditado"].ToString();
                            fila["FechaNoAcreditado"] = reader["FechaNoAcreditado"].ToString();
                            fila["Recursos"] = reader["Recursos"].ToString();
                            fila["CodEstado"] = reader["CodEstado"].ToString();
                            fila["NomEstado"] = reader["NomEstado"].ToString();
                            fila["Fecha"] = reader["Fecha"].ToString();

                            datatable.Rows.Add(fila);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        com.Dispose();
                        con.Close();
                        con.Dispose();
                    }
                }
            }
        }

        private void crearTabla()
        {
            tabla = new Table();
            tabla.CssClass = "Grilla";

            titulos();

            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                filaN = new TableRow();

                if (!CodProyecto.Equals(datatable.Rows[i]["CodProyecto"].ToString()))
                {
                    CodProyecto = datatable.Rows[i]["CodProyecto"].ToString();

                    prAcreditado = datatable.Rows[i]["NomEstado"].ToString();

                    if (String.IsNullOrEmpty(datatable.Rows[i]["CodProyecto"].ToString())) { CodProyecto2 = ""; } else { CodProyecto2 = datatable.Rows[i]["CodProyecto"].ToString(); }

                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomProyecto"].ToString())) { NomProyecto = ""; } else { NomProyecto = datatable.Rows[i]["NomProyecto"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NombreAcreditador"].ToString())) { Acreditador = ""; } else { Acreditador = datatable.Rows[i]["NombreAcreditador"].ToString() + " " + datatable.Rows[i]["ApellidosAcreditado"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["Fecha"].ToString())) { Fecha = ""; } else { Fecha = datatable.Rows[i]["Fecha"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomCiudad"].ToString())) { Ciudad = ""; } else { Ciudad = datatable.Rows[i]["NomCiudad"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomDepartamento"].ToString())) { Departamento = ""; } else { Departamento = datatable.Rows[i]["NomDepartamento"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomInstitucion"].ToString())) { Institucion = ""; } else { Institucion = datatable.Rows[i]["NomInstitucion"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomUnidad"].ToString())) { Unidad = ""; } else { Unidad = datatable.Rows[i]["NomUnidad"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomSector"].ToString())) { Sector = ""; } else { Sector = datatable.Rows[i]["NomSector"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["NomSubSector"].ToString())) { SubSector = ""; } else { SubSector = datatable.Rows[i]["NomSubSector"].ToString(); }
                    if (String.IsNullOrEmpty(datatable.Rows[i]["Recursos"].ToString())) { Recursos = ""; } else { Recursos = datatable.Rows[i]["Recursos"].ToString(); }

                    if (String.IsNullOrEmpty(prAcreditado))
                    {
                        prAcreditado = "";
                    }

                    filaN.Cells.Add(celdaNormal(CodProyecto2, 1, ""));
                    filaN.Cells.Add(celdaNormal(NomProyecto, 1, ""));
                    filaN.Cells.Add(celdaNormal(Acreditador, 1, ""));
                    filaN.Cells.Add(celdaNormal(Fecha, 1, ""));
                    filaN.Cells.Add(celdaNormal(Ciudad, 1, ""));
                    filaN.Cells.Add(celdaNormal(Departamento, 1, ""));
                    filaN.Cells.Add(celdaNormal(Institucion, 1, ""));
                    filaN.Cells.Add(celdaNormal(Unidad, 1, ""));
                    filaN.Cells.Add(celdaNormal(Sector, 1, ""));
                    filaN.Cells.Add(celdaNormal(SubSector, 1, ""));
                    filaN.Cells.Add(celdaNormal(Recursos, 1, ""));

                    strColProyectoDetalleFila(i);

                    if (String.IsNullOrEmpty(prAcreditado))
                    {
                        filaN.Cells.Add(celdaNormal("[[desabilitado]]", 1, ""));
                    }
                    else
                    {
                        filaN.Cells.Add(celdaNormal(prAcreditado, 1, ""));
                    }

                    if (String.IsNullOrEmpty(prAcreditado))
                    {
                        ObservacionFinal = "";
                    }
                    else
                    {
                        ObservacionFinal = datatable.Rows[i]["ObservacionFinal"].ToString();
                    }

                    filaN.Cells.Add(celdaNormal(ObservacionFinal, 1, ""));
                }
                else
                {
                    filaN.Cells.Add(celdaNormal("", 11, ""));
                    strColProyectoDetalleFila(i);
                    filaN.Cells.Add(celdaNormal("", 2, ""));
                }
                tabla.Rows.Add(filaN);
            }

            P_Tabla.Controls.Add(tabla);
        }

        private void strColProyectoDetalleFila(Int32 i)
        {
            Emprendedor = datatable.Rows[i]["NombreEmprendedor"].ToString() + " " + datatable.Rows[i]["ApellidosEmprendedor"].ToString();

            if (String.IsNullOrEmpty(datatable.Rows[i]["ObservacionPendiente"].ToString()))
            {
                ObsPendiente = "";
            }
            else
            {
                ObsPendiente = datatable.Rows[i]["ObservacionPendiente"].ToString();
            }

            if (String.IsNullOrEmpty(datatable.Rows[i]["ObservacionNoAcreditado"].ToString()))
            {
                ObsNoAcreditado = "";
            }
            else
            {
                ObsNoAcreditado = datatable.Rows[i]["ObservacionNoAcreditado"].ToString();
            }

            CheckBox Asignado = new CheckBox();
            Asignado.Enabled = false;
            CheckBox Pendiente = new CheckBox();
            Pendiente.Enabled = false;
            CheckBox Subsanado = new CheckBox();
            Subsanado.Enabled = false;
            CheckBox Acreditado = new CheckBox();
            Acreditado.Enabled = false;
            CheckBox NoAcreditado = new CheckBox();
            NoAcreditado.Enabled = false;

            if(!String.IsNullOrEmpty(datatable.Rows[i]["Pendiente"].ToString()))
            {
                if(Boolean.Parse(datatable.Rows[i]["Pendiente"].ToString()))
                {
                    Pendiente.Checked = true;
                }else
                {
                    Pendiente.Checked = false;
                }
            }

            if(!String.IsNullOrEmpty(datatable.Rows[i]["Subsanado"].ToString()))
            {
                if(Boolean.Parse(datatable.Rows[i]["Subsanado"].ToString()))
                {
                    Subsanado.Checked = true;
                }else
                {
                    Subsanado.Checked = false;
                }
            }

            if (!String.IsNullOrEmpty(datatable.Rows[i]["Acreditado"].ToString()))
            {
                if (Boolean.Parse(datatable.Rows[i]["Acreditado"].ToString()))
                {
                    Acreditado.Checked = true;
                }
                else
                {
                    Acreditado.Checked = false;
                }
            }

            if (!String.IsNullOrEmpty(datatable.Rows[i]["NoAcreditado"].ToString()))
            {
                if (Boolean.Parse(datatable.Rows[i]["NoAcreditado"].ToString()))
                {
                    NoAcreditado.Checked = true;
                }
                else
                {
                    NoAcreditado.Checked = false;
                }
            }

            if(!NoAcreditado.Checked && !Pendiente.Checked && !Acreditado.Checked && !Subsanado.Checked)
            {
                Asignado.Checked = true;
            }
            else
            {
                Asignado.Checked = false;
            }

            filaN.Cells.Add(celdaNormal(Emprendedor, 1, ""));

            TableCell control = new TableCell();
            control.Controls.Add(Asignado);
            filaN.Cells.Add(control);

            control = new TableCell();
            control.Controls.Add(Pendiente);
            filaN.Cells.Add(control);

            filaN.Cells.Add(celdaNormal(ObsPendiente, 1, ""));

            control = new TableCell();
            control.Controls.Add(Subsanado);
            filaN.Cells.Add(control);

            control = new TableCell();
            control.Controls.Add(Acreditado);
            filaN.Cells.Add(control);

            control = new TableCell();
            control.Controls.Add(NoAcreditado);
            filaN.Cells.Add(control);

            filaN.Cells.Add(celdaNormal(ObsNoAcreditado, 1, ""));

        }

        private TableHeaderCell crearceladtitulo(String mensaje, Int32 colspan, String cssestilo)
        {
            TableHeaderCell celda1 = new TableHeaderCell();
            celda1.ColumnSpan = colspan;
            celda1.CssClass = cssestilo;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private TableCell celdaNormal(String mensaje, Int32 colspan, String cssestilo)
        {
            TableCell celda1 = new TableCell();
            celda1.ColumnSpan = colspan;
            celda1.CssClass = cssestilo;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private void titulos()
        {
            TableHeaderRow filaTtitulo = new TableHeaderRow();

            filaTtitulo.Cells.Add(crearceladtitulo("No.", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Plan de Negocio", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Acreditador", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Fecha Acta parcial", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Ciudad", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Departamento", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Nombre Institucion", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Nombre Unidad", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Nombre Sector", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Nombre SubSector", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Recursos Solicitados", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Emprendedor", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Detalle acreditacion", 7, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Estado del Proyecto", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Observaciones Acreditación Plan", 1, ""));

            tabla.Rows.Add(filaTtitulo);

            filaTtitulo = new TableHeaderRow();

            filaTtitulo.Cells.Add(crearceladtitulo("", 12, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Asignado", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Pendiente", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Observaciones Pendiente", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Subsanado", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Acreditado", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("No Acreditado", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("Observaciones NoAcreditado", 1, ""));
            filaTtitulo.Cells.Add(crearceladtitulo("", 2, ""));

            tabla.Rows.Add(filaTtitulo);
        }

        protected void B_Excel_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.SaveFileDialog fichero = new System.Windows.Forms.SaveFileDialog();
            //fichero.Filter = "excel (*.xls)|*.xls";
            //fichero.FileName = "planesenacreditacionconvocatoria" + HttpContext.Current.Session["idconvocatoriaevals"].ToString();
            ////if (fichero.showdialog() == system.windows.forms.dialogresult.ok)
            ////{
            //Microsoft.Office.Interop.Excel._Application aplicacion;
            //Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
            //Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
            //aplicacion = new Microsoft.Office.Interop.Excel.Application();
            //libros_trabajo = aplicacion.Workbooks.Add();
            //hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
            ////recorremos el datagridview rellenando la hoja de trabajo


            //for (int i = 0; i < datatable.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < 22; j++)
            //    {
            //        hoja_trabajo.Cells[i + 4, j + 1] = datatable.Rows[i][j].ToString();
            //    }
            //}
            //libros_trabajo.SaveAs(fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
            //libros_trabajo.Close(true);
            //aplicacion.Quit();

            BorrarControl();
        }

        public void BorrarControl()
        {
            Table TablaAExcel;
            TablaAExcel = tabla;

            foreach (TableRow fila in TablaAExcel.Rows)
            {
                foreach (TableCell celda in fila.Cells)
                {
                    try
                    {
                        foreach (Control control in celda.Controls)
                        {
                            if (control is CheckBox)
                            {
                                CheckBox checkbox = (CheckBox)control;

                                Label label = new Label();

                                if (checkbox.Checked)
                                {
                                    label.Text = "SI";
                                }
                                else
                                {
                                    label.Text = "NO";
                                }

                                celda.Controls.Remove(control);
                                celda.Controls.Add(label);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }

            exportar(TablaAExcel);
        }

        private void exportar(Table TablaExportar)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "inline;filename=planesenacreditacionconvocatoria" + HttpContext.Current.Session["idconvocatoriaevals"].ToString() + ".xls");
            Response.Charset = "";
            EnableViewState = false;
            var oStringWriter = new System.IO.StringWriter();
            var oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            TablaExportar.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }
    }
}