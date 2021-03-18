using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class HistoricoObservacionEmpresaModel
    {
        public long id_HistoricoObserEmpresa { get; set; }
        public int codProyecto { get; set; }
        public int codEmpresa { get; set; }
        public string Observacion_Old { get; set; }
        public string Observacion_New { get; set; }
        public string motivoCambio { get; set; }
        public DateTime fechaCambio { get; set; }
        public int codContactoCambio { get; set; }
    }
}
