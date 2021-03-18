using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class DatosActaModelDTO
    {
        public int codProyecto { get; set; }
        public int codContacto { get; set; }
        public int codRol { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identificacion { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string nombreRol { get; set; }

    }

    public class InfoInterventorModelDTO
    {
        public int id { get; set; }
        public string nombreInterventor { get; set; }

        public string rutaLogo { get; set; }

    }
}
