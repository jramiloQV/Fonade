using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fonade.Status
{
    [Serializable]
    public class LogSesionActiv
    {
        public string Usuario { get; set; }
        public string Posicion { get; set; }
    }
}