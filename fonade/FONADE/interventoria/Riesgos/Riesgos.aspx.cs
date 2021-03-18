using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Clases;

namespace Fonade.FONADE.interventoria.Riesgos
{
    public partial class Riesgos : Negocio.Base_Page
    {
        public Int32 CodigoProyecto { get; set; }

        public Boolean AllowUpdate { get {
            return usuario.CodGrupo.Equals(Constantes.CONST_Interventor);
        } set { } }
        protected void Page_Load(object sender, EventArgs e)
        {            
            try
            {
                if (Session["CodProyecto"] == null)
                    throw new ApplicationException("No se pudo obtener el codigo del proyecto, intentenlo de nuevo.");

                CodigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);                                                
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

        public List<Riesgo> getRiesgos()
        {
            var codigoProyecto = Session["CodProyecto"] != null ? Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]) : 0; 
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from riesgos in db.InterventorRiesgo
                                join eje in db.EjeFuncional on riesgos.CodEjeFuncional equals eje.Id_EjeFuncional
                                where riesgos.CodProyecto.Equals(codigoProyecto)
                                select new Riesgo{
                                    Id = riesgos.Id_Riesgo,
                                    Riesgos = riesgos.Riesgo,
                                    EjeFuncional= eje.NomEjeFuncional,
                                    Mitigacion = riesgos.Mitigacion,
                                    Observacion = riesgos.Observacion                               
                                }).ToList();
                return entities;
            }
        }

        protected void gvRiesgos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           try {
               if (e.CommandName.Equals("deleteRiesgo"))
                {
                    if (e.CommandArgument != null)
                    {
                        int idRiesgo = Convert.ToInt32(e.CommandArgument);

                        var codigoCoordinador = getCoordinador();
                        var riesgo = getRiesgo(idRiesgo);

                        riesgo.Tarea = "Borrar";

                        var idRiesgoTmp = insertRiesgoTmp(riesgo);
                        agendarTareaCoordinador(codigoCoordinador, riesgo.CodigoProyecto, idRiesgoTmp);

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El riesgo que se desea eliminar se ha enviado para que el coordinador de interventoria lo apruebe.');", true);
                    }
                }
                if (e.CommandName.Equals("updateRiesgo"))
                {
                    if (e.CommandArgument != null)
                    {
                        int idRiesgo = Convert.ToInt32(e.CommandArgument);

                        Session["idRiesgo"] = idRiesgo;

                        Redirect(null, "~/FONADE/interventoria/Riesgos/VerRiesgo.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=600,top=50");
                    }
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

        protected Riesgo getRiesgo(int idRiesgo) {
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

        protected void agendarTareaCoordinador(int codigoCoordinador, int codigoProyecto,int idRiesgoTmp) {

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

        protected Int32 insertRiesgoTmp(Riesgo riesgo) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = new InterventorRiesgoTMP
                {
                    Id_Riesgo = riesgo.Id,
                    CodProyecto = riesgo.CodigoProyecto,
                    Riesgo = riesgo.Riesgos,
                    Mitigacion = riesgo.Mitigacion,
                    CodEjeFuncional = riesgo.CodigoEjeFuncional,
                    Observacion = riesgo.Observacion,
                    Tarea = riesgo.Tarea
                };

                db.InterventorRiesgoTMP.InsertOnSubmit(entity);
                db.SubmitChanges();

                return entity.IdTmp;
            }
        }

        protected Int32 getCoordinador() {
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

        protected void lnkNewRiesgo_Click(object sender, EventArgs e)
        {
            Session["idRiesgo"] = null;
            Redirect(null, "~/FONADE/interventoria/Riesgos/VerRiesgo.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=600,top=50");
        }
        
    }

    public class Riesgo {
        public int Id { get; set; }
        public string Riesgos { get; set; }
        public short CodigoEjeFuncional { get; set; }
        public string EjeFuncional { get; set; }
        public string Mitigacion { get; set; }
        public string Observacion { get; set; }
        public Int32 CodigoProyecto { get; set; }
        public string Tarea { get; set; }

        public bool AprobadoCoordinador { get; set; }
        public bool AprobadoGerente { get; set; }
    }
}