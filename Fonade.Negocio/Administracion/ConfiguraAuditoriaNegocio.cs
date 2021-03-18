using Fonade.DbAccess;
using Fonade.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Administracion
{
    /// <summary>
    /// Clase para configuración de auditoria.
    /// </summary>
    public class ConfiguraAuditoriaNegocio
    {
        StringBuilder sbQuery = new StringBuilder();
        SQLManager db = new SQLManager();

        /// <summary>
        /// Listado de parametros de auditoria
        /// </summary>
        /// <returns> Listado de parametros </returns>
        public List<Parametro> TraerParametrosConfiguraAuditoriaInicio()
        {
            List<Parametro> lst = new List<Parametro>();
            sbQuery = new StringBuilder();
            IDataReader reader;

            sbQuery.Append("SELECT Id_parametro, nomparametro, Valor FROM parametro ");
            sbQuery.Append("WHERE nomparametro='ActivarActualizacionInformacionUsuarios' OR ");
            sbQuery.Append(" nomparametro='TiempoValidacionActualizacionInformacionUsuarios'");
            try
            {
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Parametro obj = new Parametro();
                while (reader.Read())
                {
                    obj = new Parametro();
                    obj.id = Convert.ToInt32(reader["Id_parametro"]);
                    obj.nomparametro = reader["nomparametro"].ToString();
                    obj.valor = reader["valor"].ToString();

                    lst.Add(obj);
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

            return lst;
        }
        /// <summary>
        /// Actualizar datos de un listado de parametros
        /// </summary>
        /// <param name="lstPrametros"> Listado de parametros que se actualizaran </param>
        public void ActualizarParametro(List<Parametro> lstPrametros)
        {
            String resultado = String.Empty;
            db.Open();
            foreach (Parametro item in lstPrametros)
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("SELECT ID_Parametro FROM parametro ");
                sbQuery.Append("WHERE nomparametro='" + item.nomparametro + "'  ");

                
                resultado = db.ExecuteScalar(sbQuery.ToString(), CommandType.Text).ToString();
                if (!String.IsNullOrEmpty(resultado))
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("UPDATE parametro SET ");
                    sbQuery.Append("valor ='" + item.valor + "' ");
                    sbQuery.Append("WHERE nomparametro ='" + item.nomparametro + "' ");
                }
                else
                {
                    sbQuery = new StringBuilder();
                    sbQuery.Append("INSERT INTO parametro ( ");
                    sbQuery.Append("nomparametro,  valor ");
                    sbQuery.Append(") VALUES ( ");
                    sbQuery.Append("'" + item.nomparametro + "' ");
                    sbQuery.Append(",'" + item.valor + "' ");
                    sbQuery.Append(")");
                }

                db.ExecuteNonQuery(sbQuery.ToString(), CommandType.Text);
            }
            db.Close();
        }
    }
}

