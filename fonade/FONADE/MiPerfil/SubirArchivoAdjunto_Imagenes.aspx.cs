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
using Fonade.Account;
using System.Text;
//using System.Web.Configuration;


namespace Fonade.FONADE.MiPerfil
{
    public partial class SubirArchivoAdjunto_Imagenes : Negocio.Base_Page
    {
        protected String codigoUsuario = String.Empty;
        protected string tipoDocumento = String.Empty;
        protected int codigoProyecto;
        protected int codigoCertificado;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codigoCertificado = (int)(!string.IsNullOrEmpty(HttpContext.Current.Session["idCertificacionEstudios"].ToString()) ? Convert.ToInt64(HttpContext.Current.Session["idCertificacionEstudios"]) : 0);
                codigoProyecto = (int)(!string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? Convert.ToInt64(HttpContext.Current.Session["CodProyecto"]) : 0);
                codigoUsuario = HttpContext.Current.Session["USER"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["USER"].ToString()) ? HttpContext.Current.Session["USER"].ToString() : "0";
                tipoDocumento = HttpContext.Current.Session["TipoDocumento"].ToString();

            }
            catch (Exception)
            {
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "A ocurrido un error intentando anexar el documento", "window.close();", true);
            }

        }

        protected void SubirArchivo_Click(object sender, EventArgs e)
        {
            if (SubirArchivo.Text == "Enviar")
                Crear();
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe enviar un archivo correcto');", true);
                return;
            }
        }

        /// <summary>
        /// Metodo para subir una documento de identidad o certificado de estudios a la base de datos y al sistema de archivos
        /// Modificado por por Marcel Solera @marztres 
        /// </summary>
        private void Crear()
        {
            try
            {
                int hashDirectorioUsuario = Convert.ToInt32(codigoUsuario) / 2000;
                string directorioDestino = "contactoAnexos\\" + hashDirectorioUsuario.ToString() + "\\ContactoAnexo_" + codigoUsuario.ToString() + "\\"; //Directorio destino archivo
                string extencionAchivo = System.IO.Path.GetExtension(Archivo.PostedFile.FileName);                
                string nombreArchivo = System.IO.Path.GetFileName(Archivo.FileName);
                string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
                StringBuilder querySql = new StringBuilder();

                if (Session["Cedula"] != null)
                {
                    nombreArchivo = Session["Cedula"].ToString() + extencionAchivo;
                }

                // ¿ Es valido el archivo ?
                if (!Archivo.HasFile)
                    throw new ApplicationException("Archivo invalido");
                // ¿ Es image valida ? 
                if (Archivo.PostedFile.ContentType != "image/jpeg" && (Archivo.PostedFile.ContentType != "application/pdf"))
                    throw new ApplicationException("Adjunte un pdf o una imagen, los demas tipos son invalidos.");
                // ¿ Pesa mas de diez megas ?
                if (!(Archivo.PostedFile.ContentLength < 10485760))
                    throw new ApplicationException("El archivo es muy pesado, maximo 10 megas.");
                //¿ Nombre archivo empty?
                if (string.IsNullOrEmpty(Archivo.FileName))
                    throw new ApplicationException("Nombre de archivo invalido o no lo a seleccionado");

                //Verificamos si existe el certificado.
                Boolean existeCertificado = buscarCertificado(codigoUsuario.ToString(), codigoProyecto.ToString(), codigoCertificado.ToString(), tipoDocumento);

                //¿ Existe certificado? si update,no insert
                querySql = getQuery(existeCertificado, tipoDocumento, directorioBase, directorioDestino, nombreArchivo, codigoUsuario, codigoProyecto.ToString(), codigoCertificado.ToString());

                uploadFile(Archivo, directorioBase, directorioDestino, nombreArchivo, extencionAchivo);
                Session["Cedula"] = null;
                if (ExecuteNonQuery(querySql.ToString()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
                }
                else
                {
                    throw new ApplicationException("No fue posible guardar el archivo, podrias intentarlo de nuevo.");
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }

        //Verifica si un certificado existe
        protected Boolean buscarCertificado(string _codigoUsuario, string _codigoProyecto, string _codigoCertificado, string _tipoDocumento)
        {
            Datos.Consultas motorSql = new Datos.Consultas();
            string queryCertificados = string.Empty;

            if (_tipoDocumento.ToLower().Trim() == "FotocopiaDocumento".ToLower().Trim())
                queryCertificados = "SELECT TOP 1 NombreArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + _codigoUsuario + " AND TipoArchivo ='" + _tipoDocumento + "' AND CodProyecto = '" + _codigoProyecto + "'";
            else
                queryCertificados = "SELECT TOP 1 NombreArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + _codigoUsuario + " AND TipoArchivo ='" + _tipoDocumento + "' AND CodProyecto = " + _codigoProyecto + " AND CodContactoEstudio='" + _codigoCertificado + "'";

            DataTable RS = motorSql.ObtenerDataTable(queryCertificados, "text");

            if (RS.Rows.Count != 0)
                return true;
            else
                return false;
        }

        //Executa consultas Tipo Insert y Update, retorna si afecto al menos una fila.
        protected Boolean ExecuteNonQuery(string _sqlQuery)
        {
            int filasAfectadas = 0;
            SqlCommand sqlCommand = new SqlCommand();
            using(SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    sqlCommand = new SqlCommand(_sqlQuery, sqlConnection);
                    sqlCommand.CommandType = CommandType.Text;

                    //Verificamos sin la conexión con sql server esta abierta.
                    if (sqlConnection != null && sqlConnection.State != ConnectionState.Open)
                        sqlConnection.Open();

                    //Si fue al menos una fila afectada retornamos true, transacción exitosa.
                    filasAfectadas = sqlCommand.ExecuteNonQuery();
                    //sqlConnection.Close();
                    //sqlConnection.Dispose();
                    sqlCommand.Dispose();
                }
                finally
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();

                }
            }
            
            if (filasAfectadas == 0)
                return false;
            else
                return true;
        }

        protected void uploadFile(FileUpload _archivo, string _directorioBase, string _directorioDestino, string _nomArchivo, string _extArchivo)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(_directorioBase + _directorioDestino))
                Directory.CreateDirectory(_directorioBase + _directorioDestino);
            //¿ Existe el archivo ? si delete
            if (File.Exists(_directorioBase + _directorioDestino + _nomArchivo))
                File.Delete(_directorioBase + _directorioDestino + _nomArchivo);

            Archivo.SaveAs(_directorioBase + _directorioDestino + _nomArchivo); //Guardamos el archivo
        }

        //Obtiene la consulta sql a ejecutar, si es documento o certificado y si esta actualización o insersión.
        protected StringBuilder getQuery(Boolean _existeCertificado, string _tipoDocumento, string _directorioBase, string _directorioDestino, string _nombreArchivo, string _codigoUsuario, string _codigoProyecto, string _codigoCertificado)
        {
            StringBuilder querySql = new StringBuilder();

            if (_existeCertificado)
            {
                if (_tipoDocumento.ToLower().Trim() == "FotocopiaDocumento".ToLower().Trim())
                    querySql.AppendFormat(" UPDATE ContactoArchivosAnexos SET ruta = '{0}', NombreArchivo = '{1}' WHERE CodContacto = '{2}' AND CodProyecto = '{3}' AND TipoArchivo = '{4}'", _directorioDestino + _nombreArchivo, _nombreArchivo, _codigoUsuario, _codigoProyecto, _tipoDocumento);
                else
                    querySql.AppendFormat(" UPDATE ContactoArchivosAnexos SET ruta = '{0}', NombreArchivo = '{1}' WHERE CodContacto = '{2}' AND CodProyecto = '{3}' AND CodContactoEstudio='{4}'", _directorioDestino + _nombreArchivo, _nombreArchivo, _codigoUsuario, _codigoProyecto, _codigoCertificado);
            }
            else
            {
                if (tipoDocumento.ToLower().Trim() == "FotocopiaDocumento".ToLower().Trim())
                    querySql.AppendFormat("INSERT INTO ContactoArchivosAnexos (CodContacto, ruta, NombreArchivo,TipoArchivo,CodProyecto) VALUES ('{0}','{1}','{2}','{3}','{4}')", codigoUsuario, (_directorioDestino + _nombreArchivo), _nombreArchivo, tipoDocumento, _codigoProyecto);
                else
                    querySql.AppendFormat("INSERT INTO ContactoArchivosAnexos (CodContacto, ruta, NombreArchivo,TipoArchivo,CodProyecto,CodContactoEstudio) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}')", codigoUsuario, (_directorioDestino + _nombreArchivo), _nombreArchivo, tipoDocumento, _codigoProyecto, _codigoCertificado);
            }

            return querySql;
        }
    
    
    }
}