using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    /// <summary>
    /// Clase que representa los requerimientos de infraestructura necesarios en un plan de negocio
    /// </summary>
    [Serializable]
    public class RequerimientosNeg
    {
        #region Variables

        /// <summary>
        /// Identificador primario de la infraestructura
        /// </summary>
        public int IdProyectoInfraestructura { get; set; }

        /// <summary>
        /// Código del tipo de infraestructura
        /// </summary>
        public byte CodTipoInfraestructura { get; set; }

        /// <summary>
        /// Identificador primario de la fuente
        /// </summary>
        public int IdFuente { get; set; }

        /// <summary>
        /// Nombre de la infraestructura
        /// </summary>
        public string NomInfraestructura { get; set; }

        /// <summary>
        /// Valor unitario
        /// </summary>
        public Nullable<decimal> ValorUnidad { get; set; }

        /// <summary>
        /// Valor unitario para presentar en formato
        /// </summary>
        public string ValorUnidadCadena { get; set; }

        /// <summary>
        /// Cantidad
        /// </summary>
        public Nullable<double> Cantidad { get; set; }

        /// <summary>
        /// Descripción requisitos técnicos de la infraestructura
        /// </summary>
        public string RequisitosTecnicos { get; set; }

        /// <summary>
        /// Descripción fuente de finanaciación
        /// </summary>
        public string FuenteFinanciacion { get; set; }

        #endregion

    }
}
