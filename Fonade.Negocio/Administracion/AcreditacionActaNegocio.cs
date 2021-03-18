using Datos;
using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Administracion
{
    public class AcreditacionActaNegocio
    {
        SQLManager db = new SQLManager();
        StringBuilder sbQuery = new StringBuilder();
        Consultas consultas = new Consultas();

        /// <summary>
        /// Metodo que devuelve la lista de los contactos que pueden crear Actas parciales de Acreditacion
        /// </summary>
        /// <returns> Una lista de strings con lso emails de los contactos </returns>
        public List<Contacto> TraerUsuariosCreaActaParcialAcreditacion(int? _codOperador)
        {

            var listContactos = (from c in consultas.Db.Contacto
                                 where c.flagActaParcial == true && c.Inactivo == false
                                 && c.codOperador == _codOperador
                                 select c).ToList();
            //List<Contacto> lstEmails = new List<Contacto>();
            //IDataReader reader;
            //sbQuery = new StringBuilder();
            //sbQuery.Append("SELECT Id_Contacto          ");
            //sbQuery.Append("      ,Nombres              ");
            //sbQuery.Append("      ,Apellidos            ");
            //sbQuery.Append("      ,CodTipoIdentificacion");
            //sbQuery.Append("      ,Identificacion       ");
            //sbQuery.Append("      ,Genero               ");
            //sbQuery.Append("      ,FechaNacimiento      ");
            //sbQuery.Append("      ,Cargo                ");
            //sbQuery.Append("      ,Email                ");
            //sbQuery.Append("      ,Direccion            ");
            //sbQuery.Append("      ,Telefono             ");
            //sbQuery.Append("      ,Fax                  ");
            //sbQuery.Append("      ,Experiencia          ");
            //sbQuery.Append("      ,Dedicacion           ");
            //sbQuery.Append("      ,HojaVida             ");
            //sbQuery.Append("      ,Intereses            ");
            //sbQuery.Append("      ,CodInstitucion       ");
            //sbQuery.Append("      ,CodCiudad            ");
            //sbQuery.Append("      ,Clave                ");
            //sbQuery.Append("      ,Inactivo             ");
            //sbQuery.Append("      ,InactivoAsignacion   ");
            //sbQuery.Append("      ,CodTipoAprendiz      ");
            //sbQuery.Append("      ,fechaCreacion        ");
            //sbQuery.Append("      ,LugarExpedicionDI    ");
            //sbQuery.Append("      ,InformacionIncompleta");
            //sbQuery.Append("      ,fechaActualizacion   ");
            //sbQuery.Append("      ,fechaCambioClave     ");
            //sbQuery.Append("      ,flagAcreditador      ");
            //sbQuery.Append("      ,flagActaParcial      ");
            //sbQuery.Append("      ,flagGeneraReporte    ");
            //sbQuery.Append("  FROM Contacto             ");
            //sbQuery.Append("  WHERE flagActaParcial = 1 ");

            //try
            //{
            //    db.Open();
            //    reader = db.ExecuteDataReader(sbQuery.ToString(), CommandType.Text);
            //    Contacto oContacto = new Contacto();
            //    while (reader.Read())
            //    {
            //        oContacto = new Contacto();

            //        oContacto.Id_Contacto = Convert.ToInt32(reader["Id_Contacto"]);
            //        oContacto.Nombres = reader["Nombres"].ToString();
            //        oContacto.Apellidos = reader["Apellidos"].ToString();
            //        oContacto.CodTipoIdentificacion = Convert.ToInt16(reader["CodTipoIdentificacion"]);
            //        oContacto.Identificacion = Convert.ToDouble(reader["Identificacion"]);
            //        if (!DBNull.Value.Equals(reader["Genero"]))
            //            oContacto.Genero = Convert.ToChar(reader["Genero"]);
            //        if (!DBNull.Value.Equals(reader["FechaNacimiento"]))
            //            oContacto.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);
            //        oContacto.Cargo = reader["Cargo"].ToString();
            //        oContacto.Email = reader["Email"].ToString();
            //        oContacto.Direccion = reader["Direccion"].ToString();
            //        oContacto.Telefono = reader["Telefono"].ToString();
            //        oContacto.Fax = reader["Fax"].ToString();
            //        oContacto.Experiencia = reader["Experiencia"].ToString();
            //        oContacto.Dedicacion = reader["Dedicacion"].ToString();
            //        oContacto.HojaVida = reader["HojaVida"].ToString();
            //        oContacto.Intereses = reader["Intereses"].ToString();
            //        if (!DBNull.Value.Equals(reader["CodInstitucion"]))
            //            oContacto.CodInstitucion = Convert.ToInt32(reader["CodInstitucion"]);
            //        if (!DBNull.Value.Equals(reader["CodCiudad"]))
            //        oContacto.CodCiudad = Convert.ToInt32(reader["CodCiudad"]);
            //        if (!DBNull.Value.Equals(reader["LugarExpedicionDI"]))
            //        oContacto.LugarExpedicionDI = Convert.ToInt32(reader["LugarExpedicionDI"]);  

            //        lstEmails.Add(oContacto);
            //    }
            //    reader.Close();

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    db.Close();
            //}
            //return lstEmails;

            return listContactos;
        }
    }
}
