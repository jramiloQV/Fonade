using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;

namespace Fonade.Negocio
{
    /// <summary>
    /// Clase para manejar información de la operación de roles
    /// </summary>
    public class OperacionesRoleNegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary>
        /// Obtiene el rol del usuario
        /// </summary>
        /// <param name="codProyecto">Codigo del proyecto</param>
        /// <param name="idContacto">Codigo del contacto </param>
        /// <returns> Objecto Usuario </returns>
        public Object ObtenerRolUsuario(String codProyecto, int idContacto)
        {
            Object result;
            sbQuery = new StringBuilder();

            try
            {
                sbQuery.Append(" SELECT CodRol From ProyectoContacto ");
                sbQuery.Append(" Where inactivo=0 and FechaInicio<=getdate() and FechaFin is null ");
                sbQuery.Append(" and CodProyecto = " + codProyecto + "  And  CodContacto =" + idContacto);

                db.Open();

                result = db.ExecuteScalar(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return result;
        }

        /// <summary>
        /// Obtiene la ultima actualización de un pestaña
        /// </summary>
        /// <param name="codProyecto">Codigo del proyecto </param>
        /// <param name="codTab"> Codigo de la pestaña</param>
        /// <returns> Listado de modificaciones de una pestaña </returns>
        public List<String> UltimaModificacion(String codProyecto, int codTab)
        {
            List<String> result = new List<string>();
            sbQuery = new StringBuilder();
            IDataReader reader;
            try
            {
                sbQuery.Append(" SELECT Nombres + ' ' + Apellidos AS nombre, FechaModificacion, Realizado ");
                sbQuery.Append(" FROM TabProyecto, Contacto ");
                sbQuery.Append(" where Id_Contacto = CodContacto AND CodTab = " + codTab);
                sbQuery.Append(" AND CodProyecto = " + codProyecto);

                db.Open();

                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);
                
                if (reader.Read())
                {
                    result.Add(reader["nombre"].ToString());

                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = Convert.ToDateTime(reader["FechaModificacion"]).ToString("MMM",
                                        System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    sMes = UppercaseFirst(sMes);

                    //Obtener la hora en minúscula.
                    string hora = Convert.ToDateTime(reader["FechaModificacion"]).ToString("hh:mm:ss tt",
                                        System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();

                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am"))
                    {
                        hora = hora.Replace("am", "a.m");
                    }
                    if (hora.Contains("pm"))
                    {
                        hora = hora.Replace("pm", "p.m");
                    }

                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    result.Add(sMes + " " + Convert.ToDateTime(reader["FechaModificacion"]).Day +
                                " de " + Convert.ToDateTime(reader["FechaModificacion"]).Year + " " + hora + "."
                        );

                    result.Add(reader["Realizado"].ToString());
                }
                reader.Close();
                reader.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return result;
        }

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        public static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".                
        /// </summary>
        /// <param name="codProyecto">Codigo del proyecto</param>
        /// <param name="id_contacto">Codigo del contacto</param>
        /// <returns> Numero del postit </returns>
        public int Obtener_numPostIt(Int32 codProyecto, Int32 id_contacto)
        {
            Int32 numPosIt = 0;

            sbQuery = new StringBuilder();
            IDataReader reader;
            try
            {
                sbQuery.Append("Select count(tr.Id_TareaUsuarioRepeticion) as cantidad");
                sbQuery.Append(" From tareausuariorepeticion tr INNER JOIN tareausuario t ON T.Id_TareaUsuario = TR.CodTareaUsuario");
                sbQuery.Append(" INNER JOIN Contacto c  ON c.id_contacto = t.codcontactoagendo");
                sbQuery.Append(" INNER JOIN Contacto c2 ON c2.id_contacto = t.codcontacto,tareaprograma tp");
                sbQuery.Append(" Where tr.fechacierre is null and tp.Id_TareaPrograma = 5 and t.CodProyecto = " + codProyecto);
                sbQuery.Append(" and T.CodContacto = " + id_contacto);

                db.Open();

                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                if (reader.Read())
                {
                    numPosIt = Convert.ToInt32(reader["cantidad"].ToString());
                }
                reader.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return numPosIt;
        }

        /// <summary>
        /// Metodo para devolver la fecha en ANSI
        /// </summary>
        /// <param name="fecha"> Fecha formatear</param>
        /// <returns> Fecha en ANSI </returns>
        public static String FechaANSI(DateTime fecha)
        {
            StringBuilder fechaResult = new StringBuilder();
            fechaResult.Append(fecha.Year.ToString());
            if (fecha.Month < 10)
                fechaResult.Append("0" + fecha.Month);
            else
                fechaResult.Append(fecha.Month);
            if (fecha.Day < 10)
                fechaResult.Append("0" + fecha.Day);
            else
                fechaResult.Append(fecha.Day);

            fechaResult.Append(" ");

            if (fecha.Hour < 10)
                fechaResult.Append("0" + fecha.Hour);
            else
                fechaResult.Append(fecha.Hour);

            fechaResult.Append(":");

            if (fecha.Minute < 10)
                fechaResult.Append("0" + fecha.Minute);
            else
                fechaResult.Append(fecha.Minute);

            fechaResult.Append(":");

            if (fecha.Second < 10)
                fechaResult.Append("0" + fecha.Second);
            else
                fechaResult.Append(fecha.Second);

            return fechaResult.ToString();
        }
    }
}
