using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimInfoPagosModel
    {
        public Int64 id { get; set; }
        public int idPagoActividad { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaModificado { get; set; }
        public string codigoPago { get; set; }
        public string Actividad { get; set; }
        public decimal Valor { get; set; }
        public string Concepto { get; set; }
        public string verificoDocumentos { get; set; }
        public string verificoActivosEstado { get; set; }
        public string Observacion { get; set; }
    }

    public class ActaSeguimInventarioContratoModel
    {
        public Int64 id { get; set; }       
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaModificado { get; set; }
        public string descripcionRecursos { get; set; }        
        public decimal valorActivos { get; set; }
        public DateTime fechaCargaAnexo { get; set; }        
    }

    public class ActaSeguimAporteEmprendedorModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime fechaModificado { get; set; }
        public string descripcion { get; set; }
        public string metaEmprendedor { get; set; }
    }
}
