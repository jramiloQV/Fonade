using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fonade.Negocio.Entidades;

namespace Fonade.Negocio.Evaluacion
{
    /// <summary>
    /// Clase para manejar la información de catalogo de actividades
    /// </summary>
    public class CatalogoActividadPONegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();
        Consultas consultas = new Consultas();

        /// <summary>
        /// Metodo que agrega la lista de actividades de proyecto a la tabla(proyectoactividadPOInterventor)
        /// </summary>
        /// <param name="ObjCatActividadPO"> Objecto de Catalogo Actividad </param>
        /// <returns> Cantidad de inserciones </returns>
        public int Agregar_proyectoactividadPOInterventor(Fonade.Negocio.Entidades.ProyectoActividadPOInterventorEntity ObjCatActividadPO)
        {

            int creado = 0;
            var query = "INSERT INTO proyectoactividadPOInterventor (NomActividad,CodProyecto,Item,Metas) VALUES ('" + ObjCatActividadPO.NomActividad;
            query += "', " + ObjCatActividadPO.CodProyecto + ", " + ObjCatActividadPO.Item + ", '" + ObjCatActividadPO.Metas + "');";
            query += " Select @@ROWCOUNT";
            creado = (int) consultas.RetornarEscalar(query, "text");

            return creado;
        }

        /// <summary>
        /// Metodo que agrega actaliza la información de las actividades de proyecto en la tabla(proyectoactividadPOInterventor)
        /// </summary>
        /// <param name="ObjCatActividadPO"> Objeto de Proyecto actividad Interventor</param>
        /// <returns> Cantidad de inserciones </returns>
        public int Actualizar_proyectoactividadPOInterventor(Fonade.Negocio.Entidades.ProyectoActividadPOInterventorEntity ObjCatActividadPO)
        {            
            int actualizado = 0;
            try
            {
                sbQuery = new StringBuilder();
                sbQuery.Append("Update proyectoactividadPOInterventor set CodProyecto = ");
                sbQuery.Append(ObjCatActividadPO.CodProyecto);
                sbQuery.Append(",Item = ");
                sbQuery.Append(ObjCatActividadPO.Item);
                sbQuery.Append(",NomActividad = '");
                sbQuery.Append(ObjCatActividadPO.NomActividad);
                sbQuery.Append("', Metas = '");
                sbQuery.Append(ObjCatActividadPO.Metas);
                sbQuery.Append("'' Where CodProyecto = ");
                sbQuery.Append(ObjCatActividadPO.CodProyecto);
                sbQuery.Append(" and Item = ");
                sbQuery.Append(ObjCatActividadPO.Item);
            
                db.Open();
                actualizado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                db.Close();
            }
            return actualizado;
        }
        
        /// <summary>
        /// Metodo que lista las actividades de proyecto tabla(proyectoactividadPOInterventor)
        /// </summary>
        /// <param name="CodProyecto">Codigo de proyecto</param>
        /// <param name="Item"> Codigo de actividad </param>
        /// <returns>Listado de Actividades de un proyecto </returns>
       
        public List<Negocio.Entidades.ProyectoActividadPOInterventorEntity> GetActividadPOInterventor(int CodProyecto, int Item)
        {
            List<Negocio.Entidades.ProyectoActividadPOInterventorEntity> lst = new List<Negocio.Entidades.ProyectoActividadPOInterventorEntity>();
            IDataReader reader;
            sbQuery = new StringBuilder();
            try
            {
                sbQuery.Append("select * from proyectoactividadPOInterventor where CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" and Item = ");
                sbQuery.Append(Item);

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Negocio.Entidades.ProyectoActividadPOInterventorEntity oPryActividadInter = new Negocio.Entidades.ProyectoActividadPOInterventorEntity();
                while (reader.Read())
                {
                    oPryActividadInter = new Negocio.Entidades.ProyectoActividadPOInterventorEntity();
                    oPryActividadInter.Id_Actividad = Convert.ToInt32(reader["Id_Actividad"]);
                    oPryActividadInter.NomActividad = reader["NomActividad"].ToString();
                    oPryActividadInter.CodProyecto = Convert.ToInt32(reader["CodProyecto"]);
                    oPryActividadInter.Item = Convert.ToDouble(reader["Item"]);
                    oPryActividadInter.Metas = reader["Metas"].ToString();

                    lst.Add(oPryActividadInter);
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
        /// Metodo que lista las actividades de proyecto tabla(proyectoactividadPOInterventor)
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <param name="Item"> Codigo de actividad </param>
        /// <returns>Listado de actividades de un proyecto</returns>
        public int OptenerId_ActividadPOInterventor(String CodProyecto, int Item)
        {
            int Id_Actividad = 0;
            IDataReader reader;
            sbQuery = new StringBuilder();
            try
            {
                sbQuery.Append("SELECT id_Actividad FROM proyectoactividadPOInterventor WHERE  CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" and Item = ");
                sbQuery.Append(Item);

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Id_Actividad = Convert.ToInt32(reader["id_Actividad"]);
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
            return Id_Actividad;
        }

        /// <summary>
        /// Metodo que elimina los registros de las actividades ya actualizadas en la tabla definitiva
        /// </summary>
        /// <param name="CodProyecto">Codigo del proyecto</param>
        /// <param name="CodActividad">Codigo de la actividad</param>
        /// <returns> int Cantidad de inserciones </returns>
        public int Elimina_Actividad_proyectoactividadPOInterventorTMP(int CodProyecto, int CodActividad)
        {
            int eliminado = 0;
            var query = "DELETE FROM proyectoactividadPOInterventorTMP WHERE CodProyecto = " + CodProyecto + " AND Id_Actividad = " + CodActividad + "; ";
            query += "Select @@ROWCOUNT;";

            eliminado = (int) consultas.RetornarEscalar(query, "text");
            
            return eliminado;
        }
        
        /// <summary>
        /// Metodo que lista las actividades temporales de un proyecto  tabla (proyectoactividadPOInterventorTMP)
        /// </summary>
        /// <param name="CodProyecto"> Codigo de proyecto </param>
        /// <param name="CodActividad"> Codigo de actividad </param>
        /// <returns> Listado de actividades temporales de un proyecto </returns>
        public List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity> GetActividadPOInterventorTPM(int CodProyecto, int CodActividad)
        {
            List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity> lst = new List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity>();
            IDataReader reader;
            sbQuery = new StringBuilder();
            try
            {
                sbQuery.Append("select * from proyectoactividadPOInterventorTMP where CodProyecto = ");
                sbQuery.Append(CodProyecto);
                sbQuery.Append(" and Id_Actividad = ");
                sbQuery.Append(CodActividad);

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity oPryActividadInter = new Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity();
                while (reader.Read()){
                    oPryActividadInter = new Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity();
                    oPryActividadInter.Id_Actividad = Convert.IsDBNull(reader["Id_Actividad"])?0:Convert.ToInt32(reader["Id_Actividad"]);
                    oPryActividadInter.NomActividad = Convert.IsDBNull(reader["NomActividad"])?string.Empty : Convert.ToString(reader["NomActividad"]);
                    oPryActividadInter.CodProyecto = Convert.IsDBNull(reader["CodProyecto"]) ? 0 : Convert.ToInt32(reader["CodProyecto"]);
                    oPryActividadInter.Item = Convert.IsDBNull(reader["Item"]) ? 0 : Convert.ToDouble(reader["Item"]);
                    oPryActividadInter.Metas = Convert.IsDBNull(reader["Metas"]) ? string.Empty : Convert.ToString(reader["Metas"]);
                    lst.Add(oPryActividadInter);
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
        /// Metodo que lista las actividades temporales por mes de un proyecto  tabla(proyectoactividadPOMesInterventorTMP)
        /// </summary>
        /// <param name="CodActividad">Codigo de actividad</param>
        /// <returns> Actividades temporales de un proyecto </returns> 
        public List<Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity> GetActividadesTMPDefinitivas(int CodActividad)
        {
            List<Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity> lst = new List<Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity>();
            IDataReader reader;
            sbQuery = new StringBuilder();
            try
            {
                sbQuery.Append("select distinct * from ProyectoActividadPOMesInterventorTMP WHERE  CodActividad = ");
                sbQuery.Append(CodActividad);
                sbQuery.Append(" and valor is not null ");

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity objActMesInterventorTMP = new Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity();
                while (reader.Read())
                {
                    objActMesInterventorTMP = new Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity();
                    objActMesInterventorTMP.CodActividad = Convert.ToInt32(reader["CodActividad"]);
                    objActMesInterventorTMP.Mes = Convert.ToInt32(reader["Mes"]);
                    objActMesInterventorTMP.CodTipoFinanciacion = Convert.ToInt32(reader["CodTipoFinanciacion"]);
                    objActMesInterventorTMP.Valor = Convert.ToInt32(reader["Valor"]);

                    lst.Add(objActMesInterventorTMP);
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
        /// Agregar o eliminar actividades por mes del interventor
        /// </summary>
        /// <param name="objActMesInterventorTMP">Objecto de entity framework Proyecto actividad por mes</param>
        public void Agregar_o_Eliminar_ActividadMesInterventor(Negocio.Entidades.ProyectoActividadPOMesInterventorTMPEntity objActMesInterventorTMP)
        {
            IDataReader reader;
            sbQuery = new StringBuilder();
            List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity> lst = new List<Negocio.Entidades.ProyectoActividadPOInterventorTMPEntity>();

            try
            {
                sbQuery.Append("SELECT Count(CodActividad) as Cantidad from ProyectoActividadPOMesInterventor WHERE  CodActividad = ");
                sbQuery.Append(objActMesInterventorTMP.CodActividad);
                sbQuery.Append(" AND MES = ");
                sbQuery.Append(objActMesInterventorTMP.Mes);
                sbQuery.Append(" group by CodActividad");

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Negocio.Entidades.ProyectoActividadPOMesInterventorEntity objActMesInterventor = new Negocio.Entidades.ProyectoActividadPOMesInterventorEntity();
                objActMesInterventor.CodActividad = objActMesInterventorTMP.CodActividad;
                objActMesInterventor.Mes = objActMesInterventorTMP.Mes;
                objActMesInterventor.CodTipoFinanciacion = objActMesInterventorTMP.CodTipoFinanciacion;
                objActMesInterventor.Valor = objActMesInterventorTMP.Valor;
                if (reader.Read())
                {
                    Actualizar_ProyectoActividadPOMesInterventor(objActMesInterventor);                    
                }
                else
                {
                    Agregar_ProyectoActividadPOMesInterventor(objActMesInterventor);
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
        }

        /// <summary>
        /// Metodo que lista las actividades mensuales de un proyecto  tabla(proyectoactividadPOMesInterventorTMP)
        /// </summary>
        /// <param name="Actividad">Codigo de actividad</param>
        /// <param name="mes"> numero de mes </param>
        /// <returns> List<ProyectoactividadPOMesInterventor> Actividades definitivas de un proyecto </returns>
        public List<Negocio.Entidades.ProyectoActividadPOMesInterventorEntity> GetActiPOMesInterventor(int Actividad, int mes)
        {
            List<Negocio.Entidades.ProyectoActividadPOMesInterventorEntity> lst = new List<Negocio.Entidades.ProyectoActividadPOMesInterventorEntity>();
            IDataReader reader;
            sbQuery = new StringBuilder();
            try
            {
                sbQuery.Append("select * from ProyectoActividadPOMesInterventor WHERE  CodActividad = ");
                sbQuery.Append(Actividad);
                sbQuery.Append(" and mes = ");
                sbQuery.Append(mes);

                db.Open();
                reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);

                Negocio.Entidades.ProyectoActividadPOMesInterventorEntity objActMesInterventor = new Negocio.Entidades.ProyectoActividadPOMesInterventorEntity();
                while (reader.Read())
                {
                    objActMesInterventor = new Negocio.Entidades.ProyectoActividadPOMesInterventorEntity();
                    objActMesInterventor.CodActividad = Convert.ToInt32(reader["CodActividad"]);
                    objActMesInterventor.Mes = Convert.ToInt32(reader["Mes"]);
                    objActMesInterventor.CodTipoFinanciacion = Convert.ToInt32(reader["CodTipoFinanciacion"]);
                    objActMesInterventor.Valor = Convert.ToInt32(reader["Valor"]);

                    lst.Add(objActMesInterventor);
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
        /// Metodo que inserta las actividades mensuales definitivas de un proyecto  tabla(proyectoactividadPOMesInterventor)
        /// </summary>
        /// <param name="objActMesInterventor">Objeto de EF Proyecto actividad interventor </param>
        public void Agregar_ProyectoActividadPOMesInterventor(Negocio.Entidades.ProyectoActividadPOMesInterventorEntity objActMesInterventor)
        {
            int creado = 0;
            sbQuery = new StringBuilder();
            sbQuery.Append("INSERT INTO ProyectoActividadPOMesInterventor (CodActividad,Mes,CodTipoFinanciacion,Valor) VALUES (");
            sbQuery.Append(objActMesInterventor.CodActividad);
            sbQuery.Append(",");
            sbQuery.Append(objActMesInterventor.Mes);
            sbQuery.Append(",");
            sbQuery.Append(objActMesInterventor.CodTipoFinanciacion);
            sbQuery.Append(",");
            sbQuery.Append(objActMesInterventor.Valor);
            sbQuery.Append(")");
            try
            {
                db.Open();
                creado = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
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

        /// <summary>
        /// Metodo que actualiza las actividades mensuales definitivas de un proyecto  tabla(proyectoactividadPOMesInterventor)
        /// </summary>
        /// <param name="objActMesInterventor"> Objeto <ProyectoactividadPOMesInterventor></param>
        public void Actualizar_ProyectoActividadPOMesInterventor(Negocio.Entidades.ProyectoActividadPOMesInterventorEntity objActMesInterventor)
        {
            sbQuery = new StringBuilder();

            sbQuery.Append("UPDATE ProyectoActividadPOMesInterventor SET CodActividad = ");
            sbQuery.Append(objActMesInterventor.CodActividad);
            sbQuery.Append(", Mes = ");
            sbQuery.Append(objActMesInterventor.Mes);
            sbQuery.Append(", CodTipoFinanciacion = ");
            sbQuery.Append(objActMesInterventor.CodTipoFinanciacion);
            sbQuery.Append(", Valor = ");
            sbQuery.Append(objActMesInterventor.Valor);
            sbQuery.Append(" WHERE CodActividad = ");
            sbQuery.Append(objActMesInterventor.CodActividad);
            sbQuery.Append(" AND MES = ");
            sbQuery.Append(objActMesInterventor.Mes);
            try
            {
                db.Open();
                db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
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

        /// <summary>
        /// Metodo que elimina las actividades mensuales temporales de un proyecto  tabla(proyectoactividadPOMesInterventorTMP)
        /// </summary>
        /// <param name="CodActividad"> Codigo de actividad </param>
        public void Borrar_ProyectoActividadPOMesInterventorTMP(int CodActividad)
        {
            sbQuery = new StringBuilder();
            sbQuery.Append("DELETE ProyectoActividadPOMesInterventorTMP WHERE CodActividad = ");
            sbQuery.Append(CodActividad);
            try
            {
                db.Open();
                db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
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
    }
}
