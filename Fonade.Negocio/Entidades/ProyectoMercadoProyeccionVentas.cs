using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    /// <summary> Clase de tipo Entidad que hace referecia a la tabla
    /// ProyectoMercadoProyeccionVentas que no esta en el modelo por no tener un llave primaria
    /// </summary>
    public class ProyectoMercadoProyeccionVentas
    {
        public int CodProyecto { get; set; }

        public DateTime FechaArranque { get; set; }

        public short CodPeriodo { get; set; }

        public short TiempoProyeccion { get; set; }

        public String MetodoProyeccion { get; set; }

        public String PoliticaCartera { get; set; }

        public String CostoVenta { get; set; }

        public String justificacion { get; set; }
    }
}
