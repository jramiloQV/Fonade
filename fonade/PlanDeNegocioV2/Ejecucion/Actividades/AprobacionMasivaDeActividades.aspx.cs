using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Ejecucion.Actividades;
using Fonade.Negocio;
using Fonade.Clases;

namespace Fonade.PlanDeNegocioV2.Ejecucion.Actividades
{
    public partial class AprobacionMasivaDeActividades : Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                gvMain.Visible = false;
                lblSubTitle.Visible = false;
                CountActividadesPendientes();
            }
        }

        protected void CountActividadesPendientes() {
            var actividadesPlanOperativo = ActividadPlanOperativo.CountTemporal(usuario.CodOperador);

            lblActividadesPlanOperativo.Text = actividadesPlanOperativo + " actividades de plan operativo  pendientes por aprobar.";

            if (actividadesPlanOperativo.Equals(0))
                btnAprobar.Enabled = false;
        }

        protected void btnAprobar_Click(object sender, EventArgs e)
        {
            List<string> resultados = new List<string>();
            var actividades = ActividadPlanOperativo.GetTemporal(usuario.CodOperador);           

            foreach (var item in actividades)
            {
                try
                {     
                    switch (item.Tarea)
                    {
                        case "Adicionar":
                            if (!ActividadPlanOperativo.ExistByName(item.CodProyecto,item.NomActividad,(Int32)item.Item)) {
                                var meses = ActividadPlanOperativo.GetMesesTemporal(item.Id_Actividad);
                                if(!meses.Count.Equals(0)) {
                                    var newEntityId = ActividadPlanOperativo.Add(item.NomActividad, item.CodProyecto, (Int32)item.Item, item.Metas);

                                    foreach (var mes in meses)
                                    {
                                        ActividadPlanOperativo.AddMeses(newEntityId, (Int16)mes.Mes, (Int16)mes.CodTipoFinanciacion, mes.Valor.GetValueOrDefault(0));
                                    }

                                    ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                                    ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);

                                    EnviarTarea(item.CodProyecto, "Plan operativo", "creada", item.NomActividad);
                                    resultados.Add("La actividad " + item.Id_Actividad + " se inserto correctamente.");
                                }
                                else
                                {
                                    ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                                    ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);
                                    throw new ApplicationException("La actividad " + item.Id_Actividad + " que intenta adicionar tiene inconsistencias en su información de meses, se procede a eliminar.");
                                }
                            }
                            else
                            {
                                ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                                ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);
                                throw new ApplicationException("La actividad " + item.Id_Actividad +" que intenta adicionar ya existe, se procede a eliminar.");
                            }                            
                            break;
                        case "Modificar":
                            if (!item.Id_Actividad.Equals(0) && !item.CodProyecto.Equals(0) ) {
                                if (ActividadPlanOperativo.Exist(item.Id_Actividad))
                                {
                                    var meses = ActividadPlanOperativo.GetMesesTemporal(item.Id_Actividad);
                                    if (!meses.Count.Equals(0))
                                    {
                                        //Update
                                        ActividadPlanOperativo.Update(item.Id_Actividad, item.NomActividad, (Int16)item.Item, item.Metas);

                                        foreach (var mes in meses)
                                        {
                                            ActividadPlanOperativo.UpdateMes(item.Id_Actividad, (Int16)mes.Mes.GetValueOrDefault(1), (Int16)mes.CodTipoFinanciacion.GetValueOrDefault(1), mes.Valor.GetValueOrDefault(0));
                                        }

                                        ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                                        ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);

                                        EnviarTarea(item.CodProyecto, "Plan operativo", "modificada", item.NomActividad);
                                        resultados.Add("La actividad " + item.Id_Actividad + " se modificó correctamente.");
                                    }
                                    else
                                    {
                                        ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                                        ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);
                                        throw new ApplicationException("La actividad " + item.Id_Actividad + " que intenta modificar tiene inconsistencias en su información de meses, se procede a eliminar.");
                                    }
                                }
                                else
                                {
                                    ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                                    ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);
                                    throw new ApplicationException("La actividad " + item.Id_Actividad + " que intenta modificar al parecer fue eliminada,no se puede continuar y se procede a eliminar.");
                                }
                            }
                            else
                            {
                                ActividadPlanOperativo.DeleteTemporalById(item.Id);
                                throw new ApplicationException("La actividad " + item.NomActividad +  " del proyecto " + item.CodProyecto + " que intenta modificar tiene información inconsistente, se procede a eliminar.");
                            }                                
                            break;
                        case "Borrar":
                            ActividadPlanOperativo.DeleteMeses(item.Id_Actividad);
                            ActividadPlanOperativo.Delete(item.Id_Actividad);

                            ActividadPlanOperativo.DeleteMesesTemporal(item.Id_Actividad);
                            ActividadPlanOperativo.DeleteTemporal(item.Id_Actividad);

                            EnviarTarea(item.CodProyecto, "Plan operativo", "eliminada", item.NomActividad);
                            resultados.Add("La actividad " + item.Id_Actividad + " se eliminó correctamente.");
                            break;
                        default:    
                            break;
                    }                  
                }
                catch (ApplicationException ex)
                {
                    resultados.Add(ex.Message);
                }
                catch (Exception ex)
                {
                    resultados.Add("Error inesperado: Actividad N° " + item.Id_Actividad + ",Tipo:" + item.Tarea + ",Proyecto:"+item.CodProyecto + ",Nombre actividad:"+ item.NomActividad + ",Error:" + ex.Message);
                }                
            }
            gvMain.DataSource = resultados;
            gvMain.DataBind();
            gvMain.Visible = true;
            lblSubTitle.Visible = true;
            CountActividadesPendientes();
        }

        protected void EnviarTarea(int codigoProyecto,string tipoActividad, string accion, string nombreActividad) {

            var codigoInterventor = ActividadPlanOperativo.GetInterventorId(codigoProyecto);

            if (codigoInterventor != null) {
            AgendarTarea agenda = new AgendarTarea(
                                                  codigoInterventor.GetValueOrDefault(),
                                                  "Actividad de "+ tipoActividad+" " + accion + " - Proyecto " + codigoProyecto,
                                                  "Revisar actividad de " + tipoActividad + " - Actividad --> " + nombreActividad + "<br>Observaciones:</br> Aprobado por gerente interventor de manera masiva.",
            codigoProyecto.ToString(),
            2,
            "0",
            false,
            1,
            true,
            false,
            usuario.IdContacto,
            "CodProyecto=" + codigoProyecto,
            "",
            "Catálogo Actividad" + tipoActividad);

            agenda.Agendar();

            }                       
        }
    }
}