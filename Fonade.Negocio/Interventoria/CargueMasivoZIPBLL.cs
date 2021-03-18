using Datos;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using static Fonade.Negocio.Interventoria.CorreosNotificacionBLL;

namespace Fonade.Negocio.Interventoria
{
    public class CargueMasivoZIPBLL
    {
        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<CargueMasivoMessage> cargarArchivo(tipoDeCargaArchivo _tipoArchivo
                                                    , int _idUsuario, FileUpload fileUpload
                                                    , string _correoEnvia
                                                    , ref string mensajeError)
        {
            List<CargueMasivoMessage> listCarga = new List<CargueMasivoMessage>();
            try
            {
                string BaseFile = _tipoArchivo + "_" + _idUsuario + "_"
                                     + string.Format("{0:00}", DateTime.Now.Day)
                                     + string.Format("{0:00}", DateTime.Now.Month)
                                     + DateTime.Now.Year + "_"
                                     + string.Format("{0:00}", DateTime.Now.Hour)
                                     + string.Format("{0:00}", DateTime.Now.Minute) + ".zip";

                string BaseDirectory = ConfigurationManager.AppSettings.Get("RutaIP")
                            + ConfigurationManager.AppSettings.Get("DirVirtual") + _tipoArchivo + "\\";

                string FullBaseFilDirectory = BaseDirectory + BaseFile;

                string OutPutDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "Proyecto\\";

                string rutaArchivo = FullBaseFilDirectory;

                if (!fileUpload.HasFile)
                    throw new ApplicationException("Archivo invalido");
                if (!fileUpload.FileName.Contains(".zip"))
                    throw new ApplicationException("Debe ser un archivo con extensión .zip");
                if (!(fileUpload.PostedFile.ContentLength < 1048576000))
                    throw new ApplicationException("El archivo es muy pesado, maximo 100 megas.");

                cargarZip(fileUpload, rutaArchivo, BaseDirectory);

                listCarga = descomprimirZip(rutaArchivo, OutPutDirectory, _idUsuario
                                            , _tipoArchivo, _correoEnvia);


                mensajeError = "OK";
            }
            catch (ApplicationException ex)
            {

                mensajeError = "Advertencia, detalle : " + ex.Message;
            }
            catch (Exception ex)
            {

                mensajeError = ex.Message;
            }
            return listCarga;
        }

        private void cargarZip(FileUpload _archivo, string _rutaArchivo, string _BaseDirectory)
        {
            if (!Directory.Exists(_BaseDirectory))
                Directory.CreateDirectory(_BaseDirectory);
            if (File.Exists(_rutaArchivo))
                File.Delete(_rutaArchivo);
            //if (Directory.Exists(OutPutDirectory))
            //Directory.Delete(OutPutDirectory);

            _archivo.SaveAs(_rutaArchivo);
        }

        private List<CargueMasivoMessage> descomprimirZip(string fullBaseFileDirectory
                                                        , string OutPutDirectory, int _codUsuario
                                                        , tipoDeCargaArchivo _tipoArchivo, string _correoEnvia)
        {
            List<CargueMasivoMessage> messages = new List<CargueMasivoMessage>();

            using (ZipFile zip = ZipFile.Read(fullBaseFileDirectory))
            {
                foreach (ZipEntry zipFile in zip)
                {
                    try
                    {
                        string mensajeError = "";
                        if (!IsValidFile(zipFile.FileName, ref mensajeError))
                            throw new ApplicationException(mensajeError);

                        var codigoProyecto = GetCodigoProyecto(zipFile.FileName);

                        if (!ProyectoExist(codigoProyecto, ref mensajeError))
                            throw new ApplicationException(mensajeError);

                        //Validar Operador si es administrador no se valida
                        if (OperadorXContacto(_codUsuario).IdOperador != 0)
                        {
                            if (OperadorXContacto(_codUsuario).IdOperador != OperadorXProyecto(codigoProyecto).IdOperador)
                                throw new ApplicationException("El operador no es el mismo del proyecto");
                        }


                        var customOutPutDirectory = OutPutDirectory + "Proyecto_" + codigoProyecto + "\\";

                        if (!Directory.Exists(customOutPutDirectory))
                            Directory.CreateDirectory(customOutPutDirectory);
                        if (File.Exists(customOutPutDirectory + zipFile.FileName))
                            throw new ApplicationException("Existe un archivo con el mismo nombre, renombrelo para poder subirlo.");

                        //Se coloca el archivo en la carpeta del proyecto
                        zipFile.Extract(customOutPutDirectory, ExtractExistingFileAction.OverwriteSilently);

                        InsertRegistroDB(codigoProyecto, zipFile.FileName, _codUsuario, _tipoArchivo);

                        //si es diferente a otrosdocumentos se requiere notificacion por correo
                        if (tipoDeCargaArchivo.OtrosDocumentos != _tipoArchivo)
                        {
                            //Traer usuarios para enviar correos
                            var usuariosEnviarCorreo = correosNotificacionBLL.enviarCorreoContactosDTO(codigoProyecto);

                            var emprendedores = usuariosEnviarCorreo.Where(x => x.GrupoContacto == "Emprendedor")
                                                                    .Select(x => x).ToList();

                            bool correosEnviados = false;

                            //Recorrer emprendedores
                            foreach (var emp in emprendedores)
                            {
                                //traer Mensaje del correo
                                string MensajeCorreo = buscarMensajeCorreo(_tipoArchivo, codigoProyecto
                                                                        , emp.Nombres, _correoEnvia);
                                string asunto = buscarAsuntoCorreo(_tipoArchivo);
                                //Enviar Correo a Emprendedor
                                correosEnviados = EnviarCorreo(codigoProyecto, _codUsuario, emp.codContacto, emp.Nombres
                                                                , emp.Email, asunto, MensajeCorreo, ref mensajeError);


                                if (!correosEnviados)
                                {
                                    //ingresar notificacion de NO envio
                                    messages.Add(new CargueMasivoMessage
                                    {
                                        Archivo = zipFile.FileName,
                                        CodigoProyecto = null,
                                        Message = "Advertencia : " + mensajeError,
                                        Error = ErrorType.Warning
                                    });
                                }


                                if (tipoDeCargaArchivo.ContratosDeCooperacionFirmados != _tipoArchivo)
                                {
                                    //Enviar Correos Asesores

                                    //Asesores
                                    var asesores = usuariosEnviarCorreo.Where(x => x.GrupoContacto == "Asesor")
                                                                        .Select(x => x).ToList();

                                    var mensajesRespuestaAsesores = EnviarCorreoOtrosPerfiles(asesores, codigoProyecto, _codUsuario, emp.codContacto, emp.Nombres
                                                               , asunto, MensajeCorreo, zipFile.FileName, ref mensajeError);

                                    messages.AddRange(mensajesRespuestaAsesores);
                                  
                                    //Enviar Correos Interventores
                                    //Interventores
                                    var interventores = usuariosEnviarCorreo.Where(x => x.GrupoContacto == "Interventor")
                                                                        .Select(x => x).ToList();

                                    var mensajesRespuestaInterventores= EnviarCorreoOtrosPerfiles(interventores, codigoProyecto, _codUsuario, emp.codContacto, emp.Nombres
                                                                , asunto, MensajeCorreo, zipFile.FileName, ref mensajeError);

                                    messages.AddRange(mensajesRespuestaInterventores);
                                }
                            }

                            if (correosEnviados)
                            {
                                //Si se realizó todo correctamente
                                messages.Add(new CargueMasivoMessage
                                {
                                    Archivo = zipFile.FileName,
                                    CodigoProyecto = codigoProyecto,
                                    Message = "Cargado exitosamente.",
                                    Url = ConfigurationManager.AppSettings.Get("RutaWebSite") + "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + zipFile.FileName,
                                    Error = ErrorType.success
                                });
                            }
                        }
                        else //Otros Documentos
                        {
                            //Si se realizó todo correctamente
                            messages.Add(new CargueMasivoMessage
                            {
                                Archivo = zipFile.FileName,
                                CodigoProyecto = codigoProyecto,
                                Message = "Cargado exitosamente.",
                                Url = ConfigurationManager.AppSettings.Get("RutaWebSite") + "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + zipFile.FileName,
                                Error = ErrorType.success
                            });
                        }
                    }
                    catch (ApplicationException ex)
                    {
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = null,
                            Message = "Advertencia : " + ex.Message,
                            Error = ErrorType.Warning
                        });
                    }
                    catch (Exception ex)
                    {
                        messages.Add(new CargueMasivoMessage
                        {
                            Archivo = zipFile.FileName,
                            CodigoProyecto = null,
                            Message = "Advertencia : " + ex.Message,
                            Error = ErrorType.Error
                        });
                    }
                }
            }

            return messages;
        }

        private List<CargueMasivoMessage> EnviarCorreoOtrosPerfiles(List<ContactosEnviarCorreoDTO> _contactosRecibe, int _codProyecto, int _codUsuario
                                               , int _codContactoEmprendedor, string _nombreEmprendedor
                                               , string _asunto, string _mensajeCorreo, string _nombreArchivo, ref string mensajeError)
        {
            List<CargueMasivoMessage> messages = new List<CargueMasivoMessage>();
            //Recorrer contactos
            foreach (var contact in _contactosRecibe)
            {
                //Enviar Correo a contacto
                bool correosEnviados = EnviarCorreo(_codProyecto, _codUsuario, _codContactoEmprendedor, _nombreEmprendedor
                                            , contact.Email, _asunto, _mensajeCorreo, ref mensajeError);

                if (!correosEnviados)
                {
                    //ingresar notificacion de NO envio
                    messages.Add(new CargueMasivoMessage
                    {
                        Archivo = _nombreArchivo,
                        CodigoProyecto = null,
                        Message = "Advertencia : " + mensajeError,
                        Error = ErrorType.Warning
                    });
                }
            }

            return messages;
        }

        private bool EnviarCorreo(int _codProyecto, int _codContactoEnvia
                                    , int _codContactoRecibe, string _NombresRecibe
                                    , string _correoRecibe, string _Asunto
                                    , string _MensajeCorreo, ref string mensajeError)
        {
            bool enviado = false;
            //Ingresar en la tabla los datos de correos a enviar.                        
            int codCorreoInforme = 0;

            if (IngresarInformacionCorreo(_codProyecto, _codContactoEnvia, _codContactoRecibe
                                          , _MensajeCorreo, _Asunto, _correoRecibe, ref codCorreoInforme
                                          , ref mensajeError))
            {
                //Enviar Correo
                if (correosNotificacionBLL.EnviarCorreoaDestinatario(_Asunto, _correoRecibe, _NombresRecibe
                                                                    , _MensajeCorreo, ref mensajeError))
                {
                    //Actualizar tabla de envio de correo
                    if (correosNotificacionBLL.actualizarEnvioCorreo(codCorreoInforme, ref mensajeError))
                    {
                        enviado = true;
                    }
                    else
                    {
                        mensajeError = "El archivo se cargó correctamente, se envió la notificacion por correo a " + _correoRecibe + " #correo: " + codCorreoInforme + " : " + mensajeError;
                        enviado = false;
                    }
                }
                else
                {
                    mensajeError = "El archivo se cargó correctamente, pero no se logró enviar la notificacion por correo a " + _correoRecibe + " : " + mensajeError;
                    enviado = false;
                }
            }
            else
            {
                mensajeError = "El archivo se cargó correctamente, pero no se logró enviar la notificacion por correo a " + _correoRecibe + " : " + mensajeError;
                enviado = false;
            }

            return enviado;
        }

        private string buscarAsuntoCorreo(tipoDeCargaArchivo _tipoArchivo)
        {
            string asunto = "";

            if (_tipoArchivo == tipoDeCargaArchivo.ContratosDeCooperacion
                || _tipoArchivo == tipoDeCargaArchivo.ContratosDeCooperacionFirmados)
            {
                asunto = "Contrato de Cooperación e Instructivos";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ActasDeLiquidacion)
            {
                asunto = "Acta de Liquidación del Contrato";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ProrrogasDeContratos)
            {
                asunto = "Prorroga Contrato de Cooperación";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ActasDeTerminacion)
            {
                asunto = "Acta de Terminación del Contrato";
            }

            return asunto;
        }

        private string buscarMensajeCorreo(tipoDeCargaArchivo _tipoArchivo, int _codProyecto, string _nombreRecibe
                                            , string _correoEnvia)
        {
            string mensaje = "";

            var operador = OperadorXProyecto(_codProyecto);

            string numeroContrato = buscarContratoEmpresa(_codProyecto);

            if (_tipoArchivo == tipoDeCargaArchivo.ContratosDeCooperacionFirmados)
            {
                mensaje = "Emprendedor(a) "
                          + Environment.NewLine +
                          _nombreRecibe
                          + Environment.NewLine + Environment.NewLine +
                          "Asunto:  Contrato de Cooperación e Instructivos "
                          + Environment.NewLine + Environment.NewLine +
                          "Se informa que el contrato de cooperacion del proyecto ya se encuentra " +
                          "cargado y firmado en la pestaña contrato, seccion archivos.";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ContratosDeCooperacion)
            {
                mensaje = "Emprendedor(a) "
                          + Environment.NewLine +
                          _nombreRecibe
                          + Environment.NewLine + Environment.NewLine +
                          "Asunto:  Contrato de Cooperación e Instructivos "
                          + Environment.NewLine + Environment.NewLine +
                          "Se informa que el contrato de cooperación No " + numeroContrato + "  " +
                          "se encuentra cargada en la plataforma Fondo Emprender " +
                          "con el fin de que sea firmada digitalmente, " +
                          "y posteriormente enviada a " + operador.NombreOperador + " " +
                          "a la dirección " + _correoEnvia + ". " +
                          "con el fin de surtir el trámite interno pertinente.";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ActasDeLiquidacion)
            {
                mensaje = "Emprendedor(a) "
                          + Environment.NewLine +
                          _nombreRecibe
                          + Environment.NewLine + Environment.NewLine +
                          "Asunto: Acta de Liquidación del Contrato " + numeroContrato
                          + Environment.NewLine + Environment.NewLine +
                          "Se informa que acta liquidación del contrato de cooperación No " + numeroContrato + " " +
                          "se encuentra cargada en la plataforma de fondo Emprender " +
                          "en la pestaña de contrato, con el fin de que sea firmada " +
                          "y posteriormente enviada a " + operador.NombreOperador + " a la " +
                          "dirección " + _correoEnvia + ". " +
                          "Esta debe ser recibida  y diligenciada virtualmente lo más pronto posible, " +
                          "con el fin de surtir el trámite interno pertinente.";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ProrrogasDeContratos)
            {
                mensaje = "Emprendedor(a) "
                          + Environment.NewLine +
                          _nombreRecibe
                          + Environment.NewLine + Environment.NewLine +
                          "Asunto: Prorroga Contrato de Cooperación"
                          + Environment.NewLine + Environment.NewLine +
                          "Se informa que la prórroga al contrato de cooperación No " + numeroContrato + " " +
                          "la cual se encuentra cargada en la plataforma Fondo Emprender  " +
                          "con el fin de que sea firmada, y posteriormente " +
                          "enviada a " + operador.NombreOperador + "  a " +
                          "la dirección " + _correoEnvia + ", con el fin de surtir el trámite interno pertinente.";
            }

            if (_tipoArchivo == tipoDeCargaArchivo.ActasDeTerminacion)
            {
                mensaje = "Emprendedor(a) "
                          + Environment.NewLine +
                          _nombreRecibe
                          + Environment.NewLine + Environment.NewLine +
                          "Asunto:  Acta de Terminación del Contrato " + numeroContrato
                          + Environment.NewLine + Environment.NewLine +
                          "Se informa que acta Terminación del contrato de cooperación No " + numeroContrato + " " +
                          "se encuentra cargada en la plataforma de fondo Emprender en la pestaña de contrato, " +
                          "con el fin de que sea firmada y posteriormente enviada a " + operador.NombreOperador + " " +
                          "a la dirección " + _correoEnvia + ". " +
                          "Esta debe ser recibida  y diligenciada virtualmente lo más pronto posible, " +
                          "con el fin de surtir el trámite interno pertinente. ";
            }

            return mensaje;
        }

        private string buscarContratoEmpresa(int _codProyecto)
        {
            string contrato = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                contrato = (from ce in db.ContratoEmpresas
                            join e in db.Empresas on ce.CodEmpresa equals e.id_empresa
                            where e.codproyecto == _codProyecto
                            select ce.NumeroContrato).FirstOrDefault();
            }

            return contrato;
        }

        CorreosNotificacionBLL correosNotificacionBLL = new CorreosNotificacionBLL();

        private bool IngresarInformacionCorreo(int _codProyecto, int _codUsuarioEnvia
                                                , int _codUsuarioRecibe, string MensajeCorreo
                                                , string _asunto
                                                , string _correoRecibe
                                                , ref int idCorreo
                                                , ref string MensajeError)
        {

            bool ingresado = correosNotificacionBLL.insertarInfoCorreo(_codUsuarioEnvia
                                                                    , _codUsuarioRecibe
                                                                    , _codProyecto
                                                                    , _correoRecibe, MensajeCorreo
                                                                    , _asunto
                                                                    , ref idCorreo
                                                                    , ref MensajeError);


            return ingresado;
        }

        private string nombreContacto(int _codContacto)
        {
            string nombre = "-";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var usuario = (from u in db.Contacto
                               join ug in db.GrupoContactos on u.Id_Contacto equals ug.CodContacto
                               where u.Id_Contacto == _codContacto
                               select new
                               {
                                   u.Nombres,
                                   u.Apellidos,
                                   ug.CodGrupo
                               }).FirstOrDefault();

                //if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                //{
                nombre = usuario.Nombres + " " + usuario.Apellidos;
                //}
            }

            return nombre;
        }



        private void InsertRegistroDB(int codigoProyecto, string fileName, int _codUsuario, tipoDeCargaArchivo _tipoArchivo)
        {
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var archivo = new ArchivosAdicionalesContrato
                {
                    CodContacto = _codUsuario,
                    CodProyecto = codigoProyecto,
                    FechaIngreso = DateTime.Now,
                    NombreArchivo = fileName,
                    NombreContacto = nombreContacto(_codUsuario),
                    ruta = "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + fileName,
                    TipoArchivo = _tipoArchivo.ToString(),
                    Eliminado = false
                };
                db.ArchivosAdicionalesContrato.InsertOnSubmit(archivo);
                db.SubmitChanges();
            }

            //using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            //{
            //    var archivoContrato = new ContratosArchivosAnexo
            //    {
            //        CodProyecto = codigoProyecto,
            //        NombreArchivo = fileName,
            //        ruta = "Documentos/Proyecto/Proyecto_" + codigoProyecto + "/" + fileName,
            //        CodContacto = _codUsuario,
            //        FechaIngreso = DateTime.Now
            //    };
            //    db.ContratosArchivosAnexos.InsertOnSubmit(archivoContrato);
            //    db.SubmitChanges();
            //}
        }

        private Operador buscarOperadorXId(int _codOperador)
        {
            Operador operador = new Operador();
            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(conexion))
            {
                operador = (from o in db.Operador
                            where o.IdOperador == _codOperador
                            select o).FirstOrDefault();
            }
            return operador;
        }

        private Operador OperadorXContacto(int _codContacto)
        {
            int codOperador = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codOperador = (from p in db.Contacto
                               where p.Id_Contacto == _codContacto
                               select p.codOperador).FirstOrDefault() ?? 0;
            }

            if (codOperador == 0)
            {
                Operador operador = new Operador
                {
                    IdOperador = 0
                };
                return operador;
            }
            else
            {
                return buscarOperadorXId(codOperador);
            }
        }

        private Operador OperadorXProyecto(int _codProyecto)
        {
            int codOperador = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codOperador = (from p in db.Proyecto
                               where p.Id_Proyecto == _codProyecto
                               select p.codOperador).FirstOrDefault() ?? 0;
            }

            return buscarOperadorXId(codOperador);
        }

        private bool ProyectoExist(int codigoProyecto, ref string mensajeError)
        {
            bool valido = true;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                if (!db.Proyecto.Any(exist => exist.Id_Proyecto.Equals(codigoProyecto)))
                {
                    valido = false;
                    mensajeError = "No existe el proyecto.";
                }

            }
            return valido;
        }

        private int GetCodigoProyecto(string fileName)
        {
            var indiceCodigoProyecto = fileName.IndexOf("-");
            var codigoProyecto = fileName.Substring(0, indiceCodigoProyecto);

            return Convert.ToInt32(codigoProyecto);
        }

        private bool IsValidFile(string fileName, ref string mensajeError)
        {
            bool valido = true;
            if (!fileName.Contains(".pdf"))
            {
                valido = false;
                mensajeError = "Debe ser un archivo con extensión .pdf";
            }

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                if (fileName.Contains(invalidChar))
                {
                    valido = false;
                    mensajeError = "Contiene caracteres especiales.";
                    break;
                }
            }

            try
            {
                var indiceCodigoProyecto = fileName.IndexOf("-");
                var codigoProyecto = fileName.Substring(0, indiceCodigoProyecto);
                int value;
                if (!int.TryParse(codigoProyecto, out value))
                    throw new Exception();

            }
            catch (Exception)
            {
                valido = false;
                mensajeError = "Formato invalido. ex : CodigoProyecto-xxxxxxxx.pdf";
            }

            return valido;
        }

        public List<ArchivosEspeciales> getArchivosEspeciales(int _codProyecto)
        {
            List<ArchivosEspeciales> archivosEspeciales = new List<ArchivosEspeciales>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                archivosEspeciales = (from a in db.ArchivosAdicionalesContrato
                                      where a.CodProyecto == _codProyecto
                                      && a.Eliminado == false
                                      select new ArchivosEspeciales
                                      {
                                          CodContacto = a.CodContacto,
                                          Eliminado = a.Eliminado,
                                          CodProyecto = a.CodProyecto,
                                          FechaIngreso = a.FechaIngreso,
                                          idArchivo = a.idArchivo,
                                          NombreArchivo = a.NombreArchivo,
                                          NombreContacto = a.NombreContacto,
                                          ruta = a.ruta,
                                          TipoArchivo = a.TipoArchivo,
                                          NombreTipoArchivo = obtenerNombreTipoDeCargaArchivo(a.TipoArchivo)
                                      }).ToList();
            }

            return archivosEspeciales;
        }

        public bool eliminarArchivosEspecialContrato(int _idArchivo, int _codUsuario)
        {
            bool elimado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var archivo = (from a in db.ArchivosAdicionalesContrato
                               where a.idArchivo == _idArchivo
                               select a).FirstOrDefault();

                archivo.Eliminado = true;
                archivo.EliminadoPor = _codUsuario;
                archivo.fechaEliminado = DateTime.Now;

                db.SubmitChanges();

                elimado = true;
            }

            return elimado;
        }

        private string buscarNombre(int _codContacto)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = db.Contacto.Where(x => x.Id_Contacto == _codContacto)
                                    .Select(x => x.Nombres + " " + x.Apellidos).FirstOrDefault();
            }

            return nombre;
        }
        public static List<tipoCargaArchivo> tipoCargaArchivo()
        {
            List<tipoCargaArchivo> list = new List<tipoCargaArchivo>();

            foreach (var tipo in Enum.GetValues(typeof(tipoDeCargaArchivo)))
            {
                if (tipoDeCargaArchivo.OtrosDocumentos != (tipoDeCargaArchivo)tipo)
                {
                    tipoCargaArchivo tipoArchivo = new tipoCargaArchivo
                    {
                        idtipo = tipo.ToString(),
                        tipo = obtenerNombreTipoDeCargaArchivo(tipo.ToString())
                    };

                    list.Add(tipoArchivo);
                }

            }

            return list;
        }

        public static string obtenerNombreTipoDeCargaArchivo(string tipo)
        {
            string nombre = "";
            if (tipoDeCargaArchivo.ActasDeLiquidacion.ToString() == tipo)
                return "Acta de liquidacion";
            if (tipoDeCargaArchivo.ActasDeTerminacion.ToString() == tipo)
                return "Acta de terminacion";
            if (tipoDeCargaArchivo.ContratosDeCooperacion.ToString() == tipo)
                return "Contrato de cooperacion";
            if (tipoDeCargaArchivo.ProrrogasDeContratos.ToString() == tipo)
                return "Prorroga de contrato";
            if (tipoDeCargaArchivo.ContratoGarantiasMobiliarias.ToString() == tipo)
                return "Contrato de garantias mobiliarias";
            if (tipoDeCargaArchivo.Contrapartidas.ToString() == tipo)
                return "Contrapartidas";
            if (tipoDeCargaArchivo.ContratosDeCooperacionFirmados.ToString() == tipo)
                return "Contrato de cooperacion firmado";
            if (tipoDeCargaArchivo.ActaDeInicio.ToString() == tipo)
                return "Acta de Inicio";
            if (tipoDeCargaArchivo.Pagare.ToString() == tipo)
                return "Pagaré";
            if (tipoDeCargaArchivo.InstruccionesDePagare.ToString() == tipo)
                return "Instrucciones de Pagaré";

            return nombre;
        }

        private bool UploadFile(FileUpload archivo, string _rutaArchivo, int _codProyecto
                                    , string nombreArchivo, ref string Error)
        {
            bool cargado = false;

            try
            {
                
                string rutaFinal = _rutaArchivo + nombreArchivo;

                if (!Directory.Exists(_rutaArchivo))
                    Directory.CreateDirectory(_rutaArchivo);
                if (File.Exists(rutaFinal))
                    File.Delete(rutaFinal);
                //if (Directory.Exists(OutPutDirectory))
                //Directory.Delete(OutPutDirectory);

                archivo.SaveAs(rutaFinal);

                cargado = true;
            }
            catch (Exception ex)
            {
                cargado = false;
                Error = ex.Message;
            }
            return cargado;
        }

        private bool ingresarRegistroArchivoAdicionalContrato(int _codproyecto, int _codContacto
                                            , string _nomArchivo
                                            , string _tipoDeCargaArchivo, ref string error)
        {
            bool ingresado = false;

            try
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    string rutaDef = "Documentos/Proyecto/Proyecto_" + _codproyecto + "/" + _nomArchivo;

                    var archivoscargado = (from h in db.ArchivosAdicionalesContrato
                                           where h.CodProyecto == _codproyecto
                                           && h.CodContacto == _codContacto
                                           && h.TipoArchivo == _tipoDeCargaArchivo.ToString()
                                           && h.Eliminado == false
                                           select h).FirstOrDefault();

                    if (archivoscargado == null)
                    {
                        ArchivosAdicionalesContrato archivo = new ArchivosAdicionalesContrato
                        {
                            CodContacto = _codContacto,
                            CodProyecto = _codproyecto,
                            Eliminado = false,
                            FechaIngreso = DateTime.Now,
                            NombreArchivo = _nomArchivo,
                            NombreContacto = buscarNombre(_codContacto),
                            ruta = rutaDef,
                            TipoArchivo = _tipoDeCargaArchivo.ToString()
                        };

                        db.ArchivosAdicionalesContrato.InsertOnSubmit(archivo);
                        db.SubmitChanges();
                        ingresado = true;
                    }
                    else
                    {
                        //habActaSeguimiento.fechaCarga = DateTime.Now;
                        ingresado = false;
                        error = "No puede volver a ingresar el mismo tipo de archivo";
                    }
                }
            }
            catch (Exception ex)
            {
                ingresado = false;
                error = ex.Message;
            }

            return ingresado;
        }

        public bool validarCargaArchivoAdicional(int _codProyecto, string _tipoDeCargaArchivo, int _codContacto)
        {
            int count = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var archivoscargado = (from h in db.ArchivosAdicionalesContrato
                                       where h.CodProyecto == _codProyecto
                                       && h.TipoArchivo == _tipoDeCargaArchivo.ToString()
                                       && h.CodContacto == _codContacto
                                       && h.Eliminado == false
                                       select h).ToList();

                count = archivoscargado.Count();
            }

            return count == 0;
        }

        public bool cargarArchivoAdicionalEmprendedor(FileUpload fileUpload, string _tipoArchivo
                                        , int _codProyecto, int _codUsuario, ref string mensajeError)
        {
            bool ingresado = false;

            string OutPutDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "Proyecto\\";

            var customOutPutDirectory = OutPutDirectory + "Proyecto_" + _codProyecto + "\\";

            if (!Directory.Exists(customOutPutDirectory))
                Directory.CreateDirectory(customOutPutDirectory);
            if (File.Exists(customOutPutDirectory + fileUpload.FileName))
            {
                mensajeError = "Existe un archivo con el mismo nombre, renombrelo para poder subirlo.";
                ingresado = false;
            }
            else
            {

                //Agregar Registro en la BD
                string rutaArchivo = customOutPutDirectory;
                try
                {
                    if (fileUpload.HasFile)
                    {
                        if (!(fileUpload.PostedFile.ContentLength < 1048576000))
                            throw new ApplicationException("El archivo es muy pesado, maximo 100 megas.");

                        //validar si ya se cargo el archivo
                        if (validarCargaArchivoAdicional(_codProyecto, _tipoArchivo, _codUsuario))
                        {
                            if (!UploadFile(fileUpload, rutaArchivo, _codProyecto
                                                        , fileUpload.FileName, ref mensajeError))
                                throw new ApplicationException("No se logró cargar el archivo: " + mensajeError);

                            //Ingresar ruta Archivo en BD
                            if (ingresarRegistroArchivoAdicionalContrato(_codProyecto, _codUsuario,
                                                                          fileUpload.FileName, _tipoArchivo,
                                                                          ref mensajeError))
                            {
                                ingresado = true;
                            }
                        }
                        else
                        {
                            mensajeError = "Ya realizó el cargue del archivo firmado para este documento. Pongase en contacto con su interventor";
                        }
                    }
                    else
                    {
                        ingresado = false;
                        mensajeError = "No seleccionó ningun archivo.";
                    }
                }
                catch (ApplicationException ex)
                {
                    ingresado = false;
                    mensajeError = "Advertencia, detalle: " + ex.Message;
                }
                catch (Exception ex)
                {
                    ingresado = false;
                    mensajeError = "Lo lamentamos sucedió un error: " + ex.Message;
                }
            }            

            return ingresado;
        }
    }
    public class ArchivosEspeciales
    {
        public int idArchivo { get; set; }
        public int CodProyecto { get; set; }
        public string NombreArchivo { get; set; }
        public string ruta { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int CodContacto { get; set; }
        public string NombreContacto { get; set; }
        public string TipoArchivo { get; set; }
        public string NombreTipoArchivo { get; set; }
        public bool Eliminado { get; set; }
    }

    public enum ErrorType
    {
        success,
        Error,
        Warning
    }
    public class CargueMasivoMessage
    {
        public int? CodigoProyecto { get; set; }
        public ErrorType Error { get; set; }
        public string MessageColor
        {
            get
            {
                if (Error.Equals(ErrorType.Warning))
                    return "#FFFF66";
                if (Error.Equals(ErrorType.Error))
                    return "#FFCCCC";
                if (Error.Equals(ErrorType.success))
                    return "#29de61";
                return "#DCDCDC";
            }
            set { }
        }
        public string Message { get; set; }
        public string Archivo { get; set; }
        public string Url { get; set; }
        public Boolean ShowUrl { get { return Error.Equals(ErrorType.success); } set { } }
    }
}
