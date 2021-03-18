using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    public class GrillaDocumentos
    {
        #region Propiedades Documentos evaluación y Anexos

        /// <summary>
        /// Identificador primario del documento
        /// </summary>
        public int Id_documento { get; set; }

        /// <summary>
        /// Nombre del documento
        /// </summary>
        public string NombreDocumento { get; set; }

        /// <summary>
        /// Fecha ingreso del documento al sistema
        /// </summary>
        public string Fecha { get; set; }

        /// <summary>
        /// URL del archivo en el servidor
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// Nombre físico del documento
        /// </summary>
        public string NombreDocumentoFormato { get; set; }

        /// <summary>
        /// Icono del documento a presentar en la grilla
        /// </summary>
        public string Icono { get; set; }

        /// <summary>
        /// Código del formato del documento
        /// </summary>
        public int CodigoDocumentoFormato { get; set; }

        /// <summary>
        /// Tab al que se asocia el documento
        /// </summary>
        public string Tab { get; set; }

        #endregion

        #region Propiedades Documento de Acreditación
        
        /// <summary>
        /// Ruta física de un archivo de acreditación
        /// </summary>
        public string Ruta { get; set; }

        /// <summary>
        /// Descripción título obtenido asociado a un archivo de acreditación
        /// </summary>
        public string TituloObtenido { get; set; }

        /// <summary>
        /// Descripción nivel de estudio asociado a un archivo de acreditación
        /// </summary>
        public string NomNivelEstudio { get; set; }

        /// <summary>
        /// Nombres de la persona asociada al archivo de acreditación
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Descripción tipo de un archivo de acreditación
        /// </summary>
        public string TipoArchivo { get; set; }

        /// <summary>
        /// Descripción detallada tipo de un archivo de acreditación
        /// </summary>
        public string TipoArchivoDescripcion { get; set; }

        /// <summary>
        /// Descripción detallada de un archivo de acreditación
        /// </summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Año del título de un documento de acreditación
        /// </summary>
        public int Aniotitulo { get; set; }

        #endregion

        #region Propiedades Documento Consultado

        /// <summary>
        /// Comentario del archivo
        /// </summary>
        public string Comentario { get; set; }

        /// <summary>
        /// Extensión del archivo cargado
        /// </summary>
        public string Extension { get; set; }

        #endregion
        public Boolean Bloqueado { get; set; }
    }
}
