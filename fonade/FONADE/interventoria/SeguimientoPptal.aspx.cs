using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using Fonade.Error;

namespace Fonade.FONADE.interventoria
{
    public partial class SeguimientoPptal : Negocio.Base_Page
    {
        string CodProyecto;
        string CodEmpresa;
        string CodConvocatoria;
        string anioConvocatoria;

        string txtSQL;
        protected void Page_Load(object sender, EventArgs e)
        {
            Culture = "es-CO";
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

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

            if (!IsPostBack)
            {
                InfroHead();
                llenarGid();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/05/2014.
        /// Modificación al código fuente.
        /// </summary>
        private void InfroHead()
        {
            #region Versión de Mauricio Arias Olave.
            //Inicializar variables:
            Double SMLV_Obtenido = 0;
            Double ValorRecomendado = 0;
            Double presupuesto_recomendado = 0;
            Double presupuesto_disponible = 0;
            Double presupuesto_aprobado = 0;

            try
            {
                //Primera consulta.
                txtSQL = " SELECT CodConvocatoria, ValorRecomendado, DatePart(yyyy,fechaInicio) AS AnnoConvocatoria " +
                         " FROM Evaluacionobservacion, Convocatoria  WHERE CodProyecto = " + CodProyecto +
                         " AND CodConvocatoria = Id_Convocatoria ORDER BY CodConvocatoria DESC ";

                //Asignar resultados de la primera consulta.
                var dt_sql_1 = consultas.ObtenerDataTable(txtSQL, "text");

                //Revisar si tiene datos... "debería tener"...
                if (dt_sql_1.Rows.Count > 0)
                {
                    //Asignar el valor de "ValorRecomendado" a la variable con el mismo nombre.
                    ValorRecomendado = Double.Parse(dt_sql_1.Rows[0]["ValorRecomendado"].ToString());

                    //Asignar el valor de "CodConvocatoria" a la variable con el mismo nombre.
                    CodConvocatoria = dt_sql_1.Rows[0]["CodConvocatoria"].ToString();

                    //Obtener el SMLV dependiendo de la consulta anterior.
                    txtSQL = " SELECT SalarioMinimo FROM SalariosMinimos WHERE AñoSalario = " + dt_sql_1.Rows[0]["AnnoConvocatoria"].ToString();

                    //Asignar resultados de la segunda consulta a variable DataTable.
                    var dt_sql_2 = consultas.ObtenerDataTable(txtSQL, "text");

                    //Revisar si tiene datos... "debería tener"...
                    if (dt_sql_2.Rows.Count > 0)
                    {
                        //Asignar resultados a variables internas.
                        SMLV_Obtenido = Double.Parse(dt_sql_2.Rows[0]["SalarioMinimo"].ToString());

                        //Presupuesto Recomendado por Fondo Emprender:
                        presupuesto_recomendado = ValorRecomendado * SMLV_Obtenido;

                        //Mostrar resultados (Presupuesto Recomendado por Fondo Emprender:)
                        lblemprender.Text = "$" + presupuesto_recomendado.ToString("0,0.00", CultureInfo.InvariantCulture);

                        //Consulta #3: Obtener Presupuesto Aprobado por Interventoría:
                        txtSQL = " SELECT SUM(CantidadDinero) AS Total FROM PagoActividad " +
                                 " WHERE Estado >= 1 AND Estado < 5 " +
                                 " AND CodProyecto =  " + CodProyecto;

                        //Asignar resultados de la tercera consulta a variable DataTable.
                        var dt_sql_3 = consultas.ObtenerDataTable(txtSQL, "text");

                        //Revisar si tiene datos... "debería tener"...
                        string total = dt_sql_3.Rows[0]["Total"].ToString();

                        if (dt_sql_3.Rows.Count > 0 && total != "")
                        {
                            //Presupuesto Aprobado por Interventoría: //drDet["fondoTotal"] = "$" + totalFondo.ToString("0,0.00", CultureInfo.InvariantCulture);
                            presupuesto_aprobado = Double.Parse(dt_sql_3.Rows[0]["Total"].ToString());
                            lblinterventoria.Text = "$" + presupuesto_aprobado.ToString("0,0.00", CultureInfo.InvariantCulture);

                            //Presupuesto Disponible:
                            presupuesto_disponible = presupuesto_recomendado - presupuesto_aprobado;
                            lbldisponible.Text = "$" + presupuesto_disponible.ToString("0,0.00", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            lblinterventoria.Text = "$0,00";
                            lbldisponible.Text = "$" + presupuesto_recomendado.ToString("0,0.00", CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblemprender.Text = "";
                lblinterventoria.Text = "";
                lbldisponible.Text = "";
            }
            #endregion            
        }

        protected void gvpresupuesto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Lista")
            {
                var param = e.CommandArgument.ToString();
                HttpContext.Current.Session["CodPagoActividad"] = param;
                Response.Redirect("CatalogoDocumentoPagos.aspx?Accion=Lista");
            }
            else if (e.CommandName == "Reintegros")
            {
                var param = e.CommandArgument.ToString();

                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Reintegros/VerReintegro.aspx?codigo=" + param);
            }
        }

        private void llenarGid()
        {
            if (!string.IsNullOrEmpty(CodProyecto))
            {
                var dt = from pi in consultas.Db.MD_PresupuestoInterventor(Convert.ToInt32(CodProyecto))
                         select new
                         {
                             pi.Id_PagoActividad,
                             pi.NomPagoActividad,
                             pi.FechaInterventor,
                             CantidadDinero = pi.CantidadDinero.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                             Estado = new Func<string>(() =>
                                 {
                                     switch (Convert.ToInt32(pi.Estado))
                                     {
                                         case Constantes.CONST_EstadoInterventor:
                                             return "Interventor";
                                         case Constantes.CONST_EstadoCoordinador:
                                             return "Coordinador";
                                         case Constantes.CONST_EstadoFiduciaria:
                                             return "Fiduciaria";
                                         case Constantes.CONST_EstadoAprobadoFA:
                                             return "Aprobado";
                                         case Constantes.CONST_EstadoRechazadoFA:
                                             return "Rechazado";
                                         default:
                                             return "Interventor";
                                     }
                                 })(),
                             pi.NomTipoIdentificacion,
                             pi.NumIdentificacion,
                             pi.Nombre,
                             pi.Apellido,
                             pi.RazonSocial,
                             pi.FechaRtaFA,
                             //ValorReteFuente = new Func<string>(() =>
                             //    {
                             //        try
                             //        {
                             //            return "" + Convert.ToInt32(pi.ValorReteFuente);
                             //        }
                             //        catch (FormatException) { return "0"; }
                             //    })(),
                             ValorReteFuente = pi.ValorReteFuente.HasValue ? 
                             pi.ValorReteFuente.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) : "0,00",
                             //ValorReteIVA = new Func<string>(() =>
                             //    {
                             //        try
                             //        {
                             //            return "" + Convert.ToInt32(pi.ValorReteIVA);
                             //        }
                             //        catch (FormatException) { return "0"; }
                             //    })(),
                             ValorReteIVA = pi.ValorReteIVA.HasValue ? 
                             pi.ValorReteIVA.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) : "0,00",
                             //ValorReteICA = new Func<string>(() =>
                             //    {
                             //        try
                             //        {
                             //            return "" + Convert.ToInt32(pi.ValorReteICA);
                             //        }
                             //        catch (FormatException) { return "0"; }
                             //    })(),
                             ValorReteICA = pi.ValorReteICA.HasValue ? 
                             pi.ValorReteICA.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) : "0,00",
                             //OtrosDescuentos = new Func<string>(() =>
                             //    {
                             //        try
                             //        {
                             //            return "" + Convert.ToInt32(pi.OtrosDescuentos);
                             //        }
                             //        catch (FormatException) { return "0"; }
                             //    })(),
                             OtrosDescuentos = pi.OtrosDescuentos.HasValue ? 
                             pi.OtrosDescuentos.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) : "0,00",
                             //ValorPagado = new Func<string>(() =>
                             //   {
                             //       try
                             //       {
                             //           return "" + Convert.ToInt32(pi.ValorPagado);
                             //       }
                             //       catch (FormatException) { return "0"; }
                             //   })(),
                             ValorPagado = pi.ValorPagado.HasValue ? 
                             pi.ValorPagado.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) : "0,00",
                             pi.CodigoPago,
                             ObservacionesFA = new Func<string>(() =>
                               {
                                   using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                                   {
                                       if (pi.Estado.Equals(Constantes.CONST_EstadoRechazadoFA) && pi.FechaRtaFA == null)
                                       {
                                           var observacion = db.PagosActaSolicitudPagos.SingleOrDefault(filter => filter.CodPagoActividad.Equals(pi.Id_PagoActividad) && filter.Aprobado.Equals(false));
                                           if (observacion != null)
                                               return "Observación coordinador Interventoria : " + observacion.Observaciones;
                                           else
                                               return string.Empty;
                                       }
                                       else
                                       {
                                           if (consultas.Db.Reintegros.Any(filter1 => filter1.CodigoPago == pi.Id_PagoActividad))
                                               return pi.ObservacionesFA + " - Pago con reintegros.";
                                           else
                                               return pi.ObservacionesFA;
                                       }
                                   }
                               })(),
                             FechaIngresoPago = pi.FechaIngresoPago,
                             FechaIngresoInterventor = pi.FechaIngresoInterventor,
                             FechaAprobacionInterventor = pi.FechaAprobacionInterventor,
                             FechaIngresoCoordinacion = pi.FechaIngresoCoordinador,
                             FechaAprobacionORechazoCoordinador = pi.FechaAprobacionORechazoCoordinador,
                             FechaRespuestaFiduciaria = pi.FechaRespuestaFiduciaria,
                             UltimoReintegro = consultas.Db.Reintegros.Any(filter1 => filter1.CodigoPago == pi.Id_PagoActividad) ? consultas.Db.Reintegros.Where(filter => filter.CodigoPago == pi.Id_PagoActividad).OrderByDescending(FilterOrder => FilterOrder.FechaIngreso).FirstOrDefault().ValorReintegro : 0
                         };

                gvpresupuesto.DataSource = dt;
                gvpresupuesto.DataBind();
            }
        }

        protected void lnkPagos_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = (from pi in consultas.Db.MD_PresupuestoInterventor(Convert.ToInt32(CodProyecto))
                          select new
                          {
                              codigo = pi.Id_PagoActividad,
                              NombrePago = pi.NomPagoActividad,
                              FechaInterventor = pi.FechaInterventor,
                              CantidadDinero = pi.CantidadDinero.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                              Estado = new Func<string>(() =>
                              {
                                  switch (Convert.ToInt32(pi.Estado))
                                  {
                                      case Constantes.CONST_EstadoInterventor:
                                          return "Interventor";
                                      case Constantes.CONST_EstadoCoordinador:
                                          return "Coordinador";
                                      case Constantes.CONST_EstadoFiduciaria:
                                          return "Fiduciaria";
                                      case Constantes.CONST_EstadoAprobadoFA:
                                          return "Aprobado";
                                      case Constantes.CONST_EstadoRechazadoFA:
                                          return "Rechazado";
                                      default:
                                          return "Interventor";
                                  }
                              })(),
                              TipoIdentificacion = pi.NomTipoIdentificacion,
                              Identificacion = pi.NumIdentificacion,
                              pi.Nombre,
                              pi.Apellido,
                              pi.RazonSocial,
                              pi.FechaRtaFA,
                              //ValorReteFuente = new Func<string>(() =>
                              //    {
                              //        try
                              //        {
                              //            return "" + Convert.ToInt32(pi.ValorReteFuente);
                              //        }
                              //        catch (FormatException) { return "0"; }
                              //    })(),
                              ValorReteFuente = pi.ValorReteFuente.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                              //ValorReteIVA = new Func<string>(() =>
                              //    {
                              //        try
                              //        {
                              //            return "" + Convert.ToInt32(pi.ValorReteIVA);
                              //        }
                              //        catch (FormatException) { return "0"; }
                              //    })(),
                              ValorReteIVA = pi.ValorReteIVA.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                              //ValorReteICA = new Func<string>(() =>
                              //    {
                              //        try
                              //        {
                              //            return "" + Convert.ToInt32(pi.ValorReteICA);
                              //        }
                              //        catch (FormatException) { return "0"; }
                              //    })(),
                              ValorReteICA = pi.ValorReteICA.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                              //OtrosDescuentos = new Func<string>(() =>
                              //    {
                              //        try
                              //        {
                              //            return "" + Convert.ToInt32(pi.OtrosDescuentos);
                              //        }
                              //        catch (FormatException) { return "0"; }
                              //    })(),
                              OtrosDescuentos = pi.OtrosDescuentos.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                              //ValorPagado = new Func<string>(() =>
                              //   {
                              //       try
                              //       {
                              //           return "" + Convert.ToInt32(pi.ValorPagado);
                              //       }
                              //       catch (FormatException) { return "0"; }
                              //   })(),
                              ValorPagado = pi.ValorPagado.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                              pi.CodigoPago,
                              ObservacionesFA = new Func<string>(() =>
                              {
                                  using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                                  {
                                      if (pi.Estado.Equals(Constantes.CONST_EstadoRechazadoFA) && pi.FechaRtaFA == null)
                                      {
                                          var observacion = db.PagosActaSolicitudPagos.SingleOrDefault(filter => filter.CodPagoActividad.Equals(pi.Id_PagoActividad) && filter.Aprobado.Equals(false));
                                          if (observacion != null)
                                              return "Observación coordinador Interventoria : " + observacion.Observaciones;
                                          else
                                              return string.Empty;
                                      }
                                      else
                                      {
                                          if (consultas.Db.Reintegros.Any(filter1 => filter1.CodigoPago == pi.Id_PagoActividad))
                                              return pi.ObservacionesFA + " - Pago con reintegros.";
                                          else
                                              return pi.ObservacionesFA;
                                      }
                                  }
                              })(),
                              FechaIngresoPago = pi.FechaIngresoPago,
                              FechaIngresoInterventor = pi.FechaIngresoInterventor,
                              FechaAprobacionInterventor = pi.FechaAprobacionInterventor,
                              FechaIngresoCoordinacion = pi.FechaIngresoCoordinador,
                              FechaAprobacionORechazoCoordinador = pi.FechaAprobacionORechazoCoordinador,
                              FechaRespuestaFiduciaria = pi.FechaRespuestaFiduciaria,
                              UltimoReintegro = consultas.Db.Reintegros.Any(filter1 => filter1.CodigoPago == pi.Id_PagoActividad) ? consultas.Db.Reintegros.Where(filter => filter.CodigoPago == pi.Id_PagoActividad).OrderByDescending(FilterOrder => FilterOrder.FechaIngreso).FirstOrDefault().ValorReintegro : 0
                          }).ToList();


                var dtPago = ConvertToDataTable(dt);

                DataSet ds = new DataSet("Organization");
                ds.Tables.Add(dtPago);

                ExportDataSetToExcel(ds);

            }
            catch (Exception ex)
            {
                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                //throw;
            }
        }

        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)

        {

            PropertyDescriptorCollection properties =

            TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)

                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)

            {

                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)

                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);

            }

            return table;
        }

        private void ExportDataSetToExcel(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                System.IO.StringWriter sw = new System.IO.StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                Page pagina = new Page();
                System.Web.UI.HtmlControls.HtmlForm form = new System.Web.UI.HtmlControls.HtmlForm();
                GridView dg = new GridView();
                dg.EnableViewState = false;
                dg.DataSource = ds.Tables[0];
                dg.DataBind();
                pagina.EnableEventValidation = false;
                pagina.DesignerInitialize();
                pagina.Controls.Add(form);
                form.Controls.Add(dg);
                pagina.RenderControl(htw);
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=DescargaPagos.xls");
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.Default;
                Response.Write(sb.ToString());
                //Response.End(); se comentaria porque genera excepcion al terminar la peticion
                //Se agregan las linea de abajo.
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }

        }


        //private void ExportDataSetToExcel(DataSet ds)
        //{
        //    try
        //    {
        //        Excel.Application excelApp = new Excel.Application();
        //        string directorioDestino = "DescargarPagos\\" + "PagoByProyecto.xlsx"; //"DescargarPagos\\" + DateTime.Now.ToShortDateString() + "pagosProyecto.xlsx";
        //        string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        //        Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(directorioBase + directorioDestino);


        //        excelApp.DisplayAlerts = false;
        //        for (int i = excelApp.ActiveWorkbook.Worksheets.Count; i > 0; i--)
        //        {
        //            Excel.Worksheet wkSheet = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[i];
        //            if (wkSheet.Name == "Table1")
        //            {
        //                wkSheet.Delete();
        //            }
        //        }
        //        excelApp.DisplayAlerts = true;



        //        foreach (DataTable table in ds.Tables)
        //        {
        //            //Add a new worksheet to workbook with the Datatable name
        //            Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Sheets.Add();
        //            excelWorkSheet.Name = table.TableName;

        //            for (int i = 1; i < table.Columns.Count + 1; i++)
        //            {
        //                excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
        //            }

        //            for (int j = 0; j < table.Rows.Count; j++)
        //            {
        //                for (int k = 0; k < table.Columns.Count; k++)
        //                {
        //                    excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
        //                }
        //            }
        //        }

        //        excelWorkBook.Save();
        //        excelWorkBook.Close();
        //        excelApp.Quit();

        //        string url = directorioBase + directorioDestino;
        //        Utilidades.DescargarArchivo(url);
        //    }
        //    catch (Exception ex)
        //    {
        //        //excelWorkBook.Close(false);
        //        //excelApp.Quit();
        //    }
        //}
    }
}