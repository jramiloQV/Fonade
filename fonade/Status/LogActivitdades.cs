using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace Fonade.Status
{
    public class LogActivitdades
    {
        /// Handles error by accepting the error message
        /// Displays the page on which the error occured
        public static void WriteError(int tipoActividad, string instruccion)
        {
            //TipoActividad  = 1 -> Plan Operativo
            //TipoActividad  = 2 -> Nomina
            //TipoActividad  = 3 -> Produccion
            //TipoActividad  = 4 -> Ventas

            try
            {
                string nombreArchivo = "";
                if (tipoActividad == 1)
                {
                    nombreArchivo = "PlanOperativo";
                }
                else if (tipoActividad == 2)
                {
                    nombreArchivo = "Nomina";
                }
                else if (tipoActividad == 3)
                {
                    nombreArchivo = "Produccion";
                }
                else if (tipoActividad == 4)
                {
                    nombreArchivo = "Ventas";
                }

                string path = "~/Status/" + nombreArchivo + "_" + DateTime.Today.ToString("dd-MM-yy") + ".txt";
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    File.Create(System.Web.HttpContext.Current.Server.MapPath(path)).Close();
                }
                using (StreamWriter w = File.AppendText(System.Web.HttpContext.Current.Server.MapPath(path)))
                {
                    w.WriteLine("\r\nLog Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString(CultureInfo.InvariantCulture));
                    string linea1 = "Instruccion: " + instruccion;

                    w.WriteLine(linea1);

                    w.WriteLine("__________________________");
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {

                string mensajeEx = ex.Message.ToString();

                WriteError(tipoActividad, ex.Message);
            }

        }
    }
}