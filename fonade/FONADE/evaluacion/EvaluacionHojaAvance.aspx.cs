#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>15 - 07 - 2014</Fecha>
// <Archivo>EvaluacionHojaAvance.cs</Archivo>

#endregion

#region using

using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionHojaAvance : Negocio.Base_Page
    {
        #region variables globaes

        int Codproyecto;
        int CodConvocatoria;

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 15 - 07 - 2014
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //recoge el id del proyecto y de la convocatoria de la sesion
            Codproyecto = HttpContext.Current.Session["Codproyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Codproyecto"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["Codproyecto"].ToString()) : 0;
            CodConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"].ToString()) : 0;

            //si no es evaluador no actualiza la informacion
            if (usuario.CodGrupo != Constantes.CONST_Evaluador)
            {
                btn_Enviar.Visible = false;
                btn_Enviar.Enabled = false;
                btn_Limpiar.Visible = false;
                btn_Limpiar.Enabled = false;
            }
            else
            {
                btn_Enviar.Visible = true;
                btn_Enviar.Enabled = true;
                btn_Limpiar.Visible = true;
                btn_Limpiar.Enabled = true;
            }

            //si no es una sobrecarga de la pagina realiza la accion
            if (!IsPostBack)
            {
                //llama al metodo que carga la informacion de evaluacion de seguimiento
                llenarInformacion();
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 15 - 07 - 2014
        /// Metodo que se encarga de llenar la tabla con la informacion
        /// de la evaluacion de seguimiento
        /// del proyecto
        /// </summary>
        private void llenarInformacion()
        {
            #region recoge y valida la informacion del seguimiento

            //recoge de la BD con Linq un objeto EvaluacionSeguimientos de acuerdo al id del proyecto y la convocatoria
            var seguimiento = (from es in consultas.Db.EvaluacionSeguimientos
                               where es.CodProyecto == Codproyecto
                               && es.CodConvocatoria == CodConvocatoria
                               select es).FirstOrDefault();

            //si existe el objeto realiza las validaciones de datos
            if (seguimiento != null)
            {
                #region activa o desatctiva segun resultado de consulta Linq y asi mismo chechead los CheckBox

                if (Convert.ToBoolean(seguimiento.LecturaPlanNegocio))
                {
                    cbx_LecturaPlanNegocio.Checked = true;
                    cbx_LecturaPlanNegocio.Enabled = false;
                }
                else
                {
                    cbx_LecturaPlanNegocio.Checked = false;
                    cbx_LecturaPlanNegocio.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.SolicitudInformacionEmprendedor))
                {
                    cbx_SolicitudInformacionEmprendedor.Checked = true;
                    cbx_SolicitudInformacionEmprendedor.Enabled = false;
                }
                else
                {
                    cbx_SolicitudInformacionEmprendedor.Checked = false;
                    cbx_SolicitudInformacionEmprendedor.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.Antecedentes))
                {
                    cbx_Antecedentes.Checked = true;
                    cbx_Antecedentes.Enabled = false;
                }
                else
                {
                    cbx_Antecedentes.Checked = false;
                    cbx_Antecedentes.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.DefinicionObjetivos))
                {
                    cbx_DefinicionObjetivos.Checked = true;
                    cbx_DefinicionObjetivos.Enabled = false;
                }
                else
                {
                    cbx_DefinicionObjetivos.Checked = false;
                    cbx_DefinicionObjetivos.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.EquipoTrabajo))
                {
                    cbx_EquipoTrabajo.Checked = true;
                    cbx_EquipoTrabajo.Enabled = false;
                }
                else
                {
                    cbx_EquipoTrabajo.Checked = false;
                    cbx_EquipoTrabajo.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.JustificacionProyecto))
                {
                    cbx_JustificacionProyecto.Checked = true;
                    cbx_JustificacionProyecto.Enabled = false;
                }
                else
                {
                    cbx_JustificacionProyecto.Checked = false;
                    cbx_JustificacionProyecto.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.ResumenEjecutivo))
                {
                    cbx_ResumenEjecutivo.Checked = true;
                    cbx_ResumenEjecutivo.Enabled = false;
                }
                else
                {
                    cbx_ResumenEjecutivo.Checked = false;
                    cbx_ResumenEjecutivo.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.CaracterizacionProducto))
                {
                    cbx_CaracterizacionProducto.Checked = true;
                    cbx_CaracterizacionProducto.Enabled = false;
                }
                else
                {
                    cbx_CaracterizacionProducto.Checked = false;
                    cbx_CaracterizacionProducto.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.EstrategiasGarantiasComercializacion))
                {
                    cbx_EstrategiasGarantiasComercializacion.Checked = true;
                    cbx_EstrategiasGarantiasComercializacion.Enabled = false;
                }
                else
                {
                    cbx_EstrategiasGarantiasComercializacion.Checked = false;
                    cbx_EstrategiasGarantiasComercializacion.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.IdentificacionMercadoObjetivo))
                {
                    cbx_IdentificacionMercadoObjetivo.Checked = true;
                    cbx_IdentificacionMercadoObjetivo.Enabled = false;
                }
                else
                {
                    cbx_IdentificacionMercadoObjetivo.Checked = false;
                    cbx_IdentificacionMercadoObjetivo.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.IdentificacionEvaluacionCanales))
                {
                    cbx_IdentificacionEvaluacionCanales.Checked = true;
                    cbx_IdentificacionEvaluacionCanales.Enabled = false;
                }
                else
                {
                    cbx_IdentificacionEvaluacionCanales.Checked = false;
                    cbx_IdentificacionEvaluacionCanales.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.ProyeccionVentas))
                {
                    cbx_ProyeccionVentas.Checked = true;
                    cbx_ProyeccionVentas.Enabled = false;
                }
                else
                {
                    cbx_ProyeccionVentas.Checked = false;
                    cbx_ProyeccionVentas.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.CaracterizacionTecnicaProductoServicio))
                {
                    cbx_CaracterizacionTecnicaProductoServicio.Checked = true;
                    cbx_CaracterizacionTecnicaProductoServicio.Enabled = false;
                }
                else
                {
                    cbx_CaracterizacionTecnicaProductoServicio.Checked = false;
                    cbx_CaracterizacionTecnicaProductoServicio.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.DefinicionProcesoProduccionImplementarIndicesTecnicos))
                {
                    cbx_DefinicionProcesoProduccionImplementarIndicesTecnicos.Checked = true;
                    cbx_DefinicionProcesoProduccionImplementarIndicesTecnicos.Enabled = false;
                }
                else
                {
                    cbx_DefinicionProcesoProduccionImplementarIndicesTecnicos.Checked = false;
                    cbx_DefinicionProcesoProduccionImplementarIndicesTecnicos.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.IdentificacionValoracionRequerimientosEquipamiento))
                {
                    cbx_IdentificacionValoracionRequerimientosEquipamiento.Checked = true;
                    cbx_IdentificacionValoracionRequerimientosEquipamiento.Enabled = false;
                }
                else
                {
                    cbx_IdentificacionValoracionRequerimientosEquipamiento.Checked = false;
                    cbx_IdentificacionValoracionRequerimientosEquipamiento.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.ProgramaProduccion))
                {
                    cbx_ProgramaProduccion.Checked = true;
                    cbx_ProgramaProduccion.Enabled = false;
                }
                else
                {
                    cbx_ProgramaProduccion.Checked = false;
                    cbx_ProgramaProduccion.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.AnalisisTramitesRequisitosLegales))
                {
                    cbx_AnalisisTramitesRequisitosLegales.Checked = true;
                    cbx_AnalisisTramitesRequisitosLegales.Enabled = false;
                }
                else
                {
                    cbx_AnalisisTramitesRequisitosLegales.Checked = false;
                    cbx_AnalisisTramitesRequisitosLegales.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.CompromisosInstitucionalesPrivadosPublicos))
                {
                    cbx_CompromisosInstitucionalesPrivadosPublicos.Checked = true;
                    cbx_CompromisosInstitucionalesPrivadosPublicos.Enabled = false;
                }
                else
                {
                    cbx_CompromisosInstitucionalesPrivadosPublicos.Checked = false;
                    cbx_CompromisosInstitucionalesPrivadosPublicos.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.OrganizacionEmpresarialPropuesta))
                {
                    cbx_OrganizacionEmpresarialPropuesta.Checked = true;
                    cbx_OrganizacionEmpresarialPropuesta.Enabled = false;
                }
                else
                {
                    cbx_OrganizacionEmpresarialPropuesta.Checked = false;
                    cbx_OrganizacionEmpresarialPropuesta.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.CuantificacionInversionRequerida))
                {
                    cbx_CuantificacionInversionRequerida.Checked = true;
                    cbx_CuantificacionInversionRequerida.Enabled = false;
                }
                else
                {
                    cbx_CuantificacionInversionRequerida.Checked = false;
                    cbx_CuantificacionInversionRequerida.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.PerspectivasRentabilidad))
                {
                    cbx_PerspectivasRentabilidad.Checked = true;
                    cbx_PerspectivasRentabilidad.Enabled = false;
                }
                else
                {
                    cbx_PerspectivasRentabilidad.Checked = false;
                    cbx_PerspectivasRentabilidad.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.EstadosFinancieros))
                {
                    cbx_EstadosFinancieros.Checked = true;
                    cbx_EstadosFinancieros.Enabled = false;
                }
                else
                {
                    cbx_EstadosFinancieros.Checked = false;
                    cbx_EstadosFinancieros.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.PresupuestosCostosProduccion))
                {
                    cbx_PresupuestosCostosProduccion.Checked = true;
                    cbx_PresupuestosCostosProduccion.Enabled = false;
                }
                else
                {
                    cbx_PresupuestosCostosProduccion.Checked = false;
                    cbx_PresupuestosCostosProduccion.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.PresupuestoIngresosOperacion))
                {
                    cbx_PresupuestoIngresosOperacion.Checked = true;
                    cbx_PresupuestoIngresosOperacion.Enabled = false;
                }
                else
                {
                    cbx_PresupuestoIngresosOperacion.Checked = false;
                    cbx_PresupuestoIngresosOperacion.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.ContemplaManejoAmbiental))
                {
                    cbx_ContemplaManejoAmbiental.Checked = true;
                    cbx_ContemplaManejoAmbiental.Enabled = false;
                }
                else
                {
                    cbx_ContemplaManejoAmbiental.Checked = false;
                    cbx_ContemplaManejoAmbiental.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.ModeloFinanciera))
                {
                    cbx_ModeloFinanciera.Checked = true;
                    cbx_ModeloFinanciera.Enabled = false;
                }
                else
                {
                    cbx_ModeloFinanciera.Checked = false;
                    cbx_ModeloFinanciera.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.IndicesRentabilidad))
                {
                    cbx_IndicesRentabilidad.Checked = true;
                    cbx_IndicesRentabilidad.Enabled = false;
                }
                else
                {
                    cbx_IndicesRentabilidad.Checked = false;
                    cbx_IndicesRentabilidad.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.Viabilidad))
                {
                    cbx_Viabilidad.Checked = true;
                    cbx_Viabilidad.Enabled = false;
                }
                else
                {
                    cbx_Viabilidad.Checked = false;
                    cbx_Viabilidad.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.IndicadoresGestion))
                {
                    cbx_IndicadoresGestion.Checked = true;
                    cbx_IndicadoresGestion.Enabled = false;
                }
                else
                {
                    cbx_IndicadoresGestion.Checked = false;
                    cbx_IndicadoresGestion.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.PlanOperativo))
                {
                    cbx_PlanOperativo.Checked = true;
                    cbx_PlanOperativo.Enabled = false;
                }
                else
                {
                    cbx_PlanOperativo.Checked = false;
                    cbx_PlanOperativo.Enabled = true;
                }

                if (Convert.ToBoolean(seguimiento.InformeEvaluacion))
                {
                    cbx_InformeEvaluacion.Checked = true;
                    cbx_InformeEvaluacion.Enabled = false;
                }
                else
                {
                    cbx_InformeEvaluacion.Checked = false;
                    cbx_InformeEvaluacion.Enabled = true;
                }

                #endregion
            }
            else
            {
                foreach(Control control in form1.Controls)
                {
                    if (control is CheckBox)
                    {
                        ((CheckBox)control).Checked = false;
                    }
                }
            }

            #endregion
        }

        private EvaluacionSeguimiento retornaSeguimiento(ref EvaluacionSeguimiento seguimiento)
        {
            seguimiento.LecturaPlanNegocio = cbx_LecturaPlanNegocio.Checked;
            seguimiento.SolicitudInformacionEmprendedor = cbx_SolicitudInformacionEmprendedor.Checked;
            seguimiento.Antecedentes = cbx_Antecedentes.Checked;
            seguimiento.DefinicionObjetivos = cbx_DefinicionObjetivos.Checked;
            seguimiento.EquipoTrabajo = cbx_EquipoTrabajo.Checked;
            seguimiento.JustificacionProyecto = cbx_JustificacionProyecto.Checked;
            seguimiento.ResumenEjecutivo = cbx_ResumenEjecutivo.Checked;
            seguimiento.CaracterizacionProducto = cbx_CaracterizacionProducto.Checked;
            seguimiento.EstrategiasGarantiasComercializacion = cbx_EstrategiasGarantiasComercializacion.Checked;
            seguimiento.IdentificacionMercadoObjetivo = cbx_IdentificacionMercadoObjetivo.Checked;
            seguimiento.IdentificacionEvaluacionCanales = cbx_IdentificacionEvaluacionCanales.Checked;
            seguimiento.ProyeccionVentas = cbx_ProyeccionVentas.Checked;
            seguimiento.CaracterizacionTecnicaProductoServicio = cbx_CaracterizacionTecnicaProductoServicio.Checked;
            seguimiento.DefinicionProcesoProduccionImplementarIndicesTecnicos = cbx_DefinicionProcesoProduccionImplementarIndicesTecnicos.Checked;
            seguimiento.IdentificacionValoracionRequerimientosEquipamiento = cbx_IdentificacionValoracionRequerimientosEquipamiento.Checked;
            seguimiento.ProgramaProduccion = cbx_ProgramaProduccion.Checked;
            seguimiento.AnalisisTramitesRequisitosLegales = cbx_AnalisisTramitesRequisitosLegales.Checked;
            seguimiento.CompromisosInstitucionalesPrivadosPublicos = cbx_CompromisosInstitucionalesPrivadosPublicos.Checked;
            seguimiento.OrganizacionEmpresarialPropuesta = cbx_OrganizacionEmpresarialPropuesta.Checked;
            seguimiento.CuantificacionInversionRequerida = cbx_CuantificacionInversionRequerida.Checked;
            seguimiento.PerspectivasRentabilidad = cbx_PerspectivasRentabilidad.Checked;
            seguimiento.EstadosFinancieros = cbx_EstadosFinancieros.Checked;
            seguimiento.PresupuestosCostosProduccion = cbx_PresupuestosCostosProduccion.Checked;
            seguimiento.PresupuestoIngresosOperacion = cbx_PresupuestoIngresosOperacion.Checked;
            seguimiento.ContemplaManejoAmbiental = cbx_ContemplaManejoAmbiental.Checked;
            seguimiento.ModeloFinanciera = cbx_ModeloFinanciera.Checked;
            seguimiento.IndicesRentabilidad = cbx_IndicesRentabilidad.Checked;
            seguimiento.Viabilidad = cbx_Viabilidad.Checked;
            seguimiento.IndicadoresGestion = cbx_IndicadoresGestion.Checked;
            seguimiento.PlanOperativo = cbx_PlanOperativo.Checked;
            seguimiento.InformeEvaluacion = cbx_InformeEvaluacion.Checked;

            return seguimiento;
        }

        /// <summary>
        /// Diego Quiñonez
        /// 15 - 07 - 2014
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Enviar_Click(object sender, EventArgs e)
        {
            //recoge de la BD con Linq un objeto EvaluacionSeguimientos de acuerdo al id del proyecto y la convocatoria
            var seguimiento = (from es in consultas.Db.EvaluacionSeguimientos
                               where es.CodProyecto == Codproyecto
                               && es.CodConvocatoria == CodConvocatoria
                               select es).FirstOrDefault();

            #region recoge nuevos datos para ser actualizados en la BD dentro de un objeto de tipo EvaluacionSeguimientos

            if (seguimiento != null)
            {
                retornaSeguimiento(ref seguimiento);
            }
            else
            {
                EvaluacionSeguimiento seguimiento1 = new EvaluacionSeguimiento();

                retornaSeguimiento(ref seguimiento1);

                seguimiento1.CodContacto = usuario.IdContacto;
                seguimiento1.CodConvocatoria = CodConvocatoria;
                seguimiento1.CodProyecto = Codproyecto;
                seguimiento1.FechaActualizacion = DateTime.Now;

                consultas.Db.EvaluacionSeguimientos.InsertOnSubmit(seguimiento1);
            }
            #endregion

            try
            {
                ////completa la accion para insertar una fila a la tabla en BD de EvaluacionSeguimientos
                //consultas.Db.EvaluacionSeguimientos.InsertOnSubmit(seguimiento);
                //envio los cambios realizados
                consultas.Db.SubmitChanges();

                llenarInformacion();
            }
            catch (Exception)
            {
                ClientScriptManager cm = this.ClientScript;
                cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('No se pudo actualizar la informacion e evaluacion de seguimiento');</script>");
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 15 - 07 - 2014
        /// metodo disparado al onclick del boton btn_limpiar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_Limpiar_Click(object sender, EventArgs e)
        {
            //se vuelve a cargar la infomacion inicial de la evaluacion de seguimiento
            llenarInformacion();
        }
    }
}