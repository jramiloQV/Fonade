using Datos;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using Fonade.Negocio.Utilidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


namespace Fonade.Negocio.PlanDeNegocioV2.Interventoria
{
    public class CargueMasivoInformesBLL
    {
        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public bool GuardarInformacionCorreo(int _codProyecto, int _codUsuarioEnvia
                                            , int _codRecomendacionInter
                                            , ref long codCorreoInforme
                                            , ref string mensajeError)
        {
            bool guardado = false;

            try
            {
                var emprendedores = codEmprendedores(_codProyecto);

                foreach (var e in emprendedores)
                {
                    using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                    {
                        CorreosInfomesConsolidados entity = new CorreosInfomesConsolidados();

                        entity.codProyecto = _codProyecto;
                        entity.codContactoEnvia = _codUsuarioEnvia;
                        entity.codRecomendacionInterventoria = _codRecomendacionInter;
                        entity.codContactoRecibe = e.codContacto;
                        entity.EmailRecibe = e.Email;
                        entity.Enviado = false;
                        entity.FechaHoraGenerado = DateTime.Now;
                        entity.Mensaje = Mensaje(_codProyecto, _codRecomendacionInter, e.codContacto, _codUsuarioEnvia);
                        entity.NombresEnvia = nombreXContacto(_codUsuarioEnvia);
                        entity.NombresRecibe = nombreXContacto(e.codContacto);

                        db.CorreosInfomesConsolidados.InsertOnSubmit(entity);
                        db.SubmitChanges();

                        codCorreoInforme = entity.idCorreo;

                        //Enviar Correo
                        if (codCorreoInforme > 0)
                        {
                            if (EnviarCorreo("",e.Email,entity.NombresRecibe
                                , entity.Mensaje
                                , ref mensajeError))
                            {
                                //Actualizar fecha y hora de envio
                                if (actualizarEnvioCorreo(codCorreoInforme))
                                {
                                    guardado = true;
                                    mensajeError = "";
                                }
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                guardado = false;
                mensajeError = "Lo lamentamos, no se guardo correctamente la informacion del correo: "+ex.Message;
            }

            return guardado;
        }

        public correoInformeDTO getCorreoInforme(long _idCorreo)
        {
            correoInformeDTO informeDTO = new correoInformeDTO();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                informeDTO = (from c in db.CorreosInfomesConsolidados
                              where c.idCorreo == _idCorreo
                              select new correoInformeDTO
                              {
                                  idCorreo = c.idCorreo,
                                  emailRecibe = c.EmailRecibe,
                                  fechaHoraEnvio = c.FechaHoraEnvio.ToString(),
                                  mensaje = c.Mensaje,
                                  nombreEnvia = c.NombresEnvia,
                                  nombreRecibe = c.NombresRecibe
                              }).FirstOrDefault();
            }

                return informeDTO;
        }


        private bool actualizarEnvioCorreo(long _codCorreo)
        {
            bool actualizado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var correoInforme = (from c in db.CorreosInfomesConsolidados
                                     where c.idCorreo == _codCorreo
                                     select c
                                     ).FirstOrDefault();

                correoInforme.FechaHoraEnvio = DateTime.Now;
                correoInforme.Enviado = true;
                db.SubmitChanges();

                actualizado = true;
            }

            return actualizado;
        }

        private bool EnviarCorreo(string DeEmail, string ParaEmail, string ParaNombre, string Mensaje
                                    , ref string mensajeError)
        {
            bool enviado = false;

            try
            {

                CorreoAdvanced correo = new CorreoAdvanced(DeEmail
                    , "Notificación del informe de seguimiento consolidado"
                    , ParaEmail
                    , ParaNombre
                    , "Notificación del informe de seguimiento consolidado"
                    , Mensaje);
                string mensaje = correo.Enviar();
                if (mensaje != "OK")
                {
                    mensajeError = "Se cargó el archivo correctamente. Lastimosamente no se logró enviar el correo de notificación. "+mensaje;
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
                    + ParaNombre + " no pudo ser enviada. (" + e.GetType().Name + ")";
                enviado = false;
            }

            return enviado;
        }

        private string nombreXContacto(int _codContacto)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from c in db.Contacto
                          where c.Id_Contacto == _codContacto
                          select c.Nombres + " " + c.Apellidos
                          ).FirstOrDefault();
            }

            return nombre;
        }

        private string nombreXProyecto(int _codProyecto)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from c in db.Proyecto
                          where c.Id_Proyecto == _codProyecto
                          select c.NomProyecto
                          ).FirstOrDefault();
            }

            return nombre;
        }

        private string mensajeAdicionalCondonacion(int _codRecomendacion, int _codProyecto)
        {
            string msj = "";

            if(_codRecomendacion==1)//Condonación y ordenar el no reembolso de recursos.
            {
                msj = "De acuerdo con lo anterior, la empresa "+ _codProyecto + " " + nombreXProyecto(_codProyecto).ToUpper() + ", " +
                      "es candidata para que le sean condonados los recursos asignados por parte del SENA, " +
                      "una vez quede en firme la respectiva resolución de condonación expedida por ésta Entidad." +
                      "<br />" +
                      "<br />";
            }

            return msj;
        }

        private string mensajeCumplimiento(int _codRecomendacion)
        {
            string msj = "";

            if (_codRecomendacion == 1)//Condonación y ordenar el no reembolso de recursos.
            {
                msj = "cumplimiento";
            }
            else
            {
                msj = "incumplimiento";
            }

            return msj;
        }

        private string Mensaje(int _codProyecto, int _codRecomendacion, int _codContactoRecibe, int _codContactoEnvia)
        {
            
            string interventoria = buscarEntidadInterventoria(_codProyecto).ToUpper().TrimEnd(' ');
            string operador = buscarOperador(_codProyecto).ToUpper();
            string recomendacion = recomendacionInterventoria(_codRecomendacion).ToUpper();

            string mensaje = "Bogotá D.C., "+ DateTime.Now.Date.ToShortDateString()
                                +"<br />" +
                                 "<br />" +
                                "Señor(a)" +
                                "<br />" +
                                "BENEFICIARIO(A)" +
                                "<br />" +
                                nombreXContacto(_codContactoRecibe).ToUpper() +
                                "<br />" +
                                _codProyecto+" "+ nombreXProyecto(_codProyecto).ToUpper() +
                                "<br />" +
                                "<br />" +
                                "Respetado señor(a):" +
                                "<br />" +
                                "<br />" +
                                "Con la presente comunicación le notificamos que la "+ interventoria +
                                ", en su calidad de Interventoria asignada para el " +
                                "seguimiento del referido contrato de cooperación " +
                                "empresarial identificado con ID "+ _codProyecto + " " + nombreXProyecto(_codProyecto).ToUpper() + ", " +
                                "emitió el informe de seguimiento consolidado con recomendación de "+ recomendacion + 
                                "Este se encuentra soportado con la información cargada o registrada " +
                                "en el sistema de información del Fondo Emprender, en las actas seguimiento realizadas " +
                                "por la interventoría y con todos los documentos que evidencian la ejecución del contrato." +
                                "<br />" +
                                "<br />" +
                                mensajeAdicionalCondonacion(_codRecomendacion, _codProyecto)+
                                "Como responsable del seguimiento de la ejecución del " +
                                "plan de negocios "+ _codProyecto + " " + nombreXProyecto(_codProyecto).ToUpper() + ", " +
                                "se da CONSTANCIA del "+mensajeCumplimiento(_codRecomendacion)+" por parte del emprendedor de las obligaciones " +
                                "contractuales pactadas y de los indicadores de gestión y resultado como consta en el " +
                                "Informe de seguimiento consolidado realizado por la Interventoría ejercida por la " +
                                interventoria + " y que ha sido notificado por medio del sistema de información del " +
                                "Fondo Emprender." +
                                "<br />" +
                                "<br />" +
                                "Con el fin de garantizar su derecho a la defensa, " +
                                "la contradicción y el debido proceso, se comunica que el referido informe " +
                                "consolidado se encuentra cargado en la pestaña Contrato del Sistema de " +
                                "Información del Fondo Emprender, con el fin que en el término de Diez(10) días " +
                                "hábiles siguientes al recibo de la presente comunicación, " +
                                "nos allegue  pronunciamiento sobre este a la dirección de correo: " +
                                emailOperadorXContacto(_codContactoEnvia)+" " +
                                "En caso de no recibir un pronunciamiento oficial, en el plazo establecido, " +
                                "sobre el contenido del informe de seguimiento consolidado, este se dará por aceptado." +
                                "<br />" +
                                "<br />" +
                                "Cordialmente," +
                                "<br />" +
                                "<br />" +
                                "Fondo Emprender" +
                                "<br />" +
                                "Sistema Nacional de Aprendizaje - SENA";
            return mensaje;
        }

        private string emailOperadorXContacto(int _codContactoEnvia)
        {
            string email = "";
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var codOperador = (from u in db.Contacto
                            where u.Id_Contacto == _codContactoEnvia
                            select u.codOperador).FirstOrDefault()??0;

                if(codOperador == 1)//Enterritorio
                {
                    email = ConfigurationManager.AppSettings.Get("EmailOperadorEnterritorio");
                }
                if (codOperador == 2)//Nacional
                {
                    email = ConfigurationManager.AppSettings.Get("EmailOperadorNacional");
                }
            }

                return email;
        }

        private string recomendacionInterventoria(int _codRecomendacion)
        {
            string recomendacion = "";

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                recomendacion = (from r in db.RecomendacionInterventoria
                                 where r.idRecomendacion == _codRecomendacion
                                 select r.RecomendacionInterventoria1).FirstOrDefault().ToUpper();

                if (_codRecomendacion == 1 || _codRecomendacion == 2)//1. Condonado //2. No condonado
                {
                    recomendacion = recomendacion + " asignados a su plan de negocio.";
                }
                                
            }

            return recomendacion;
        }

        private string buscarOperador(int _codProyecto)
        {
            OperadorController operadorController = new OperadorController();
            string operador = operadorController.nombreOperadorXProyecto(_codProyecto).ToUpper();

            if (operador == "ENTERRITORIO")
            {
                operador = "ENTERRITORIO (ANTES FONADE)";
            }

            return operador;
        }

        private string buscarEntidadInterventoria(int _codProyecto)
        {
            string interventoria = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                interventoria = (from e in db.Empresas
                                 join ei in db.EmpresaInterventors on e.id_empresa equals ei.CodEmpresa
                                 join entInt in db.EntidadInterventors on ei.CodContacto equals entInt.IdContactoInterventor
                                 join entidad in db.EntidadInterventoria on entInt.IdEntidad equals entidad.Id
                                 where e.codproyecto == _codProyecto
                                 && ei.Inactivo == false
                                 && ei.FechaFin == null
                                 select entidad.Nombre
                                 ).FirstOrDefault() ?? "";

                if (interventoria != "")
                {
                    interventoria = " de " + interventoria;
                }
            }

            return interventoria;
        }

        private List<EmprendedorDTO> codEmprendedores(int _codProyecto)
        {
            List<EmprendedorDTO> emprendedores = new List<EmprendedorDTO>();

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                emprendedores = (from con in db.Contacto
                                 join proCon in db.ProyectoContactos
                                 on con.Id_Contacto equals proCon.CodContacto
                                 where
                                    (proCon.CodRol == Constantes.CONST_RolEmprendedor)
                                    && proCon.CodProyecto == _codProyecto
                                    && proCon.FechaFin == null
                                    && proCon.Inactivo.Equals(false)
                                 select new EmprendedorDTO()
                                 {
                                     Identificacion = con.Identificacion,
                                     Nombres = con.Nombres + " " + con.Apellidos,
                                     Telefono = con.Telefono,
                                     Email = con.Email,
                                     codContacto = con.Id_Contacto
                                 }).ToList();
            }

            return emprendedores;
        }

        public class EmprendedorDTO
        {
            public int codContacto { get; set; }
            public double Identificacion { get; set; }
            public string Nombres { get; set; }
            public string Telefono { get; set; }
            public string Email { get; set; }
        }

        public class correoInformeDTO
        {
            public long idCorreo { get; set; }
            public string nombreEnvia { get; set; }
            public string nombreRecibe { get; set; }
            public string emailRecibe { get; set; }
            public string mensaje { get; set; }        
            public string fechaHoraEnvio { get; set; }
        }
    }
}
