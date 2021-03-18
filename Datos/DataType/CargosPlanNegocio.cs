using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    /// <summary>
    /// Clase que representa los cargos en la grilla de un plan de negocio
    /// </summary>
    [Serializable]
    public class CargosPlanNegocio
    {
        /// <summary>
        /// Identificador primario del Cargo
        /// </summary>
        public int Id_Cargo {get; set;}

        /// <summary>
        /// Nombre del cargo
        /// </summary>
        public string Cargo {get; set;}


        /// <summary>
        /// Valor otros gastos formateado
        /// </summary>
        public string OtrosGastosCadena { get; set; }

        /// <summary>
        /// Unidad de tiempo
        /// </summary>
        public string UnidadTiempo { get; set; }

        /// <summary>
        /// Tiempo de vinculación
        /// </summary>
        public System.Nullable<int> TiempoVinculacion { get; set; }


        /// <summary>
        /// Valor remuneración formateado
        /// </summary>
        public string ValorRemunCadena { get; set; }

        
        /// <summary>
        /// Valor remuneración primer año formateado
        /// </summary>
        public string ValorRemunPrimerAnioCadena { get; set; }

        /// <summary>
        /// Valor con Prestaciones formateado
        /// </summary>
        public string ValorPrestacionesCadena { get; set; }


        /// <summary>
        /// Valor fondo emprender formateado
        /// </summary>
        public string ValorFondoEmprenderCadena { get; set; }

        /// <summary>
        /// Valor aportes emprendedor formateado
        /// </summary>
        public string AportesEmprendedorCadena { get; set; }


        /// <summary>
        /// Valor ingresos ventas formateado
        /// </summary>
        public string IngresosVentasCadena { get; set; }
    }
}
