﻿using System;
using System.Web;
using System.Text;
using System.Security.Cryptography;

 namespace Fonade.FONADE.interventoria
{
    /// <summary>
    /// 31/DIC/2014 WAFS 
    /// Permite grabar el XML firmado de pagos en una variable de sesion
    /// </summary>
    public class PagosAp : IHttpHandler, System.Web.SessionState.IRequiresSessionState, System.Web.SessionState.IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string Firmado = "";
            if (context.Request.Form.Get("APCif") != null)
            {
                //si es sal + sha1 igual entonces session data = ok
                //sino retorna 404
                //retorno de pagosactividadfiduciaria
                DateTime now = DateTime.Now;
                var salt = "wilfer";
                var contents = salt ;
                var h = Hash(contents + now.ToString("yyyy-MM-dd") );

                if (context.Request.Form.Get("APCif") == h)
                {
                    context.Session["APCif"] = 1;
                    context.Response.Write("200, OK");
                }else{
                    context.Session["APCif"] = 0;
                    context.Response.Write("400, Server Challenge Failed.");
                }
                return;
            }
            Firmado = context.Request["XMLCif"];


            //Firmado = Encoding.UTF8.GetString(Convert.FromBase64String(context.Request["XMLCif"]));
            //Firmado = Encoding.UTF8.GetString(Convert.FromBase64String(context.Request.Params.Get("XMLCif")));
            context.Session["Firma"] = Firmado;
        }

        static string Hash(string input)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (byte b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}