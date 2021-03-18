using Datos;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria.Riesgos
{
    public partial class AprobarRiesgo : Negocio.Base_Page
    {      
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {      
                if (Session["idRiesgoTmp"] != null)
                {
                    int idRiesgo = (int)Session["idRiesgoTmp"];

                    var riesgo = getRiesgo(idRiesgo);

                    if (!Page.IsPostBack)
                        setDatosFormulario(riesgo);
                }
                else
                {
                    throw new ApplicationException("No se pudo obtener la información para aprobar el riesgo.");
                }
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        protected void setDatosFormulario(Riesgo riesgo)
        {
            lblTitulo.Text = riesgo.Tarea + " riesgos Identificados y Mitigación.";

            txtRiesgo.Text = riesgo.Riesgos;
            txtMitigacion.Text = riesgo.Mitigacion;
            txtObservacion.Text = riesgo.Observacion;

            cmbEjeFuncional.DataBind();
            cmbEjeFuncional.ClearSelection();           
            cmbEjeFuncional.SelectedValue = riesgo.CodigoEjeFuncional.ToString();

            if (usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorInterventor))
                chkAprobarRiesgo.Checked = riesgo.AprobadoCoordinador;
            if (usuario.CodGrupo.Equals(Constantes.CONST_GerenteInterventor))
                chkAprobarRiesgo.Checked = riesgo.AprobadoGerente;
        }

        protected Riesgo getRiesgo(int idRiesgoTmp)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from riesgoTmp in db.InterventorRiesgoTMP                              
                              where riesgoTmp.IdTmp.Equals(idRiesgoTmp)
                              select new Riesgo
                              {
                                  Id = riesgoTmp.Id_Riesgo,
                                  Riesgos = riesgoTmp.Riesgo,
                                  CodigoEjeFuncional = riesgoTmp.CodEjeFuncional.GetValueOrDefault(2),                                  
                                  Mitigacion = riesgoTmp.Mitigacion,
                                  Observacion = riesgoTmp.Observacion,
                                  CodigoProyecto = riesgoTmp.CodProyecto,
                                  Tarea = riesgoTmp.Tarea,
                                  AprobadoCoordinador = riesgoTmp.ChequeoCoordinador.GetValueOrDefault(false),
                                  AprobadoGerente = riesgoTmp.ChequeoGerente.GetValueOrDefault(false),
                              }).SingleOrDefault();

                if (entity == null)
                    throw new ApplicationException("No se encontro la información de este riesgo.");

                return entity;
            }
        }

        public List<EjeFuncional> getEjeFuncional()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from eje in db.EjeFuncional
                                select eje).ToList();
                return entities;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
        }

        protected List<Int32> getGerenteInterventor()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from grupo in db.GrupoContactos
                                         where grupo.CodGrupo.Equals( Constantes.CONST_GerenteInterventor)
                                select grupo.CodContacto).ToList();

                return entities;
            }
        }

        protected void agendarTareaGerente(int codigoGerenteInterventor, int codigoProyecto, int idRiesgoTmp)
        {
            AgendarTarea agenda = new AgendarTarea(
                                    codigoGerenteInterventor,
                                    "Revisión Riesgos al Plan Operativo",
                                    "Revisión Adición, Modificación o Borrado de Riesgos del interventor al Plan Operativo",
                                    codigoProyecto.ToString(),
                                    23,
                                    "0",
                                    false,
                                    1,
                                    false,
                                    false,
                                    usuario.IdContacto,
                                    idRiesgoTmp.ToString(),
                                    "",
                                    "");
            agenda.Agendar();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int idRiesgo = (int)Session["idRiesgoTmp"];
                var riesgo = getRiesgo(idRiesgo);
                               
                if (usuario.CodGrupo.Equals(Constantes.CONST_CoordinadorInterventor))
                {
                    if (chkAprobarRiesgo.Checked)
                    {
                        if(riesgo.Tarea.Equals("Adicionar"))
                        {
                            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                            {
                                var nuevoRiesgo = new InterventorRiesgo
                                {
                                    Riesgo = riesgo.Riesgos,
                                    Mitigacion = riesgo.Mitigacion,
                                    Observacion = riesgo.Observacion,
                                    CodProyecto = riesgo.CodigoProyecto,
                                    CodEjeFuncional = riesgo.CodigoEjeFuncional                                    
                                };

                                db.InterventorRiesgo.InsertOnSubmit(nuevoRiesgo);
                                db.SubmitChanges();
                                
                                var riesgoTmp = db.InterventorRiesgoTMP.Single(filter => filter.IdTmp.Equals(idRiesgo));

                                db.InterventorRiesgoTMP.DeleteOnSubmit(riesgoTmp);                                
                                db.SubmitChanges();

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Aprobada creación de riesgo.');", true);
                            }                            
                        }
                        if (riesgo.Tarea.Equals("Modificar"))
                        {
                            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                            {                                
                                var modificarRiesgo = db.InterventorRiesgo.Single(filter => filter.Id_Riesgo.Equals(riesgo.Id));

                                modificarRiesgo.Riesgo = riesgo.Riesgos;
                                modificarRiesgo.Mitigacion = riesgo.Mitigacion;
                                modificarRiesgo.Observacion = riesgo.Observacion;
                                modificarRiesgo.CodEjeFuncional = riesgo.CodigoEjeFuncional;
                                
                                db.SubmitChanges();

                                var riesgoTmp = db.InterventorRiesgoTMP.Single(filter => filter.IdTmp.Equals(idRiesgo));

                                db.InterventorRiesgoTMP.DeleteOnSubmit(riesgoTmp);
                                db.SubmitChanges();

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Aprobada modificación de riesgo.');", true);
                            }                          
                        }
                        if (riesgo.Tarea.Equals("Borrar"))
                        {
                            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                            {
                                var eliminarRiesgo = db.InterventorRiesgo.Single(filter => filter.Id_Riesgo.Equals(riesgo.Id));
                                db.InterventorRiesgo.DeleteOnSubmit(eliminarRiesgo);

                                var riesgoTmp = db.InterventorRiesgoTMP.Single(filter => filter.IdTmp.Equals(idRiesgo));

                                db.InterventorRiesgoTMP.DeleteOnSubmit(riesgoTmp);
                                db.SubmitChanges();

                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Aprobada eliminación de riesgo.');", true);
                            }
                        }
                    }
                }
                               
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }
    }
}