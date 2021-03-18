using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Fonade.Error
{
    /// <summary>
    /// ErrHandler Capturar los errores no controlados
    /// </summary>
    public class ErrHandler 
    {
        /// <summary>
        /// Writes the error.
        /// Handles error by accepting the error message
        /// Displays the page on which the error occured
        /// </summary>
        /// <param name="mensaje">The mensaje.</param>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <param name="stackTrace">The stack trace.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="usuario">The usuario.</param>
        /// <param name="codUsuario">The cod usuario.</param>
        public static void WriteError(string mensaje, string url, string data, string stackTrace, string innerException, string usuario, string codUsuario)
        {
            try
            {
                

                string path = "~/Error/" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string errUbicacion = "Error in: " + System.Web.HttpContext.Current.Request.Url.ToString() + ".";
                    string errUrl = "Error URL: " + url + ".";
                    string errData = "Error Data: " + data + ".";
                    string errMensaje = "Error Message: " + mensaje;
                    string errInnerException = "Error InnerException: " + innerException;
                    string errStackTrace = "Error StackTrace: " + stackTrace;
                    string errUsuario = "Error Usuario: " + usuario;
                    string errCodUsuario = "Error CodUsuario: " + codUsuario;
                    w.WriteLine(errUbicacion);
                    w.WriteLine(errUrl);
                    w.WriteLine(errData);
                    w.WriteLine(errMensaje);
                    w.WriteLine(errInnerException);
                    w.WriteLine(errStackTrace);
                    w.WriteLine(usuario);
                    w.WriteLine(codUsuario);
                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                string urlEx = "CapturaError";

                string mensajeEx = ex.Message.ToString();
                string dataEx = ex.Data.ToString();
                string stackTraceEx = ex.StackTrace.ToString();
                string innerExceptionEx = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                WriteError(mensajeEx, urlEx, dataEx, stackTraceEx, innerExceptionEx, "ErrInterno", "ErrInterno");
            }

        }
    }
}