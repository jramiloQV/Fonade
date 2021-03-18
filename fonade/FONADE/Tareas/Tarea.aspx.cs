using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Clases;

namespace Fonade.FONADE.Tareas
{
    public partial class Tarea : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["IdTarea"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo de la tarea.");
                var idTarea = Convert.ToInt32(HttpContext.Current.Session["IdTarea"]);

                var tarea = getTarea(idTarea);

                if (tarea == null)
                    throw new ApplicationException("No se pudo obtener la información de la tarea.");

                if (!Page.IsPostBack)
                    SetTarea(tarea);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void SetTarea(VerTarea tarea)
        {
            txtRemitente.Text = tarea.NombreCompletoContactoAgendo;
            txtDestinatario.Text = tarea.NombreCompletoContactoDestinatario;
            txtTipo.Text = tarea.Tipo;
            if (tarea.IdProyecto != null)
            {
                pnlPlanDeNegocio.Visible = true; txtProyecto.Text = tarea.NombreProyecto;
            }
            else
                pnlPlanDeNegocio.Visible = false;
            txtAsunto.Text = tarea.Asunto;
            txtDescripcion.Text = tarea.Descripcion;
            txtFecha.Text = tarea.FechaFormated;
            imgUrgencia.ImageUrl = tarea.Urgencia;
            lblUrgencia.Text = tarea.NombreUrgencia;
            txtObservaciones.Text = tarea.Respuesta;
            if (tarea.Finalizada)
            {
                txtObservaciones.Enabled = false;
                chkFinalizada.Checked = true;
                chkFinalizada.Enabled = false;
                btnSave.Enabled = false;
                btnSave.Visible = false;
            }

            if (tarea.RequiereRespuesta)
                lblObservacion.Text = "Observacion (*) :";

            if (tarea.Tipo.ToLower().Trim().Equals("Asignar Coordinador de Interventoria Riesgos".ToLower().Trim()) || tarea.Tipo.ToLower().Trim().Equals("Asignar Gerente Riesgos".ToLower().Trim()))
            {
                lnkSolicitud.Visible = true;
                lnkSolicitud.CommandArgument = tarea.Parametros;
            }

            if (tarea.Tipo.ToLower().Trim().Equals("Tarea especial de interventoria".ToLower().Trim()))
            {
                lnkVerTareasEspeciales.Visible = true;
                lnkVerTareasEspeciales.CommandArgument = tarea.Parametros;
            }
        }



        protected VerTarea getTarea(int idTarea)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from tarea in db.TareaUsuarios
                                join programa in db.TareaProgramas on tarea.CodTareaPrograma equals programa.Id_TareaPrograma
                                join repeticion in db.TareaUsuarioRepeticions on tarea.Id_TareaUsuario equals repeticion.CodTareaUsuario
                                join contactoAgendo in db.Contacto on tarea.CodContactoAgendo equals contactoAgendo.Id_Contacto
                                join grupoContactoAgendo in db.GrupoContactos on contactoAgendo.Id_Contacto equals grupoContactoAgendo.CodContacto
                                join contactoRemitente in db.Contacto on tarea.CodContacto equals contactoRemitente.Id_Contacto
                                join proyecto in db.Proyecto on tarea.CodProyecto equals proyecto.Id_Proyecto into proyectoTarea
                                from proyectosTareas in proyectoTarea.DefaultIfEmpty()
                                where repeticion.Id_TareaUsuarioRepeticion.Equals(idTarea)
                                select new VerTarea
                                {
                                    Tipo = programa.NomTareaPrograma,
                                    Ejecutable = programa.Ejecutable,
                                    Icono = programa.Icono,
                                    Id = tarea.Id_TareaUsuario,
                                    Asunto = tarea.NomTareaUsuario,
                                    Descripcion = tarea.Descripcion,
                                    CodigoTarea = tarea.CodTareaPrograma,
                                    RecordatorioEmail = tarea.RecordatorioEmail,
                                    NivelUrgencia = tarea.NivelUrgencia,
                                    RecordatorioPantalla = tarea.RecordatorioPantalla,
                                    RequiereRespuesta = tarea.RequiereRespuesta,
                                    CodigoContactoAgendo = tarea.CodContactoAgendo,
                                    IdTareaUsuarioRepeticion = repeticion.Id_TareaUsuarioRepeticion,
                                    Parametros = repeticion.Parametros,
                                    Fecha = repeticion.Fecha,
                                    Respuesta = repeticion.Respuesta,
                                    IdProyecto = proyectosTareas.Id_Proyecto,
                                    NombreProyecto = proyectosTareas.NomProyecto,
                                    GrupoContactoAgendo = grupoContactoAgendo.CodGrupo,
                                    NombreContactoAgendo = contactoAgendo.Nombres,
                                    ApellidoContactoAgendo = contactoAgendo.Apellidos,
                                    EmailContactoAgendo = contactoAgendo.Email,
                                    CodigoContactoDestinatario = contactoRemitente.Id_Contacto,
                                    NombreContactoDestinatario = contactoRemitente.Nombres,
                                    ApellidoContactoDestinatario = contactoRemitente.Apellidos,
                                    EmailContactoDestinatario = contactoRemitente.Email,
                                    GrupoContacto = usuario.CodGrupo,
                                    FechaCierre = repeticion.FechaCierre
                                }).SingleOrDefault();

                return entities;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (HttpContext.Current.Session["IdTarea"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo de la tarea.");
                var idTarea = Convert.ToInt32(HttpContext.Current.Session["IdTarea"]);
                var tarea = getTarea(idTarea);
                if (tarea == null)
                    throw new ApplicationException("No se pudo obtener la información de la tarea.");

                if (tarea.RequiereRespuesta)
                    FieldValidate.ValidateString("Observación de respuesta", txtObservaciones.Text, true);

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var tareaUsuario = db.TareaUsuarioRepeticions.Single(filter => filter.Id_TareaUsuarioRepeticion.Equals(tarea.IdTareaUsuarioRepeticion));
                    if (chkFinalizada.Checked)
                    {
                        tareaUsuario.FechaCierre = DateTime.Now;
                        txtObservaciones.Enabled = false;
                        chkFinalizada.Enabled = false;
                        btnSave.Enabled = false;
                        btnSave.Visible = false;
                    }

                    tareaUsuario.Respuesta = txtObservaciones.Text;

                    if (tarea.RequiereRespuesta)
                    {
                        AgendarTarea agenda = new AgendarTarea(
                                                               tarea.CodigoContactoAgendo,
                                                               "Respuesta para " + tarea.Asunto + (chkFinalizada.Checked.Equals(true) ? " (Final) " : " (Parcial) "),
                                                               "Actividad : " + tarea.Descripcion + " <BR> Respuesta : " + txtObservaciones.Text,
                                                               tarea.IdProyecto == null ? "null" : tarea.IdProyecto.ToString(),
                                                               tarea.CodigoTarea,
                                                               "0",
                                                               tarea.RecordatorioEmail,
                                                               tarea.NivelUrgencia,
                                                               false,
                                                               false,
                                                               tarea.CodigoContactoDestinatario,
                                                               null,
                                                               null,
                                                               null
                                                               );
                        agenda.Agendar();
                    }
                    db.SubmitChanges();
                    lblError.Visible = false;

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información guardada exitosamente.');", true);
                }
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia, detalle : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error al guardar la información, detalle : " + ex.Message;
            }
        }

        protected void lnkSolicitud_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            var idRiesgoTmp = Convert.ToInt32(link.CommandArgument);

            Session["idRiesgoTmp"] = idRiesgoTmp;
            Redirect(null, "~/FONADE/interventoria/Riesgos/AprobarRiesgo.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=600,top=50");
        }

        protected void lnkVerTareas_Click(object sender, EventArgs e)
        {
            LinkButton link = (LinkButton)sender;
            var tareaEspecial = link.CommandArgument;

            Response.Redirect("~/PlanDeNegocioV2/Administracion/Interventoria/TareaEspecial/VerTareaEspecial.aspx?" + tareaEspecial);
        }
    }

    public class VerTarea
    {
        public string Tipo { get; set; }
        public string Ejecutable { get; set; }
        public string Icono { get; set; }
        public int Id { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public int CodigoTarea { get; set; }
        public Boolean RecordatorioEmail { get; set; }
        public short NivelUrgencia { get; set; }
        public string Urgencia
        {
            get
            {
                if (NivelUrgencia.Equals(Constantes.CONST_PostIt))
                    return "/Images/IcoPost.gif";
                else
                    return "/Images/Tareas/Urgencia" + NivelUrgencia + ".gif";
            }
            set { }
        }
        public string NombreUrgencia
        {
            get
            {
                var nombreUrgencia = "Muy baja";

                if (NivelUrgencia.Equals(1))
                    nombreUrgencia = "Muy alta";
                if (NivelUrgencia.Equals(2))
                    nombreUrgencia = "Alta";
                if (NivelUrgencia.Equals(3))
                    nombreUrgencia = "Normal";
                if (NivelUrgencia.Equals(4))
                    nombreUrgencia = "Baja";

                return nombreUrgencia;
            }
            set { }
        }
        public Boolean RequiereRespuesta { get; set; }
        public Boolean RecordatorioPantalla { get; set; }
        public int CodigoContactoAgendo { get; set; }
        public int IdTareaUsuarioRepeticion { get; set; }
        public string Parametros { get; set; }
        public DateTime Fecha { get; set; }
        public string FechaFormated
        {
            get
            {
                return Fecha.getFechaConFormato(true);
            }
            set
            {
            }
        }
        public int? IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string NombreContactoAgendo { get; set; }
        public string ApellidoContactoAgendo { get; set; }
        public string NombreCompletoContactoAgendo
        {
            get
            {
                return hideEvaluadorNameFromEmprendedor ? "Evaluador" : NombreContactoAgendo + ' ' + ApellidoContactoAgendo;
            }
            set { }
        }
        public string EmailContactoAgendo { get; set; }
        public int GrupoContacto { get; set; }
        public int GrupoContactoAgendo { get; set; }
        public int CodigoContactoDestinatario { get; set; }
        public string NombreContactoDestinatario { get; set; }
        public string ApellidoContactoDestinatario { get; set; }
        public string NombreCompletoContactoDestinatario
        {
            get
            {
                return hideEvaluadorNameFromEmprendedor_Destinatario ? "Evaluador" : NombreContactoDestinatario + ' ' + ApellidoContactoDestinatario;
            }
            set { }
        }
        public string EmailContactoDestinatario { get; set; }
        public string Respuesta { get; set; }
        public DateTime? FechaCierre { get; set; }

        public Boolean Finalizada
        {
            get
            {
                return FechaCierre != null ? true : false;
            }
            set { }
        }

        public bool hideEvaluadorNameFromEmprendedor
        {
            get
            {
                return (GrupoContacto.Equals(Constantes.CONST_Emprendedor) 
                    || GrupoContacto.Equals(Constantes.CONST_Asesor) 
                    || GrupoContacto.Equals(Constantes.CONST_JefeUnidad)) && GrupoContactoAgendo.Equals(Constantes.CONST_Evaluador);
            }
            set { }
        }

        public bool hideEvaluadorNameFromEmprendedor_Destinatario
        {
            get
            {
                if (getIdGrupo(CodigoContactoDestinatario) == Constantes.CONST_Evaluador)
                    return true;
                else
                    return false;
            }
            set { }
        }

        public int getIdGrupo(int idContacto)
        {
            int idGrupo = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                idGrupo = (from gc in db.GrupoContactos
                           where gc.CodContacto == idContacto
                           select gc.CodGrupo).FirstOrDefault();
            }

            return idGrupo;
        }

        public string _cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
    }
}