using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Datos;
using System.IO;

namespace Fonade.FONADE.Proyecto
{
    public partial class CatalogoDocumento : Negocio.Base_Page
    {
        #region Variables globales.

        String txtTab;
        String CodProyecto;
        String txtSQL;
        Boolean bRepetido;
        String Accion;
        Boolean Miembro;
        String CodDocumento;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
                txtTab = HttpContext.Current.Session["txtTab"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["txtTab"].ToString()) ? HttpContext.Current.Session["txtTab"].ToString() : "0";
                Accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "0";
                btn_Accion.Attributes.Add("onclick", "javascript:CerrarVentana()");
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

            //Se valida que si tenga datos "válidos".
            if (CodProyecto == "0") { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

            if (!IsPostBack)
            {
                //Consultar si es miembro del proyecto relacionado
                Miembro = fnMiembroProyecto(usuario.IdContacto, CodProyecto);

                if (Accion == "Nuevo") { btn_Accion.Text = "Crear"; LimpiarCampos(); }
                else if (Accion == "Acreditacion")
                {
                    CargarGrillaDocumentosAcreditacion();
                }
                else
                {
                    CargarGrillaDocumentos();
                }
            }
        }

        private void Eliminar(String CodDocumento)
        {
            //Inicializar las variables.
            SqlCommand cmd = new SqlCommand();

            try
            {
                //Borrar la inversión
                txtSQL = "update Documento set borrado=1 where Id_Documento = " + CodDocumento;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    //NEW RESULTS:
                    
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();
                    CargarGrillaDocumentos();
                }
                catch { }
                finally {

                    con.Close();
                    con.Dispose();
                }

                #region Modificacion para Eliminar el archivo Físicamente COMENTADO!
                ////Vladimir Delgado Barbosa 15 Mayo de 2007
                //Dim RSB
                //txtSQL = "select distinct URL from Documento where borrado=1 and Id_Documento ="&CodDocumento
                //set RSB= Conn.execute(txtSQL)
                //if Not RSB.EOF then BorrarArchivo(RSB("URL"))
                ////Fin de modificacion 
                #endregion
            }
            catch { }
        }

        private void Crear()
        {
            SqlCommand Cmd = new SqlCommand();

            try
            {
                string url = string.Empty;

                string ruta = System.IO.Path.GetFileName(Archivo.PostedFile.FileName);
                string extenion = System.IO.Path.GetExtension(Archivo.PostedFile.FileName);

                if (!Archivo.HasFile)
                {
                    extenion = "link    ";
                }

                txtSQL = "select Id_DocumentoFormato from DocumentoFormato where Extension = '" + extenion.ToLower() + "'";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if(dt.Rows.Count>0)
                {
                    if (!string.IsNullOrEmpty(Link.Text)) { url = Link.Text; }

                    //string saveLocation = "M:/FonadeDocumentos/Anexos/Usuario" + usuario.IdContacto + "/";
                    string saveLocation = ConfigurationManager.AppSettings.Get("DirVirtual") + "FonadeDocumentos\\Anexos\\Usuario" + usuario.IdContacto + "\\" + Session["TabInvoca"] + "\\";

                    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + saveLocation))
                        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("RutaIP") + saveLocation);

                    saveLocation = saveLocation + ruta;

                    if ((System.IO.File.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + saveLocation)))
                        System.IO.File.Delete(ConfigurationManager.AppSettings.Get("RutaIP") + saveLocation);
                    else
                        if (!string.IsNullOrEmpty(ruta))
                            Archivo.PostedFile.SaveAs(ConfigurationManager.AppSettings.Get("RutaIP") + saveLocation);

                    txtSQL = "INSERT INTO Documento(NomDocumento,URL,Fecha,CodProyecto,CodDocumentoFormato,CodContacto,Comentario,Borrado,CodTab,CodEstado)" +
                        "VALUES('" + NomDocumento.Text + "','" + (!string.IsNullOrEmpty(saveLocation) ? saveLocation : url) + "',getDate()," + CodProyecto + ",'" + dt.Rows[0][0].ToString() + "'," + usuario.IdContacto + ",'" +
                    Comentario.Text + "',0,'" + txtTab + "',1)";
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                    try
                    {
                        //NEW RESULTS:

                        Cmd = new SqlCommand(txtSQL, con);

                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                        Cmd.CommandType = CommandType.Text;
                        Cmd.ExecuteNonQuery();
                        //con.Close();
                        //con.Dispose();
                        Cmd.Dispose();


                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Archivo fue guardado'); window.opener.location.href = window.opener.location.href;window.close(); ", true);
                        return;
                    }
                    catch
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo generar el registro'); window.opener.location.href = window.opener.location.href;", true);
                        return;
                    }
                    finally {

                        con.Close();
                        con.Dispose();
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El ario no es alido');", true);
                    return;
                }
            }
            catch (Exception)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo generar el registro');", true);
                return;
            }
        }

        private void Actualizar()
        {
            //Inicializar las variables.
            SqlCommand cmd = new SqlCommand();

            var idDocumento = Session["CodDocumento"];

            try
            {
                //Borrar la inversión
                txtSQL = " Update Documento set NomDocumento ='" + NomDocumento.Text + "'," +
                         " Comentario = '" + Comentario.Text + "'," +
                         " codTab = " + txtTab;

                if (Link.Text != "") { txtSQL = txtSQL + ", Url='" + Link.Text + "'"; }

                txtSQL = txtSQL + " WHERE Id_Documento = " + idDocumento;
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    //NEW RESULTS:
                    
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    //con.Close();
                    //con.Dispose();
                    cmd.Dispose();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.history.go(-2);", true);
                    return;
                }
                catch { }
                finally {
                    con.Close();
                    con.Dispose();
                }
            }
            catch { }
        }

        private string Validar()
        {
            //Inicializar variables.
            String msg = "";

            try
            {
                if (bRepetido) { msg = "Ya existe un Documento con ese Nombre."; }

                if (NomDocumento.Text.Trim() == "") { msg = Texto("TXT_NOMBRE_REQ"); }

                if (Accion == "Nuevo")
                { if (!Archivo.HasFile && string.IsNullOrEmpty(Link.Text)) { msg = "El archivo o link del documento es requerido"; } }

                return msg;
            }
            catch (Exception ex) { msg = "Error: " + ex.Message; return msg; }
        }

        private void CargarInfo_Documento()
        {
            //Inicializar variables.
            DataTable RsDocumento = new DataTable();

            try
            {
                //Mostrar panel.
                pnl_datos.Visible = true;
                pnl_Grilla.Visible = false;

                txtSQL = " SELECT nomdocumento, url, comentario, extension " +
                         " FROM Documento, documentoformato " +
                         " WHERE Id_DocumentoFormato = CodDocumentoFormato and Id_Documento=" + CodDocumento;

                RsDocumento = consultas.ObtenerDataTable(txtSQL, "text");

                lblTitulo.Text = "EDITAR DOCUMENTO";

                if (RsDocumento.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(RsDocumento.Rows[0]["nomdocumento"].ToString()))
                    { NomDocumento.Text = RsDocumento.Rows[0]["nomdocumento"].ToString(); }

                    if (!String.IsNullOrEmpty(RsDocumento.Rows[0]["Extension"].ToString()) && RsDocumento.Rows[0]["Extension"].ToString() == "Link")
                    { Link.Visible = true; Link.Text = RsDocumento.Rows[0]["Extension"].ToString(); }

                    if (!String.IsNullOrEmpty(RsDocumento.Rows[0]["comentario"].ToString()))
                    { Comentario.Text = RsDocumento.Rows[0]["comentario"].ToString(); }
                }

                btn_Accion.Text = "Actualizar";
            }
            catch
            {
                pnl_datos.Visible = false;
                pnl_Grilla.Visible = true;
            }
        }

        private void CargarGrillaDocumentos()
        {
            //Inicializar variables.
            DataTable RsDocumento = new DataTable();

            try
            {
                txtSQL = " select id_documento, nomdocumento, fecha, url, nomdocumentoformato, icono, CodDocumentoFormato " +
                         " from documento, documentoformato " +
                         " where id_documentoformato = coddocumentoformato and borrado = 0 " +
                         " and codproyecto = " + CodProyecto + " and URL like'%" + Session["TabInvoca"] + "%'";

                if (txtTab != "0")
                {
                    txtSQL += txtSQL + " and codTab = " + txtTab + "  order by nomdocumento";
                }

                RsDocumento = consultas.ObtenerDataTable(txtSQL, "text");

                HttpContext.Current.Session["docs"] = RsDocumento;
                gv_Documentos.DataSource = RsDocumento;
                gv_Documentos.DataBind();

                pnl_datos.Visible = false;
                pnl_Grilla.Visible = true;
                btn_Accion.Text = "Actualizar";
            }
            catch { }
        }

        private void CargarGrillaDocumentosAcreditacion()
        {
            string consulta = " SELECT  cast(a.Ruta as varchar(100)) as Ruta, cast(e.TituloObtenido as varchar(100)) as TituloObtenido, cast(es.NomNivelEstudio as varchar(100)) as NomNivelEstudio, cast(c.Nombres as varchar(100)) as Nombres, cast(c.Apellidos as varchar(100)) as Apellidos, cast(T1.Texto as varchar(100)) as TipoArchivo, cast(T2.Texto as varchar(100)) as TipoArchivoDescripcion ";
            consulta += " , cast((case when a.CodContactoEstudio is null then '' else  e.TituloObtenido + ' (' +  es.NomNivelEstudio + ')' end) as varchar(100) ) as Descripcion  ";
            consulta += " , cast((isnull(e.anotitulo,datepart(year,getdate())) ) as varchar(10)) as ano_titulo ";
            consulta += " FROM ContactoArchivosAnexos AS a ";
            consulta += " LEFT OUTER JOIN Contacto c ON c.Id_Contacto = a.CodContacto ";
            consulta += " LEFT OUTER JOIN ContactoEstudio AS e  on e.Id_ContactoEstudio = a.CodContactoEstudio";
            consulta += " LEFT OUTER JOIN NivelEstudio AS es ON e.CodNivelEstudio = es.Id_NivelEstudio ";
            consulta += " LEFT OUTER JOIN texto AS T1 ON T1.NomTexto=A.TipoArchivo ";
            consulta += " LEFT OUTER JOIN texto AS T2 ON T2.NomTexto=CONCAT(A.TipoArchivo,'_desc')  ";
            consulta += " WHERE a.CodProyecto = {0}";
            consulta += " ORDER BY a.TipoArchivo, c.Id_Contacto, ano_titulo Desc";

            IEnumerable<BORespuestaDocumentosAcreditacion> respuesta = consultas.Db.ExecuteQuery<BORespuestaDocumentosAcreditacion>(consulta, Convert.ToInt32(CodProyecto));

            DataTable datos = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("Id_Documento");
            datos.Columns.Add("URL");
            datos.Columns.Add("icono");
            datos.Columns.Add("tipo");
            datos.Columns.Add("nombre");
            datos.Columns.Add("descripcion");

            foreach (BORespuestaDocumentosAcreditacion item in respuesta)
            {

                DataRow dr = datos.NewRow();
                dr["CodProyecto"] = CodProyecto;
                dr["URL"] = item.Ruta;
                dr["tipo"] = item.TipoArchivo;
                dr["nombre"] = item.Nombres + " " + item.Apellidos;
                if (item.Descripcion != "")
                    dr["descripcion"] = item.TipoArchivo + " - " + item.Descripcion;
                else
                    dr["descripcion"] = item.TipoArchivo + " - " + item.TipoArchivoDescripcion;

                datos.Rows.Add(dr);
            }

            gw_DocumentosAcreditacion.DataSource = datos;
            gw_DocumentosAcreditacion.DataBind();
        }

        protected void btn_Accion_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String validado = "";

            validado = Validar();
            string OpenerPage = string.Empty;
            OpenerPage = "window.opener.document.getElementById('hidInsumo').value='1';";
            if (validado == "")
            {
               
                if (btn_Accion.Text == "Crear")
                {
                    Crear();
                }
                else
                {
                    if (btn_Accion.Text == "Actualizar")
                    { Actualizar(); }
                }
                 ClientScriptManager cm = this.ClientScript;
                 cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>  " + OpenerPage + "   window.opener.location.href = window.opener.location.href;window.close(); window.opener.reload(); </script>");
            }
        }

        protected void gv_Documentos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnk = e.Row.FindControl("lnk_eliminar") as LinkButton;
                var img = e.Row.FindControl("imgDoc") as ImageButton;
                var hdf = e.Row.FindControl("hdf_icono") as HiddenField;
                var lnk2 = e.Row.FindControl("lnk_NomDoc") as LinkButton;
                var lbl = e.Row.FindControl("lbl_Fecha") as Label;
                DateTime fecha = new DateTime();

                if (lnk != null && img != null && hdf != null && lnk2 != null && lbl != null)
                {
                    if (Miembro && usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        lnk.Visible = true;
                        lnk.OnClientClick = "return alerta();";
                    }
                    else
                    { lnk.Visible = false; lnk2.Enabled = false; }

                    if (hdf.Value != "")
                    { img.ImageUrl = "../../Images/" + hdf.Value; }
                    else { img.ImageUrl = "../../Images/FileMain.gif"; }

                    if (img.CommandArgument == "17")
                    {
                        //Se redirecciona cuando se cliquee a "OfflineProcesaCarga.asp"
                    }
                    else
                    {
                        //Se abre una nueva ventana emergente "_blank".
                    }

                    try
                    {
                        fecha = DateTime.Parse(lbl.Text);
                        lbl.Text = fecha.ToString();

                        #region Formatear la fecha.

                        //Obtener el nombre del mes (las primeras tres letras).
                        string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));

                        //Obtener la hora en minúscula.
                        string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();

                        //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                        if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }

                        //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                        lbl.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                        #endregion
                    }
                    catch { fecha = DateTime.Today; lbl.Text = ""; }
                }
            }
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile file = Request.Files["myFile"];

            //check file was submitted
            if (file != null && file.ContentLength > 0)
            {
                string fname = Path.GetFileName(file.FileName);
                file.SaveAs(Server.MapPath(Path.Combine("~/App_Data/", fname)));
            }
        }

        private void LimpiarCampos()
        {
            pnl_Grilla.Visible = false;
            pnl_datos.Visible = true;
            NomDocumento.Text = "";
            Link.Text = "";
            Comentario.Text = "";
            lblTitulo.Text = "NUEVO DOCUMENTO";
        }


        protected void gv_Documentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "eliminar")
            {
                Eliminar(e.CommandArgument.ToString());
            }
            if (e.CommandName == "Descargar")
            {
                /*
                 * IMPORTANTE: LA DIRECCION DE SERVIDOR DE ARCHIVOS DEBE INCLUIRSE EN LA VARIABLE DE CONFIGURACION DEL WEB CONFIG
                 * SIN ESA ESTRUCTURA DE ARCHIVOS NO FUNCIONA EL PROCESO.
                 */
                
                var qaz = 
                string.Format(System.Configuration.ConfigurationManager.AppSettings["FileServer"].ToString(),((ImageButton)e.CommandSource).AlternateText);
                qaz = HttpUtility.UrlDecode(qaz);
                System.Diagnostics.Debug.Write(qaz);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "oiu", string.Format("window.open('{0}')", Server.UrlPathEncode(qaz)), true);
                System.Diagnostics.Debug.Write(Server.UrlPathEncode(qaz));

                //Se tiene que validar a qué página vá a dirigirse, o puede ser "OfflineProcesaCarga.aspx" 
                //o se hace la descarga directa.
            }
            if (e.CommandName == "editar")
            {
                CodDocumento = e.CommandArgument.ToString();
                Session["codDocumento"] = CodDocumento;
                CargarInfo_Documento();
            }
        }
        protected void Btn_cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;window.close();", true);
        }
    }
}