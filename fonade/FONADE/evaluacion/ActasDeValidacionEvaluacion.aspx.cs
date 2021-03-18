using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Error;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// ActasDeValidacionEvaluacion
    /// </summary>
    
    public partial class ActasDeValidacionEvaluacion : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Session["idacta"] = null;

            if ((usuario.CodGrupo == Constantes.CONST_GerenteInterventor
                || usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor
                || usuario.CodGrupo == Constantes.CONST_Interventor)) {
                    btnAgregarActa.Visible = false;
                    imgAgregarProyecto.Visible = false;
            }

        }

        /// <summary>
        /// Gets the actas validacion.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="maxRows">The maximum rows.</param>
        /// <returns></returns>
        public List<ActaFinalDeValidacion> getActasValidacion(int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                             
                var entities = (from actas in db.EvaluacionActas
                                join convocatorias in db.Convocatoria on actas.CodConvocatoria equals convocatorias.Id_Convocatoria
                                where convocatorias.codOperador == usuario.CodOperador
                                select new ActaFinalDeValidacion
                                {
                                    Id = actas.Id_Acta,
                                    Nombre = actas.NomActa,
                                    Numero = actas.NumActa,
                                    Convocatoria = convocatorias.NomConvocatoria,
                                    Publicado = actas.publicado.GetValueOrDefault(false),
                                    GrupoUsuario = usuario.CodGrupo
                                }).OrderBy(filter => filter.Numero).Skip(startIndex).Take(maxRows).ToList();
                
                return entities;
            }
        }

        /// <summary>
        /// Gets the acta validacion count.
        /// </summary>
        /// <returns></returns>
        public int getActaValidacionCount()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from actas in db.EvaluacionActas
                                join convocatorias in db.Convocatoria on actas.CodConvocatoria equals convocatorias.Id_Convocatoria
                                where convocatorias.codOperador == usuario.CodOperador
                                select new ActaFinalDeValidacion
                                {
                                    Id = actas.Id_Acta,
                                    Nombre = actas.NomActa,
                                    Numero = actas.NumActa,
                                    Convocatoria = convocatorias.NomConvocatoria,
                                    Publicado = actas.publicado.GetValueOrDefault(false)
                                }).Count();

                return entities;
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gvActasValidacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvActasValidacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {            
            if (e.CommandName.Equals("eliminar"))
            {
                if (e.CommandArgument != null)
                {
                    try
                    {
                        int idActa = Convert.ToInt32(e.CommandArgument);

                        ActaFinalDeValidacion.EliminarActaValidacionYProyectos(idActa);

                        gvActasValidacion.DataBind();
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al eliminar el acta, intentelo de nuevo.');", true);

                        string url = Request.Url.ToString();

                        string mensaje = ex.Message.ToString();
                        string data = ex.Data.ToString();
                        string stackTrace = ex.StackTrace.ToString();
                        string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                        // Log the error
                        ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                    }                    
                }
            }
            else if (e.CommandName.Equals("actualizar"))
            {
                if (e.CommandArgument != null)
                {                   
                    int idActa = Convert.ToInt32(e.CommandArgument.ToString().Split(';')[0]);

                    Session["idacta"] = idActa;
                    Session["publicado"] = e.CommandArgument.ToString().Split(';')[1];

                    Response.Redirect("CrearActa.aspx");
                }
            }
        }
    }

    /// <summary>
    /// ActaFinalDeValidacion
    /// </summary>
    public class ActaFinalDeValidacion
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the nombre.
        /// </summary>
        /// <value>
        /// The nombre.
        /// </value>
        public string Nombre { get; set; }
        /// <summary>
        /// Gets or sets the numero.
        /// </summary>
        /// <value>
        /// The numero.
        /// </value>
        public string Numero { get; set; }
        /// <summary>
        /// Gets or sets the convocatoria.
        /// </summary>
        /// <value>
        /// The convocatoria.
        /// </value>
        public string Convocatoria { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ActaFinalDeValidacion"/> is publicado.
        /// </summary>
        /// <value>
        ///   <c>true</c> if publicado; otherwise, <c>false</c>.
        /// </value>
        public Boolean Publicado { get; set; }
        /// <summary>
        /// Gets or sets the grupo usuario.
        /// </summary>
        /// <value>
        /// The grupo usuario.
        /// </value>
        public int GrupoUsuario { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [se puede eliminar].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [se puede eliminar]; otherwise, <c>false</c>.
        /// </value>
        public Boolean SePuedeEliminar { 
            get
            {
                return !Publicado && SePuedeEditar;
            }
            set {}
        }
        /// <summary>
        /// Gets or sets a value indicating whether [se puede editar].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [se puede editar]; otherwise, <c>false</c>.
        /// </value>
        public Boolean SePuedeEditar
        {
            get {
                return !(GrupoUsuario == Constantes.CONST_GerenteInterventor
                          || GrupoUsuario == Constantes.CONST_CoordinadorInterventor
                          || GrupoUsuario == Constantes.CONST_Interventor);
            }
            set {}
        }

        /// <summary>
        /// Eliminars the acta validacion y proyectos.
        /// </summary>
        /// <param name="idActa">The identifier acta.</param>
        public static void EliminarActaValidacionYProyectos(int idActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Se eliminan los proyecto asociados al acta de validación.
                var proyectos = GetProyectosActaValidacion(idActa);
                proyectos.ForEach(proyecto => {
                    EliminarProyectoActaValidacion(proyecto);
                });

                EliminarActaValidacion(idActa);
             }
        }

        /// <summary>
        /// Eliminars the acta validacion.
        /// </summary>
        /// <param name="idActa">The identifier acta.</param>
        public static void EliminarActaValidacion(int idActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = db.EvaluacionActas.Single(filter => filter.Id_Acta.Equals(idActa));
                
                db.EvaluacionActas.DeleteOnSubmit(entity);
                db.SubmitChanges();                               
            }            
        }

        /// <summary>
        /// Eliminars the proyecto acta validacion.
        /// </summary>
        /// <param name="proyectoActa">The proyecto acta.</param>
        public static void EliminarProyectoActaValidacion(Datos.EvaluacionActaProyecto proyectoActa)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.EvaluacionActaProyectos.DeleteOnSubmit(proyectoActa);
                db.SubmitChanges();
            }
        }


        /// <summary>
        /// Gets the proyectos acta validacion.
        /// </summary>
        /// <param name="idActa">The identifier acta.</param>
        /// <returns></returns>
        public static List<Datos.EvaluacionActaProyecto> GetProyectosActaValidacion(int idActa) 
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from proyectosActa in db.EvaluacionActaProyectos where proyectosActa.CodActa.Equals(idActa) select proyectosActa).ToList();               
                
                return entities;
            }
        }
    }

}