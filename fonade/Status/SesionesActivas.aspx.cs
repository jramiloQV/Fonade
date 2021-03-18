using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Status
{
    public partial class SesionesActivas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int currentNumberOfUsers = Global.CurrentNumberOfUsers;
            int totalNumberOfUsers = Global.TotalNumberOfUsers;
            DateTime fechaInicioServer = Global.FechaInicioServer;

            lblCurrentNumberOfUsers.Text = currentNumberOfUsers.ToString();
            lblTotalNumberOfUsers.Text = totalNumberOfUsers.ToString();
            lblFechaHoraServerStart.Text = fechaInicioServer.ToString();



            //lblSesiones.Text = Application["ActiveUsers"].ToString();

            //if (Convert.ToInt32(Application["ActiveUsers"].ToString()) > 
            //    Convert.ToInt32(Application["TopeMaximo"].ToString()))
            //{
            //    Application.Lock();
            //    Application["TopeMaximo"] = Application["ActiveUsers"];
            //    Application.UnLock();
            //}

            //lblTopeMaximo.Text = Application["TopeMaximo"].ToString();

            //var logSesions = (List<LogSesionActiv>)Application["Usuarios"];

            //var query = from p in logSesions.GroupBy(p => p.Usuario)
            //            select new
            //            {
            //                Count = p.Count(),
            //                p.First().Usuario,
            //            };

            //gvResultado.DataSource = query.OrderByDescending(x=>x.Count).ToList();
            //gvResultado.DataBind();
        }

       
    }
}