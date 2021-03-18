using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using System.IO;
using System.Configuration;
using Fonade.Account;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad
{
    public partial class UpdateEntidad : System.Web.UI.Page
    {        
        public int CodigoEntidad
        {
            get
            {
                if (Request.QueryString["codigo"] != null)
                    return Convert.ToInt32(Request.QueryString["codigo"]);
                else
                    return 0;
            }
            set { }
        }

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
                GetInfoEntidad();

                ValidateUsers();
            }
        }

        public void GetInfoEntidad()
        {
            try
            {
                var entidad = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.FirstOrDefault(CodigoEntidad);

                if (entidad != null)
                {
                    txtNombre.Text = entidad.Nombre.Trim();
                    txtNombreCorto.Text = entidad.NombreCorto.Trim();
                    txtNumeroPoliza.Text = entidad.NumeroPoliza.Trim();
                    txtFechaPoliza.Text = entidad.FechaPoliza.ToString("dd/MM/yyyy");
                    txtPersonaACargo.Text = entidad.PersonaACargo.Trim();
                    txtTelefono.Text = entidad.TelefonoOficina.Trim();
                    txtCelular.Text = entidad.TelefonoCelular.Trim();
                    txtDireccion.Text = entidad.Direccion.Trim();
                    txtDependencia.Text = entidad.Dependencia.Trim();
                    txtEmail.Text = entidad.Email.Trim();
                    imgLogo.ImageUrl = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + entidad.ImagenLogo.Trim();

                    string nomOperador = Negocio.PlanDeNegocioV2.Administracion.Interventoria
                                    .Entidades.Entidad.GetOperadores(entidad.codOperador)
                                    .First().NombreOperador;

                    txtOperador.Text = nomOperador;
                }
                else
                {
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Entidades.aspx");
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                btnAdicionar.Visible = false;
                lblError.Text = "Advertencia:" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error inesperado :" + ex.Message;
            }
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
            FieldValidate.ValidateString("Email", txtEmail.Text.Trim(), true, 100, true);
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
                ValidarDatos();

                var entidad = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.FirstOrDefault(CodigoEntidad);

                var rutaArchivo = string.Empty;
                                
                if (fuArchivo.HasFile)
                {
                    rutaArchivo = UploadFile(fuArchivo);
                }
                                    
                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.ExistEntidadByName(txtNombre.Text, CodigoEntidad))
                    throw new ApplicationException("Existe una entidad con ese mismo nombre.");

                if (Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.ExistEntidadByShortName(txtNombreCorto.Text, CodigoEntidad))
                    throw new ApplicationException("Existe una entidad con ese mismo nombre corto.");

                var newEntity = new Datos.EntidadInterventoria
                {
                    Id = entidad.Id,
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
                    FechaActualizacion = DateTime.Now
                };

                Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.Update(newEntity);

                SetDepartamentos(newEntity.Id);

                Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Entidades.aspx",true);
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
            Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Zonas.Zona.DeleteAllByEntidad(idEntidad);
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

        public List<Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.EntidadDepartamentoDTO> GetDepartamentos(Int32 idEntidad)
        {
            return Negocio.PlanDeNegocioV2.Administracion.Interventoria.Entidades.Entidad.GetDepartamentosByEntidad(idEntidad);
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
                HiddenField codigoZona = (HiddenField)e.Row.FindControl("hdCodigoZona");

                cmbZonas.DataSource = GetZonas(Convert.ToInt32(codigoDepartamento.Value));
                cmbZonas.DataBind();

                cmbZonas.Items.Insert(0, new ListItem("Ninguna", "0"));
                cmbZonas.ClearSelection();
                cmbZonas.Items.FindByValue(String.IsNullOrEmpty(codigoZona.Value) ? "0" : codigoZona.Value).Selected = true;
            }
        }
    }
}