using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Interventoria
{
    /// <summary>
    /// Clase para manejar las variables de negocio
    /// </summary>
    public class CatalogoVariableNegocio
    {

        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary> Metodo que Agrega un registro a la tabla de Variable       
        /// <param: Objeto > Objecto variable </param>
        /// <returns>Numero de registros mofificados</returns>        
        public int Agregar_Variable(Fonade.Negocio.Entidades.Variable ObjVariable)
        {
            int creado = 0;
            sbQuery = new StringBuilder();
            sbQuery.Append("INSERT INTO Variable (NomVariable, CodTipoVariable) VALUES ('");
            sbQuery.Append(ObjVariable.NomVariable);
            sbQuery.Append("','");
            sbQuery.Append(ObjVariable.CodTipoVariable);
            sbQuery.Append("')");
            try
            {
                db.Open();
                creado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return creado;
        }
    }
}
