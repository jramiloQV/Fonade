using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.Clases
{
    /// <summary>
    /// AgendarTarea
    /// </summary>  
    public class AgendarTarea : Negocio.Base_Page
    {
        private int ParaQuien { get; set; }
        private string NomTarea { get; set; }
        private string Descripcion { get; set; }
        private string CodProyecto { get; set; }
        private int CodTareaPrograma { get; set; }
        private string Recurrente { get; set; }
        private bool RecordatorioEmail { get; set; }
        private int NivelUrgencia { get; set; }
        private bool RecordatorioPantalla { get; set; }
        private bool RequiereRespuesta { get; set; }
        private int CodUsuarioAgendo { get; set; }
        private string Parametros { get; set; }
        private string DocumentoRelacionado { get; set; }
        private string Programa { get; set; }

        // <summary>
        // RecordatorioEmail = "bit".
        // </summary>
        //private int i_RecordatorioEmail = 0;

        // <summary>
        // RecordatorioPantalla = "bit".
        // </summary>
        //private int i_RecordatorioPantalla = 0;

        // <summary>
        // RequiereRespuesta = "bit".
        // </summary>
        //private int i_RequiereRespuesta = 0;

        /// <summary>
        /// Agendar tarea "básicamente se asignan los valores obtenidos en los parámetros a las variables internas".
        /// </summary>
        /// <param name="paraQuien"></param>
        /// <param name="nomTarea"></param>
        /// <param name="descripcion"></param>
        /// <param name="codProyecto"></param>
        /// <param name="codTareaPrograma"></param>
        /// <param name="recurrente"></param>
        /// <param name="recordatorioEmail"></param>
        /// <param name="nivelUrgencia"></param>
        /// <param name="recordatorioPantalla"></param>
        /// <param name="requiereRespuesta"></param>
        /// <param name="codUsuarioAgendo"></param>
        /// <param name="parametros"></param>
        /// <param name="documentoRelacionado"></param>
        /// <param name="programa"></param>
public AgendarTarea(int paraQuien, string nomTarea, string descripcion, string codProyecto, int codTareaPrograma, string recurrente, bool recordatorioEmail, int nivelUrgencia,
        bool recordatorioPantalla, bool requiereRespuesta, int codUsuarioAgendo,
        string parametros, string documentoRelacionado, string programa)
        {
            this.ParaQuien = paraQuien;
            this.NomTarea = nomTarea;
            this.Descripcion = descripcion;
            this.CodProyecto = string.IsNullOrEmpty(codProyecto) ? "NULL" : codProyecto;
            this.CodTareaPrograma = codTareaPrograma;
            this.Recurrente = recurrente == "" ? "0" : recurrente;
            this.RecordatorioEmail = recordatorioEmail;
            this.NivelUrgencia = nivelUrgencia;
            this.RecordatorioPantalla = recordatorioPantalla;
            this.RequiereRespuesta = requiereRespuesta;
            this.CodUsuarioAgendo = usuario.IdContacto;
            this.Parametros = parametros;
            this.DocumentoRelacionado = documentoRelacionado;
            this.Programa = programa;

        }

        /// <summary>
        /// Agendar tareas
        /// </summary>
        /// <returns>Cadena con la respuesta</returns>
        public string Agendar()
        {
            //Inicializar variables.
            DataTable RS = new DataTable();
            Consultas consulta = new Consultas();

            //Obtener el código de la tarea para generarla.
            int codigoTarea = InsertarTareaNueva();

            //Obtener la "respuesta (mensaje que verá el usuario al finalizar el proceso de generar tareas)".
            string respuesta = InsertarRepeticionTareaNueva(codigoTarea);

            if (this.RecordatorioEmail)
            {
                try
                {
                    var queryContacto = (from c in consulta.Db.Contacto
                                         where c.Id_Contacto == this.ParaQuien
                                         select new { c.Email, c.Nombres, c.Apellidos }).FirstOrDefault();

                    Correo correo = new Correo(usuario.Email, "Tarea Pendiente Fondo Emprender", queryContacto.Email, (queryContacto.Nombres + queryContacto.Apellidos), this.NomTarea, this.Descripcion);
                    correo.Enviar();
                }
                catch(Exception e)
                { 
                    respuesta = "La notificación por correo al usuario " + this.ParaQuien + " no pudo ser enviada. ("+e.GetType().Name+")"; 
                }
            }
            return respuesta;
        }

        /// <summary>
        /// Obtener el código de la tarea a generar "es decir, el nuevo ID a usar para la nueva tarea".
        /// </summary>
        /// <returns>Código de la tarea a usar // Si devuelve cero, es porque hubo un error o NO consultó el ID.</returns>
        private int InsertarTareaNueva()
        {
            String txtSQL = "";
            int codigoTarea = 0;
            Consultas consulta = new Consultas();
            DataTable RSTemporal = new DataTable();
            SqlCommand cmd = new SqlCommand();
            int i_RecordatorioEmail = 0;
            int i_RecordatorioPantalla = 0;
            int i_RequiereRespuesta = 0;

            if (RecordatorioEmail) { i_RecordatorioEmail = 1; }
            if (RecordatorioPantalla) { i_RecordatorioPantalla = 1; }
            if (RequiereRespuesta) { i_RequiereRespuesta = 1; }
            if (NomTarea.Contains("'")) { NomTarea = NomTarea.Replace("'", "&#39;"); }
            if (Descripcion.Contains("'")) { Descripcion = Descripcion.Replace("'", "&#39;"); }

            try
            {
                txtSQL = " INSERT INTO TareaUsuario " +
                         "  (CodContacto," +
                         "  CodProyecto,  " +
                         "  NomTareaUsuario, " +
                         "  Descripcion,  " +
                         "  CodTareaPrograma, " +
                         "  Recurrente, " +
                         "  RecordatorioEmail, " +
                         "  NivelUrgencia, " +
                         "  RecordatorioPantalla," +
                         "  RequiereRespuesta, " +
                         "  CodContactoAgendo) " +
                         "  VALUES " +
                         "  (" + ParaQuien + ", " +
                         "  " + CodProyecto + ",  " +
                         "  '" + NomTarea + "', " +
                         "  '" + Descripcion + "',  " +
                         "  " + CodTareaPrograma + ", " +
                         "  '" + Recurrente + "', " +
                         "  " + i_RecordatorioEmail + ", " +
                         "  " + NivelUrgencia + ", " +
                         "  " + i_RecordatorioPantalla + "," +
                         "  " + i_RequiereRespuesta + ", " +
                         "  " + CodUsuarioAgendo + ")"; 

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    cmd = new SqlCommand();

                    if (con != null) { 
                        if (con.State != ConnectionState.Open || con.State != ConnectionState.Broken ) { con.Open(); }
                    }

                    cmd.CommandType = CommandType.Text;

                    cmd.Connection = con;
                    cmd.CommandText = txtSQL;
                    
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    if (CodProyecto.ToString().Trim()=="")
                    {
                        //Se averigua el autonumérico generado en TareaUsuario.
                        txtSQL = " SELECT Max(Id_TareaUsuario) AS Maximo FROM TareaUsuario WHERE CodContacto = " + ParaQuien +
                                 " AND NomTareaUsuario = '" + NomTarea + "' AND CodTareaPrograma = " + CodTareaPrograma +
                                 " AND Recurrente = '" + Recurrente + "' AND RecordatorioEmail = " + i_RecordatorioEmail +
                                 " AND NivelUrgencia = " + NivelUrgencia + " AND RecordatorioPantalla = " + i_RecordatorioPantalla +
                                 " AND RequiereRespuesta = " + i_RequiereRespuesta +
                                 " AND CodContactoAgendo = " + CodUsuarioAgendo +
                                 " AND CodProyecto is NULL " ;

                    }
                    else { 
                    //Se averigua el autonumérico generado en TareaUsuario.
                    txtSQL = " SELECT Max(Id_TareaUsuario) AS Maximo FROM TareaUsuario WHERE CodContacto = " + ParaQuien +
                             " AND NomTareaUsuario = '" + NomTarea + "' AND CodTareaPrograma = " + CodTareaPrograma +
                             " AND Recurrente = '" + Recurrente + "' AND RecordatorioEmail = " + i_RecordatorioEmail +
                             " AND NivelUrgencia = " + NivelUrgencia + " AND RecordatorioPantalla = " + i_RecordatorioPantalla +
                             " AND RequiereRespuesta = " + i_RequiereRespuesta +
                             " AND CodContactoAgendo = " + CodUsuarioAgendo;
                        if(CodProyecto.Trim().ToLower() == "null")
                        {
                            txtSQL += " AND CodProyecto IS NULL";
                        }
                        else
                        {
                            txtSQL += " AND CodProyecto = " + CodProyecto;
                        }        
                    }
                    
                    RSTemporal = consulta.ObtenerDataTable(txtSQL, "text");

                    if (RSTemporal.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(RSTemporal.Rows[0]["Maximo"].ToString()))
                        {
                            codigoTarea = Int32.Parse(RSTemporal.Rows[0]["Maximo"].ToString());
                        } 
                    }

                    RSTemporal = null;
                }
                catch (Exception ex) { string error_msg = ex.Message; codigoTarea = 0; }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
            catch { codigoTarea = 0; }
            return codigoTarea;
        }

        /// <summary>
        /// Realiza la inserción en la tabla "TareaUsuarioRepeticion".
        /// </summary>
        /// <param name="codigoTarea">Código de la tarea recién generada y consultada en el método "InsertarTareaNueva".</param>
        /// <returns>respuesta "mensaje que verá el usuario al terminarse el proceso de generar la tarea".</returns>
        private string InsertarRepeticionTareaNueva(int codigoTarea)
        {
            //Inicializar variables.
            Consultas consulta = new Consultas();
            SqlCommand cmd = new SqlCommand();
            String txtSQL = "";
            string respuesta = "La tarea " + this.NomTarea + " ha sido agendada.";

            try
            {
                //Se inserta en la tabla de repeticiones TareaUsuarioRepeticion.
                txtSQL = " INSERT INTO TareaUsuarioRepeticion (Fecha, CodTareaUsuario, Parametros) " +
                         " VALUES (GETDATE()," + codigoTarea + ",'" + Parametros + "'" + ") ";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                try
                {
                    cmd = new SqlCommand(txtSQL, con);

                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                catch (Exception ex) { respuesta = ex.Message; }
                finally { con.Close(); con.Dispose(); }
            }
            catch (Exception ex) { respuesta = ex.Message; }

            return respuesta;
        }
    }
}