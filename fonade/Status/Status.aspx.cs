using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Status
{
    public partial class Status : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
            var isValidDB = TestConnection();

            if (isValidDB)
                idStatus.Text = "200";
            else
                idStatus.Text = "400";
        }

        private bool TestConnection()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                int oldTimeOut = db.CommandTimeout;

                try
                {
                    db.CommandTimeout = 1;
                    
                    return db.DatabaseExists();
                }
                catch
                {
                    return false;
                }
                finally
                {
                    db.CommandTimeout = oldTimeOut;
                }
            }
        }


    }
}