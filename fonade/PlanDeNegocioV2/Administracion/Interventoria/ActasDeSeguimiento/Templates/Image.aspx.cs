using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Templates
{
    public partial class Image : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["FileName"] != null)

            {

                try

                {

                    // Read the file and convert it to Byte Array

                    //string filePath = "C:\\images\\";
                    string[] filename = Request.QueryString["FileName"].Split('\\');

                    string filePath = baseDirectory + filename[0];                    

                    string contenttype = "image/" +

                    Path.GetExtension(filename[1].Replace(".", ""));

                    //string ruta = @"" + filePath + "/" + filename[1];

                    //////

                    string path;

                    path = System.IO.Path.GetDirectoryName
                  (System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                    path = string.Concat(baseDirectory + filename[0]+"\\"+ filename[1]); //"M:\\"

                    char[] charBuf = path.ToCharArray();
                    char[] charbuf2 = new char[path.Length - 6];

                    for (int i = 6; i < path.Length; i++)
                        charbuf2[i - 6] = charBuf[i];

                    string ruta = "";
                    //ruta = new string(charbuf2);
                    ruta = path;

                    //////

                    FileStream fs = new FileStream(ruta,FileMode.Open, FileAccess.Read);

                    BinaryReader br = new BinaryReader(fs);

                    Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                    br.Close();

                    fs.Close();



                    //Write the file to response Stream

                    Response.Buffer = true;

                    Response.Charset = "";

                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    Response.ContentType = contenttype;

                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);

                    Response.BinaryWrite(bytes);

                    Response.Flush();

                    Response.End();

                }

                catch (Exception ex)
                {

                }

            }
        }
        public string baseDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
    }
}