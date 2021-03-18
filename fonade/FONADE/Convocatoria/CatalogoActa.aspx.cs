using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Datos;
using Fonade.Negocio;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Configuration;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// CatalogoActa
    /// </summary>    
    public partial class CatalogoActa : Negocio.Base_Page
    {
        String CodActividad;
        int Mes;
        String txtSQL;

        /// <summary>
        /// Si este valor contiene datos, significa que se generará un nuevo documento.
        /// </summary>
        String Accion_Docs;

        #region Variables que usa FONADE clásico.

        String CodProyecto;
        String NombreArchivo;
        String RutaHttpDestino;
        String RutaDestino;
        String RutaDestinoTemp;
        String ArchivoSubido;
        String txtFormato;
        String RsFormato;
        String CodFormato;
        String NomFormato;
        String FSObject;
        String Carpeta;
        String SubCarpetas;
        String NuevaCarpeta;
        String txtNomMedio;
        String txtCarpeta;
        String txtDescripcion;
        String numTimeOut;
        String txtTab;
        String txtLink;

        //Variables para upload de interventoría
        String CodConvocatoria;
        String CodActa;
        String txtNomActa;
        String txtNumActa;
        String txtFechaActa;
        String CodInterventoria;
        String mes;
        String TipoInterventor;

        Int64 TamanoArchivo;
        String FormatoArchivo;

        #endregion

        /*Variables de instancia para registro de carga de archivos.*/
        static string _codActividad { get; set; } static string _mes { get; set; } static string _codcontacto { get; set; } static string _url { get; set; }
        static string _codigoformato { get; set; } static string _fecha { get; set; } static string _borrado { get; set; } static string _comentario { get; set; }
        static string _codtipointv { get; set; } static string _nombre { get; set; } static bool _trsct { get; set; }
        static HttpPostedFile[] files { get; set; }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodConvocatoria = Session["CodConvoca"].ToString();
            var Accion_Docs = Session["Accion"].ToString();

            if(!Page.IsPostBack)
            {
                ValidarGrupo();
            }

            if (Accion_Docs == "NuevoDocumento")
            {
                pnlPrincipal.Visible = false;
                pnl_NuevoDoc.Visible = true;
            }
            else
            {
                pnlPrincipal.Visible = true;
                pnl_NuevoDoc.Visible = false;
                lbltitulo.Text = "Listado Actas";
            }
            
            
        }

        /// <summary>
        /// Llenar la grilla de documentos.
        /// </summary>
        private void ValidarGrupo()
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor) { }

            try
            {
                var txtSQl = "Select id_acta, NomActa, FechaActa, Fecha, url from ConvocatoriaActa Where CodConvocatoria = " + Session["IdConvocatoria"].ToString();
                txtSQl += " And Borrado = 0";

                var dt = consultas.ObtenerDataTable(txtSQl, "text");

                HttpContext.Current.Session["dtDocumentos"] = dt;
                GrvDocumentos.DataSource = dt;
                GrvDocumentos.DataBind();
            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }
        }

        /// <summary>
        /// Método para ordernar/sortear.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            var sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;

                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        /// <summary>
        /// Paginación de la grilla.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrvDocumentosPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrvDocumentos.PageIndex = e.NewPageIndex;
            GrvDocumentos.DataSource = HttpContext.Current.Session["dtDocumentos"];
            GrvDocumentos.DataBind();
        }

        /// <summary>
        /// GRVs the documentos sorting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GridViewSortEventArgs"/> instance containing the event data.</param>
        protected void GrvDocumentosSorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GrvDocumentos.DataSource = HttpContext.Current.Session["dtDocumentos"];
                GrvDocumentos.DataBind();
            }
        }

        /// <summary>
        /// Crear el nuevo documento...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrear_Click(object sender, EventArgs e)
        {
            var basePath = System.Configuration.ConfigurationManager.AppSettings.Get("RutaIP") + System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"FolderActas\";
            if (Archivo.HasFile)
            {
                var fileName = Path.GetFileName(Archivo.FileName).Split('.');
                var nombreArchvio = Session["IdConvocatoria"].ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Minute.ToString() + "." + fileName[1];
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                Archivo.SaveAs(basePath + nombreArchvio);
                var dirUrl = System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"FolderActas\" + nombreArchvio;
                var extencion = (from df in consultas.Db.DocumentoFormatos
                                 where df.Extension.Trim() == Path.GetExtension(basePath + nombreArchvio)
                                 select df).FirstOrDefault();
                var borrado = "0";
                var comentario = Comentario.Text;
                
                var query = "Insert Into ConvocatoriaActa(NumActa, NomActa, FechaActa, Fecha, URL, CodConvocatoria, CodDocumentoFormato, CodContacto, Comentario, Borrado) ";
                query += "Values('" + txtNumeroActa.Text.Trim() + "','" + txtNombre.Text + "',TRY_PARSE('" + txtFecha.Text + "' AS DATETIME USING 'en-GB'),GetDate(),'" + dirUrl + "'," + Session["IdConvocatoria"].ToString() + "," + extencion.Id_DocumentoFormato.ToString() + "," + usuario.IdContacto + ",'" + comentario + "'," + borrado + ")";
                ejecutaReader(query, 2);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Documento registrado.');", true);
                Session["Accion"] = "Lista";
                Response.Redirect("CatalogoActa.aspx");
            }

        }

        private void EditarArchvio(string idFile)
        {
            var txt = "Select * from AvanceActividadPOAnexos where Id = " + idFile;
            var query = consultas.ObtenerDataTable(txt, "text");
            if (query.Rows.Count > 0)
            {
                //NomDocumento.Text = query.Rows[0].ItemArray[1].ToString();
                //dd_TipoInterventor.SelectedValue = query.Rows[0].ItemArray[5].ToString();
                Comentario.Text = query.Rows[0].ItemArray[8].ToString();
            }

        }
        /// <summary>
        /// Actializar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            var nombreArchvio = string.Empty;
            var dirUrl = string.Empty;
            var basePath = System.Configuration.ConfigurationManager.AppSettings.Get("RutaIP") + System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\";
            if (Archivo.HasFile)
            {
                nombreArchvio = Archivo.FileName;
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                Archivo.SaveAs(basePath + nombreArchvio);
                dirUrl = ", Url = '" + (System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\" + nombreArchvio).Replace(@"\", @"/") + "'";
            }
            var codActvidadf = Session["CodActividad"].ToString();
            var nomDoc = ""; //NomDocumento.Text; // Path.GetFileNameWithoutExtension(basePath + nombreArchvio);
            var codContac = usuario.IdContacto.ToString();
            var mesAct = Session["linkid"].ToString();
            var extencion = (from df in consultas.Db.DocumentoFormatos
                             where df.Extension == Path.GetExtension(basePath + nombreArchvio)
                             select df).FirstOrDefault();
            var fecha = DateTime.Today.ToShortDateString();
            var comentario = Comentario.Text;
            var idFile = Request.QueryString["idFile"];

            var query = "Update AvanceActividadPOAnexos Set NomDocumento = '" + nomDoc + "'" + dirUrl + ", Fecha = TRY_PARSE('" + fecha + "' AS DATETIME USING 'en-GB'), Comentario = '" + comentario + "' Where Id = " + idFile;

            ejecutaReader(query, 2);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Documento actualizado!'); window.opener.location.reload()", true);
            Response.Redirect("CatalogoActividadPOInterventor.aspx");
        }

        /// <summary>
        /// Handles the RowCommand event of the GrvDocumentos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void GrvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                var id = e.CommandArgument.ToString();
                consultas.Db.ExecuteCommand("Update ConvocatoriaActa set Borrado = 1 where Id_Acta =  " + id);
                Session["Accion"] = "Lista";
                Response.Redirect("CatalogoActa.aspx");
            }
        }


    }
}