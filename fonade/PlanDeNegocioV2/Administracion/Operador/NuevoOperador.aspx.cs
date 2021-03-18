using Fonade.Account;
using Fonade.Clases;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.Operador
{
    public partial class NuevoOperador : Negocio.Base_Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        public bool obligatorio { get { return (idOperadorSession == 0); } }

        public int idOperadorSession { 
            get { 
                return 
                    Session["idOperador"] == null ? 0 : Convert.ToInt32(Session["idOperador"].ToString());  
            } 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ValidateUsers();
                if (idOperadorSession != 0) {
                    cargarDatos(idOperadorSession);
                }
                
            }
        }

        private void cargarDatos(int? _idOperador)
        {
            if(_idOperador != null)
            {
                var operador = operadorController.getOperador(_idOperador);

                txtNombre.Text = operador.NombreOperador;
                txtNit.Text = operador.NitOperador;
                txtTelefono.Text = operador.TelefonoOperador;
                txtDireccion.Text = operador.DireccionOperador;
                txtEmail.Text = operador.EmailOperador;
                txtRepresentante.Text = operador.NombreRepresentante;
                txtTelRepresentante.Text = operador.TelefonoRepresentante;
                txtEmailRepresentante.Text = operador.EmailRepresentante;
                txtEmailObsAcreditacion.Text = operador.EmailObservacionAcreditacion;
            }
        }

        public void ValidateUsers()
        {
            try
            {
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "";

                var rutaArchivo = string.Empty;
                if (!fuArchivo.HasFile && idOperadorSession == 0)
                {
                    throw new ApplicationException("La imagen del operador es obligatoria!");
                }
                else
                {
                    if (fuArchivo.FileName != string.Empty)
                    {
                        rutaArchivo = UploadFile(fuArchivo);
                    }
                }

                OperadorModel model = new OperadorModel
                {
                    DireccionOperador = txtDireccion.Text,
                    EmailObservacionAcreditacion = txtEmailObsAcreditacion.Text,
                    EmailOperador = txtEmail.Text,
                    EmailRepresentante = txtEmailRepresentante.Text,
                    FechaCreacion = DateTime.Now,
                    NitOperador = txtNit.Text,
                    NombreOperador = txtNombre.Text,
                    NombreRepresentante = txtRepresentante.Text,
                    Rutalogo = rutaArchivo,
                    TelefonoOperador = txtTelefono.Text,
                    TelefonoRepresentante = txtTelRepresentante.Text,
                    idOperador = idOperadorSession
                };

                if (idOperadorSession != 0) //actualizar operador
                {
                    actualizarOperadorExistente(model, ref mensaje);
                }
                else //nuevo operador
                {
                    InsertarNuevoOperador(model, ref mensaje);
                }

                
            }
            catch (ApplicationException exApp)
            {
                Alert(exApp.Message);
            }
        }

        private void actualizarOperadorExistente(OperadorModel model, ref string mensaje)
        {
            if (actualizarOperador(model, ref mensaje))
            {
                Alert("Operador Actualizado Exitosamente!"
                    , "Operadores.aspx");
                //Response.Redirect("~/PlanDeNegocioV2/Administracion/Operador/Operadores.aspx", true);
            }
            else
            {
                Alert(mensaje);
            }
        }

        private bool actualizarOperador(OperadorModel model, ref string mensaje)
        {
            bool update = false;

            //validar el correo del acreditador
            if (!validarCorreo(txtEmailObsAcreditacion.Text))
            {
                Alert("El email de observacion de acreditacion NO es valildo.");
            }
            else
            {
                update = operadorController.actualizarOperador(model, ref mensaje);
            }

            return update;
        }

        private void InsertarNuevoOperador(OperadorModel model, ref string mensaje)
        {
            if (insertarOperador(model, ref mensaje))
            {
                Alert("Operador Creado Exitosamente!"
                    , "Operadores.aspx");
                //Response.Redirect("~/PlanDeNegocioV2/Administracion/Operador/Operadores.aspx", true);
            }
            else
            {
                Alert(mensaje);
            }
        }

        protected string UploadFile(FileUpload _archivo)
        {
            string directorioDestino = "Operadores\\";
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string nombreArchivo = "LogoOperador_" + DateTime.Now.GetShortDateOnlyNumbersWithUnderscore() + Path.GetExtension(_archivo.FileName);

            // ¿ Es valido el archivo ?
            if (!_archivo.HasFile)
                throw new ApplicationException("Archivo invalido");
            // ¿ Es image valida ? 
            if (_archivo.PostedFile.ContentType.ToLower() != "image/jpg" &&
                    _archivo.PostedFile.ContentType.ToLower() != "image/jpeg" &&
                    _archivo.PostedFile.ContentType.ToLower() != "image/pjpeg" &&
                    _archivo.PostedFile.ContentType.ToLower() != "image/x-png" &&
                    _archivo.PostedFile.ContentType.ToLower() != "image/png")
            {
                throw new ApplicationException("Los formatos permitidos son jpg, jpeg y png, los demas tipos son invalidos.");
            }
            if (Path.GetExtension(_archivo.PostedFile.FileName).ToLower() != ".jpg"
            && Path.GetExtension(_archivo.PostedFile.FileName).ToLower() != ".png"
            && Path.GetExtension(_archivo.PostedFile.FileName).ToLower() != ".jpeg")
            {
                throw new ApplicationException("Los formatos permitidos son jpg, jpeg y png, los demas tipos son invalidos.");
            }

            UploadFileToServer(_archivo, directorioBase, directorioDestino, nombreArchivo);

            return directorioDestino + nombreArchivo;
        }

        protected void UploadFileToServer(FileUpload _archivo, string _directorioBase, string _directorioDestino, string fileName)
        {
            // ¿ Carpeta de destino existe ?
            if (!Directory.Exists(_directorioBase + _directorioDestino))
                Directory.CreateDirectory(_directorioBase + _directorioDestino);
            if (File.Exists(_directorioBase + _directorioDestino + _archivo.FileName))
                File.Delete(_directorioBase + _directorioDestino + _archivo.FileName);

            _archivo.SaveAs(_directorioBase + _directorioDestino + fileName); //Guardamos el archivo
        }

        OperadorController operadorController = new OperadorController();

        private bool insertarOperador(OperadorModel model, ref string mensaje)
        {
            bool insert = false;

            //validar el correo del acreditador
            if (!validarCorreo(txtEmailObsAcreditacion.Text))
            {
                Alert("El email de observacion de acreditacion NO es valildo.");
            }
            else
            {
                insert = operadorController.InsertarOperador(model, ref mensaje);
            }

            return insert;
        }

        private Boolean validarCorreo(String email)
        {
            String expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private void Alert(string mensaje, string ruta="")
        {
            if(ruta=="")
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
            else
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');" +
                    "window.location.href='"+ ruta + "';", true);
        }
    }
}