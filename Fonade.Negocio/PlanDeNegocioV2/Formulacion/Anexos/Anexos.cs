
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Datos.DataType;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.Anexos
{
    public static class Anexos
    {
        #region Variables
        
        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        /// <summary>
        /// Icono por defecto de las grillas
        /// </summary>
        static string iconoDefault
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["IconoDefault"];
            }
        }

        #endregion

        /// <summary>
        /// Consulta el listado de documentos anexos y de evaluación asociados a un proyecto y según el estado en
        /// el que se encuentre
        /// </summary>
        /// <param name="codEstado">Código estado del proyecto</param>
        /// <param name="codProyecto">Código del proyecto</param>
        /// <returns>Listado de documentos según filtro</returns>
        public static List<GrillaDocumentos> getDocumentos(int codEstado, int codProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                var consulta = (       from doc in db.Documentos
                               join unitab in db.Tabs
                               on doc.CodTab equals unitab.Id_Tab into tablaTmp
                               from tmp in tablaTmp.DefaultIfEmpty()
                               join formato in db.DocumentoFormatos
                               on doc.CodDocumentoFormato equals formato.Id_DocumentoFormato
                               where doc.CodEstado == codEstado
                               && doc.Borrado == false
                               && doc.CodProyecto == codProyecto
                               && doc.CodDocumentoFormato != 17
                               && doc.CodDocumentoFormato != 19
                               orderby doc.NomDocumento
                               select new GrillaDocumentos()
                               {
                                   Id_documento = doc.Id_Documento,
                                   NombreDocumento = doc.NomDocumento,
                                   Fecha = construirFecha(doc.Fecha),
                                   URL = doc.URL,
                                   NombreDocumentoFormato = formato.NomDocumentoFormato,
                                   Icono = formato.Icono != null ? formato.Icono : iconoDefault,
                                   CodigoDocumentoFormato = doc.CodDocumentoFormato,
                                   Tab = tmp.NomTab

                               }).ToList();

                return consulta != null ? consulta : new List<GrillaDocumentos>();
            }

        }

        public static List<GrillaDocumentos> getDocumentosEvalCtrlAnexo(string tabInvoca, int codigoProyecto, int codigoTab = 0)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                if (codigoTab != 0)
                {
                    var consulta = (

                                   from doc in db.Documentos
                                   join formato in db.DocumentoFormatos
                                   on doc.CodDocumentoFormato equals formato.Id_DocumentoFormato
                                   where doc.Borrado == false
                                   && doc.CodProyecto == codigoProyecto
                                   && doc.URL.Contains(tabInvoca)
                                   && doc.CodTab == codigoTab
                                   orderby doc.NomDocumento
                                   select new GrillaDocumentos()
                                   {
                                       Id_documento = doc.Id_Documento,
                                       NombreDocumento = doc.NomDocumento,
                                       Fecha = construirFecha(doc.Fecha),
                                       URL = doc.URL,
                                       NombreDocumentoFormato = formato.NomDocumentoFormato,
                                       Icono = formato.Icono != null ? formato.Icono : iconoDefault,
                                       CodigoDocumentoFormato = doc.CodDocumentoFormato
                                   }).ToList();

                    return consulta != null ? consulta : new List<GrillaDocumentos>();
                }
                else
                {
                    var consulta = (

                                   from doc in db.Documentos
                                   join formato in db.DocumentoFormatos
                                   on doc.CodDocumentoFormato equals formato.Id_DocumentoFormato
                                   where doc.Borrado == false
                                   && doc.CodProyecto == codigoProyecto
                                   && doc.URL.Contains(tabInvoca)
                                   orderby doc.NomDocumento
                                   select new GrillaDocumentos()
                                   {
                                       Id_documento = doc.Id_Documento,
                                       NombreDocumento = doc.NomDocumento,
                                       Fecha = construirFecha(doc.Fecha),
                                       URL = doc.URL,
                                       NombreDocumentoFormato = formato.NomDocumentoFormato,
                                       Icono = formato.Icono != null ? formato.Icono : iconoDefault,
                                       CodigoDocumentoFormato = doc.CodDocumentoFormato
                                   }).ToList();

                    return consulta != null ? consulta : new List<GrillaDocumentos>();
                }
            }
        }

        /// <summary>
        /// Obtiene los documentos de acreditación de un emprendedor
        /// </summary>
        /// <param name="codProyecto">Código del proyecto</param>
        /// <returns>Listado de documentos de acreditación asociados al proyecto</returns>
        public static List<GrillaDocumentos> getDocumentosAcreditacion(int codProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                var consulta1 = (from contactoArch in db.ContactoArchivosAnexos1
                               join contacto in db.Contacto
                               on contactoArch.CodContacto equals contacto.Id_Contacto into tablaFlt1
                               from regFlt1 in tablaFlt1.DefaultIfEmpty()
                               join contactoEst in db.ContactoEstudios
                               on contactoArch.CodContactoEstudio equals contactoEst.Id_ContactoEstudio into tablaFlt2
                               from regFlt2 in tablaFlt2.DefaultIfEmpty()
                               join nivelEst in db.NivelEstudios
                               on regFlt2.CodNivelEstudio equals nivelEst.Id_NivelEstudio into tablaFlt3
                               from regFlt3 in tablaFlt3.DefaultIfEmpty()
                               join txt1 in db.Textos
                               on contactoArch.TipoArchivo equals txt1.NomTexto into tablaFlt4
                               from regFlt4 in tablaFlt4
                               where contactoArch.CodProyecto == codProyecto
                               select new
                               {
                                   Id_ContactoArchivosAnexos = contactoArch.Id_ContactoArchivosAnexos,
                                   Ruta = contactoArch.ruta,
                                   TituloObtenido = regFlt2.TituloObtenido,
                                   NomNivelEstudio = regFlt3.NomNivelEstudio,
                                   Nombre = regFlt1.Nombres + " " + regFlt1.Apellidos,
                                   TipoArchivo = contactoArch.Descripcion != null ? contactoArch.Descripcion : regFlt4.Texto1,
                                   NomTexto = regFlt4.NomTexto,
                                   ContactoEst = contactoArch.CodContactoEstudio,
                                   Aniotitulo = regFlt2.AnoTitulo != null ? int.Parse(regFlt2.AnoTitulo.Value.ToString()) : DateTime.Now.Year,
                                   NomTextoDesc = string.Format("{0}_desc",regFlt4.NomTexto),
                                   Bloqueado = contactoArch.Bloqueado.GetValueOrDefault(false)
                               }).ToList();


                var consulta2 = (from datos in db.Textos
                                 where datos.NomTexto.EndsWith("_desc")
                                 select datos).ToList();


                var consulta = (from reg1 in consulta1
                                join reg2 in consulta2
                                on reg1.NomTextoDesc equals reg2.NomTexto into tb
                                from regfin in tb
                                select new GrillaDocumentos()
                                {
                                   Id_documento = reg1.Id_ContactoArchivosAnexos,
                                   Ruta = reg1.Ruta,
                                   TituloObtenido = reg1.TituloObtenido,
                                   NomNivelEstudio = reg1.NomNivelEstudio,
                                   Nombre = reg1.Nombre,
                                   TipoArchivo = reg1.TipoArchivo,
                                   TipoArchivoDescripcion = regfin.Texto1,
                                   Descripcion = reg1.ContactoEst == null
                                                 ? string.Format("{0} - {1}", reg1.TipoArchivo, regfin.Texto1)
                                                 : string.Format("{0} - {1} - {2}", reg1.TipoArchivo, reg1.TituloObtenido + "(" + reg1.NomNivelEstudio + ")",regfin.Texto1), 
                                   Aniotitulo = reg1.Aniotitulo,
                                   Bloqueado = reg1.Bloqueado
                                }).OrderBy(y => y.TipoArchivo).ToList();

                return consulta != null ? consulta : new List<GrillaDocumentos>();
            }
        }

        /// <summary>
        /// Consulta el documento actual
        /// </summary>
        /// <param name="idDocumento">Identificador primario del documento consultado</param>
        /// <returns>Verdadero si la operación fue exitosa falso en otro escenario</returns>
        public static bool updBorradoDocumento(int idDocumento)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    Documento docUpd = db.Documentos.Where(reg => reg.Id_Documento == idDocumento).FirstOrDefault();

                    if (docUpd != null)
                    {
                        docUpd.Borrado = true;
                        db.SubmitChanges();
                    }
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Obtiene los datos de un documento
        /// </summary>
        /// <param name="idDocumento">Identificador primario del documento</param>
        /// <returns>Información del documento</returns>
        public static GrillaDocumentos getDocumento(int idDocumento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                return (from doc in db.Documentos
                             from formato in db.DocumentoFormatos
                             where doc.CodDocumentoFormato == formato.Id_DocumentoFormato &&
                             doc.Id_Documento == idDocumento
                             select new GrillaDocumentos()
                             { 
                                 NombreDocumento = doc.NomDocumento,
                                 URL = doc.URL, 
                                 Comentario = doc.Comentario, 
                                 Extension = formato.Extension 
                             }).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene los datos de un documento
        /// </summary>
        /// <param name="idDocumento">Identificador primario del documento</param>
        /// <returns>Información del documento</returns>
        public static Documento getDocumentoNormal(int idDocumento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                return (from doc in db.Documentos
                        from formato in db.DocumentoFormatos
                        where doc.CodDocumentoFormato == formato.Id_DocumentoFormato &&
                        doc.Id_Documento == idDocumento
                        select doc).SingleOrDefault();
            }
        }
        
        public static ContactoArchivosAnexo getDocumentoNormalAcreditacion(int idDocumento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                return (from doc in db.ContactoArchivosAnexos1                        
                        where doc.Id_ContactoArchivosAnexos.Equals(idDocumento) 
                        select doc).SingleOrDefault();
            }
        }

        /// <summary>
        /// Constuye el formato de fecha presentado en la grilla
        /// </summary>
        /// <param name="fecha">Fecha</param>
        /// <returns>Fecha formateada</returns>
        private static string construirFecha(DateTime fecha)
        {
            string cadena = "";
            string mes = "";

            switch (fecha.Month)
            {
                case 1:
                    mes = "Ene";
                    break;
                case 2:
                    mes = "Feb";
                    break;
                case 3:
                    mes = "Mar";
                    break;
                case 4:
                    mes = "Abr";
                    break;
                case 5:
                    mes = "May";
                    break;
                case 6:
                    mes = "Jun";
                    break;
                case 7:
                    mes = "Jul";
                    break;
                case 8:
                    mes = "Ago";
                    break;
                case 9:
                    mes = "Sep";
                    break;
                case 10:
                    mes = "Oct";
                    break;
                case 11:
                    mes = "Nov";
                    break;
                default:
                    mes = "Dic";
                    break;
            }

            cadena = mes + fecha.ToString(" d 'de' yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

            return cadena;
        }

        /// <summary>
        /// Obtiene el formato de un documento
        /// </summary>
        /// <param name="formato">Nombre del formato</param>
        /// <returns>Formato consultado</returns>
        public static DocumentoFormato getDocumentoFormato(string formato)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                return (from df in db.DocumentoFormatos
                        where df.Extension == formato
                        select df).FirstOrDefault();
            }
        }

        /// <summary>
        /// Inserta un nuevo formato de documento
        /// </summary>
        /// <param name="nuevoformato">Nuevo Formato</param>
        /// <returns>Verdadero si la operación fue exitosa falso en otro escenario</returns>
        public static bool insDocumentoFormato(DocumentoFormato nuevoformato)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    db.DocumentoFormatos.InsertOnSubmit(nuevoformato);
                    db.SubmitChanges();
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Inserta y/o actualiza parcial o completa un documento
        /// </summary>
        /// <param name="doc">Documento a insertar / actualizar</param>
        /// <param name="esnuevo">Determina si el registro es nuevo</param>
        /// <param name="esparcial">Determina si se realizará parcial o en su totalidad la actualizacion de un registro</param>
        /// <returns>Verdadero si la operación fue exitosa falso en otro escenario</returns>
        public static bool setDocumento(Documento doc, bool esnuevo, bool esparcial)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    if (esnuevo)
                    {
                        db.Documentos.InsertOnSubmit(doc);
                    }
                    else
                    {
                        var reg = (from datos in db.Documentos
                                   where datos.Id_Documento == doc.Id_Documento
                                   select datos).SingleOrDefault();

                        if(reg != null)
                        {
                            reg.NomDocumento = doc.NomDocumento;
                            reg.Comentario = doc.Comentario;

                            if (!esparcial)
                            {
                                reg.Tab = doc.Tab;

                                if(doc.URL != "")
                                {
                                    reg.URL = doc.URL;
                                }
                                
                            }
                            
                        }
                    }
                    
                    db.SubmitChanges();
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        public static bool setDocumentoAcreditacion(ContactoArchivosAnexo doc, bool esnuevo, bool esparcial)
        {
            bool operacionOk = true;            
            try
            {                
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {                    
                    if (esnuevo)
                    {
                        db.ContactoArchivosAnexos1.InsertOnSubmit(doc);
                    }
                    else
                    {
                        var reg = (from datos in db.ContactoArchivosAnexos1
                                   where datos.Id_ContactoArchivosAnexos == doc.Id_ContactoArchivosAnexos
                                   select datos).SingleOrDefault();

                        if (reg != null)
                        {
                            reg.Descripcion = doc.Descripcion;
                            reg.Observacion = doc.Observacion;

                            if (!esparcial)
                            {                                
                                if (doc.ruta != "")
                                {
                                    reg.ruta = doc.ruta;
                                }

                            }

                        }
                    }

                    db.SubmitChanges();
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }


        public static bool updBorradoDocumentoAcreditacion(int idDocumento)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    ContactoArchivosAnexo docUpd = db.ContactoArchivosAnexos1.Where(reg => reg.Id_ContactoArchivosAnexos == idDocumento).FirstOrDefault();

                    if (docUpd != null)
                    {
                        db.ContactoArchivosAnexos1.DeleteOnSubmit(docUpd);
                        db.SubmitChanges();
                    }
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        public static GrillaDocumentos getDocumentoAcreditacion(int idDocumento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                return (from doc in db.ContactoArchivosAnexos1                        
                        where 
                            doc.Id_ContactoArchivosAnexos == idDocumento
                        select new GrillaDocumentos()
                        {
                            NombreDocumento = doc.Descripcion,
                            URL = doc.ruta,
                            Comentario = doc.Observacion,
                            Extension = string.Empty
                        }).SingleOrDefault();
            }
        }
    }
}
