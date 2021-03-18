using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace Fonade.Negocio.Mensajes
{
    public class Mensajes
    {
        /// <summary>
        /// GetMensaje
        /// </summary>
        /// <param name="idMensaje">idMensaje</param>
        /// <returns>string</returns>
        public static string GetMensaje(int idMensaje)
        {
            DataSet ds = new DataSet();
            string rutaXML = HttpContext.Current.Server.MapPath(@"\DataMensajes\DataMensajes.xml");
            ds.ReadXml(rutaXML);
            return ds.Tables[0].Select("IdMensaje= '" + idMensaje.ToString()+ "'")[0]["Mensaje"].ToString();
        }
    }
}
