using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class HistoricoIndicadorGenericoModel
    {
        public long idHistoricoIndicadorGen { get; set; }
        public int id_IndicadorGenerico { get; set; }
        public int CodEmpresa { get; set; }
        public string NombreIndicador { get; set; }
        public string Descripcion { get; set; }
        public int Numerador_Old { get; set; }
        public int Denominador_Old { get; set; }
        public string Evaluacion_Old { get; set; }
        public string Observacion_Old { get; set; }
        public int Numerador_New { get; set; }
        public int Denominador_New { get; set; }
        public string Evaluacion_New { get; set; }
        public string Observacion_New { get; set; }
        public string MotivoCambio { get; set; }
        public DateTime FechaCambio { get; set; }
        public int CodContactoCambio { get; set; }
    }
}
