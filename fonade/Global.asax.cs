using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Net;
using Fonade.Error;
using Fonade.Account;
using Fonade.Status;

namespace Fonade
{
    public class Global : System.Web.HttpApplication
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ?
                    (FonadeUser)HttpContext.Current.Session["usuarioLogged"] :
                    (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set { }
        }

        private static int totalNumberOfUsers = 0;
        private static int currentNumberOfUsers = 0;
        private static DateTime fechaInicioServer = DateTime.Now;

        void Application_Start(object sender, EventArgs e)
        {
            //List<LogSesionActiv> logSesions = new List<LogSesionActiv>();
            //Application["ActiveUsers"] = 0;
            //Application["TopeMaximo"] = 0;

            ////Ver usuarios
            //Application["Usuarios"] = logSesions;            
        }

        void Application_End(object sender, EventArgs e)
        {
        }

        void Application_Error(object sender, EventArgs e)

        {
            //Code that runs when an unhandled error occurs
            Exception objErr = Server.GetLastError().GetBaseException();

            string url = Request.Url.ToString();

            string mensaje = objErr.Message.ToString();
            string data = objErr.Data.ToString();
            string stackTrace = objErr.StackTrace.ToString();
            string innerException = objErr.InnerException == null ? "" : objErr.InnerException.Message.ToString();

            // Log the error
            ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, Usuario.Email, Usuario.IdContacto.ToString());
        }

        void Session_Start(object sender, EventArgs e)
        {
            totalNumberOfUsers += 1;
            currentNumberOfUsers += 1;

            //Session.Timeout = 20;
            //Session["Start"] = DateTime.Now;

            //Application.Lock();
            //Application["ActiveUsers"] = Convert.ToInt32(Application["ActiveUsers"].ToString()) + 1;
            //Application.UnLock();

            ////Ver usuarios
            //LogSesionActiv logS = new LogSesionActiv() {                
            //    Posicion = Application["ActiveUsers"].ToString(),
            //    Usuario = Request.ServerVariables["REMOTE_ADDR"].ToString()
            //};

            //var logSesions = (List<LogSesionActiv>)Application["Usuarios"];
            //logSesions.Add(logS);

        }

        void Session_End(object sender, EventArgs e)
        {
            currentNumberOfUsers -= 1;

            //Application.Lock();
            //Application["ActiveUsers"] = Convert.ToInt32(Application["ActiveUsers"].ToString()) - 1;
            //Application.UnLock();

            //var logSesions = (List<LogSesionActiv>)Application["Usuarios"];
            //var delLog = logSesions.FirstOrDefault(x => x.Usuario == Request.ServerVariables["REMOTE_ADDR"].ToString());
            //logSesions.Remove(delLog);

            //Application.Lock();
            //Application["ActiveUsers"] = logSesions;
            //Application.UnLock();
        }

        public static int TotalNumberOfUsers
        {
            get
            {
                return totalNumberOfUsers;
            }
        }

        public static int CurrentNumberOfUsers
        {
            get
            {
                return currentNumberOfUsers;
            }
        }

        public static DateTime FechaInicioServer
        {
            get
            {
                return fechaInicioServer;
            }
        }
    }


}









