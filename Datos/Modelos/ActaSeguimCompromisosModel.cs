using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimCompromisosModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaModificado { get; set; }
        public string compromiso { get; set; }
        public DateTime fechaPropuestaEjec { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }
        public DateTime? fechaCumpliCompromiso { get ; set; }
    }

    public class ActaSeguimCompromisosHistoricoModel
    {
        public Int64 id { get; set; }
        public Int64 idCompromiso { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaModificado { get; set; }
        public string compromiso { get; set; }
        public DateTime fechaPropuestaEjec { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }
        public DateTime? fechaCumpliCompromiso { get; set; }
    }
}
