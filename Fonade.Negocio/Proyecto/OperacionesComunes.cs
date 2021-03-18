using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Proyecto
{
    public class OperacionesComunes
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        Consultas consultas = new Consultas();

        /// <summary> Metodo que devuelve los periodos registrados en la tabla de Periodo
        /// </summary>
        /// <returns>Devuelve una lista generica con los datos</returns>
        public List<Periodo> Periodos()
        {

            var lPeriodo = (from p in consultas.Db.Periodos
                            select p).ToList();
            return lPeriodo;            
        }
    }
}
