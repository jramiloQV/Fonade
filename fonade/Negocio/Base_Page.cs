using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using Datos;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using LinqKit;
using System.Data.Linq.SqlClient;
using System.Configuration;
using Fonade.Controles;
using System.IO;
using Fonade.FONADE.Proyecto.Templates;
using TSHAK.Components;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Fonade.Negocio.Utility;

namespace Fonade.Negocio
{

    public class Base_Page : System.Web.UI.Page
    {

        protected FonadeUser usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set { }
        }
        [ContextStatic]
        protected Consultas consultas;

        protected bool miembro = false;
        protected bool redirect = false;
        protected bool realizado = false;
        protected int codEstado;
        protected RespuestaCargue respuesta;
        protected string errorMessageDetail;
        protected string void_establecerTitulo(string[] codgrupo, string Accion, string defaulttitle)
        {
            string temp_title = string.Empty;
            string title = string.Empty;
            if (codgrupo.Contains(Constantes.CONST_Perfil_Fiduciario.ToString())) temp_title = "USUARIO FIDUCIARIA";
            else if (codgrupo.Contains(Constantes.CONST_CallCenter.ToString())) temp_title = "USUARIO CALL CENTER";
            else if (codgrupo.Contains(Constantes.CONST_CallCenterOperador.ToString())) temp_title = "USUARIO CALL CENTER OPERADOR";
            else if (codgrupo.Contains(Constantes.CONST_GerenteEvaluador.ToString())) temp_title = "USUARIO GERENTE EVALUADOR";
            else if (codgrupo.Contains(Constantes.CONST_LiderRegional.ToString())) temp_title = "USUARIO LIDER REGIONAL";
            else if (codgrupo.Contains(Constantes.CONST_GerenteInterventor.ToString())) temp_title = "USUARIO GERENTE INTERVENTOR";
            else if (codgrupo.Contains(Constantes.CONST_PerfilAcreditador.ToString())) temp_title = "USUARIO ACREDITADOR";
            else if (codgrupo.Contains(Constantes.CONST_PerfilAbogado.ToString())) temp_title = "USUARIO ABOGADO";
            else if (codgrupo.Contains(Constantes.CONST_AdministradorSistema.ToString()) || codgrupo.Contains(Constantes.CONST_AdministradorSena.ToString())) temp_title = "USUARIO ADMINISTRADOR";
            else
            {
                temp_title = defaulttitle;
            }

            switch (Accion)
            {
                case "Editar":
                    title = "EDITAR " + temp_title;
                    break;
                case "Crear":
                    title = "NUEVO " + temp_title;
                    break;
                case "":
                    title = temp_title;
                    break;
                default: title = temp_title; break;

            }
            return title;
        }
        protected string void_establecerTitulo(string defaulttitle)
        {
            return defaulttitle;
        }

        protected override void OnLoad(EventArgs e)
        {
            consultas = new Consultas();
            base.OnLoad(e);
        }

        public Base_Page()
        {
            consultas = new Consultas();
        }

        protected void procesarCampo(ref TextBox txtBox, ref HtmlEditorExtender htmlExtender, ref HtmlGenericControl panel, string txtValue, bool Miembro, bool bRealizado, string codConvocatoria)
        {
            txtBox.Text = txtValue;
            if (Miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado && codConvocatoria == "")
            {
                panel.Visible = false;
            }
            else
            {
                if (Miembro == true && usuario.CodGrupo == Constantes.CONST_Evaluador && !bRealizado && codConvocatoria != "")
                {
                    panel.Visible = false;
                }

                else
                {
                    panel.Visible = true;
                    panel.InnerHtml = txtValue;
                    txtBox.Visible = false;
                }
            }
        }

        protected void procesarCampo(ref CKEditor.NET.CKEditorControl txtBox, ref HtmlGenericControl panel, string txtValue, bool Miembro, bool bRealizado, string codConvocatoria)
        {
            txtBox.Text = txtValue.htmlDecode();
            if (Miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado && codConvocatoria == "")
            {
                panel.Visible = false;
            }
            else
            {
                if (Miembro == true && usuario.CodGrupo == Constantes.CONST_Evaluador && !bRealizado && codConvocatoria != "")
                {
                    panel.Visible = false;
                }

                else
                {
                    panel.Visible = true;
                    panel.InnerHtml = txtValue.htmlDecode();
                    txtBox.Visible = false;
                }
            }
        }

        protected void inicioEncabezado(string codProyecto, string codConvocatoria, int txtTab)
        {

            if (usuario.CodGrupo == Constantes.CONST_Asesor
               || usuario.CodGrupo == Constantes.CONST_Emprendedor
               || usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador
               || usuario.CodGrupo == Constantes.CONST_Evaluador)
            {
                if (!fnMiembroProyecto(usuario.IdContacto, codProyecto))
                {
                    redirect = true;
                    miembro = false;
                }
                else
                {
                    redirect = false;
                    miembro = true;
                }
            }

            if (usuario.CodGrupo == Constantes.CONST_JefeUnidad)
            {
                int codInstitucion = consultas.Db.Proyecto
                                                 .Where(t => t.Id_Proyecto == Convert.ToInt32(codProyecto)
                                                ).FirstOrDefault().CodInstitucion;

                if (codInstitucion != 0)
                {
                    if (usuario.CodInstitucion != codInstitucion)
                    {
                        redirect = true;
                    }
                }
            }


            if (txtTab != null)
            {
                var query = from tur in consultas.Db.TareaUsuarioRepeticions
                            from tu in consultas.Db.TareaUsuarios
                            from tp in consultas.Db.TareaProgramas
                            where
                                tp.Id_TareaPrograma == tu.CodTareaPrograma
                                && tu.Id_TareaUsuario == tur.CodTareaUsuario
                                && tu.CodProyecto == Convert.ToInt32(codProyecto)
                                && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                                && (tu.CodContacto == usuario.IdContacto || tu.CodContactoAgendo == usuario.IdContacto)
                                && tur.FechaCierre == null
                            select tur;

                var predicate = PredicateBuilder.False<Datos.TareaUsuarioRepeticion>();

                if (codConvocatoria == "")
                {
                    predicate.And(t => SqlMethods.Like(t.Parametros, "%tab=" + txtTab + "%"));
                }
                else
                {
                    predicate.And(t => SqlMethods.Like(t.Parametros, "%tabEval=" + txtTab + "%"));
                }

                int numPostIt = query.Count();

                if (string.IsNullOrEmpty(codConvocatoria) || codConvocatoria == "0")
                {
                    var sql = from p in consultas.Db.Proyecto
                              join t in consultas.Db.TabProyectos on p.Id_Proyecto equals t.CodProyecto into ps
                              from t in ps.DefaultIfEmpty()
                              where t.CodTab == txtTab
                                    && p.Id_Proyecto == Convert.ToInt32(codProyecto)
                              select
                              new
                              {
                                  codEstado = p.CodEstado,
                                  realizado = t == null ? false : t.Realizado
                              };
                    foreach (var obj in sql)
                    {
                        if (obj.realizado != null)
                            realizado = obj.realizado;
                        codEstado = obj.codEstado;
                        break;
                    }
                }
                else
                {
                    var sql1 = from p in consultas.Db.Proyecto
                               join t in consultas.Db.TabEvaluacionProyectos on p.Id_Proyecto equals t.CodProyecto into ps
                               from t in ps.DefaultIfEmpty()
                               where t.CodTabEvaluacion == txtTab
                                     && p.Id_Proyecto == Convert.ToInt32(codProyecto)
                                     && t.CodConvocatoria == Convert.ToInt32(codConvocatoria)
                               select new
                               {
                                   codEstado = p.CodEstado,
                                   realizado = t.Realizado
                               };
                    foreach (var obj in sql1)
                    {
                        if (obj.realizado != null)
                            realizado = obj.realizado;
                        codEstado = obj.codEstado;
                        break;
                    }
                }

            }

            if (codConvocatoria != "")
            {
                if (usuario.CodGrupo != Constantes.CONST_CoordinadorEvaluador
                    && usuario.CodGrupo != Constantes.CONST_Evaluador
                    && usuario.CodGrupo != Constantes.CONST_GerenteEvaluador)
                {
                    redirect = true;
                }
            }
            else
            {
                if (codEstado < Constantes.CONST_Convocatoria
                    && usuario.CodGrupo == Constantes.CONST_GerenteEvaluador)
                {
                    redirect = true;
                }
            }
        }

        protected void construirEncabezado()
        {
        }

        /// <summary>
        /// determina si el usuario con el id de contacto suministrado es miembro del proyecto
        /// </summary>
        /// <param name="codProyecto"></param>
        /// <param name="idContacto"></param>        
        protected bool fnMiembroProyecto(int idContacto, string codProyecto)
        {
            try
            {
                var query = (from pc in consultas.Db.ProyectoContactos
                             where pc.CodProyecto == int.Parse(codProyecto)
                                   && pc.CodContacto == idContacto
                                   && pc.Inactivo == false
                                   && pc.FechaInicio.Date <= DateTime.Now.Date
                                   && pc.FechaFin == null
                             select new
                             {
                                 pc.CodContacto,
                                 pc.CodRol
                             }
                                        ).FirstOrDefault();

                HttpContext.Current.Session["CodRol"] = "";

                if (query.CodContacto > 0)
                {
                    HttpContext.Current.Session["CodRol"] = query.CodRol;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// Metodo usado para descargar cualquier tipo de archivo
        /// </summary>
        /// <param name="path">Ruta Fisica o virtual del archivo (Fisica Eje: c:\\directorio\documento.pdf),(Virtual Eje: ~\Directorio\documento.pdf)  </param>
        /// <param name="rutaFisica">Dato Booleano que identifica si la ruta sera fisica o virtual</param>
        protected void DescargarArchivo(string path, bool rutaFisica = true)
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
            HttpContext.Current.Response.End();
        }

        protected bool CargarArchivoServidor(FileUpload archivoCarga, string pathDestino, string nombreDocumento, string extencion, string keyrutaTemp)
        {
            respuesta = new RespuestaCargue();
            respuesta.Extencion = extencion;
            if (CargarTemporal(archivoCarga, pathDestino, nombreDocumento, keyrutaTemp))
            {
                if (CargarDocumentoFinal(pathDestino, nombreDocumento, keyrutaTemp))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected bool CargarTemporal(FileUpload archivoCarga, string pathDestino, string nombreDocumento, string keyRutaTemp)
        {
            string pathDestinoTEMP = ConfigurationManager.AppSettings.Get("RutaIP");
            String RutamasArchivo = pathDestinoTEMP + pathDestino + nombreDocumento + "." + respuesta.Extencion;
            pathDestinoTEMP = pathDestinoTEMP + pathDestino;

            if (File.Exists(ResolveUrl(RutamasArchivo)) == false)
            {
                if (!System.IO.Directory.Exists(ResolveUrl(pathDestinoTEMP)))
                {
                    System.IO.Directory.CreateDirectory(ResolveUrl(pathDestinoTEMP));
                }
                try
                {
                    archivoCarga.PostedFile.SaveAs(ResolveUrl(pathDestinoTEMP + nombreDocumento + "." + respuesta.Extencion));
                    respuesta.PathTemporal = pathDestinoTEMP + nombreDocumento + "." + respuesta.Extencion;
                    return true;
                }
                catch
                {
                    respuesta.Mensaje = "Error No se pudo subir el documento a la carpeta TMP: ";
                    return false;
                }
            }
            return true;
        }

        protected bool CargarDocumentoFinal(string pathDestino, string nombreDocumento, string keyRutaTemp)
        {
            var pathDestinoTEMP = ConfigurationManager.AppSettings.Get("RutaIP");
            pathDestinoTEMP = pathDestinoTEMP + pathDestino;

            if (File.Exists(ResolveUrl(pathDestinoTEMP + nombreDocumento + "." + respuesta.Extencion)) == false)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(ResolveUrl(pathDestinoTEMP));
                    Request.Files[0].SaveAs(ResolveUrl(pathDestinoTEMP + nombreDocumento + "." + respuesta.Extencion));

                }
                catch
                {
                    respuesta.Mensaje = "Error No se pudo crear la carpeta: " + pathDestino;
                    return false;
                }
            }

            byte[] archivoPlano = File.ReadAllBytes(ResolveUrl(pathDestinoTEMP + nombreDocumento + "." + respuesta.Extencion));
            File.WriteAllBytes(ResolveUrl(pathDestinoTEMP + nombreDocumento + "." + respuesta.Extencion), archivoPlano);
            respuesta.Mensaje = "OK";
            respuesta.PathFisico = pathDestino + nombreDocumento + "." + respuesta.Extencion;

            return true;
        }

        protected void PintarFilasGrid(GridView obj, int posicion, string[] texto)
        {
            for (int i = 0; i < obj.Rows.Count; i++)
            {
                if (texto.Any(ext => obj.Rows[i].Cells[posicion].Text.EndsWith(ext)))
                {
                    obj.Rows[i].Cells[posicion].Text = "<span class='TitulosRegistrosGrilla'>" + obj.Rows[i].Cells[posicion].Text + "</span>";
                }
            }
        }

        protected bool habilitarGuardado(string CodigoProyecto, string CodigoConvocatoria, int ConstanteIndice)
        {
            bool retorno = false;
            inicioEncabezado(CodigoProyecto, CodigoConvocatoria, ConstanteIndice);

            if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
            {
                retorno = true;
            }

            return retorno;
        }

        protected bool habilitarGuardadoEval(string CodigoProyecto, string CodigoConvocatoria, int ConstanteIndice, int usuarioEvaluacion)
        {
            bool retorno = false;
            inicioEncabezado(CodigoProyecto, CodigoConvocatoria, ConstanteIndice);

            if (miembro == true && usuario.CodGrupo == usuarioEvaluacion && realizado == false)
            {
                retorno = true;
            }

            return retorno;
        }

        protected string UsuarioActual()
        {
            consultas = new Consultas();
            usuario = HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);

            string query = "DECLARE @loginC VARCHAR(30), @infoC VARBINARY(128) SET @loginC=" + usuario.IdContacto.ToString() + " SET @infoC=CAST(@loginC AS VARBINARY(128)) SET CONTEXT_INFO @infoC";

            return query;
        }

        public string obtenerUltimaActualizacion(int idTab, string codProyecto)
        {
            consultas = new Consultas();

            bool nuevo = true;
            bool disabled = false;
            bool guardar = false;

            var conv = (from con in consultas.Db.ConvocatoriaProyectos
                        where con.CodProyecto == Convert.ToInt32(codProyecto)
                        orderby con.CodConvocatoria descending
                        select con).FirstOrDefault();

            if (conv != null)
            {
                if (conv.CodConvocatoria == 1 && codEstado >= Constantes.CONST_Evaluacion)
                    nuevo = false;
            }


            var query = (from tbp in consultas.Db.TabProyectos
                         from con in consultas.Db.Contacto
                         where tbp.CodContacto == con.Id_Contacto
                               && tbp.CodTab == idTab
                               && tbp.CodProyecto == Convert.ToInt32(codProyecto)
                         select tbp).FirstOrDefault();

            if (query != null)
            {
                var proy_c = (from pc in consultas.Db.ProyectoContactos
                              where pc.CodProyecto == Convert.ToInt32(codProyecto)
                              && pc.CodContacto == usuario.IdContacto
                              select pc).FirstOrDefault();

                if (proy_c != null)
                {
                    if (!(miembro
                        && (proy_c.CodRol == Constantes.CONST_RolAsesorLider && codEstado == Constantes.CONST_Inscripcion)
                        || (codEstado == Constantes.CONST_Evaluacion && proy_c.CodRol == Constantes.CONST_RolEvaluador && nuevo))
                        || query.Contacto.Nombres == "")
                    {
                        disabled = true;
                    }
                    if (miembro
                        && (proy_c.CodRol == Constantes.CONST_RolAsesorLider && codEstado == Constantes.CONST_Inscripcion)
                        || (codEstado == Constantes.CONST_Evaluacion && proy_c.CodRol == Constantes.CONST_RolEvaluador && nuevo)
                        || query.Contacto.Nombres != "")
                    {
                        guardar = true;
                    }
                }
            }
            else
            {
                query = new TabProyecto();
                disabled = true;
            }

            ProyectoTabRealizado tb_realizado = new ProyectoTabRealizado(query, nuevo, disabled, guardar);
            return tb_realizado.TransformText();
        }

        public void RedirectPage(bool cerrar = false, string mensaje = "", string close = "")
        {
            if (cerrar && !string.IsNullOrEmpty(mensaje))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.opener.location.reload();window.close();", true);
            }
            else if (cerrar && string.IsNullOrEmpty(mensaje))
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.opener.location.reload();window.close();", true);
            }
            else if (!string.IsNullOrEmpty(mensaje) && !cerrar)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "alert('" + mensaje + "');", true);
            }
            else
            {
                if (!string.IsNullOrEmpty(close))
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "window.close();", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "location.reload();", true);
                }

            }

        }

        /// utilizado en la creacion de usuarios (todos los perfiles)
        /// </summary>      
        public string GeneraClave()
        {

            string newPassword = Membership.GeneratePassword(15, 0);
            newPassword = Regex.Replace(newPassword, @"[^a-zA-Z0-9]", m => "9");
            string Especial = Membership.GeneratePassword(3, 3);
            newPassword = newPassword + Especial; ;
            return newPassword;

        }
        //public string GeneraClave()
        //{
        //    int num1;
        //    int num2;
        //    int num3;
        //    int NumAleatorio;
        //    string fnGeneraClave;
        //    Random RandomClass = new Random();
        //    num1 = (Int32)((RandomClass.Next(1, 9)) + 1);
        //    num2 = (Int32)((RandomClass.Next(1, 9)) + 1);
        //    num3 = (Int32)((9 * RandomClass.Next(1, 9)) + 1);
        //    var cuantos = (from pm in consultas.Db.PasswordModelos
        //                   select pm).Count();
        //    NumAleatorio = (Int32)((RandomClass.Next(1, cuantos)) + 1);	// Generate random value between 1 and 9.
        //    var txtPalabra = (from pm in consultas.Db.PasswordModelos
        //                      where pm.Id_PasswordModelo == NumAleatorio
        //                      select pm).FirstOrDefault();
        //    fnGeneraClave = String.Concat(txtPalabra.Palabra.ToString().Trim(), num1.ToString(), num2.ToString(), num3.ToString());
        //    return fnGeneraClave;
        //}

        /// Redireccion url
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="windowFeatures"></param>
        public static void Redirect(HttpResponse response, string url, string target, string windowFeatures)
        {

            if ((String.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && String.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }

        public SqlDataReader ejecutaReader(String sql, int obj)
        {
            SqlDataReader reader = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                if (conn != null)
                    conn.Close();

                conn.Open();

                if (obj == 1)
                    reader = cmd.ExecuteReader();
                else
                    cmd.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                reader = null;
                /* validacion requerida ya que en la mayoria de los casos la exception es disparada por falta de un PK en la tabla de la BD
                 * por lo cual el usuario no tiene control sobre esto, de lo contrario si la exception se dispara por alguna otra razon, manda un throw
                 * indicando por que se produjo la exception 
                 * ERROR HEREDADO DEL ASP CLASICO
                 */
                if (!se.Message.Contains("no PK on table"))
                    throw;
            }
            catch (Exception)
            {
                reader = null;
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();

            }

            return reader;
        }

        public SecureQueryString CreateQueryStringUrl(string parametros = "")
        {
            SecureQueryString querystringSeguro;

            if (!string.IsNullOrEmpty(parametros))
            {
                querystringSeguro = new SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 }, Request[parametros]);
            }
            else
            {
                querystringSeguro = new SecureQueryString(new Byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 8 });
            }

            return querystringSeguro;
        }

        public Int32 ObtenerProrroga(string codProyecto)
        {
            //Inicializar variables.
            int prorroga_obtenida = 0;
            try
            {
                var sqlConsulta = "select prorroga from ProyectoProrroga where codproyecto = " + codProyecto;

                //Asignar valores a variable DataTable.
                var tabla = consultas.ObtenerDataTable(sqlConsulta, "text");

                //Si tiene datos, convierte el valor obtenido, de lo contrario devuelve cero.
                if (tabla.Rows.Count > 0)
                {
                    prorroga_obtenida = Int32.Parse(tabla.Rows[0]["prorroga"].ToString());
                    return prorroga_obtenida;
                }
                else
                    return prorroga_obtenida;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Método usado en "DeclaraVariables.inc" de FONADE Clásico.
        /// Usado para obtener el valor "Texto" de la tabla "Texto", este valor será usado en la creación
        /// de mensajes cuando el CheckBox "chk_actualizarInfo" esté chequeado; Si el resultado de la consulta
        /// NO trae datos, según FONADE Clásico, crea un registro con la información dada.
        /// </summary>
        /// <param name="NomTexto">Nombre del texto a consultar.</param>
        /// <returns>NomTexto consultado.</returns>
        public string Texto(String NomTexto)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            bool correcto = false;

            txtSQL = "SELECT Texto FROM Texto WHERE NomTexto='" + NomTexto + "'";

            var resultado = consultas.ObtenerDataTable(txtSQL, "text");

            if (resultado.Rows.Count > 0)
                return resultado.Rows[0]["Texto"].ToString();
            else
            {
                txtSQL = "INSERT INTO Texto (NomTexto, Texto) VALUES ('" + NomTexto + "','" + NomTexto + "')";

                cmd = new SqlCommand(txtSQL, conn);

                correcto = EjecutarSQL(conn, cmd);

                if (correcto == false)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error en inserción de TEXTO.')", true);
                    return NomTexto;
                }
                else
                {
                    return NomTexto;
                }
            }
        }

        /// Generar registros en tabla "LogEnvios".
        /// </summary>
        /// <param name="p_Asunto">Asunto.</param>
        /// <param name="p_EnviadoPor">Enviado Por.</param>
        /// <param name="p_EnviadoA">Enviado A:</param>
        /// <param name="p_Programa">Programa:</param>
        /// <param name="codProyectoActual">Código del proyecto</param>
        /// <param name="p_Exitoso">Exitoso "1/0".</param>
        public void prLogEnvios(String p_Asunto, String p_EnviadoPor, String p_EnviadoA, String p_Programa, Int32 codProyectoActual, Boolean p_Exitoso)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            String sqlConsulta = "";
            bool correcto = false;

            try
            {
                sqlConsulta = " INSERT INTO LogEnvios (Fecha, Asunto, EnviadoPor, EnviadoA, Programa, CodProyecto, Exitoso) " +
                              " VALUES (GETDATE(),'" + p_Asunto + "','" + p_EnviadoPor + "','" + p_EnviadoA + "','" + p_Programa + "'," + codProyectoActual + "," + p_Exitoso + ") ";

                cmd = new SqlCommand(sqlConsulta, conn);
                correcto = EjecutarSQL(conn, cmd);
            }
            catch (Exception e)
            {
                throw new Exception("Error cargando el log de envio de correos", e);
            }
        }

        /// Método que recibe la conexión y la consulta SQL y la ejecuta.
        /// </summary>
        /// <param name="p_connection">Conexión</param>
        /// <param name="p_cmd">Consulta SQL.</param>
        /// <returns>TRUE = Sentencia SQL ejecutada correctamente. // FALSE = Error.</returns>
        public bool EjecutarSQL(SqlConnection p_connection, SqlCommand p_cmd)
        {
            //Ejecutar controladamente la consulta SQL.
            try
            {
                p_connection.Open();
                p_cmd.ExecuteReader();
                p_connection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                p_connection.Close();
                p_connection.Dispose();
            }
        }

        /// Obtener el código de la convocatoria, este método debe ser empleado cuando la variable de sesión
        /// y/u otras variables relacionadas al código de la convocatoria del proyecto seleccionado.
        /// </summary>
        /// <returns> String con datos = código de la convocatoria obtenida. // String vacío = No hay datos.</returns>
        public string ObtenerCodigoConvocatoria(string codProyecto)
        {
            string txtSQL;
            string cod_convoc = string.Empty;
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                txtSQL = "SELECT CodConvocatoria FROM ConvocatoriaProyecto WHERE CodProyecto = " + codProyecto;
                dt = consultas.ObtenerDataTable(txtSQL, "text");
                if (dt.Rows.Count > 0)
                {
                    cod_convoc = dt.Rows[0]["CodConvocatoria"].ToString();
                    txtSQL = null;
                    dt = null;
                    return cod_convoc;
                }
                else
                {
                    txtSQL = null;
                    dt = null;
                    return string.Empty;
                }
            }
            catch
            {
                txtSQL = null;
                dt = null;
                return string.Empty;
            }
        }

        public String String_EjecutarSQL(SqlConnection p_connection, SqlCommand p_cmd)
        {
            try
            {
                p_connection.Open();
                p_cmd.ExecuteReader();
                p_connection.Close();
                return string.Empty;
            }
            catch (Exception ex)
            {
                p_connection.Close();
                return ex.Message;
            }
            finally
            {
                p_connection.Close();
                p_connection.Dispose();
            }
        }

        /// <summary>
        /// Actualiza tab Evaluacion
        /// </summary>
        /// <param name="txtTab">Tab.</param>
        /// <param name="CodProyecto">Código del proyecto.</param>
        /// <param name="CodConvocatoria">Código de la convocatoria.</param>
        public void prActualizarTabEval(String txtTab, String CodProyecto, String CodConvocatoria)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String txtSQL;

            try
            {
                txtSQL = " SELECT * FROM tabEvaluacionProyecto WHERE codTabEvaluacion = " + txtTab +
                         " AND codproyecto = " + CodProyecto + " AND codConvocatoria = " + CodConvocatoria;

                var RSTab = consultas.ObtenerDataTable(txtSQL, "text");

                //Solo inserta si tiene datos.
                if (RSTab.Rows.Count == 0)
                {

                    try
                    {
                        cmd = new SqlCommand("MD_prActualizarTabEval", con);
                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodTabEvaluacion", txtTab);
                        cmd.Parameters.AddWithValue("@CodProyecto", CodProyecto);
                        cmd.Parameters.AddWithValue("@CodConvocatoria", CodConvocatoria);
                        cmd.Parameters.AddWithValue("@CodContacto", usuario.IdContacto);
                        cmd.Parameters.Add("@FechaModificacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Caso", "INSERT");
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }
                    catch
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
                else
                {
                    try
                    {
                        cmd = new SqlCommand("MD_prActualizarTabEval", con);
                        if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CodTabEvaluacion", txtTab);
                        cmd.Parameters.AddWithValue("@CodProyecto", CodProyecto);
                        cmd.Parameters.AddWithValue("@CodConvocatoria", CodConvocatoria);
                        cmd.Parameters.AddWithValue("@CodContacto", usuario.IdContacto);
                        cmd.Parameters.AddWithValue("@FechaModificacion", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Caso", "UPDATE");
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }
                    catch
                    {
                        con.Close();
                        con.Dispose();
                        cmd.Dispose();
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                }

                RSTab = null;
            }
            catch (Exception ex)
            {
                errorMessageDetail = ex.Message;
            }
        }

        /// <summary>
        /// Actualiza una pestaña padre
        /// </summary>
        /// <param name="txtTab">Tab.</param>
        /// <param name="CodProyecto">Código del proyecto.</param>
        /// <param name="CodConvocatoria">Código de la convocatoria.</param>
        public int prActualizarTabPadre(String txtTab, String CodProyecto, String CodConvocatoria)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            DataTable RS = new DataTable();
            String txtTabs = "";
            String txtTabPadre = "";
            int flag = 0;

            try
            {
                if (CodConvocatoria == "")
                    txtSQL = " select codtab from tab where id_tab = " + txtTab;
                else
                    txtSQL = " select codtabEvaluacion as codTab from tabEvaluacion where id_tabEvaluacion = " + txtTab;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count == 0)
                {
                    txtTabs = txtTab;
                    txtTabPadre = txtTab;
                }
                else
                {
                    txtTabPadre = RS.Rows[0]["CodTab"].ToString();
                    if (CodConvocatoria == "")
                    {
                        txtSQL = " select id_tab from tab where codtab = " + txtTabPadre;
                    }
                    else
                    {
                        txtSQL = " select id_tabEvaluacion as id_tab from tabEvaluacion where codtabEvaluacion = " + txtTabPadre;
                    }

                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow row in RS.Rows)
                    {
                        if (txtTabs == "")
                            txtTabs = txtTabs + row["id_tab"].ToString();
                        else
                            txtTabs = txtTabs + "," + row["id_tab"].ToString();
                    }
                }

                //Verificar si estan marcados como realizados todos los hijos del tab padre.                
                if (CodConvocatoria == string.Empty) //si es tab de proyecto.
                {
                    if (txtTabPadre == Constantes.CONST_Impacto.ToString() || txtTabPadre == Constantes.CONST_ResumenEjecutivo.ToString())
                    {
                        txtSQL = " select realizado from tabproyecto where CodTab in (" + txtTabs + ") and codproyecto = " + CodProyecto;

                        RS = consultas.ObtenerDataTable(txtSQL, "text");
                        flag = 1;
                        if (RS.Rows.Count > 0)
                        {
                            foreach (DataRow row in RS.Rows)
                            {
                                if (row["realizado"].ToString() == "False" || row["realizado"].ToString() == "0")
                                {
                                    flag = 0;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            flag = 0;
                        }
                    }
                    else
                    {
                        txtSQL = " SELECT count(T.id_tab)- sum( case when realizado = 1 then 1 else 0 end) Faltan " +
                                 " FROM tab T LEFT JOIN tabProyecto TP ON T.id_tab=TP.codTab " +
                                 " AND codproyecto = " + CodProyecto +
                                 " WHERE T.codTab = " + txtTabPadre;

                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        flag = 0;

                        if (RS.Rows.Count > 0)
                        {
                            if (RS.Rows[0]["Faltan"].ToString() == "0" || String.IsNullOrEmpty(RS.Rows[0]["Faltan"].ToString())) { flag = 1; }
                        }
                    }
                }
                else
                {
                    txtSQL = " SELECT count(T.id_tabEvaluacion)- sum( case when realizado = 1 then 1 else 0 end) Faltan " +
                             " FROM tabEvaluacion T RIGHT JOIN tabEvaluacionProyecto TP ON " +
                             " T.id_tabEvaluacion=TP.codTabEvaluacion AND codConvocatoria = " + CodConvocatoria +
                             " AND codproyecto=" + CodProyecto + " WHERE  T.codTabEvaluacion = " + txtTabPadre;

                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RS.Rows.Count > 0)
                    {
                        if (RS.Rows[0]["Faltan"].ToString() == "0" || String.IsNullOrEmpty(RS.Rows[0]["Faltan"].ToString()))
                        {
                            flag = 1;
                        }
                    }
                }

                //Actualizar el estado del tab padre.
                if (CodConvocatoria == "")
                    txtSQL = " select realizado from tabproyecto where codtab = " + txtTabPadre + " and codproyecto = " + CodProyecto;
                else
                    txtSQL = " select realizado from tabEvaluacionProyecto where codtabEvaluacion = " + txtTabPadre + " and codConvocatoria = " + CodConvocatoria + " and codproyecto = " + CodProyecto;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count == 0)
                {
                    //Si es tab de proyecto.
                    if (CodConvocatoria == "")
                    {
                        txtSQL = " insert into tabproyecto(codtab,codproyecto,codcontacto,fechamodificacion,realizado) " +
                                 " values(" + txtTabPadre + "," + CodProyecto + "," + usuario.IdContacto + ",GETDATE()," + flag + ")";
                    }
                    else
                    {
                        txtSQL = " insert into tabEvaluacionproyecto(codtabEvaluacion,codproyecto,codConvocatoria,codcontacto,fechamodificacion,realizado) " +
                                 " values(" + txtTabPadre + "," + CodProyecto + "," + CodConvocatoria + "," + usuario.IdContacto + ",GETDATE()," + flag + ")";
                    }
                }
                else
                {
                    if (CodConvocatoria == "")
                        txtSQL = " update tabproyecto set realizado = " + flag + " where codtab = " + txtTabPadre + " and codproyecto = " + CodProyecto;
                    else
                        txtSQL = " update tabEvaluacionproyecto set realizado = " + flag + " where codtabEvaluacion = " + txtTabPadre + " and codConvocatoria = " + CodConvocatoria + " and codproyecto = " + CodProyecto;
                }

                ejecutaReader(txtSQL, 2);

                return flag;
            }
            catch (Exception ex)
            {
                return flag = 0;
            }
        }

        /// </summary>
        /// <param name="txtTab">Tab.</param>
        /// <param name="CodProyecto">Código del proyecto.</param>
        public void prActualizarTabInter(String txtTab, String CodProyecto)
        {
            //Inicializar variables.
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            DataTable RSTab = new DataTable();

            try
            {
                txtSQL = " SELECT * FROM tabInterventorProyecto WHERE codTabInterventor = " + txtTab + " AND codproyecto = " + CodProyecto;
                RSTab = consultas.ObtenerDataTable(txtSQL, "text");

                if (RSTab.Rows.Count > 0)
                {
                    txtSQL = " INSERT INTO tabInterventorProyecto (CodTabInterventor, CodProyecto, CodContacto, FechaModificacion) " +
                             " VALUES (" + txtTab + "," + CodProyecto + ",  " + usuario.IdContacto + ",GETDATE()) ";
                }
                else
                {
                    txtSQL = " UPDATE tabInterventorProyecto " +
                             " SET codcontacto = " + usuario.IdContacto + ", fechamodificacion = GETDATE() " +
                             " WHERE codTabInterventor = " + txtTab + " and Codproyecto = " + CodProyecto;
                }

                try
                {
                    cmd = new SqlCommand(txtSQL, con);
                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    con.Dispose();
                    cmd.Dispose();
                }
                catch
                {
                    con.Close();
                    con.Dispose();
                    cmd.Dispose();
                }
                finally
                {
                    con.Close();
                    con.Dispose();
                }

                RSTab = null;
            }
            catch { }
        }

        /// <summary>
        /// Actualziar tab hija
        /// </summary>
        /// <param name="txtTab">Tab</param>
        /// <param name="CodProyecto">Código del proyecto</param>
        public void prActualizarTab(String txtTab, String CodProyecto)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String txtSQL;
            DataTable RSTab = new DataTable();

            try
            {
                txtSQL = " select * from tabproyecto where codTab = " + txtTab + " and codproyecto = " + CodProyecto;
                RSTab = consultas.ObtenerDataTable(txtSQL, "text");

                if (RSTab.Rows.Count == 0)
                {
                    txtSQL = " INSERT INTO tabproyecto (codtab,codproyecto,codcontacto,fechamodificacion,completo) " +
                             " VALUES (" + txtTab + "," + CodProyecto + ",  " + usuario.IdContacto + ",GETDATE(),1) ";
                }
                else
                {
                    if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                    {
                        txtSQL = " UPDATE tabproyecto" +
                                 " SET codcontacto = " + usuario.IdContacto + ", fechamodificacion = GETDATE(), completo = 1 " +
                                 " WHERE codtab = " + txtTab + " and Codproyecto = " + CodProyecto;
                    }
                    if (usuario.CodGrupo == Constantes.CONST_Asesor)
                    {
                        txtSQL = " UPDATE tabproyecto" +
                                 " SET  realizado = 1 " +
                                 " WHERE codtab = " + txtTab + " and Codproyecto = " + CodProyecto;
                    }
                }

                ejecutaReader(txtSQL, 2);
                RSTab = null;
            }
            catch (Exception ex)
            {
                errorMessageDetail = ex.Message;
            }
        }

        /// <summary>
        /// Última Convocatoria en la que participó el proyecto.
        /// </summary>
        /// <param name="CodProyecto"> Código del proyecto. </param>
        /// <returns>boolean.</returns>
        public bool es_bNuevo(String CodProyecto)
        {
            bool re = true;

            try
            {
                string txtSQL = " select codconvocatoria from convocatoriaproyecto " +
                                " where codproyecto = " + CodProyecto + " order by codconvocatoria desc";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["codconvocatoria"].ToString() == "1" && codEstado >= Constantes.CONST_Evaluacion)
                    {
                        re = false;
                        return re;
                    }
                }
                return re;
            }
            catch
            {
                return re;
            }
        }

        /// <summary>
        /// Averiguar si "está en acta".
        /// </summary>
        /// <param name="CodProyecto">Código del proyecto.</param>
        /// <param name="CodConvocatoria">Código de la convocatoria.</param>
        /// <returns>boolean.</returns>
        public bool es_EnActa(String CodProyecto, String CodConvocatoria)
        {
            bool re = false;

            try
            {
                string txtSQL = " select count(codproyecto) from evaluacionactaproyecto, evaluacionacta  " +
                                " where id_acta=codacta and codproyecto = " + CodProyecto +
                                " and codconvocatoria = " + CodConvocatoria;

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    re = true;
                    return re;
                }

                dt = null;
                return re;
            }
            catch
            {
                return re;
            }
        }

        /// <summary>     
        /// actualiza con un solo parametro
        /// Al parecer sólo lo usa la página "ProyectoResumenEquipo.aspx".
        /// </summary>
        /// <param name="txtTab">Tab.</param>
        public void m_prActualizarTabPadre(String txtTab, String CodProyecto, String CodConvocatoria)
        {
            String txtSQL;
            String txtTabs = "";
            Int32 txtTabPadre = 0;
            Boolean flag = false;
            DataTable RS = new DataTable();
            SqlCommand cmd = new SqlCommand();

            if (CodConvocatoria == "")
            {
                txtSQL = " select codtab from tab where id_tab = " + txtTab;
            } //si es tab de proyecto
            else
            {
                txtSQL = " select codtabEvaluacion as codTab from tabEvaluacion where id_tabEvaluacion = " + txtTab;
            }

            RS = consultas.ObtenerDataTable(txtSQL, "text");

            if (RS.Rows.Count > 0)
            {
                if (String.IsNullOrEmpty(RS.Rows[0]["codtab"].ToString()))
                {
                    txtTabs = txtTab;
                    txtTabPadre = Int32.Parse(txtTab);
                }
                else
                {
                    txtTabPadre = Int32.Parse(RS.Rows[0]["codtab"].ToString());
                    if (CodConvocatoria == "") { txtSQL = "select id_tab from tab where codtab = " + txtTabPadre; }//si es tab de proyecto
                    else { txtSQL = "select id_tabEvaluacion as id_tab from tabEvaluacion where codtabEvaluacion = " + txtTabPadre; }

                    RS = new DataTable();
                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow row in RS.Rows)
                    {
                        if (txtTabs == "")
                        {
                            txtTabs = txtTabs + row["id_tab"].ToString();
                        }
                        else
                        {
                            txtTabs = txtTabs + ", " + row["id_tab"].ToString();
                        }
                    }
                }
            }

            //Verificar si estan marcados como realizados todos los hijos del tab padre.
            if (CodConvocatoria == "") //si es tab de proyecto
            {
                if (txtTabPadre == Constantes.CONST_Impacto || txtTabPadre == Constantes.CONST_ResumenEjecutivo)
                {

                    txtSQL = " select realizado from tabproyecto where CodTab IN (" + txtTabs + ") AND codproyecto = " + CodProyecto;

                    RS = new DataTable();
                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    flag = true;
                    if (RS.Rows.Count > 0)
                    {
                        foreach (DataRow row in RS.Rows)
                        {
                            try
                            {
                                if (!Boolean.Parse(row["realizado"].ToString()))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorMessageDetail = ex.Message;
                            }
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    txtSQL = " SELECT count(T.id_tab)- sum( case when realizado is not null then 1 else 0 end) AS Faltan " +
                             " FROM tab T " +
                             " LEFT JOIN tabProyecto TP ON T.id_tab=TP.codTab AND codproyecto = " + CodProyecto +
                             " WHERE T.codTab = " + txtTabPadre;

                    RS = new DataTable();
                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                    flag = false;

                    if (RS.Rows.Count > 0)
                    {
                        try { if (Int32.Parse(RS.Rows[0]["Faltan"].ToString()) == 0) { flag = true; } }
                        catch (Exception ex)
                        {
                            errorMessageDetail = ex.Message;
                        }
                    }
                }
            }
            else
            {
                txtSQL = " SELECT count(T.id_tabEvaluacion)- sum( case when realizado is not null then 1 else 0 end) AS Faltan " +
                         " FROM tabEvaluacion T " +
                         " LEFT JOIN tabEvaluacionProyecto TP ON " +
                         " T.id_tabEvaluacion=TP.codTabEvaluacion AND " +
                         " codConvocatoria = " + CodConvocatoria + " AND " +
                         " codproyecto = " + CodProyecto +
                         " WHERE T.codTabEvaluacion = " + txtTabPadre;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                flag = false;
                if (RS.Rows.Count > 0)
                {
                    try
                    {
                        if (Int32.Parse(RS.Rows[0]["Faltan"].ToString()) == 0)
                        {
                            flag = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessageDetail = ex.Message;
                    }
                }
            }

            RS = new DataTable();

            //Actualizar el estado del tab padre.
            if (CodConvocatoria == "")//si es tab de proyecto.
            {
                txtSQL = " select realizado from tabproyecto where codtab = " + txtTabPadre + " and codproyecto = " + CodProyecto;
            }
            else
            {
                txtSQL = " select realizado from tabEvaluacionProyecto where codtabEvaluacion = " + txtTabPadre + " and codConvocatoria = " + CodConvocatoria + " and codproyecto = " + CodProyecto;
            }

            RS = consultas.ObtenerDataTable(txtSQL, "text");

            if (RS.Rows.Count == 0)
            {
                if (CodConvocatoria == "") //si es tab de proyecto
                {
                    txtSQL = " insert into tabproyecto(codtab,codproyecto,codcontacto,fechamodificacion,realizado) " +
                             " values(" + txtTabPadre + ", " + CodProyecto + ", " + usuario.IdContacto + ", GETDATE(), " + flag + ") ";
                }
                else
                {
                    txtSQL = " insert into tabEvaluacionproyecto(codtabEvaluacion,codproyecto,codConvocatoria,codcontacto,fechamodificacion,realizado) " +
                             " values(" + txtTabPadre + ", " + CodProyecto + ", " + CodConvocatoria + ", " + usuario.IdContacto + ", GETDATE(), " + flag + ")";
                }
            }
            else
            {
                if (CodConvocatoria == "") //si es tab de proyecto
                {
                    txtSQL = " update tabproyecto set realizado = " + flag + " where codtab = " + txtTabPadre + " and codproyecto = " + CodProyecto;
                }
                else
                {
                    txtSQL = " update tabEvaluacionproyecto set realizado = " + flag + " where codtabEvaluacion = " + txtTabPadre + " and codConvocatoria = " + CodConvocatoria + " and codproyecto = " + CodProyecto;
                }

            }

            ejecutaReader(txtSQL, 2);

            RS = null;
        }

        /// <summary>
        /// Verifica si el tab esta realizado o no.
        /// </summary>                
        public bool esRealizado(String txtTab, String CodProyecto, String CodConvocatoria)
        {
            //Inicializar variables.
            String txtSQL;
            Boolean realizado = false;

            if (string.IsNullOrEmpty(CodConvocatoria))
            {
                if (Session["idConvoca"] != null)
                {
                    CodConvocatoria = Session["idConvoca"].ToString();
                }
            }

            var proyecto = (from p in consultas.Db.Proyecto1s
                            where p.Id_Proyecto == Convert.ToInt32(CodProyecto)
                            select p).FirstOrDefault();

            try
            {
                txtSQL = "select top 1 codestado,ISNULL(realizado,0)realizado from proyecto ";
                var tabla = "";
                if (CodConvocatoria == "" || (!string.IsNullOrEmpty(CodConvocatoria) && proyecto.CodEstado <= Constantes.CONST_Convocatoria))
                {
                    tabla = "tabproyecto";
                    txtSQL = txtSQL + " left join tabproyecto on  id_proyecto=codproyecto and codtab = " + txtTab;
                }
                else
                {
                    tabla = "tabevaluacionproyecto";
                    txtSQL = txtSQL + " left join tabevaluacionproyecto on  id_proyecto=codproyecto and codtabevaluacion = " + txtTab;
                }

                txtSQL = txtSQL + " where id_proyecto = " + CodProyecto + " ORDER BY " + tabla + ".FechaModificacion DESC";

                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count == 0)
                {
                    realizado = false;
                }
                else
                {
                    if (!String.IsNullOrEmpty(RS.Rows[0]["realizado"].ToString()))
                    {
                        try
                        {
                            realizado = Boolean.Parse(RS.Rows[0]["realizado"].ToString());
                        }
                        catch
                        {
                            realizado = false;
                        }
                    }
                }

                RS = null;
                return realizado;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Verifica si el tab esta realizado o no en la fase de evaluación.
        /// </summary>                
        public bool esRealizado(Int32 codigoTab, Int32 codigoProyecto, Int32 codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from tabEvaluacion in db.TabEvaluacionProyectos
                              where
                                   tabEvaluacion.CodProyecto.Equals(codigoProyecto)
                                   && tabEvaluacion.CodConvocatoria.Equals(codigoConvocatoria)
                                   && tabEvaluacion.CodTabEvaluacion.Equals(codigoTab)
                                   && tabEvaluacion.Realizado.Equals(true)
                              select
                                   tabEvaluacion.Realizado
                             ).Any();

                return entity;
            }
        }

        /// Ultima Convocatoria en la que participo el proyecto...
        /// </summary>
        /// <param name="CodProyecto">Código del proyecto.</param>
        /// <returns>boolean.</returns>
        public bool es_nuevo_proyecto(String CodProyecto)
        {
            //Inicializar variables.
            String txtSQL;
            DataTable rs = new DataTable();
            bool bNuevo = true;

            try
            {
                txtSQL = " select codconvocatoria from convocatoriaproyecto where codproyecto = " + CodProyecto +
                         " order by codconvocatoria desc ";
                rs = consultas.ObtenerDataTable(txtSQL, "text");

                if (rs.Rows.Count > 0)
                {
                    if (Int32.Parse(rs.Rows[0]["codconvocatoria"].ToString()) == 1 && usuario.CodGrupo >= Constantes.CONST_Evaluacion) { bNuevo = false; }
                }

                return bNuevo;
            }
            catch
            {
                return bNuevo;
            }
        }

        /// <summary>
        /// Obtener el estado de proyecto y del tab.
        /// </summary>
        /// <param name="txtTab">tab.</param>
        /// <param name="CodProyecto">Código del proyecto.</param>
        /// <param name="CodConvocatoria">Código de la convocatoria "puede ser un valor vacío pero NO un (valor) NULL."</param>
        /// <returns>CodigoEstado.</returns>
        public Int32 CodEstado_Proyecto(String txtTab, String CodProyecto, String CodConvocatoria)
        {
            String txtSQL;
            Int32 CodigoEstado = 0;
            DataTable RS = new DataTable();

            try
            {
                //Estado de proyecto y del tab
                txtSQL = "select codestado,realizado from proyecto ";

                if (CodConvocatoria.Trim() == "")
                {
                    txtSQL = txtSQL + "left join tabproyecto on  id_proyecto=codproyecto and codtab = " + txtTab;
                }
                else
                {
                    txtSQL = txtSQL + "left join tabevaluacionproyecto on  id_proyecto=codproyecto and codtabevaluacion =" + txtTab;
                }
                txtSQL = txtSQL + " where id_proyecto = " + CodProyecto;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(RS.Rows[0]["CodEstado"].ToString()))
                    {
                        CodigoEstado = Int32.Parse(RS.Rows[0]["CodEstado"].ToString()); HttpContext.Current.Session["CodEstado"] = CodigoEstado.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                errorMessageDetail = Ex.Message;
            }

            HttpContext.Current.Session["CodEstado"] = CodigoEstado;
            return CodigoEstado;
        }

        /// <summary>
        /// Marcar si está realizado o no validacion planes vs. convocatoria
        /// </summary>
        /// <param name="txtTab">Tab.</param>
        /// <param name="CodProyecto">Código del proyecto.</param>
        /// <param name="CodConvocatoria">Código de la convocatoria "con datos para ejecutar en las pestañas de EVALUACIÓN".</param>
        /// <param name="Realizado">Si está realizado o no.</param>
        public int Marcar(String txtTab, String CodProyecto, String CodConvocatoria, Boolean Realizado)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            String txtSQL = "";
            DataTable rs = new DataTable();
            Int32 numTabsEval = 0;
            Int32 valor = 0;
            int flag = 0;

            try
            {

                //Si el plan esta en formulación y no esta en convocatoria
                if (CodConvocatoria == "")
                {
                    txtSQL = "select * from tabproyecto where codTab=" + txtTab + " and codproyecto=" + CodProyecto;

                    DataTable dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                        {
                            txtSQL = "update tabproyecto set codcontacto = " + usuario.IdContacto +
                            ",fechamodificacion = getdate() " +
                            "where codtab = " + txtTab + " and Codproyecto=" + CodProyecto;
                        }
                        if (usuario.CodGrupo == Constantes.CONST_Asesor)
                        {
                            txtSQL = "update tabproyecto set Realizado = " + (Realizado ? 1 : 0) +
                            " where codtab = " + txtTab + " and Codproyecto=" + CodProyecto;
                        }
                    }
                    else
                    {
                        txtSQL = "insert into tabproyecto (codtab,codproyecto,codcontacto,fechamodificacion) " +
                    "values(" + txtTab + "," + CodProyecto + "," + usuario.IdContacto + ",getdate())";
                    }
                }

                //Si el plan posee una convocatoria
                if (CodConvocatoria != "")
                {
                    txtSQL = "select * from tabEvaluacionproyecto where codtabEvaluacion = " + txtTab + " and codConvocatoria = " + CodConvocatoria + " and codproyecto = " + CodProyecto;

                    DataTable dt = consultas.ObtenerDataTable(txtSQL, "text");

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador)
                        {
                            txtSQL = "update tabEvaluacionproyecto set Realizado = " + (Realizado ? 1 : 0) +
                            " where codtabEvaluacion = " + txtTab + " and codConvocatoria = " + CodConvocatoria + " and codproyecto = " + CodProyecto;
                        }
                    }
                }

                ejecutaReader(txtSQL, 2);

                //Actualizar el tab padre
                flag = prActualizarTabPadre(txtTab, CodProyecto, CodConvocatoria);

                //Si el coordinador aprueba una evaluación el emprendedor no debe poder realizar ningún cambio
                if (CodConvocatoria != "")
                {
                    //Calcular número de tabs de evaluación
                    rs = consultas.ObtenerDataTable("SELECT isnull(COUNT(0),0) as Total FROM TabEvaluacion WHERE codTabEvaluacion is NULL", "text");
                    valor = Int32.Parse(rs.Rows[0]["Total"].ToString());
                    numTabsEval = valor - 2; //No tomar en cuenta los tab informes y desempeño de evaluador.

                    //Calcular cuantos tabs estan aprobados
                    txtSQL = " select count(tep.codtabevaluacion) as conteo from tabevaluacionproyecto tep,tabevaluacion te " +
                             " where id_tabevaluacion=tep.codtabevaluacion  and realizado = 1 and te.codtabevaluacion is null " +
                             " and codproyecto = " + CodProyecto + " and codconvocatoria = " + CodConvocatoria;
                    rs = consultas.ObtenerDataTable(txtSQL, "text");

                    if (Int32.Parse(rs.Rows[0]["conteo"].ToString()) == numTabsEval)
                    {
                        //Si todos los tabs se encuentran aprobados el emprendedor no debe poder realizar cambios
                        txtSQL = " Update tabproyecto Set realizado = 1 where codproyecto = " + CodProyecto;

                        //Ejecutar SQL.
                        ejecutaReader(txtSQL, 2);
                    }

                    rs = null;
                }

                return flag = 0;
            }
            catch (Exception ex)
            {
                return flag = 0;
            }
        }


        public static string RemoveHTMLTags(string content)
        {
            var cleaned = string.Empty;
            string textOnly = string.Empty;
            var tagRemove = new Regex(@"<[^>]*(>|$)");
            var compressSpaces = new Regex(@"[\s\r\n]+");
            textOnly = tagRemove.Replace(content, string.Empty);
            textOnly = compressSpaces.Replace(textOnly, " ");
            cleaned = textOnly;
            return cleaned;
        }

        /// <summary>
        /// Retornar el String para colocar en la grilla de acuerdo al valor obtenido.
        /// </summary>
        /// <param name="Estado">Estado "numérico".</param>
        /// <returns>EstadoPago</returns>
        public string EstadoPago(Int32 Estado)
        {
            //Inicializar variables.
            String EstadoPago = "";

            try
            {
                switch (Estado)
                {
                    case Constantes.CONST_EstadoEdicion: EstadoPago = "Edición"; break;
                    case Constantes.CONST_EstadoInterventor: EstadoPago = "Interventor"; break;
                    case Constantes.CONST_EstadoCoordinador: EstadoPago = "Coordinador"; break;
                    case Constantes.CONST_EstadoFiduciaria: EstadoPago = "Fiduciaria"; break;
                    case Constantes.CONST_EstadoAprobadoFA: EstadoPago = "Aprobado"; break;
                    case Constantes.CONST_EstadoRechazadoFA: EstadoPago = "Rechazado"; break;
                    default: EstadoPago = ""; break;
                }

                return EstadoPago;
            }
            catch
            {
                return EstadoPago;
            }
        }

        /// <summary>
        /// Función para detectar si Capicom se encuentra instalado en la máquina
        /// </summary>
        public string SignDataExternal(String Datos)
        {
            Process compiler = new Process();
            compiler.StartInfo.FileName = @"C:\signtkfonade\GenSign.exe";
            compiler.StartInfo.Arguments = "1, " + Datos;
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.Start();

            var output = new List<string>();

            while (compiler.StandardOutput.Peek() > -1)
            {
                output.Add(compiler.StandardOutput.ReadLine());
            }

            compiler.WaitForExit();

            string alloutput = string.Join("", output.ToArray());

            return alloutput;
        }

        /// <summary>
        /// Función para verificar la firma digital sobre un mensaje de datos. Verifica Integridad.
        /// EN FONADE clásico NO tiene parámetros, aquí se agregan para mejorar la comprensión del código fuente.
        /// </summary>
        /// <param name="Datos">Datos "Xml"</param>
        /// <param name="Firma">Firma</param>
        public bool VerifySign(String Datos, String Firma)
        {
            return String.IsNullOrEmpty(Datos) || String.IsNullOrEmpty(Firma);
        }

        /// <summary>
        /// Función para verificar si el certificado de firma se encuentra en la Lista de Certificados Revocados CRL
        /// </summary>
        /// <param name="Datos">Datos "Xml"</param>
        /// <param name="Firma">Firma</param>
        /// <returns>true = continuar / false = stop.</returns>
        public bool Validatecrl(String Datos, String Firma)
        {
            return String.IsNullOrEmpty(Datos) || String.IsNullOrEmpty(Firma);
        }

        /// <summary>
        /// Función para validar si el certificado de firma proviene de una Autoridad de Certificación de confianza
        /// </summary>
        /// <param name="Datos">Datos</param>
        /// <param name="Firma">Firma</param>
        /// <returns>boolean.</returns>
        public bool ValidateRoot(String Datos, String Firma)
        {
            return String.IsNullOrEmpty(Datos) || String.IsNullOrEmpty(Firma);
        }

        /// <summary>
        /// Función para validar si el certificado de firma se encuentra dentro de su período de validez
        /// </summary>
        /// <param name="Datos">Datos</param>
        /// <param name="Firma">Firma</param>
        /// <returns>boolean.</returns>
        public bool Validatetime(String Datos, String Firma)
        {
            return String.IsNullOrEmpty(Datos) || String.IsNullOrEmpty(Firma);
        }

        /// <summary>
        /// Funcion para traer el codigo de rechazo de la firma digital
        /// </summary>
        /// <param name="Hexa">Valor.</param>
        /// <returns>int</returns>
        public int TraerCodRechazoFirmaDigital(String Hexa)
        {
            String txtSQL;
            DataTable rsRechazo = new DataTable();
            DataTable rsRechazoAux = new DataTable();
            int i_TraerCodRechazoFirmaDigital = 0;

            try
            {
                txtSQL = "SELECT Id_RechazoFirmaDigital FROM RechazoFirmaDigital WHERE Hexadecimal = '" + Hexa.Trim() + "'";
                rsRechazo = consultas.ObtenerDataTable(txtSQL, "text");

                if (rsRechazo.Rows.Count > 0)
                {
                    i_TraerCodRechazoFirmaDigital = Int32.Parse(rsRechazo.Rows[0]["Id_RechazoFirmaDigital"].ToString());
                }
                else
                {
                    txtSQL = "SELECT Id_RechazoFirmaDigital FROM RechazoFirmaDigital WHERE Hexadecimal = 'Otro'";
                    rsRechazoAux = consultas.ObtenerDataTable(txtSQL, "text");

                    if (rsRechazoAux.Rows.Count > 0)
                    {
                        i_TraerCodRechazoFirmaDigital = Int32.Parse(rsRechazoAux.Rows[0]["Id_RechazoFirmaDigital"].ToString());
                    }
                    else
                    {
                        i_TraerCodRechazoFirmaDigital = 10;
                    }
                    rsRechazoAux = null;
                }

                rsRechazo = null;
                i_TraerCodRechazoFirmaDigital = 1;
                return i_TraerCodRechazoFirmaDigital;
            }
            catch
            {
                return i_TraerCodRechazoFirmaDigital;
            }
        }

        /// <summary>
        /// Valida que los datos del firmante no esten nulos
        /// </summary>
        public bool ValidaFirmantes(String Datos, String Firma)
        {
            return String.IsNullOrEmpty(Datos) || String.IsNullOrEmpty(Firma);
        }

        protected string LogAud(int idcontac, string mensaje)
        {
            string query = "Insert into LogAuditoria (Fecha, codContacto, Evento) values ('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'," + idcontac + ", '" + mensaje + "')";

            return query;
        }

    }
}