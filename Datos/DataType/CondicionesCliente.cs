using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    [Serializable]
    /// <summary>
    /// Clase que presenta en la grilla las condiciones comerciales de los clientes
    /// en un plan de proyecto
    /// </summary>
    public class CondicionesCliente
    {
        #region Propiedades

        /// <summary>
        /// Identificador primario del cliente
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string Cliente{ get; set; }

        /// <summary>
        /// Frecuencia de compra
        /// </summary>
        public string FrecuenciaCompra{ get; set; }

        /// <summary>
        /// Características de la compra
        /// </summary>
        public string CaracteristicasCompra{ get; set; }

        /// <summary>
        /// Sitio de compra 
        /// </summary>
        public string SitioCompra{ get; set; }

        /// <summary>
        /// Forma de pago
        /// </summary>
        public string FormaPago{ get; set; }

        /// <summary>
        /// Precio
        /// </summary>
        public decimal Precio{ get; set; }

        /// <summary>
        /// Precio en formato cadena
        /// </summary>
        public string PrecioCadena { get; set; }

        /// <summary>
        /// Requisitos post-venta
        /// </summary>
        public string RequisitosPostVenta{ get; set; }

        /// <summary>
        /// Garantías
        /// </summary>
        public string Garantias{ get; set; }

        /// <summary>
        /// Margen de comercialización
        /// </summary>
        public string Margen { get; set; }

        #endregion
    }
}
