using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador
{
    public class OperadorModel
    {
        public int idOperador { get; set; }
        public string NombreOperador { get; set; }
        public string TelefonoOperador { get; set; }
        public string DireccionOperador { get; set; }
        public string NitOperador { get; set; }
        public string EmailOperador { get; set; }
        public string NombreRepresentante { get; set; }
        public string TelefonoRepresentante { get; set; }
        public string EmailRepresentante { get;  set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string EmailObservacionAcreditacion { get; set; }
        public string Rutalogo { get; set; }
    }
}
