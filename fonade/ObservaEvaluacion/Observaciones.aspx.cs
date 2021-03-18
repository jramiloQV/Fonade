using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Datos;
using System.Net.Mail;
using System.Configuration;

namespace Fonade.ObservaEvaluacion
{
    public partial class Observaciones : Negocio.Base_Page //System.Web.UI.Page
    {
        Consultas consulta = new Consultas();

        #region Eventoos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CargarCombo();
                CargarComboonvocatorias();
            }
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            var mensaje = string.Empty;
            var resp = Validar();

            switch(resp)
            {
                case 0:
                    mensaje = "LA OBSERVACIÓN HA SIDO ENVIADA CON EXITO";
                    break;
                case 1:
                    mensaje = "LO SENTIMOS, PERO PARA ESTE PLAN DE NEGOCIOS YA SE RECIBIO OBSERVACION!";
                    break;
                case 2:
                    mensaje = "EL PROYECTO INGRESADO NO ESTA REGISTRADO O NO PERTENECE A LA CONVOCATORIA SELECCIONADA";
                        break;
                case 3:
                    mensaje = "EL PROYECTO INGRESADO NO ESTA REGISTRADO O NO PERTENECE A LA CONVOCATORIA SELECCIONADA";
                    break;
                case 4:
                    mensaje = "LA OBSERVACIÓN SE REGISTRÓ, PERO HUBO UN ERROR EN EL ENVIO DE NOTIFICACIÓN POR CORREO AL ENCARGADO.  COMUNIQUESE CON EL ADMINISTRADOR.";
                    break;
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "')", true);
        }
        #endregion


        #region Meodos
        private void CargarCombo()
        {
            ddlPerfil.Items.Insert(0, new ListItem("-- Seleccione --", "0"));
            ddlPerfil.Items.Insert(1, new ListItem("Asesor", "1"));
            ddlPerfil.Items.Insert(2, new ListItem("Emprendedor", "2"));
            ddlPerfil.Items.Insert(3, new ListItem("Jefe de Unidad", "3"));
        }

        private void CargarComboonvocatorias()
        {
            var dt = new DataTable();
            var conn = ConfigurationManager.ConnectionStrings["SubComponent"].ConnectionString;
            var query = "select Descripcion, ' - ', valor from LovObjetoSE where NomLovObjetose='Convocatorias'";
            var sqlconect = new SqlConnection(conn);

            var cmd = new SqlCommand(query, sqlconect);

            sqlconect.Open();

            var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            sqlconect.Close();
            da.Dispose();

            ddlConvocatorias.DataSource = dt;
            ddlConvocatorias.DataTextField = "Descripcion";
            ddlConvocatorias.DataValueField = "valor";
            ddlConvocatorias.DataBind();
            ddlConvocatorias.Items.Insert(0, new ListItem("Seleccione", "0"));

        }

        private int Validar()
        {
            int resp = 0;
            var txtSql = string.Empty;
            var nomProyecto = string.Empty;
            var nomConvoca = string.Empty;

            txtSql = "select CodProyecto from ConvocatoriaProyecto Where Codproyecto=" + txtNoPlanNegocio.Text + " and CodConvocatoria=" + ddlConvocatorias.SelectedValue;
            var dt = consulta.ObtenerDataTable(txtSql, "text");

            if(dt.Rows.Count == 0)
            {
                resp = 3;
            }
            else
            {
                txtSql = "SELECT NomProyecto FROM Proyecto P WHERE P.Id_Proyecto=" + txtNoPlanNegocio.Text;

                dt = consulta.ObtenerDataTable(txtSql, "text");

                if (dt.Rows.Count > 0)
                {
                    nomProyecto = dt.Rows[0].ItemArray[0].ToString().ToUpper();
                }

                if (nomProyecto != null)
                {
                    txtSql = "SELECT NomConvocatoria, id_convocatoria FROM Convocatoria where id_convocatoria = " + ddlConvocatorias.SelectedValue;
                    dt = consulta.ObtenerDataTable(txtSql, "text");

                    if (dt.Rows.Count > 0)
                    {
                        nomConvoca = dt.Rows[0].ItemArray[0].ToString();
                    }

                    txtSql = "SELECT count(Id_ObservacionesEvaluacion) FROM ObservacionesEvaluacion WHERE CodProyecto=" + txtNoPlanNegocio.Text + " and nombreconvocatoria ='" + nomConvoca + "'";
                    dt = consulta.ObtenerDataTable(txtSql, "text");

                    if (int.Parse(dt.Rows[0].ItemArray[0].ToString()) == 0)
                    {
                        if (dt.Rows[0].ItemArray[0].ToString() == "0")
                        {
                            txtSql = "SELECT NomConvocatoria,id_convocatoria  FROM Convocatoria where id_convocatoria = " + ddlConvocatorias.SelectedValue;
                            dt = consulta.ObtenerDataTable(txtSql, "text");

                            if (dt.Rows.Count > 0)
                            {
                                string Comentarios = txtComentarios.Text.Replace("'", "").Replace("\"","");


                                txtSql = "INSERT INTO ObservacionesEvaluacion (NomObservacionesEvaluacion, codconvocatoria, CodProyecto, NombreConvocatoria, Nombres, Email, Perfil, Comentarios, Fecha) ";
                                txtSql += " VALUES('" + txtNoPlanNegocio.Text + " - " 
                                    + dt.Rows[0].ItemArray[0].ToString() + "', " 
                                    + dt.Rows[0].ItemArray[1].ToString() + " , " 
                                    + txtNoPlanNegocio.Text + ", '" 
                                    + ddlConvocatorias.SelectedItem + "', '" 
                                    + txtNombres.Text + "', '" 
                                    + txtEmail.Text + "', '" 
                                    + ddlPerfil.SelectedValue + "', '" 
                                    + Comentarios + "', Getdate())";
                                ejecutaReader(txtSql, 2);

                                int coddConvocatoria = Convert.ToInt32(dt.Rows[0].ItemArray[1].ToString());

                                if (EnviarCorreo(dt, coddConvocatoria))
                                {
                                    resp = 0;
                                }
                                else
                                {
                                    resp = 4;
                                }

                                LimpiarForm();
                            }
                        }
                    }
                    else
                    {
                        resp = 1;
                    }
                }
            }

            return resp;

        }

        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private bool EnviarCorreo(DataTable dt, int _codConvocatoria)
        {
            var resp = false;
            //var emailDestinatario = ConfigurationManager.AppSettings["mailObservaciones"].ToString();
            //var emailDestinatarioCC = ConfigurationManager.AppSettings["mailCCObservaciones"].ToString();

            string emailDestinatario = "";
            string emailDestinatarioCC = "";

            int? codOperador;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                codOperador = (from e in db.Convocatoria
                               where e.Id_Convocatoria == _codConvocatoria
                               select e.codOperador).FirstOrDefault();
            }

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena)) {
                emailDestinatario = (from e in db.ObservacionesEvaluacionCorreo
                                     where e.codOperador == codOperador
                                     select e.correo_principal).FirstOrDefault();

                emailDestinatarioCC = (from e in db.ObservacionesEvaluacionCorreo
                                     where e.codOperador == codOperador
                                     select e.correo_secundario).FirstOrDefault();
            }

            var emailRemitente = ConfigurationManager.AppSettings["Email"].ToString();
            var bodyTemplate = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='http://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
                "<title></title><style type='text/css'>body {margin: 0; padding: 0; min-width: 100%!important;} .content {width: 100%; max-width: 600px; font-family: arial; font-size:}  </style></head><body yahoo bgcolor='#f6f8f1'><table width='100%' bgcolor='#f6f8f1' border='0' cellpadding='0' cellspacing='0'><tr><td>" +
                "<table class='content' align='center' cellpadding='0' cellspacing='0' border='0'><tr><td>" +
                "<br>Buen día, <br><br>Se há generado un nuevo comentario:<br><b>Nombres:</b> " + txtNombres.Text + "<br>" +
                "<b>Email:</b> " + txtEmail.Text + "<br>" +
                "<b>Rol:</b> " + ddlPerfil.SelectedItem.ToString() + "<br>" +
                "<b>ID Proyecto:</b> " + txtNoPlanNegocio.Text + "<br>" +
                "<b>Convocatoria:</b> " + ddlConvocatorias.SelectedItem.ToString() + "<br>" +
                "<b>Comentarios: </b><br><i>" + txtComentarios.Text + "</i><br>" +
                "<br> Cordialmente, <br> SENA - Fondo Emprender</td></tr>" +
                "</table></td></tr>" +
                "</table>" +
                "</body>" +
                "</html>";


            //<b>Dirección:</b> [[direccion]]<br><b>Ciudad:</b> [[ciudad]]<br><b>Pais</b> [[pais]]<br><b>Número de teléfono:</b> [[telefono]]<br><b>Número de fax:</b> [[fax]]<br>

            try
            {
                MailMessage mail;
                mail = new MailMessage();
                mail.To.Add(new MailAddress(emailDestinatario));
                mail.To.Add(new MailAddress(emailDestinatarioCC));
                mail.From = new MailAddress(emailRemitente);
                mail.Subject = "Nuevo comentario convocatoria";
                mail.Body = bodyTemplate;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;

                var smtp = ConfigurationManager.AppSettings.Get("SMTP");
                var port = int.Parse(ConfigurationManager.AppSettings.Get("SMTP_UsedPort"));
                SmtpClient client = new SmtpClient(smtp, port);
                using (client)
                {
                    var usuarioEmail = ConfigurationManager.AppSettings.Get("SMTPUsuario");
                    var passwordEmail = ConfigurationManager.AppSettings.Get("SMTPPassword");
                    client.Credentials = new System.Net.NetworkCredential(usuarioEmail, passwordEmail);
                    client.EnableSsl = false;
                    client.Send(mail);
                }
                resp = true;
            }
            catch(Exception ex)
            {
                resp = false;
            }

            return resp;
        }

        private void LimpiarForm()
        {
            txtComentarios.Text = "";
            txtEmail.Text = "";
            txtNombres.Text = "";
            txtNoPlanNegocio.Text = "";
            ddlConvocatorias.SelectedValue = "0";
            ddlPerfil.SelectedValue = "0";
        }
        #endregion
        

        
    }
}