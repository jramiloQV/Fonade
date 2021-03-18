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
using System.IO;
using System.Configuration;
using Fonade.Clases;
using System.Text.RegularExpressions;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoNominaInter : Negocio.Base_Page
    {
        String CodNomina;
        int Mes;
        String txtSQL;

        /// <summary>
        /// Si este valor contiene datos, significa que se generará un nuevo documento.
        /// </summary>
        String Accion_Docs;

        /*Variables de instancia para registro de carga de archivos.*/
        static string _codCargo { get; set; } static string _mes { get; set; } static string _codcontacto { get; set; } static string _url { get; set; }
        static string _codigoformato { get; set; } static string _fecha { get; set; } static string _borrado { get; set; } static string _comentario { get; set; }
        static string _codtipointv { get; set; } static string _nombre { get; set; } static bool _trsct { get; set; }
        static HttpPostedFile[] files { get; set; }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CodNomina = Convert.ToString(HttpContext.Current.Session["CodNomina"]??"0");
                Mes = Convert.ToInt32(HttpContext.Current.Session["MesDeLaNomina"]??0);
                Accion_Docs = Convert.ToString(HttpContext.Current.Session["Accion_Docs"]??string.Empty);
                var Accion = Convert.ToString(Request.QueryString["Accion"]??string.Empty);

                if(string.IsNullOrEmpty(Accion))
                {
                    if (Accion_Docs == "NuevoDocumento")
                    {
                        pnl_NuevoDoc.Visible = true;
                        pnlPrincipal.Visible = false;
                    }
                    else
                    {
                        pnl_NuevoDoc.Visible = false;
                        pnlPrincipal.Visible = true;
                    }
                }
                else
                {
                    pnl_NuevoDoc.Visible = true;
                    pnlPrincipal.Visible = false;
                    var parametros = Request.QueryString["Parametros"].ToString();
                    CargardatosEdit(parametros);
                }

                CargarTiposDeDocumento();
                ValidarGrupo();
            }
        }

        private void ValidarGrupo()
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            {
            }

            try
            {
                consultas.Parameters = null;

                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@CodCargo",
                                                       Value = CodNomina
                                                   },
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@Mes",
                                                       Value = Mes
                                                   }
                                           };

                var dtDocumentos = consultas.ObtenerDataTable("MD_Consultar_Documento_Nomina");

                if (dtDocumentos.Rows.Count != 0)
                {
                    HttpContext.Current.Session["dtDocumentos"] = dtDocumentos;
                    GrvDocumentos.DataSource = dtDocumentos;
                    GrvDocumentos.DataBind();
                }
                else
                {
                    GrvDocumentos.DataSource = dtDocumentos;
                    GrvDocumentos.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

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

        protected void GrvDocumentosPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CodNomina = Convert.ToString(HttpContext.Current.Session["CodNomina"] ?? "0");
            Mes = Convert.ToInt32(HttpContext.Current.Session["MesDeLaNomina"] ?? 0);

            GrvDocumentos.PageIndex = e.NewPageIndex;
            GrvDocumentos.DataSource = HttpContext.Current.Session["dtDocumentos"];
            GrvDocumentos.DataBind();
        }

        protected void GrvDocumentosSorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                CodNomina = Convert.ToString(HttpContext.Current.Session["CodNomina"] ?? "0");
                Mes = Convert.ToInt32(HttpContext.Current.Session["MesDeLaNomina"] ?? 0);

                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                GrvDocumentos.DataSource = HttpContext.Current.Session["dtDocumentos"];
                GrvDocumentos.DataBind();
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Accion"] = "actualizar";
            Redirect(null, "../evaluacion/CatalogoNominaPOInterventoria.aspx", "_self", "menubar=0,scrollbars=1,width=710,height=400,top=100");
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
				if (btnCrear.Text == "Crear")
				{
					var basePath = System.Configuration.ConfigurationManager.AppSettings.Get("RutaIP") + System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodNom" + Session["idAnexoNomina"].ToString() + @"\mes" + Session["MesDeLaNomina"].ToString() + @"\";
					if (Archivo.HasFile)
					{
						if (!Regex.IsMatch(Archivo.FileName, @"^[a-zA-Z0-9. áéíóúÁÉÍÓÚñÑ]+$"))
						{
							throw new ApplicationException("El nombre del archivo no debe contener caracteres especiales.");
						}

						var nombreArchvio = DateTime.Now.Millisecond + Archivo.FileName.RemoveAccent();
						if (!Directory.Exists(basePath))
						{
							Directory.CreateDirectory(basePath);
						}
						Archivo.SaveAs(basePath + nombreArchvio);
						var codActvidadf = Session["idAnexoNomina"].ToString();
						//var nomDoc = Path.GetFileNameWithoutExtension(basePath + nombreArchvio);
						var nomDoc = NomDocumento.Text; // 
						var codContac = usuario.IdContacto.ToString();
						var dirUrl = (System.Configuration.ConfigurationManager.AppSettings.Get("DirVirtual") + @"InterventoriaProyecto\" + "CodPrj" + Session["CodProyecto"].ToString() + @"\" + "CodNom" + Session["idAnexoNomina"].ToString() + @"\mes" + Session["MesDeLaNomina"].ToString() + @"\" + nombreArchvio).Replace(@"\", @"/");
						var mesAct = Session["MesDeLaNomina"].ToString();
						var extencion = (from df in consultas.Db.DocumentoFormatos
										 where df.Extension == Path.GetExtension(basePath + nombreArchvio)
										 select df).FirstOrDefault();
						var fecha = DateTime.Today.ToShortDateString();
						var borrado = "0";
						var comentario = Comentario.Text;
						var codtipointv = "1";

						var query = "Insert Into AvanceCargoPOAnexos Values(" + codActvidadf + ",'" + nomDoc + "'," + codContac + ",'" + dirUrl + "'," + mesAct + "," + extencion.Id_DocumentoFormato.ToString() + ",getdate()," + borrado + ",'" + comentario + "'," + codtipointv + ")";
						ejecutaReader(query, 2);

						int CodActividad = Convert.ToInt32(codActvidadf);
						int Mes = Convert.ToInt32(mesAct);

						var avancesCargo = (from aa in consultas.Db.AvanceCargoPOMes
												where aa.CodCargo == CodActividad && aa.Mes == Mes //int.Parse(parametros[0])
												select aa).ToList();

						foreach (var avance in avancesCargo)
						{
							avance.FechaAvance = DateTime.Now;
						}

						consultas.Db.SubmitChanges();

						ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Documento registrado.'); window.opener.location.reload()", true);

						HttpContext.Current.Session["Accion"] = "";
						HttpContext.Current.Session["Accion_Docs"] = "List";
						Response.Redirect("CatalogoNominaInter.aspx");
					}
				}
				else
				{
					var param = Request.QueryString["Parametros"].ToString().Split(';');
					var sql = "Update AvanceCargoPOAnexos Set NomDocumento = '" + NomDocumento.Text.Trim() + "', Comentario = '" + Comentario.Text + "' " +
						"Where Mes = " + param[0] + " And NomDocumento = '" + param[1] + "' And CodCargo = " + param[2];

					ejecutaReader(sql, 2);
					HttpContext.Current.Session["Accion"] = "";
					HttpContext.Current.Session["Accion_Docs"] = "List";
					Response.Redirect("CatalogoNominaInter.aspx");
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
            var _qry = string.Format("INSERT INTO [dbo].[AvanceCargoPOAnexos]" +
            "([CodCargo],[NomDocumento],[CodContacto],[URL],[Mes],[CodDocumentoFormato],[Fecha],[Borrado],[Comentario],[CodTipoInterventor]) " +
            "VALUES ({0},'{1}',{2},'{3}',{4},{5},Convert(datetime,'{6}',103),{7},'{8}',{9}) ",
            _codCargo, _nombre, _codcontacto, _url, _mes, _codigoformato, _fecha, _borrado, _comentario, _codtipointv);
            var rdx = new Clases.genericQueries().executeQueryReader(_qry, 1);
            files = null;
            _trsct = true;
            System.Threading.Thread.CurrentThread.Join();
        }

        /// <summary>
        /// Cargar DropDownList de tipos de documento.
        /// </summary>
        private void CargarTiposDeDocumento()
        {
            try
            {
                txtSQL = "SELECT * FROM TipoDocumento";
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                dd_TipoInterventor.Items.Clear();

                foreach (DataRow row in RS.Rows)
                {
                    ListItem item = new ListItem();
                    item.Text = row["NomTipoDocumento"].ToString();
                    item.Value = row["Id_TipoDocumento"].ToString();
                    dd_TipoInterventor.Items.Add(item);
                }
            }
            catch { }
        }

        private void CargardatosEdit(string parametros)
        {
            var param = parametros.Split(';');

            var sqlText = "Select * from AvanceCargoPOAnexos where Mes = " + param[0] + " And NomDocumento= '" + param[1] + "' And CodCargo = " + param[2] + " And Borrado = 0";
            var dt = consultas.ObtenerDataTable(sqlText, "text");

            NomDocumento.Text = dt.Rows[0].ItemArray[1].ToString();
            Comentario.Text = dt.Rows[0].ItemArray[8].ToString();
            dd_TipoInterventor.SelectedIndex = 1;
            btnCrear.Text = "Actualizar";
            lblSubirArchivo.Visible = false;
            Archivo.Visible = false;
        }

        protected void GrvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName.Equals("EditarDoc"))
            {
                Response.Redirect("CatalogoNominaInter.aspx?Accion=Edit&Parametros=" + e.CommandArgument.ToString());
            }

            if(e.CommandName.Equals("Borrar"))
            {
                var param = e.CommandArgument.ToString().Split(';');

                var sql = "Update AvanceCargoPOAnexos Set Borrado = 1 Where Mes = " + param[2] + " And NomDocumento = '" + param[3] + "' And CodCargo = " + param[0] + " And Borrado = 0";

                ejecutaReader(sql, 2);
                HttpContext.Current.Session["Accion"] = "";
                HttpContext.Current.Session["Accion_Docs"] = "List";
                Response.Redirect("CatalogoNominaInter.aspx");
            }
        }

        protected void GrvDocumentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                var del = e.Row.FindControl("btn_Borrar") as ImageButton;

                if (usuario.CodGrupo == Constantes.CONST_Interventor 
                    || usuario.CodGrupo == Constantes.CONST_Asesor
                    || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                    || usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                    || usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
                    del.Visible = false;

                //validar si el avance esta aprobado
                del.Visible = !validarAprobacionAvanceNomina(Convert.ToInt32(CodNomina), Mes);

                if(usuario.CodGrupo == Constantes.CONST_Asesor)
                {
                    del.Visible = false;
                }
            }
        }

        private bool validarAprobacionAvanceNomina(int _codCargo, int _codMes) 
        {
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            bool valido = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion)) {
                valido = (from a in db.AvanceCargoPOMes
                          where a.CodCargo == _codCargo && a.Mes == _codMes
                          select a.Aprobada).FirstOrDefault() ?? false;
            }

            return valido;
        }
    }
}