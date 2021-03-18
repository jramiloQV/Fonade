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
using Fonade.Negocio.FonDBLight;
using Datos.Modelos;
using Datos;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento
{
    public partial class CrearActaSeguimiento : System.Web.UI.Page
    {
        public int CodigoActa
        {
            get
            {
                if (Request.QueryString["acta"] != null)
                    return Convert.ToInt32(Request.QueryString["acta"]);
                else
                    return 0;
            }
            set { }
        }

        public int CodigoProyecto
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
                txtFechaVisita.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                txtFechaFinalVisita.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                ValidateUsers();
                GetInfo();
            }
        }

        Negocio.FonDBLight.ProyectoController proyectoController = new Negocio.FonDBLight.ProyectoController();
        Negocio.FonDBLight.EmpresaController empresaController = new Negocio.FonDBLight.EmpresaController();
        Negocio.FonDBLight.ConvenioController convenioController = new Negocio.FonDBLight.ConvenioController();
        Negocio.FonDBLight.ConvocatoriaController convocatoriaController = new Negocio.FonDBLight.ConvocatoriaController();
        Negocio.FonDBLight.SectorController sectorController = new Negocio.FonDBLight.SectorController();
        Negocio.FonDBLight.ContratoController contratoController = new Negocio.FonDBLight.ContratoController();
        Negocio.FonDBLight.TipoActaSeguimientoController tipoActaSeguimiento = new Negocio.FonDBLight.TipoActaSeguimientoController();
        Negocio.FonDBLight.UsuarioController usuarioController = new Negocio.FonDBLight.UsuarioController();
        Negocio.FonDBLight.ActaSeguimientoController actaSeguimientoController = new Negocio.FonDBLight.ActaSeguimientoController();
        Negocio.FonDBLight.ActaSeguimientoDatosController actaSeguimientoDatosController = new Negocio.FonDBLight.ActaSeguimientoDatosController();

        int valorAprobadoGlobal;
        /// <summary>
        /// Carga informacion en la pagina.
        /// William PL
        /// </summary>        
        public void GetInfo()
        {
            try
            {
                var actas = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActasByProyecto(CodigoProyecto);

                int numActa = actas.Count;

                //muestra el numero del acta + 1
                lblNumActa.Text = numActa.ToString();

                //Obtenemos el contrato y las empresas
                var contrato = Negocio.PlanDeNegocioV2.Administracion.Interventoria.Abogado.GetContratoByProyecto(CodigoProyecto, Usuario.CodOperador);

                if (contrato.Any())
                {
                    if (!contrato.First().HasInfoCompleted)
                    {
                        throw new ApplicationException("Este proyecto no tiene información de contratos completa");
                    }
                    else
                    {
                        //informacion de contrato y empresa
                        var infoContrato = contrato.First().Contrato;

                        //emprendedores asociados al proyecto
                        var emprendedores = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetEmprendedoresYEquipoTrabajo(CodigoProyecto);

                        if (!emprendedores.Any())
                            throw new ApplicationException("Este proyecto no tiene emprendedores activos.");

                        string contratistas = string.Empty;

                        foreach (var emprendedor in emprendedores)
                        {
                            if (string.IsNullOrEmpty(contratistas))
                            {
                                contratistas += emprendedor.Nombres + "-" + emprendedor.Identificacion;
                            }
                            else
                            {
                                contratistas += "," + emprendedor.Nombres + "-" + emprendedor.Identificacion;
                            }
                        }
                        //Mostrar contrato
                        lblNumContrato.Text = infoContrato.NumeroContrato;

                        //Mostrar fecha publicacion acta de inicio
                        txtFechaInicio.Text = FechaActainicio(CodigoProyecto);

                        //Motrar Prorroga
                        
                        int pr = Prorroga(CodigoProyecto);
                        lblProrroga.Text = (pr == 0) ? "N/A" : pr.ToString();

                        //Obtener Nombre del proyecto                        
                        var proyecto = proyectoController.GetInfoProyecto(CodigoProyecto);
                        lblNombreProyecto.Text = proyecto.codProyecto.ToString() + " - " + proyecto.NombreProyecto;

                        //Obtener datos de la empresa
                        var empresa = empresaController.GetEmpresaByProyecto(CodigoProyecto);
                        lblNombreEmpresa.Text = empresa.razonSocial;
                        lblNitEmpresa.Text = empresa.nit;
                        lblDomicilioEmpresa.Text = empresa.Direccion;

                        //Obtener datos de convenio
                        var convenio = convenioController.GetConvenioByProyecto(CodigoProyecto);
                        lblContratoMarcoInter.Text = convenio.NomConveio;
                                                
                        lblContratistas.Text = contratistas;

                        var convocatoria = convocatoriaController.GetConvocatoriaByProyecto(CodigoProyecto);
                        lblConvocatoriaCorte.Text = convocatoria.Convocatoria;

                        var sector = sectorController.GetSectorByProyecto(CodigoProyecto);
                        lblSectorEconomico.Text = sector.NomSector;
                        lblSubSector.Text = sector.NomSubSector;

                        lblObjeto.Text = infoContrato.ObjetoContrato;

                        lblObjetivoVisita.Text = "Verificar las condiciones generales del proyecto, " +
                                                "verificar el cumplimiento de las obligaciones contractuales, " +
                                                "legales y reglamentarias, y el avance del cumplimiento de los " +
                                                "indicadores establecidos";

                        var contratoInterventor = contratoController.GetContratoByEmpresa(empresa.idEmpresa);
                        if(contratoInterventor == null)
                            throw new ApplicationException("El interventor no tiene Contrato asociado con la empresa.");

                        lblContratoInterventoria.Text = contratoInterventor.NumeroContrato;


                        //Pendiente
                        
                        lblValorAprobado.Text = FieldValidate.moneyFormat((double)infoContrato.ValorInicialEnPesos.GetValueOrDefault(0), true);

                        if (numActa>1)
                        {
                            ActaSeguimientoDatosController datosController = new ActaSeguimientoDatosController();

                            var datos = datosController.obtenerDatosActa((numActa - 1), CodigoProyecto);

                            txtCorreoGestorTecnico.Text = datos.EmailGestorTecnicoSena;
                            txtNombreGestorTecnico.Text = datos.NombreGestorTecnicoSena;
                            txtTelefonoGestorTecnico.Text = datos.TelefonoGestorTecnicoSena;

                            if (!String.IsNullOrEmpty(datos.EmailGestorOperativoSena))
                            {
                                txtCorreoGestorOperativo.Text = datos.EmailGestorOperativoSena;
                                txtNombreGestorOperativo.Text = datos.NombreGestorOperativoSena;
                                txtTelefonoGestorOperativo.Text = datos.TelefonoGestorOperativoSena;
                            }
                            else
                            {
                                DatosActaDTOController datosActaDTOController = new DatosActaDTOController();
                                var datosContac = datosActaDTOController.GetContactosProyecto(CodigoProyecto);
                                var Asesor = datosContac.Where(x => (x.codRol == Constantes.CONST_RolAsesor || x.codRol == Constantes.CONST_RolAsesorLider)).FirstOrDefault();

                                if (Asesor != null)
                                {
                                    txtNombreGestorOperativo.Text = Asesor.Nombres.ToUpper() + " " + Asesor.Apellidos.ToUpper();
                                    txtCorreoGestorOperativo.Text = Asesor.Email;
                                    txtTelefonoGestorOperativo.Text = Asesor.Telefono;
                                }
                                else
                                {
                                    Alert("El proyecto no cuenta con un asesor asignado.");
                                    return;
                                }
                            }
                            
                        }
                        else
                        {
                            DatosActaDTOController datosActaDTOController = new DatosActaDTOController();
                            var datosContac = datosActaDTOController.GetContactosProyecto(CodigoProyecto);
                            var Asesor = datosContac.Where(x => (x.codRol == Constantes.CONST_RolAsesor || x.codRol == Constantes.CONST_RolAsesorLider)).FirstOrDefault();

                            if (Asesor != null)
                            {
                                txtNombreGestorOperativo.Text = Asesor.Nombres.ToUpper() + " " + Asesor.Apellidos.ToUpper();
                                txtCorreoGestorOperativo.Text = Asesor.Email;
                                txtTelefonoGestorOperativo.Text = Asesor.Telefono;
                            }
                            else
                            {
                                Alert("El proyecto no cuenta con un asesor asignado.");
                                return;
                            }
                        }

                    }
                }
            }
            catch (ApplicationException ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                btnAdicionar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }

        private int Prorroga(int _CodProyecto)
        {
            int prorroga = 0;

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                prorroga = (from P in db.ProyectoProrrogas
                            where P.CodProyecto == _CodProyecto
                            select P.Prorroga
                                ).FirstOrDefault();


            }

            return prorroga;
        }

        private string FechaActainicio(int _CodProyecto)
        {
            string fecha = "";

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                fecha = (from A in db.ActaSeguimientoInterventoria
                                where A.IdTipoActa == 1 && A.IdProyecto == _CodProyecto && A.NumeroActa == 0
                                select A.FechaCreacion
                                ).FirstOrDefault().ToString("dd/MM/yyyy hh:mm tt");


            }

            return fecha;
        }

        public void ValidateUsers()
        {
            try
            {
                if (!(Usuario.CodGrupo == Datos.Constantes.CONST_Interventor || Usuario.CodGrupo == Datos.Constantes.CONST_CoordinadorInterventor || Usuario.CodGrupo == Datos.Constantes.CONST_GerenteInterventor))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtNombreGestorTecnico.Text.Equals(""))
                    throw new ApplicationException("El Nombre del Gestor Técnico es Obligatorio.");
                if (txtCorreoGestorTecnico.Text.Equals(""))
                    throw new ApplicationException("El Correo del Gestor Técnico es Obligatorio.");
                if (txtTelefonoGestorTecnico.Text.Equals(""))
                    throw new ApplicationException("El Telefono del Gestor Técnico es Obligatorio.");
                if (txtNombreGestorOperativo.Text.Equals(""))
                    throw new ApplicationException("El Nombre del Gestor Operativo es Obligatorio.");
                if (txtCorreoGestorOperativo.Text.Equals(""))
                    throw new ApplicationException("El Correo del Gestor Operativo es Obligatorio.");
                if (txtTelefonoGestorOperativo.Text.Equals(""))
                    throw new ApplicationException("El Telefono del Gestor Operativo es Obligatorio.");
                if (txtFechaVisita.Text.Equals(""))
                    throw new ApplicationException("La fecha de la visita es obligatoria.");

                //Obtener Tipo Acta
                var tipoActa = tipoActaSeguimiento.GetTipoActaByID(2); //2 - Acta de Seguimiento                

                //Carga datos Acta de seguimiento
                ActaSeguimientoModel actaSeguimiento = new ActaSeguimientoModel();
                actaSeguimiento.idProyecto = CodigoProyecto;
                actaSeguimiento.idTipoActa = tipoActa.idActa;
                actaSeguimiento.Nombre = tipoActa.Tipo;
                actaSeguimiento.FechaCreacion = Convert.ToDateTime(txtFechaInicio.Text);
                actaSeguimiento.Publicado = false;
                actaSeguimiento.NumeroActa = Convert.ToInt32(lblNumActa.Text);
                actaSeguimiento.idUsuarioCreacion = Usuario.IdContacto;
                actaSeguimiento.FechaPublicacion = Convert.ToDateTime(txtFechaVisita.Text);
                actaSeguimiento.FechaFinalVisita = Convert.ToDateTime(txtFechaFinalVisita.Text);
                

                string Error = "";
                //Insertar ActaSeguimiento
                if (actaSeguimientoController.InsertActaSeguimiento(ref actaSeguimiento, ref Error))
                {
                    //Alert("Insercion de acta correcta");
                    ActaSeguimientoDatosModel actadatos = new ActaSeguimientoDatosModel();
                    actadatos.idActa = actaSeguimiento.idActa;
                    actadatos.numActa = actaSeguimiento.NumeroActa;
                    actadatos.NumContrato = lblNumContrato.Text;
                    actadatos.FechaActaInicio = actaSeguimiento.FechaCreacion;
                    actadatos.FechaPublicacion = actaSeguimiento.FechaPublicacion;
                    actadatos.FechaFinalVisita = actaSeguimiento.FechaFinalVisita;
                    actadatos.Prorroga = lblProrroga.Text;
                    actadatos.NombrePlanNegocio = lblNombreProyecto.Text;
                    actadatos.NombreEmpresa = lblNombreEmpresa.Text;
                    actadatos.NitEmpresa = lblNitEmpresa.Text;
                    actadatos.ContratoMarcoInteradmin = lblContratoMarcoInter.Text;
                    actadatos.ContratoInterventoria = lblContratoInterventoria.Text;
                    actadatos.Contratista = lblContratistas.Text;
                    actadatos.ValorAprobado = lblValorAprobado.Text;
                    actadatos.DomicilioPrincipal = lblDomicilioEmpresa.Text;
                    actadatos.Convocatoria = lblConvocatoriaCorte.Text;
                    actadatos.SectorEconomico = lblSectorEconomico.Text;
                    actadatos.ObjetoProyecto = lblObjeto.Text;
                    actadatos.ObjetoVisita = lblObjetivoVisita.Text;
                    actadatos.NombreGestorTecnicoSena = txtNombreGestorTecnico.Text;
                    actadatos.EmailGestorTecnicoSena = txtCorreoGestorTecnico.Text;
                    actadatos.TelefonoGestorTecnicoSena = txtTelefonoGestorTecnico.Text;
                    actadatos.NombreGestorOperativoSena = txtNombreGestorOperativo.Text;
                    actadatos.EmailGestorOperativoSena = txtCorreoGestorOperativo.Text;
                    actadatos.TelefonoGestorOperativoSena = txtTelefonoGestorOperativo.Text;
                    actadatos.FechaActualizado = DateTime.Now;

                    

                    if (actaSeguimientoDatosController.InsertActaSeguimientoDatos(actadatos, ref Error))
                    {
                        //Alert("Insercion de acta correcta");
                        Session["idProyecto"] = CodigoProyecto;
                        Session["idActa"] = actadatos.numActa;
                        Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/Master/MainMenuActasSeg.aspx");                        
                    }
                    else
                    {
                        throw new ApplicationException("No se registraron los datos del gestor correctamente.");
                    }
                }
                else
                {
                    throw new ApplicationException("No se registró el acta correctamente.");
                }
                

                //Old Code
                //var actaInicio = Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.GetActaById(CodigoActa);

                //if (actaInicio == null)
                //    throw new ApplicationException("No se logro encontrar información de esta acta");

                //actaInicio.Publicado = true;
                //actaInicio.FechaActualizacion = DateTime.Now;

                //Negocio.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimientos.ActaSeguimiento.InsertOrUpdateActa(actaInicio);

                //Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + actaInicio.IdProyecto, true);
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + CodigoProyecto, true);
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }
    }
}