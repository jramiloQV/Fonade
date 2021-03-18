using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fonade.Negocio.Entidades;
using Fonade.DbAccess;
using System.Data.SqlClient;

namespace Fonade.Negocio.Interventoria
{
    /// <summary> 
    /// Clase para el manejo de la información de acreditación
    /// </summary>    
    public class ProyectoAcreditacionNegocio
    {
        SQLManager db = new SQLManager();
        
        /// <summary>
        /// Agrega un registro al Historico de acreditación
        /// </summary>
        /// <param name="oAcreditacion"> Objecto de acreditación </param>
        /// <returns>Numero de inserciones</returns>
        public int Agregar(HistoricoEmailAcreditacionEntity oAcreditacion)
        {
            int reg = 0;

            StringBuilder sbQuery = new StringBuilder();

            try
            {
                sbQuery.Append("INSERT INTO HistoricoEmailAcreditacion ( ");
                sbQuery.Append(" CodProyecto           ");
                sbQuery.Append(",CodConvocatoria       ");
                sbQuery.Append(",CodContacto           ");
                sbQuery.Append(",Email                 ");
                sbQuery.Append(",Fecha                 ");
                sbQuery.Append(",CodContactoEnvia      ");
                sbQuery.Append(",CodEstadoAcreditacion ");
                sbQuery.Append(" ) VALUES ( ");
                sbQuery.Append(" @CodProyecto");
                sbQuery.Append(",@CodConvocatoria");
                sbQuery.Append(",@CodContacto");
                sbQuery.Append(",@Email");
                sbQuery.Append(",@Fecha");
                if (oAcreditacion.CodContactoEnvia != null)
                    sbQuery.Append(",@CodContactoEnvia");
                if (oAcreditacion.CodEstadoAcreditacion != null)
                    sbQuery.Append(",@CodEstadoAcreditacion");
                sbQuery.Append(" ) ");

                db.Open();
                
                db.Parametros.Add(new SqlParameter("@CodProyecto", oAcreditacion.CodProyecto));
                db.Parametros.Add(new SqlParameter("@CodConvocatoria", oAcreditacion.CodConvocatoria));
                db.Parametros.Add(new SqlParameter("@CodContacto", oAcreditacion.CodContacto));
                db.Parametros.Add(new SqlParameter("@Email", oAcreditacion.Email));
                db.Parametros.Add(new SqlParameter("@Fecha", oAcreditacion.Fecha));
                if (oAcreditacion.CodContactoEnvia != null)
                    db.Parametros.Add(new SqlParameter("@CodContactoEnvia", oAcreditacion.CodContactoEnvia));
                if (oAcreditacion.CodEstadoAcreditacion != null)
                    db.Parametros.Add(new SqlParameter("@CodEstadoAcreditacion", oAcreditacion.CodEstadoAcreditacion));

                reg = db.ExecuteNonQuery(sbQuery.ToString(), System.Data.CommandType.Text);
            }
            catch (SqlException)
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
