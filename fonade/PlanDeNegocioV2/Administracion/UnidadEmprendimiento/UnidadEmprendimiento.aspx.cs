using Datos;
using Fonade.Clases;
using Fonade.Error;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Administracion.UnidadEmprendimiento
{
    public partial class UnidadEmprendimiento : Negocio.Base_Page
    {
        public int? IdInstitucion { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Boolean IsUpdate = false;
            try
            {
                if (Session["idUnidad"] != null)
                {
                    IdInstitucion = (int)Session["idUnidad"];

                    if (!Page.IsPostBack)
                        setDatosFormulario(IdInstitucion.Value);

                    IsUpdate = true;
                }
                else
                {
                    IsUpdate = false;
                }

                if (IsUpdate)
                {
                    lblTituloActualizar.Visible = true;
                    btnActualizarUnidad.Visible = true;
                    divMotivoCambio.Visible = true;         
                }
                else
                {
                    lblTituloCrear.Visible = true;
                    btnCrearUnidad.Visible = true;
                }

                lblError.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !, detalle : " + ex.Message.Replace("'", string.Empty);
            }
        }

        private void setDatosFormulario(int idInstitucion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.Institucions.FirstOrDefault(filter => filter.Id_Institucion.Equals(idInstitucion));

                if (entity == null)
                    throw new ApplicationException("No se pudo obtner la información de la institución.");

                txtNombreInstitucion.Text = entity.NomInstitucion;
                txtNombreUnidad.Text = entity.NomUnidad;
                txtNitInstitucion.Text = entity.Nit.ToString();
                txtDireccion.Text = entity.Direccion;
                txtMotivoCambioJefeUnidad.Text = entity.MotivoCambioJefe != null ? entity.MotivoCambioJefe : "";

                cmbTipoInstitucion.DataBind();
                cmbTipoInstitucion.ClearSelection();
                cmbTipoInstitucion.Items.FindByValue(entity.CodTipoInstitucion.ToString()).Selected = true;

                var ciudad = db.Ciudad.First(filter => filter.Id_Ciudad.Equals(entity.CodCiudad));

                cmbDepartamento.DataBind();
                cmbDepartamento.ClearSelection();
                cmbDepartamento.Items.FindByValue(ciudad.CodDepartamento.ToString()).Selected = true;
                

                cmbCiudad.DataBind();
                cmbCiudad.ClearSelection();
                cmbCiudad.Items.FindByValue(ciudad.Id_Ciudad.ToString()).Selected = true;                

                txtSitioWeb.Text = entity.WebSite;
                txtCriterio.Text = entity.CriteriosSeleccion;
                txtTelefono.Text = entity.Telefono;

                var jefeUnidadActual = (from jefesDeUnidad in db.InstitucionContacto
                                        join contactos in db.Contacto on jefesDeUnidad.CodContacto equals contactos.Id_Contacto
                                        where jefesDeUnidad.FechaFin == null && jefesDeUnidad.CodInstitucion.Equals(idInstitucion)
                                        select new
                                        {
                                            codigoJefeUnidad = contactos.Id_Contacto,
                                            nombreCompleto = contactos.Nombres + " " + contactos.Apellidos,
                                            MotivoCambioJefe = jefesDeUnidad.MotivoCambioJefe
                                        }).FirstOrDefault();

                if (jefeUnidadActual != null) {
                    hfCodigoJefeDeUnidad.Value = jefeUnidadActual.codigoJefeUnidad.ToString();
                    lblJefeUnidad.Text = jefeUnidadActual.nombreCompleto;                    
                    btnCambiarDatos.Visible = true;
                }
            }
        }

        public List<TipoInstitucion> getTipoUnidad()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tipoInstitucion in db.TipoInstitucions
                                select tipoInstitucion
                                ).ToList();

                return entities;
            }
        }

        public List<Departamento> getDepartamentos()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from departamentoColombia in db.departamento
                                select new Departamento
                                {
                                    Id = departamentoColombia.Id_Departamento,
                                    Nombre = departamentoColombia.NomDepartamento
                                }
                                    ).ToList();
                return entities;
            }
        }

        public List<Ciudad> getCiudades(int codigoDepartamento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from ciudadesColombia in db.Ciudad
                                where ciudadesColombia.CodDepartamento == codigoDepartamento
                                select new Ciudad
                                {
                                    Id = ciudadesColombia.Id_Ciudad,
                                    Nombre = ciudadesColombia.NomCiudad,
                                    CodigoDepartamento = ciudadesColombia.CodDepartamento
                                }
                                    ).ToList();
                return entities;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cerrarForm();
        }

        private void cerrarForm()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }

        protected void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    FieldValidate.ValidateString("Tipo de unidad", cmbTipoInstitucion.SelectedValue,true);
                    FieldValidate.ValidateString("Nombre de unidad", txtNombreUnidad.Text,true,80);
                    FieldValidate.ValidateString("Nombre de centro o institución", txtNombreInstitucion.Text, true, 80);
                    FieldValidate.ValidateString("Nit", txtNitInstitucion.Text, true, 18);
                    FieldValidate.ValidateString("Departamento", cmbDepartamento.SelectedValue, true);
                    FieldValidate.ValidateString("Ciudad", cmbCiudad.SelectedValue, true);
                    FieldValidate.ValidateString("Dirección", txtDireccion.Text, true);
                    FieldValidate.ValidateString("Sitio web", txtSitioWeb.Text, false,100);
                    FieldValidate.ValidateString("Teléfono", txtTelefono.Text, true);
                    FieldValidate.ValidateString("Razón de adición o cambio de Jefe de Unidad", txtMotivoCambioJefeUnidad.Text, true);

                    FieldValidate.ValidateString("Criterio de selección", txtCriterio.Text, true);
                    FieldValidate.ValidateString("Jefe de unidad", hfCodigoJefeDeUnidad.Value, true);

                    if (db.Institucions.Any(filter => filter.NomUnidad.Contains(txtNombreUnidad.Text) && filter.NomInstitucion.Contains(txtNombreInstitucion.Text)))
                        throw new ApplicationException("Ya existe una institución con ese nombre y unidad");

                    var newEntity = new Datos.Institucion
                    {
                        NomInstitucion = txtNombreInstitucion.Text.Trim(),
                        NomUnidad = txtNombreUnidad.Text.Trim(),
                        Nit = Convert.ToDecimal(txtNitInstitucion.Text.Trim().Replace(".", "")),
                        Direccion = txtDireccion.Text.Trim(),
                        Telefono = txtTelefono.Text,
                        Fax = string.Empty,
                        CodCiudad = int.Parse(cmbCiudad.SelectedValue),
                        Inactivo = false,
                        CriteriosSeleccion = txtDireccion.Text.Trim(),
                        CodTipoInstitucion = Convert.ToByte(cmbTipoInstitucion.SelectedValue.ToString()),
                        WebSite = txtTelefono.Text
                    };

                    db.Institucions.InsertOnSubmit(newEntity);
                    db.SubmitChanges();

                    AddJefeUnidadAInstitucion(Convert.ToInt32(hfCodigoJefeDeUnidad.Value), newEntity.Id_Institucion);
                    
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.__doPostBack('jefeUnidad','jefeUnidad');window.close();", true);                    
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error, detalle : " + ex.Message;
            }
        }

        public void AddJefeUnidadAInstitucion(int codigoContacto, int codigoInstitucion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = db.InstitucionContacto.Where(filter => filter.CodInstitucion.Equals(codigoInstitucion)
                                                                             && filter.FechaFin == null
                                                                  ).ToList();                
                if (!entities.Any())
                {
                    var entity = new InstitucionContacto
                    {
                        CodContacto = codigoContacto,
                        CodInstitucion = codigoInstitucion,
                        FechaInicio = DateTime.Now,
                        MotivoCambioJefe = txtMotivoCambioJefeUnidad.Text
                    };

                    db.InstitucionContacto.InsertOnSubmit(entity);

                    var isJefeUnidad = db.GrupoContactos.Any(filter => filter.CodContacto.Equals(codigoContacto) && filter.CodGrupo.Equals(Constantes.CONST_JefeUnidad));

                    if (!isJefeUnidad)
                    {
                        var nuevoGrupoJefe = new GrupoContacto
                        {
                            CodContacto = codigoContacto,
                            CodGrupo = Constantes.CONST_JefeUnidad
                        };

                        db.GrupoContactos.InsertOnSubmit(nuevoGrupoJefe);
                    }

                    var infoJefeUnidad = db.Contacto.FirstOrDefault(filter => filter.Id_Contacto.Equals(codigoContacto));

                    if (infoJefeUnidad != null)
                    {
                        infoJefeUnidad.Inactivo = false;
                        enviarEmail("Credenciales de acceso a fondo emprender jefe unidad", infoJefeUnidad.Email, infoJefeUnidad.Email, infoJefeUnidad.Clave);
                        enviarEmail("Credenciales de acceso a fondo emprender jefe unidad", usuario.Email, infoJefeUnidad.Email, infoJefeUnidad.Clave, true);
                    }

                    db.SubmitChanges();                                           
                }
                else
                {
                    var isContactoActive = false;            
                    foreach (var entity in entities)
                    {
                        if (!entity.CodContacto.Equals(codigoContacto))
                        {
                            entity.FechaFin = DateTime.Now;
                            entity.MotivoCambioJefe = txtMotivoCambioJefeUnidad.Text;

                            var contactoJefe = db.Contacto.First(filter => filter.Id_Contacto.Equals(entity.CodContacto));
                            contactoJefe.CodInstitucion = Constantes.CONST_UnidadTemporal;
                        }
                        else
                        {
                            isContactoActive = true;
                            entity.MotivoCambioJefe = txtMotivoCambioJefeUnidad.Text;

                            var contactoJefe = db.Contacto.First(filter => filter.Id_Contacto.Equals(entity.CodContacto));
                            contactoJefe.CodInstitucion = codigoInstitucion;
                            contactoJefe.Inactivo = false;

                            if (contactoJefe != null)
                            {
                                enviarEmail("Credenciales de acceso a fondo emprender jefe unidad", contactoJefe.Email, contactoJefe.Email, contactoJefe.Clave);
                                enviarEmail("Credenciales de acceso a fondo emprender jefe unidad", usuario.Email, contactoJefe.Email, contactoJefe.Clave, true);
                            }
                        }
                    }

                    if (!isContactoActive) {
                        var entity = new InstitucionContacto
                        {
                            CodContacto = codigoContacto,
                            CodInstitucion = codigoInstitucion,
                            FechaInicio = DateTime.Now,
                            MotivoCambioJefe = txtMotivoCambioJefeUnidad.Text
                        };                        
                        db.InstitucionContacto.InsertOnSubmit(entity);

                        var contactoJefe = db.Contacto.First(filter => filter.Id_Contacto.Equals(codigoContacto));
                        contactoJefe.CodInstitucion = codigoInstitucion;
                        contactoJefe.Inactivo = false;

                        var isJefeUnidad = db.GrupoContactos.Any(filter => filter.CodContacto.Equals(codigoContacto) &&  filter.CodGrupo.Equals(Constantes.CONST_JefeUnidad));
                        if (!isJefeUnidad) {
                            var nuevoGrupoJefe = new GrupoContacto {
                                CodContacto = codigoContacto,
                                CodGrupo = Constantes.CONST_JefeUnidad
                            };

                            db.GrupoContactos.InsertOnSubmit(nuevoGrupoJefe);
                        }

                        if (contactoJefe != null)
                        {
                            enviarEmail("Credenciales de acceso a fondo emprender jefe unidad", contactoJefe.Email, contactoJefe.Email, contactoJefe.Clave);
                            enviarEmail("Credenciales de acceso a fondo emprender jefe unidad", usuario.Email, contactoJefe.Email, contactoJefe.Clave, true);
                        }
                    }
                    db.SubmitChanges();
                }
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                var IdInstitucion = (int)Session["idUnidad"];
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    FieldValidate.ValidateString("Tipo de unidad", cmbTipoInstitucion.SelectedValue, true);
                    FieldValidate.ValidateString("Nombre de unidad", txtNombreUnidad.Text, true, 80);
                    FieldValidate.ValidateString("Nombre de centro o institución", txtNombreInstitucion.Text, true, 80);
                    FieldValidate.ValidateString("Nit", txtNitInstitucion.Text, true, 18);
                    FieldValidate.ValidateString("Departamento", cmbDepartamento.SelectedValue, true);
                    FieldValidate.ValidateString("Ciudad", cmbCiudad.SelectedValue, true);
                    FieldValidate.ValidateString("Dirección", txtDireccion.Text, true);
                    FieldValidate.ValidateString("Sitio web", txtSitioWeb.Text, false, 100);
                    FieldValidate.ValidateString("Teléfono", txtTelefono.Text, true);
                    FieldValidate.ValidateString("Razón de adición o cambio de Jefe de Unidad", txtMotivoCambioJefeUnidad.Text, true);
                    FieldValidate.ValidateString("Criterio de selección", txtCriterio.Text, true);
                    FieldValidate.ValidateString("Jefe de unidad", hfCodigoJefeDeUnidad.Value, true);

                    if (db.Institucions.Any(filter => filter.NomUnidad.Contains(txtNombreUnidad.Text) && filter.NomInstitucion.Contains(txtNombreInstitucion.Text) && !filter.Id_Institucion.Equals(IdInstitucion)))
                        throw new ApplicationException("Ya existe una institución con ese nombre y unidad");

                    var institucionActual = db.Institucions.First( filter =>
                                                                            filter.Id_Institucion.Equals(IdInstitucion));

                    institucionActual.NomInstitucion =

                    institucionActual.NomInstitucion = txtNombreInstitucion.Text.Trim();
                    institucionActual.NomUnidad = txtNombreUnidad.Text.Trim();
                    institucionActual.Nit = Convert.ToDecimal(txtNitInstitucion.Text.Trim().Replace(".", ""));
                    institucionActual.Direccion = txtDireccion.Text.Trim();
                    institucionActual.Telefono = txtTelefono.Text;
                    institucionActual.Fax = string.Empty;
                    institucionActual.CodCiudad = int.Parse(cmbCiudad.SelectedValue);
                    institucionActual.Inactivo = false;
                    institucionActual.CriteriosSeleccion = txtDireccion.Text.Trim();
                    institucionActual.CodTipoInstitucion = Convert.ToByte(cmbTipoInstitucion.SelectedValue.ToString());
                    institucionActual.WebSite = txtTelefono.Text;
                    institucionActual.MotivoCambioJefe = txtMotivoCambioJefeUnidad.Text;

                    AddJefeUnidadAInstitucion(Convert.ToInt32(hfCodigoJefeDeUnidad.Value), IdInstitucion);

                    db.SubmitChanges();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.__doPostBack('jefeUnidad','jefeUnidad');window.close();", true);
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Error, detalle : " + ex.Message;
            }
        }
        public void mostrarModal()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "show", "$(function () { $('#" + ModalBuscarJefe.ClientID + "').modal('show'); });", true);
            UpdatePanel2.Update();

            nuevoJefeUnidad.Visible = false;
            buscador.Visible = true;
            listadoUsuarios.Visible = true;
            lblErrorCrearJefeUnidad.Visible = false;
            txtNombreJefeUnidad.Text = string.Empty;
            txtApellidoJefeUnidad.Text = string.Empty;
            txtIdentificacionJefeUnidad.Text = string.Empty;
            txtEmailJefeUnidad.Text = string.Empty;
            txtCargoJefeUnidad.Text = string.Empty;
            txtTelefonoPersonalJefeUnidad.Text = string.Empty;
        }

        protected void btnBuscarJefeUnidad_Click(object sender, EventArgs e)
        {
            mostrarModal();
        }

        protected void btnCerrarBuscarJefe_Click(object sender, EventArgs e)
        {
            cerrarModalBuscarJefe();
        }

        public void cerrarModalBuscarJefe()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalBuscarJefe.ClientID + "').modal('hide'); });", true);
            UpdatePanel1.Update();
        }

        public List<BusquedaJefe> getBusquedaJefe(string identificacion)
        {
            int x = 0;
            if (Int32.TryParse(identificacion, out x))
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entities = (from contacto in db.Contacto
                                    from grupoContacto in db.GrupoContactos.Where(filterContacto => filterContacto.CodContacto.Equals(contacto.Id_Contacto)).DefaultIfEmpty()
                                    from grupo in db.Grupos.Where(filterGrupo => filterGrupo.Id_Grupo.Equals(grupoContacto.CodGrupo)).DefaultIfEmpty()
                                    from institucionJefeUnidad in db.InstitucionContacto.Where(filterInstitucion => filterInstitucion.CodContacto.Equals(contacto.Id_Contacto) && filterInstitucion.FechaFin == null).DefaultIfEmpty()
                                    from instituciones in db.Institucions.Where(filterInstituciones => filterInstituciones.Id_Institucion.Equals(institucionJefeUnidad.CodInstitucion)).DefaultIfEmpty()
                                    where contacto.Identificacion.Equals(identificacion)
                                    select new BusquedaJefe
                                    {
                                        Id = contacto.Id_Contacto,
                                        Nombre = contacto.Nombres,
                                        Apellido = contacto.Apellidos,
                                        Identificacion = contacto.Identificacion,
                                        CodigoGrupo = grupo == null ? 0 : grupo.Id_Grupo,
                                        NombreGrupo = grupo.NomGrupo == null ? "Sin rol" : grupo.NomGrupo,
                                        InstitucionActual = instituciones == null ? "N/A" : instituciones.NomInstitucion + " (" + instituciones.NomUnidad + ")",
                                        EstadoJefe = contacto.Inactivo.Equals(false) ? "Activo" : "Inactivo" 
                                    }
                                   ).ToList();

                    return entities;
                }
            }
            else
            {
                return new List<BusquedaJefe>();
            }
        }

        protected void btnBuscarJefe_Click(object sender, EventArgs e)
        {
            gvUsuarios.DataBind();
        }

        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("asignar"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros = e.CommandArgument.ToString().Split(';');

                    int codigoContacto = Convert.ToInt32(parametros[0]);
                    string nombreContacto = parametros[1];

                    hfCodigoJefeDeUnidad.Value = codigoContacto.ToString();
                    lblJefeUnidad.Text = nombreContacto;

                    cerrarModalBuscarJefe();
                }
            }
        }

        protected void btnCrearJefe_Click(object sender, EventArgs e)
        {
            nuevoJefeUnidad.Visible = true;
            buscador.Visible = false;
            listadoUsuarios.Visible = false;
            lblErrorCrearJefeUnidad.Visible = false;
            lblTitleJefeUnidad.Text = "Crear jefe de unidad";
            btnActualizarJefeUnidad.Visible = false;
            btnCrearJefeUnidad.Visible = true;
        }

        protected void btnCancelarCrearJefeUnidad_Click(object sender, EventArgs e)
        {
            nuevoJefeUnidad.Visible = false;
            buscador.Visible = true;
            listadoUsuarios.Visible = true;
            lblErrorCrearJefeUnidad.Visible = false;
            txtNombreJefeUnidad.Text = string.Empty;
            txtApellidoJefeUnidad.Text = string.Empty;
            txtIdentificacionJefeUnidad.Text = string.Empty;
            txtEmailJefeUnidad.Text = string.Empty;            
            txtCargoJefeUnidad.Text = string.Empty;
            txtTelefonoPersonalJefeUnidad.Text = string.Empty;            
        }

        protected void btnCrearJefeUnidad_Click(object sender, EventArgs e)
        {
            try
            {
                validateDatosJefeUnidad();

                var nombreJefe = txtNombreJefeUnidad.Text;
                var apellidoJefe = txtApellidoJefeUnidad.Text;
                var codigoTipoIdentificacion = cmbTipoDocumento.SelectedValue;
                var identificacionJefe = txtIdentificacionJefeUnidad.Text;
                var emailJefe = txtEmailJefeUnidad.Text;                                                              
                var cargoJefeUnidad = txtCargoJefeUnidad.Text;
                var telefonoJefeUnidad = txtTelefonoPersonalJefeUnidad.Text;
                var codigoCiudadJefe = Convert.ToInt32(cmbCiudadJefe.SelectedValue);

                var jefeUnidad = new JefeUnidad
                {
                    Nombre = nombreJefe,
                    Apellido = apellidoJefe,
                    Identificacion = Convert.ToInt32(identificacionJefe),
                    Email = emailJefe,                    
                    Telefono = telefonoJefeUnidad,
                    Cargo = cargoJefeUnidad,
                    TipoIdentificacion = Convert.ToInt16(codigoTipoIdentificacion),
                    CodigoCiudadNacimiento = codigoCiudadJefe                    
                };

                if (!validarContactoExistente(jefeUnidad))
                {
                    insertarJefeUnidad(jefeUnidad);
                    insertarGrupoContacto(jefeUnidad);
                }
                else
                {
                    throw new ApplicationException("Existe un usuario con esa cedula o email.");
                }

                hfCodigoJefeDeUnidad.Value = jefeUnidad.Id.ToString();
                lblJefeUnidad.Text = jefeUnidad.NombreCompleto;

                lblErrorCrearJefeUnidad.Visible = false;

                btnCancelarCrearJefeUnidad_Click(btnCancelarCrearJefeUnidad, new EventArgs());

                cerrarModalBuscarJefe();
                btnCambiarDatos.Visible = true;
            }
            catch (ApplicationException ex)
            {
                lblErrorCrearJefeUnidad.Visible = true;
                lblErrorCrearJefeUnidad.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblErrorCrearJefeUnidad.Visible = true;
                lblErrorCrearJefeUnidad.Text = "Sucedio un error inesperado al crear el jefe de unidad.";

                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
            }
        }

        protected void validateDatosJefeUnidad()
        {
            FieldValidate.ValidateString("Nombre del jefe de unidad", txtNombreJefeUnidad.Text, true);
            FieldValidate.ValidateString("Apellido del jefe de unidad", txtApellidoJefeUnidad.Text, true);
            FieldValidate.ValidateNumeric("Identificación del jefe de unidad", txtIdentificacionJefeUnidad.Text, true);
            FieldValidate.ValidateString("Email del jefe de unidad", txtEmailJefeUnidad.Text, true, 300, true);                                               
            FieldValidate.ValidateString("Cargo del funcionario", txtCargoJefeUnidad.Text, true);
            FieldValidate.ValidateString("Telefono del funcionario", txtTelefonoPersonalJefeUnidad.Text, true);            
        }

        public List<TipoIdentificacion> getTiposIdentificacion()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var tipoidentificación = (from tipo in db.TipoIdentificacions
                                          select new TipoIdentificacion
                                          {
                                              Id = tipo.Id_TipoIdentificacion,
                                              Nombre = tipo.NomTipoIdentificacion
                                          }
                                    ).ToList();
                return tipoidentificación;
            }
        }

        protected bool validarContactoExistente(JefeUnidad jefeUnidad, bool exist = false)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!exist)
                {
                    Datos.Contacto contactoExiste = db.Contacto.FirstOrDefault(contacto => contacto.Email == jefeUnidad.Email || contacto.Identificacion == jefeUnidad.Identificacion);

                    //Si existe algun contacto con ese email o identificacion
                    return contactoExiste != null;
                }               
                else{
                    Datos.Contacto contactoExiste = db.Contacto.FirstOrDefault(contacto => (contacto.Email == jefeUnidad.Email || contacto.Identificacion == jefeUnidad.Identificacion) && !contacto.Id_Contacto.Equals(jefeUnidad.Id));

                    return contactoExiste != null;
                }
            }
        }
                
        protected void insertarJefeUnidad(JefeUnidad jefeUnidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                Datos.Contacto nuevoJefeUnidad = new Datos.Contacto
                {
                    Nombres = jefeUnidad.Nombre,
                    Apellidos = jefeUnidad.Apellido,
                    CodTipoIdentificacion = jefeUnidad.TipoIdentificacion,
                    Identificacion = jefeUnidad.Identificacion,
                    Email = jefeUnidad.Email,
                    Clave = GeneraClave(),
                    CodInstitucion = Constantes.CONST_UnidadTemporal,                                                            
                    CodCiudad = jefeUnidad.CodigoCiudadNacimiento,
                    Telefono = jefeUnidad.Telefono,                    
                    Inactivo = false,
                    Cargo = jefeUnidad.Cargo
                };

                db.Contacto.InsertOnSubmit(nuevoJefeUnidad);
                db.SubmitChanges();

                jefeUnidad.Id = nuevoJefeUnidad.Id_Contacto;                
            }
        }

        protected void actualizarJefeUnidad(JefeUnidad jefeUnidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var jefeUnidadActual = (from contactos in db.Contacto
                                        where contactos.Id_Contacto.Equals(jefeUnidad.Id)
                                        select contactos).FirstOrDefault();

                jefeUnidadActual.Nombres = jefeUnidad.Nombre;
                jefeUnidadActual.Apellidos = jefeUnidad.Apellido;
                jefeUnidadActual.Identificacion = jefeUnidad.Identificacion;
                jefeUnidadActual.Email = jefeUnidad.Email;
                jefeUnidadActual.Telefono = jefeUnidad.Telefono;
                jefeUnidadActual.Cargo = jefeUnidad.Cargo;
                jefeUnidadActual.CodTipoIdentificacion = jefeUnidad.TipoIdentificacion;
                jefeUnidadActual.CodCiudad = jefeUnidad.CodigoCiudadNacimiento;                

                db.SubmitChanges();
            }
        }

        protected void insertarGrupoContacto(JefeUnidad jefeUnidad)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!db.GrupoContactos.Any(contacto => contacto.CodContacto == jefeUnidad.Id))
                {
                    Datos.GrupoContacto grupoContacto = new Datos.GrupoContacto
                    {
                        CodGrupo = Constantes.CONST_JefeUnidad,
                        CodContacto = jefeUnidad.Id
                    };

                    db.GrupoContactos.InsertOnSubmit(grupoContacto);
                    db.SubmitChanges();
                }
            }
        }

        private bool enviarEmail(string asunto, string emailRemitente, string emailDestinatario, string password, bool isSupervisor = false)
        {
            bool errorMessage = false;
            string bodyTemplate = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta name=\"viewport\" content=\"width=device-width\"/><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"/><title>Fondo Emprender</title><style type=\"text/css\">*,.collapse{padding:0}.btn,.social .soc-btn{text-align:center;font-weight:700}.btn,ul.sidebar li a{text-decoration:none;cursor:pointer}.container,table.footer-wrap{clear:both!important}*{margin:0;font-family:\"Helvetica Neue\",Helvetica,Helvetica,Arial,sans-serif}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100%!important;height:100%}.content table,table.body-wrap,table.footer-wrap,table.head-wrap{width:100%}a{color:#2BA6CB}.btn{color:#FFF;background-color:#666;padding:10px 16px;margin-right:10px;display:inline-block}p.callout{padding:15px;background-color:#ECF8FF;margin-bottom:15px}.callout a{font-weight:700;color:#2BA6CB}table.social{background-color:#ebebeb}.social .soc-btn{padding:3px 7px;font-size:12px;margin-bottom:10px;text-decoration:none;color:#FFF;display:block}a.fb{background-color:#3B5998!important}a.tw{background-color:#1daced!important}a.gp{background-color:#DB4A39!important}a.ms{background-color:#000!important}.sidebar .soc-btn{display:block;width:100%}.header.container table td.logo{padding:15px}.header.container table td.label{padding:15px 15px 15px 0}.footer-wrap .container td.content p{border-top:1px solid #d7d7d7;padding-top:15px;font-size:10px;font-weight:700}h1,h2{font-weight:200}h1,h2,h3,h4,h5,h6{font-family:HelveticaNeue-Light,\"Helvetica Neue Light\",\"Helvetica Neue\",Helvetica,Arial,\"Lucida Grande\",sans-serif;line-height:1.1;margin-bottom:15px;color:#000}h1 small,h2 small,h3 small,h4 small,h5 small,h6 small{font-size:60%;color:#6f6f6f;line-height:0;text-transform:none}h1{font-size:44px}h2{font-size:37px}h3,h4{font-weight:500}h3{font-size:27px}h4{font-size:23px}h5,h6{font-weight:900}h5{font-size:17px}h6,p,ul{font-size:14px}h6{text-transform:uppercase;color:#444}.collapse{margin:0!important}p,ul{margin-bottom:10px;font-weight:400;line-height:1.6}p.lead{font-size:17px}p.last{margin-bottom:0}ul li{margin-left:5px;list-style-position:inside}ul.sidebar li,ul.sidebar li a{display:block;margin:0}ul.sidebar{background:#ebebeb;display:block;list-style-type:none}ul.sidebar li a{color:#666;padding:10px 16px;border-bottom:1px solid #777;border-top:1px solid #FFF}.column tr td,.content{padding:15px}ul.sidebar li a.last{border-bottom-width:0}ul.sidebar li a h1,ul.sidebar li a h2,ul.sidebar li a h3,ul.sidebar li a h4,ul.sidebar li a h5,ul.sidebar li a h6,ul.sidebar li a p{margin-bottom:0!important}.container{display:block!important;max-width:600px!important;margin:0 auto!important}.content{max-width:600px;margin:0 auto;display:block}.column{width:300px;float:left}.column-wrap{padding:0!important;margin:0 auto;max-width:600px!important}.column table{width:100%}.social .column{width:280px;min-width:279px;float:left}.clear{display:block;clear:both}@media only screen and (max-width:600px){a[class=btn]{display:block!important;margin-bottom:10px!important;background-image:none!important;margin-right:0!important}div[class=column]{width:auto!important;float:none!important}table.social div[class=column]{width:auto!important}}</style></head> <body bgcolor=\"#FFFFFF\"><table class=\"head-wrap\" bgcolor=\"#FFFFFF\"><tr><td></td><td class=\"header container\" ><div class=\"content\"><table bgcolor=\"#FFFFFF\"><tr><td><img src=\"{{logo}}\"/></td><td align=\"right\"><h6 class=\"collapse\"></h6></td></tr></table></div></td><td></td></tr></table><table class=\"body-wrap\"><tr><td></td><td class=\"container\" bgcolor=\"#FFFFFF\"><div class=\"content\"><table><tr><td><h3>Hola, Credenciales de acceso a Fondo Emprender para jefe de unidad</h3><p class=\"lead\"> A continuación encontrara las credenciales para entrar a la plataforma.</p><table align=\"left\" class=\"column\"><tr><td><p class=\"\"><strong> Credenciales de acceso : </strong></p><p>Rol : <strong>{{rol}}</strong><br/>Email : <strong>{{email}}</strong><br/> Clave : <strong>{{clave}}</strong> </p></td></tr></table></td></tr></table></div></td><td></td></tr></table></body></html>";
            string urlLogoFondoEmprender = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

            MailMessage mail;
            mail = new MailMessage();

            if (isSupervisor)
                mail.To.Add(new MailAddress(emailRemitente));
            else
                mail.To.Add(new MailAddress(emailDestinatario));
            mail.From = new MailAddress(emailRemitente);
            mail.Subject = asunto;
            mail.Body = bodyTemplate.ReplaceWord("{{rol}}", "Jefe de unidad").ReplaceWord("{{email}}", emailDestinatario).ReplaceWord("{{clave}}", password).ReplaceWord("{{logo}}", urlLogoFondoEmprender);
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            var smtp = ConfigurationManager.AppSettings.Get("SMTP");
            var port = int.Parse(ConfigurationManager.AppSettings.Get("SMTP_UsedPort"));
            SmtpClient client = new SmtpClient(smtp, port);
            using (client)
            {
                var usuarioEmail = ConfigurationManager.AppSettings.Get("SMTPUsuario");
                var passwordEmail = ConfigurationManager.AppSettings.Get("SMTPPassword");
                client.Credentials = new System.Net.NetworkCredential(usuarioEmail, passwordEmail);
                client.EnableSsl = false;
                //client.
                client.Send(mail);
                errorMessage = true;
            }
            return errorMessage;
        }

        protected void btnCambiarDatos_Click(object sender, EventArgs e)
        {
            mostrarModalCambioDeDatos();
        }

        public void mostrarModalCambioDeDatos()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "show", "$(function () { $('#" + ModalBuscarJefe.ClientID + "').modal('show'); });", true);
            UpdatePanel2.Update();

            ShowEditarJefeUnidad();
        }

        protected void ShowEditarJefeUnidad()
        {
            nuevoJefeUnidad.Visible = true;
            buscador.Visible = false;
            listadoUsuarios.Visible = false;
            lblErrorCrearJefeUnidad.Visible = false;
            btnCrearJefeUnidad.Visible = false;
            btnActualizarJefeUnidad.Visible = true;
            lblTitleJefeUnidad.Text = "Actualizar jefe de unidad";
            GetJefeUnidad(Convert.ToInt32(hfCodigoJefeDeUnidad.Value));
        }

        protected void btnActualizarJefeUnidad_Click(object sender, EventArgs e)
        {
            try
            {
                validateDatosJefeUnidad();

                var nombreJefe = txtNombreJefeUnidad.Text;
                var apellidoJefe = txtApellidoJefeUnidad.Text;
                var codigoTipoIdentificacion = cmbTipoDocumento.SelectedValue;
                var identificacionJefe = txtIdentificacionJefeUnidad.Text;
                var emailJefe = txtEmailJefeUnidad.Text;
                var cargoJefeUnidad = txtCargoJefeUnidad.Text;
                var telefonoJefeUnidad = txtTelefonoPersonalJefeUnidad.Text;
                var codigoCiudadJefe = Convert.ToInt32(cmbCiudadJefe.SelectedValue);

                var jefeUnidad = new JefeUnidad
                {
                    Id = Convert.ToInt32(hfCodigoJefeDeUnidad.Value),
                    Nombre = nombreJefe,
                    Apellido = apellidoJefe,
                    Identificacion = Convert.ToInt32(identificacionJefe),
                    Email = emailJefe,
                    Telefono = telefonoJefeUnidad,
                    Cargo = cargoJefeUnidad,
                    TipoIdentificacion = Convert.ToInt16(codigoTipoIdentificacion),
                    CodigoCiudadNacimiento = codigoCiudadJefe
                };

                if (!validarContactoExistente(jefeUnidad,true))
                {
                    actualizarJefeUnidad(jefeUnidad);
                    insertarGrupoContacto(jefeUnidad);
                }
                else
                {
                    throw new ApplicationException("Existe un usuario con esa cedula o email.");
                }

                hfCodigoJefeDeUnidad.Value = jefeUnidad.Id.ToString();
                lblJefeUnidad.Text = jefeUnidad.NombreCompleto;

                lblErrorCrearJefeUnidad.Visible = false;

                btnCancelarCrearJefeUnidad_Click(btnCancelarCrearJefeUnidad, new EventArgs());

                cerrarModalBuscarJefe();
            }
            catch (ApplicationException ex)
            {
                lblErrorCrearJefeUnidad.Visible = true;
                lblErrorCrearJefeUnidad.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblErrorCrearJefeUnidad.Visible = true;
                lblErrorCrearJefeUnidad.Text = "Sucedio un error inesperado al crear el jefe de unidad.";
            }
        }

        protected void GetJefeUnidad(int codigoContacto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var jefeUnidadActual = (from contactos in db.Contacto
                                        where contactos.Id_Contacto.Equals(codigoContacto)
                                        select new JefeUnidad
                                        {
                                            Id = contactos.Id_Contacto,
                                            Identificacion = contactos.Identificacion,
                                            Nombre = contactos.Nombres,
                                            Apellido = contactos.Apellidos,
                                            Email = contactos.Email,
                                            TipoIdentificacion = contactos.CodTipoIdentificacion,
                                            CodigoCiudadNacimiento = contactos.CodCiudad.GetValueOrDefault(0),
                                            Cargo = contactos.Cargo,
                                            Telefono = contactos.Telefono                                                                                                                      
                                        }).FirstOrDefault();

                if (jefeUnidadActual != null)
                {
                    txtNombreJefeUnidad.Text = jefeUnidadActual.Nombre;
                    txtApellidoJefeUnidad.Text = jefeUnidadActual.Apellido;                    
                    cmbTipoDocumento.DataBind();
                    cmbTipoDocumento.ClearSelection();
                    cmbTipoDocumento.Items.FindByValue(jefeUnidadActual.TipoIdentificacion.ToString()).Selected = true;
                    txtIdentificacionJefeUnidad.Text = jefeUnidadActual.Identificacion.ToString();
                    txtEmailJefeUnidad.Text = jefeUnidadActual.Email;
                    txtCargoJefeUnidad.Text = jefeUnidadActual.Cargo;
                    txtTelefonoPersonalJefeUnidad.Text = jefeUnidadActual.Telefono;

                    var ciudad = db.Ciudad.First(filter => filter.Id_Ciudad.Equals(jefeUnidadActual.CodigoCiudadNacimiento));

                    cmbDepartamentoUnidad.DataBind();
                    cmbDepartamentoUnidad.ClearSelection();
                    cmbDepartamentoUnidad.Items.FindByValue(ciudad.CodDepartamento.ToString()).Selected = true;


                    cmbCiudadJefe.DataBind();
                    cmbCiudadJefe.ClearSelection();
                    cmbCiudadJefe.Items.FindByValue(jefeUnidadActual.CodigoCiudadNacimiento.ToString()).Selected = true;
                }                
            }
        }
    }

    public class Departamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    public class Ciudad
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int CodigoDepartamento { get; set; }
    }
    public class TipoIdentificacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }
    public class BusquedaJefe
    {
        public Int32 Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            }
            set { }
        }
        public double Identificacion { get; set; }
        public int CodigoGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public Boolean AllowAsignarJefe
        {
            get
            {
                var allow = false;

                if (CodigoGrupo.Equals(0))
                {
                    allow = true;
                }

                if (CodigoGrupo.Equals(Constantes.CONST_JefeUnidad))
                {
                    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        allow = !db.InstitucionContacto.Any(filter => filter.CodContacto.Equals(Id)
                                                                      && filter.FechaFin.Equals(null));
                    }
                }

                return allow;
            }
            set { }
        }
        public string linkAsignar
        {
            get
            {
                return AllowAsignarJefe ? "Asignar usuario a esta unidad" : "No se puede asignar este usuario";
            }
            set { }
        }                
        public string InstitucionActual { get; set; }        
        public string EstadoJefe { get; set; }

    }

    public class JefeUnidad
    {
        public int Id { get; set; }
        public double Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            } set {}
        }
        public Int16 TipoIdentificacion { get; set; }
        public int CodigoCiudadNacimiento { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }        
    }
}