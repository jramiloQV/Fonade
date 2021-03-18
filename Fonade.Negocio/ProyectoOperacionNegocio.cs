using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fonade.Negocio.Utility;

namespace Fonade.Negocio
{
    public class ProyectoOperacionNegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary> 
        /// Agrega un registro a Estrategia de Mercado
        /// </summary>
        /// <param name="oPrMercadoEstrategia">El objeto con los datos a insertar en la tabla</param>
        /// <returns>Un Entero indicado si hubo un numero de inserciones o no</returns>
        public int Agregar(ProyectoOperacion oPrMercadoEstrategia)
        {
            int reg = 0;
            sbQuery = new StringBuilder();

            sbQuery.Append("INSERT INTO ProyectoOperacion ( ");
            sbQuery.Append(" CodProyecto         ");
            sbQuery.Append(",FichaProducto ");
            sbQuery.Append(",EstadoDesarrollo ");
            sbQuery.Append(",DescripcionProceso ");
            sbQuery.Append(",Necesidades ");
            sbQuery.Append(",PlanProduccion ");
            sbQuery.Append(" ) VALUES ( ");

            sbQuery.Append("   " + oPrMercadoEstrategia.CodProyecto);
            sbQuery.Append(", '" + oPrMercadoEstrategia.FichaProducto + "' ");
            sbQuery.Append(", '" + oPrMercadoEstrategia.EstadoDesarrollo + "' ");
            sbQuery.Append(", '" + oPrMercadoEstrategia.DescripcionProceso + "' ");
            sbQuery.Append(", '" + oPrMercadoEstrategia.Necesidades + "' ");
            sbQuery.Append(", '" + oPrMercadoEstrategia.PlanProduccion + "' ");
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
            return reg;
        }

        /// <summary>
        /// Modifica un registro de estrategia de mercado
        /// </summary>
        /// <param name="oPrMercadoEstrategia">Objeto de estrategia de Mercado </param>
        /// <returns> Numero de inserciones</returns>
        public int Modificar(ProyectoOperacion oPrMercadoEstrategia)
        {
            int reg = 0;
            sbQuery = new StringBuilder();

            sbQuery.Append("UPDATE ProyectoOperacion SET ");
            sbQuery.Append(" FichaProducto     ='" + oPrMercadoEstrategia.FichaProducto + "' ");
            sbQuery.Append(",EstadoDesarrollo  ='" + oPrMercadoEstrategia.EstadoDesarrollo + "' ");
            sbQuery.Append(",DescripcionProceso='" + oPrMercadoEstrategia.DescripcionProceso + "' ");
            sbQuery.Append(",Necesidades       ='" + oPrMercadoEstrategia.Necesidades + "' ");
            sbQuery.Append(",PlanProduccion    ='" + oPrMercadoEstrategia.PlanProduccion + "' ");
            sbQuery.Append(" WHERE  CodProyecto ="  + oPrMercadoEstrategia.CodProyecto);

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
            return reg;
        }
    }
}
