using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Clases;
using Fonade.Negocio;

namespace Fonade.FONADE.PlandeNegocio
{
    /// <summary>
    /// Creación de planes de negocio
    /// Author : @marztres
    /// </summary>
    public partial class CrearPlanDeNegocio : Negocio.Base_Page
    {
        public int Id_VersionProyecto
        {
            get { return (int)Session["IdVersionProyecto"]; }
            set { Session["IdVersionProyecto"] = value; }
        }


        private PlanDeNegocioDetalle planNegocioActual;
        private Boolean esActualizacion = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.AllKeys.Contains("IdVersionProyecto"))
                    Id_VersionProyecto = int.Parse(Request.QueryString["IdVersionProyecto"]);
                
                //plan actual=1
                if (Id_VersionProyecto.Equals(1))
                {
                    LabelSector.Text = "Sector:";
                    LabelLugar.Text = "Lugar de Ejecución:";
                }
                else
                {
                    LabelSector.Text = "¿En que sector se encuentra clasificado el proyecto a desarrollar?:";
                    LabelLugar.Text = "¿En dónde se localizará la empresa?:";
                }


                if (Session["codigoPlanDeNegocio"] != null)
                {
                    btnActualizarPlanDeNegocio.Visible = true;
                    lblTituloActualizarPlan.Visible = true;
                    esActualizacion = true;

                    int codigoPlanDeNegocio = (int)Session["codigoPlanDeNegocio"];

                    planNegocioActual = getPlanDeNegocio(codigoPlanDeNegocio);

                    if (!Page.IsPostBack)
                        setDatosFormulario();

                    if (esActualizacion)
                    {
                        gvEmprendedores.Visible = true;
                        gvEmprendedores.DataSourceID = "dataEmprendedores";
                        gvEmprendedores.DataBind();
                        Session["codigoEmprendedor"] = null;
                    }
                }
                else
                {
                    txtNombre.Enabled = true;
                    btnCrearPlanDeNegocio.Visible = true;
                    lblTituloCrearPlan.Visible = true;
                }
                lblError.Visible = false;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        private void setDatosFormulario()
        {
            txtNombre.Text = planNegocioActual.Nombre.htmlDecode();
            txtDescripcion.Text = planNegocioActual.Descripcion.htmlDecode();

            cmbDepartamento.DataBind();
            cmbDepartamento.ClearSelection();
            cmbDepartamento.Items.FindByValue(planNegocioActual.CodigoDepartamento.ToString()).Selected = true;

            cmbCiudad.DataBind();
            cmbCiudad.ClearSelection();
            cmbCiudad.Items.FindByValue(planNegocioActual.CodigoCiudad.ToString()).Selected = true;

            cmbSector.DataBind();
            cmbSector.ClearSelection();
            cmbSector.Items.FindByValue(planNegocioActual.CodigoSector.ToString()).Selected = true;

            cmbSubSector.DataBind();
            cmbSubSector.ClearSelection();
            cmbSubSector.Items.FindByValue(planNegocioActual.CodigoSubSector.ToString()).Selected = true;

            // Si el proyecto esta en etapa de inscripción 
            // se permite agregar emprendedores y eliminarlos  
            if (planNegocioActual.Estado == Constantes.CONST_Inscripcion)
            {
                imageAdicionEmprendedor.Visible = true;
                linkAdicionarEmprendedor.Visible = true;
                gvEmprendedores.Columns[0].Visible = true;
                txtNombre.Enabled = true;
            }
            else
            {
                imageAdicionEmprendedor.Visible = false;
                linkAdicionarEmprendedor.Visible = false;
                gvEmprendedores.Columns[0].Visible = false;
                txtNombre.Enabled = false;
                cmbDepartamento.Enabled = false;
                cmbCiudad.Enabled = false;
                cmbSector.Enabled = false;
                cmbSubSector.Enabled = false;
                txtDescripcion.Enabled = false;
            }
        }

        private PlanDeNegocioDetalle getPlanDeNegocio(int codigoPlanDeNegocio)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var planDeNegocio = (from planes in db.Proyecto
                                     join ciudadNegocio in db.Ciudad on planes.CodCiudad equals ciudadNegocio.Id_Ciudad
                                     join departamentoNegocio in db.departamento on ciudadNegocio.CodDepartamento equals departamentoNegocio.Id_Departamento
                                     join subsector in db.SubSector on planes.CodSubSector equals subsector.Id_SubSector
                                     join sector in db.Sector on subsector.CodSector equals sector.Id_Sector
                                     where planes.CodContacto == usuario.IdContacto
                                           && planes.CodInstitucion == usuario.CodInstitucion
                                           && planes.Inactivo == false
                                           && planes.Id_Proyecto == codigoPlanDeNegocio
                                     select new PlanDeNegocioDetalle
                                     {
                                         Id = planes.Id_Proyecto,
                                         Nombre = planes.NomProyecto,
                                         Descripcion = planes.Sumario,
                                         CodigoCiudad = planes.CodCiudad,
                                         CodigoDepartamento = ciudadNegocio.CodDepartamento,
                                         CodigoSubSector = planes.CodSubSector,
                                         CodigoSector = subsector.CodSector,
                                         Estado = planes.CodEstado
                                     }).FirstOrDefault();

                return planDeNegocio;
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

                departamentos.Insert(0, new Departamento() { Id=0,Nombre= "Seleccione un departamento" });
                
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

                ciudades.Insert(0,new Ciudad() { Id = 0, CodigoDepartamento = 0 , Nombre="Seleccione una ciudad" });
                                
                return ciudades;
            }
        }

        /// <summary>
        /// Obtiene el listado de las sectores
        /// <returns>Lista de sectores</returns>
        public List<SectorNegocio> getSectores()
        {
            int codigoSectorSeleccionado = 0;
            if (Session["codigoPlanDeNegocio"] != null)
            {
                int codigoPlanDeNegocio = (int)Session["codigoPlanDeNegocio"];

                planNegocioActual = getPlanDeNegocio(codigoPlanDeNegocio);
                codigoSectorSeleccionado = planNegocioActual.CodigoSector;
            }

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var sectores = (from sector in db.Sector
                                where sector.Activo == true || sector.Id_Sector == codigoSectorSeleccionado
                                orderby sector.NomSector 
                                select new SectorNegocio
                                {
                                    Id = sector.Id_Sector,
                                    Nombre = sector.NomSector
                                }                               
                               ).ToList();

                sectores.Insert(0, new SectorNegocio() { Id = 0, Nombre = "Seleccione un sector" });

                return sectores;
            }
        }

        /// <summary>
        /// Obtiene el listado de los subsectores
        /// </summary>
        /// <returns>Lista de subsectores</returns>
        public List<SubSectorNegocio> getSubSectores(int codigoSector)
        {
            int codigoSubSectorSeleccionado = 0;
            if (Session["codigoPlanDeNegocio"] != null)
            {
                int codigoPlanDeNegocio = (int)Session["codigoPlanDeNegocio"];

                planNegocioActual = getPlanDeNegocio(codigoPlanDeNegocio);
                codigoSubSectorSeleccionado = planNegocioActual.CodigoSubSector;
            }

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var subsectores = (from subsector in db.SubSector
                                   where subsector.CodSector == codigoSector 
                                   && (subsector.Activo == true || subsector.Id_SubSector == codigoSubSectorSeleccionado)
                                   orderby subsector.NomSubSector
                                   select new SubSectorNegocio
                                   {
                                       Id = subsector.Id_SubSector,
                                       Nombre = subsector.Codigo + " - " + subsector.NomSubSector,
                                       CodigoSector = subsector.CodSector
                                   }
                                    ).ToList();

                subsectores.Insert(0, new SubSectorNegocio() { Id=0,Nombre="Seleccione un subsector",CodigoSector=0});

                return subsectores;
            }
        }

        protected void btnCrearPlanDeNegocio_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreProyecto = txtNombre.Text;
                string descripcionProyecto = txtDescripcion.Text;

                FieldValidate.ValidateString("Nombre del proyecto", nombreProyecto, true, 300);
                FieldValidate.ValidateString("Descripción del proyecto", descripcionProyecto, true, 5000);

                FieldValidate.ValidateString("Departamento", cmbDepartamento.SelectedValue, true, 0, false, true);
                FieldValidate.ValidateString("Ciudad", cmbCiudad.SelectedValue, true, 0, false, true);
                FieldValidate.ValidateString("Sector", cmbSector.SelectedValue, true, 0, false, true);
                FieldValidate.ValidateString("Subsector", cmbSubSector.SelectedValue, true, 0, false, true);

                int codigodDepartamento = Convert.ToInt32(cmbDepartamento.SelectedValue);
                int codigoCiudad = Convert.ToInt32(cmbCiudad.SelectedValue);
                int codigoSector = Convert.ToInt32(cmbSector.SelectedValue);
                int codigoSubsector = Convert.ToInt32(cmbSubSector.SelectedValue);

                if (existePlanDeNegocioDuplicado(nombreProyecto.htmlEncode()))
                    throw new ApplicationException("Ya existe un plan de negocio con ese mismo nombre.");

                Proyecto1 nuevoProyecto = new Proyecto1
                {
                    NomProyecto = Id_VersionProyecto.Equals(Constantes.CONST_PlanV2) ? nombreProyecto.ToUpper() : nombreProyecto.ToUpper().htmlEncode(),
                    Sumario = Id_VersionProyecto.Equals(Constantes.CONST_PlanV2) ? descripcionProyecto : descripcionProyecto.htmlEncode(),
                    FechaCreacion = DateTime.Now,
                    CodTipoProyecto = 1,
                    CodEstado = Constantes.CONST_Inscripcion,
                    CodCiudad = codigoCiudad,
                    CodSubSector = codigoSubsector,
                    CodContacto = usuario.IdContacto,
                    CodInstitucion = usuario.CodInstitucion,
                    IdVersionProyecto = Id_VersionProyecto
                };

                insertPlanDeNegocio(nuevoProyecto);
                agendarTareaJefeUnidad(nuevoProyecto.Id_Proyecto);
                Session["codigoPlanDeNegocio"] = nuevoProyecto.Id_Proyecto;
                Response.Redirect(Request.RawUrl, false);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        /// <summary>
        /// Agendar tarea a jefe de unidad
        /// </summary>
        /// <param name="codigoNuevoProyecto">Codigo del proyecto que fue creado</param>
        public void agendarTareaJefeUnidad(int codigoNuevoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var jefeDeUnidad = (from institucion in consultas.Db.InstitucionContacto where institucion.FechaFin == null && institucion.CodInstitucion == usuario.CodInstitucion select institucion).FirstOrDefault();

                if (jefeDeUnidad != null)
                {
                    AgendarTarea nuevaTarea = new AgendarTarea(
                                              jefeDeUnidad.CodContacto,
                                              "Registro Plan de Negocio",
                                              "Registro Plan de Negocio",
                                              codigoNuevoProyecto.ToString(),
                                              1,
                                              "0",
                                              true,
                                              0,
                                              true,
                                              true,
                                              usuario.IdContacto,
                                              string.Empty,
                                              string.Empty,
                                              string.Empty);
                    nuevaTarea.Agendar();
                }
            }
        }

        /// <summary>
        /// Insertar plan de negocio nuevo
        /// </summary>
        /// <param name="nuevoProyecto">Objecto de tipo proyecto de base de datos</param>
        public void insertPlanDeNegocio(Proyecto1 nuevoProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.Proyecto1s.InsertOnSubmit(nuevoProyecto);
                db.SubmitChanges();

                foreach (ProyectoGastosModelo modeloDeGasto in db.ProyectoGastosModelos)
                {
                    ProyectoGasto nuevoGasto = new ProyectoGasto
                    {
                        CodProyecto = nuevoProyecto.Id_Proyecto,
                        Descripcion = modeloDeGasto.Descripcion,
                        Valor = 0,
                        Tipo = modeloDeGasto.Tipo,
                        Protegido = true
                    };

                    db.ProyectoGastos.InsertOnSubmit(nuevoGasto);
                    db.SubmitChanges();
                }
            }
        }

        /// <summary>
        /// Verifica si existe un plan de negocio con nombre duplicado
        /// </summary>        
        /// <param name="nombre">Nombre del proyecto</param>
        /// <param name="codigoProyecto"> Codigo del proyecto opcional </param>
        /// <returns>Si existe un plan de negocio con nombre duplicado</returns>
        public Boolean existePlanDeNegocioDuplicado(string nombre, int codigoProyecto = 0)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var existePlanesDeNegocio = (from planes in db.Proyecto
                                             where
                                                  planes.NomProyecto.Equals(nombre) &&
                                                  (codigoProyecto == 0 || (codigoProyecto != 0 && planes.Id_Proyecto != codigoProyecto))
                                             select planes).Any();
                return existePlanesDeNegocio;
            }
        }

        protected void btnActualizarPlanDeNegocio_Click(object sender, EventArgs e)
        {
            try
            {
                string nombreProyecto = txtNombre.Text;
                string descripcionProyecto = txtDescripcion.Text;

                FieldValidate.ValidateString("Nombre del proyecto", nombreProyecto, true, 300);
                FieldValidate.ValidateString("Descripción del proyecto", descripcionProyecto, true, 5000);

                FieldValidate.ValidateString("Departamento", cmbDepartamento.SelectedValue, true,0,false,true);
                FieldValidate.ValidateString("Ciudad", cmbCiudad.SelectedValue, true, 0, false, true);
                FieldValidate.ValidateString("Sector", cmbSector.SelectedValue, true, 0, false, true);
                FieldValidate.ValidateString("Subsector", cmbSubSector.SelectedValue, true, 0, false, true);

                int codigodDepartamento = Convert.ToInt32(cmbDepartamento.SelectedValue);
                int codigoCiudad = Convert.ToInt32(cmbCiudad.SelectedValue);
                int codigoSector = Convert.ToInt32(cmbSector.SelectedValue);
                int codigoSubsector = Convert.ToInt32(cmbSubSector.SelectedValue);
                
                if (existePlanDeNegocioDuplicado(nombreProyecto, planNegocioActual.Id))
                    throw new ApplicationException("Ya existe un plan de negocio con ese mismo nombre.");

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    Proyecto1 planActual = db.Proyecto1s.Single(proyecto => proyecto.Id_Proyecto == planNegocioActual.Id);

                    planActual.NomProyecto = Id_VersionProyecto.Equals(Constantes.CONST_PlanV2) ? nombreProyecto.ToUpper() : nombreProyecto.ToUpper().htmlEncode();
                    planActual.Sumario = Id_VersionProyecto.Equals(Constantes.CONST_PlanV2) ? descripcionProyecto : descripcionProyecto.htmlEncode();
                    planActual.CodCiudad = codigoCiudad;
                    planActual.CodSubSector = codigoSubsector;

                    db.SubmitChanges();
                }

                Response.Redirect("~/FONADE/PlandeNegocio/PlanDeNegocio.aspx", false);
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Lo sentimos sucedio un error inesperado, intentalo de nuevo por favor !";
            }
        }

        /// <summary>
        /// Obtiene el listado de emprendedores
        /// </summary>
        /// <returns>Lista de emprendedores</returns>
        public List<EmprendedorNegocio> getEmprendedores()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var emprendedores = (from proyectocontacto in db.ProyectoContactos
                                     join contacto in db.Contacto on proyectocontacto.CodContacto equals contacto.Id_Contacto
                                     where proyectocontacto.Inactivo == false
                                           && proyectocontacto.CodProyecto == (int)Session["codigoPlanDeNegocio"]
                                           && proyectocontacto.CodRol == Constantes.CONST_RolEmprendedor
                                     orderby contacto.Nombres
                                     select new EmprendedorNegocio
                                     {
                                         Id = contacto.Id_Contacto,
                                         Nombre = contacto.Nombres,
                                         Apellido = contacto.Apellidos,
                                         Email = contacto.Email,
                                         CodigoProyecto = (int)Session["codigoPlanDeNegocio"]
                                     }
                                    ).ToList();
                return emprendedores;
            }
        }

        protected void gvEmprendedores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("deleteEmprendedor"))
            {
                try
                {
                    if (e.CommandArgument != null)
                    {
                        string[] parametros;
                        parametros = e.CommandArgument.ToString().Split(';');

                        int codigoProyecto = Convert.ToInt32(parametros[0]);
                        int codigoContacto = Convert.ToInt32(parametros[1]);

                        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                        {
                            var tareasPendientes = (from tareas in db.TareaUsuarios
                                                    join tareaRepeticion in db.TareaUsuarioRepeticions on tareas.Id_TareaUsuario equals tareaRepeticion.CodTareaUsuario
                                                    where tareas.CodContacto == codigoContacto
                                                          && tareaRepeticion.FechaCierre == null
                                                    select tareas).Any();
                            if (tareasPendientes)
                                throw new ApplicationException("No se puede eliminar el emprendedor porque tiene tareas pendientes");

                            GrupoContacto grupocontacto = db.GrupoContactos.Single(grupo => grupo.CodContacto.Equals(codigoContacto));

                            db.GrupoContactos.DeleteOnSubmit(grupocontacto);

                            ProyectoContacto proyectocontacto = db.ProyectoContactos.Single(proyecto => proyecto.CodContacto == codigoContacto
                                                                                            && proyecto.CodProyecto == codigoProyecto
                                                                                            && proyecto.Inactivo == false
                                                                                            && proyecto.CodRol == Constantes.CONST_RolEmprendedor);
                            proyectocontacto.Inactivo = true;
                            proyectocontacto.FechaFin = DateTime.Now;

                            Contacto contacto = db.Contacto.Single(contactoProyecto => contactoProyecto.Id_Contacto == codigoContacto);
                            contacto.Inactivo = true;

                            lblError.Visible = false;
                            db.SubmitChanges();
                            gvEmprendedores.DataBind();
                        }
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
                    lblError.Text = "Sucedio un error inesperado al eliminar el emprendedor.";
                }
            }
            else if (e.CommandName.Equals("updateEmprendedor"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros;
                    parametros = e.CommandArgument.ToString().Split(';');

                    int codigoProyecto = Convert.ToInt32(parametros[0]);
                    int codigoContacto = Convert.ToInt32(parametros[1]);

                    Session["codigoEmprendedor"] = codigoContacto;

                    Redirect(null, "~/FONADE/PlandeNegocio/CrearEmprendedor.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=600,top=50");
                }
            }
        }

        protected void linkAdicionarEmprendedor_Click(object sender, EventArgs e)
        {
            Session["codigoEmprendedor"] = null;
            Redirect(null, "~/FONADE/PlandeNegocio/CrearEmprendedor.aspx", "_blank", "menubar=0,scrollbars=1,width=1024,height=600,top=50");
        }

        protected void imageAdicionEmprendedor_Click(object sender, ImageClickEventArgs e)
        {
            Session["codigoEmprendedor"] = null;
            Redirect(null, "~/FONADE/PlandeNegocio/CrearEmprendedor.aspx", "_blank", "menubar=0,scrollbars=1,width=1024,height=600,top=50");
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

    public class SectorNegocio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }

    public class SubSectorNegocio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public int CodigoSector { get; set; }
    }

    public class EmprendedorNegocio
    {
        public int Id { get; set; }
        public double Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Clave { get; set; }
        public string Direccion { get; set; }
        public string NombreCompleto
        {
            get
            {
                return Nombre + " " + Apellido;
            }
        }
        public int CodigoProyecto { get; set; }
        public bool AllowUpdate {
            get {
                return Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.getEstadoProyecto(CodigoProyecto) == Constantes.CONST_Inscripcion;
            }
            set {                
            }
        }
        public Int16 TipoIdentificacion { get; set; }
        public int CodigoDepartamentoExpedicion
        {
            get
            {
                if (this.CodigoCiudadExpedicion == 0)
                {
                    return 0;
                }
                else
                {
                    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        return db.Ciudad.First(filter => filter.Id_Ciudad.Equals(this.CodigoCiudadExpedicion)).CodDepartamento;
                    }
                }
            }
            set { }
        }
        public int CodigoCiudadExpedicion { get; set; }
        public int CodigoDepartamentoNacimiento
        {
            get
            {
                if (this.CodigoCiudadNacimiento == 0)
                {
                    return 0;
                }
                else
                {
                    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        return db.Ciudad.First(filter => filter.Id_Ciudad.Equals(this.CodigoCiudadNacimiento)).CodDepartamento;
                    }
                }
            }
            set { }
        }
        public int CodigoCiudadNacimiento { get; set; }
        public int CodigoDepartamentoResidencia
        {
            get
            {
                if (this.CodigoCiudadResidencia == 0)
                {
                    return 0;
                }
                else
                {
                    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        return db.Ciudad.First(filter => filter.Id_Ciudad.Equals(this.CodigoCiudadResidencia)).CodDepartamento;
                    }
                }
            }
            set { }
        }
        public int CodigoCiudadResidencia { get; set; }
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
        
    }
}