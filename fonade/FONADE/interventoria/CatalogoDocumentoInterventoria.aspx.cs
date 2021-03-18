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
using Fonade.Clases;
using Fonade.Clases;
using System.Text.RegularExpressions;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoDocumentoInterventoria : Negocio.Base_Page
    {
        int CodActividad;
        int Mes;
        String txtSQL;

        /// <summary>
        /// Si este valor contiene datos, significa que se generará un nuevo documento.
        /// </summary>
        String Accion_Docs, accion;

        #region Variables que usa FONADE clásico.

        int CodProyecto;
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
        static string _codActividad  { get; set; } static string _mes { get; set; } static string _codcontacto { get; set; } static string _url { get; set; }
        static string _codigoformato { get; set; } static string _fecha { get; set; } static string _borrado { get; set; } static string _comentario { get; set; }
        static string _codtipointv { get; set; } static string _nombre { get; set; } static bool _trsct { get; set; }
        static HttpPostedFile[] files { get; set; }

        public Boolean AllowDelete { 
            get {
                if (usuario == null)
                    return false;
                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {           
                    if (Request.QueryString["lock"] != null)
                    {
                        var locked = Request.QueryString["lock"];
                        if (locked.Equals("true"))
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        return true;
                    }
                }                
                else
                    return false;
            } 
            set {
            } 
        }

        private void SetSessiones(string session, string valor)
        {
            Session[session] = valor;            
        }

        private string GetSessiones(string session)
        {
            if(Session[session]!= null)
            {
                return Session[session].ToString() ?? "";
            }
            else
            {
                return null;
            }
        }

        private void guardarValoresEnSesion()
        {
            string rquestString = "do";

            if (Request.QueryString[rquestString] != null)
                SetSessiones(rquestString, Request.QueryString[rquestString].ToString());

            rquestString = "prj";

            if (Request.QueryString[rquestString] != null)
                SetSessiones(rquestString, Request.QueryString[rquestString].ToString());

            rquestString = "act";

            if (Request.QueryString[rquestString] != null)
                SetSessiones(rquestString, Request.QueryString[rquestString].ToString());

            rquestString = "mes";

            if (Request.QueryString[rquestString] != null)
                SetSessiones(rquestString, Request.QueryString[rquestString].ToString());

            rquestString = "idFile";

           // if (Request.QueryString[rquestString] != null)
                SetSessiones(rquestString, Request.QueryString[rquestString]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //accion = Request.QueryString["do"];
                //CodProyecto = int.Parse(Request.QueryString["prj"].ToString()); // Session["CodProyecto"].ToString();
                //CodActividad = int.Parse(Request.QueryString["act"].ToString()); // Convert.ToString(HttpContext.Current.Session["CodActividad"] ?? 0);
                //_codActividad = Request.QueryString["act"]; // Convert.ToString(HttpContext.Current.Session["CodActividad"] ?? string.Empty);
                //Mes = int.Parse(Request.QueryString["mes"].ToString()); // Convert.ToInt32(HttpContext.Current.Session["linkid"] ?? 0);

                guardarValoresEnSesion();

                accion = GetSessiones("do");
                CodProyecto = int.Parse(GetSessiones("prj").ToString()); 
                CodActividad = int.Parse(GetSessiones("act").ToString());
                _codActividad = GetSessiones("act"); 
                Mes = int.Parse(GetSessiones("mes").ToString()); 

                _mes = Convert.ToString(HttpContext.Current.Session["linkid"] ?? string.Empty);
                Accion_Docs = Convert.ToString(HttpContext.Current.Session["Accion_Docs"]??string.Empty);
                _codcontacto = usuario.IdContacto.ToString();

                if (Accion_Docs == "NuevoDocumento")
                {
                    pnlPrincipal.Visible = false; 
                    pnl_NuevoDoc.Visible = true; 
                    CargarTiposDeDocumento();
                }
                else
                {
                    pnlPrincipal.Visible = true;
                    pnl_NuevoDoc.Visible = false;

                    //var idFile = Request.QueryString["idFile"];
                    var idFile = GetSessiones("idFile");
                                        
                    if (idFile != null)
                    {
                        pnlPrincipal.Visible = false;
                        pnl_NuevoDoc.Visible = true;
                        CargarTiposDeDocumento();
                        EditarArchvio(idFile);
                        btnActualizar.Visible = true;
                        btnCrear.Visible = false;
                    }
                }
                ValidarGrupo();
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
                consultas.Parameters = null;

                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodActividad",
                                                       Value = CodActividad
                                                   },
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@Mes",
                                                       Value = Mes
                                                   }
                                           };

                var dtDocumentos = consultas.ObtenerDataTable("MD_Consultar_Documento");

                HttpContext.Current.Session["dtDocumentos"] = dtDocumentos;
                GrvDocumentos.DataSource = dtDocumentos;
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
        /// Sorteo/order by de los documentos.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
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
        /// Volver a la página anterior.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Redirect(null, "CatalogoActividadPOInterventor.aspx?do=" + Request.QueryString["do"] + "&prj=" + Request.QueryString["prj"] + "&act=" + Request.QueryString["act"] + "&mes=" + Request.QueryString["mes"], "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// REVISAR ARCHIVO "UploadInterventoria", "Class_Uploader" y archivos relacionados,
        /// </summary>
        /// <returns></returns>
        private string Validar()
        {
            string msg = "";

            try
            {
                #region Obtener el nombre válido del archivo.

                switch (NombreArchivo)
                {
                    case "%":
                        NombreArchivo = NombreArchivo.Replace("%", "");
                        break;
                    case "'":
                        NombreArchivo = NombreArchivo.Replace("'", "");
                        break;
                    case "&":
                        NombreArchivo = NombreArchivo.Replace("&", "");
                        break;
                    case "?":
                        NombreArchivo = NombreArchivo.Replace("?", "");
                        break;
                    case "#":
                        NombreArchivo = NombreArchivo.Replace("#", "");
                        break;
                    default:
                        break;
                }

                #endregion

                //Si el archivo existe...
                if (File.Exists("path"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El archivo que intenta subir ya existe, deberá cambiarle el nombre y volver a intentarlo.')", true);
                }

                if (NomDocumento.Text.Trim().Length > 80)
                {
                    //Exceso de caracteres del nombre del documento.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El archivo que intenta subir posee un nombre demasiado extenso, deberá cambiarle el nombre por uno mas corto.')", true);
                }

                switch (FormatoArchivo)
                {
                    case "jpg":
                    case "bmp":
                    case "gif":
                    case "tif":
                    case "png":
                        if (TamanoArchivo > 10000000)
                        { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('la imagen " + FormatoArchivo + " que intenta subir tiene un tamaño de " + TamanoArchivo + "bytes es muy pesada, debera optimizarla o convertirla a otro formato como jpg de menor tamaño.')", true); }
                        break;
                    default:
                        break;
                }

                return msg;
            }
            catch (Exception ex)
            { msg = "Error: " + ex.Message; return msg; }
        }

        /// <summary>
        /// Crear el nuevo documento...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateString("Comentario", Comentario.Text,true,255);

                //var basePath = System.Configuration.ConfigurationManager.AppSettings["RutaDocumentosInterventoria"] + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\";
                var basePath = System.Configuration.ConfigurationManager.AppSettings.Get("RutaIP") + System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\";
                if(Archivo.HasFile)
                {	
					if (!Regex.IsMatch(Archivo.FileName, @"^[a-zA-Z0-9. áéíóúÁÉÍÓÚñÑ]+$"))
					{
						throw new ApplicationException("El nombre del archivo no debe contener caracteres especiales.");
					}

                    var nombreArchvio = DateTime.Now.Millisecond + Archivo.FileName.RemoveAccent();
                    if(!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    Archivo.SaveAs(basePath + nombreArchvio);
                    var codActvidadf = Session["CodActividad"].ToString();
                    //var nomDoc = Path.GetFileNameWithoutExtension(basePath + nombreArchvio);
                    var nomDoc = NomDocumento.Text; // 
                    var codContac = usuario.IdContacto.ToString();
                    var dirUrl = (System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\" + nombreArchvio).Replace(@"\", @"/");
                    var mesAct = Session["linkid"].ToString();
                    var extencion = (from df in consultas.Db.DocumentoFormatos
                                      where df.Extension == Path.GetExtension(basePath + nombreArchvio)
                                      select df).FirstOrDefault();
                    var fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var borrado = "0";
                    var comentario = Comentario.Text;
                    var codtipointv = "1";

                    var query = "Insert Into AvanceActividadPOAnexos Values(" + codActvidadf + ",'" + nomDoc + "'," + codContac + ",'" + dirUrl + "'," + mesAct + "," + extencion.Id_DocumentoFormato.ToString() + ",TRY_PARSE('" + fecha + "' AS DATETIME USING 'en-GB')," + borrado + ",'" + comentario + "'," + codtipointv + ")";
                    ejecutaReader(query, 2);

					CodActividad = Convert.ToInt32(codActvidadf);
					Mes = Convert.ToInt32(mesAct);

					var avancesActividad = (from aa in consultas.Db.AvanceActividadPOMes
											where aa.CodActividad == CodActividad && aa.Mes == Mes //int.Parse(parametros[0])
											select aa).ToList();

					foreach (var avance in avancesActividad)
					{
						avance.FechaAvance = DateTime.Now;
					}

					consultas.Db.SubmitChanges();

					ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Documento registrado.'); window.opener.location.reload()", true);
                    Response.Redirect("CatalogoActividadPOInterventor.aspx?do=" + Request.QueryString["do"] + "&prj=" + Request.QueryString["prj"] + "&act=" + Request.QueryString["act"] + "&mes=" + Request.QueryString["mes"]);
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }

        }

        /// <summary>
        /// Cargar DropDownList de tipos de documento.
        /// </summary>
        private void CargarTiposDeDocumento()
        {
            txtSQL = "SELECT * FROM TipoDocumento";
            var RS = consultas.ObtenerDataTable(txtSQL, "text");

            dd_TipoInterventor.DataSource = RS;
            dd_TipoInterventor.DataTextField = "NomTipoDocumento";
            dd_TipoInterventor.DataValueField = "Id_TipoDocumento";
            dd_TipoInterventor.DataBind();
            dd_TipoInterventor.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        private static void starter(){
            Array.ForEach<HttpPostedFile>(files, asyncFileProc);
        }

        private static void asyncFileProc(HttpPostedFile fileContent){
            var fileIdx = files.Length;
            var filePath = string.Format(System.Configuration.ConfigurationManager.AppSettings["Avance"], "Documentos", "PlanOperativo", fileIdx, fileContent.FileName);
            fileContent.SaveAs(filePath);
            var codigoformato_ = new DataTable();
            codigoformato_.Load(new Clases.genericQueries().executeQueryReader("SELECT * FROM [dbo].[DocumentoFormato]"));
            var uhb = codigoformato_.Select(string.Format("Extension='{0}'", (fileContent.FileName.Substring(fileContent.FileName.LastIndexOf('.'), fileContent.FileName.Length - fileContent.FileName.LastIndexOf('.')))));
            _codigoformato = uhb != null ? uhb.FirstOrDefault()["Id_DocumentoFormato"].ToString() : "0";
            _url = filePath;
            _codtipointv = "1";
            _nombre = fileContent.FileName;
            _borrado = "0";
            _fecha = DateTime.Today.ToShortDateString();
            var _qry = string.Format("INSERT INTO [dbo].[AvanceActividadPOAnexos]" +
            "([CodActividad],[NomDocumento],[CodContacto],[URL],[Mes],[CodDocumentoFormato],[Fecha],[Borrado],[Comentario],[CodTipoInterventor])" + 
            "VALUES ({0},'{1}',{2},'{3}',{4},{5},Convert(datetime,'{6}',103),{7},'{8}',{9})", 
            _codActividad, _nombre, _codcontacto, _url, _mes, _codigoformato, _fecha, _borrado, _comentario, _codtipointv);
            var rdx = new Clases.genericQueries().executeQueryReader(_qry, 1);
            files = null;
            _trsct = true;
            System.Threading.Thread.CurrentThread.Join();
        }

        private void EditarArchvio(string idFile)
        {
            var txt = "Select * from AvanceActividadPOAnexos where Id = " + idFile;
            var query = consultas.ObtenerDataTable(txt, "text");
            if(query.Rows.Count > 0)
            {
                NomDocumento.Text = query.Rows[0].ItemArray[1].ToString();
                dd_TipoInterventor.SelectedValue = query.Rows[0].ItemArray[5].ToString();
                Comentario.Text = query.Rows[0].ItemArray[8].ToString();
            }

        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            var nombreArchvio = string.Empty;
            var dirUrl = string.Empty;
            var basePath = System.Configuration.ConfigurationManager.AppSettings.Get("RutaIP") + System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\";
            if (Archivo.HasFile)
            {
                nombreArchvio = DateTime.Now.Millisecond + Archivo.FileName.RemoveAccent();
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                Archivo.SaveAs(basePath + nombreArchvio);
                dirUrl = ", Url = '"+  (System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodAct" + Session["CodActividad"].ToString() + @"\mes" + Session["linkid"].ToString() + @"\" + nombreArchvio).Replace(@"\", @"/") + "'";
            }
            var codActvidadf = Session["CodActividad"].ToString();
            var nomDoc = NomDocumento.Text; // Path.GetFileNameWithoutExtension(basePath + nombreArchvio);
            var codContac = usuario.IdContacto.ToString();
            var mesAct = Session["linkid"].ToString();
            var extencion = (from df in consultas.Db.DocumentoFormatos
                             where df.Extension == Path.GetExtension(basePath + nombreArchvio)
                             select df).FirstOrDefault();
            var fecha = DateTime.Today.ToShortDateString();
            var comentario = Comentario.Text;

            //var idFile = Request.QueryString["idFile"];
            var idFile = GetSessiones("idFile");

            var query = "Update AvanceActividadPOAnexos Set NomDocumento = '" + nomDoc + "'" + dirUrl + ", Fecha = TRY_PARSE('" + fecha + "' AS DATETIME USING 'en-GB'), Comentario = '" + comentario + "' Where Id = " + idFile;

            ejecutaReader(query, 2);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Documento actualizado!'); window.opener.location.reload()", true);

            //Response.Redirect("CatalogoActividadPOInterventor.aspx?do=" + Request.QueryString["do"] + "&prj=" + Request.QueryString["prj"] + "&act=" + Request.QueryString["act"] + "&mes=" + Request.QueryString["mes"]);
            string queryDo = GetSessiones("do");
            string queryPrj = GetSessiones("prj");
            string queryAct = GetSessiones("act");
            string queryMes = GetSessiones("mes");

            Response.Redirect("CatalogoActividadPOInterventor.aspx?do=" + queryDo + "&prj=" 
                + queryPrj + "&act=" + queryAct + "&mes=" + queryMes);
        }

        protected void GrvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("VerArchivo"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros;
                    parametros = e.CommandArgument.ToString().Split(';');

                    var nombreArchivo = parametros[0];
                    var urlArchivo = parametros[1];

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombreArchivo);
                    Response.TransmitFile(urlArchivo);
                    Response.End();
                }
            }

            if (e.CommandName == "Borrar")
            {
                var id = e.CommandArgument.ToString();
                consultas.Db.ExecuteCommand("update AvanceActividadPOAnexos set Borrado = 1 where Id =  " + id);
                Response.Redirect("CatalogoDocumentoInterventoria.aspx");
            }
        }

        protected void GrvDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var host = HttpContext.Current.Request.Url.Host;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnk = (HyperLink)e.Row.FindControl("lnkDoc");
                var ruta = "/" + lnk.NavigateUrl;
                lnk.NavigateUrl = ruta;
            }
        }

    }
}