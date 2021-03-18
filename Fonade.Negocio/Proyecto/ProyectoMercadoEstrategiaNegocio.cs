using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio
{
    public class ProyectoMercadoEstrategiaNegocio
    {

        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary> metodo que Agrega un registro a la tabla de ProyectoMercadoEstrategia con
        /// los datos que vienen en el objeto
        /// </summary>
        /// <param name="oPrMercadoEstrategia">El Objeto para adicionar a la tabla</param>
        /// <returns>un Entero que indica si se ha agregado (1) registro o No (0) a la tabla</returns>
        /// <remarks>2014/10/24 Roberto Alvarado</remarks>
        public int Agregar(ProyectoMercadoEstrategia oPrMercadoEstrategia)
        {
            int reg = 0;

            sbQuery = new StringBuilder();
            sbQuery.Append(" INSERT INTO  ProyectoMercadoEstrategias ( ");
            sbQuery.Append(" CodProyecto                 ");
            sbQuery.Append(",ConceptoProducto            ");
            sbQuery.Append(",EstrategiasDistribucion     ");
            sbQuery.Append(",EstrategiasPrecio           ");
            sbQuery.Append(",EstrategiasPromocion        ");
            sbQuery.Append(",EstrategiasComunicacion     ");
            sbQuery.Append(",EstrategiasServicio         ");
            sbQuery.Append(",PresupuestoMercado          ");
            sbQuery.Append(",EstrategiasAprovisionamiento");
            sbQuery.Append(" ) VALUES ( ");
            sbQuery.Append("  " + oPrMercadoEstrategia.CodProyecto + "  ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.ConceptoProducto + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.EstrategiasDistribucion + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.EstrategiasPrecio + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.EstrategiasPromocion + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.EstrategiasComunicacion + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.EstrategiasServicio + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.PresupuestoMercado + "' ");
            sbQuery.Append(",'" + oPrMercadoEstrategia.EstrategiasAprovisionamiento + "' ");
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


        /// <summary> Metodo que Modifica un registro a la tabla de ProyectoMercadoEstrategia con
        /// los datos que vienen en el objeto
        /// </summary>
        /// <param name="oPrMercadoEstrategia">El Objeto para adicionar a la tabla</param>
        /// <returns>un Entero que indica si se ha agregado (1) registro o No (0) a la tabla</returns>
        public int Modificar(ProyectoMercadoEstrategia oPrMercadoEstrategia)
        {
            int reg = 0;

            sbQuery = new StringBuilder();
            sbQuery.Append(" UPDATE  ProyectoMercadoEstrategias SET ");
            sbQuery.Append(" ConceptoProducto            ='" + oPrMercadoEstrategia.ConceptoProducto + "' ");
            sbQuery.Append(",EstrategiasDistribucion     ='" + oPrMercadoEstrategia.EstrategiasDistribucion + "' ");
            sbQuery.Append(",EstrategiasPrecio           ='" + oPrMercadoEstrategia.EstrategiasPrecio + "' ");
            sbQuery.Append(",EstrategiasPromocion        ='" + oPrMercadoEstrategia.EstrategiasPromocion + "' ");
            sbQuery.Append(",EstrategiasComunicacion     ='" + oPrMercadoEstrategia.EstrategiasComunicacion + "' ");
            sbQuery.Append(",EstrategiasServicio         ='" + oPrMercadoEstrategia.EstrategiasServicio  + "' ");
            sbQuery.Append(",PresupuestoMercado          ='" + oPrMercadoEstrategia.PresupuestoMercado + "' ");
            sbQuery.Append(",EstrategiasAprovisionamiento='" + oPrMercadoEstrategia.EstrategiasAprovisionamiento + "' ");
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" CodProyecto = " + oPrMercadoEstrategia.CodProyecto);

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
