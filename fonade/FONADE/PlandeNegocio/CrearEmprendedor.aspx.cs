using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using System.Globalization;
using System.Web.Security;
using Datos;
using System.Configuration;
using System.Net.Mail;

namespace Fonade.FONADE.PlandeNegocio
{
    public partial class CrearEmprendedor : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuarioLogged"] == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Lo sentimos tu sesión ha expirado');", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "close", "window.close();", true);
                return;
            }

            Boolean esActualizacion = false;
            try
            {
                if (Session["codigoPlanDeNegocio"] != null)
                {
                    int codigoPlanDeNegocio = (int)Session["codigoPlanDeNegocio"];
                    hfCodigoProyecto.Value = codigoPlanDeNegocio.ToString();

                    if (Session["codigoEmprendedor"] != null)
                    {
                        int codigoEmprendedor = (int)Session["codigoEmprendedor"];
                        hfCodigoEmprendedor.Value = codigoEmprendedor.ToString();
                        esActualizacion = true;
                    }
                    else
                    {
                        esActualizacion = false;
                    }

                    if (esActualizacion)
                    {
                        if (!Page.IsPostBack)
                            setDatosFormulario();

                        btnActualizarEmprendedor.Visible = true;
                        lblTituloActualizarEmprendedor.Visible = true;
                        btnActualizarEmprendedorVistaPrevia.Visible = true;

                        btnCrearEmprendedor.Visible = false;
                        lblTituloCrearEmprendedor.Visible = false;
                        btnGuardarEmprendedorVistaPrevia.Visible = false;
                    }
                    else
                    {
                        btnActualizarEmprendedor.Visible = false;
                        lblTituloActualizarEmprendedor.Visible = false;
                        btnActualizarEmprendedorVistaPrevia.Visible = false;

                        btnCrearEmprendedor.Visible = true;
                        lblTituloCrearEmprendedor.Visible = true;
                        btnGuardarEmprendedorVistaPrevia.Visible = true;
                    }
                }
                else
                {
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentelo de nuevo.");
                }
                lblErrorCrearEmprendedor.Visible = false;
            }
            catch (ApplicationException ex)
            {
                btnCrearEmprendedor.Visible = false;
                btnActualizarEmprendedor.Visible = false;

                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Lo sentimos sucedio un error, detalle : " + ex.Message;
            }
            catch (Exception ex)
            {
                btnCrearEmprendedor.Visible = false;
                btnActualizarEmprendedor.Visible = false;

                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        private void setDatosFormulario()
        {
            int codigoPlanDeNegocio = Convert.ToInt32(hfCodigoProyecto.Value);
            int codigoEmprendedor = Convert.ToInt32(hfCodigoEmprendedor.Value);

            EmprendedorNegocio emprendedor = getEmprendedor(codigoPlanDeNegocio, codigoEmprendedor);

            if (emprendedor == null)
                throw new ApplicationException("No se pudo obtener la información del emprendedor a actualizar, intentelo de nuevo.");

            txtNombres.Text = emprendedor.Nombre;
            txtApellidos.Text = emprendedor.Apellido;

            cmbTipoDocumento.DataBind();
            cmbTipoDocumento.ClearSelection();
            cmbTipoDocumento.Items.FindByValue(emprendedor.TipoIdentificacion.ToString()).Selected = true;

            txtIdentificacion.Text = emprendedor.Identificacion.ToString();

            if (emprendedor.CodigoDepartamentoExpedicion != 0)
            {
                cmbDepartamentoExpedicion.DataBind();
                cmbDepartamentoExpedicion.ClearSelection();
                cmbDepartamentoExpedicion.Items.FindByValue(emprendedor.CodigoDepartamentoExpedicion.ToString()).Selected = true;
            }

            if (emprendedor.CodigoCiudadExpedicion != 0)
            {
                cmbCiudadExpedicion.DataBind();
                cmbCiudadExpedicion.ClearSelection();
                cmbCiudadExpedicion.Items.FindByValue(emprendedor.CodigoCiudadExpedicion.ToString()).Selected = true;
            }



            txtEmail.Text = emprendedor.Email;

            cmbGenero.DataBind();
            cmbGenero.ClearSelection();
            cmbGenero.Items.FindByValue(emprendedor.Genero.ToString()).Selected = true;

            txtFechaNacimiento.Text = emprendedor.FechaNacimiento.ToShortDateString();

            if (emprendedor.CodigoDepartamentoNacimiento != 0)
            {
                cmbDepartamentoNacimiento.DataBind();
                cmbDepartamentoNacimiento.ClearSelection();
                cmbDepartamentoNacimiento.Items.FindByValue(emprendedor.CodigoDepartamentoNacimiento.ToString()).Selected = true;
            }

            if (emprendedor.CodigoCiudadNacimiento != 0)
            {
                cmbCiudadNacimiento.DataBind();
                cmbCiudadNacimiento.ClearSelection();
                cmbCiudadNacimiento.Items.FindByValue(emprendedor.CodigoCiudadNacimiento.ToString()).Selected = true;
            }

            if (emprendedor.CodigoDepartamentoResidencia != 0)
            {
                cmbDepartamentoResidencia.DataBind();
                cmbDepartamentoResidencia.ClearSelection();
                cmbDepartamentoResidencia.Items.FindByValue(emprendedor.CodigoDepartamentoResidencia.ToString()).Selected = true;
            }

            if (emprendedor.CodigoCiudadResidencia != 0)
            {
                cmbCiudadResidencia.DataBind();
                cmbCiudadResidencia.ClearSelection();
                cmbCiudadResidencia.Items.FindByValue(emprendedor.CodigoCiudadResidencia.ToString()).Selected = true;
            }

            txtTelefonoFijo.Text = emprendedor.Telefono;

            txtDireccionEmprendedor.Text = emprendedor.Direccion;

            if (emprendedor.TipoAprendiz != null)
            {
                cmbTipoAprendiz.DataBind();
                cmbTipoAprendiz.ClearSelection();
                cmbTipoAprendiz.Items.FindByValue(emprendedor.TipoAprendiz.Value.ToString()).Selected = true;
            }

            Datos.ContactoEstudio estudio = getEstudioContacto(codigoEmprendedor);

            if (estudio != null)
            {
                cmbNivelEstudio.DataBind();
                cmbNivelEstudio.ClearSelection();
                cmbNivelEstudio.Items.FindByValue(estudio.CodNivelEstudio.ToString()).Selected = true;

                ProgramaAcademico programa = getProgramaAcademico(estudio.CodProgramaAcademico.Value);

                hfcodigoProgramaRealizado.Value = programa.Id.ToString();
                txtProgramaRealizado.Text = programa.Nombre;

                hfCodigoInstitucionEducativa.Value = programa.CodigoInstitucionEducativa.ToString();
                hfCodigoCiudadInstitucionEducativa.Value = programa.CodigoCiudad.ToString();
                txtInstitucionEducativa.Text = programa.InstitucionEducativa;
                txtCiudadInstitucion.Text = programa.Ciudad;

                cmbEstadoEstudio.ClearSelection();
                cmbEstadoEstudio.Items.FindByValue(estudio.Finalizado.ToString()).Selected = true;

                txtFechaInicioEstudio.Text = estudio.FechaInicio.Value.ToShortDateString();

                if (estudio.Finalizado == 1)
                {
                    txtFechaGraduacionEstudio.Text = estudio.FechaGrado.Value.ToShortDateString();
                    if (estudio.FechaUltimoCorte != null)
                        txtFechaFinalizacionEstudio.Text = estudio.FechaUltimoCorte.Value.ToShortDateString();
                }
                else if (estudio.Finalizado == 0)
                {
                    txtHorasDedicadas.Text = estudio.SemestresCursados.ToString();
                }
            }
        }

        /// <summary>
        /// Obtiene los datos de un emprendedor
        /// </summary>
        /// <returns>Emprendedor</returns>
        public EmprendedorNegocio getEmprendedor(int codigoPlanDeNegocio, int codigoEmprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var emprendedores = (from contacto in db.Contacto
                                     where contacto.Id_Contacto == codigoEmprendedor
                                     select new EmprendedorNegocio
                                     {
                                         Id = contacto.Id_Contacto,
                                         Nombre = contacto.Nombres,
                                         Apellido = contacto.Apellidos,
                                         TipoIdentificacion = contacto.CodTipoIdentificacion,
                                         Identificacion = contacto.Identificacion,
                                         CodigoCiudadExpedicion = contacto.LugarExpedicionDI.GetValueOrDefault(0),
                                         Email = contacto.Email,
                                         Genero = contacto.Genero.GetValueOrDefault('M'),
                                         FechaNacimiento = contacto.FechaNacimiento.GetValueOrDefault(DateTime.Now),
                                         CodigoCiudadNacimiento = contacto.CodCiudad.GetValueOrDefault(0),
                                         Telefono = contacto.Telefono,
                                         Direccion = contacto.Direccion,
                                         CodigoProyecto = codigoPlanDeNegocio,
                                         TipoAprendiz = contacto.CodTipoAprendiz,
                                         CodigoCiudadResidencia = contacto.codCiudadResidencia.GetValueOrDefault(0)
                                     }
                                    ).FirstOrDefault();

                return emprendedores;
            }
        }

        /// <summary>
        /// Obtiene programas academicos.
        /// </summary>
        /// <returns>Lista de programas academicos</returns>
        public ProgramaAcademico getProgramaAcademico(int codigoProgramaAcademico)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var programaAcademico = (from programa in db.ProgramaAcademicos
                                         join institucion in db.InstitucionEducativas on programa.CodInstitucionEducativa equals institucion.Id_InstitucionEducativa
                                         join ciudad in db.Ciudad on programa.CodCiudad equals ciudad.Id_Ciudad
                                         where programa.Id_ProgramaAcademico == codigoProgramaAcademico
                                         select new ProgramaAcademico
                                         {
                                             Id = programa.Id_ProgramaAcademico,
                                             Nombre = programa.NomProgramaAcademico,
                                             CodigoInstitucionEducativa = programa.CodInstitucionEducativa,
                                             InstitucionEducativa = institucion.NomInstitucionEducativa,
                                             CodigoCiudad = programa.CodCiudad,
                                             Ciudad = ciudad.NomCiudad
                                         }).FirstOrDefault();

                return programaAcademico;
            }
        }

        protected Datos.ContactoEstudio getEstudioContacto(int codigoEmprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ContactoEstudios.FirstOrDefault(estudio => estudio.FlagIngresadoAsesor == 1 && estudio.CodContacto == codigoEmprendedor);
            }
        }

        /// <summary>
        /// Obtiene el listado de departamentos
        /// </summary>
        /// <returns>Lista de departamentos</returns>
        public List<Departamento> getDepartamentos()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var departamentos = (from departamentoColombia in db.departamento
                                     select new Departamento
                                     {
                                         Id = departamentoColombia.Id_Departamento,
                                         Nombre = departamentoColombia.NomDepartamento
                                     }
                                    ).ToList();
                return departamentos;
            }
        }

        /// <summary>
        /// Obtiene el listado de las ciudades
        /// </summary>
        /// <returns>Lista de ciudades</returns>
        public List<Ciudad> getCiudades(int codigoDepartamento)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var ciudades = (from ciudadesColombia in db.Ciudad
                                where ciudadesColombia.CodDepartamento == codigoDepartamento
                                select new Ciudad
                                {
                                    Id = ciudadesColombia.Id_Ciudad,
                                    Nombre = ciudadesColombia.NomCiudad,
                                    CodigoDepartamento = ciudadesColombia.CodDepartamento
                                }
                                    ).ToList();
                return ciudades;
            }
        }

        /// <summary>
        /// Obtiene el listado de todas las ciudades
        /// </summary>
        /// <returns>Lista de todas las ciudades</returns>
        public List<Ciudad> getAllCiudades()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var ciudades = (from ciudadesColombia in db.Ciudad
                                select new Ciudad
                                {
                                    Id = ciudadesColombia.Id_Ciudad,
                                    Nombre = ciudadesColombia.NomCiudad,
                                    CodigoDepartamento = ciudadesColombia.CodDepartamento
                                }).OrderBy(order => order.Nombre).ToList();
                return ciudades;
            }
        }

        /// <summary>
        /// Obtiene el listado de tipos de identificacion
        /// </summary>
        /// <returns>Lista de tipo de identificación</returns>
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

        /// <summary>
        /// Obtiene el listado de nivel de estudio
        /// </summary>
        /// <returns>Lista de nivel de estudio</returns>
        public List<NivelEstudio> getNivelesEstudio()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var nivelesEstudio = (from nivel in db.NivelEstudios
                                      select new NivelEstudio
                                      {
                                          Id = nivel.Id_NivelEstudio,
                                          Nombre = nivel.NomNivelEstudio
                                      }
                                    ).ToList();
                return nivelesEstudio;
            }
        }

        /// <summary>
        /// Obtiene el listado de tipo  de aprendiz
        /// </summary>
        /// <returns>Lista de nivel de tipo de aprendiz</returns>
        public List<TipoAprendiz> getTiposDeAprendiz()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var tiposAprendiz = (from tipo in db.TipoAprendizs
                                     orderby tipo.NomTipoAprendiz ascending
                                     select new TipoAprendiz
                                     {
                                         Id = tipo.Id_TipoAprendiz,
                                         Nombre = tipo.NomTipoAprendiz
                                     }
                                    ).ToList();
                return tiposAprendiz;
            }
        }

        protected void gvEmprendedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnBuscarEstudio_Click(object sender, EventArgs e)
        {
            gvProgramaAcademico.DataBind();
        }

        protected void btnCrearPrograma_Click(object sender, EventArgs e)
        {
            nuevoProgramaAcademico.Visible = true;
            buscador.Visible = false;
            listadoProgramas.Visible = false;
            lblErrorProgramaAcademico.Visible = false;
        }

        /// <summary>
        /// Obtiene el listado de programas academicos.
        /// </summary>
        /// <returns>Lista de programas academicos</returns>
        public List<ProgramaAcademico> getProgramasAcademicos(int startIndex, int maxRows, int codigoNivelEstudio, int codigoCiudad, string nombrePrograma = "")
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var planesDeNegocio = (from programa in db.ProgramaAcademicos
                                       join institucion in db.InstitucionEducativas on programa.CodInstitucionEducativa equals institucion.Id_InstitucionEducativa
                                       join ciudad in db.Ciudad on programa.CodCiudad equals ciudad.Id_Ciudad
                                       where programa.CodNivelEstudio == codigoNivelEstudio
                                             && (nombrePrograma == null || (nombrePrograma != null && programa.NomProgramaAcademico.ToLower().Contains(nombrePrograma)))
                                             && (nombrePrograma == null || (nombrePrograma != null && programa.CodCiudad.Equals(codigoCiudad)))
                                             && institucion.NomInstitucionEducativa != ""
                                       select new ProgramaAcademico
                                       {
                                           Id = programa.Id_ProgramaAcademico,
                                           Nombre = programa.NomProgramaAcademico,
                                           CodigoInstitucionEducativa = programa.CodInstitucionEducativa,
                                           InstitucionEducativa = institucion.NomInstitucionEducativa,
                                           CodigoCiudad = programa.CodCiudad,
                                           Ciudad = ciudad.NomCiudad
                                       }).OrderBy(plan => plan.InstitucionEducativa).Distinct().Skip(startIndex).Take(maxRows).ToList();

                return planesDeNegocio;
            }
        }

        /// <summary>
        /// Obtiene el numero de planes de negocio de ese usuario.
        /// </summary>
        /// <returns>Numero de planes de negocio</returns>
        public int getProgramasAcademicosCount(int codigoNivelEstudio, int codigoCiudad, string nombrePrograma = "")
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener listado de planes de negocio
                var planesDeNegocio = (from programa in db.ProgramaAcademicos
                                       join institucion in db.InstitucionEducativas on programa.CodInstitucionEducativa equals institucion.Id_InstitucionEducativa
                                       join ciudad in db.Ciudad on programa.CodCiudad equals ciudad.Id_Ciudad
                                       where programa.CodNivelEstudio == codigoNivelEstudio
                                             && (String.IsNullOrEmpty(nombrePrograma) || (!String.IsNullOrEmpty(nombrePrograma) && programa.NomProgramaAcademico.ToLower().Contains(nombrePrograma)))
                                             && (String.IsNullOrEmpty(nombrePrograma) || (!String.IsNullOrEmpty(nombrePrograma) && programa.CodCiudad.Equals(codigoCiudad)))
                                             && institucion.NomInstitucionEducativa != ""
                                       select new ProgramaAcademico
                                       {
                                           Id = programa.Id_ProgramaAcademico,
                                           Nombre = programa.NomProgramaAcademico,
                                           CodigoInstitucionEducativa = programa.CodInstitucionEducativa,
                                           InstitucionEducativa = institucion.NomInstitucionEducativa,
                                           CodigoCiudad = programa.CodCiudad,
                                           Ciudad = ciudad.NomCiudad
                                       }).Distinct().Count();

                return planesDeNegocio;
            }
        }

        /// <summary>
        /// Obtiene el listado de instituciones academicas.
        /// </summary>
        /// <returns>Lista de instituciones academicas</returns>
        public List<InstitucionAcademica> getInstitucionesAcademicas(int codigoNivelEstudio)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var instituciones = (from institucion in db.InstitucionEducativas
                                     join programa in db.ProgramaAcademicos on institucion.Id_InstitucionEducativa equals programa.CodInstitucionEducativa
                                     where programa.CodNivelEstudio == codigoNivelEstudio
                                           && programa.NomMunicipio != ""
                                           && programa.NomDepartamento != ""
                                     select new InstitucionAcademica
                                     {
                                         Id = institucion.Id_InstitucionEducativa,
                                         Nombre = institucion.NomInstitucionEducativa,
                                         Municipio = programa.NomMunicipio,
                                         Departamento = programa.NomDepartamento
                                     }).Distinct().OrderBy(order => order.Nombre).ToList();

                instituciones.Add(new InstitucionAcademica
                {
                    Id = -1,
                    Nombre = "OTRA"
                });

                return instituciones;
            }
        }

        protected void btnCancelarNuevoProgramaAcademico_Click(object sender, EventArgs e)
        {
            nuevoProgramaAcademico.Visible = false;
            buscador.Visible = true;
            listadoProgramas.Visible = true;
            lblErrorProgramaAcademico.Visible = false;
            txtNuevoPrograma.Text = "";
            txtNuevaInstitucion.Text = "";
        }

        protected void cmbInstitucionEducativa_TextChanged(object sender, EventArgs e)
        {
            if (cmbInstitucionEducativa.SelectedValue == "-1")
            {
                formNuevaInstitucion.Visible = true;
            }
            else
            {
                formNuevaInstitucion.Visible = false;
            }
        }

        protected void cmbInstitucionEducativa_DataBound(object sender, EventArgs e)
        {
            if (cmbInstitucionEducativa.SelectedValue == "-1")
            {
                formNuevaInstitucion.Visible = true;
            }
            else
            {
                formNuevaInstitucion.Visible = false;
            }
        }

        protected void btnNuevoProgramaAcademico_Click(object sender, EventArgs e)
        {
            try
            {
                ProgramaAcademico programaAcademico = new ProgramaAcademico();
                InstitucionAcademica institucionEducativa = new InstitucionAcademica();

                programaAcademico.Nombre = txtNuevoPrograma.Text;
                programaAcademico.CodigoCiudad = Convert.ToInt32(cmbCiudadInstitucion.SelectedValue);
                programaAcademico.Ciudad = cmbCiudadInstitucion.SelectedItem.Text;
                programaAcademico.codigoDepartamento = Convert.ToInt32(cmbDepartamentoInstitucion.SelectedValue);
                programaAcademico.departamento = cmbDepartamentoInstitucion.SelectedItem.Text;

                programaAcademico.codigoNivelEstudio = Convert.ToInt32(cmbNivelEstudio.SelectedValue);

                institucionEducativa.Id = Convert.ToInt32(cmbInstitucionEducativa.SelectedValue);
                institucionEducativa.Nombre = institucionEducativa.esNuevaInstitucion ? txtNuevaInstitucion.Text : cmbInstitucionEducativa.SelectedItem.Text;

                if (institucionEducativa.esNuevaInstitucion)
                    FieldValidate.ValidateString("Nueva institución educativa", institucionEducativa.Nombre, true);

                FieldValidate.ValidateString("Nombre del nuevo programa academico", programaAcademico.Nombre, true);

                if (institucionEducativa.esNuevaInstitucion)
                {
                    crearInstitucionEducativa(institucionEducativa);
                }

                programaAcademico.CodigoInstitucionEducativa = institucionEducativa.Id;

                crearProgramaAcademico(programaAcademico);

                hfcodigoProgramaRealizado.Value = programaAcademico.Id.ToString();
                txtProgramaRealizado.Text = programaAcademico.Nombre;
                hfCodigoInstitucionEducativa.Value = programaAcademico.CodigoInstitucionEducativa.ToString();
                txtInstitucionEducativa.Text = institucionEducativa.Nombre;
                hfCodigoCiudadInstitucionEducativa.Value = programaAcademico.CodigoCiudad.ToString();
                txtCiudadInstitucion.Text = programaAcademico.Ciudad;
                lblErrorProgramaAcademico.Visible = false;

                cerrarModalProgramaAcademico();
                txtNuevoPrograma.Text = "";
                txtNuevaInstitucion.Text = "";
            }
            catch (ApplicationException ex)
            {
                lblErrorProgramaAcademico.Visible = true;
                lblErrorProgramaAcademico.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblErrorProgramaAcademico.Visible = true;
                lblErrorProgramaAcademico.Text = "Sucedio un error inesperado al crear el programa academico.";
            }
        }

        private void crearProgramaAcademico(ProgramaAcademico programa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                int consecutivoCodigoprograma = (from programas in db.ProgramaAcademicos select programas.Id_ProgramaAcademico).OrderByDescending(ultimo => ultimo).First() + 1;

                Datos.ProgramaAcademico nuevoPrograma = new Datos.ProgramaAcademico
                {
                    Id_ProgramaAcademico = consecutivoCodigoprograma,
                    NomProgramaAcademico = programa.Nombre,
                    Nombre = "N/A",
                    CodInstitucionEducativa = programa.CodigoInstitucionEducativa,
                    Estado = "ACTIVO",
                    Metodologia = "N/A",
                    NomMunicipio = programa.Ciudad,
                    NomDepartamento = programa.departamento,
                    CodNivelEstudio = programa.codigoNivelEstudio,
                    CodCiudad = programa.CodigoCiudad
                };

                db.ProgramaAcademicos.InsertOnSubmit(nuevoPrograma);
                db.SubmitChanges();
                programa.Id = nuevoPrograma.Id_ProgramaAcademico;
            }
        }

        private void crearInstitucionEducativa(InstitucionAcademica institucion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                int consecutivoCodigoInstitucion = (from instituciones in db.InstitucionEducativas select instituciones.Id_InstitucionEducativa).OrderByDescending(ultimo => ultimo).First() + 1;

                Datos.InstitucionEducativa nuevaInstitucion = new Datos.InstitucionEducativa
                {
                    Id_InstitucionEducativa = consecutivoCodigoInstitucion,
                    NomInstitucionEducativa = institucion.Nombre
                };

                db.InstitucionEducativas.InsertOnSubmit(nuevaInstitucion);

                db.SubmitChanges();

                institucion.Id = nuevaInstitucion.Id_InstitucionEducativa;
            }
        }

        protected void gvProgramaAcademico_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("seleccionarPrograma"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros;
                    parametros = e.CommandArgument.ToString().Split(';');

                    string codigoProgramaRealizado = parametros[0];
                    string programaRealizado = parametros[1];
                    string ciudadPrograma = parametros[2];
                    string codigoInstitucionEducativa = parametros[3];
                    string institucionEducativa = parametros[4];
                    string codigoCiudadInstitucionEducativa = parametros[5];

                    hfcodigoProgramaRealizado.Value = codigoProgramaRealizado;
                    txtProgramaRealizado.Text = programaRealizado;
                    txtInstitucionEducativa.Text = institucionEducativa;
                    hfCodigoInstitucionEducativa.Value = codigoInstitucionEducativa;
                    txtCiudadInstitucion.Text = ciudadPrograma;
                    hfCodigoCiudadInstitucionEducativa.Value = codigoCiudadInstitucionEducativa;

                    cerrarModalProgramaAcademico();
                }
            }
        }

        protected void cmbEstadoEstudio_TextChanged(object sender, EventArgs e)
        {
            if (cmbEstadoEstudio.SelectedValue == "0")
            {
                formFechaFinalizacion.Visible = false;

                formFechaGraduacion.Visible = false;

                formHorasDedicadas.Visible = true;
            }
            else
            {
                formFechaFinalizacion.Visible = true;
                formFechaGraduacion.Visible = true;
                formHorasDedicadas.Visible = false;
            }
            txtFechaFinalizacionEstudio.Text = "";
            txtFechaGraduacionEstudio.Text = "";
            txtHorasDedicadas.Text = "";
        }

        protected void cmbEstadoEstudio_Load(object sender, EventArgs e)
        {
            if (cmbEstadoEstudio.SelectedValue == "0")
            {
                formFechaFinalizacion.Visible = false;

                formFechaGraduacion.Visible = false;

                formHorasDedicadas.Visible = true;
            }
            else
            {
                formFechaFinalizacion.Visible = true;
                formFechaGraduacion.Visible = true;
                formHorasDedicadas.Visible = false;
            }
        }

        protected void btnCerrarModalProgramaAcademico_Click(object sender, EventArgs e)
        {
            cerrarModalProgramaAcademico();
        }

        public void mostrarModalProgramaAcademico()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "show", "$(function () { $('#" + ModalEstudios.ClientID + "').modal('show'); });", true);
            UpdatePanel2.Update();
        }

        public void cerrarModalProgramaAcademico()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalEstudios.ClientID + "').modal('hide'); });", true);
            UpdatePanel1.Update();
        }

        protected void imbtn_institucion_Click(object sender, ImageClickEventArgs e)
        {
            mostrarModalProgramaAcademico();
        }

        protected void imbtn_nivel_Click(object sender, ImageClickEventArgs e)
        {
            mostrarModalProgramaAcademico();
        }

        protected void btnCrearEmprendedor_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdentificacion.Text.Equals(txtIdentificacionConfirm.Text))
                {
                    EmprendedorNegocio emprendedor = validarCamposObligatorios();

                    if (!validarContactoExistente(emprendedor))
                    {
                        mostrarModalVistaPrevia();
                    }
                    else
                    {
                        if (!emprendedor.ExisteContactoValidoInactivo)
                            throw new ApplicationException("Ya existe un contacto con ese email o identificación");

                        mostrarModalVistaPrevia();
                    }

                    lblErrorCrearEmprendedor.Visible = false;
                }
                else
                {
                    txtIdentificacionConfirm.Focus();
                    throw new ApplicationException("Favor confirmar el numero de identificación.");
                }

            }
            catch (ApplicationException ex)
            {
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor, detalle : " + ex.Message;
            }
        }

        private bool enviarEmail(string asunto, string emailRemitente, string emailDestinatario, string password)
        {
            try
            {
                bool errorMessage = false;
                string bodyTemplate = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta name=\"viewport\" content=\"width=device-width\"/><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"/><title>Fondo Emprender</title><style type=\"text/css\">*,.collapse{padding:0}.btn,.social .soc-btn{text-align:center;font-weight:700}.btn,ul.sidebar li a{text-decoration:none;cursor:pointer}.container,table.footer-wrap{clear:both!important}*{margin:0;font-family:\"Helvetica Neue\",Helvetica,Helvetica,Arial,sans-serif}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100%!important;height:100%}.content table,table.body-wrap,table.footer-wrap,table.head-wrap{width:100%}a{color:#2BA6CB}.btn{color:#FFF;background-color:#666;padding:10px 16px;margin-right:10px;display:inline-block}p.callout{padding:15px;background-color:#ECF8FF;margin-bottom:15px}.callout a{font-weight:700;color:#2BA6CB}table.social{background-color:#ebebeb}.social .soc-btn{padding:3px 7px;font-size:12px;margin-bottom:10px;text-decoration:none;color:#FFF;display:block}a.fb{background-color:#3B5998!important}a.tw{background-color:#1daced!important}a.gp{background-color:#DB4A39!important}a.ms{background-color:#000!important}.sidebar .soc-btn{display:block;width:100%}.header.container table td.logo{padding:15px}.header.container table td.label{padding:15px 15px 15px 0}.footer-wrap .container td.content p{border-top:1px solid #d7d7d7;padding-top:15px;font-size:10px;font-weight:700}h1,h2{font-weight:200}h1,h2,h3,h4,h5,h6{font-family:HelveticaNeue-Light,\"Helvetica Neue Light\",\"Helvetica Neue\",Helvetica,Arial,\"Lucida Grande\",sans-serif;line-height:1.1;margin-bottom:15px;color:#000}h1 small,h2 small,h3 small,h4 small,h5 small,h6 small{font-size:60%;color:#6f6f6f;line-height:0;text-transform:none}h1{font-size:44px}h2{font-size:37px}h3,h4{font-weight:500}h3{font-size:27px}h4{font-size:23px}h5,h6{font-weight:900}h5{font-size:17px}h6,p,ul{font-size:14px}h6{text-transform:uppercase;color:#444}.collapse{margin:0!important}p,ul{margin-bottom:10px;font-weight:400;line-height:1.6}p.lead{font-size:17px}p.last{margin-bottom:0}ul li{margin-left:5px;list-style-position:inside}ul.sidebar li,ul.sidebar li a{display:block;margin:0}ul.sidebar{background:#ebebeb;display:block;list-style-type:none}ul.sidebar li a{color:#666;padding:10px 16px;border-bottom:1px solid #777;border-top:1px solid #FFF}.column tr td,.content{padding:15px}ul.sidebar li a.last{border-bottom-width:0}ul.sidebar li a h1,ul.sidebar li a h2,ul.sidebar li a h3,ul.sidebar li a h4,ul.sidebar li a h5,ul.sidebar li a h6,ul.sidebar li a p{margin-bottom:0!important}.container{display:block!important;max-width:600px!important;margin:0 auto!important}.content{max-width:600px;margin:0 auto;display:block}.column{width:300px;float:left}.column-wrap{padding:0!important;margin:0 auto;max-width:600px!important}.column table{width:100%}.social .column{width:280px;min-width:279px;float:left}.clear{display:block;clear:both}@media only screen and (max-width:600px){a[class=btn]{display:block!important;margin-bottom:10px!important;background-image:none!important;margin-right:0!important}div[class=column]{width:auto!important;float:none!important}table.social div[class=column]{width:auto!important}}</style></head> <body bgcolor=\"#FFFFFF\"><table class=\"head-wrap\" bgcolor=\"#FFFFFF\"><tr><td></td><td class=\"header container\" ><div class=\"content\"><table bgcolor=\"#FFFFFF\"><tr><td><img src=\"{{logo}}\"/></td><td align=\"right\"><h6 class=\"collapse\"></h6></td></tr></table></div></td><td></td></tr></table><table class=\"body-wrap\"><tr><td></td><td class=\"container\" bgcolor=\"#FFFFFF\"><div class=\"content\"><table><tr><td><h3>Hola, Bienvenido a Fondo Emprender</h3><p class=\"lead\"> A continuación encontrara las credenciales para entrar a la plataforma.</p><table align=\"left\" class=\"column\"><tr><td><p class=\"\"><strong> Credenciales de acceso : </strong></p><p>Rol : <strong>{{rol}}</strong><br/>Email : <strong>{{email}}</strong><br/> Clave : <strong>{{clave}}</strong> </p></td></tr></table></td></tr></table></div></td><td></td></tr></table></body></html>";
                string urlLogoFondoEmprender = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

                MailMessage mail;
                mail = new MailMessage();
                mail.To.Add(new MailAddress(emailDestinatario));
                mail.From = new MailAddress(emailRemitente);
                mail.Subject = asunto;
                mail.Body = bodyTemplate.ReplaceWord("{{rol}}", "Emprendedor").ReplaceWord("{{email}}", emailDestinatario).ReplaceWord("{{clave}}", password).ReplaceWord("{{logo}}", urlLogoFondoEmprender);
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
            catch (Exception)
            {
                return false;
            }

        }

        protected void insertarEmprendedor(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                string nuevaclave = GeneraClave();
                Datos.Contacto nuevoEmprendedor = new Datos.Contacto
                {
                    Nombres = emprendedor.Nombre,
                    Apellidos = emprendedor.Apellido,
                    CodTipoIdentificacion = emprendedor.TipoIdentificacion,
                    Identificacion = emprendedor.Identificacion,
                    Email = emprendedor.Email,
                    Clave = Utilidades.Encrypt.GetSHA256(nuevaclave),
                    CodInstitucion = emprendedor.CodigoUnidadEmprendimiento,
                    CodTipoAprendiz = emprendedor.TipoAprendiz,
                    Genero = emprendedor.Genero,
                    FechaNacimiento = emprendedor.FechaNacimiento,
                    CodCiudad = emprendedor.CodigoCiudadNacimiento,
                    Telefono = emprendedor.Telefono,
                    Direccion = emprendedor.Direccion,
                    LugarExpedicionDI = emprendedor.CodigoCiudadExpedicion,
                    Inactivo = false,
                    AceptoTerminosYCondiciones = false,
                    codCiudadResidencia = emprendedor.CodigoCiudadResidencia
                };

                db.Contacto.InsertOnSubmit(nuevoEmprendedor);
                db.SubmitChanges();

                emprendedor.Id = nuevoEmprendedor.Id_Contacto;
                emprendedor.Clave = nuevaclave;//nuevoEmprendedor.Clave;
            }
        }

        protected void actualizarEmprendedor(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                Datos.Contacto emprendedorExistente = db.Contacto.SingleOrDefault(contacto => contacto.Id_Contacto == emprendedor.Id);

                if (emprendedorExistente == null)
                    throw new ApplicationException("Sucedio un error al obtener los datos del contacto, intentelo de nuevo");

                emprendedorExistente.Nombres = emprendedor.Nombre;
                emprendedorExistente.Apellidos = emprendedor.Apellido;
                emprendedorExistente.CodTipoIdentificacion = emprendedor.TipoIdentificacion;
                emprendedorExistente.Email = emprendedor.Email;
                emprendedorExistente.CodInstitucion = emprendedor.CodigoUnidadEmprendimiento;
                emprendedorExistente.CodTipoAprendiz = emprendedor.TipoAprendiz;
                emprendedorExistente.Genero = emprendedor.Genero;
                emprendedorExistente.FechaNacimiento = emprendedor.FechaNacimiento;
                emprendedorExistente.CodCiudad = emprendedor.CodigoCiudadNacimiento;
                emprendedorExistente.codCiudadResidencia = emprendedor.CodigoCiudadResidencia;
                emprendedorExistente.Telefono = emprendedor.Telefono;
                emprendedorExistente.LugarExpedicionDI = emprendedor.CodigoCiudadExpedicion;
                emprendedorExistente.Inactivo = false;
                emprendedorExistente.fechaActualizacion = DateTime.Now;
                emprendedorExistente.Direccion = emprendedor.Direccion;

                if (!emprendedor.ExisteContactoValidoInactivo)
                    emprendedorExistente.Identificacion = emprendedor.Identificacion;

                if (emprendedor.ExisteContactoValidoInactivo)
                {
                    emprendedorExistente.Clave = GeneraClave();
                    emprendedorExistente.AceptoTerminosYCondiciones = false;
                }

                db.SubmitChanges();

                emprendedor.Clave = emprendedorExistente.Clave;
            }
        }

        protected void insertarGrupoContacto(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!db.GrupoContactos.Any(contacto => contacto.CodContacto == emprendedor.Id))
                {
                    Datos.GrupoContacto grupoContacto = new Datos.GrupoContacto
                    {
                        CodGrupo = Constantes.CONST_Emprendedor,
                        CodContacto = emprendedor.Id
                    };

                    db.GrupoContactos.InsertOnSubmit(grupoContacto);
                    db.SubmitChanges();
                }
            }
        }

        protected void insertarProyectoContacto(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                Datos.ProyectoContacto proyectoContacto = new ProyectoContacto
                {
                    CodContacto = emprendedor.Id,
                    CodProyecto = emprendedor.CodigoProyecto,
                    CodRol = Constantes.CONST_RolEmprendedor,
                    FechaInicio = DateTime.Now
                };

                db.ProyectoContactos.InsertOnSubmit(proyectoContacto);
                db.SubmitChanges();
            }
        }

        protected void insertarEstudioContacto(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                Datos.ContactoEstudio estudioExistente = db.ContactoEstudios.FirstOrDefault(estudio => estudio.FlagIngresadoAsesor == 1 && estudio.CodContacto == emprendedor.Id);

                if (estudioExistente != null)
                {
                    estudioExistente.CodProgramaAcademico = emprendedor.CodigoProgramaAcademico;
                    estudioExistente.TituloObtenido = emprendedor.ProgramaAcademico;
                    estudioExistente.Institucion = emprendedor.InstitucionEducativa;
                    estudioExistente.CodCiudad = emprendedor.CodigoCiudadInstitucionEducativa;
                    estudioExistente.CodNivelEstudio = emprendedor.NivelEstudio;
                    estudioExistente.Finalizado = emprendedor.IsEstudioFinalizado ? 1 : 0;
                    estudioExistente.FechaInicio = emprendedor.FechaInicioEstudio;
                    estudioExistente.FechaGrado = emprendedor.FechaGraduacionEstudio;
                    estudioExistente.FechaFinMaterias = emprendedor.FechaFinalizacionEstudio;
                    estudioExistente.FechaUltimoCorte = emprendedor.FechaFinalizacionEstudio;
                    estudioExistente.SemestresCursados = emprendedor.HorasDedicadas;
                    estudioExistente.AnoTitulo = emprendedor.AnioGraduacion;
                    estudioExistente.fechaActualizacion = DateTime.Now;

                    db.SubmitChanges();
                }
                else
                {
                    Datos.ContactoEstudio contactoEstudio = new ContactoEstudio
                    {
                        CodContacto = emprendedor.Id,
                        CodProgramaAcademico = emprendedor.CodigoProgramaAcademico,
                        TituloObtenido = emprendedor.ProgramaAcademico,
                        Institucion = emprendedor.InstitucionEducativa,
                        CodCiudad = emprendedor.CodigoCiudadInstitucionEducativa,
                        CodNivelEstudio = emprendedor.NivelEstudio,
                        Finalizado = emprendedor.IsEstudioFinalizado ? 1 : 0,
                        FechaInicio = emprendedor.FechaInicioEstudio,
                        FechaGrado = emprendedor.FechaGraduacionEstudio,
                        FechaFinMaterias = emprendedor.FechaFinalizacionEstudio,
                        FechaUltimoCorte = emprendedor.FechaFinalizacionEstudio,
                        SemestresCursados = emprendedor.HorasDedicadas,
                        AnoTitulo = emprendedor.AnioGraduacion,
                        FlagIngresadoAsesor = 1,
                        fechaCreacion = DateTime.Now
                    };

                    db.ContactoEstudios.InsertOnSubmit(contactoEstudio);
                    db.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Validación para saber si el email o identificación ya esta siendo usada por otro contacto :
        /// </summary>
        /// <param name="emprendedor">Emprendedor a validar</param>
        /// <returns>Si existe o no en los contactos</returns>
        protected bool validarContactoExistente(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                Datos.Contacto emprendedorExistente = db.Contacto.FirstOrDefault(contacto => contacto.Email == emprendedor.Email || contacto.Identificacion == emprendedor.Identificacion);

                //Si existe algun contacto con ese email o identificacion
                if (emprendedorExistente != null)
                {
                    // Si el contacto esta inactivo
                    if (emprendedorExistente.Inactivo && !emprendedor.ExisteContacto)
                    {
                        Datos.ProyectoContacto proyectoContacto = db.ProyectoContactos.FirstOrDefault(contacto => contacto.CodContacto == emprendedorExistente.Id_Contacto && contacto.Inactivo == true);
                        if (proyectoContacto != null)
                        {
                            emprendedor.Id = emprendedorExistente.Id_Contacto;
                            emprendedor.ExisteContactoValidoInactivo = true;
                            emprendedor.ExisteContacto = true;
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        //Si el contacto esta activo y
                        //es el mismo que se va actualizar.
                        if (emprendedor.ExisteContacto && emprendedorExistente.Id_Contacto == emprendedor.Id)
                        {
                            return false;
                        }
                        //Si el contacto esta activo y no es el mismo.
                        else
                        {
                            return true;
                        }
                    }
                }
                //Sino existe ningun contacto con ese email u identificación
                else
                {
                    return false;
                }
            }
        }

        protected void btnActualizarEmprendedor_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIdentificacion.Text.Equals(txtIdentificacionConfirm.Text))
                {
                    int codigoPlanDeNegocio = Convert.ToInt32(hfCodigoProyecto.Value);
                    int codigoEmprendedor = Convert.ToInt32(hfCodigoEmprendedor.Value);
                    EmprendedorNegocio emprendedor = validarCamposObligatorios();
                    emprendedor.ExisteContacto = true;
                    emprendedor.Id = codigoEmprendedor;
                    emprendedor.CodigoProyecto = codigoPlanDeNegocio;

                    if (validarContactoExistente(emprendedor))
                        throw new ApplicationException("Ya existe un contacto con ese email u identificación");

                    mostrarModalVistaPrevia();

                    lblErrorCrearEmprendedor.Visible = false;
                }
                else
                {
                    txtIdentificacionConfirm.Focus();
                    throw new ApplicationException("Favor confirmar el numero de identificación.");
                }
            }
            catch (ApplicationException ex)
            {
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor.";
            }
        }

        private string convertirAFecha(string _fecha)
        {
            char[] delitadores = new char[] { '/', '-' };

            string[] args = _fecha.Split(delitadores);

            //- Verificar si tiene dd/mm/yyyy ó dd-mm-yyyy
            if (args.Length == 3)
            {
                //- Hacer el recorrido de las partes de la fecha para hacer el autocomplete
                for (int i = 0; i < args.Length - 1; i++)
                {
                    //- Si tiene menos de 2 caracteres significa que se tendrá que agregar el 0
                    if (args[i].Length < 2)
                    {
                        args[i] = args[i].PadLeft(2, '0');
                    }
                }
            }

            String Fecha = args[0] + "/" + args[1] + "/" + args[2];

            return Fecha;
        }

        /// <summary>
        /// Validación de campos obligatorios
        /// </summary>
        /// <returns> Emprendedor </returns>
        private EmprendedorNegocio validarCamposObligatorios()
        {
            EmprendedorNegocio emprendedor = new EmprendedorNegocio();
            string nombre;
            string apellido;
            Int16 tipoIdentificacion;
            double identificacion;
            int codigoDepartamentoExpedicion;
            int codigoCiudadExpedicion;
            string email;
            char genero;
            DateTime fechaNacimiento;
            int codigoDepartamentoNacimiento;
            int codigoCiudadNacimiento;
            string telefonoFijo;
            string direccion;
            int codigoDepartamentoResidencia;
            int codigoCiudadResidencia;
            int nivelEstudio;
            int codigoInstitucionEducativa;
            string institucionEducativa = txtInstitucionEducativa.Text;
            int codigoCiudadInstitucionEducativa;
            int codigoProgramaRealizado;
            string programaRealizado = txtProgramaRealizado.Text;
            bool isEstudioFinalizado;
            DateTime fechaInicioEstudio;
            DateTime? fechaFinalizacionEstudio = null;
            DateTime? fechaGraduacionEstudio = null;
            int? horasDedicadas = null;
            int tipoAprendiz;
            int codigoProyecto = Convert.ToInt32(hfCodigoProyecto.Value);
            int codigoUnidadEmprendimiento = usuario.CodInstitucion;
            DateTime fechaNacimientoValidaMenor = DateTime.Now.AddYears(-90);
            DateTime fechaNacimientoValidaMayor = DateTime.Now.AddYears(-10);

            nombre = txtNombres.Text;
            FieldValidate.ValidateString("Nombre", nombre, true, 100);

            apellido = txtApellidos.Text;
            FieldValidate.ValidateString("Apellido", apellido, true, 100);

            tipoIdentificacion = Convert.ToInt16(cmbTipoDocumento.SelectedValue);

            FieldValidate.ValidateNumeric("Identificación", txtIdentificacion.Text, true);
            identificacion = Convert.ToDouble(txtIdentificacion.Text);

            codigoDepartamentoExpedicion = Convert.ToInt32(cmbDepartamentoExpedicion.SelectedValue);
            codigoCiudadExpedicion = Convert.ToInt32(cmbCiudadExpedicion.SelectedValue);

            email = txtEmail.Text;
            FieldValidate.ValidateString("Email", email, true, 100, true);

            FieldValidate.ValidateString("Género", cmbGenero.SelectedValue, true, 1);
            genero = Char.Parse(cmbGenero.SelectedValue);

            if (txtFechaNacimiento.Text.Length == 9)
                txtFechaNacimiento.Text = "0" + txtFechaNacimiento.Text;

            FieldValidate.ValidateString("Fecha de nacimiento", txtFechaNacimiento.Text, true);
            fechaNacimiento = DateTime.ParseExact(txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            FieldValidate.ValidateIsFechaEntreRango("fecha mínima " + fechaNacimientoValidaMenor.ToShortDateString(), fechaNacimientoValidaMenor, "fecha de nacimiento", fechaNacimiento, "fecha máxima " + fechaNacimientoValidaMayor.ToShortDateString(), fechaNacimientoValidaMayor);
            DateTime fechaInicioEstudioValida = fechaNacimiento.AddYears(6);

            codigoDepartamentoNacimiento = Convert.ToInt32(cmbDepartamentoNacimiento.SelectedValue);
            codigoCiudadNacimiento = Convert.ToInt32(cmbCiudadNacimiento.SelectedValue);
            telefonoFijo = txtTelefonoFijo.Text;

            FieldValidate.ValidateString("Teléfono", telefonoFijo, true, 100);
            nivelEstudio = Convert.ToInt32(cmbNivelEstudio.SelectedValue);

            direccion = txtDireccionEmprendedor.Text;
            FieldValidate.ValidateString("Direccion", direccion, true, 120);

            codigoDepartamentoResidencia = Convert.ToInt32(cmbDepartamentoResidencia.SelectedValue);
            codigoCiudadResidencia = Convert.ToInt32(cmbCiudadResidencia.SelectedValue);

            FieldValidate.ValidateNumeric("Programa realizado", hfcodigoProgramaRealizado.Value, true);
            codigoProgramaRealizado = Convert.ToInt32(hfcodigoProgramaRealizado.Value);

            FieldValidate.ValidateNumeric("Institución educativa", hfCodigoInstitucionEducativa.Value, true);
            codigoInstitucionEducativa = Convert.ToInt32(hfCodigoInstitucionEducativa.Value);

            codigoCiudadInstitucionEducativa = Convert.ToInt32(hfCodigoCiudadInstitucionEducativa.Value);

            isEstudioFinalizado = Convert.ToInt32(cmbEstadoEstudio.SelectedValue) == 0 ? false : true;

            FieldValidate.ValidateString("Fecha de inicio de estudios", txtFechaInicioEstudio.Text, true);

            string fechaIniEst = convertirAFecha(txtFechaInicioEstudio.Text);

            fechaInicioEstudio = DateTime.ParseExact(fechaIniEst, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            FieldValidate.ValidateIsFechaEntreRango("Fecha minina " + fechaInicioEstudioValida.ToShortDateString(), fechaInicioEstudioValida, "Fecha de inicio estudio", fechaInicioEstudio, "Fecha maxima " + DateTime.Now.ToShortDateString(), DateTime.Now);


            tipoAprendiz = Convert.ToInt32(cmbTipoAprendiz.SelectedValue);

            if (isEstudioFinalizado)
            {
                FieldValidate.ValidateString("Fecha de finalización de estudios", txtFechaFinalizacionEstudio.Text, true);

                string fechaFinEst = convertirAFecha(txtFechaFinalizacionEstudio.Text);
                fechaFinalizacionEstudio = DateTime.ParseExact(fechaFinEst, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                FieldValidate.ValidateIsDateMayor("Fecha finalización de estudios", fechaFinalizacionEstudio.Value, "Fecha de hoy", DateTime.Now);

                FieldValidate.ValidateString("Fecha de graduación de estudios", txtFechaGraduacionEstudio.Text, true);

                string fechaGradoEst = convertirAFecha(txtFechaGraduacionEstudio.Text);
                fechaGraduacionEstudio = DateTime.ParseExact(fechaGradoEst, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                if (FieldValidate.isFechaEntreRango(fechaInicioEstudio, fechaGraduacionEstudio.Value, fechaFinalizacionEstudio.Value))
                    throw new ApplicationException("La fecha de graduación debe estar despues de la fecha de finalización de estudio");
                if (FieldValidate.isFechaMayor(fechaGraduacionEstudio.Value, DateTime.Now))
                    throw new ApplicationException("La fecha de graduación debe estar antes de la fecha de hoy");

                FieldValidate.ValidateIsFechaEntreRango("Fecha de inicio ", fechaInicioEstudio, "Fecha de finalización", fechaFinalizacionEstudio.Value, "Fecha de graduación", fechaGraduacionEstudio.Value);
            }
            else
            {
                FieldValidate.ValidateNumeric("Horas dedicadas u semestre actual", txtHorasDedicadas.Text, true);
                horasDedicadas = Convert.ToInt32(txtHorasDedicadas.Text);
            }

            emprendedor.Nombre = nombre;
            emprendedor.Apellido = apellido;
            emprendedor.TipoIdentificacion = tipoIdentificacion;
            emprendedor.Identificacion = identificacion;
            emprendedor.CodigoDepartamentoExpedicion = codigoDepartamentoExpedicion;
            emprendedor.CodigoCiudadExpedicion = codigoCiudadExpedicion;
            emprendedor.Email = email;
            emprendedor.Genero = genero;
            emprendedor.FechaNacimiento = fechaNacimiento;
            emprendedor.CodigoDepartamentoNacimiento = codigoDepartamentoNacimiento;
            emprendedor.CodigoCiudadNacimiento = codigoCiudadNacimiento;
            emprendedor.Telefono = telefonoFijo;
            emprendedor.Direccion = direccion;
            emprendedor.CodigoDepartamentoResidencia = codigoDepartamentoResidencia;
            emprendedor.CodigoCiudadResidencia = codigoCiudadResidencia;
            emprendedor.NivelEstudio = nivelEstudio;
            emprendedor.CodigoInstitucionEducativa = codigoInstitucionEducativa;
            emprendedor.InstitucionEducativa = institucionEducativa;
            emprendedor.CodigoCiudadInstitucionEducativa = codigoCiudadInstitucionEducativa;
            emprendedor.CodigoProgramaAcademico = codigoProgramaRealizado;
            emprendedor.ProgramaAcademico = programaRealizado;
            emprendedor.IsEstudioFinalizado = isEstudioFinalizado;
            emprendedor.FechaInicioEstudio = fechaInicioEstudio;
            emprendedor.FechaFinalizacionEstudio = fechaFinalizacionEstudio;
            emprendedor.FechaGraduacionEstudio = fechaGraduacionEstudio;
            emprendedor.HorasDedicadas = horasDedicadas;
            emprendedor.TipoAprendiz = tipoAprendiz;
            emprendedor.CodigoProyecto = codigoProyecto;
            emprendedor.CodigoUnidadEmprendimiento = codigoUnidadEmprendimiento;

            txtNombresVistaPrevia.Text = emprendedor.Nombre;
            txtApellidosVistaPrevia.Text = emprendedor.Apellido;
            txtTipoDocumentoVistaPrevia.Text = cmbTipoDocumento.SelectedItem.Text;
            txtIdentificacionVistaPrevia.Text = emprendedor.Identificacion.ToString();
            txtDepartamentoExpedicionVistaPrevia.Text = cmbDepartamentoExpedicion.SelectedItem.Text;
            txtCiudadExpedicionVistaPrevia.Text = cmbCiudadExpedicion.SelectedItem.Text;
            txtEmailVistaPrevia.Text = emprendedor.Email;
            txtGeneroVistaPrevia.Text = cmbGenero.SelectedItem.Text;
            txtFechaNacimientoVistaPrevia.Text = emprendedor.FechaNacimiento.ToShortDateString();
            txtDepartamentoNacimientoVistaPrevia.Text = cmbDepartamentoNacimiento.SelectedItem.Text;
            txtCiudadNacimientoVistaPrevia.Text = cmbCiudadNacimiento.SelectedItem.Text;
            txtTelefonoFijoVistaPrevia.Text = emprendedor.Telefono;
            txtDireccionEmpVistaPrevia.Text = emprendedor.Direccion;
            txtCiudadResidencia.Text = cmbCiudadResidencia.SelectedItem.Text;
            txtDepartamentoResidencia.Text = cmbDepartamentoResidencia.SelectedItem.Text;
            txtNivelEstudioVistaPrevia.Text = cmbNivelEstudio.SelectedItem.Text;
            txtProgramaRealizadoVistaPrevia.Text = emprendedor.ProgramaAcademico;
            txtInstitucionEducativaVistaPrevia.Text = emprendedor.InstitucionEducativa;
            txtCiudadInstitucionVistaPrevia.Text = txtCiudadInstitucion.Text;
            txtEstadoEstudioVistaPrevia.Text = cmbEstadoEstudio.SelectedItem.Text;
            txtTipoAprendizVistaPrevia.Text = cmbTipoAprendiz.SelectedItem.Text;
            txtFechaInicioEstudioVistaPrevia.Text = emprendedor.FechaInicioEstudio.ToShortDateString();

            if (isEstudioFinalizado)
            {
                formFechaFinalizacionVistaPrevia.Visible = true;
                formFechaGraduacionVistaPrevia.Visible = true;
                formHorasDedicadasVistaPrevia.Visible = false;
                txtFechaFinalizacionEstudioVistaPrevia.Text = emprendedor.FechaFinalizacionEstudio.Value.ToShortDateString();
                txtFechaGraduacionEstudioVistaPrevia.Text = emprendedor.FechaGraduacionEstudio.Value.ToShortDateString();
            }
            else
            {
                formFechaFinalizacionVistaPrevia.Visible = false;
                formFechaGraduacionVistaPrevia.Visible = false;
                formHorasDedicadasVistaPrevia.Visible = true;
                txtHorasDedicadasVistaPrevia.Text = emprendedor.HorasDedicadas.Value.ToString();
            }

            return emprendedor;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            cerrarFormEmprendedor();
        }

        private void cerrarYActualizarFormEmprendedor()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }

        private void cerrarFormEmprendedor()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }

        public void mostrarModalVistaPrevia()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel3, UpdatePanel3.GetType(), "show", "$(function () { $('#" + ModalVistaPrevia.ClientID + "').modal('show'); });", true);
            UpdatePanel3.Update();
        }

        public void cerrarModalVistaPrevia()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalVistaPrevia.ClientID + "').modal('hide'); });", true);
            UpdatePanel1.Update();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            mostrarModalVistaPrevia();
        }

        protected void btnModificarVistaPrevia_Click(object sender, EventArgs e)
        {
            cerrarModalVistaPrevia();
        }

        protected void btnActualizarEmprendedorVistaPrevia_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoPlanDeNegocio = Convert.ToInt32(hfCodigoProyecto.Value);
                int codigoEmprendedor = Convert.ToInt32(hfCodigoEmprendedor.Value);
                EmprendedorNegocio emprendedor = validarCamposObligatorios();
                emprendedor.ExisteContacto = true;
                emprendedor.Id = codigoEmprendedor;
                emprendedor.CodigoProyecto = codigoPlanDeNegocio;

                if (validarContactoExistente(emprendedor))
                    throw new ApplicationException("Ya existe un contacto con ese email u identificación");

                actualizarEmprendedor(emprendedor);
                insertarGrupoContacto(emprendedor);
                insertarEstudioContacto(emprendedor);

                if (!enviarEmail("Registro a fondo emprender", usuario.Email, emprendedor.Email, emprendedor.Clave))
                {
                    lblErrorCrearEmprendedor.Visible = true;
                    lblErrorCrearEmprendedor.Text = "Advertencia : No se logró enviar la notificacion al emprendedor.";
                }

                cerrarYActualizarFormEmprendedor();
                lblErrorCrearEmprendedor.Visible = false;
            }
            catch (ApplicationException ex)
            {
                cerrarModalVistaPrevia();
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                cerrarModalVistaPrevia();
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor.";
            }
        }

        protected void btnGuardarEmprendedorVistaPrevia_Click(object sender, EventArgs e)
        {
            try
            {
                cerrarModalCertificadoAsesor();
                cerrarModalVistaPrevia();

                EmprendedorNegocio emprendedor = validarCamposObligatorios();

                if (!validarContactoExistente(emprendedor))
                {
                    insertarEmprendedor(emprendedor);
                    insertarGrupoContacto(emprendedor);
                    insertarProyectoContacto(emprendedor);
                    insertarEstudioContacto(emprendedor);
                }
                else
                {
                    if (!emprendedor.ExisteContactoValidoInactivo)
                        throw new ApplicationException("Ya existe un contacto con ese email u identificación");

                    actualizarEmprendedor(emprendedor);
                    insertarGrupoContacto(emprendedor);
                    insertarProyectoContacto(emprendedor);
                    insertarEstudioContacto(emprendedor);
                }

                if (!enviarEmail("Registro a fondo emprender", usuario.Email, emprendedor.Email, emprendedor.Clave))
                {
                    lblErrorCrearEmprendedor.Visible = true;
                    lblErrorCrearEmprendedor.Text = "Advertencia : No se logró enviar el correo de notificacion al emprendedor.";
                }

                cerrarYActualizarFormEmprendedor();
                lblErrorCrearEmprendedor.Visible = false;
            }
            catch (ApplicationException ex)
            {
                cerrarModalCertificadoAsesor();
                cerrarModalVistaPrevia();
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                cerrarModalCertificadoAsesor();
                cerrarModalVistaPrevia();
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor.";
            }
        }

        public void mostrarModalCertificadoAsesor()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "show", "$(function () { $('#" + ModalCertificadoAsesor.ClientID + "').modal('show'); });", true);
            UpdatePanel4.Update();
        }

        public void cerrarModalCertificadoAsesor()
        {
            ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalCertificadoAsesor.ClientID + "').modal('hide'); });", true);
            UpdatePanel1.Update();
        }

        protected void btnCertificarRegistro_Click(object sender, EventArgs e)
        {

        }

        protected void btnNoCertificarRegistro_Click(object sender, EventArgs e)
        {
            cerrarModalCertificadoAsesor();
        }

        protected void btnVerCertificado_Click(object sender, EventArgs e)
        {
            EmprendedorNegocio emprendedor = validarCamposObligatorios();

            lblNombreAsesor.Text = usuario.Nombres;
            lblNombreEmprendedor.Text = emprendedor.NombreCompleto;
            lblCedulaEmprendedor.Text = emprendedor.Identificacion.ToString();
            lblNombreProyecto.Text = getNombreProyecto(emprendedor.CodigoProyecto);
            lblUnidadEmpredimientoTexto.Text = getUnidadEmprendimiento();
            lblUnidadEmpredimiento.Text = getUnidadEmprendimiento();
            lblNombrePerfil.Text = "Asesor";
            lblCedulaAsesor.Text = usuario.Identificacion.ToString();

            mostrarModalCertificadoAsesor();
        }

        protected string getUnidadEmprendimiento()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.Institucions.SingleOrDefault(filter => filter.Id_Institucion.Equals(usuario.CodInstitucion));

                return entity != null ? entity.NomUnidad : "Unidad de emprendimiento";
            }
        }

        protected string getNombreProyecto(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.Proyecto.SingleOrDefault(filter => filter.Id_Proyecto.Equals(codigoProyecto));

                return entity != null ? entity.NomProyecto : "Proyecto de emprendimiento";
            }
        }

    }

    public class TipoIdentificacion
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }

    public class NivelEstudio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }

    public class TipoAprendiz
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }

    public class ProgramaAcademico
    {
        public int Id { get; set; }
        private string nombre;
        public string Nombre
        {
            get
            {
                return this.nombre.ToUpper();
            }
            set
            {
                this.nombre = value;
            }
        }
        public int CodigoInstitucionEducativa { get; set; }
        public string InstitucionEducativa { get; set; }
        public int CodigoCiudad { get; set; }
        public string Ciudad { get; set; }
        public int codigoDepartamento { get; set; }
        public string departamento { get; set; }
        public int codigoNivelEstudio { get; set; }

    }

    public class InstitucionAcademica
    {

        public int Id { get; set; }

        private string nombre;
        public string Nombre
        {
            get
            {
                return this.nombre.ToUpper();
            }
            set
            {
                this.nombre = value;
            }
        }
        public string Municipio { get; set; }
        public string Departamento { get; set; }

        public string Locacion
        {
            get
            {
                return Municipio + " (" + Departamento + ")";
            }
        }

        public string NombreYLocacion
        {
            get
            {
                return (Nombre.Substring(0, Math.Min(Nombre.Length, 50)) + " " + Locacion).ToUpper().Trim();
            }
        }
        public Boolean esNuevaInstitucion
        {
            get
            {
                return Id == -1 ? true : false;
            }
        }
    }

    #region OldCode
    //public partial class CrearEmprendedor : Negocio.Base_Page
    //{
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        if (Session["usuarioLogged"] == null)
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "msg", "alert('Lo sentimos tu sesión ha expirado');", true);
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "close", "window.close();", true);
    //            return;
    //        }

    //        Boolean esActualizacion = false;
    //        try
    //        {
    //            if (Session["codigoPlanDeNegocio"] != null)
    //            {
    //                int codigoPlanDeNegocio = (int)Session["codigoPlanDeNegocio"];
    //                hfCodigoProyecto.Value = codigoPlanDeNegocio.ToString();

    //                if (Session["codigoEmprendedor"] != null)
    //                {
    //                    int codigoEmprendedor = (int)Session["codigoEmprendedor"];
    //                    hfCodigoEmprendedor.Value = codigoEmprendedor.ToString();
    //                    esActualizacion = true;
    //                }
    //                else
    //                {
    //                    esActualizacion = false;
    //                }

    //                if (esActualizacion)
    //                {
    //                    if (!Page.IsPostBack)
    //                        setDatosFormulario();

    //                    btnActualizarEmprendedor.Visible = true;
    //                    lblTituloActualizarEmprendedor.Visible = true;
    //                    btnActualizarEmprendedorVistaPrevia.Visible = true;

    //                    btnCrearEmprendedor.Visible = false;
    //                    lblTituloCrearEmprendedor.Visible = false;
    //                    btnGuardarEmprendedorVistaPrevia.Visible = false;
    //                }
    //                else
    //                {
    //                    btnActualizarEmprendedor.Visible = false;
    //                    lblTituloActualizarEmprendedor.Visible = false;
    //                    btnActualizarEmprendedorVistaPrevia.Visible = false;

    //                    btnCrearEmprendedor.Visible = true;
    //                    lblTituloCrearEmprendedor.Visible = true;
    //                    btnGuardarEmprendedorVistaPrevia.Visible = true;
    //                }
    //            }
    //            else
    //            {
    //                throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentelo de nuevo.");
    //            }
    //            lblErrorCrearEmprendedor.Visible = false;
    //        }
    //        catch (ApplicationException ex)
    //        {
    //            btnCrearEmprendedor.Visible = false;
    //            btnActualizarEmprendedor.Visible = false;

    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Lo sentimos sucedio un error, detalle : " + ex.Message;
    //        }
    //        catch (Exception ex)
    //        {
    //            btnCrearEmprendedor.Visible = false;
    //            btnActualizarEmprendedor.Visible = false;

    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
    //        }
    //    }

    //    private void setDatosFormulario()
    //    {
    //        int codigoPlanDeNegocio = Convert.ToInt32(hfCodigoProyecto.Value);
    //        int codigoEmprendedor = Convert.ToInt32(hfCodigoEmprendedor.Value);

    //        EmprendedorNegocio emprendedor = getEmprendedor(codigoPlanDeNegocio, codigoEmprendedor);

    //        if (emprendedor == null)
    //            throw new ApplicationException("No se pudo obtener la información del emprendedor a actualizar, intentelo de nuevo.");

    //        txtNombres.Text = emprendedor.Nombre;
    //        txtApellidos.Text = emprendedor.Apellido;

    //        cmbTipoDocumento.DataBind();
    //        cmbTipoDocumento.ClearSelection();
    //        cmbTipoDocumento.Items.FindByValue(emprendedor.TipoIdentificacion.ToString()).Selected = true;

    //        txtIdentificacion.Text = emprendedor.Identificacion.ToString();

    //        if (emprendedor.CodigoDepartamentoExpedicion != 0)
    //        {
    //            cmbDepartamentoExpedicion.DataBind();
    //            cmbDepartamentoExpedicion.ClearSelection();
    //            cmbDepartamentoExpedicion.Items.FindByValue(emprendedor.CodigoDepartamentoExpedicion.ToString()).Selected = true;
    //        }

    //        if (emprendedor.CodigoCiudadExpedicion != 0)
    //        {
    //            cmbCiudadExpedicion.DataBind();
    //            cmbCiudadExpedicion.ClearSelection();
    //            cmbCiudadExpedicion.Items.FindByValue(emprendedor.CodigoCiudadExpedicion.ToString()).Selected = true;
    //        }



    //        txtEmail.Text = emprendedor.Email;

    //        cmbGenero.DataBind();
    //        cmbGenero.ClearSelection();
    //        cmbGenero.Items.FindByValue(emprendedor.Genero.ToString()).Selected = true;

    //        txtFechaNacimiento.Text = emprendedor.FechaNacimiento.ToShortDateString();

    //        if (emprendedor.CodigoDepartamentoNacimiento != 0)
    //        {
    //            cmbDepartamentoNacimiento.DataBind();
    //            cmbDepartamentoNacimiento.ClearSelection();
    //            cmbDepartamentoNacimiento.Items.FindByValue(emprendedor.CodigoDepartamentoNacimiento.ToString()).Selected = true;
    //        }

    //        if (emprendedor.CodigoCiudadNacimiento != 0)
    //        {
    //            cmbCiudadNacimiento.DataBind();
    //            cmbCiudadNacimiento.ClearSelection();
    //            cmbCiudadNacimiento.Items.FindByValue(emprendedor.CodigoCiudadNacimiento.ToString()).Selected = true;
    //        }

    //        if (emprendedor.CodigoDepartamentoResidencia != 0)
    //        {
    //            cmbDepartamentoResidencia.DataBind();
    //            cmbDepartamentoResidencia.ClearSelection();
    //            cmbDepartamentoResidencia.Items.FindByValue(emprendedor.CodigoDepartamentoResidencia.ToString()).Selected = true;
    //        }

    //        if (emprendedor.CodigoCiudadResidencia != 0)
    //        {
    //            cmbCiudadResidencia.DataBind();
    //            cmbCiudadResidencia.ClearSelection();
    //            cmbCiudadResidencia.Items.FindByValue(emprendedor.CodigoCiudadResidencia.ToString()).Selected = true;
    //        }

    //        txtTelefonoFijo.Text = emprendedor.Telefono;

    //        txtDireccionEmprendedor.Text = emprendedor.Direccion;

    //        if (emprendedor.TipoAprendiz != null)
    //        {
    //            cmbTipoAprendiz.DataBind();
    //            cmbTipoAprendiz.ClearSelection();
    //            cmbTipoAprendiz.Items.FindByValue(emprendedor.TipoAprendiz.Value.ToString()).Selected = true;
    //        }

    //        Datos.ContactoEstudio estudio = getEstudioContacto(codigoEmprendedor);

    //        if (estudio != null)
    //        {
    //            cmbNivelEstudio.DataBind();
    //            cmbNivelEstudio.ClearSelection();
    //            cmbNivelEstudio.Items.FindByValue(estudio.CodNivelEstudio.ToString()).Selected = true;

    //            ProgramaAcademico programa = getProgramaAcademico(estudio.CodProgramaAcademico.Value);

    //            hfcodigoProgramaRealizado.Value = programa.Id.ToString();
    //            txtProgramaRealizado.Text = programa.Nombre;

    //            hfCodigoInstitucionEducativa.Value = programa.CodigoInstitucionEducativa.ToString();
    //            hfCodigoCiudadInstitucionEducativa.Value = programa.CodigoCiudad.ToString();
    //            txtInstitucionEducativa.Text = programa.InstitucionEducativa;
    //            txtCiudadInstitucion.Text = programa.Ciudad;

    //            cmbEstadoEstudio.ClearSelection();
    //            cmbEstadoEstudio.Items.FindByValue(estudio.Finalizado.ToString()).Selected = true;

    //            txtFechaInicioEstudio.Text = estudio.FechaInicio.Value.ToShortDateString();

    //            if (estudio.Finalizado == 1)
    //            {
    //                txtFechaGraduacionEstudio.Text = estudio.FechaGrado.Value.ToShortDateString();
    //                if (estudio.FechaUltimoCorte != null)
    //                    txtFechaFinalizacionEstudio.Text = estudio.FechaUltimoCorte.Value.ToShortDateString();
    //            }
    //            else if (estudio.Finalizado == 0)
    //            {
    //                txtHorasDedicadas.Text = estudio.SemestresCursados.ToString();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene los datos de un emprendedor
    //    /// </summary>
    //    /// <returns>Emprendedor</returns>
    //    public EmprendedorNegocio getEmprendedor(int codigoPlanDeNegocio, int codigoEmprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var emprendedores = (from contacto in db.Contacto
    //                                 where contacto.Id_Contacto == codigoEmprendedor
    //                                 select new EmprendedorNegocio
    //                                 {
    //                                     Id = contacto.Id_Contacto,
    //                                     Nombre = contacto.Nombres,
    //                                     Apellido = contacto.Apellidos,
    //                                     TipoIdentificacion = contacto.CodTipoIdentificacion,
    //                                     Identificacion = contacto.Identificacion,
    //                                     CodigoCiudadExpedicion = contacto.LugarExpedicionDI.GetValueOrDefault(0),
    //                                     Email = contacto.Email,
    //                                     Genero = contacto.Genero.GetValueOrDefault('M'),
    //                                     FechaNacimiento = contacto.FechaNacimiento.GetValueOrDefault(DateTime.Now),
    //                                     CodigoCiudadNacimiento = contacto.CodCiudad.GetValueOrDefault(0),
    //                                     Telefono = contacto.Telefono,
    //                                     Direccion = contacto.Direccion,
    //                                     CodigoProyecto = codigoPlanDeNegocio,
    //                                     TipoAprendiz = contacto.CodTipoAprendiz,
    //                                     CodigoCiudadResidencia = contacto.codCiudadResidencia.GetValueOrDefault(0)
    //                                 }
    //                                ).FirstOrDefault();

    //            return emprendedores;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene programas academicos.
    //    /// </summary>
    //    /// <returns>Lista de programas academicos</returns>
    //    public ProgramaAcademico getProgramaAcademico(int codigoProgramaAcademico)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var programaAcademico = (from programa in db.ProgramaAcademicos
    //                                     join institucion in db.InstitucionEducativas on programa.CodInstitucionEducativa equals institucion.Id_InstitucionEducativa
    //                                     join ciudad in db.Ciudad on programa.CodCiudad equals ciudad.Id_Ciudad
    //                                     where programa.Id_ProgramaAcademico == codigoProgramaAcademico
    //                                     select new ProgramaAcademico
    //                                     {
    //                                         Id = programa.Id_ProgramaAcademico,
    //                                         Nombre = programa.NomProgramaAcademico,
    //                                         CodigoInstitucionEducativa = programa.CodInstitucionEducativa,
    //                                         InstitucionEducativa = institucion.NomInstitucionEducativa,
    //                                         CodigoCiudad = programa.CodCiudad,
    //                                         Ciudad = ciudad.NomCiudad
    //                                     }).FirstOrDefault();

    //            return programaAcademico;
    //        }
    //    }

    //    protected Datos.ContactoEstudio getEstudioContacto(int codigoEmprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            return db.ContactoEstudios.FirstOrDefault(estudio => estudio.FlagIngresadoAsesor == 1 && estudio.CodContacto == codigoEmprendedor);
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de departamentos
    //    /// </summary>
    //    /// <returns>Lista de departamentos</returns>
    //    public List<Departamento> getDepartamentos()
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var departamentos = (from departamentoColombia in db.departamento
    //                                 select new Departamento
    //                                 {
    //                                     Id = departamentoColombia.Id_Departamento,
    //                                     Nombre = departamentoColombia.NomDepartamento
    //                                 }
    //                                ).ToList();
    //            return departamentos;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de las ciudades
    //    /// </summary>
    //    /// <returns>Lista de ciudades</returns>
    //    public List<Ciudad> getCiudades(int codigoDepartamento)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var ciudades = (from ciudadesColombia in db.Ciudad
    //                            where ciudadesColombia.CodDepartamento == codigoDepartamento
    //                            select new Ciudad
    //                            {
    //                                Id = ciudadesColombia.Id_Ciudad,
    //                                Nombre = ciudadesColombia.NomCiudad,
    //                                CodigoDepartamento = ciudadesColombia.CodDepartamento
    //                            }
    //                                ).ToList();
    //            return ciudades;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de todas las ciudades
    //    /// </summary>
    //    /// <returns>Lista de todas las ciudades</returns>
    //    public List<Ciudad> getAllCiudades()
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var ciudades = (from ciudadesColombia in db.Ciudad
    //                            select new Ciudad
    //                            {
    //                                Id = ciudadesColombia.Id_Ciudad,
    //                                Nombre = ciudadesColombia.NomCiudad,
    //                                CodigoDepartamento = ciudadesColombia.CodDepartamento
    //                            }).OrderBy(order => order.Nombre).ToList();
    //            return ciudades;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de tipos de identificacion
    //    /// </summary>
    //    /// <returns>Lista de tipo de identificación</returns>
    //    public List<TipoIdentificacion> getTiposIdentificacion()
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var tipoidentificación = (from tipo in db.TipoIdentificacions
    //                                      select new TipoIdentificacion
    //                                      {
    //                                          Id = tipo.Id_TipoIdentificacion,
    //                                          Nombre = tipo.NomTipoIdentificacion
    //                                      }
    //                                ).ToList();
    //            return tipoidentificación;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de nivel de estudio
    //    /// </summary>
    //    /// <returns>Lista de nivel de estudio</returns>
    //    public List<NivelEstudio> getNivelesEstudio()
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var nivelesEstudio = (from nivel in db.NivelEstudios
    //                                  select new NivelEstudio
    //                                  {
    //                                      Id = nivel.Id_NivelEstudio,
    //                                      Nombre = nivel.NomNivelEstudio
    //                                  }
    //                                ).ToList();
    //            return nivelesEstudio;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de tipo  de aprendiz
    //    /// </summary>
    //    /// <returns>Lista de nivel de tipo de aprendiz</returns>
    //    public List<TipoAprendiz> getTiposDeAprendiz()
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var tiposAprendiz = (from tipo in db.TipoAprendizs
    //                                 orderby tipo.NomTipoAprendiz ascending
    //                                 select new TipoAprendiz
    //                                 {
    //                                     Id = tipo.Id_TipoAprendiz,
    //                                     Nombre = tipo.NomTipoAprendiz
    //                                 }
    //                                ).ToList();
    //            return tiposAprendiz;
    //        }
    //    }

    //    protected void gvEmprendedores_RowCommand(object sender, GridViewCommandEventArgs e)
    //    {

    //    }

    //    protected void btnBuscarEstudio_Click(object sender, EventArgs e)
    //    {
    //        gvProgramaAcademico.DataBind();
    //    }

    //    protected void btnCrearPrograma_Click(object sender, EventArgs e)
    //    {
    //        nuevoProgramaAcademico.Visible = true;
    //        buscador.Visible = false;
    //        listadoProgramas.Visible = false;
    //        lblErrorProgramaAcademico.Visible = false;
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de programas academicos.
    //    /// </summary>
    //    /// <returns>Lista de programas academicos</returns>
    //    public List<ProgramaAcademico> getProgramasAcademicos(int startIndex, int maxRows, int codigoNivelEstudio, int codigoCiudad, string nombrePrograma = "")
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var planesDeNegocio = (from programa in db.ProgramaAcademicos
    //                                   join institucion in db.InstitucionEducativas on programa.CodInstitucionEducativa equals institucion.Id_InstitucionEducativa
    //                                   join ciudad in db.Ciudad on programa.CodCiudad equals ciudad.Id_Ciudad
    //                                   where programa.CodNivelEstudio == codigoNivelEstudio
    //                                         && (nombrePrograma == null || (nombrePrograma != null && programa.NomProgramaAcademico.ToLower().Contains(nombrePrograma)))
    //                                         && (nombrePrograma == null || (nombrePrograma != null && programa.CodCiudad.Equals(codigoCiudad)))
    //                                         && institucion.NomInstitucionEducativa != ""
    //                                   select new ProgramaAcademico
    //                                   {
    //                                       Id = programa.Id_ProgramaAcademico,
    //                                       Nombre = programa.NomProgramaAcademico,
    //                                       CodigoInstitucionEducativa = programa.CodInstitucionEducativa,
    //                                       InstitucionEducativa = institucion.NomInstitucionEducativa,
    //                                       CodigoCiudad = programa.CodCiudad,
    //                                       Ciudad = ciudad.NomCiudad
    //                                   }).OrderBy(plan => plan.InstitucionEducativa).Distinct().Skip(startIndex).Take(maxRows).ToList();

    //            return planesDeNegocio;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el numero de planes de negocio de ese usuario.
    //    /// </summary>
    //    /// <returns>Numero de planes de negocio</returns>
    //    public int getProgramasAcademicosCount(int codigoNivelEstudio, int codigoCiudad, string nombrePrograma = "")
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            //Consulta para obtener listado de planes de negocio
    //            var planesDeNegocio = (from programa in db.ProgramaAcademicos
    //                                   join institucion in db.InstitucionEducativas on programa.CodInstitucionEducativa equals institucion.Id_InstitucionEducativa
    //                                   join ciudad in db.Ciudad on programa.CodCiudad equals ciudad.Id_Ciudad
    //                                   where programa.CodNivelEstudio == codigoNivelEstudio
    //                                         && (String.IsNullOrEmpty(nombrePrograma) || (!String.IsNullOrEmpty(nombrePrograma) && programa.NomProgramaAcademico.ToLower().Contains(nombrePrograma)))
    //                                         && (String.IsNullOrEmpty(nombrePrograma) || (!String.IsNullOrEmpty(nombrePrograma) && programa.CodCiudad.Equals(codigoCiudad)))
    //                                         && institucion.NomInstitucionEducativa != ""
    //                                   select new ProgramaAcademico
    //                                   {
    //                                       Id = programa.Id_ProgramaAcademico,
    //                                       Nombre = programa.NomProgramaAcademico,
    //                                       CodigoInstitucionEducativa = programa.CodInstitucionEducativa,
    //                                       InstitucionEducativa = institucion.NomInstitucionEducativa,
    //                                       CodigoCiudad = programa.CodCiudad,
    //                                       Ciudad = ciudad.NomCiudad
    //                                   }).Distinct().Count();

    //            return planesDeNegocio;
    //        }
    //    }

    //    /// <summary>
    //    /// Obtiene el listado de instituciones academicas.
    //    /// </summary>
    //    /// <returns>Lista de instituciones academicas</returns>
    //    public List<InstitucionAcademica> getInstitucionesAcademicas(int codigoNivelEstudio)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var instituciones = (from institucion in db.InstitucionEducativas
    //                                 join programa in db.ProgramaAcademicos on institucion.Id_InstitucionEducativa equals programa.CodInstitucionEducativa
    //                                 where programa.CodNivelEstudio == codigoNivelEstudio
    //                                       && programa.NomMunicipio != ""
    //                                       && programa.NomDepartamento != ""                                                                          
    //                                 select new InstitucionAcademica
    //                                 {
    //                                     Id = institucion.Id_InstitucionEducativa,
    //                                     Nombre = institucion.NomInstitucionEducativa,
    //                                     Municipio = programa.NomMunicipio,
    //                                     Departamento = programa.NomDepartamento
    //                                 }).Distinct().OrderBy(order => order.Nombre).ToList();

    //            instituciones.Add(new InstitucionAcademica
    //            {
    //                Id = -1,
    //                Nombre = "OTRA"
    //            });

    //            return instituciones;
    //        }
    //    }

    //    protected void btnCancelarNuevoProgramaAcademico_Click(object sender, EventArgs e)
    //    {
    //        nuevoProgramaAcademico.Visible = false;
    //        buscador.Visible = true;
    //        listadoProgramas.Visible = true;
    //        lblErrorProgramaAcademico.Visible = false;
    //        txtNuevoPrograma.Text = "";
    //        txtNuevaInstitucion.Text = "";
    //    }

    //    protected void cmbInstitucionEducativa_TextChanged(object sender, EventArgs e)
    //    {
    //        if (cmbInstitucionEducativa.SelectedValue == "-1")
    //        {
    //            formNuevaInstitucion.Visible = true;
    //        }
    //        else
    //        {
    //            formNuevaInstitucion.Visible = false;
    //        }
    //    }

    //    protected void cmbInstitucionEducativa_DataBound(object sender, EventArgs e)
    //    {
    //        if (cmbInstitucionEducativa.SelectedValue == "-1")
    //        {
    //            formNuevaInstitucion.Visible = true;
    //        }
    //        else
    //        {
    //            formNuevaInstitucion.Visible = false;
    //        }
    //    }

    //    protected void btnNuevoProgramaAcademico_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            ProgramaAcademico programaAcademico = new ProgramaAcademico();
    //            InstitucionAcademica institucionEducativa = new InstitucionAcademica();

    //            programaAcademico.Nombre = txtNuevoPrograma.Text;
    //            programaAcademico.CodigoCiudad = Convert.ToInt32(cmbCiudadInstitucion.SelectedValue);
    //            programaAcademico.Ciudad = cmbCiudadInstitucion.SelectedItem.Text;
    //            programaAcademico.codigoDepartamento = Convert.ToInt32(cmbDepartamentoInstitucion.SelectedValue);
    //            programaAcademico.departamento = cmbDepartamentoInstitucion.SelectedItem.Text;

    //            programaAcademico.codigoNivelEstudio = Convert.ToInt32(cmbNivelEstudio.SelectedValue);

    //            institucionEducativa.Id = Convert.ToInt32(cmbInstitucionEducativa.SelectedValue);
    //            institucionEducativa.Nombre = institucionEducativa.esNuevaInstitucion ? txtNuevaInstitucion.Text : cmbInstitucionEducativa.SelectedItem.Text;

    //            if (institucionEducativa.esNuevaInstitucion)
    //                FieldValidate.ValidateString("Nueva institución educativa", institucionEducativa.Nombre, true);

    //            FieldValidate.ValidateString("Nombre del nuevo programa academico", programaAcademico.Nombre, true);

    //            if (institucionEducativa.esNuevaInstitucion)
    //            {
    //                crearInstitucionEducativa(institucionEducativa);
    //            }

    //            programaAcademico.CodigoInstitucionEducativa = institucionEducativa.Id;

    //            crearProgramaAcademico(programaAcademico);

    //            hfcodigoProgramaRealizado.Value = programaAcademico.Id.ToString();
    //            txtProgramaRealizado.Text = programaAcademico.Nombre;
    //            hfCodigoInstitucionEducativa.Value = programaAcademico.CodigoInstitucionEducativa.ToString();
    //            txtInstitucionEducativa.Text = institucionEducativa.Nombre;
    //            hfCodigoCiudadInstitucionEducativa.Value = programaAcademico.CodigoCiudad.ToString();
    //            txtCiudadInstitucion.Text = programaAcademico.Ciudad;
    //            lblErrorProgramaAcademico.Visible = false;

    //            cerrarModalProgramaAcademico();
    //            txtNuevoPrograma.Text = "";
    //            txtNuevaInstitucion.Text = "";
    //        }
    //        catch (ApplicationException ex)
    //        {
    //            lblErrorProgramaAcademico.Visible = true;
    //            lblErrorProgramaAcademico.Text = "Advertencia : " + ex.Message;
    //        }
    //        catch (Exception ex)
    //        {
    //            lblErrorProgramaAcademico.Visible = true;
    //            lblErrorProgramaAcademico.Text = "Sucedio un error inesperado al crear el programa academico.";
    //        }
    //    }

    //    private void crearProgramaAcademico(ProgramaAcademico programa)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {

    //            int consecutivoCodigoprograma = (from programas in db.ProgramaAcademicos select programas.Id_ProgramaAcademico).OrderByDescending(ultimo => ultimo).First() + 1;

    //            Datos.ProgramaAcademico nuevoPrograma = new Datos.ProgramaAcademico
    //            {
    //                Id_ProgramaAcademico = consecutivoCodigoprograma,
    //                NomProgramaAcademico = programa.Nombre,
    //                Nombre = "N/A",
    //                CodInstitucionEducativa = programa.CodigoInstitucionEducativa,
    //                Estado = "ACTIVO",
    //                Metodologia = "N/A",
    //                NomMunicipio = programa.Ciudad,
    //                NomDepartamento = programa.departamento,
    //                CodNivelEstudio = programa.codigoNivelEstudio,
    //                CodCiudad = programa.CodigoCiudad
    //            };

    //            db.ProgramaAcademicos.InsertOnSubmit(nuevoPrograma);
    //            db.SubmitChanges();
    //            programa.Id = nuevoPrograma.Id_ProgramaAcademico;
    //        }
    //    }

    //    private void crearInstitucionEducativa(InstitucionAcademica institucion)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {

    //            int consecutivoCodigoInstitucion = (from instituciones in db.InstitucionEducativas select instituciones.Id_InstitucionEducativa).OrderByDescending(ultimo => ultimo).First() + 1;

    //            Datos.InstitucionEducativa nuevaInstitucion = new Datos.InstitucionEducativa
    //            {
    //                Id_InstitucionEducativa = consecutivoCodigoInstitucion,
    //                NomInstitucionEducativa = institucion.Nombre
    //            };

    //            db.InstitucionEducativas.InsertOnSubmit(nuevaInstitucion);

    //            db.SubmitChanges();

    //            institucion.Id = nuevaInstitucion.Id_InstitucionEducativa;
    //        }
    //    }

    //    protected void gvProgramaAcademico_RowCommand(object sender, GridViewCommandEventArgs e)
    //    {
    //        if (e.CommandName.Equals("seleccionarPrograma"))
    //        {
    //            if (e.CommandArgument != null)
    //            {
    //                string[] parametros;
    //                parametros = e.CommandArgument.ToString().Split(';');

    //                string codigoProgramaRealizado = parametros[0];
    //                string programaRealizado = parametros[1];
    //                string ciudadPrograma = parametros[2];
    //                string codigoInstitucionEducativa = parametros[3];
    //                string institucionEducativa = parametros[4];
    //                string codigoCiudadInstitucionEducativa = parametros[5];

    //                hfcodigoProgramaRealizado.Value = codigoProgramaRealizado;
    //                txtProgramaRealizado.Text = programaRealizado;
    //                txtInstitucionEducativa.Text = institucionEducativa;
    //                hfCodigoInstitucionEducativa.Value = codigoInstitucionEducativa;
    //                txtCiudadInstitucion.Text = ciudadPrograma;
    //                hfCodigoCiudadInstitucionEducativa.Value = codigoCiudadInstitucionEducativa;

    //                cerrarModalProgramaAcademico();
    //            }
    //        }
    //    }

    //    protected void cmbEstadoEstudio_TextChanged(object sender, EventArgs e)
    //    {
    //        if (cmbEstadoEstudio.SelectedValue == "0")
    //        {
    //            formFechaFinalizacion.Visible = false;

    //            formFechaGraduacion.Visible = false;

    //            formHorasDedicadas.Visible = true;
    //        }
    //        else
    //        {
    //            formFechaFinalizacion.Visible = true;
    //            formFechaGraduacion.Visible = true;
    //            formHorasDedicadas.Visible = false;
    //        }
    //        txtFechaFinalizacionEstudio.Text = "";
    //        txtFechaGraduacionEstudio.Text = "";
    //        txtHorasDedicadas.Text = "";
    //    }

    //    protected void cmbEstadoEstudio_Load(object sender, EventArgs e)
    //    {
    //        if (cmbEstadoEstudio.SelectedValue == "0")
    //        {
    //            formFechaFinalizacion.Visible = false;

    //            formFechaGraduacion.Visible = false;

    //            formHorasDedicadas.Visible = true;
    //        }
    //        else
    //        {
    //            formFechaFinalizacion.Visible = true;
    //            formFechaGraduacion.Visible = true;
    //            formHorasDedicadas.Visible = false;
    //        }
    //    }

    //    protected void btnCerrarModalProgramaAcademico_Click(object sender, EventArgs e)
    //    {
    //        cerrarModalProgramaAcademico();
    //    }

    //    public void mostrarModalProgramaAcademico()
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel2, UpdatePanel2.GetType(), "show", "$(function () { $('#" + ModalEstudios.ClientID + "').modal('show'); });", true);
    //        UpdatePanel2.Update();
    //    }

    //    public void cerrarModalProgramaAcademico()
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalEstudios.ClientID + "').modal('hide'); });", true);
    //        UpdatePanel1.Update();
    //    }

    //    protected void imbtn_institucion_Click(object sender, ImageClickEventArgs e)
    //    {
    //        mostrarModalProgramaAcademico();
    //    }

    //    protected void imbtn_nivel_Click(object sender, ImageClickEventArgs e)
    //    {
    //        mostrarModalProgramaAcademico();
    //    }

    //    protected void btnCrearEmprendedor_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            if (txtIdentificacion.Text.Equals(txtIdentificacionConfirm.Text))
    //            {
    //                EmprendedorNegocio emprendedor = validarCamposObligatorios();

    //                if (!validarContactoExistente(emprendedor))
    //                {
    //                    mostrarModalVistaPrevia();
    //                }
    //                else
    //                {
    //                    if (!emprendedor.ExisteContactoValidoInactivo)
    //                        throw new ApplicationException("Ya existe un contacto con ese email u identificación");

    //                    mostrarModalVistaPrevia();
    //                }

    //                lblErrorCrearEmprendedor.Visible = false;
    //            }
    //            else
    //            {
    //                txtIdentificacionConfirm.Focus();
    //                throw new ApplicationException("Favor confirmar el numero de identificación.");
    //            }

    //        }
    //        catch (ApplicationException ex)
    //        {
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Advertencia: " + ex.Message;
    //        }
    //        catch (Exception ex)
    //        {
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor, detalle : " + ex.Message;
    //        }
    //    }

    //    private bool enviarEmail(string asunto, string emailRemitente, string emailDestinatario, string password)
    //    {
    //        try
    //        {
    //            bool errorMessage = false;
    //            string bodyTemplate = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta name=\"viewport\" content=\"width=device-width\"/><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"/><title>Fondo Emprender</title><style type=\"text/css\">*,.collapse{padding:0}.btn,.social .soc-btn{text-align:center;font-weight:700}.btn,ul.sidebar li a{text-decoration:none;cursor:pointer}.container,table.footer-wrap{clear:both!important}*{margin:0;font-family:\"Helvetica Neue\",Helvetica,Helvetica,Arial,sans-serif}img{max-width:100%}body{-webkit-font-smoothing:antialiased;-webkit-text-size-adjust:none;width:100%!important;height:100%}.content table,table.body-wrap,table.footer-wrap,table.head-wrap{width:100%}a{color:#2BA6CB}.btn{color:#FFF;background-color:#666;padding:10px 16px;margin-right:10px;display:inline-block}p.callout{padding:15px;background-color:#ECF8FF;margin-bottom:15px}.callout a{font-weight:700;color:#2BA6CB}table.social{background-color:#ebebeb}.social .soc-btn{padding:3px 7px;font-size:12px;margin-bottom:10px;text-decoration:none;color:#FFF;display:block}a.fb{background-color:#3B5998!important}a.tw{background-color:#1daced!important}a.gp{background-color:#DB4A39!important}a.ms{background-color:#000!important}.sidebar .soc-btn{display:block;width:100%}.header.container table td.logo{padding:15px}.header.container table td.label{padding:15px 15px 15px 0}.footer-wrap .container td.content p{border-top:1px solid #d7d7d7;padding-top:15px;font-size:10px;font-weight:700}h1,h2{font-weight:200}h1,h2,h3,h4,h5,h6{font-family:HelveticaNeue-Light,\"Helvetica Neue Light\",\"Helvetica Neue\",Helvetica,Arial,\"Lucida Grande\",sans-serif;line-height:1.1;margin-bottom:15px;color:#000}h1 small,h2 small,h3 small,h4 small,h5 small,h6 small{font-size:60%;color:#6f6f6f;line-height:0;text-transform:none}h1{font-size:44px}h2{font-size:37px}h3,h4{font-weight:500}h3{font-size:27px}h4{font-size:23px}h5,h6{font-weight:900}h5{font-size:17px}h6,p,ul{font-size:14px}h6{text-transform:uppercase;color:#444}.collapse{margin:0!important}p,ul{margin-bottom:10px;font-weight:400;line-height:1.6}p.lead{font-size:17px}p.last{margin-bottom:0}ul li{margin-left:5px;list-style-position:inside}ul.sidebar li,ul.sidebar li a{display:block;margin:0}ul.sidebar{background:#ebebeb;display:block;list-style-type:none}ul.sidebar li a{color:#666;padding:10px 16px;border-bottom:1px solid #777;border-top:1px solid #FFF}.column tr td,.content{padding:15px}ul.sidebar li a.last{border-bottom-width:0}ul.sidebar li a h1,ul.sidebar li a h2,ul.sidebar li a h3,ul.sidebar li a h4,ul.sidebar li a h5,ul.sidebar li a h6,ul.sidebar li a p{margin-bottom:0!important}.container{display:block!important;max-width:600px!important;margin:0 auto!important}.content{max-width:600px;margin:0 auto;display:block}.column{width:300px;float:left}.column-wrap{padding:0!important;margin:0 auto;max-width:600px!important}.column table{width:100%}.social .column{width:280px;min-width:279px;float:left}.clear{display:block;clear:both}@media only screen and (max-width:600px){a[class=btn]{display:block!important;margin-bottom:10px!important;background-image:none!important;margin-right:0!important}div[class=column]{width:auto!important;float:none!important}table.social div[class=column]{width:auto!important}}</style></head> <body bgcolor=\"#FFFFFF\"><table class=\"head-wrap\" bgcolor=\"#FFFFFF\"><tr><td></td><td class=\"header container\" ><div class=\"content\"><table bgcolor=\"#FFFFFF\"><tr><td><img src=\"{{logo}}\"/></td><td align=\"right\"><h6 class=\"collapse\"></h6></td></tr></table></div></td><td></td></tr></table><table class=\"body-wrap\"><tr><td></td><td class=\"container\" bgcolor=\"#FFFFFF\"><div class=\"content\"><table><tr><td><h3>Hola, Bienvenido a Fondo Emprender</h3><p class=\"lead\"> A continuación encontrara las credenciales para entrar a la plataforma.</p><table align=\"left\" class=\"column\"><tr><td><p class=\"\"><strong> Credenciales de acceso : </strong></p><p>Rol : <strong>{{rol}}</strong><br/>Email : <strong>{{email}}</strong><br/> Clave : <strong>{{clave}}</strong> </p></td></tr></table></td></tr></table></div></td><td></td></tr></table></body></html>";
    //            string urlLogoFondoEmprender = ConfigurationManager.AppSettings["RutaWebSite"] + "/" + ConfigurationManager.AppSettings["logoEmail"];

    //            MailMessage mail;
    //            mail = new MailMessage();
    //            mail.To.Add(new MailAddress(emailDestinatario));
    //            mail.From = new MailAddress(emailRemitente);
    //            mail.Subject = asunto;
    //            mail.Body = bodyTemplate.ReplaceWord("{{rol}}", "Emprendedor").ReplaceWord("{{email}}", emailDestinatario).ReplaceWord("{{clave}}", password).ReplaceWord("{{logo}}", urlLogoFondoEmprender);
    //            mail.BodyEncoding = System.Text.Encoding.UTF8;
    //            mail.IsBodyHtml = true;

    //            var smtp = ConfigurationManager.AppSettings.Get("SMTP");
    //            var port = int.Parse(ConfigurationManager.AppSettings.Get("SMTP_UsedPort"));
    //            SmtpClient client = new SmtpClient(smtp, port);
    //            using (client)
    //            {
    //                var usuarioEmail = ConfigurationManager.AppSettings.Get("SMTPUsuario");
    //                var passwordEmail = ConfigurationManager.AppSettings.Get("SMTPPassword");
    //                client.Credentials = new System.Net.NetworkCredential(usuarioEmail, passwordEmail);
    //                client.EnableSsl = false;
    //                //client.
    //                client.Send(mail);
    //                errorMessage = true;
    //            }
    //            return errorMessage;
    //        }
    //        catch (Exception)
    //        {
    //            return false;
    //        }

    //    }

    //    protected void insertarEmprendedor(EmprendedorNegocio emprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            Datos.Contacto nuevoEmprendedor = new Datos.Contacto
    //            {
    //                Nombres = emprendedor.Nombre,
    //                Apellidos = emprendedor.Apellido,
    //                CodTipoIdentificacion = emprendedor.TipoIdentificacion,
    //                Identificacion = emprendedor.Identificacion,
    //                Email = emprendedor.Email,
    //                Clave = GeneraClave(),
    //                CodInstitucion = emprendedor.CodigoUnidadEmprendimiento,
    //                CodTipoAprendiz = emprendedor.TipoAprendiz,
    //                Genero = emprendedor.Genero,
    //                FechaNacimiento = emprendedor.FechaNacimiento,
    //                CodCiudad = emprendedor.CodigoCiudadNacimiento,
    //                Telefono = emprendedor.Telefono,
    //                Direccion = emprendedor.Direccion,
    //                LugarExpedicionDI = emprendedor.CodigoCiudadExpedicion,
    //                Inactivo = false,
    //                AceptoTerminosYCondiciones = false,
    //                codCiudadResidencia = emprendedor.CodigoCiudadResidencia
    //            };

    //            db.Contacto.InsertOnSubmit(nuevoEmprendedor);
    //            db.SubmitChanges();

    //            emprendedor.Id = nuevoEmprendedor.Id_Contacto;
    //            emprendedor.Clave = nuevoEmprendedor.Clave;
    //        }
    //    }

    //    protected void actualizarEmprendedor(EmprendedorNegocio emprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            Datos.Contacto emprendedorExistente = db.Contacto.SingleOrDefault(contacto => contacto.Id_Contacto == emprendedor.Id);

    //            if (emprendedorExistente == null)
    //                throw new ApplicationException("Sucedio un error al obtener los datos del contacto, intentelo de nuevo");

    //            emprendedorExistente.Nombres = emprendedor.Nombre;
    //            emprendedorExistente.Apellidos = emprendedor.Apellido;
    //            emprendedorExistente.CodTipoIdentificacion = emprendedor.TipoIdentificacion;
    //            emprendedorExistente.Email = emprendedor.Email;
    //            emprendedorExistente.CodInstitucion = emprendedor.CodigoUnidadEmprendimiento;
    //            emprendedorExistente.CodTipoAprendiz = emprendedor.TipoAprendiz;
    //            emprendedorExistente.Genero = emprendedor.Genero;
    //            emprendedorExistente.FechaNacimiento = emprendedor.FechaNacimiento;
    //            emprendedorExistente.CodCiudad = emprendedor.CodigoCiudadNacimiento;
    //            emprendedorExistente.codCiudadResidencia = emprendedor.CodigoCiudadResidencia;
    //            emprendedorExistente.Telefono = emprendedor.Telefono;
    //            emprendedorExistente.LugarExpedicionDI = emprendedor.CodigoCiudadExpedicion;
    //            emprendedorExistente.Inactivo = false;
    //            emprendedorExistente.fechaActualizacion = DateTime.Now;
    //            emprendedorExistente.Direccion = emprendedor.Direccion;

    //            if (!emprendedor.ExisteContactoValidoInactivo)
    //                emprendedorExistente.Identificacion = emprendedor.Identificacion;

    //            if (emprendedor.ExisteContactoValidoInactivo)
    //            {
    //                emprendedorExistente.Clave = GeneraClave();
    //                emprendedorExistente.AceptoTerminosYCondiciones = false;
    //            }

    //            db.SubmitChanges();

    //            emprendedor.Clave = emprendedorExistente.Clave;
    //        }
    //    }

    //    protected void insertarGrupoContacto(EmprendedorNegocio emprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            if (!db.GrupoContactos.Any(contacto => contacto.CodContacto == emprendedor.Id))
    //            {
    //                Datos.GrupoContacto grupoContacto = new Datos.GrupoContacto
    //                {
    //                    CodGrupo = Constantes.CONST_Emprendedor,
    //                    CodContacto = emprendedor.Id
    //                };

    //                db.GrupoContactos.InsertOnSubmit(grupoContacto);
    //                db.SubmitChanges();
    //            }
    //        }
    //    }

    //    protected void insertarProyectoContacto(EmprendedorNegocio emprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            Datos.ProyectoContacto proyectoContacto = new ProyectoContacto
    //            {
    //                CodContacto = emprendedor.Id,
    //                CodProyecto = emprendedor.CodigoProyecto,
    //                CodRol = Constantes.CONST_RolEmprendedor,
    //                FechaInicio = DateTime.Now
    //            };

    //            db.ProyectoContactos.InsertOnSubmit(proyectoContacto);
    //            db.SubmitChanges();
    //        }
    //    }

    //    protected void insertarEstudioContacto(EmprendedorNegocio emprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            Datos.ContactoEstudio estudioExistente = db.ContactoEstudios.FirstOrDefault(estudio => estudio.FlagIngresadoAsesor == 1 && estudio.CodContacto == emprendedor.Id);

    //            if (estudioExistente != null)
    //            {
    //                estudioExistente.CodProgramaAcademico = emprendedor.CodigoProgramaAcademico;
    //                estudioExistente.TituloObtenido = emprendedor.ProgramaAcademico;
    //                estudioExistente.Institucion = emprendedor.InstitucionEducativa;
    //                estudioExistente.CodCiudad = emprendedor.CodigoCiudadInstitucionEducativa;
    //                estudioExistente.CodNivelEstudio = emprendedor.NivelEstudio;
    //                estudioExistente.Finalizado = emprendedor.IsEstudioFinalizado ? 1 : 0;
    //                estudioExistente.FechaInicio = emprendedor.FechaInicioEstudio;
    //                estudioExistente.FechaGrado = emprendedor.FechaGraduacionEstudio;
    //                estudioExistente.FechaFinMaterias = emprendedor.FechaFinalizacionEstudio;
    //                estudioExistente.FechaUltimoCorte = emprendedor.FechaFinalizacionEstudio;
    //                estudioExistente.SemestresCursados = emprendedor.HorasDedicadas;
    //                estudioExistente.AnoTitulo = emprendedor.AnioGraduacion;
    //                estudioExistente.fechaActualizacion = DateTime.Now;

    //                db.SubmitChanges();
    //            }
    //            else
    //            {
    //                Datos.ContactoEstudio contactoEstudio = new ContactoEstudio
    //                {
    //                    CodContacto = emprendedor.Id,
    //                    CodProgramaAcademico = emprendedor.CodigoProgramaAcademico,
    //                    TituloObtenido = emprendedor.ProgramaAcademico,
    //                    Institucion = emprendedor.InstitucionEducativa,
    //                    CodCiudad = emprendedor.CodigoCiudadInstitucionEducativa,
    //                    CodNivelEstudio = emprendedor.NivelEstudio,
    //                    Finalizado = emprendedor.IsEstudioFinalizado ? 1 : 0,
    //                    FechaInicio = emprendedor.FechaInicioEstudio,
    //                    FechaGrado = emprendedor.FechaGraduacionEstudio,
    //                    FechaFinMaterias = emprendedor.FechaFinalizacionEstudio,
    //                    FechaUltimoCorte = emprendedor.FechaFinalizacionEstudio,
    //                    SemestresCursados = emprendedor.HorasDedicadas,
    //                    AnoTitulo = emprendedor.AnioGraduacion,
    //                    FlagIngresadoAsesor = 1,
    //                    fechaCreacion = DateTime.Now
    //                };

    //                db.ContactoEstudios.InsertOnSubmit(contactoEstudio);
    //                db.SubmitChanges();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Validación para saber si el email o identificación ya esta siendo usada por otro contacto :
    //    /// </summary>
    //    /// <param name="emprendedor">Emprendedor a validar</param>
    //    /// <returns>Si existe o no en los contactos</returns>
    //    protected bool validarContactoExistente(EmprendedorNegocio emprendedor)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            Datos.Contacto emprendedorExistente = db.Contacto.FirstOrDefault(contacto => contacto.Email == emprendedor.Email || contacto.Identificacion == emprendedor.Identificacion);

    //            //Si existe algun contacto con ese email o identificacion
    //            if (emprendedorExistente != null)
    //            {
    //                // Si el contacto esta inactivo
    //                if (emprendedorExistente.Inactivo && !emprendedor.ExisteContacto)
    //                {
    //                    Datos.ProyectoContacto proyectoContacto = db.ProyectoContactos.FirstOrDefault(contacto => contacto.CodContacto == emprendedorExistente.Id_Contacto && contacto.Inactivo == true);
    //                    if (proyectoContacto != null)
    //                    {
    //                        emprendedor.Id = emprendedorExistente.Id_Contacto;
    //                        emprendedor.ExisteContactoValidoInactivo = true;
    //                        emprendedor.ExisteContacto = true;
    //                        return true;
    //                    }
    //                    else
    //                    {
    //                        return true;
    //                    }
    //                }
    //                else
    //                {
    //                    //Si el contacto esta activo y
    //                    //es el mismo que se va actualizar.
    //                    if (emprendedor.ExisteContacto && emprendedorExistente.Id_Contacto == emprendedor.Id)
    //                    {
    //                        return false;
    //                    }
    //                    //Si el contacto esta activo y no es el mismo.
    //                    else
    //                    {
    //                        return true;
    //                    }
    //                }
    //            }
    //            //Sino existe ningun contacto con ese email u identificación
    //            else
    //            {
    //                return false;
    //            }
    //        }
    //    }

    //    protected void btnActualizarEmprendedor_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            if (txtIdentificacion.Text.Equals(txtIdentificacionConfirm.Text))
    //            {
    //                int codigoPlanDeNegocio = Convert.ToInt32(hfCodigoProyecto.Value);
    //                int codigoEmprendedor = Convert.ToInt32(hfCodigoEmprendedor.Value);
    //                EmprendedorNegocio emprendedor = validarCamposObligatorios();
    //                emprendedor.ExisteContacto = true;
    //                emprendedor.Id = codigoEmprendedor;
    //                emprendedor.CodigoProyecto = codigoPlanDeNegocio;

    //                if (validarContactoExistente(emprendedor))
    //                    throw new ApplicationException("Ya existe un contacto con ese email u identificación");

    //                mostrarModalVistaPrevia();

    //                lblErrorCrearEmprendedor.Visible = false;
    //            }
    //            else
    //            {
    //                txtIdentificacionConfirm.Focus();
    //                throw new ApplicationException("Favor confirmar el numero de identificación.");
    //            }
    //        }
    //        catch (ApplicationException ex)
    //        {
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Advertencia: " + ex.Message;
    //        }
    //        catch (Exception ex)
    //        {
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor.";
    //        }
    //    }

    //    private string convertirAFecha(string _fecha)
    //    {
    //        char[] delitadores = new char[] { '/', '-' };

    //        string[] args = _fecha.Split(delitadores);

    //        //- Verificar si tiene dd/mm/yyyy ó dd-mm-yyyy
    //        if (args.Length == 3)
    //        {
    //            //- Hacer el recorrido de las partes de la fecha para hacer el autocomplete
    //            for (int i = 0; i < args.Length - 1; i++)
    //            {
    //                //- Si tiene menos de 2 caracteres significa que se tendrá que agregar el 0
    //                if (args[i].Length < 2)
    //                {
    //                    args[i] = args[i].PadLeft(2, '0');
    //                }
    //            }
    //        }

    //        String Fecha = args[0] + "/" + args[1] + "/" + args[2];

    //        return Fecha;
    //    }

    //    /// <summary>
    //    /// Validación de campos obligatorios
    //    /// </summary>
    //    /// <returns> Emprendedor </returns>
    //    private EmprendedorNegocio validarCamposObligatorios()
    //    {
    //        EmprendedorNegocio emprendedor = new EmprendedorNegocio();
    //        string nombre;
    //        string apellido;
    //        Int16 tipoIdentificacion;
    //        double identificacion;
    //        int codigoDepartamentoExpedicion;
    //        int codigoCiudadExpedicion;
    //        string email;
    //        char genero;
    //        DateTime fechaNacimiento;
    //        int codigoDepartamentoNacimiento;
    //        int codigoCiudadNacimiento;
    //        string telefonoFijo;
    //        string direccion;
    //        int codigoDepartamentoResidencia;
    //        int codigoCiudadResidencia;
    //        int nivelEstudio;
    //        int codigoInstitucionEducativa;
    //        string institucionEducativa = txtInstitucionEducativa.Text;
    //        int codigoCiudadInstitucionEducativa;
    //        int codigoProgramaRealizado;
    //        string programaRealizado = txtProgramaRealizado.Text;
    //        bool isEstudioFinalizado;
    //        DateTime fechaInicioEstudio;
    //        DateTime? fechaFinalizacionEstudio = null;
    //        DateTime? fechaGraduacionEstudio = null;
    //        int? horasDedicadas = null;
    //        int tipoAprendiz;
    //        int codigoProyecto = Convert.ToInt32(hfCodigoProyecto.Value);
    //        int codigoUnidadEmprendimiento = usuario.CodInstitucion;
    //        DateTime fechaNacimientoValidaMenor = DateTime.Now.AddYears(-90);
    //        DateTime fechaNacimientoValidaMayor = DateTime.Now.AddYears(-10);

    //        nombre = txtNombres.Text;
    //        FieldValidate.ValidateString("Nombre", nombre, true, 100);

    //        apellido = txtApellidos.Text;
    //        FieldValidate.ValidateString("Apellido", apellido, true, 100);

    //        tipoIdentificacion = Convert.ToInt16(cmbTipoDocumento.SelectedValue);

    //        FieldValidate.ValidateNumeric("Identificación", txtIdentificacion.Text, true);
    //        identificacion = Convert.ToDouble(txtIdentificacion.Text);

    //        codigoDepartamentoExpedicion = Convert.ToInt32(cmbDepartamentoExpedicion.SelectedValue);
    //        codigoCiudadExpedicion = Convert.ToInt32(cmbCiudadExpedicion.SelectedValue);

    //        email = txtEmail.Text;
    //        FieldValidate.ValidateString("Email", email, true, 100, true);

    //        FieldValidate.ValidateString("Género", cmbGenero.SelectedValue, true, 1);
    //        genero = Char.Parse(cmbGenero.SelectedValue);

    //        if (txtFechaNacimiento.Text.Length == 9)
    //            txtFechaNacimiento.Text = "0" + txtFechaNacimiento.Text;

    //        FieldValidate.ValidateString("Fecha de nacimiento", txtFechaNacimiento.Text, true);
    //        fechaNacimiento = DateTime.ParseExact(txtFechaNacimiento.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    //        FieldValidate.ValidateIsFechaEntreRango("fecha mínima " + fechaNacimientoValidaMenor.ToShortDateString(), fechaNacimientoValidaMenor, "fecha de nacimiento", fechaNacimiento, "fecha máxima " + fechaNacimientoValidaMayor.ToShortDateString(), fechaNacimientoValidaMayor);
    //        DateTime fechaInicioEstudioValida = fechaNacimiento.AddYears(6);

    //        codigoDepartamentoNacimiento = Convert.ToInt32(cmbDepartamentoNacimiento.SelectedValue);
    //        codigoCiudadNacimiento = Convert.ToInt32(cmbCiudadNacimiento.SelectedValue);
    //        telefonoFijo = txtTelefonoFijo.Text;

    //        FieldValidate.ValidateString("Teléfono", telefonoFijo, true, 100);
    //        nivelEstudio = Convert.ToInt32(cmbNivelEstudio.SelectedValue);

    //        direccion = txtDireccionEmprendedor.Text;
    //        FieldValidate.ValidateString("Direccion", direccion, true, 120);

    //        codigoDepartamentoResidencia = Convert.ToInt32(cmbDepartamentoResidencia.SelectedValue);
    //        codigoCiudadResidencia = Convert.ToInt32(cmbCiudadResidencia.SelectedValue);

    //        FieldValidate.ValidateNumeric("Programa realizado", hfcodigoProgramaRealizado.Value, true);
    //        codigoProgramaRealizado = Convert.ToInt32(hfcodigoProgramaRealizado.Value);

    //        FieldValidate.ValidateNumeric("Institución educativa", hfCodigoInstitucionEducativa.Value, true);
    //        codigoInstitucionEducativa = Convert.ToInt32(hfCodigoInstitucionEducativa.Value);

    //        codigoCiudadInstitucionEducativa = Convert.ToInt32(hfCodigoCiudadInstitucionEducativa.Value);

    //        isEstudioFinalizado = Convert.ToInt32(cmbEstadoEstudio.SelectedValue) == 0 ? false : true;

    //        FieldValidate.ValidateString("Fecha de inicio de estudios", txtFechaInicioEstudio.Text, true);

    //        string fechaIniEst = convertirAFecha(txtFechaInicioEstudio.Text);

    //        fechaInicioEstudio = DateTime.ParseExact(fechaIniEst, "dd/MM/yyyy", CultureInfo.InvariantCulture);

    //        FieldValidate.ValidateIsFechaEntreRango("Fecha minina " + fechaInicioEstudioValida.ToShortDateString(), fechaInicioEstudioValida, "Fecha de inicio estudio", fechaInicioEstudio, "Fecha maxima " + DateTime.Now.ToShortDateString(), DateTime.Now);


    //        tipoAprendiz = Convert.ToInt32(cmbTipoAprendiz.SelectedValue);

    //        if (isEstudioFinalizado)
    //        {
    //            FieldValidate.ValidateString("Fecha de finalización de estudios", txtFechaFinalizacionEstudio.Text, true);

    //            string fechaFinEst = convertirAFecha(txtFechaFinalizacionEstudio.Text);
    //            fechaFinalizacionEstudio = DateTime.ParseExact(fechaFinEst, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    //            FieldValidate.ValidateIsDateMayor("Fecha finalización de estudios", fechaFinalizacionEstudio.Value, "Fecha de hoy", DateTime.Now);

    //            FieldValidate.ValidateString("Fecha de graduación de estudios", txtFechaGraduacionEstudio.Text, true);

    //            string fechaGradoEst = convertirAFecha(txtFechaGraduacionEstudio.Text);
    //            fechaGraduacionEstudio = DateTime.ParseExact(fechaGradoEst, "dd/MM/yyyy", CultureInfo.InvariantCulture);

    //            if (FieldValidate.isFechaEntreRango(fechaInicioEstudio, fechaGraduacionEstudio.Value, fechaFinalizacionEstudio.Value))
    //                throw new ApplicationException("La fecha de graduación debe estar despues de la fecha de finalización de estudio");
    //            if (FieldValidate.isFechaMayor(fechaGraduacionEstudio.Value, DateTime.Now))
    //                throw new ApplicationException("La fecha de graduación debe estar antes de la fecha de hoy");

    //            FieldValidate.ValidateIsFechaEntreRango("Fecha de inicio ", fechaInicioEstudio, "Fecha de finalización", fechaFinalizacionEstudio.Value, "Fecha de graduación", fechaGraduacionEstudio.Value);
    //        }
    //        else
    //        {
    //            FieldValidate.ValidateNumeric("Horas dedicadas u semestre actual", txtHorasDedicadas.Text, true);
    //            horasDedicadas = Convert.ToInt32(txtHorasDedicadas.Text);
    //        }

    //        emprendedor.Nombre = nombre;
    //        emprendedor.Apellido = apellido;
    //        emprendedor.TipoIdentificacion = tipoIdentificacion;
    //        emprendedor.Identificacion = identificacion;
    //        emprendedor.CodigoDepartamentoExpedicion = codigoDepartamentoExpedicion;
    //        emprendedor.CodigoCiudadExpedicion = codigoCiudadExpedicion;
    //        emprendedor.Email = email;
    //        emprendedor.Genero = genero;
    //        emprendedor.FechaNacimiento = fechaNacimiento;
    //        emprendedor.CodigoDepartamentoNacimiento = codigoDepartamentoNacimiento;
    //        emprendedor.CodigoCiudadNacimiento = codigoCiudadNacimiento;
    //        emprendedor.Telefono = telefonoFijo;
    //        emprendedor.Direccion = direccion;
    //        emprendedor.CodigoDepartamentoResidencia = codigoDepartamentoResidencia;
    //        emprendedor.CodigoCiudadResidencia = codigoCiudadResidencia;
    //        emprendedor.NivelEstudio = nivelEstudio;
    //        emprendedor.CodigoInstitucionEducativa = codigoInstitucionEducativa;
    //        emprendedor.InstitucionEducativa = institucionEducativa;
    //        emprendedor.CodigoCiudadInstitucionEducativa = codigoCiudadInstitucionEducativa;
    //        emprendedor.CodigoProgramaAcademico = codigoProgramaRealizado;
    //        emprendedor.ProgramaAcademico = programaRealizado;
    //        emprendedor.IsEstudioFinalizado = isEstudioFinalizado;
    //        emprendedor.FechaInicioEstudio = fechaInicioEstudio;
    //        emprendedor.FechaFinalizacionEstudio = fechaFinalizacionEstudio;
    //        emprendedor.FechaGraduacionEstudio = fechaGraduacionEstudio;
    //        emprendedor.HorasDedicadas = horasDedicadas;
    //        emprendedor.TipoAprendiz = tipoAprendiz;
    //        emprendedor.CodigoProyecto = codigoProyecto;
    //        emprendedor.CodigoUnidadEmprendimiento = codigoUnidadEmprendimiento;

    //        txtNombresVistaPrevia.Text = emprendedor.Nombre;
    //        txtApellidosVistaPrevia.Text = emprendedor.Apellido;
    //        txtTipoDocumentoVistaPrevia.Text = cmbTipoDocumento.SelectedItem.Text;
    //        txtIdentificacionVistaPrevia.Text = emprendedor.Identificacion.ToString();
    //        txtDepartamentoExpedicionVistaPrevia.Text = cmbDepartamentoExpedicion.SelectedItem.Text;
    //        txtCiudadExpedicionVistaPrevia.Text = cmbCiudadExpedicion.SelectedItem.Text;
    //        txtEmailVistaPrevia.Text = emprendedor.Email;
    //        txtGeneroVistaPrevia.Text = cmbGenero.SelectedItem.Text;
    //        txtFechaNacimientoVistaPrevia.Text = emprendedor.FechaNacimiento.ToShortDateString();
    //        txtDepartamentoNacimientoVistaPrevia.Text = cmbDepartamentoNacimiento.SelectedItem.Text;
    //        txtCiudadNacimientoVistaPrevia.Text = cmbCiudadNacimiento.SelectedItem.Text;
    //        txtTelefonoFijoVistaPrevia.Text = emprendedor.Telefono;
    //        txtDireccionEmpVistaPrevia.Text = emprendedor.Direccion;
    //        txtCiudadResidencia.Text = cmbCiudadResidencia.SelectedItem.Text;
    //        txtDepartamentoResidencia.Text = cmbDepartamentoResidencia.SelectedItem.Text;
    //        txtNivelEstudioVistaPrevia.Text = cmbNivelEstudio.SelectedItem.Text;
    //        txtProgramaRealizadoVistaPrevia.Text = emprendedor.ProgramaAcademico;
    //        txtInstitucionEducativaVistaPrevia.Text = emprendedor.InstitucionEducativa;
    //        txtCiudadInstitucionVistaPrevia.Text = txtCiudadInstitucion.Text;
    //        txtEstadoEstudioVistaPrevia.Text = cmbEstadoEstudio.SelectedItem.Text;
    //        txtTipoAprendizVistaPrevia.Text = cmbTipoAprendiz.SelectedItem.Text;
    //        txtFechaInicioEstudioVistaPrevia.Text = emprendedor.FechaInicioEstudio.ToShortDateString();

    //        if (isEstudioFinalizado)
    //        {
    //            formFechaFinalizacionVistaPrevia.Visible = true;
    //            formFechaGraduacionVistaPrevia.Visible = true;
    //            formHorasDedicadasVistaPrevia.Visible = false;
    //            txtFechaFinalizacionEstudioVistaPrevia.Text = emprendedor.FechaFinalizacionEstudio.Value.ToShortDateString();
    //            txtFechaGraduacionEstudioVistaPrevia.Text = emprendedor.FechaGraduacionEstudio.Value.ToShortDateString();
    //        }
    //        else
    //        {
    //            formFechaFinalizacionVistaPrevia.Visible = false;
    //            formFechaGraduacionVistaPrevia.Visible = false;
    //            formHorasDedicadasVistaPrevia.Visible = true;
    //            txtHorasDedicadasVistaPrevia.Text = emprendedor.HorasDedicadas.Value.ToString();
    //        }

    //        return emprendedor;
    //    }

    //    protected void btnCancelar_Click(object sender, EventArgs e)
    //    {
    //        cerrarFormEmprendedor();
    //    }

    //    private void cerrarYActualizarFormEmprendedor()
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
    //    }

    //    private void cerrarFormEmprendedor()
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
    //    }

    //    public void mostrarModalVistaPrevia()
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel3, UpdatePanel3.GetType(), "show", "$(function () { $('#" + ModalVistaPrevia.ClientID + "').modal('show'); });", true);
    //        UpdatePanel3.Update();
    //    }

    //    public void cerrarModalVistaPrevia()
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalVistaPrevia.ClientID + "').modal('hide'); });", true);
    //        UpdatePanel1.Update();
    //    }

    //    protected void Button1_Click(object sender, EventArgs e)
    //    {
    //        mostrarModalVistaPrevia();
    //    }

    //    protected void btnModificarVistaPrevia_Click(object sender, EventArgs e)
    //    {
    //        cerrarModalVistaPrevia();
    //    }

    //    protected void btnActualizarEmprendedorVistaPrevia_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            int codigoPlanDeNegocio = Convert.ToInt32(hfCodigoProyecto.Value);
    //            int codigoEmprendedor = Convert.ToInt32(hfCodigoEmprendedor.Value);
    //            EmprendedorNegocio emprendedor = validarCamposObligatorios();
    //            emprendedor.ExisteContacto = true;
    //            emprendedor.Id = codigoEmprendedor;
    //            emprendedor.CodigoProyecto = codigoPlanDeNegocio;

    //            if (validarContactoExistente(emprendedor))
    //                throw new ApplicationException("Ya existe un contacto con ese email u identificación");

    //            actualizarEmprendedor(emprendedor);
    //            insertarGrupoContacto(emprendedor);
    //            insertarEstudioContacto(emprendedor);

    //            if(!enviarEmail("Registro a fondo emprender", usuario.Email, emprendedor.Email, emprendedor.Clave))
    //            {
    //                lblErrorCrearEmprendedor.Visible = true;
    //                lblErrorCrearEmprendedor.Text = "Advertencia : No se logró enviar la notificacion al emprendedor.";
    //            }

    //            cerrarYActualizarFormEmprendedor();
    //            lblErrorCrearEmprendedor.Visible = false;
    //        }
    //        catch (ApplicationException ex)
    //        {
    //            cerrarModalVistaPrevia();
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Advertencia : " + ex.Message;
    //        }
    //        catch (Exception ex)
    //        {
    //            cerrarModalVistaPrevia();
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor.";
    //        }
    //    }

    //    protected void btnGuardarEmprendedorVistaPrevia_Click(object sender, EventArgs e)
    //    {
    //        try
    //        {
    //            cerrarModalCertificadoAsesor();
    //            cerrarModalVistaPrevia();

    //            EmprendedorNegocio emprendedor = validarCamposObligatorios();

    //            if (!validarContactoExistente(emprendedor))
    //            {
    //                insertarEmprendedor(emprendedor);
    //                insertarGrupoContacto(emprendedor);
    //                insertarProyectoContacto(emprendedor);
    //                insertarEstudioContacto(emprendedor);
    //            }
    //            else
    //            {
    //                if (!emprendedor.ExisteContactoValidoInactivo)
    //                    throw new ApplicationException("Ya existe un contacto con ese email u identificación");

    //                actualizarEmprendedor(emprendedor);
    //                insertarGrupoContacto(emprendedor);
    //                insertarProyectoContacto(emprendedor);
    //                insertarEstudioContacto(emprendedor);
    //            }

    //            if(!enviarEmail("Registro a fondo emprender", usuario.Email, emprendedor.Email, emprendedor.Clave))
    //            {
    //                lblErrorCrearEmprendedor.Visible = true;
    //                lblErrorCrearEmprendedor.Text = "Advertencia : No se logró enviar el correo de notificacion al emprendedor.";
    //            }

    //            cerrarYActualizarFormEmprendedor();
    //            lblErrorCrearEmprendedor.Visible = false;
    //        }
    //        catch (ApplicationException ex)
    //        {
    //            cerrarModalCertificadoAsesor();
    //            cerrarModalVistaPrevia();
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Advertencia : " + ex.Message;
    //        }
    //        catch (Exception ex)
    //        {
    //            cerrarModalCertificadoAsesor();
    //            cerrarModalVistaPrevia();
    //            lblErrorCrearEmprendedor.Visible = true;
    //            lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el emprendedor.";
    //        }
    //    }

    //    public void mostrarModalCertificadoAsesor()
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel4, UpdatePanel4.GetType(), "show", "$(function () { $('#" + ModalCertificadoAsesor.ClientID + "').modal('show'); });", true);
    //        UpdatePanel4.Update();
    //    }

    //    public void cerrarModalCertificadoAsesor()
    //    {
    //        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "hide", "$(function () { $('#" + ModalCertificadoAsesor.ClientID + "').modal('hide'); });", true);
    //        UpdatePanel1.Update();
    //    }

    //    protected void btnCertificarRegistro_Click(object sender, EventArgs e)
    //    {

    //    }

    //    protected void btnNoCertificarRegistro_Click(object sender, EventArgs e)
    //    {
    //        cerrarModalCertificadoAsesor();
    //    }

    //    protected void btnVerCertificado_Click(object sender, EventArgs e)
    //    {
    //        EmprendedorNegocio emprendedor = validarCamposObligatorios();

    //        lblNombreAsesor.Text = usuario.Nombres;
    //        lblNombreEmprendedor.Text = emprendedor.NombreCompleto;
    //        lblCedulaEmprendedor.Text = emprendedor.Identificacion.ToString();
    //        lblNombreProyecto.Text = getNombreProyecto(emprendedor.CodigoProyecto);
    //        lblUnidadEmpredimientoTexto.Text = getUnidadEmprendimiento();
    //        lblUnidadEmpredimiento.Text = getUnidadEmprendimiento();
    //        lblNombrePerfil.Text = "Asesor";
    //        lblCedulaAsesor.Text = usuario.Identificacion.ToString();

    //        mostrarModalCertificadoAsesor();
    //    }

    //    protected string getUnidadEmprendimiento()
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var entity = db.Institucions.SingleOrDefault(filter => filter.Id_Institucion.Equals(usuario.CodInstitucion));

    //            return entity != null ? entity.NomUnidad : "Unidad de emprendimiento";
    //        }
    //    }

    //    protected string getNombreProyecto(int codigoProyecto)
    //    {
    //        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            var entity = db.Proyecto.SingleOrDefault(filter => filter.Id_Proyecto.Equals(codigoProyecto));

    //            return entity != null ? entity.NomProyecto : "Proyecto de emprendimiento";
    //        }
    //    }

    //}

    //public class TipoIdentificacion
    //{
    //    public int Id { get; set; }
    //    public string Nombre { get; set; }

    //}

    //public class NivelEstudio
    //{
    //    public int Id { get; set; }
    //    public string Nombre { get; set; }

    //}

    //public class TipoAprendiz
    //{
    //    public int Id { get; set; }
    //    public string Nombre { get; set; }

    //}

    //public class ProgramaAcademico
    //{
    //    public int Id { get; set; }
    //    private string nombre;
    //    public string Nombre
    //    {
    //        get
    //        {
    //            return this.nombre.ToUpper();
    //        }
    //        set
    //        {
    //            this.nombre = value;
    //        }
    //    }
    //    public int CodigoInstitucionEducativa { get; set; }
    //    public string InstitucionEducativa { get; set; }
    //    public int CodigoCiudad { get; set; }
    //    public string Ciudad { get; set; }
    //    public int codigoDepartamento { get; set; }
    //    public string departamento { get; set; }
    //    public int codigoNivelEstudio { get; set; }

    //}

    //public class InstitucionAcademica
    //{

    //    public int Id { get; set; }

    //    private string nombre;
    //    public string Nombre
    //    {
    //        get
    //        {
    //            return this.nombre.ToUpper();
    //        }
    //        set
    //        {
    //            this.nombre = value;
    //        }
    //    }
    //    public string Municipio { get; set; }
    //    public string Departamento { get; set; }

    //    public string Locacion
    //    {
    //        get
    //        {
    //            return Municipio + " (" + Departamento + ")";
    //        }
    //    }

    //    public string NombreYLocacion
    //    {
    //        get
    //        {
    //            return (Nombre.Substring(0, Math.Min(Nombre.Length, 50)) + " " + Locacion).ToUpper().Trim();
    //        }
    //    }
    //    public Boolean esNuevaInstitucion
    //    {
    //        get
    //        {
    //            return Id == -1 ? true : false;
    //        }
    //    }
    //}
    #endregion
}