using Datos;
using Fonade.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.Interventoria
{
    public class CorreosNotificacionBLL
    {
        string conexion = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public bool insertarInfoCorreo(int _codContactoEnvia, int _codContactoRecibe, int _codProyecto
                                    , string _correoRecibe
                                    , string _mensaje, string _asunto, ref int idCorreo
                                    , ref string mensajeError)
        {
            bool insertado = false;
            try
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    CorreoNotificacionCargaArhivoInterventoria correo = new CorreoNotificacionCargaArhivoInterventoria
                    {
                        codContactoEnvia = _codContactoEnvia,
                        codContactoRecibe = _codContactoRecibe,
                        codProyecto = _codProyecto,
                        EmailEnvia = buscarEmailContacto(_codContactoEnvia),
                        //EmailRecibe = buscarEmailContacto(_codContactoRecibe),
                        Asunto = _asunto,
                        EmailRecibe = _correoRecibe,
                        Enviado = false,
                        FechaHoraGenerado = DateTime.Now,
                        Mensaje = _mensaje,
                        NombreEnvia = buscarNombre(_codContactoEnvia),
                        NombreRecibe = buscarNombre(_codContactoRecibe)
                    };

                    db.CorreoNotificacionCargaArhivoInterventoria.InsertOnSubmit(correo);
                    db.SubmitChanges();
                    idCorreo = correo.idCorreo;
                    insertado = true;

                }
            }
            catch (Exception ex)
            {
                idCorreo = 0;
                insertado = false;
                mensajeError = ex.Message;
            }

            return insertado;
        }

        public bool actualizarEnvioCorreo(int idCorreo, ref string mensajeError)
        {
            bool actualizado = false;

            try
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    var correo = (from c in db.CorreoNotificacionCargaArhivoInterventoria
                                  where c.idCorreo == idCorreo
                                  select c).FirstOrDefault();

                    correo.Enviado = true;
                    correo.FechaHoraEnvio = DateTime.Now;

                    db.SubmitChanges();

                    actualizado = true;
                }
            }
            catch (Exception ex)
            {
                actualizado = false;
                mensajeError = ex.Message;
            }

            return actualizado;
        }


        public bool EnviarCorreoaDestinatario(string _Asunto, string paraEmail, string paraNombre
                                            , string Mensaje, ref string mensajeError)
        {
            bool enviado = false;

            try
            {

                CorreoAdvanced correo = new CorreoAdvanced(""
                    , _Asunto
                    , paraEmail
                    , paraNombre
                    , _Asunto
                    , Mensaje);
                string mensaje = correo.Enviar();
                if (mensaje != "OK")
                {
                    mensajeError = "Se cargó el archivo correctamente. Lastimosamente no se logró enviar el correo de notificacion. " + mensaje;
                    enviado = false;
                }
                else
                {
                    enviado = true;
                    mensajeError = "";
                }

            }
            catch (Exception e)
            {
                mensajeError = "La notificación por correo al usuario "
                    + paraEmail + " no pudo ser enviada. (" + e.GetType().Name + ")";
                enviado = false;
            }

            return enviado;
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

        private string buscarEmailContacto(int _codContacto)
        {
            string correo = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                correo = db.Contacto.Where(x => x.Id_Contacto == _codContacto)
                                    .Select(x => x.Email).FirstOrDefault();
            }

            return correo;
        }

        public List<ContactosEnviarCorreoDTO> enviarCorreoContactosDTO(int _codProyecto)
        {
            List<ContactosEnviarCorreoDTO> contactos = new List<ContactosEnviarCorreoDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                contactos = (from u in db.UsuariosPorProyecto(_codProyecto)
                             where u.EstadoContacto == "Activo" &&
                             (u.GrupoContacto == "Asesor" || u.GrupoContacto == "Emprendedor"
                             || u.GrupoContacto == "Interventor")
                             select new ContactosEnviarCorreoDTO
                             {
                                 codContacto = u.CodigoContacto,
                                 Email = u.EmailContacto,
                                 GrupoContacto = u.GrupoContacto,
                                 Nombres = u.NombresContacto
                             }).ToList();
            }

            return contactos;
        }

        public enum tipoDeCargaArchivo
        {            
            ContratosDeCooperacion = 1,
            ActasDeTerminacion = 2,
            ProrrogasDeContratos = 3,
            ActasDeLiquidacion = 4,
            OtrosDocumentos = 5,
            ContratoGarantiasMobiliarias = 6,
            Contrapartidas = 7,
            ContratosDeCooperacionFirmados = 8,
            ActaDeInicio = 9,
            Pagare = 10,
            InstruccionesDePagare = 11
        }

        public class tipoCargaArchivo
        {
            public string idtipo { get; set; }
            public string tipo { get; set; }
        } 

        public class ContactosEnviarCorreoDTO
        {
            public int codContacto { get; set; }
            public string Nombres { get; set; }
            public string Email { get; set; }
            public string GrupoContacto { get; set; }
        }

    }
}
