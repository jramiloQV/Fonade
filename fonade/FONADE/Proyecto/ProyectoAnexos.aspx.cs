using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.IO;
using Fonade.Controles;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Globalization;
using System.Data.SqlClient;
using Fonade.Negocio.Utility;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoAnexos : Negocio.Base_Page
    {
        private string codProyecto;
        private string codConvocatoria;
        public int txtTab = Constantes.CONST_Anexos;
        String[] yyyys = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        public Boolean esMiembro;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        public Boolean bRealizado;
        /// <summary>
        /// Código del estado.
        /// </summary>
        public Int32 CodigoEstado;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los valores necesarios
            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "";

            inicioEncabezado(codProyecto, codConvocatoria, txtTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, codConvocatoria);

            //Consultar el "Estado" del proyecto.
            CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, codConvocatoria);

            if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && CodigoEstado == Constantes.CONST_Inscripcion)
            { 
                pnlAdicionarAnexos.Visible = true;                
            }

            if (CodigoEstado == Constantes.CONST_Inscripcion)
            {
                pnlDocumentosDeEvaluacion.Visible = false;
            }
            else
            {
                pnlDocumentosDeEvaluacion.Visible = true;
            }

            if (CodigoEstado == Constantes.CONST_Evaluacion)
            {                
                tb_eval.Visible = true;

                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor)
                { pnlAdicionarDocumentoEvaluacion.Visible = true; }
            }

            if (!IsPostBack)
            {
                CargarGridAnexos();
                CargarGridDocumentosEvaluacion();
                CargarGridDocumentosAcreditacion();
                CargarArchivosContrato();
            }
            if(Archivo.HasFile)
            {
                Session["NombreArchivo"] = Archivo.FileName;
            }
        }
       
        public void CargarArchivosContrato()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                var entities = db.ContratosArchivosAnexos
                         .Where(
                                selector =>
                                selector.CodProyecto.Equals(Convert.ToInt32(codProyecto)))
                         .Select(
                                filter => new SoporteHelper.Archivos.ArchivoContrato
                                {
                                    Id = filter.IdContratoArchivoAnexo,
                                    Nombre = filter.NombreArchivo,
                                    CodigoProyecto = filter.CodProyecto.GetValueOrDefault(0),
                                    Url = filter.ruta
                                }).ToList();
                
                gvContratos.DataSource = entities;
                gvContratos.DataBind();
            }
        }

        protected void gvFormulacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
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
            }
            catch (Exception ex)
            {                
            }
        }

        protected void CargarGridAnexos()
        {
            string consulta = " select cast(id_documento as int) as Id_Documento, cast(nomdocumento as varchar(100)) as NombreDocumento, cast(fecha as datetime) as Fecha, cast(url as varchar(1000)) as URL, cast(nomdocumentoformato as varchar(100)) as NombreDocumentoFormato, case icono when NULL then 'IcoDocNormal.gif' else  cast(icono as varchar(100)) end as Icono, cast(CodDocumentoFormato as int) as CodigoDocumentoFormato, cast(nomtab as varchar(100))  as NomTab";
            consulta += " from  documentoformato, tab  RIGHT OUTER JOIN documento d on id_tab=d.codtab ";
            consulta += " where id_documentoformato=coddocumentoformato ";
            consulta += " and codestado={0} and borrado=0 and codproyecto ={1}  ";
            consulta += " and CodDocumentoFormato <> 19 and CodDocumentoFormato <>17 order by nomdocumento ";

            IEnumerable<BORespuestaAnexos> respuesta = consultas.Db.ExecuteQuery<BORespuestaAnexos>(consulta, Constantes.CONST_Inscripcion, Convert.ToInt32(codProyecto));

            DataTable datos = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("Id_Documento");
            datos.Columns.Add("URL");
            datos.Columns.Add("icono");
            datos.Columns.Add("nombre");
            datos.Columns.Add("tab");
            datos.Columns.Add("fecha");

            foreach (BORespuestaAnexos item in respuesta)
            {
                DataRow dr = datos.NewRow();
                dr["CodProyecto"] = codProyecto;
                dr["Id_Documento"] = item.Id_Documento;
                dr["URL"] = item.URL;
                dr["icono"] = item.Icono;
                dr["nombre"] = item.NombreDocumento.htmlDecode();
                dr["tab"] = item.NomTab;
                //dr["fecha"] = string.Format("{0: MMM d} de {1: yyyy HH:mm:ss tt}", item.Fecha, item.Fecha);
                dr["fecha"] = yyyys[item.Fecha.Month] + item.Fecha.ToString(" d 'de' yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                datos.Rows.Add(dr);
            }

            gw_Anexos.DataSource = datos;
            gw_Anexos.DataBind();

            for (int i = 0; i < gw_Anexos.Rows.Count; i++)
            {
                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && CodigoEstado == Constantes.CONST_Inscripcion)
                {
                    ((ImageButton)gw_Anexos.Rows[i].Cells[0].FindControl("btn_Borrar")).Visible = true;
                    ((Button)gw_Anexos.Rows[i].Cells[2].FindControl("btnEditar")).Visible = true;
                    ((Label)gw_Anexos.Rows[i].Cells[2].FindControl("lblEditar")).Visible = false;
                }
            }
        }

        protected void CargarGridDocumentosEvaluacion()
        {
            string consulta = " select cast(id_documento as int) as Id_Documento, cast(nomdocumento as varchar(100)) as NombreDocumento, cast(fecha as datetime) as Fecha, cast(url as varchar(1000)) as URL, cast(nomdocumentoformato as varchar(1000)) as NombreDocumentoFormato, case icono when NULL then 'IcoDocNormal.gif' else  cast(icono as varchar(100)) end as Icono, cast(CodDocumentoFormato as int) as CodigoDocumentoFormato, cast(nomtab as varchar(100))  as NomTab ";
            consulta += " from  documentoformato, tab RIGHT OUTER JOIN documento d on id_tab=d.codtab ";
            consulta += " where id_documentoformato=coddocumentoformato  and codestado={0} and borrado=0 ";
            consulta += " and codproyecto ={1} and CodDocumentoFormato <> 19 and CodDocumentoFormato <>17 order by nomdocumento ";

            IEnumerable<BORespuestaAnexos> respuesta = consultas.Db.ExecuteQuery<BORespuestaAnexos>(consulta, Constantes.CONST_Evaluacion, Convert.ToInt32(codProyecto));

            DataTable datos = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("Id_Documento");
            datos.Columns.Add("URL");
            datos.Columns.Add("icono");
            datos.Columns.Add("nombre");
            datos.Columns.Add("tab");
            datos.Columns.Add("fecha");

            foreach (BORespuestaAnexos item in respuesta)
            {
                DataRow dr = datos.NewRow();
                dr["CodProyecto"] = codProyecto;
                dr["Id_Documento"] = item.Id_Documento;
                dr["URL"] = item.URL;
                dr["icono"] = item.Icono;
                dr["nombre"] = item.NombreDocumento.htmlDecode();
                dr["tab"] = item.NomTab;

                dr["fecha"] = yyyys[item.Fecha.Month] + item.Fecha.ToString(" d 'de' yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                datos.Rows.Add(dr);
            }

            gw_DocumentosEvaluacion.DataSource = datos;
            gw_DocumentosEvaluacion.DataBind();

            for (int i = 0; i < gw_DocumentosEvaluacion.Rows.Count; i++)
            {
                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && CodigoEstado == Constantes.CONST_Evaluacion)
                {
                    ((ImageButton)gw_DocumentosEvaluacion.Rows[i].Cells[0].FindControl("btn_Borrar")).Visible = true;
                    ((LinkButton)gw_DocumentosEvaluacion.Rows[i].Cells[2].FindControl("btnEditar")).Visible = true;
                    ((Label)gw_DocumentosEvaluacion.Rows[i].Cells[2].FindControl("lblEditar")).Visible = false;
                }
            }
        }

        protected void CargarGridDocumentosAcreditacion()
        {
            var consulta = "Select ca.Ruta, ce.TituloObtenido, ne.NomNivelEstudio, c.Nombres, c.Apellidos, t1.Texto TipoArchivo, T2.Texto TipoArchivoDescripcion,";
            consulta += "Case when ca.CodContactoEstudio Is NULL Then '' else ce.TituloObtenido + ' (' + ne.NomNivelEstudio + ')' end Descripcion,";
            consulta += "IsNULL(ce.anotitulo, datepart(year, getdate())) ano_titulo from ContactoArchivosAnexos ca";
            consulta += " Left Join Contacto c on c.Id_Contacto = ca.CodContacto";
            consulta += " Left Join ContactoEstudio ce on ce.Id_ContactoEstudio = ca.CodContactoEstudio";
            consulta += " Left join NivelEstudio ne on ne.Id_NivelEstudio = ce.CodNivelEstudio";
            consulta += " Inner join texto t1 on t1.NomTexto = ca.TipoArchivo";
            consulta += " Inner Join texto t2 on t2.NomTexto = ca.TipoArchivo + '_desc'";
            consulta += " Where ca.codProyecto = " + codProyecto ;

            //IEnumerable<BORespuestaDocumentosAcreditacion> respuesta = consultas.Db.ExecuteQuery<BORespuestaDocumentosAcreditacion>(consulta, Convert.ToInt32(codProyecto));

            var dt = consultas.ObtenerDataTable(consulta, "text");

            DataTable datos = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("Id_Documento");
            datos.Columns.Add("URL");
            datos.Columns.Add("icono");
            datos.Columns.Add("tipo");
            datos.Columns.Add("nombre");
            datos.Columns.Add("descripcion");
            //falta el (/) a la izquierd de la ruta
            foreach(DataRow fila in dt.Rows)
            {
                var dr = datos.NewRow();
                dr["CodProyecto"] = codProyecto;
                var ruta = fila["Ruta"].ToString().Replace("\\\\10.3.3.118\\Documentos\\\\", "").Replace("//", @"\");
                dr["URL"] = ruta;
                dr["tipo"] = fila["TipoArchivo"].ToString();
                dr["nombre"] = fila["Nombres"].ToString() + " " + fila["Apellidos"].ToString();
                if(string.IsNullOrEmpty(fila["Descripcion"].ToString()))
                {
                    dr["descripcion"] = fila["TipoArchivo"].ToString() + " - " + fila["TipoArchivoDescripcion"].ToString();
                }
                else
                {
                    dr["descripcion"] = fila["TipoArchivo"].ToString() + " - " + fila["Descripcion"].ToString() + " - " + fila["TipoArchivoDescripcion"].ToString();
                }
                datos.Rows.Add(dr);
            }

            gw_DocumentosAcreditacion.DataSource = datos;
            gw_DocumentosAcreditacion.DataBind();
        }

        private Documento getDocumentoActual(string idDocumento)
        {
            var query = (from p in consultas.Db.Documentos
                         where p.Id_Documento == Convert.ToInt32(idDocumento)
                         select p).First();

            return query;

        }

        protected void gw_Anexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void gw_DocumentosEvaluacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void gw_DocumentosAcreditacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void AccionGrid(string accion, string argumento)
        {
            string Id_Documento = "0";

            switch (accion)
            {
                case "VerDocumento":

                    string url = string.Empty;
                
                    // Se envía vacio paa detectar los errores y se debe cambiar en la grilla para que se utilice un hipervinculo
                    string saveLocation = "";

                    url = saveLocation;

                    DescargarArchivo(url);
                    break;
                case "Editar":
                    Id_Documento = argumento;
                    CarcarFormularioEdicion(Id_Documento);
                    break;
                case "Borrar":
                    Id_Documento = argumento;

                    Documento datoActual = getDocumentoActual(Id_Documento);
                    datoActual.Borrado = true;

                    consultas.Db.SubmitChanges();
                    CargarGridAnexos();
                    CargarGridDocumentosEvaluacion();
                    break;
            }
        }

        protected void btnAdicionarInversion_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlCrearDocumento.Visible = true;
            btnCrearAnexo.Text = "Crear";
            txtNombreDocumento.Text = "";
            txtLink.Text = "";
            txtComentario.Text = "";
        }

        private void CarcarFormularioEdicion(string idDocumento)
        {
            pnlPrincipal.Visible = false;
            pnlCrearDocumento.Visible = true;
            Session["idDocumento2"] = idDocumento;

            txtNombreDocumento.Text = "";
            txtLink.Text = "";
            txtComentario.Text = "";

            var query = (from d in consultas.Db.Documentos
                         from pf in consultas.Db.DocumentoFormatos
                         where d.CodDocumentoFormato == pf.Id_DocumentoFormato &&
                         d.Id_Documento == Convert.ToInt32(idDocumento)
                         select new { d.NomDocumento, d.URL, d.Comentario, pf.Extension }
                             ).First();

            txtNombreDocumento.Text = query.NomDocumento.htmlDecode();
            tdLink.Visible = false;
            if (query.Extension.Trim().ToLower() == "link")
            {
                txtLink.Text = query.URL;
                tdLink.Visible = true;
            }
            hddIdDocumento.Value = idDocumento;
            tdSubir.Visible = false;
            txtComentario.Text = query.Comentario;
            btnCrearAnexo.Text = "Actualizar";
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 28/05/2014.
        /// Limitado por tamaño la carga de archivos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCrearAnexo_Click(object sender, EventArgs e)
        {
            //Inicializar variables.
            ClientScriptManager cm = this.ClientScript;

            if (btnCrearAnexo.Text != "Actualizar")
            {
                if (!Archivo.HasFile)
                {
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No ha subido ningún archivo.');</script>");
                    return;
                }
                else
                {
                    if (Archivo.PostedFile.ContentLength > 10485760) // = 10MB
                    {
                        cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('El tamaño del archivo debe ser menor a 10 Mb.');</script>");
                        return;
                    }
                    else
                    {
                        #region Procesar la información adjunta al archivo seleccionado.
                        if (hddIdDocumento.Value == "")
                        {
                            string txtFormato = "Link";
                            string codFormato = String.Empty;
                            string NomFormato = String.Empty;
                            string RutaHttpDestino = String.Empty;
                            String Filename = String.Empty;
                            int CodCarpeta = 0;

                            if (txtLink.Text == "")
                            {
                                string[] extencion = Archivo.PostedFile.FileName.ToString().Trim().Split('.');
                                txtFormato = "." + extencion[extencion.Length - 1];
                            }
                            if (txtFormato.ToLower() == ".asp" || txtFormato.ToLower() == "php" || txtFormato.ToLower() == "xml" || txtFormato.ToLower() == "aspx" || txtFormato.ToLower() == "exe")
                            {
                                lblMensajeError.Text = "El Archivo que intenta adjuntar no es permitido";
                            }

                            try
                            {
                                var query = (from df in consultas.Db.DocumentoFormatos
                                             where df.Extension == txtFormato
                                             select new { df.Id_DocumentoFormato, df.NomDocumentoFormato }).FirstOrDefault();

                                codFormato = query.Id_DocumentoFormato.ToString(); ;
                                NomFormato = query.NomDocumentoFormato;
                            }
                            catch
                            {

                                DocumentoFormato datos = new DocumentoFormato();
                                datos.NomDocumentoFormato = "Archivo " + txtFormato;
                                datos.Extension = txtFormato;
                                datos.Icono = "IcoDocNormal.gif";

                                consultas.Db.DocumentoFormatos.InsertOnSubmit(datos);
                                consultas.Db.SubmitChanges();

                                var query = (from df in consultas.Db.DocumentoFormatos
                                             where df.Extension == txtFormato
                                             select new { df.Id_DocumentoFormato, df.NomDocumentoFormato }).FirstOrDefault();

                                codFormato = query.Id_DocumentoFormato.ToString(); ;
                                NomFormato = query.NomDocumentoFormato;
                            }
                            if (txtLink.Text == "")
                            {
                                CodCarpeta = Convert.ToInt32(codProyecto) / 2000;
                                RutaHttpDestino = ConfigurationManager.AppSettings.Get("DirVirtual") + "FonadeDocumentos\\Anexos\\Usuario" + usuario.IdContacto + "\\Anexos\\";
                                var rutaFull = ConfigurationManager.AppSettings.Get("RutaIP") + RutaHttpDestino;

                                if (!System.IO.Directory.Exists(rutaFull))
                                {
                                    System.IO.Directory.CreateDirectory(rutaFull);
                                }

                                Filename = System.IO.Path.GetFileName(Archivo.FileName);
                                //RutaHttpDestino = RutaHttpDestino + Filename;

                                // yo --> 17 08 2016
                                //if (!CargarArchivoServidor(Archivo, RutaHttpDestino, Archivo.FileName.Substring(0, Archivo.FileName.ToString().IndexOf('.')).cleanSpecialChars(true), txtFormato.Substring(1), ConfigurationManager.AppSettings.Get("RutaDocumentosTEMP")))
                                    //{
                                    //    lblMensajeError.Text = respuesta.Mensaje;
                                    //    return;
                                    //}

                                Archivo.SaveAs(rutaFull + Session["NombreArchivo"].ToString());

                                //Archivo.SaveAs
                                //Cambiar la forma de hacerlo
                                //RutaHttpDestino = ConfigurationManager.AppSettings.Get("DirVirtual2")+"Proyecto/" + Math.Abs(Convert.ToInt32(codProyecto) / 2000) + "/" + @"Proyecto_" + codProyecto + @"/" + Archivo.FileName.ToString();
                                RutaHttpDestino = RutaHttpDestino + Session["NombreArchivo"].ToString();


                            }
                            else
                            {
                                if (txtLink.Text.Contains("http://") == false)
                                {
                                    txtLink.Text = "http://" + txtLink.Text;
                                }
                                RutaHttpDestino = txtLink.Text;
                            }

                            string txtSQL = "INSERT INTO Documento(NomDocumento,URL,Fecha,CodProyecto,CodDocumentoFormato,CodContacto,Comentario,Borrado,CodTab,CodEstado)" +
                                            "VALUES('" + txtNombreDocumento.Text.htmlEncode() + "','" + RutaHttpDestino + "',getDate()," + codProyecto + ",'" + codFormato + "'," + usuario.IdContacto + ",'" + txtComentario.Text + "',0,'" + txtTab + "'," + CodigoEstado.ToString() + ")";
                            RutaHttpDestino = null;
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            try
                            {
                                //NEW RESULTS:

                                SqlCommand Cmd = new SqlCommand(txtSQL, con);

                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                Cmd.CommandType = CommandType.Text;
                                Cmd.ExecuteNonQuery();
                                //con.Close();
                                //con.Dispose();
                                Cmd.Dispose();


                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Archivo fue guardado'); window.close();", true);
                                Response.Redirect(Request.RawUrl);
                                //return;
                            }
                            catch
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo generar el registro');", true);
                                return;
                            }
                            finally
                            {
                                con.Close();
                                con.Dispose();
                            }

                        }
                        else
                        {
                            SqlCommand cmd = new SqlCommand();

                            try
                            {
                                //Borrar la inversión
                                string txtSQL = " Update Documento set NomDocumento ='" + txtNombreDocumento.Text.htmlEncode() + "'," +
                                         " Comentario = '" + txtComentario.Text + "'," +
                                         " codTab = " + txtTab;

                                if (txtLink.Text != "") { txtSQL = txtSQL + ", Url='" + txtLink.Text + "'"; }

                                txtSQL = txtSQL + " WHERE Id_Documento = " + hddIdDocumento.Value;
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
                                }
                                catch { }
                                finally
                                {
                                    con.Close();
                                    con.Dispose();
                                }
                            }
                            catch { }

                            hddIdDocumento.Value = string.Empty;
                        }
                        //consultas.Db.SubmitChanges();
                        CargarGridAnexos();

                        pnlPrincipal.Visible = true;
                        pnlCrearDocumento.Visible = false;
                        #endregion
                    }
                }
            }
            else
            {
                var documento = (from d in consultas.Db.Documentos
                                 where d.Id_Documento == Convert.ToInt32(Session["idDocumento2"].ToString())
                                 select d).FirstOrDefault();
                documento.NomDocumento = txtNombreDocumento.Text.Trim();
                documento.Comentario = txtComentario.Text.Trim();
                consultas.Db.SubmitChanges();
                CargarGridAnexos();

                pnlPrincipal.Visible = true;
                pnlCrearDocumento.Visible = false;
                tdSubir.Visible = true;

            }
        }

        protected void btnCerrarAnexo_Click(object sender, EventArgs e)
        {
            pnlPrincipal.Visible = true;
            pnlCrearDocumento.Visible = false;
            hddIdDocumento.Value = "";
        }

        #region Métodos de Mauricio Arias Olave.

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
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

        #endregion

        protected void Image2_Click(object sender, ImageClickEventArgs e)
        {
            pnlPrincipal.Visible = false;
            pnlCrearDocumento.Visible = true;
            btnCrearAnexo.Text = "Crear";
            txtNombreDocumento.Text = "";
           // txtLink.Text = "";
            txtComentario.Text = "";
        }
    }

    public class BORespuestaAnexos
    {
        public int Id_Documento { get; set; }
        public string NombreDocumento { get; set; }
        public DateTime Fecha { get; set; }
        public string URL { get; set; }
        public string NombreDocumentoFormato { get; set; }
        public string Icono { get; set; }
        public int CodigoDocumentoFormato { get; set; }
        public string NomTab { get; set; }
    }

    public class BORespuestaDocumentosAcreditacion
    {

        public string Ruta { get; set; }
        public string TituloObtenido { get; set; }
        public string NomNivelEstudio { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string TipoArchivo { get; set; }
        public string TipoArchivoDescripcion { get; set; }
        public string Descripcion { get; set; }
        public string ano_titulo { get; set; }
    }
}
