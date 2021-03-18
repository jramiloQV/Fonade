using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ConvenioModel
    {
        public int idConvenio { get; set;}
        public string NomConveio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime Fechafin { get; set; }
        public string Descripcion { get; set; }
        public int codContactoFiduciaria { get; set; }
    }
}
