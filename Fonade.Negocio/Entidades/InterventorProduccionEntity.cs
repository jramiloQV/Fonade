using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Entidades
{
    public class InterventorProduccionEntity
    {
        public int Id_produccion { get; set; }

        public int CodProyecto { get; set; }

        public String NomProducto { get; set; }

        public int CodProducto { get; set; }

        public int Mes { get; set; }

        public int Tipo { get; set; }

        public Double Valor { get; set; }
    }
}
