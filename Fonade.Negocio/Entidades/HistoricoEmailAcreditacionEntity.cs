using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    public class HistoricoEmailAcreditacionEntity
    {
        public int Id_HistoricoEmailAcreditacion { get; set; }
        public int CodProyecto { get; set; }
        public int CodConvocatoria { get; set; }
        public int CodContacto { get; set; }
        public string Email { get; set; }
        public DateTime Fecha { get; set; }
        public int? CodContactoEnvia { get; set; }
        public int? CodEstadoAcreditacion { get; set; }
    }
}
