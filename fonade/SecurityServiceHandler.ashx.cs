using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System;
using System.Security.Cryptography;
using System.Text;



namespace Gensignservice
{


    /// <summary>
    /// Summary description for SecurityServiceHandler
    /// </summary>
    public class SecurityServiceHandler : IHttpHandler
    {
        public static bool checkToken(string a, string rand, string token)
        {
            DateTime now = DateTime.Now;
            var salt = "wilfer";
            var contents = salt + a;
            var h = Hash(contents + now.ToString("yyyy-MM-dd") + rand);
            return (h == token);
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
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            //context.Response.Write("Hello World");

            var token = context.Request.Form.Get("token"); // "D31B08530D1140C45C60F33149088EC75E3173FE";
            var a = context.Request.Form.Get("a");
            var rand = context.Request.Form.Get("rand");
            if (checkToken(a, rand, token))
            {
                context.Response.Write("100,Token looks OK");
            }
            else
            {
                context.Response.Write("500,Token looks BAD.");
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