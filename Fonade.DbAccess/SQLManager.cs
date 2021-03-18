using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Fonade.DbAccess
{
    /// <summary> Clase que encapsula la funcionalidad de ADO.NET
    /// Esto para aprovechar y desligar de la logica del negocio y de los formularios
    /// esta funcionalidad
    /// </summary>
    public class SQLManager
    {

        public SqlConnection Conexion { get; set; }

        public SqlCommand Comando { get; set; }

        public List<SqlParameter> Parametros { get; set; }
  


        public SQLManager()
        {
            Conexion = new SqlConnection();
            Comando = new SqlCommand();

            Parametros = new List<SqlParameter>();
        }

   
        /// <summary> Metodo que abre la conexion con el motor de base de datos 
        /// SQl Server usando la cadena de conexion que se encuentra en el web.config
        /// </summary>
        public void Open()
        {
            if (Conexion.State == ConnectionState.Open)
                return;

            try
            {
                Conexion.ConnectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString();
                Conexion.Open();
            }
            catch (SqlException)
            {
                throw;
            }
            
        }

        /// <summary> Metodo que permite ejecutar sentencias SQL al motor de base de datos SQL
        /// Esto devuelve un objeto Datareader con el resultado de la sentencia
        /// </summary>
        /// <param name="query">La sentencia a ejecutar en el motor d base de datos</param>
        /// <param name="tipo">El tipo de Comando, si es procediemiento almacenado, un Texto o un table direct</param>
        /// <returns>Un objeto IDataReader a recorrer</returns>
        public IDataReader ExecuteDataReader(String query, CommandType tipo)
        {
            IDataReader reader;

            try
            {
                Comando = Conexion.CreateCommand();
                Comando.CommandText = query;
                Comando.CommandType = tipo;

                reader = Comando.ExecuteReader();
                
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                Comando.Dispose();
            }

            return reader;
        }

        /// <summary> Metodo que ejecuta una sentencia que modifica el motor de Base de datos 
        /// tales como INSERT, UPDATE o DELETE
        /// </summary>
        /// <param name="query">la sentencia a ejecutar</param>
        /// <param name="tipo">El tipo de Comando, si es procediemiento almacenado, un Texto o un table direct</param>
        /// <returns>Un entero que indica el nuemro de registros afectados por la operacion</returns>
        public int ExecuteNonQuery(String query, CommandType tipo)
        {
            int reg = 0;

            try
            {
                Comando = Conexion.CreateCommand();

                if(Parametros.Count >0)
                {
                    foreach (SqlParameter item in Parametros)
                    {
                        Comando.Parameters.Add(item);
                    }
                }
                Comando.CommandText = query;
                Comando.CommandType = tipo;

                reg = Comando.ExecuteNonQuery();
                Comando.Dispose();
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                Conexion.Close();
                Conexion.Dispose();
            }
            return reg;
        }

        /// <summary> Metodo que devuelve un objeto al ejecutar la sentencia SQL
        /// que se envia al motor a ejecutar
        /// </summary>
        /// <param name="query">El query o sentencia a ejecutar</param>
        /// <param name="tipo">El tipo de Comando, si es procediemiento almacenado, un Texto o un table direct</param>
        /// <returns>Un objeto unico con el valor devuelto por la sentencia</returns>
        /// <remarks>Se debe hacer el casting coreecto del objeto en la capa de negocio</remarks>
        public object ExecuteScalar(String query, CommandType tipo)
        {
            object obj;

            try
            {
                Comando = Conexion.CreateCommand();
                Comando.CommandText = query;
                Comando.CommandType = tipo;

                obj = Comando.ExecuteScalar();
                Comando.Dispose();
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                Conexion.Close();
                Conexion.Dispose();
            }
            return obj;
        }

        /// <summary> Cierra la conexion al motor de base de datos SQL
        /// </summary>
        public void Close()
        {
            Conexion.Close();
            Conexion.Dispose();
        }

        /// <summary> Metodo que crea Parametros para las condiciones del where de los querys
        /// </summary>
        /// <param name="paramName">Nombre del Parametro</param>
        /// <param name="sqlDbType"></param>
        /// <param name="value"></param>
        public void AddParameter(String paramName, DbType sqlDbType, Object value)
        {
            SqlParameter param = new SqlParameter();
            param.DbType = sqlDbType;
            param.ParameterName = paramName;
            param.Value = value;

            Comando.Parameters.Add(param);
        }
       
   }
}
