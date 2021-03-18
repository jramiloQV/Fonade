using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace Fonade.Clases
{
    /// <summary>
    /// Correo
    /// </summary>
    public class Correo
    {
        private string DeEmail { get; set; }
        private string DeNombre { get; set; }
        private string ParaEmail { get; set; }
        private string ParaNombre { get; set; }
        private string Asunto { get; set; }
        private string Cuerpo { get; set; }
        private string ConCopiaOculta { get; set; }

        private string SMTP = ConfigurationManager.AppSettings.Get("SMTP");
        private string SMTPUsuario = ConfigurationManager.AppSettings.Get("SMTPUsuario");
        private string SMTPPassword = ConfigurationManager.AppSettings.Get("SMTPPassword");
        private int SMTP_UsedPort = Int32.Parse(ConfigurationManager.AppSettings.Get("SMTP_UsedPort"));
        private String QuienEnvia = ConfigurationManager.AppSettings.Get("Email");

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="Correo"/> class.
        /// </summary>
        /// <param name="deEmail">email de quien envia.</param>
        /// <param name="deNombre">nombre de quien envia.</param>
        /// <param name="paraEmail">email destinatario .</param>
        /// <param name="paraNombre">nombre destinatario.</param>
        /// <param name="asunto">asunto.</param>
        /// <param name="cuerpo">cuerpo.</param>
        public Correo(string deEmail, string deNombre, string paraEmail, string paraNombre, string asunto, string cuerpo
                        , string conCopiaOculta = "")
        {
            this.DeEmail = deEmail;
            this.DeNombre = deNombre;
            this.ParaEmail = paraEmail;
            this.ParaNombre = paraNombre;
            this.Asunto = asunto;
            this.Cuerpo = cuerpo;
            this.ConCopiaOculta = conCopiaOculta;
        }

        /// <summary>
        /// Metodo que envia el correo
        /// </summary>
        public void Enviar()
        {
            var Emailtemplate = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory.Insert(
                    AppDomain.CurrentDomain.BaseDirectory.Length, "FONADE\\Plantillas\\WorkItem.html"));
            var strBody = string.Format(Emailtemplate.ReadToEnd(), this.Cuerpo, ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"]);
            Emailtemplate.Close(); Emailtemplate.Dispose(); Emailtemplate = null;
            
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress(this.QuienEnvia, this.DeNombre);
            correo.To.Add(new MailAddress(this.ParaEmail, this.ParaNombre));

            if (ConCopiaOculta!="")
            {
                MailAddress bcc = new MailAddress(ConCopiaOculta);
                correo.Bcc.Add(bcc);
            }            

            correo.Subject = this.Asunto;
            correo.Body = strBody.ReplaceWord("http://www.fondoemprender.com/logo", ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"]);
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.Normal;

            string smtpservidor = this.SMTP;
            string userSmtp = this.SMTPUsuario;
            string passwordSmtp = this.SMTPPassword;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = smtpservidor;
            
            smtp.Port = SMTP_UsedPort; //Puerto para envío de correos.
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(userSmtp, passwordSmtp);
            
            smtp.Credentials = credentials;
            try { smtp.Send(correo); }
            catch (Exception){               
                correo.From = new MailAddress("info@fondoemprender.com", this.DeNombre);                
            }
        }
    }
}