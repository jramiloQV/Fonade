using Fonade.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Fonade.Negocio.Entidades;
using System.Data;

namespace Fonade.Negocio.Proyecto
{

    public class ProyectoMercadoProyeccionVentasNegocio
    {               
        Consultas consultas = new Consultas();

        /// <summary>
        /// Obtener proyecciones de ventas
        /// </summary>
        /// <param name="codProyecto">Codido de proyecto</param>
        /// <returns> Listado de proyectos de ventas </returns>
        public List<ProyectoMercadoProyeccionVenta> GetProyeccionesVenta(int codProyecto)
        {
            var proyectoMercadoVent = (from pmv in consultas.Db.ProyectoMercadoProyeccionVentas where pmv.CodProyecto == codProyecto select pmv).ToList();
            return proyectoMercadoVent;
        }
    }
}
