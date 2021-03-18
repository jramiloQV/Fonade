using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    /// <summary>
    /// Clase que presenta la relación de los emprendedores y su perfil en un plan de negocio
    /// </summary>
    [Serializable]
    public class EquipoTrabajo
    {
        /// <summary>
        /// Identificador primario
        /// </summary>
        public int? IdEmprendedorPerfil { get; set; }

        /// <summary>
        /// Identificador primario del emprendedor
        /// </summary>
        public int IdContacto { get; set; }

        /// <summary>
        /// Nombre del emprendedor
        /// </summary>
        public string NombreEmprendedor { get; set; }

        /// <summary>
        /// Perfil del emprendedor en el plan de negocio
        /// </summary>
        public string Perfil { get; set; }

        /// <summary>
        /// Rol del emprendedor en el plan de negocio
        /// </summary>
        public string Rol { get; set; }
    }
}
