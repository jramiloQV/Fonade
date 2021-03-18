using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fonade;
using Fonade.Negocio.Utility;

namespace Fonade.Negocio.Proyecto
{

    /// <summary>
    /// Clase que se encraga de las operaciones referentes a Proyecto Mercado Investigacion
    /// </summary>
    public class ProyectoMercadoInvestigacionNegocio
    {

        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary> Metodo que adiciona un registro a la tabla con los datos que vienen
        /// en el objeto que se pasa como parametro
        /// </summary>
        /// <param name="oPrMercadoEstrategia">El objeto con los datos a insertar en la tabla</param>
        /// <returns>Un Entero indicado si hubo un numero de inserciones o no</returns>
        public int Agregar(ProyectoMercadoInvestigacion oPrMercadoEstrategia)
        {
            int reg = 0;
            if(oPrMercadoEstrategia.CodProyecto != 0 || oPrMercadoEstrategia.CodProyecto != null)
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO ProyectoMercadoInvestigacion ( ");
                sbQuery.Append(" CodProyecto         ");
                sbQuery.Append(",AnalisisSector      ");
                sbQuery.Append(",AnalisisMercado     ");
                sbQuery.Append(",AnalisisCompetencia ");
                sbQuery.Append(",Objetivos           ");
                sbQuery.Append(",Justificacion       ");
                sbQuery.Append(" ) VALUES ( ");
                sbQuery.Append("  " + oPrMercadoEstrategia.CodProyecto);
                sbQuery.Append(", '" + oPrMercadoEstrategia.AnalisisSector + "' ");
                sbQuery.Append(", '" + oPrMercadoEstrategia.AnalisisMercado + "' ");
                sbQuery.Append(", '" + oPrMercadoEstrategia.AnalisisCompetencia + "' ");
                sbQuery.Append(", '" + oPrMercadoEstrategia.Objetivos + "' ");
                sbQuery.Append(", '" + oPrMercadoEstrategia.Justificacion + "' ");
                sbQuery.Append(" )");

                try
                {
                    db.Open();
                    reg = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    db.Close();
                }
            }
            return reg;
        }

        /// <summary>
        /// Modificar un registro de proyecciones de ventas
        /// </summary>
        /// <param name="oPrMercadoEstrategia"> Objeto de proyecciones de ventas </param>
        /// <returns> 1 Si fue moficido 0 sino se realizo la modificación </returns>
        public int Modificar(ProyectoMercadoInvestigacion oPrMercadoEstrategia)
        {
            int reg = 0;
            if (oPrMercadoEstrategia.CodProyecto != 0 || oPrMercadoEstrategia.CodProyecto != null)
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE ProyectoMercadoInvestigacion SET ");
                sbQuery.Append(" AnalisisSector      ='" + oPrMercadoEstrategia.AnalisisSector + "' ");
                sbQuery.Append(",AnalisisMercado     ='" + oPrMercadoEstrategia.AnalisisMercado + "' ");
                sbQuery.Append(",AnalisisCompetencia ='" + oPrMercadoEstrategia.AnalisisCompetencia + "' ");
                sbQuery.Append(",Objetivos           ='" + oPrMercadoEstrategia.Objetivos + "' ");
                sbQuery.Append(",Justificacion       ='" + oPrMercadoEstrategia.Justificacion + "' ");
                sbQuery.Append(" WHERE  CodProyecto  = " + oPrMercadoEstrategia.CodProyecto);

                try
                {
                    db.Open();
                    reg = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    db.Close();
                }
            }
            return reg;
        }
    }
}
