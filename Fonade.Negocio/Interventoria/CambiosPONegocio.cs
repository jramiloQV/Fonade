using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Interventoria
{
    /// <summary>
    /// Clase para cambios de planes de negocio
    /// </summary>
    public class CambiosPONegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();

        /// <summary>
        /// Eliminar una actividad del proyecto
        /// </summary>
        /// <param name="CodProyecto"> Codigo del proyecto </param>
        /// <param name="NomActividad">Nombre de la actividad </param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int Eliminar_ProyectoActividadPOInterventorTMP(int CodProyecto, String NomActividad)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM ProyectoActividadPOInterventorTMP WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" and NomActividad = '");
                sbQuery.Append(NomActividad);
                sbQuery.Append("'");
            
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }

        /// <summary>
        /// Metodo que elimina las actividades mensuales temporales del proyecto
        /// </summary>
        /// <param name="CodActividad">Codigo de actividad</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int Eliminar_ProyectoActividadPOMesInterventorTMP(int CodActividad)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM ProyectoActividadPOMesInterventorTMP WHERE CodActividad = ");
                sbQuery.Append(CodActividad);
            
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }
        
        /// <summary>
        /// Metodo que elimina las actividades mensuales del proyecto
        /// </summary>
        /// <param name="CodActividad">Codigo de actividad</param>
        /// <returns>  Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int Eliminar_ProyectoActividadPOMesInterventor(int CodActividad)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM ProyectoActividadPOMesInterventor WHERE CodActividad = ");
                sbQuery.Append(CodActividad);
            
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }


        /// <summary>
        /// Metodo que elimina la información temporal de nómina e interventoría
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int Eliminar_InterventorNominaTMP(int CodProyecto)
        {
            int eliminado = 0;
            sbQuery = new StringBuilder();
            sbQuery.Append("DELETE FROM InterventorNominaTMP WHERE CodProyecto = ");
            sbQuery.Append(CodProyecto);
            try
            {
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }

        /// <summary>
        ///  Metodo que elimina la información temporal de producción
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>

        public int Eliminar_InterventorProduccionTMP(int CodProyecto)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM InterventorProduccionTMP WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
                    db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }

        /// <summary>
        ///  Metodo que elimina la información temporal de las ventas
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int Eliminar_InterventorVentasTMP(int CodProyecto)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("DELETE FROM InterventorVentasTMP WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
            
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }

        /// <summary>
        ///  Metodo para aprobar la tarea Borrar y enviarla al interventor
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int EnviaTareaBorrar_A_GerentePOInterventorTMP(int CodProyecto, String NomActividad)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE ProyectoActividadPOInterventorTMP  SET ChequeoCoordinador = 1,Tarea = 'Borrar' WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" and NomActividad = '");
                sbQuery.Append(CodProyecto);
                sbQuery.Append("'");
            
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }


        /// <summary>
        ///  Metodo para aprobar la tarea Borrar y enviarla al gerente interventor para su eliminación definitiva
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int EnviaTareaBorrarNominaTMP_A_GerenteInterventor(int CodProyecto, int ID_Nomina)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE InterventorNominaTMP SET ChequeoCoordinador = 1,Tarea = 'Borrar' WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" AND ID_Nomina = ");
                sbQuery.Append("ID_Nomina");
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }

        /// <summary>
        ///  Metodo para aprobar la tarea Borrar Produccion y enviarla al gerente interventor para su eliminación definitiva
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>
        public int EnviaTareaBorrarProduccionTMP_A_GerenteInterventor(int CodProyecto, int Id_Produccion)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE InterventorProduccionTMP SET ChequeoCoordinador = 1,Tarea = 'Borrar' WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" AND Id_Produccion = ");
                sbQuery.Append(Id_Produccion);
                    db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }

        /// <summary>
        ///  Metodo para aprobar la tarea Borrar Ventas y enviarla al gerente interventor para su eliminación definitiva
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <returns> Entero >= 1 que indica que el proceso se realizó correctamente </returns>         
        public int EnviaTareaBorrarVentasTMP_A_GerenteInterventor(int CodProyecto, String NomProducto)
        {
            int eliminado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("UPDATE InterventorVentasTMP SET ChequeoCoordinador = 1,Tarea = 'Borrar' WHERE CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" AND NomProducto = '");
                sbQuery.Append(NomProducto);
                sbQuery.Append("'");
            
                db.Open();
                eliminado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return eliminado;
        }
    }
}
