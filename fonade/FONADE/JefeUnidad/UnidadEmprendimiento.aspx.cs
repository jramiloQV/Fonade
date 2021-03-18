using Datos;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.JefeUnidad
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

                if (IsUpdate) {
                    lblTituloActualizar.Visible = true;
                    btnActualizarUnidad.Visible = true;
                    divMotivoCambio.Visible = true;
                    btnCambiarDatos.Visible = true;
                }
                else {
                    lblTituloCrear.Visible = true;
                    btnCrearUnidad.Visible = true;
                }

                lblError.Visible = false;
            }     
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !, detalle : " + ex.Message.Replace("'",string.Empty);
            }
        }

        private void setDatosFormulario(int idInstitucion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.Institucions.FirstOrDefault(filter => filter.Id_Institucion.Equals(idInstitucion));

                if (entity == null)
                    throw new ApplicationException("No se pudo obtner la información de la institución.");

                txtNombreInstitucion.Text = "";
                txtNombreUnidad.Text = "";
                txtNitInstitucion.Text = "";
                txtDireccion.Text = "";
                txtDireccion.Text = "";
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
                    if (db.Institucions.Any(filter => filter.NomUnidad.Contains("")))
                        throw new ApplicationException("Existe una institución c");

                    var newEntity = new Datos.Institucion
                    {
                        NomInstitucion = txtNombreInstitucion.Text.Trim(),
                        NomUnidad = txtNombreUnidad.Text.Trim(),
                        Nit = Convert.ToDecimal(txtNitInstitucion.Text.Trim().Replace(".", "")),
                        Direccion = txtDireccion.Text.Trim(),
                        Telefono = string.Empty,
                        Fax = string.Empty,
                        CodCiudad = int.Parse(cmbCiudad.SelectedValue),
                        Inactivo = false,
                        CriteriosSeleccion = txtDireccion.Text.Trim(),
                        CodTipoInstitucion = Convert.ToByte(cmbTipoInstitucion.SelectedValue.ToString()),
                        WebSite = string.Empty
                    };

                    db.Institucions.InsertOnSubmit(newEntity);
                    db.SubmitChanges();
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
                lblError.Text = "Error, detalle : "  + ex.Message;
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {

        }
        public void mostrarModal()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "show", "$(function () { $('#" + ModalBuscarJefe.ClientID + "').modal('show'); });", true);
            UpdatePanel2.Update();
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
                                    where contacto.Identificacion.Equals(identificacion)
                                    select new BusquedaJefe
                                    {
                                        Id = contacto.Id_Contacto,
                                        Nombre = contacto.Nombres,
                                        Apellido = contacto.Apellidos,
                                        Identificacion = contacto.Identificacion,
                                        CodigoGrupo = grupo == null ? 0 : grupo.Id_Grupo,
                                        NombreGrupo = grupo.NomGrupo == null ? "Sin rol" : grupo.NomGrupo
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

            txtTelefonoUnidad.Text = string.Empty;
            txtFaxUnidad.Text = string.Empty;
            txtSitioWebUnidad.Text = string.Empty;

            txtCargoJefeUnidad.Text = string.Empty;
            txtTelefonoPersonalJefeUnidad.Text = string.Empty;
            txtFaxPersonalJefeUnidad.Text = string.Empty;
        }

        protected void btnCrearJefeUnidad_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    validateDatosJefeUnidad();

            //    var nombreJefe = txtNombreJefeUnidad.Text;
            //    var apellidoJefe = txtApellidoJefeUnidad.Text;
            //    var codigoTipoIdentificacion = cmbTipoDocumento.SelectedValue;
            //    var identificacionJefe = txtIdentificacionJefeUnidad.Text;
            //    var emailJefe = txtEmailJefeUnidad.Text;

            //    var codigoCiudadUnidad = cmbCiudadUnidad.SelectedValue;
            //    var telefonoUnidad = txtTelefonoUnidad.Text;
            //    var faxUnidad = txtFaxUnidad.Text;
            //    var sitioWebUnidad = txtSitioWebUnidad.Text;
                
            //    var cargoJefeUnidad = txtCargoJefeUnidad.Text;
            //    var telefonoJefeUnidad = txtTelefonoPersonalJefeUnidad.Text;
            //    var faxTelefonoUnida = txtFaxPersonalJefeUnidad.Text;

            //    if (!validarContactoExistente(emprendedor))
            //    {
            //        insertarEmprendedor(emprendedor);
            //        insertarGrupoContacto(emprendedor);                    
            //    }

            //    lblErrorCrearJefeUnidad.Visible = false;
            //}
            //catch (ApplicationException ex)
            //{
            //    lblErrorCrearJefeUnidad.Visible = true;
            //    lblErrorCrearJefeUnidad.Text = "Advertencia : " + ex.Message;
            //}
            //catch (Exception ex)
            //{
            //    lblErrorCrearJefeUnidad.Visible = true;
            //    lblErrorCrearJefeUnidad.Text = "Sucedio un error inesperado al crear el jefe de unidad.";
            //}
        }

        protected void validateDatosJefeUnidad() {
            FieldValidate.ValidateString("Nombre del jefe de unidad", txtNombreJefeUnidad.Text, true);
            FieldValidate.ValidateString("Apellido del jefe de unidad", txtApellidoJefeUnidad.Text, true);
            FieldValidate.ValidateNumeric("Identificación del jefe de unidad", txtIdentificacionJefeUnidad.Text, true);
            FieldValidate.ValidateString("Email del jefe de unidad", txtEmailJefeUnidad.Text, true, 300, true);

            FieldValidate.ValidateNumeric("Telefono de la unidad", txtTelefonoUnidad.Text, true);
            FieldValidate.ValidateNumeric("Fax de la unidad", txtFaxUnidad.Text, true);
            FieldValidate.ValidateString("Sitio web de la unidad", txtSitioWebUnidad.Text, true);

            FieldValidate.ValidateString("Cargo del funcionario", txtCargoJefeUnidad.Text, true);
            FieldValidate.ValidateString("Telefono del funcionario", txtTelefonoPersonalJefeUnidad.Text, true);
            FieldValidate.ValidateNumeric("Fax del funcionario", txtFaxPersonalJefeUnidad.Text, false);
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

        //protected bool validarContactoExistente(EmprendedorNegocio emprendedor)
        //{
        //    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
        //    {
        //        Datos.Contacto emprendedorExistente = db.Contactos.FirstOrDefault(contacto => contacto.Email == emprendedor.Email || contacto.Identificacion == emprendedor.Identificacion);

        //        //Si existe algun contacto con ese email o identificacion
        //        if (emprendedorExistente != null)
        //        {
        //            // Si el contacto esta inactivo
        //            if (emprendedorExistente.Inactivo && !emprendedor.ExisteContacto)
        //            {
        //                Datos.ProyectoContacto proyectoContacto = db.ProyectoContactos.FirstOrDefault(contacto => contacto.CodContacto == emprendedorExistente.Id_Contacto && contacto.Inactivo == true);
        //                if (proyectoContacto != null)
        //                {
        //                    emprendedor.Id = emprendedorExistente.Id_Contacto;
        //                    emprendedor.ExisteContactoValidoInactivo = true;
        //                    emprendedor.ExisteContacto = true;
        //                    return true;
        //                }
        //                else
        //                {
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                //Si el contacto esta activo y
        //                //es el mismo que se va actualizar.
        //                if (emprendedor.ExisteContacto && emprendedorExistente.Id_Contacto == emprendedor.Id)
        //                {
        //                    return false;
        //                }
        //                //Si el contacto esta activo y no es el mismo.
        //                else
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //        //Sino existe ningun contacto con ese email u identificación
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //protected void insertarEmprendedor(EmprendedorNegocio emprendedor)
        //{
        //    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
        //    {
        //        Datos.Contacto nuevoEmprendedor = new Datos.Contacto
        //        {
        //            Nombres = emprendedor.Nombre,
        //            Apellidos = emprendedor.Apellido,
        //            CodTipoIdentificacion = emprendedor.TipoIdentificacion,
        //            Identificacion = emprendedor.Identificacion,
        //            Email = emprendedor.Email,
        //            Clave = GeneraClave(),
        //            CodInstitucion = emprendedor.CodigoUnidadEmprendimiento,
        //            CodTipoAprendiz = emprendedor.TipoAprendiz,
        //            Genero = emprendedor.Genero,
        //            FechaNacimiento = emprendedor.FechaNacimiento,
        //            CodCiudad = emprendedor.CodigoCiudadNacimiento,
        //            Telefono = emprendedor.Telefono,
        //            LugarExpedicionDI = emprendedor.CodigoCiudadExpedicion,
        //            Inactivo = false
        //        };

        //        db.Contactos.InsertOnSubmit(nuevoEmprendedor);
        //        db.SubmitChanges();

        //        emprendedor.Id = nuevoEmprendedor.Id_Contacto;
        //        emprendedor.Clave = nuevoEmprendedor.Clave;
        //    }
        //}

        //protected void insertarGrupoContacto(EmprendedorNegocio emprendedor)
        //{
        //    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
        //    {
        //        if (!db.GrupoContactos.Any(contacto => contacto.CodContacto == emprendedor.Id))
        //        {
        //            Datos.GrupoContacto grupoContacto = new Datos.GrupoContacto
        //            {
        //                CodGrupo = Constantes.CONST_Emprendedor,
        //                CodContacto = emprendedor.Id
        //            };

        //            db.GrupoContactos.InsertOnSubmit(grupoContacto);
        //            db.SubmitChanges();
        //        }
        //    }
        //}
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
    public class BusquedaJefe {
        public Int32 Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto {
            get {
                return Nombre + " " + Apellido;
            } 
            set{}
        }
        public double Identificacion { get; set; }
        public int CodigoGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public Boolean AllowAsignarJefe { 
            get {
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
                                                                      && filter.FechaFin.Equals(null) );
                    }                    
                }
                   
                return allow;
            }
            set { }
        }
        public string linkAsignar { 
            get { 
                return AllowAsignarJefe ? "Asignar usuario a esta unidad" : "No se puede asignar este usuario";
            } 
            set {} 
        }
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
            }
        }
        public Int16 TipoIdentificacion { get; set; }
        public int CodigoCiudadUnidad { get; set; }
        public string Cargo { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public int? CodigoUnidadEmprendimiento { get; set; }
    }
}