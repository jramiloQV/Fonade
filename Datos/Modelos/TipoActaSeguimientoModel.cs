using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class TipoActaSeguimientoModel
    {
        public int idActa { get; set; }
        public string Tipo { get; set; }
        public string Codigo { get; set; }
        public string Version { get; set; }
        public string Vigencia { get; set; }

    }
}
