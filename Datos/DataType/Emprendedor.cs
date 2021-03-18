using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
   public class Emprendedor
    {

        public int IdContacto { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public string Rol { get; set; }

        public DateTime? FechaNac { get; set; }

        public string LugarNac { get; set; }        
    }
}
