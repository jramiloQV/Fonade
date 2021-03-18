using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reintegros;
using Fonade.Negocio.PlanDeNegocioV2.Interventoria;
using Fonade.Clases;
using System.IO;
using System.Configuration;
using Fonade.Account;
using System.Web.Security;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using Fonade.Negocio.Mensajes;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad
{
    public partial class NewEntidad : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ValidateUsers();
                CargarDrops();
            }
        }

        private void CargarDrops()
        {
            //operador
            cargarDDL(ddlOperador, Usuario.CodOperador);
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDDL(DropDownList ddl, int? _codOperador)
        {
            List<OperadorModel> opciones = new List<OperadorModel>();

            opciones = operadorController.cargaDLLOperador(_codOperador);
            
            ddl.DataSource = opciones;
            ddl.DataTextField = "NombreOperador"; // FieldName of Table in DataBase
            ddl.DataValueField = "idOperador";
            ddl.DataBind();
        }

        public void ValidateUsers()
        {
            try
            {
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        protected void ValidarDatos()
        {

            FieldValidate.ValidateString("Nombre", txtNombre.Text, true, 100);
            FieldValidate.ValidateString("Nombre corto", txtNombreCorto.Text, true, 50);
            FieldValidate.ValidateString("Número de poliza", txtNumeroPoliza.Text, true, 15);
            FieldValidate.ValidateString("Fecha de poliza", txtFechaPoliza.Text, true, 50);
            FieldValidate.ValidateString("Persona a cargo", txtPersonaACargo.Text, true, 100, false, false, true);
            FieldValidate.ValidateString("Telefono oficina", txtTelefono.Text, true, 10);
            FieldValidate.ValidateString("Telefono celular", txtCelular.Text, true, 10);
            FieldValidate.ValidateString("Dirección", txtDireccion.Text, true, 100);
            FieldValidate.ValidateString("Dependencia", txtDependencia.Text, true, 100);
            FieldValidate.ValidateString("Email", txtEmail.Text, true, 100, true);
            ValidateDepartamentos();
        }

        public void ValidateDepartamentos()
        {
            var atLeastOne = false;
            foreach (GridViewRow currentRow in gvDepartamentos.Rows)
            {
                CheckBox checkDepartamento = (CheckBox)currentRow.FindControl("chkDepartamento");
                DropDownList cmbZonas = currentRow.FindControl("DropDownList1") as DropDownList;
                HiddenField idDepartamento = (HiddenField)currentRow.FindControl("hdCodigoDepartamento");

                if (checkDepartamento.Checked)
                {
                    atLeastOne = true;
                }
            }

            if (!atLeastOne)
                throw new ApplicationException("Debe seleccionar al menos un departamento.");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //Validar Seleccion de Operador
                int codOperador = Convert.ToInt32(ddlOperador.SelectedItem.Value);

                if (codOperador == 0)
                {
                    throw new ApplicationException("Debe Seleccionar un operador.");
                }

                ValidarDatos();

                var rutaArchivo = string.Empty;
                if (!fuArchivo.HasFile)
                {
                    throw new ApplicationException("La imagen del logo de la entidad es obligatorio!");
                }
                else
                {
                    rutaArchivo = UploadFile(fuArchivo);
                }

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades
                    .Entidad.ExistEntidadByName(txtNombre.Text, codOperador))
                    throw new ApplicationException("Existe una entidad con ese mismo nombre.");

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades
                    .Entidad.ExistEntidadByShortName(txtNombreCorto.Text, codOperador))
                    throw new ApplicationException("Existe una entidad con ese mismo nombre corto.");

                

                var newEntity = new Datos.EntidadInterventoria
                {
                    Nombre = txtNombre.Text.Trim(),
                    NombreCorto = txtNombreCorto.Text.Trim(),
                    NumeroPoliza = txtNumeroPoliza.Text.Trim(),
                    FechaPoliza = DateTime.Parse(txtFechaPoliza.Text),
                    PersonaACargo = txtPersonaACargo.Text.Trim(),
                    TelefonoOficina = txtTelefono.Text.Trim(),
                    TelefonoCelular = txtCelular.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Dependencia = txtDependencia.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    ImagenLogo = rutaArchivo.Trim(),
                    UsuarioCreacion = Usuario.IdContacto,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    codOperador = codOperador
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.Insert(newEntity);

                SetDepartamentos(newEntity.Id);
                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Entidades.aspx", true);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }

        public void SetDepartamentos(int idEntidad)
        {
            foreach (GridViewRow currentRow in gvDepartamentos.Rows)
            {
                CheckBox checkDepartamento = (CheckBox)currentRow.FindControl("chkDepartamento");
                DropDownList cmbZonas = currentRow.FindControl("DropDownList1") as DropDownList;
                HiddenField idDepartamento = (HiddenField)currentRow.FindControl("hdCodigoDepartamento");

                if (checkDepartamento.Checked)
                {
                    var departamentoZona = new Datos.EntidadDepartamento
                    {
                        IdZona = Convert.ToInt32(cmbZonas.SelectedValue) == 0 ? (Int32?)null : Convert.ToInt32(cmbZonas.SelectedValue),
                        IdDepartamento = Convert.ToInt32(idDepartamento.Value),
                        IdEntidad = idEntidad
                    };
                    Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Zonas.Zona.InsertOrUpdateDepartamento(departamentoZona);
                }
            }
        }

        protected string UploadFile(FileUpload _archivo)
        {
            string directorioDestino = "EntidadesInterventoria\\";
            string directorioBase = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
            string nombreArchivo = "LogoEntidad_" + DateTime.Now.GetShortDateOnlyNumbersWithUnderscore() + Path.GetExtension(_archivo.FileName);

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

            // ¿ Pesa mas de diez megas ?
            if (!(_archivo.PostedFile.ContentLength < 10485760))
                throw new ApplicationException("El archivo es muy pesado, maximo 10 megas.");

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

        public List<Datos.departamento> GetDepartamentos()
        {
            return Negocio.PlanDeNegocioV2.Utilidad.General.GetDepartamentos();
        }

        public List<Datos.Zona> GetZonas(int idDepartamento)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Zonas.Zona.Get(idDepartamento);
        }

        protected void gvProyectos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList cmbZonas = (e.Row.FindControl("DropDownList1") as DropDownList);

                HiddenField codigoDepartamento = (HiddenField)e.Row.FindControl("hdCodigoDepartamento");

                cmbZonas.DataSource = GetZonas(Convert.ToInt32(codigoDepartamento.Value));
                cmbZonas.DataBind();

                cmbZonas.Items.Insert(0, new ListItem("Ninguna", "0"));
            }
        }
    }
}