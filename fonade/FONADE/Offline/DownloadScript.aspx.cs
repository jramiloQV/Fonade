using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Fonade.FONADE.Offline
{
    public partial class DownloadScript : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.Clear();

            string strFileName = Request.QueryString["fileName"].ToString();
            string strFile = strFileName.Substring(strFileName.Length, strFileName.Length - strFileName.LastIndexOf( @"\" ));
            string strFileType = Request.QueryString["type"].ToString();
            if (strFileType == "")
            {
                strFileType = "application/download";
            }

            var files = Directory.GetFiles(Server.MapPath("E:\\ftproot\\sales"));


        }
    }
}