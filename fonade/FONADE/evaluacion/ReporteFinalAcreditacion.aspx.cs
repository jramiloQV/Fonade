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

namespace Fonade.FONADE.evaluacion
{
    public partial class ReporteFinalAcreditacion : System.Web.UI.Page
    {
        String idCOnvocatoria;
        DateTime fecha;
        
        ExporTabla tablaExpor;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(HttpContext.Current.Session["EvalidConvocatoria"].ToString()))
                {
                    Response.Redirect("ActasReporteFinalAcreditacion.aspx");
                }
                else
                {
                    idCOnvocatoria = HttpContext.Current.Session["EvalidConvocatoria"].ToString();
                }
            }
            catch (Exception)
            {
                Response.Redirect("ActasReporteFinalAcreditacion.aspx");
            }

            puedeTransmitirse();

            if (!IsPostBack)
            {
                tablaExpor = new ExporTabla(idCOnvocatoria);

                tablaExpor.llenarData();
                tablaExpor.crearTabla();
                //CrearEvento();
                HttpContext.Current.Session["P_TablaEval"] = tablaExpor._Tabla;
            }

            try
            {

                P_tabla.Controls.Add((Table)Session["P_TablaEval"]);
                tablaExpor = new ExporTabla(idCOnvocatoria);
                tablaExpor._Tabla = (Table)Session["P_TablaEval"];
                CrearEvento();
                P_tabla.DataBind();
            }
            catch (Exception) { }
            
        }

        private void puedeTransmitirse()
        {
            String sql = "SELECT ID_ActaAcreditacionFinal  FROM ActaAcreditacionFinal WHERE CODCONVOCATORIA=" + idCOnvocatoria + " AND FECHATRANSMISION IS NULL";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    if (string.IsNullOrEmpty(reader[0].ToString()))
                    {
                        P_puedeTransmitirse.Visible = false;
                        P_puedeTransmitirse.Enabled = false;
                    }
                    else
                    {
                        P_puedeTransmitirse.Visible = true;
                        P_puedeTransmitirse.Enabled = true;
                    }
                }
                else
                {
                        P_puedeTransmitirse.Visible = false;
                        P_puedeTransmitirse.Enabled = false;
                }

                conn.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        protected void C_calendar_SelectionChanged(object sender, EventArgs e)
        {
            ocultar();

            LB_Calendar.Text = "" + C_calendar.SelectedDate;
            LB_Calendar.DataBind();
        }

        protected void LB_Calendar_Click(object sender, EventArgs e)
        {
            C_calendar.Visible = true;
            C_calendar.Enabled = true;

            LB_ocultar.Visible = true;
            LB_ocultar.Enabled = true;

            LB_Calendar.Visible = false;
            LB_Calendar.Enabled = false;

            fecha = C_calendar.SelectedDate;
        }

        protected void LB_ocultar_Click(object sender, EventArgs e)
        {
            ocultar();
        }

        private void ocultar()
        {
            C_calendar.Visible = false;
            C_calendar.Enabled = false;

            LB_ocultar.Visible = false;
            LB_ocultar.Enabled = false;

            LB_Calendar.Visible = true;
            LB_Calendar.Enabled = true;
        }

        protected void B_Enviar_Click(object sender, EventArgs e)
        {
            ClientScriptManager cm = this.ClientScript;

            DateTime fecha = new DateTime();
            String hora;
            String minuto;

            fecha = C_calendar.SelectedDate;
            hora = DDL_Hora.SelectedValue;
            minuto = DDL_Minuto.SelectedValue;

            if (String.IsNullOrEmpty(hora))
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Debe seleccionar la hora');</script>");
                return;
            }

            if (String.IsNullOrEmpty(minuto))
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('Debe seleccionar los minutos');</script>");
                return;
            }

            if (fecha < DateTime.Now)
            {
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('La fecha debe ser superior a la fecha actual');</script>");
                return;
            }

            String formatFecha = "" + fecha.Year + "-" + fecha.Month + "-" + fecha.Day + " " + hora + ":" + minuto;
            String sql = "UPDATE ActaAcreditacionFinal SET FECHATRANSMISION='"+ formatFecha + "' WHERE CODCONVOCATORIA=" + idCOnvocatoria;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteReader();
                conn.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            Response.Redirect("ActasReporteFinalAcreditacion.aspx");
        }

        protected void LB_Exportar_Click(object sender, EventArgs e)
        {
            Table TablaAExcel = new Table();

            tablaExpor = new ExporTabla(idCOnvocatoria);
            tablaExpor._Tabla = (Table)Session["P_TablaEval"];

            tablaExpor.BorrarControl();

            TablaAExcel = tablaExpor._TablaAExcel1;

            exportar(TablaAExcel);
        }

        private void exportar(Table TablaExportar)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "inline;filename=ReporteFinalConvocatoria" + idCOnvocatoria + ".xls");
            Response.Charset = "";
            EnableViewState = false;
            var oStringWriter = new System.IO.StringWriter();
            var oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            TablaExportar.RenderControl(oHtmlTextWriter);
            Response.Write(oStringWriter.ToString());
            Response.End();
        }

        protected void LB_Desacargar_Click(object sender, CommandEventArgs e)
        {
            String commadArgument = "";

            commadArgument = e.CommandName;

            HttpContext.Current.Session["commadArgumentAnexoEmprendedor"] = commadArgument;

            Response.Redirect("AnexoEmprendedor.aspx");
        }

        public void CrearEvento()
        {
            //Table TablaAExcel = new Table();

            //tablaExpor = new ExporTabla(idCOnvocatoria);
            //tablaExpor._Tabla = (Table)Session["P_TablaEval"];

            foreach (TableRow fila in tablaExpor._Tabla.Rows)
            {
                foreach (TableCell celda in fila.Cells)
                {
                    try
                    {
                        foreach (Control control in celda.Controls)
                        {
                            if (control is LinkButton)
                            {
                                //descargarPDF
                                LinkButton LB_Desacargar = new LinkButton();
                                LB_Desacargar = (LinkButton)control;
                                LB_Desacargar.Command += LB_Desacargar_Click;
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }

    public partial class ExporTabla
    {
        private Table tabla;
        private Table TablaAExcel;

        private String idCOnvocatoria;

        private DataTable datatable = new DataTable();
        private TableHeaderRow filaTablaTitulo;
        private TableRow filaTabla;

        private Boolean lNuevo;

        private String lCodproyecto = "";
        private String lCodEstadoProyecto = "";
        private String lNomEstadoProyectoAcreditacion = "";
        private String lObservacionAcreditacion = "";
        private String lCantidadEmprendedores = "";
        private String lNumRadicacionCRIF = "";
        private String lPendiente = "";

        private String lSubsanado = "";
        private String lAcreditado = "";
        private String lNoAcreditado = "";

        private String lNomAsesor = "";
        private String lEmailAsesor = "";
        private String lNomRolAsesor = "";
        private String lNomAsesorLider = "";
        private String lEmailAsesorLider = "";
        private String lNomRolAsesorLider;

        public Table _Tabla
        {
            get { return tabla; }
            set { tabla = value; }
        }

        public Table _TablaAExcel1
        {
            get { return TablaAExcel; }
            set { TablaAExcel = value; }
        }

        public ExporTabla(String idCOnvocatoria)
        {
            this.idCOnvocatoria = idCOnvocatoria;
        }

        public void BorrarControl()
        {
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
                            if (control is LinkButton)
                            {
                                celda.Controls.Remove(control);
                                Label label = new Label();
                                label.Text = "Descargar";
                                celda.Controls.Add(label);
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        public void llenarData()
        {
            datatable = new DataTable();

            Consultas consulta = new Consultas();

            consulta.Parameters = new[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@_IdConvocatoria",
                        Value = idCOnvocatoria
                    }
                };

            datatable = consulta.ObtenerDataTable("MD_ReporteFinalActasAcreditacio");

        }

        public void crearTabla()
        {
            tabla = new Table();
            tabla.CssClass = "Grilla";

            titulosTabla();

            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                lNuevo = false;

                if (!lCodproyecto.Equals(datatable.Rows[i]["ID_PROYECTO"].ToString()))
                {
                    lNuevo = true;
                    lCodproyecto = datatable.Rows[i]["ID_PROYECTO"].ToString();
                    lCodEstadoProyecto = getEstadoProyecto(idCOnvocatoria, lCodproyecto);
                    lNomEstadoProyectoAcreditacion = getNomEstadoproyecto(lCodEstadoProyecto);
                    lObservacionAcreditacion = datatable.Rows[i]["OBSERVACIONFINAL"].ToString();
                    lCantidadEmprendedores = datatable.Rows[i]["Cantidad"].ToString();

                    getDatosAsesor(lCodproyecto, idCOnvocatoria, "1", ref lNomAsesorLider, ref lEmailAsesorLider, ref lNomRolAsesorLider);
                    getDatosAsesor(lCodproyecto, idCOnvocatoria, "2", ref lNomAsesor, ref lEmailAsesor, ref lNomRolAsesor);

                    lNumRadicacionCRIF = datatable.Rows[i]["CRIF"].ToString();
                }
                else
                {
                    lNuevo = false;
                    lCantidadEmprendedores = "0";
                }

                if (!String.IsNullOrEmpty(datatable.Rows[i]["PENDIENTE"].ToString()))
                {
                    lPendiente = datatable.Rows[i]["OBSERVACIONPENDIENTE"].ToString();
                }

                if (!String.IsNullOrEmpty(datatable.Rows[i]["SUBSANADO"].ToString()))
                {
                    lSubsanado = datatable.Rows[i]["OBSERVACIONSUBSANADO"].ToString();
                }

                if (!String.IsNullOrEmpty(datatable.Rows[i]["ACREDITADO"].ToString()))
                {
                    lAcreditado = datatable.Rows[i]["OBSERVACIONACREDITADO"].ToString();
                }

                if (!String.IsNullOrEmpty(datatable.Rows[i]["NOACREDITADO"].ToString()))
                {
                    lNoAcreditado = datatable.Rows[i]["OBSERVACIONNOACREDITADO"].ToString();
                }

                CheckBox lAnexo1, lAnexo2, lAnexo3, lDI, lCertificaciones, lDiploma, lActa;

                lAnexo1 = seleccFromBD(datatable.Rows[i]["FLAGANEXO1"].ToString());
                lAnexo2 = seleccFromBD(datatable.Rows[i]["FLAGANEXO2"].ToString());
                lAnexo3 = seleccFromBD(datatable.Rows[i]["FLAGANEXO3"].ToString());
                lDI = seleccFromBD(datatable.Rows[i]["FLAGDI"].ToString());
                lCertificaciones = seleccFromBD(datatable.Rows[i]["FLAGCERTIFICACIONES"].ToString());
                lDiploma = seleccFromBD(datatable.Rows[i]["FLAGDIPLOMA"].ToString());
                lActa = seleccFromBD(datatable.Rows[i]["FLAGACTA"].ToString());

                filaTabla = new TableRow();

                if (lNuevo)
                {
                    filaTabla.Cells.Add(celdaNormal(lCodproyecto, 1, 1, ""));
                    filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMPROYECTO"].ToString(), 1, 1, ""));
                }
                else
                {
                    filaTabla.Cells.Add(celdaNormal(" ", 1, 1, ""));
                    filaTabla.Cells.Add(celdaNormal(" ", 1, 1, ""));
                }

                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMDEPARTAMENTO"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMCIUDAD"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["FECHAAVALPLAN"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMBRES"].ToString() + " " + datatable.Rows[i]["APELLIDOS"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMROL"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["IDENTIFICACION"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMCIUDADEXPEDICION"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["FECHANACIMIENTO"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["EMAIL"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["TELEFONOEMPRENDEDOR"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["TITULOOBTENIDO"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["SEMESTRESCURSADOS"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["FECHAINICIO"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["FECHAFINMATERIAS"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["FECHAGRADO"].ToString(), 1, 1, ""));

                filaTabla.Cells.Add(celdaNormal(lNomAsesor, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lEmailAsesor, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lNomRolAsesor, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lNomAsesorLider, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lEmailAsesorLider, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lNomRolAsesorLider, 1, 1, ""));

                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["INSTITUCION"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMINSTITUCION"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMSECTOR"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["NOMSUBSECTOR"].ToString(), 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(datatable.Rows[i]["ACREDITADOR"].ToString(), 1, 1, ""));

                filaTabla.Cells.Add(celdaNormal(seleccFromBD("true"), 1, 1, ""));

                if (String.IsNullOrEmpty(lPendiente))
                {
                    filaTabla.Cells.Add(celdaNormal(" ", 1, 1, ""));
                }
                else
                {
                    filaTabla.Cells.Add(celdaNormal(lPendiente, 1, 1, ""));
                }

                filaTabla.Cells.Add(celdaNormal(lSubsanado, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lAcreditado, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lNoAcreditado, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lObservacionAcreditacion, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lNomEstadoProyectoAcreditacion, 1, 1, ""));

                //descargarPDF
                LinkButton LB_Desacargar = new LinkButton();
                LB_Desacargar.CommandArgument = ";" + lCodproyecto + ";" + datatable.Rows[i]["ID_CONTACTO"].ToString();
                LB_Desacargar.CommandName = "" + lCodproyecto + ";" + datatable.Rows[i]["ID_CONTACTO"].ToString();
                LB_Desacargar.Text = "Descargar";
                //LB_Desacargar.Command += LB_Desacargar_Click;

                filaTabla.Cells.Add(celdaNormal(LB_Desacargar, 1, 1, ""));

                filaTabla.Cells.Add(celdaNormal(lAnexo1, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lAnexo2, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lAnexo3, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lDI, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lCertificaciones, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lDiploma, 1, 1, ""));
                filaTabla.Cells.Add(celdaNormal(lActa, 1, 1, ""));

                filaTabla.Cells.Add(celdaNormal(lNumRadicacionCRIF, 1, Int32.Parse(lCantidadEmprendedores), ""));

                tabla.Rows.Add(filaTabla);
            }
        }

        private TableHeaderCell crearceladtitulo(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            TableHeaderCell celda1 = new TableHeaderCell();
            celda1.ColumnSpan = colspan;
            celda1.RowSpan = rowspan;
            celda1.CssClass = cssestilo;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private TableCell celdaNormal(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            TableCell celda1 = new TableCell();
            celda1.ColumnSpan = colspan;
            celda1.RowSpan = rowspan;
            celda1.CssClass = cssestilo;

            Label titulo1 = new Label();
            titulo1.Text = mensaje;
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private TableCell celdaNormal(Control mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            TableCell celda1 = new TableCell();
            celda1.ColumnSpan = colspan;
            celda1.RowSpan = rowspan;
            celda1.CssClass = cssestilo;

            celda1.Controls.Add(mensaje);

            return celda1;
        }

        private void titulosTabla()
        {
            filaTablaTitulo = new TableHeaderRow();

            filaTablaTitulo.Cells.Add(crearceladtitulo("ID Proyecto", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Nombre Proyecto", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Departamento", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Ciudad", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Fecha Aval Plan", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Emprendedor", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Rol", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("No. Documento", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Lugar de Expedición", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Fecha Nacimiento", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Email Emprendedor", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Teléfono", 1, 4, ""));

            filaTablaTitulo.Cells.Add(crearceladtitulo("Formación Académica", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Semestres Cursados / No. Horas", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Fecha Inicio Programa", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Fecha Terminación Programa Académico", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Fecha Graduación", 1, 4, ""));

            filaTablaTitulo.Cells.Add(crearceladtitulo("Nombre Asesor", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Email Asesor", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Rol", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Nombre Asesor Lider", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Email Asesor Lider", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Rol", 1, 4, ""));

            filaTablaTitulo.Cells.Add(crearceladtitulo("Nombre Institución", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Unidad Emprendimiento", 1, 4, ""));

            filaTablaTitulo.Cells.Add(crearceladtitulo("Sector", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("SubSector", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Acreditador", 1, 4, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Proceso de Acreditación", 16, 1, ""));

            tabla.Rows.Add(filaTablaTitulo);

            filaTablaTitulo = new TableHeaderRow();

            filaTablaTitulo.Cells.Add(crearceladtitulo("Observaciones Proceso Acreditación", 7, 1, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Documentos Anexos", 9, 1, ""));

            tabla.Rows.Add(filaTablaTitulo);

            filaTablaTitulo = new TableHeaderRow();

            filaTablaTitulo.Cells.Add(crearceladtitulo("Asignado", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Pendiente", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Subsanado", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Acreditado", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("No Acreditado", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Observación Acreditación", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Estado Acreditación Proyecto", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Anexos", 5, 1, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Certificaciones", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Diplomas", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("Acta", 1, 2, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("# Radicaci&oacute;n CRIF", 1, 2, ""));

            tabla.Rows.Add(filaTablaTitulo);

            filaTablaTitulo = new TableHeaderRow();

            filaTablaTitulo.Cells.Add(crearceladtitulo("", 1, 1, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("1", 1, 1, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("2", 1, 1, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("3", 1, 1, ""));
            filaTablaTitulo.Cells.Add(crearceladtitulo("DI", 1, 1, ""));

            tabla.Rows.Add(filaTablaTitulo);
        }

        private String getEstadoProyecto(String pCodConvocatoria, String pCodProyecto)
        {
            String sql = "SELECT TOP 1  CODESTADO,FECHA FROM PROYECTOACREDITACION WHERE CODPROYECTO =" + pCodProyecto + " AND CODCONVOCATORIA=" + pCodConvocatoria + " ORDER BY FECHA DESC";

            sql = getScalar(sql);

            return sql;
        }

        private String getNomEstadoproyecto(String pCodEstado)
        {
            if (String.IsNullOrEmpty(pCodEstado))
            {
                return "";
            }

            String sql = "SELECT NOMESTADO FROM ESTADO WHERE ID_ESTADO=" + pCodEstado;

            sql = getScalar(sql);

            return sql;
        }

        private String getScalar(String pSentencia)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(pSentencia, conn);
            try
            {

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return "" + reader[0];
                }
                else
                {
                    return "";
                }
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return "";
        }

        private void getDatosAsesor(String pCodProyecto, String pCodConvocatoria, String pCodRolAsesor, ref String pNomAsesor, ref String pEmail, ref String pNomRol)
        {
            String sql = "SELECT C.NOMBRES, C.APELLIDOS,C.EMAIL,R.NOMBRE,R.ID_ROL   FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (C.ID_CONTACTO = PC.CODCONTACTO) JOIN ROL R ON (PC.CODROL = R.ID_ROL) WHERE PC.CODPROYECTO = " + pCodProyecto + "  AND PC.INACTIVO=0 AND PC.CODROL =" + pCodRolAsesor;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    pNomAsesor = reader["NOMBRES"].ToString() + " " + reader["APELLIDOS"].ToString();
                    pEmail = reader["EMAIL"].ToString();
                    pNomRol = reader["NOMBRE"].ToString();
                }
                else
                {
                    pNomAsesor = "";
                    pEmail = "";
                    pNomRol = "";
                }
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private CheckBox seleccFromBD(String pValor)
        {
            CheckBox checkbox = new CheckBox();
            checkbox.Enabled = false;

            try
            {
                if (Boolean.Parse(pValor))
                {
                    checkbox.Checked = true;
                }
                else
                {
                    checkbox.Checked = false;
                }
            }
            catch (Exception)
            {
                checkbox.Checked = false;
            }

            return checkbox;
        }
    }
}