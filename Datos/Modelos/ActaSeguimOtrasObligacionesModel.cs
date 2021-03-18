using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.Modelos
{
    public class ActaSeguimOtrasObligacionesModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime FechaIngresado { get; set; }
        public string DescripInfoPlataforma { get; set; }
        public string DescripTiempoEmprendedor { get; set; }
        public string DescripAcomAsesoria { get; set; }

    }

    public class ActaSeguimOtrasObligInfoPlataformaModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime FechaIngresado { get; set; }
        public string valoracion { get; set; }
        public string observacion { get; set; }
        
    }

    public class ActaSeguimOtrasObligTiempoEmpModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime FechaIngresado { get; set; }
        public string valoracion { get; set; }
        public string observacion { get; set; }
    }

    public class ActaSeguimOtrasObligAcomAsesoriaModel
    {
        public Int64 id { get; set; }
        public int codProyecto { get; set; }
        public int codConvocatoria { get; set; }
        public int numActa { get; set; }
        public int visita { get; set; }
        public DateTime FechaIngresado { get; set; }
        public string valoracion { get; set; }
        public string observacion { get; set; }
    }
}
