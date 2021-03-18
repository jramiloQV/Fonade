using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Utilidad
{
    public static class Utilidades
    {
        /// <summary>
        /// Presenta el mensaje en un alert
        /// </summary>
        /// <param name="cad">Mensaje a presentar</param>
        public static void PresentarMsj(string cad, Control ctrl, string tipo)
        {
            switch (tipo)
            {
                case "Alert":
                    ScriptManager.RegisterStartupScript(ctrl, typeof(Page), "Alert", "<script>alert('" + cad + "');</script>", false);
                    break;
                default:
                    ScriptManager.RegisterStartupScript(ctrl, typeof(Page), "Confirm", "<script>Confirm('" + cad + "');</script>", false);
                    break;
            }

        }

        /// Metodo usado para descargar cualquier tipo de archivo
        /// </summary>
        /// <param name="path">Ruta Fisica o virtual del archivo (Fisica Eje: c:\\directorio\documento.pdf),(Virtual Eje: ~\Directorio\documento.pdf)  </param>
        /// <param name="rutaFisica">Dato Booleano que identifica si la ruta sera fisica o virtual</param>
        public static void DescargarArchivo(string path, bool rutaFisica = true)
        {
            System.IO.FileInfo toDownload;

            if (rutaFisica)
            {
                toDownload = new System.IO.FileInfo(path);
            }
            else
            {
                toDownload = new System.IO.FileInfo(HttpContext.Current.Server.MapPath(path));
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
            HttpContext.Current.Response.AddHeader("Content-Length",
                        toDownload.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.WriteFile(path);            
            //HttpContext.Current.Response.End();
            //HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            //HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            //HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }

        /// <summary>
        /// Descarga en formato excel la información presentada en una grilla
        /// </summary>
        /// <param name="nomarchivo">Nombre del archivo</param>
        /// <param name="ctrl">Grilla que presenta la información a descargar</param>
        public static void DescargarArchExcel(string nomarchivo, GridView ctrl)
        {
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.AddHeader(
            "content-disposition", string.Format("attachment; filename={0}", nomarchivo));
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write("<head><meta http-equiv=Content-Type content=:" + '"' + "text/html; charset=utf-8" + '"' + "></head>");
            
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    Table table = new Table();

                    if (ctrl.HeaderRow != null)
                    {
                        PrepararControlParaExportar(ctrl.HeaderRow);
                        table.Rows.Add(ctrl.HeaderRow);
                    }

                    foreach (GridViewRow row in ctrl.Rows)
                    {
                        PrepararControlParaExportar(row);
                        table.Rows.Add(row);
                    }

                    if (ctrl.FooterRow != null)
                    {
                        PrepararControlParaExportar(ctrl.FooterRow);
                        table.Rows.Add(ctrl.FooterRow);
                    }
                    table.GridLines = ctrl.GridLines;
                    table.RenderControl(htw);

                    HttpContext.Current.Response.Write(sw.ToString());
                    HttpContext.Current.Response.End();
                }
            }
        }

        /// <summary>
        /// Convierte los posibles controles internos de la grilla en texto
        /// </summary>
        /// <param name="control">Controles internos de la grilla</param>
        private static void PrepararControlParaExportar(Control control)
        {
            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepararControlParaExportar(current);
                }
            }
        }
    }
}