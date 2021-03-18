using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    public class ReportePuntaje : REP_ConsultarReportePuntajeResult
    {
        /// <summary>
        /// Totales y resultados del puntaje
        /// </summary>
        public int TotalProtagonista { get; set; }
        public int TotalOportunidad { get; set; }
        public int TotalSolucion { get; set; }
        public int TotalDesarrollo { get; set; }
        public int TotalFuturoNegocio { get; set; }
        public int TotalRiesgo { get; set; }
        public string VlrResumenLetras { get; set; }
        public int VlrResumenNumeros { get; set; }
        public int PuntajeTotal { get; set; }

    }
}
