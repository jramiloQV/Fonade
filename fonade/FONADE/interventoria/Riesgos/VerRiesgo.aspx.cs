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
    public partial class VerRiesgo : Negocio.Base_Page
    {
        public Boolean esActualizacion { 
            get {
                return Session["idRiesgo"] != null;
            }
            set { }
        }
        public Int32 CodigoProyecto { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");

                CodigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);        

                if (Session["idRiesgo"] != null)
                {                    
                    int idRiesgo = (int)Session["idRiesgo"];

                    var riesgo = getRiesgo(idRiesgo);

                    if (!Page.IsPostBack)
                        setDatosFormulario(riesgo);                    
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

        protected void setDatosFormulario(Riesgo riesgo) {
            txtRiesgo.Text = riesgo.Riesgos;
            txtMitigacion.Text = riesgo.Mitigacion;
            txtObservacion.Text = riesgo.Observacion;

            cmbEjeFuncional.DataBind();
            cmbEjeFuncional.ClearSelection();
            //cmbEjeFuncional.Items.FindByValue(riesgo.CodigoEjeFuncional.ToString()).Selected = true;
            cmbEjeFuncional.SelectedValue = riesgo.CodigoEjeFuncional.ToString();
        }

        protected Riesgo getRiesgo(int idRiesgo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from riesgos in db.InterventorRiesgo
                              join eje in db.EjeFuncional on riesgos.CodEjeFuncional equals eje.Id_EjeFuncional
                              where riesgos.Id_Riesgo.Equals(idRiesgo)
                              select new Riesgo
                              {
                                  Id = riesgos.Id_Riesgo,
                                  Riesgos = riesgos.Riesgo,
                                  CodigoEjeFuncional = riesgos.CodEjeFuncional.GetValueOrDefault(2),
                                  EjeFuncional = eje.NomEjeFuncional,
                                  Mitigacion = riesgos.Mitigacion,
                                  Observacion = riesgos.Observacion,
                                  CodigoProyecto = riesgos.CodProyecto
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

        protected void btnNew_Click(object sender, EventArgs e)
        {
            try
            {                
                var codigoCoordinador = getCoordinador();
                               
                var idRiesgoTmp = insertRiesgoTmp(0,CodigoProyecto,"Adicionar");

                agendarTareaCoordinador(codigoCoordinador, CodigoProyecto, idRiesgoTmp);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El riesgo que se desea adicionar se ha enviado para que el coordinador de interventoria lo apruebe.');", true);
                
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

        protected Int32 getCoordinador()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var codigoCoordinador = (from interventores in db.Interventors
                                         where interventores.CodContacto.Equals(usuario.IdContacto)
                                         select interventores.CodCoordinador).SingleOrDefault();

                if (codigoCoordinador == null)
                    throw new ApplicationException("No se puede realizar la acción porque no tiene un coordinador asignado.");

                return codigoCoordinador.Value;
            }
        }

        protected void agendarTareaCoordinador(int codigoCoordinador, int codigoProyecto, int idRiesgoTmp)
        {

            AgendarTarea agenda = new AgendarTarea(
                                    codigoCoordinador,
                                    "Revisión Riesgos al Plan Operativo",
                                    "Revisión Adición, Modificación o Borrado de Riesgos del interventor al Plan Operativo",
                                    codigoProyecto.ToString(),
                                    22,
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

        protected Int32 insertRiesgoTmp(int idRiesgo,int codigoProyecto, string accionTarea)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entity = new InterventorRiesgoTMP
                {
                    Id_Riesgo = idRiesgo,
                    CodProyecto = codigoProyecto,
                    Riesgo = txtRiesgo.Text,
                    Mitigacion = txtMitigacion.Text,
                    CodEjeFuncional = Convert.ToInt16(cmbEjeFuncional.SelectedValue),
                    Observacion = txtObservacion.Text,
                    Tarea = accionTarea
                };

                db.InterventorRiesgoTMP.InsertOnSubmit(entity);
                db.SubmitChanges();

                return entity.IdTmp;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int idRiesgo = (int)Session["idRiesgo"];

                var codigoCoordinador = getCoordinador();

                var idRiesgoTmp = insertRiesgoTmp(idRiesgo, CodigoProyecto, "Modificar");

                agendarTareaCoordinador(codigoCoordinador, CodigoProyecto, idRiesgoTmp);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El riesgo que se desea modificar se ha enviado para que el coordinador de interventoria lo apruebe.');", true);

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