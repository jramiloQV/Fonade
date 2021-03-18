using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    /// <summary>
    /// Clase que representa los procesos de un producto en un plan de negocio
    /// </summary>
    [Serializable]
    public class ProductoProceso
    {
        /// <summary>
        /// Identificador primario del producto
        /// </summary>
        public int Id_Producto { get; set; }

        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string NomProducto { get; set; }

        /// <summary>
        /// Identificador primario del proceso
        /// </summary>
        public int? Id_Proceso { get; set; }

        /// <summary>
        /// Descricpión del proceso
        /// </summary>
        public string DescProceso { get; set; }

        /// <summary>
        /// Unidad de medida
        /// </summary>
        public string Unidad { get; set; }
    }
}
