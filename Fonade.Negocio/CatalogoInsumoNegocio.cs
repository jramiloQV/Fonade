using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Fonade.Negocio
{
    /// <summary>
    /// CLase para manejar información de Insumos de negocio
    /// </summary>
    public class CatalogoInsumoNegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary>
        /// Insertar un registro de precio de insumo
        /// </summary>
        /// <param name="lstInsumoPrecio"> Listado de precios de insumo </param>
        public void InsertarPreciosInsumo(List<ProyectoInsumoPrecio> lstInsumoPrecio)
        {
            foreach (ProyectoInsumoPrecio item in lstInsumoPrecio)
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("INSERT INTO ProyectoInsumoPrecio (");
                sbQuery.Append("CodInsumo");
                sbQuery.Append(", Periodo");
                sbQuery.Append(", Precio ");
                sbQuery.Append(" ) VALUES (");
                sbQuery.Append("  " + item.CodInsumo);
                sbQuery.Append(", " + item.Periodo);
                sbQuery.Append(", " + item.Precio);
                sbQuery.Append(" ) ");

                try
                {
                    db.Open();
                    db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
                }
                catch (SqlException)
                {
                    db.Close();
                    throw;
                }
                finally
                {
                    db.Close();
                }
            }
        }
        /// <summary>
        /// Inserta un insumo de un producto
        /// </summary>
        /// <param name="producto"> Codigo del producto </param>
        /// <param name="idInsumo"> Codigo del insumo</param>
        /// <param name="unidad"> Unidad de insumo </param>
        /// <param name="cant"> Cantidad de insumo </param>
        /// <param name="desperdicio"> Porcentaje de desperdicio</param>
        /// <returns> Numero de inserciones </returns>
        public int InsertProyectoProductoInsumo(int producto, int idInsumo, string unidad, int cant, int desperdicio)
        {
            sbQuery = new StringBuilder();
            int regAfect = 0;
            sbQuery.Append("INSERT INTO ProyectoProductoInsumo (");
            sbQuery.Append("CodProducto");
            sbQuery.Append(", CodInsumo");
            sbQuery.Append(", Presentacion");
            sbQuery.Append(", Cantidad ");
            sbQuery.Append(", Desperdicio ");
            sbQuery.Append(" ) VALUES (");
            sbQuery.Append("  " + producto);
            sbQuery.Append(", " + idInsumo);
            sbQuery.Append(", '" + unidad + "' ");
            sbQuery.Append(", " + cant);
            sbQuery.Append(", " + desperdicio);
            sbQuery.Append(" ) ");

            try
            {
                db.Open();
                regAfect = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (SqlException)
            {
                db.Close();
                throw;
            }
            finally
            {
                db.Close();
            }
            return regAfect;
        }
    }
}
