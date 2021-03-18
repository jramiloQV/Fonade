#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 07 - 2014</Fecha>
// <Archivo>PostIt.cs</Archivo>

#endregion

#region using

using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace Fonade.Controles
{
    /// <summary>
    /// PostIt
    /// </summary>
    
    public partial class PostIt : Negocio.Base_Page
    {
        String CodTareaPrograma;
        String ParaQuien;
        String Proyecto;
        String NomTarea;
        String Descripcion;
        String Recurrente;
        Boolean RecordatorioEmail;
        String NivelUrgencia;
        Boolean RecordatorioPantalla;
        Boolean RequiereRespuesta;
        String CodContactoAgendo;
        String DocumentoRelacionado;
        //String intTemp;

        private String codProyecto;
        private Int32 CodUsuario;
        private String Accion;
        private Int32 codGrupo;

        private Int32 CONST_PostIt;
        private String tabEval;
        private String txtCampo;

        String txtSQL = "";

        private DataTable _DataContactos;
        private Consultas consulta = new Consultas();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            recogeSession();

            if (!IsPostBack)
            {

                C_Fecha.SelectedDate = DateTime.Now;

                //busca el nombre del usuario en la base de datos
                var contacto = (from c in consulta.Db.Contacto
                                where c.Id_Contacto == CodUsuario
                                select new 
                                {
                                    c.Nombres, 
                                    c.Apellidos
                                }).FirstOrDefault();

                //muestra en un label el nombre del usuario quien asignara la tarea
                L_Nombreusuario.Text = contacto.Nombres + " " + contacto.Apellidos;

                if (String.IsNullOrEmpty(Accion))
                    L_PostIt.Text = "AGENDAR POST IT";
                else
                {
                    if (Accion.Equals("Modificar") || Accion.Equals("Consultar"))
                        L_PostIt.Text = "REVISAR POST IT";
                    else
                        L_PostIt.Text = "AGENDAR POST IT";
                }

                Queryable();

                if (_DataContactos != null)
                    CrearlistemItem(_DataContactos);
            }
        }

        /// <summary>
        /// Handles the Click event of the B_Grabar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void B_Grabar_Click(object sender, EventArgs e)
        {
            foreach (ListItem item in LB_Para.Items)
            {
                if (item.Selected)
                {
                    CodTareaPrograma = "" + CONST_PostIt;
                    ParaQuien = item.Value;
                    Proyecto = "" + codProyecto;
                    NomTarea = TB_Tarea.Text;
                    Descripcion = TB_Descripcion.Text;
                    Recurrente = "0";
                    if (DD_Email.SelectedValue == "1")
                    {
                        RecordatorioEmail = true;
                    }
                    else
                    {
                        RecordatorioEmail = false;
                    }
                    NivelUrgencia = "1";
                    RecordatorioPantalla = false;
                    RequiereRespuesta = false;
                    CodContactoAgendo = "" + CodUsuario;
                    DocumentoRelacionado = " ";

                    string conexionStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
                    
                    int last_id = 0;

                    using (var con = new SqlConnection(conexionStr))
                    {
                        using (var com = con.CreateCommand())
                        {
                            com.CommandText = "MD_NuevaTareaInsert";
                            com.CommandType = System.Data.CommandType.StoredProcedure;
                            com.Parameters.AddWithValue("@CodTareaPrograma", CodTareaPrograma);
                            com.Parameters.AddWithValue("@CodContacto", ParaQuien);
                            com.Parameters.AddWithValue("@CodProyecto", Proyecto);
                            com.Parameters.AddWithValue("@NomTareaUsuario", NomTarea);
                            com.Parameters.AddWithValue("@Descripcion", Descripcion);
                            com.Parameters.AddWithValue("@Recurrente", Recurrente);
                            com.Parameters.AddWithValue("@RecordatorioEmail", RecordatorioEmail);
                            com.Parameters.AddWithValue("@NivelUrgencia", NivelUrgencia);
                            com.Parameters.AddWithValue("@RecordatorioPantalla", RecordatorioPantalla);
                            com.Parameters.AddWithValue("@RequiereRespuesta", RequiereRespuesta);
                            com.Parameters.AddWithValue("@CodContactoAgendo", CodContactoAgendo);
                            com.Parameters.AddWithValue("@DocumentoRelacionado", DocumentoRelacionado);

                            SqlParameter retval = com.Parameters.Add("@UltimoRegistroInsertado", SqlDbType.Int);
                            retval.Direction = ParameterDirection.ReturnValue;
                            

                            try
                            {
                                con.Open();
                                com.ExecuteNonQuery(); // MISSING
                                last_id = (int)retval.Value;

                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                            finally
                            {
                                com.Dispose();
                                con.Close();
                                con.Dispose();
                            }
                        }
                    }

                    TareaUsuarioRepeticion(last_id);

                    if (RecordatorioEmail.Equals(true))
                    {
                        try
                        {
                            Consultas consulta = new Consultas();
                            String txtSQL1 = " SELECT email FROM contacto where id_contacto=  '" + ParaQuien + "'" ;
                            var resul = consulta.ObtenerDataTable(txtSQL1, "text");
                            String wdato = resul.Rows[0].ItemArray[0].ToString();

                            enviarPorEmail(wdato.ToString(), "Envío módulo de tareas", item.Text + " Tarea Pendiente Fondo Emprender " + NomTarea + " " + Descripcion);
                        }
                        catch (Exception ex) {
                            errorMessageDetail = ex.Message;
                        }
                    }
                }
            }

            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.opener.location.href = window.opener.location.href; window.close();</script>");
        }

        /// <summary>
        /// Handles the Click event of the C_Cerrar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void C_Cerrar_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Recoger las sesiones
        /// </summary>
        private void recogeSession()
        {
            try
            {
                try
                {
                    codProyecto = HttpContext.Current.Session["EvalCodProyectoPOst"].ToString();
                }
                catch (Exception)
                {
                    codProyecto = "";
                }

                if (String.IsNullOrEmpty(codProyecto))
                    codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();

                try
                {
                    CodUsuario = Int32.Parse(HttpContext.Current.Session["EvalCodUsuario"].ToString());
                }
                catch (FormatException)
                {
                    CodUsuario = usuario.IdContacto;
                }
                catch (Exception)
                {
                    CodUsuario = usuario.IdContacto;
                }
                try
                {
                    Accion = HttpContext.Current.Session["EvalAccion"].ToString();
                }
                catch (Exception)
                {
                    Accion = "Adicionar";
                }
                try
                {
                    codGrupo = usuario.CodGrupo;
                }
                catch (Exception)
                {
                    codGrupo = -1;
                }
                try
                {
                    tabEval = HttpContext.Current.Session["tabEval"].ToString();
                }
                catch (Exception)
                {
                    tabEval = "";
                }
                try
                {
                    CONST_PostIt = Int32.Parse(HttpContext.Current.Session["EvalConsPOST"].ToString());
                }
                catch (FormatException)
                {
                    CONST_PostIt = Constantes.CONST_PostIt;
                }
                catch (Exception)
                {
                    CONST_PostIt = Constantes.CONST_PostIt;
                }
                try
                {
                    txtCampo = HttpContext.Current.Session["Campo"].ToString();
                }
                catch (Exception)
                {
                    txtCampo = "nulo";
                }
            }
            catch (Exception)
            {
                ClientScriptManager cm = this.ClientScript;
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.close();</script>");
            }
        }

        /// <summary>
        /// Queryables this instance.
        /// </summary>
        private void Queryable()
        {
            txtSQL = "";
            switch (codGrupo)
            {
                case Constantes.CONST_GerenteInterventor:
                    txtSQL = @"SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, R.Nombre Rol
                                FROM Contacto,Empresa,EmpresaInterventor P, Rol R
                                where id_empresa=codempresa and Id_Contacto = CodContacto and Id_Rol=Rol and
                                P.inactivo = 0 and id_contacto<>" + CodUsuario + " and codproyecto=" + codProyecto + @"
                                union
                                SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres + ' ' + Contacto.Apellidos AS Nombre, 'Coordinador Interventoria' AS Rol
                                FROM Interventor INNER JOIN EmpresaInterventor ON Interventor.CodContacto = EmpresaInterventor.CodContacto
                                INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa
                                INNER JOIN Contacto ON Interventor.CodCoordinador = Contacto.Id_Contacto
                                WHERE (Empresa.codproyecto = " + codProyecto + ")";
                    break;
                case Constantes.CONST_CoordinadorInterventor:
                    txtSQL = @"SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, R.Nombre Rol
                                FROM Contacto,Empresa,EmpresaInterventor P, Rol R
                                where id_empresa=codempresa and Id_Contacto = CodContacto and Id_Rol=Rol and
                                P.inactivo = 0 and id_contacto<>" + CodUsuario + " and codproyecto=" + codProyecto + @"
                                union
                                SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, 'Emprendedor' Rol
                                FROM Contacto,Empresa,EmpresaContacto P
                                where id_empresa=codempresa and Id_Contacto = CodContacto and
                                id_contacto<>" + CodUsuario + " and codproyecto=" + codProyecto + "";
                    break;
                case Constantes.CONST_Interventor:
                    txtSQL = @"SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, 'Emprendedor' Rol
                                FROM Contacto,Empresa,EmpresaContacto P
                                where id_empresa=codempresa and Id_Contacto = CodContacto and
                                id_contacto<>" + CodUsuario + " and codproyecto=" + codProyecto + @"
                                UNION
                                SELECT DISTINCT Contacto.Id_Contacto, Contacto.Nombres + ' ' + Contacto.Apellidos AS Nombre, 'Coordinador Interventoria' AS Rol
                                FROM Interventor INNER JOIN EmpresaInterventor ON Interventor.CodContacto = EmpresaInterventor.CodContacto
                                INNER JOIN Empresa ON EmpresaInterventor.CodEmpresa = Empresa.id_empresa 
                                INNER JOIN Contacto ON Interventor.CodCoordinador = Contacto.Id_Contacto
                                WHERE (Empresa.codproyecto = " + codProyecto + ")";
                    break;
                case Constantes.CONST_Emprendedor:
                    txtSQL = @"SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, R.Nombre Rol
                                FROM Contacto,Empresa,EmpresaInterventor P, Rol R
                                where id_empresa=codempresa and Id_Contacto = CodContacto and Id_Rol=Rol and
                                P.inactivo = 0 and id_contacto<>" + CodUsuario + " and codproyecto=" + codProyecto + "";
                    break;
                default:
                    txtSQL = @"SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, R.Nombre Rol FROM Contacto,ProyectoContacto P, Rol R 
								where Id_Contacto = CodContacto and Id_Rol=CodRol and P.inactivo = 0 and id_contacto<>" + CodUsuario + " and codproyecto=" + codProyecto + "";
                    break;
            }
            if (!String.IsNullOrEmpty(txtSQL))
            {
                txtSQL = txtSQL + "" + " ORDER BY Nombre";
                try
                {
                    consulta = new Consultas();

                    _DataContactos = consulta.ObtenerDataTable(txtSQL, "text");
                    if (_DataContactos.Rows.Count == 0) {
                    txtSQL = string.Format(
                        "SELECT DISTINCT Id_Contacto, Nombres +' '+ Apellidos as Nombre, R.Nombre Rol FROM Contacto,ProyectoContacto P, Rol R " + 
					   	"where Id_Contacto = CodContacto and Id_Rol=CodRol and P.inactivo = 0 and id_contacto<>{0} and codproyecto={1}",CodUsuario,codProyecto);
                    _DataContactos.Load(new Clases.genericQueries().executeQueryReader(txtSQL));
                    }
                    Clases.genericQueries.recordToLog(txtSQL);
                }
                catch (NullReferenceException) { 
                    Clases.genericQueries.recordToLog(HttpContext.Current.Error.Message + HttpContext.Current.Error.StackTrace + HttpContext.Current.Error.InnerException!=null?HttpContext.Current.Error.InnerException.Message:string.Empty); 
                }
                catch (SqlException) { 
                    Clases.genericQueries.recordToLog(HttpContext.Current.Error.Message + HttpContext.Current.Error.StackTrace + HttpContext.Current.Error.InnerException != null ? HttpContext.Current.Error.InnerException.Message : string.Empty);
                }
            }
        }

        /// <summary>
        /// Crearlistems the item.
        /// </summary>
        /// <param name="_Conts">The conts.</param>
        private void CrearlistemItem(DataTable _Conts)
        {
            LB_Para.Items.Clear();
            try
            {
                for (int i = 0; i < _Conts.Rows.Count; i++)
                {
                    ListItem listItemFor = new ListItem();
                    listItemFor.Text = "" + _Conts.Rows[i]["Nombre"].ToString() + "( " + _Conts.Rows[i]["Rol"].ToString() + " )";
                    listItemFor.Value = "" + _Conts.Rows[i]["Id_Contacto"].ToString();
                    LB_Para.Items.Add(listItemFor);
                }
            }
            catch (NullReferenceException ex) {
                errorMessageDetail = ex.Message;
            }  
        }        

        private void TareaUsuarioRepeticion(int last_id)
        {
            
            String sql = "INSERT INTO TareaUsuarioRepeticion (Fecha, CodTareaUsuario, Parametros) VALUES (getdate(),'" + last_id + "'," + "'CodProyecto=" + codProyecto + "&Campo=" + txtCampo + "'" + ")";

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {

                conn.Open();
                cmd.ExecuteReader();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La tarea " + NomTarea + "  ha sido agendada.')", true);
            }
            catch (SqlException ex) {
                errorMessageDetail = ex.Message;
            }
            finally 
            { 
                conn.Close(); 
                conn.Close(); 
            }
        }

        private void enviarPorEmail(String toTxt, String asuntoTxt, String mensajeTxt)
        {
            string SMTPUsuario = ConfigurationManager.AppSettings.Get("SMTPUsuario");
            string SMTPPassword = ConfigurationManager.AppSettings.Get("SMTPPassword");
            string To;
            string Subject;
            string Body;

            MailMessage mail;

            if (!(toTxt.Trim() == ""))
            {
                To = toTxt;
                Subject = asuntoTxt;
                Body = mensajeTxt;

                mail = new MailMessage();
                mail.To.Add(new MailAddress(To));
                mail.From = new MailAddress(usuario.Email);
                mail.Subject = Subject;
                mail.Body = Body;
                mail.IsBodyHtml = false;

                SmtpClient client = new SmtpClient("smtpcorp.com", 25);
                using (client)
                {
                    client.Credentials = new System.Net.NetworkCredential(SMTPUsuario, SMTPPassword);
                    client.EnableSsl = true;
                    client.Send(mail);
                }
                System.Windows.Forms.MessageBox.Show("Mensaje enviado", "Correcto", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }
    }
}