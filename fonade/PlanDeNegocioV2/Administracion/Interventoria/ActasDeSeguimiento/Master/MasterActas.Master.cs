
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Runtime.Caching;
using Datos;
using Fonade.Negocio;
using Fonade.Account;
using System.Data;
using Fonade.Clases;
using System.Configuration;
using Fonade.Negocio.FonDBLight;
using Datos.Modelos;

namespace Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Master
{
    public partial class MasterActas : System.Web.UI.MasterPage
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public int CodigoProyecto
        {
            get
            {
                int id = Convert.ToInt32(Session["idProyecto"]);
                return id;
            }
            set { }
        }

        public int CodigoActa
        {
            get
            {
                int id = Convert.ToInt32(Session["idActa"]);
                return id;
            }
        }
        public String FechaInicioSesion
        {
            get
            {
                return DateTime.Now.getFechaConFormato();
            }
            set { }
        }
        public Boolean AllowCambiarProyecto
        {
            get
            {
                return Usuario.CodGrupo.Equals(Constantes.CONST_GerenteAdministrador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_AdministradorSistema)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CallCenter)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CallCenterOperador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_GerenteInterventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorInterventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_Interventor)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_GerenteEvaluador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorEvaluador)
                       || Usuario.CodGrupo.Equals(Constantes.CONST_PerfilAbogado);
            }
            set { }
        }

        public Boolean AllowShowAllProjects
        {
            get
            {
                return Usuario.CodGrupo.Equals(Constantes.CONST_LiderRegional)
                        || Usuario.CodGrupo.Equals(Constantes.CONST_JefeUnidad)
                        || Usuario.CodGrupo.Equals(Constantes.CONST_PerfilAbogado);
            }
            set { }
        }

        public FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set { }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                int value;
                if (!int.TryParse(CodigoProyecto.ToString(), out value))
                    throw new ApplicationException("No se encontro la información del proyecto, sera redireccionado al inicio de la aplicación para que lo intente de nuevo.");

                if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.ProyectoExist(CodigoProyecto))
                    throw new Exception("No se logro obtener la información necesaria para continuar.");

                GetProyectDetails();

                var esMienbro = Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.EsMienbroDelProyecto(CodigoProyecto, Usuario.IdContacto);

                if (!(AllowCambiarProyecto || esMienbro || AllowShowAllProjects))
                    throw new ApplicationException("No tiene permiso para ver este proyecto");
                if (!IsPostBack)
                {
                    lblFechaSesion.Text = DateTime.Now.ToLongDateString();
                    GetInfo();

                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + "');", true);
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error inesperado, sera redireccionado al inicio de la aplicación para que lo intente de nuevo. detalle :" + ex.Message + " ');", true);
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }
        }

        ActaSeguimientoDatosController actaSeguimientoDatosController = new ActaSeguimientoDatosController();

        public void GetInfo()
        {
            try
            {
                var datosActa = actaSeguimientoDatosController.obtenerDatosActa(CodigoActa, CodigoProyecto);
                if (datosActa.idActa != 0)
                {
                    txtFechaVisita.Text = datosActa.FechaPublicacion.Value.ToString("dd/MM/yyyy hh:mm tt");
                    txtFechaFinalVisita.Text = datosActa.FechaFinalVisita.Value.ToString("dd/MM/yyyy hh:mm tt");
                    lblIdActa.Text = datosActa.idActa.ToString();
                    lblActaNumero.Text = datosActa.numActa.ToString();
                    lblNumContrato.Text = datosActa.NumContrato.ToString();
                    lblFechaActaInicio.Text = datosActa.FechaActaInicio.ToString();
                    lblProrroga.Text = datosActa.Prorroga;
                    lblNombreProyecto.Text = datosActa.NombrePlanNegocio;
                    lblNombreEmpresa.Text = datosActa.NombreEmpresa;
                    lblNitEmpresa.Text = datosActa.NitEmpresa;
                    lblContratoMarcoInter.Text = datosActa.ContratoMarcoInteradmin.ToString();
                    lblContratoInterventoria.Text = datosActa.ContratoInterventoria.ToString();
                    lblContratistas.Text = datosActa.Contratista;
                    lblValorAprobado.Text = datosActa.ValorAprobado;
                    lblDomicilioEmpresa.Text = datosActa.DomicilioPrincipal;
                    lblConvocatoriaCorte.Text = datosActa.Convocatoria;
                    lblSectorEconomico.Text = datosActa.SectorEconomico;
                    lblSubSectorEconomico.Text = datosActa.SubSectorEconomico;
                    lblObjeto.Text = datosActa.ObjetoProyecto;
                    lblObjetivoVisita.Text = datosActa.ObjetoVisita;
                    txtNombreGestorTecnico.Text = datosActa.NombreGestorTecnicoSena;
                    txtCorreoGestorTecnico.Text = datosActa.EmailGestorTecnicoSena;
                    txtTelefonoGestorTecnico.Text = datosActa.TelefonoGestorTecnicoSena;
                    txtNombreGestorOperativo.Text = datosActa.NombreGestorOperativoSena;
                    txtCorreoGestorOperativo.Text = datosActa.EmailGestorOperativoSena;
                    txtTelefonoGestorOperativo.Text = datosActa.TelefonoGestorOperativoSena;
                }
                else
                {
                    throw new ApplicationException("No se ha guardado información para esta acta.");
                }
            }
            catch (ApplicationException ex)
            {
                btnGuardar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Advertencia: " + ex.Message;
            }
            catch (Exception ex)
            {
                btnGuardar.Visible = false;
                lblError.Visible = true;
                lblError.Text = "Error inesperado: " + ex.Message;
            }
        }

        protected void LoginStatus_LoggedOut(Object sender, System.EventArgs e)
        {
            MemoryCache.Default.Dispose();
            Session.Abandon();
        }

        protected void GetProyectDetails()
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(_cadena))
            {
                var entity = (from p in db.Proyecto1s
                              where p.Id_Proyecto == CodigoProyecto
                              orderby p.Id_Proyecto descending
                              select new
                              {
                                  codProyecto = p.Id_Proyecto,
                                  NombreProyecto = p.NomProyecto
                              }).FirstOrDefault();
                if (entity == null)
                    throw new ApplicationException("No se encontro la información del proyecto, sera redireccionado al inicio de la aplicación para que lo intente de nuevo.");

                lbl_title.Text = entity.codProyecto + " - " + entity.NombreProyecto;
            }

            lblNumActa.Text = CodigoActa.ToString();
        }

        protected void img_BuscarConsulta_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["consultarMaster"] = txt_busqueda.Value;
            Response.Redirect("/FONADE/MiPerfil/Consultas.aspx");
        }

        protected void lnkVolverConsultaActas_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + CodigoProyecto);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            ActaSeguimientoDatosModel actamodel = new ActaSeguimientoDatosModel();

            actamodel.idActa = Convert.ToInt32(lblIdActa.Text);
            actamodel.NombreGestorTecnicoSena = txtNombreGestorTecnico.Text;
            actamodel.TelefonoGestorTecnicoSena = txtTelefonoGestorTecnico.Text;
            actamodel.EmailGestorTecnicoSena = txtCorreoGestorTecnico.Text;
            actamodel.NombreGestorOperativoSena = txtNombreGestorOperativo.Text;
            actamodel.TelefonoGestorOperativoSena = txtTelefonoGestorOperativo.Text;
            actamodel.EmailGestorOperativoSena = txtCorreoGestorOperativo.Text;
            actamodel.FechaPublicacion = Convert.ToDateTime(txtFechaVisita.Text);
            actamodel.FechaFinalVisita = Convert.ToDateTime(txtFechaFinalVisita.Text);

            bool actualizado = actaSeguimientoDatosController.ActualizarDatosGestor(actamodel);

            //string script = @"<script type='text/javascript'>
            //            cerrarModal();
            //      </script>";

            //ScriptManager.RegisterStartupScript(this, typeof(Page), "cerrarModal", script, true);
        }

        protected void lnkPublicarActa_Click(object sender, EventArgs e)
        {
            string mensaje = "";
            if (validarDatos(CodigoProyecto, CodigoActa, ref mensaje))
            {
                if(publicarActa(CodigoProyecto, CodigoActa))
                {
                    Alert("Publicación Correcta del acta #" + CodigoActa);
                    Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/GestionarActas.aspx?codigo=" + CodigoProyecto);
                }                    
                else
                    Alert("No logró publicar el acta #"+CodigoActa);
            }
            else
            {
                Alert(mensaje);
            }
            
        }

        private bool publicarActa(int _codProyecto, int _numActa)
        {
            bool publicado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var Acta = (from a in db.ActaSeguimientoInterventoria
                            where a.IdProyecto == _codProyecto && a.NumeroActa == _numActa
                            select a).FirstOrDefault();

                if (Acta!= null)
                {
                    Acta.Publicado = true;
                    //Acta.FechaPublicacion = DateTime.Now;
                }

                db.SubmitChanges();

                publicado = true;
            }

            return publicado;
        }

        private bool validarDatos(int _codProyecto, int _numActa, ref string mensaje)
        {
            bool valido = true;
            mensaje = "No se ha guardado la informacion para: ";
            //Validar 2. RIESGOS IDENTIFICADOS EN EVALUACION
            if (!validarRiesgos(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "2. RIESGOS IDENTIFICADOS EN EVALUACION; ";
            }
            if (!validarGenEmpleo(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "3.1 Gestión en la Generación de Empleo; ";
            }
            if (!validarEjecPresupuestal(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "3.2 Gestión en Ejecución Presupuestal; ";
            }
            if (!validarGenMercadeo(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "3.3 Gestión en Mercadeo; ";
            }
            if (!validarContrapartida(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "3.4 Contrapartidas; ";
            }
            if (!validarGenProduccion(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "3.5 Gestión en Producción; ";
            }
            if (!validarGenVentas(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "3.6 Gestión en Ventas; ";
            }
            if (!validarObligacionesTipicas(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "4. VERIFICAR OBLIGACIONES TÍPICAS PARA LOS COMERCIANTES; ";
            }
            if (!validarOtrosAspectos(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "5. OTROS ASPECTOS DEL PLAN DE NEGOCIO; ";
            }
            if (!validarOtrasObligaciones(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "6. OTRAS OBLIGACIONES CONTRACTUALES; ";
            }
            if (!validarEstEmpresa(_codProyecto, _numActa))
            {
                valido = false;
                mensaje = mensaje + "7. ESTADO DE LA EMPRESA; ";
            }

            return valido;
        }

        private bool validarEstEmpresa(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimEstadoEmpresa
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaEstadoEmpresa).Count();
            }

            if (cant > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarOtrasObligaciones(int _codProyecto, int _numActa)
        {
            bool validado = false;

            int cantObl = 0;
            int cantAse = 0;
            int cantPlat = 0;
            int cantTiem = 0;

            if (_numActa > 1)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
                {
                    cantAse = (from r in db.ActaSeguimOtrasObligAcomAsesoria
                                where r.codProyecto == _codProyecto && r.numActa == _numActa
                                select r.idOtrasObligAcomAsesoria).Count();

                    cantPlat = (from r in db.ActaSeguimOtrasObligInfoPlataforma
                                where r.codProyecto == _codProyecto && r.numActa == _numActa
                                select r.idOtrasObligInfoPlataforma).Count();

                    cantTiem = (from r in db.ActaSeguimOtrasObligTiempoEmp
                                where r.codProyecto == _codProyecto && r.numActa == _numActa
                                select r.idOtrasObligTiempoEmprendedor).Count();

                }
            }
            else
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
                {
                    cantObl = (from r in db.ActaSeguimOtrasObligaciones
                               where r.codProyecto == _codProyecto && r.numActa == _numActa
                               select r.idOtrasObligaciones).Count();
                }
            }

            if ((cantTiem > 0 && cantPlat > 0 && cantAse > 0) || cantObl > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarOtrosAspectos(int _codProyecto, int _numActa)
        {
            bool validado = false;

            int cantInno = 0;
            int cantAmbi = 0;
            int cantAsp = 0;

            if (_numActa>1)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
                {
                    cantInno = (from r in db.ActaSeguimOtrosAspInnovador
                                where r.codProyecto == _codProyecto && r.numActa == _numActa
                                select r.idOtroAspInnovador).Count();

                    cantAmbi = (from r in db.ActaSeguimOtrosAspAmbiental
                                   where r.codProyecto == _codProyecto && r.numActa == _numActa
                                   select r.idOtrosAspAmbiental).Count();                  

                }
            }
            else
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
                {
                    cantAsp = (from r in db.ActaSeguimOtrosAspectos
                                where r.codProyecto == _codProyecto && r.numActa == _numActa
                                select r.idOtrosAspectos).Count();                    
                }
            }

            if ((cantInno > 0 && cantAmbi > 0)|| cantAsp > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarObligacionesTipicas(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cantCont = 0;
            int cantLaboral = 0;
            int cantTramite = 0;
            int cantTributaria = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cantCont = (from r in db.ActaSeguimObligacionesContables
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaSegObligTipicas).Count();

                cantLaboral = (from r in db.ActaSeguimObligacionesLaborales
                            where r.codProyecto == _codProyecto && r.numActa == _numActa
                            select r.idObligLaboral).Count();

                cantTramite = (from r in db.ActaSeguimObligacionesTramites
                               where r.codProyecto == _codProyecto && r.numActa == _numActa
                               select r.idObligTramites).Count();

                cantTributaria = (from r in db.ActaSeguimObligacionesTributarias
                               where r.codProyecto == _codProyecto && r.numActa == _numActa
                               select r.idObligTributaria).Count();

            }

            if (cantCont > 0 && cantLaboral > 0 && cantTramite > 0 && cantTributaria > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarGenVentas(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimGestionVentas
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaSegVentas).Count();
            }

            if (cant > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarGenProduccion(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;
            int cantEval = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                cantEval = (from r in db.IndicadorProductoEvaluacions
                        where r.IdProyecto == _codProyecto
                        select r.IdProducto).Count();
            }

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimGestionProduccion
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaSegProduccion).Count();
            }

            if (cant == cantEval)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarContrapartida(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimContrapartida
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaSegContrapartida).Count();
            }

            if (cant > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarGenMercadeo(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimGestionMercadeo
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaGestionMercadeo).Count();
            }

            if (cant > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarEjecPresupuestal(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimAporteEmprendedor
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaAporteEmp).Count();
            }
                        
            if (cant > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarGenEmpleo(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimGestionEmpleo
                        where r.codProyecto == _codProyecto && r.numActa == _numActa
                        select r.idActaGestionEmpleo).Count();
            }

            if (cant > 0)
            {
                validado = true;
            }
            return validado;
        }

        private bool validarRiesgos(int _codProyecto, int _numActa)
        {
            bool validado = false;
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimRiesgos
                            where r.CodProyecto == _codProyecto && r.NumActa == _numActa
                            select r.idActaRiesgo).Count();
            }

            if (cant>0)
            {
                validado = true;
            }
                return validado;
        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('"+ mensaje + "');", true);
        }
    }
}