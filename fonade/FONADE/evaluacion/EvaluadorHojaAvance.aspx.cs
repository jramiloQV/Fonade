#region region using 

using System;
using System.Linq;
using System.Web.UI.WebControls;
using Fonade.Negocio;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluadorHojaAvance : Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarSeguimiento();
            }
        }

        
        public void CargarSeguimiento()
        {
            string mensajeDeError = string.Empty;

            try
            {
                //var query = consultas.HojaEvaluacion(usuario.IdContacto, ref mensajeDeError);
                var idEvaluador = (from e in consultas.Db.Evaluadors where e.CodCoordinador == usuario.IdContacto select e.CodContacto).FirstOrDefault();
                var query = consultas.HojaEvaluacion(idEvaluador>0?1:0,usuario.IdContacto, ref mensajeDeError);

                if (query.Any())
                {
                    DtSeguimiento.DataSource = query;
                    DtSeguimiento.DataBind();
                    lblmensaje.Visible = false;
                }
                else
                {
                    lblmensaje.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string CargarContacto(int codcontacto)
        {
            string contacto = string.Empty;

            try
            {
                var query = consultas.Db.Contacto.FirstOrDefault(c => c.Id_Contacto == codcontacto);
               
                if (query!=null && query.Id_Contacto!=0)
                {
                    contacto = query.Nombres + ' ' + query.Apellidos;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return contacto;
        }

        protected void DtSeguimientoItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var idcontacto = e.Item.FindControl("lcontacto") as Label;
                var evaluador = e.Item.FindControl("levaluador") as Label;

                //checkbox
                var cLecturaPlanNegocio = e.Item.FindControl("cLecturaPlanNegocio") as CheckBox;
                var cSolicitudInformacionEmprendedor =
                    e.Item.FindControl("cSolicitudInformacionEmprendedor") as CheckBox;
                var cAntecedentes = e.Item.FindControl("cAntecedentes") as CheckBox;
                var cDefinicionObjetivos = e.Item.FindControl("cDefinicionObjetivos") as CheckBox;
                var cEquipoTrabajo = e.Item.FindControl("cEquipoTrabajo") as CheckBox;
                var cJustificacionProyecto = e.Item.FindControl("cJustificacionProyecto") as CheckBox;
                var cResumenEjecutivo = e.Item.FindControl("cResumenEjecutivo") as CheckBox;
                var cCaracterizacionProducto = e.Item.FindControl("cCaracterizacionProducto") as CheckBox;
                var cIdentificacionMercadoObjetivo = e.Item.FindControl("cIdentificacionMercadoObjetivo") as CheckBox;
                var cEstrategiasGarantiasComercializacion =
                    e.Item.FindControl("cEstrategiasGarantiasComercializacion") as CheckBox;
                var cIdentificacionEvaluacionCanales =
                    e.Item.FindControl("cIdentificacionEvaluacionCanales") as CheckBox;
                var cProyeccionVentas = e.Item.FindControl("cProyeccionVentas") as CheckBox;
                var cCaracterizacionTecnicaProductoServicio =
                    e.Item.FindControl("cCaracterizacionTecnicaProductoServicio") as CheckBox;
                var cDefinicionProcesoProduccionImplementarIndicesTecnicos =
                    e.Item.FindControl("cDefinicionProcesoProduccionImplementarIndicesTecnicos") as CheckBox;

                var cIdentificacionValoracionRequerimientosEquipamiento =
                    e.Item.FindControl("cIdentificacionValoracionRequerimientosEquipamiento") as CheckBox;
                var cProgramaProduccion = e.Item.FindControl("cProgramaProduccion") as CheckBox;
                var cAnalisisTramitesRequisitosLegales =
                    e.Item.FindControl("cAnalisisTramitesRequisitosLegales") as CheckBox;
                var cCompromisosInstitucionalesPrivadosPublicos =
                    e.Item.FindControl("cCompromisosInstitucionalesPrivadosPublicos") as CheckBox;
                var cOrganizacionEmpresarialPropuesta =
                    e.Item.FindControl("cOrganizacionEmpresarialPropuesta") as CheckBox;

                var cCuantificacionInversionRequerida =
                    e.Item.FindControl("cCuantificacionInversionRequerida") as CheckBox;
                var cPerspectivasRentabilidad = e.Item.FindControl("cPerspectivasRentabilidad") as CheckBox;
                var cEstadosFinancieros = e.Item.FindControl("cEstadosFinancieros") as CheckBox;
                var cPresupuestosCostosProduccion = e.Item.FindControl("cPresupuestosCostosProduccion") as CheckBox;
                var cPresupuestoIngresosOperacion = e.Item.FindControl("cPresupuestoIngresosOperacion") as CheckBox;

                var cContemplaManejoAmbiental = e.Item.FindControl("cContemplaManejoAmbiental") as CheckBox;
                var cModeloFinanciera = e.Item.FindControl("cModeloFinanciera") as CheckBox;
                var cIndicesRentabilidad = e.Item.FindControl("cIndicesRentabilidad") as CheckBox;
                var cViabilidad = e.Item.FindControl("cViabilidad") as CheckBox;
                var cIndicadoresGestion = e.Item.FindControl("cIndicadoresGestion") as CheckBox;
                var cPlanOperativo = e.Item.FindControl("cPlanOperativo") as CheckBox;
                var cInformeEvaluacion = e.Item.FindControl("cInformeEvaluacion") as CheckBox;

                /* ************************ VALIDACIONES ********************************************/

                if (idcontacto != null && !string.IsNullOrEmpty(idcontacto.Text))
                {
                    if (evaluador != null)
                    {
                        evaluador.Text = CargarContacto(Convert.ToInt32(idcontacto.Text));
                    }
                }

                /* ************************************************************************************/


                /* ************************ VALIDACIONES SI ESTA CHEQUEADO O NO PARA DESHABILIATAR ****************************************/

                if (cLecturaPlanNegocio != null)
                {
                    if (cLecturaPlanNegocio.Text == "True")
                    {
                        cLecturaPlanNegocio.Checked = true;
                        cLecturaPlanNegocio.Text = string.Empty;
                        cLecturaPlanNegocio.Enabled = false;
                    }
                    else
                    {
                        cLecturaPlanNegocio.Text = string.Empty;
                        cLecturaPlanNegocio.Enabled = false;
                    }
                }

                if (cSolicitudInformacionEmprendedor != null)
                {
                    if (cSolicitudInformacionEmprendedor.Text == "True")
                    {
                       
                        cSolicitudInformacionEmprendedor.Text = string.Empty;
                        cSolicitudInformacionEmprendedor.Enabled = false;
                        cSolicitudInformacionEmprendedor.Checked = true;
                    }
                    else
                    {
                        cSolicitudInformacionEmprendedor.Text = string.Empty;
                        cSolicitudInformacionEmprendedor.Enabled = false;
                    }
                }

                if (cAntecedentes != null)
                {
                    if (cAntecedentes.Text == "True")
                    {
                        cAntecedentes.Text = string.Empty;
                        cAntecedentes.Enabled = false;
                        cAntecedentes.Checked = true;
                    }
                    else
                    {
                        cAntecedentes.Text = string.Empty;
                        cAntecedentes.Enabled = false;
                    }
                }


                if (cDefinicionObjetivos != null)
                {
                    if (cDefinicionObjetivos.Text == "True")
                    {
                        cDefinicionObjetivos.Text = string.Empty;
                        cDefinicionObjetivos.Checked = true;
                        cDefinicionObjetivos.Enabled = false;
                    }
                    else
                    {
                        cDefinicionObjetivos.Enabled = false;
                        cDefinicionObjetivos.Text = string.Empty;
                    }
                }

                if (cEquipoTrabajo != null)
                {
                    if (cEquipoTrabajo.Text == "True")
                    {
                        cEquipoTrabajo.Text = string.Empty;
                        cEquipoTrabajo.Checked = true;
                        cEquipoTrabajo.Enabled = false;
                    }
                    else
                    {
                        cEquipoTrabajo.Enabled = false;
                        cEquipoTrabajo.Text = string.Empty;
                    }
                }

                if (cJustificacionProyecto != null)
                {
                    if (cJustificacionProyecto.Text == "True")
                    {
                        cJustificacionProyecto.Text = string.Empty;
                        cJustificacionProyecto.Checked = true;
                        cJustificacionProyecto.Enabled = false;
                    }
                    else
                    {
                        cJustificacionProyecto.Text = string.Empty;
                        cJustificacionProyecto.Enabled = false;
                    }
                }


                if (cResumenEjecutivo != null)
                {
                    if (cResumenEjecutivo.Text == "True")
                    {
                        cResumenEjecutivo.Text = string.Empty;
                        cResumenEjecutivo.Checked = true;
                        cResumenEjecutivo.Enabled = false;
                    }
                    else
                    {
                        cResumenEjecutivo.Text = string.Empty;
                        cResumenEjecutivo.Enabled = false;
                    }
                }
                if (cCaracterizacionProducto != null)
                {
                    if (cCaracterizacionProducto.Text == "True")
                    {
                        cCaracterizacionProducto.Text = string.Empty;
                        cCaracterizacionProducto.Checked = true;
                        cCaracterizacionProducto.Enabled = false;
                    }
                    else
                    {
                        cCaracterizacionProducto.Text = string.Empty;
                        cCaracterizacionProducto.Enabled = false;
                    }
                }

                if (cIdentificacionMercadoObjetivo != null)
                {
                    if (cIdentificacionMercadoObjetivo.Text == "True")
                    {

                        cIdentificacionMercadoObjetivo.Text = string.Empty;
                        cIdentificacionMercadoObjetivo.Checked = true;
                        cIdentificacionMercadoObjetivo.Enabled = false;
                    }
                    else
                    {
                        cIdentificacionMercadoObjetivo.Text = string.Empty;
                        cIdentificacionMercadoObjetivo.Enabled = false;
                    }
                }


                if (cEstrategiasGarantiasComercializacion != null)
                {
                    if (cEstrategiasGarantiasComercializacion.Text == "True")
                    {
                        cEstrategiasGarantiasComercializacion.Text = string.Empty;
                        cEstrategiasGarantiasComercializacion.Checked = true;
                        cEstrategiasGarantiasComercializacion.Enabled = false;
                    }
                    else
                    {
                        cEstrategiasGarantiasComercializacion.Text = string.Empty;
                        cEstrategiasGarantiasComercializacion.Enabled = false;
                    }
                }

                if (cIdentificacionEvaluacionCanales != null)
                {
                    if (cIdentificacionEvaluacionCanales.Text == "True")
                    {
                        cIdentificacionEvaluacionCanales.Text = string.Empty;
                        cIdentificacionEvaluacionCanales.Checked = true;
                        cIdentificacionEvaluacionCanales.Enabled = false;
                    }
                    else
                    {
                        cIdentificacionEvaluacionCanales.Text = string.Empty;
                        cIdentificacionEvaluacionCanales.Enabled = false;
                    }
                }

                if (cProyeccionVentas != null)
                {
                    if (cProyeccionVentas.Text == "True")
                    {
                        cProyeccionVentas.Text = string.Empty;
                        cProyeccionVentas.Checked = true;
                        cProyeccionVentas.Enabled = false;
                    }
                    else
                    {
                        cProyeccionVentas.Text = string.Empty;
                        cProyeccionVentas.Enabled = false;
                    }
                }

                if (cCaracterizacionTecnicaProductoServicio != null)
                {
                    if (cCaracterizacionTecnicaProductoServicio.Text == "True")
                    {
                        cCaracterizacionTecnicaProductoServicio.Text = string.Empty;
                        cCaracterizacionTecnicaProductoServicio.Checked = true;
                        cCaracterizacionTecnicaProductoServicio.Enabled = false;
                    }
                    else
                    {
                        cCaracterizacionTecnicaProductoServicio.Text = string.Empty;
                        cCaracterizacionTecnicaProductoServicio.Enabled = false;
                    }
                }

                if (cDefinicionProcesoProduccionImplementarIndicesTecnicos != null)
                {
                    if (cDefinicionProcesoProduccionImplementarIndicesTecnicos.Text == "True")
                    {
                        cDefinicionProcesoProduccionImplementarIndicesTecnicos.Text = string.Empty;
                        cDefinicionProcesoProduccionImplementarIndicesTecnicos.Checked = true;
                        cDefinicionProcesoProduccionImplementarIndicesTecnicos.Enabled = false;
                    }
                    else
                    {
                        cDefinicionProcesoProduccionImplementarIndicesTecnicos.Text = string.Empty;
                        cDefinicionProcesoProduccionImplementarIndicesTecnicos.Enabled = false;
                    }
                }

                if (cIdentificacionValoracionRequerimientosEquipamiento != null)
                {
                    if (cIdentificacionValoracionRequerimientosEquipamiento.Text == "True")
                    {
                        cIdentificacionValoracionRequerimientosEquipamiento.Text = string.Empty;
                        cIdentificacionValoracionRequerimientosEquipamiento.Checked = true;
                        cIdentificacionValoracionRequerimientosEquipamiento.Enabled = false;
                    }
                    else
                    {
                        cIdentificacionValoracionRequerimientosEquipamiento.Text = string.Empty;
                        cIdentificacionValoracionRequerimientosEquipamiento.Enabled = false;
                    }
                }


                if (cProgramaProduccion != null)
                {
                    if (cProgramaProduccion.Text == "True")
                    {
                        cProgramaProduccion.Enabled = false;
                        cProgramaProduccion.Text = string.Empty;
                        cProgramaProduccion.Checked = true;
                    }
                    else
                    {
                        cProgramaProduccion.Text = string.Empty;
                        cProgramaProduccion.Enabled = false;
                    }
                }


                if (cAnalisisTramitesRequisitosLegales != null)
                {
                    if (cAnalisisTramitesRequisitosLegales.Text == "True")
                    {
                        cAnalisisTramitesRequisitosLegales.Checked = true;
                        cAnalisisTramitesRequisitosLegales.Text = string.Empty;
                        cAnalisisTramitesRequisitosLegales.Enabled = false;
                    }
                    else
                    {
                        cAnalisisTramitesRequisitosLegales.Text = string.Empty;
                        cAnalisisTramitesRequisitosLegales.Enabled = false;
                    }
                }


                if (cCompromisosInstitucionalesPrivadosPublicos != null)
                {
                    if (cCompromisosInstitucionalesPrivadosPublicos.Text == "True")
                    {
                        cCompromisosInstitucionalesPrivadosPublicos.Checked = true;
                        cCompromisosInstitucionalesPrivadosPublicos.Text = string.Empty;
                        cCompromisosInstitucionalesPrivadosPublicos.Enabled = false;
                    }
                    else
                    {
                        cCompromisosInstitucionalesPrivadosPublicos.Text = string.Empty;
                        cCompromisosInstitucionalesPrivadosPublicos.Enabled = false;
                    }
                }

                if (cOrganizacionEmpresarialPropuesta != null)
                {
                    if (cOrganizacionEmpresarialPropuesta.Text == "True")
                    {
                        cOrganizacionEmpresarialPropuesta.Checked = true;
                        cOrganizacionEmpresarialPropuesta.Text = string.Empty;
                        cOrganizacionEmpresarialPropuesta.Enabled = false;
                    }
                    else
                    {
                        cOrganizacionEmpresarialPropuesta.Text = string.Empty;
                        cOrganizacionEmpresarialPropuesta.Enabled = false;
                    }
                }
                if (cCuantificacionInversionRequerida != null)
                {
                    if (cCuantificacionInversionRequerida.Text == "True")
                    {
                        cCuantificacionInversionRequerida.Checked = true;
                        cCuantificacionInversionRequerida.Text = string.Empty;
                        cCuantificacionInversionRequerida.Enabled = false;
                    }
                    else
                    {
                        cCuantificacionInversionRequerida.Text = string.Empty;
                        cCuantificacionInversionRequerida.Enabled = false;
                    }
                }


                if (cPerspectivasRentabilidad != null)
                {
                    if (cPerspectivasRentabilidad.Text == "True")
                    {
                        cPerspectivasRentabilidad.Checked = true;
                        cPerspectivasRentabilidad.Text = string.Empty;
                        cPerspectivasRentabilidad.Enabled = false;
                    }
                    else
                    {
                        cPerspectivasRentabilidad.Text = string.Empty;
                        cPerspectivasRentabilidad.Enabled = false;
                    }
                }
                if (cEstadosFinancieros != null)
                {
                    if (cEstadosFinancieros.Text == "True")
                    {
                        cEstadosFinancieros.Checked = true;
                        cEstadosFinancieros.Text = string.Empty;
                        cEstadosFinancieros.Enabled = false;
                    }
                    else
                    {
                        cEstadosFinancieros.Text = string.Empty;
                        cEstadosFinancieros.Enabled = false;
                    }
                }
                if (cPresupuestosCostosProduccion != null)
                {
                    if (cPresupuestosCostosProduccion.Text == "True")
                    {
                        cPresupuestosCostosProduccion.Checked = true;
                        cPresupuestosCostosProduccion.Text = string.Empty;
                        cPresupuestosCostosProduccion.Enabled = false;
                    }
                    else
                    {
                        cPresupuestosCostosProduccion.Text = string.Empty;
                        cPresupuestosCostosProduccion.Enabled = false;
                    }
                }

                if (cPresupuestoIngresosOperacion != null)
                {
                    if (cPresupuestoIngresosOperacion.Text == "True")
                    {
                        cPresupuestoIngresosOperacion.Checked = true;
                        cPresupuestoIngresosOperacion.Text = string.Empty;
                        cPresupuestoIngresosOperacion.Enabled = false;
                    }
                    else
                    {
                        cPresupuestoIngresosOperacion.Text = string.Empty;
                        cPresupuestoIngresosOperacion.Enabled = false;
                    }
                }
                if (cContemplaManejoAmbiental != null)
                {
                    if(cContemplaManejoAmbiental.Text == "True")
                    {
                        cContemplaManejoAmbiental.Checked = true;
                        cContemplaManejoAmbiental.Text = string.Empty;
                        cContemplaManejoAmbiental.Enabled = false;
                    }
                    else
                    {
                        cContemplaManejoAmbiental.Text = string.Empty;
                        cContemplaManejoAmbiental.Enabled = false;
                    }
                }

                if (cModeloFinanciera != null)
                {
                    if (cModeloFinanciera.Text == "True")
                    {
                        cModeloFinanciera.Checked = true;
                        cModeloFinanciera.Text = string.Empty;
                        cModeloFinanciera.Enabled = false;
                    }
                    else
                    {
                        cModeloFinanciera.Text = string.Empty;
                        cModeloFinanciera.Enabled = false;
                    }
                }


                if (cIndicesRentabilidad != null)
                {
                    if (cIndicesRentabilidad.Text == "True")
                    {
                        cIndicesRentabilidad.Checked = true;
                        cIndicesRentabilidad.Text = string.Empty;
                        cIndicesRentabilidad.Enabled = false;
                    }
                    else
                    {
                        cIndicesRentabilidad.Text = string.Empty;
                        cIndicesRentabilidad.Enabled = false;
                    }
                }



                if (cViabilidad != null)
                {
                    if (cViabilidad.Text == "True")
                    {
                        cViabilidad.Checked = true;
                        cViabilidad.Text = string.Empty;
                        cViabilidad.Enabled = false;
                    }
                    else
                    {
                        cViabilidad.Text = string.Empty;
                        cViabilidad.Enabled = false;
                    }
                }


                if (cIndicadoresGestion != null)
                {
                    if (cIndicadoresGestion.Text == "True")
                    {
                        cIndicadoresGestion.Checked = true;
                        cIndicadoresGestion.Text = string.Empty;
                        cIndicadoresGestion.Enabled = false;
                    }
                    else
                    {
                        cIndicadoresGestion.Text = string.Empty;
                        cIndicadoresGestion.Enabled = false;
                    }
                }


                if (cPlanOperativo != null)
                {
                    if (cPlanOperativo.Text == "True")
                    {
                        cPlanOperativo.Checked = true;
                        cPlanOperativo.Text = string.Empty;
                        cPlanOperativo.Enabled = false;
                    }
                    else
                    {
                        cPlanOperativo.Text = string.Empty;
                        cPlanOperativo.Enabled = false;
                    }
                }


                if (cInformeEvaluacion != null)
                {
                    if (cInformeEvaluacion.Text == "True")
                    {
                        cInformeEvaluacion.Checked = true;
                        cInformeEvaluacion.Text = string.Empty;
                        cInformeEvaluacion.Enabled = false;
                    }
                    else
                    {
                        cInformeEvaluacion.Text = string.Empty;
                        cInformeEvaluacion.Enabled = false;
                    }
                }


               
            }
            /* ************* FIN DEL ITEMTYPE   ***********************************************************************/

            
        }

        protected void DtSeguimiento_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}