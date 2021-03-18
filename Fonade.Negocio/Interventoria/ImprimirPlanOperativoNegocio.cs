using Datos;
using Fonade.DbAccess;
using Fonade.Negocio.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Interventoria
{
    /// <summary>
    /// Clase para impresión del plan operativo
    /// </summary>
    public class ImprimirPlanOperativoNegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary>
        /// Listado de Avances de actividad por mes
        /// </summary>
        /// <param name="CodActividad"> Codigo de actividad </param>
        /// <returns>Listado de avances por actividad</returns>
        public List<AvanceActividadPOMes> TraerAvanceActividadPOXMes(string CodActividad)
        {
            List<AvanceActividadPOMes> lista = new List<AvanceActividadPOMes>();
            try
            {                
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append("SELECT CodActividad            ");
                sbQuery.Append("      ,Mes                     ");
                sbQuery.Append("      ,CodTipoFinanciacion     ");
                sbQuery.Append("      ,Valor                   ");
                sbQuery.Append("      ,Observaciones           ");
                sbQuery.Append("      ,CodContacto             ");
                sbQuery.Append("      ,ObservacionesInterventor");
                sbQuery.Append("      ,Aprobada                ");
                sbQuery.Append("  FROM AvanceActividadPOMes    ");
                sbQuery.Append("  WHERE codactividad = " + CodActividad);
                sbQuery.Append("  ORDER By Mes ");
           
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                AvanceActividadPOMes oAvance = new AvanceActividadPOMes();
                while (reader.Read())
                {
                    oAvance = new AvanceActividadPOMes();
                    oAvance.CodActividad = Convert.ToInt32(reader["CodActividad"]);
                    oAvance.Mes = Convert.ToByte(reader["Mes"]);
                    oAvance.CodTipoFinanciacion = Convert.ToByte(reader["CodTipoFinanciacion"]);
                    oAvance.Valor = Convert.ToDecimal(reader["Valor"]);
                    oAvance.Observaciones = reader["Observaciones"].ToString();
                    if (!DBNull.Value.Equals(reader["CodContacto"]))
                        oAvance.CodContacto = Convert.ToInt32(reader["CodContacto"]);
                    oAvance.ObservacionesInterventor = reader["ObservacionesInterventor"].ToString();
                    if (!DBNull.Value.Equals(reader["Aprobada"]))
                        oAvance.Aprobada = Convert.ToBoolean(reader["Aprobada"]);

                    lista.Add(oAvance);
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

            return lista;
        }

        /// <summary>
        /// Listado de nomina interventoria por mes
        /// </summary>
        /// <param name="CodProyecto"> Codigo de proyecto </param>
        /// <returns> Listado de nomina de interventoria por mes </returns>
        public List<InterventoriaNominaEntity> TraerInterventoriaNominaXMes(int CodProyecto)
        {
            List<InterventoriaNominaEntity> lista = new List<InterventoriaNominaEntity>();
            try
            {
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append(" SELECT A.Id_Nomina        ");
                sbQuery.Append("     , A.CodProyecto      ");
                sbQuery.Append("     , A.Cargo            ");
                sbQuery.Append("     , A.Tipo AS TipoCargo");
                sbQuery.Append("     , B.CodCargo         ");
                sbQuery.Append("     , B.Mes              ");
                sbQuery.Append("     , B.Valor            ");
                sbQuery.Append("     , B.Tipo             ");
                sbQuery.Append("     FROM InterventorNomina A, InterventorNominaMes B ");
                sbQuery.Append(" WHERE a.tipo = 'Cargo' AND A.id_nomina = B.codcargo ");
                sbQuery.Append(" AND A.codproyecto = " + CodProyecto);
                sbQuery.Append(" ORDER BY A.id_nomina, B.mes, b.tipo");

            
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                InterventoriaNominaEntity oNomina = new InterventoriaNominaEntity();
                while (reader.Read())
                {
                    oNomina = new InterventoriaNominaEntity();
                    oNomina.Id_Nomina = Convert.ToInt32(reader["Id_Nomina"]);
                    oNomina.CodProyecto = Convert.ToInt32(reader["CodProyecto"]);
                    oNomina.Cargo = reader["Cargo"].ToString();
                    oNomina.Tipocargo = reader["Tipocargo"].ToString();
                    oNomina.Mes = Convert.ToInt32(reader["Mes"]);
                    oNomina.Valor = Convert.ToDouble(reader["valor"]);
                    oNomina.Tipo = Convert.ToInt32(reader["Tipo"]);

                    lista.Add(oNomina);
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
            return lista;
        }

        /// <summary>
        /// Listado de avances de cargo por mes
        /// </summary>
        /// <param name="CodCargo"> Codigo de cargo </param>
        /// <returns> Listado de avances de cargo por mes </returns>
        public List<AvanceCargoPOMes> TraerAvanceCargoPOMes(int CodCargo)
        {
            List<AvanceCargoPOMes> lista = new List<AvanceCargoPOMes>();
            try
            {
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append(" SELECT CodCargo               ");
                sbQuery.Append("      ,Mes                     ");
                sbQuery.Append("      ,CodTipoFinanciacion     ");
                sbQuery.Append("      ,Valor                   ");
                sbQuery.Append("      ,Observaciones           ");
                sbQuery.Append("      ,CodContacto             ");
                sbQuery.Append("      ,ObservacionesInterventor");
                sbQuery.Append("      ,Aprobada                ");
                sbQuery.Append("  FROM AvanceCargoPOMes        ");
                sbQuery.Append("  WHERE CodCargo = " + CodCargo);
            
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);
                AvanceCargoPOMes oAvance = new AvanceCargoPOMes();
                while (reader.Read())
                {
                    oAvance = new AvanceCargoPOMes();
                    oAvance.CodCargo = Convert.ToInt32(reader["CodCargo"]);
                    oAvance.Mes = Convert.ToByte(reader["Mes"]);
                    oAvance.CodTipoFinanciacion = Convert.ToByte(reader["CodTipoFinanciacion"]);
                    oAvance.Valor = Convert.ToDecimal(reader["Valor"]);
                    oAvance.Observaciones = reader["Observaciones"].ToString();
                    if (!DBNull.Value.Equals(reader["CodContacto"]))
                        oAvance.CodContacto = Convert.ToInt32(reader["CodContacto"]);
                    oAvance.ObservacionesInterventor = reader["ObservacionesInterventor"].ToString();
                    if (!DBNull.Value.Equals(reader["Aprobada"]))
                        oAvance.Aprobada = Convert.ToBoolean(reader["Aprobada"]);

                    lista.Add(oAvance);
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
            return lista;
        }
        /// <summary>
        /// Obtiene el listado de insumos a interventoria por mes
        /// </summary>
        /// <param name="CodProyecto"> Codigo de proyecto </param>
        /// <returns> Listado de insumos a interventoria por mes </returns>
        public List<InterventoriaNominaEntity> TraerInterventoriaInsumoXMes(int CodProyecto)
        {
            List<InterventoriaNominaEntity> lista = new List<InterventoriaNominaEntity>();
            try
            {            
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append(" SELECT A.Id_Nomina        ");
                sbQuery.Append("     , A.CodProyecto      ");
                sbQuery.Append("     , A.Cargo            ");
                sbQuery.Append("     , A.Tipo AS TipoCargo");
                sbQuery.Append("     , B.CodCargo         ");
                sbQuery.Append("     , B.Mes              ");
                sbQuery.Append("     , B.Valor            ");
                sbQuery.Append("     , B.Tipo             ");
                sbQuery.Append("     FROM InterventorNomina A, InterventorNominaMes B ");
                sbQuery.Append(" WHERE a.tipo = 'Insumo' AND A.id_nomina = B.codcargo ");
                sbQuery.Append(" AND A.codproyecto = " + CodProyecto);
                sbQuery.Append(" ORDER BY A.id_nomina, B.mes, b.tipo");
           
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                InterventoriaNominaEntity oNomina = new InterventoriaNominaEntity();
                while (reader.Read())
                {
                    oNomina = new InterventoriaNominaEntity();
                    oNomina.Id_Nomina = Convert.ToInt32(reader["Id_Nomina"]);
                    oNomina.CodProyecto = Convert.ToInt32(reader["CodProyecto"]);
                    oNomina.Cargo = reader["Cargo"].ToString();
                    oNomina.Tipocargo = reader["Tipocargo"].ToString();
                    oNomina.Mes = Convert.ToInt32(reader["Mes"]);
                    oNomina.Valor = Convert.ToDouble(reader["valor"]);
                    oNomina.Tipo = Convert.ToInt32(reader["Tipo"]);

                    lista.Add(oNomina);
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
            return lista;
        }
        /// <summary>
        /// Listado de produccion por mes a interventoria
        /// </summary>
        /// <param name="CodProyecto"> Codigo de proyecto </param>
        /// <returns>Listado de produccion por mes a interventoria</returns>
        public List<InterventorProduccionEntity> TraerInterventorProduccionXMes(int CodProyecto)
        {
            List<InterventorProduccionEntity> lista = new List<InterventorProduccionEntity>();
            try
            {
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append(" SELECT Id_produccion   ");
                sbQuery.Append("     , CodProyecto      ");
                sbQuery.Append("     , NomProducto      ");
                sbQuery.Append("     , CodProducto      ");
                sbQuery.Append("     , Mes              ");
                sbQuery.Append("     , Valor            ");
                sbQuery.Append("     , Tipo             ");
                sbQuery.Append("     FROM InterventorProduccion, InterventorProduccionMes ");
                sbQuery.Append(" WHERE id_produccion = codproducto ");
                sbQuery.Append(" AND codproyecto = " + CodProyecto);
                sbQuery.Append(" ORDER BY id_produccion, mes, tipo");
   
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                InterventorProduccionEntity oProduccion = new InterventorProduccionEntity();
                while (reader.Read())
                {
                    oProduccion = new InterventorProduccionEntity();
                    oProduccion.Id_produccion = Convert.ToInt32(reader["Id_produccion"]);
                    oProduccion.CodProyecto = Convert.ToInt32(reader["CodProyecto"]);
                    oProduccion.NomProducto = reader["NomProducto"].ToString();
                    oProduccion.Mes = Convert.ToInt32(reader["Mes"]);
                    oProduccion.Valor = Convert.ToDouble(reader["valor"]);
                    oProduccion.Tipo = Convert.ToInt32(reader["Tipo"]);

                    lista.Add(oProduccion);
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
            return lista;
        }
        /// <summary>
        /// Listado de avance de producciones por mes
        /// </summary>
        /// <param name="CodProducto"> Codigo de producto</param>
        /// <returns>Listado de avance de producciones por mes</returns>
        public List<AvanceProduccionPOMes> TraerAvanceProduccionPOMes(int CodProducto)
        {
            List<AvanceProduccionPOMes> lista = new List<AvanceProduccionPOMes>();
            IDataReader reader;
            sbQuery = new StringBuilder();

            sbQuery.Append(" SELECT CodProducto            ");
            sbQuery.Append("      ,Mes                     ");
            sbQuery.Append("      ,CodTipoFinanciacion     ");
            sbQuery.Append("      ,Valor                   ");
            sbQuery.Append("      ,Observaciones           ");
            sbQuery.Append("      ,CodContacto             ");
            sbQuery.Append("      ,ObservacionesInterventor");
            sbQuery.Append("      ,Aprobada                ");
            sbQuery.Append("  FROM AvanceProduccionPOMes   ");
            sbQuery.Append("  WHERE CodProducto = " + CodProducto);

            try
            {
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);
                AvanceProduccionPOMes oAvance = new AvanceProduccionPOMes();
                while (reader.Read())
                {
                    oAvance = new AvanceProduccionPOMes();
                    oAvance.CodProducto = Convert.ToInt32(reader["CodProducto"]);
                    oAvance.Mes = Convert.ToByte(reader["Mes"]);
                    oAvance.CodTipoFinanciacion = Convert.ToByte(reader["CodTipoFinanciacion"]);
                    oAvance.Valor = Convert.ToDecimal(reader["Valor"]);
                    oAvance.Observaciones = reader["Observaciones"].ToString();
                    if (!DBNull.Value.Equals(reader["CodContacto"]))
                        oAvance.CodContacto = Convert.ToInt32(reader["CodContacto"]);
                    oAvance.ObservacionesInterventor = reader["ObservacionesInterventor"].ToString();
                    if (!DBNull.Value.Equals(reader["Aprobada"]))
                        oAvance.Aprobada = Convert.ToBoolean(reader["Aprobada"]);

                    lista.Add(oAvance);
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
            return lista;
        }

        /// <summary>
        /// Listado de ventas por mes para interventoria
        /// </summary>
        /// <param name="CodProyecto"> Codigo de proyecto </param>
        /// <returns> Listado de ventas por mes para interventoria </returns>
        public List<InterventorProduccionEntity> TraerInterventorVentasXMes(int CodProyecto)
        {
            List<InterventorProduccionEntity> lista = new List<InterventorProduccionEntity>();
            try
            {
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append(" SELECT Id_ventas   ");
                sbQuery.Append("     , CodProyecto  ");
                sbQuery.Append("     , NomProducto  ");
                sbQuery.Append("     , CodProducto  ");
                sbQuery.Append("     , Mes          ");
                sbQuery.Append("     , Valor        ");
                sbQuery.Append("     , Tipo         ");
                sbQuery.Append("     FROM InterventorVentas, InterventorVentasMes ");
                sbQuery.Append(" WHERE id_ventas = codproducto  ");
                sbQuery.Append(" AND codproyecto = " + CodProyecto);
                sbQuery.Append(" ORDER BY id_ventas, mes, tipo");

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                InterventorProduccionEntity oProduccion = new InterventorProduccionEntity();
                while (reader.Read())
                {
                    oProduccion = new InterventorProduccionEntity();
                    oProduccion.Id_produccion = Convert.ToInt32(reader["Id_ventas"]);
                    oProduccion.CodProyecto = Convert.ToInt32(reader["CodProyecto"]);
                    oProduccion.NomProducto = reader["NomProducto"].ToString();
                    oProduccion.Mes = Convert.ToInt32(reader["Mes"]);
                    oProduccion.Valor = Convert.ToDouble(reader["valor"]);
                    oProduccion.Tipo = Convert.ToInt32(reader["Tipo"]);

                    lista.Add(oProduccion);
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
            return lista;
        }

        /// <summary>
        /// Listado de avance de ventas por mes
        /// </summary>
        /// <param name="CodProducto"> Codigo de producto </param>
        /// <returns> Listado de avance de ventas por mes </returns>
        public List<AvanceVentasPOMes> TraerAvanceVentasPOMes(int CodProducto)
        {
            List<AvanceVentasPOMes> lista = new List<AvanceVentasPOMes>();
            try
            {
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append(" SELECT CodProducto               ");
                sbQuery.Append("      ,Mes                     ");
                sbQuery.Append("      ,CodTipoFinanciacion     ");
                sbQuery.Append("      ,Valor                   ");
                sbQuery.Append("      ,Observaciones           ");
                sbQuery.Append("      ,CodContacto             ");
                sbQuery.Append("      ,ObservacionesInterventor");
                sbQuery.Append("      ,Aprobada                ");
                sbQuery.Append("  FROM AvanceVentasPOMes        ");
                sbQuery.Append("  WHERE CodProducto = " + CodProducto);
            
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);
                AvanceVentasPOMes oAvance = new AvanceVentasPOMes();
                while (reader.Read())
                {
                    oAvance = new AvanceVentasPOMes();
                    oAvance.CodProducto = Convert.ToInt32(reader["CodProducto"]);
                    oAvance.Mes = Convert.ToByte(reader["Mes"]);
                    oAvance.CodTipoFinanciacion = Convert.ToByte(reader["CodTipoFinanciacion"]);
                    oAvance.Valor = Convert.ToDecimal(reader["Valor"]);
                    oAvance.Observaciones = reader["Observaciones"].ToString();
                    if (!DBNull.Value.Equals(reader["CodContacto"]))
                        oAvance.CodContacto = Convert.ToInt32(reader["CodContacto"]);
                    oAvance.ObservacionesInterventor = reader["ObservacionesInterventor"].ToString();
                    if (!DBNull.Value.Equals(reader["Aprobada"]))
                        oAvance.Aprobada = Convert.ToBoolean(reader["Aprobada"]);

                    lista.Add(oAvance);
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
            return lista;
        }

        /// <summary>
        /// Listado de actividades del plan operativo
        /// </summary>
        /// <param name="CodProyecto"> Codigo del proyecto </param>
        /// <returns> Listado de actividades del plan operativo </returns>
        public List<PlanOperativoEntity> TraerActividadesPlanOperativo(int CodProyecto)
        {
            List<PlanOperativoEntity> lista = new List<PlanOperativoEntity>();
            try
            {
                IDataReader reader;
                sbQuery = new StringBuilder();

                sbQuery.Append("SELECT  ");
                sbQuery.Append("  CodActividad ");
                sbQuery.Append(", Mes          ");
                sbQuery.Append(", CodTipoFinanciacion");
                sbQuery.Append(", Valor        ");
                sbQuery.Append(", Id_Actividad ");
                sbQuery.Append(", NomActividad ");
                sbQuery.Append(", CodProyecto  ");
                sbQuery.Append(", Item         ");
                sbQuery.Append(", Metas        ");
                sbQuery.Append("FROM ProyectoActividadPOMesInterventor ");
                sbQuery.Append("RIGHT OUTER JOIN ProyectoActividadPOInterventor ");
                sbQuery.Append("ON ProyectoActividadPOMesInterventor.CodActividad = ProyectoActividadPOInterventor.Id_Actividad ");
                sbQuery.Append("WHERE (ProyectoActividadPOInterventor.CodProyecto = " + CodProyecto + ")");
                sbQuery.Append("AND (ProyectoActividadPOMesInterventor.CodActividad IN  ( ");
                sbQuery.Append("		 Select id_Actividad                               ");
                sbQuery.Append("		  From proyectoactividadPOInterventor Where codproyecto =" + CodProyecto + "  ");
                sbQuery.Append("	 )    ");
                sbQuery.Append("	 )    ");
                sbQuery.Append("ORDER BY ProyectoActividadPOInterventor.Item, ProyectoActividadPOMesInterventor.Mes, ");
                sbQuery.Append("ProyectoActividadPOMesInterventor.CodTipoFinanciacion                                ");
            
                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);
                PlanOperativoEntity oPlan = new PlanOperativoEntity();
                while (reader.Read())
                {
                    oPlan = new PlanOperativoEntity();
                    oPlan.CodActividad = Convert.ToInt32(reader["CodActividad"]);
                    oPlan.Mes = Convert.ToInt16(reader["Mes"]);
                    oPlan.CodTipoFinanciacion = Convert.ToInt16(reader["CodTipoFinanciacion"]);
                    oPlan.Valor = Convert.ToDouble(reader["Valor"]);
                    oPlan.Id_Actividad = Convert.ToInt32(reader["Id_Actividad"]);
                    oPlan.NomActividad = reader["NomActividad"].ToString();
                    oPlan.CodProyecto = Convert.ToInt32(reader["CodProyecto"]);
                    oPlan.Item = Convert.ToInt16(reader["Item"]);
                    oPlan.Metas = reader["Metas"].ToString();

                    lista.Add(oPlan);
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
            return lista;
        }
    }
}
