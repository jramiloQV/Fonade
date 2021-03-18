using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using Fonade.FONADE.PlandeNegocio;
using Fonade.Clases;
using System.Globalization;
using System.Net.Mail;
using System.Web.Security;
using Fonade.Error;

namespace Fonade.FONADE.MiPerfil
{
    public partial class IngresarInformacionAcademica : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Boolean esActualizacion = false;
            try
            {
                hfCodigoEmprendedor.Value = usuario.IdContacto.ToString();

                if (Request.QueryString["LoadCode"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo de estudio.");

                int codigoContactoEstudio = Convert.ToInt32(Request.QueryString["LoadCode"]);

                if (codigoContactoEstudio != 0)
                {
                    hfCodigoContactoEstudio.Value = codigoContactoEstudio.ToString();
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

                    btnCrearEmprendedor.Visible = false;
                    lblTituloCrearEmprendedor.Visible = false;                    
                }
                else
                {
                    btnActualizarEmprendedor.Visible = false;
                    lblTituloActualizarEmprendedor.Visible = false;                    

                    btnCrearEmprendedor.Visible = true;
                    lblTituloCrearEmprendedor.Visible = true;                    
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

                string url = Request.Url.ToString();

                string mensaje = ex.Message.ToString();
                string data = ex.Data.ToString();
                string stackTrace = ex.StackTrace.ToString();
                string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                // Log the error
                ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
            }
        }

        private void setDatosFormulario()
        {            
            int codigoContactoEstudio = Convert.ToInt32(hfCodigoContactoEstudio.Value);

            Datos.ContactoEstudio estudio = getEstudioContacto(codigoContactoEstudio);

            if (estudio == null)
                throw new ApplicationException("No se pudo obtener la información del estudio.");

            cmbNivelEstudio.DataBind();
            cmbNivelEstudio.ClearSelection();
            cmbNivelEstudio.Items.FindByValue(estudio.CodNivelEstudio.ToString()).Selected = true;

            if (estudio.CodProgramaAcademico != null)
            {
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

        protected Datos.ContactoEstudio getEstudioContacto(int codigoContactoEstudio)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ContactoEstudios.FirstOrDefault(estudio => estudio.Id_ContactoEstudio == codigoContactoEstudio);
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
                                     }).OrderBy(instituto => instituto.Nombre).Distinct().ToList();

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
                int codigoContacto = Convert.ToInt32(hfCodigoEmprendedor.Value);
                
                EmprendedorNegocio emprendedor = validarCamposObligatorios();
                emprendedor.Id = codigoContacto;

                insertarEstudioContacto(emprendedor);
               
                cerrarYActualizarFormEmprendedor();
                lblErrorCrearEmprendedor.Visible = false;
            }
            catch (ApplicationException ex)
            {             
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {                
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al crear el estudio.";
            }
        }

        protected void insertarEstudioContacto(EmprendedorNegocio emprendedor)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                if (emprendedor.ExisteEstudio)
                {
                    Datos.ContactoEstudio estudioExistente = db.ContactoEstudios.FirstOrDefault(estudio => estudio.Id_ContactoEstudio == emprendedor.CodigoContactoEstudio);

                    if (estudioExistente == null)
                        throw new ApplicationException("No se pudo encontrar la información del estudio.");

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
                        fechaCreacion = DateTime.Now
                    };

                    db.ContactoEstudios.InsertOnSubmit(contactoEstudio);
                    db.SubmitChanges();
                }
            }
        }

        protected void btnActualizarEmprendedor_Click(object sender, EventArgs e)
        {
            try
            {
                int codigoContacto = Convert.ToInt32(hfCodigoEmprendedor.Value);
                int codigoContactoEstudio = Convert.ToInt32(hfCodigoContactoEstudio.Value);

                EmprendedorNegocio emprendedor = validarCamposObligatorios();
                emprendedor.Id = codigoContacto;
                emprendedor.CodigoContactoEstudio = codigoContactoEstudio;
                emprendedor.ExisteEstudio = true;

                insertarEstudioContacto(emprendedor);

                cerrarYActualizarFormEmprendedor();
                lblErrorCrearEmprendedor.Visible = false;
            }
            catch (ApplicationException ex)
            {             
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {                
                lblErrorCrearEmprendedor.Visible = true;
                lblErrorCrearEmprendedor.Text = "Sucedio un error inesperado al actualizar estudio.";
            }
        }

        /// <summary>
        /// Validación de campos obligatorios
        /// </summary>
        /// <returns> Emprendedor </returns>
        private EmprendedorNegocio validarCamposObligatorios()
        {
            EmprendedorNegocio emprendedor = new EmprendedorNegocio();
                      
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
            int codigoUnidadEmprendimiento = usuario.CodInstitucion;

            nivelEstudio = Convert.ToInt32(cmbNivelEstudio.SelectedValue);

            FieldValidate.ValidateNumeric("Programa realizado", hfcodigoProgramaRealizado.Value, true);
            codigoProgramaRealizado = Convert.ToInt32(hfcodigoProgramaRealizado.Value);

            FieldValidate.ValidateNumeric("Institución educativa", hfCodigoInstitucionEducativa.Value, true);
            codigoInstitucionEducativa = Convert.ToInt32(hfCodigoInstitucionEducativa.Value);

            isEstudioFinalizado = Convert.ToInt32(cmbEstadoEstudio.SelectedValue) == 0 ? false : true;

            FieldValidate.ValidateString("Fecha de inicio de estudios", txtFechaInicioEstudio.Text, true);
            fechaInicioEstudio = DateTime.ParseExact(txtFechaInicioEstudio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            codigoCiudadInstitucionEducativa = Convert.ToInt32(hfCodigoCiudadInstitucionEducativa.Value);

            if (isEstudioFinalizado)
            {
                FieldValidate.ValidateString("Fecha de finalización de estudios", txtFechaFinalizacionEstudio.Text, true);
                fechaFinalizacionEstudio = DateTime.ParseExact(txtFechaFinalizacionEstudio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                FieldValidate.ValidateIsDateMayor("Fecha finalización de estudios", fechaFinalizacionEstudio.Value, "Fecha de hoy", DateTime.Now);

                FieldValidate.ValidateString("Fecha de graduación de estudios", txtFechaGraduacionEstudio.Text, true);
                fechaGraduacionEstudio = DateTime.ParseExact(txtFechaGraduacionEstudio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

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
            emprendedor.CodigoUnidadEmprendimiento = codigoUnidadEmprendimiento;

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

    }

    public class NivelEstudio
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

    public class EmprendedorNegocio
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
        public int CodigoProyecto { get; set; }
        public Int16 TipoIdentificacion { get; set; }
        public int CodigoDepartamentoExpedicion { get; set; }
        public int CodigoCiudadExpedicion { get; set; }
        public int CodigoDepartamentoNacimiento { get; set; }
        public int CodigoCiudadNacimiento { get; set; }
        public char Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public int NivelEstudio { get; set; }
        public int CodigoInstitucionEducativa { get; set; }
        public string InstitucionEducativa { get; set; }
        public int CodigoCiudadInstitucionEducativa { get; set; }
        public int CodigoProgramaAcademico { get; set; }
        public string ProgramaAcademico { get; set; }
        public bool IsEstudioFinalizado { get; set; }
        public DateTime FechaInicioEstudio { get; set; }
        public DateTime? FechaFinalizacionEstudio { get; set; }
        public DateTime? FechaGraduacionEstudio { get; set; }
        public int? HorasDedicadas { get; set; }
        public int? TipoAprendiz { get; set; }
        public int CodigoUnidadEmprendimiento { get; set; }
        public Int16? AnioGraduacion
        {
            get
            {
                if (IsEstudioFinalizado)
                {
                    return (Int16)this.FechaGraduacionEstudio.Value.Year;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// Si existe inactivo en la tabla contacto y es valido para usar. 
        /// </summary>
        public bool ExisteContactoValidoInactivo { get; set; }
        /// <summary>
        /// Si existe en la tabla contacto. 
        /// </summary>
        public bool ExisteContacto { get; set; }

        /// <summary>
        /// Si existe estudio
        /// </summary>
        public bool ExisteEstudio { get; set; }

        public int CodigoContactoEstudio { get; set; }
    }
}