<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="EvaluadorHojaAvanceGerenteDetalle.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluadorHojaAvanceGerenteDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:LinkButton ID="LB_Volver" runat="server" Text="Regresar a la Lista de Coordinadores" PostBackUrl="~/FONADE/evaluacion/EvaluadorHojaAvanceGerente.aspx"></asp:LinkButton>
    <br />
    <div style="overflow: scroll; padding-left: 20px; padding-right: 20px; padding-bottom: 20px;
        padding-top: 20px; margin: 20px; width:650px;">
        <asp:DataList ID="DtSeguimiento" runat="server" onitemdatabound="DtSeguimientoItemDataBound" ShowFooter="False" Style="font-weight: 700;text-align:center;" Width="88%" border=1 cellspacing="0" cellpadding="2" bordercolor="666633">
            <HeaderTemplate>
                    <thead  align="center" bgcolor="#D1D8E2">
                        <tr>
                            <td>&nbsp;</td>
                            <td>No.</td>
                            <td>Plan de Negocio</td>
                            <td>Evaluador </td>
                            <td>Lectura del Plan de negocio </td>
                            <td>Solicitud de información al emprendedor </td>
                            <td colspan="24">
                                <div align="center">
                                    Tabla de evaluación</div>
                            </td>
                            <td>Modelo financiero </td>
                            <td>Indices de rentabilidad </td>
                            <td colspan="2">Concepto y recomendaciones </td>
                            <td>Plan operativo </td>
                            <td>Informe de evaluacion </td>
                        </tr>
                        <tr bgcolor="#EDEFF3">
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td colspan="5">Generales </td>
                            <td colspan="5">Comerciales </td>
                            <td colspan="4">Técnico </td>
                            <td colspan="3">Organizacionales </td>
                            <td colspan="5">Financiero </td>
                            <td colspan="2">Medio Ambiente </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>Viabilidad </td>
                            <td>Indicadores de gestión </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                        </tr>
                        <tr>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>Antecedentes </td>
                            <td>Definición Objetivos </td>
                            <td>Equipo de Trabajo </td>
                            <td>Justificacion de proyecto </td>
                            <td>resumen ejecutivo </td>
                            <td>Caracterización del Producto </td>
                            <td>Estrategias y garantias de comercialización </td>
                            <td>Identificación Mercado Objetivo </td>
                            <td>Identificacion y evaluacion de canales </td>
                            <td>Proyeccion de ventas </td>
                            <td>Caracterizacion Técnica del Producto o Servicio </td>
                            <td>Definición del proceso de producción a implementar e indices técnicos </td>
                            <td>Identificación y valoración de los requerimientos en equipamiento y materiales y suministros </td>
                            <td>Programa de producción </td>
                            <td>Analisis en los tramites y requisitos legales para la puesta en marcha de la empresa </td>
                            <td>Compromisos institucionales privados o publicos </td>
                            <td>Organización empresarial propuesta </td>
                            <td>Cunatificacion de la inversion requerida </td>
                            <td>Perspectivas de rentabilidad </td>
                            <td>Estados financieros </td>
                            <td>Presupuestos de costos de produccion </td>
                            <td>Presupuesto de ingresos de operación </td>
                            <td colspan="2">contempla o no el manejo ambiental </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                            <td>&nbsp; </td>
                        </tr>
                    </thead>
                    </HeaderTemplate>
                    <ItemTemplate>

                                <td>
                                    <asp:Label ID="lproyecto" runat="server" Text='<%# Eval("CodProyecto") %>' />
                                    <asp:Label ID="lcontacto" runat="server" Text='<%# Eval("CodContacto") %>' Visible="False" />
                                </td>  
                                <td>
                                    <asp:Label ID="lplannegocio" runat="server" Text='<%# Eval("NomProyecto") %>' />
                                </td>
                                <td>
                                    <asp:Label ID="levaluador" runat="server" />
                                </td>
                                <!--<td>&nbsp; </td>-->
                                <td>
                                    <asp:CheckBox ID="cLecturaPlanNegocio" runat="server" Text='<%# Eval("LecturaPlanNegocio") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cSolicitudInformacionEmprendedor" runat="server" Text='<%# Eval("SolicitudInformacionEmprendedor") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cAntecedentes" runat="server" Text='<%# Eval("Antecedentes") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cDefinicionObjetivos" runat="server" Text='<%# Eval("DefinicionObjetivos") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cEquipoTrabajo" runat="server" Text='<%# Eval("EquipoTrabajo") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cJustificacionProyecto" runat="server" Text='<%# Eval("JustificacionProyecto") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cResumenEjecutivo" runat="server" Text='<%# Eval("ResumenEjecutivo") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cCaracterizacionProducto" runat="server" Text='<%# Eval("CaracterizacionProducto") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cEstrategiasGarantiasComercializacion" runat="server" Text='<%# Eval("EstrategiasGarantiasComercializacion") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cIdentificacionMercadoObjetivo" runat="server" Text='<%# Eval("IdentificacionMercadoObjetivo") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cIdentificacionEvaluacionCanales" runat="server" Text='<%# Eval("IdentificacionEvaluacionCanales") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cProyeccionVentas" runat="server" Text='<%# Eval("ProyeccionVentas") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cCaracterizacionTecnicaProductoServicio" runat="server" Text='<%# Eval("CaracterizacionTecnicaProductoServicio") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cDefinicionProcesoProduccionImplementarIndicesTecnicos" runat="server" Text='<%# Eval("DefinicionProcesoProduccionImplementarIndicesTecnicos") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cIdentificacionValoracionRequerimientosEquipamiento" runat="server" Text='<%# Eval("IdentificacionValoracionRequerimientosEquipamiento") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cProgramaProduccion" runat="server" Text='<%# Eval("ProgramaProduccion") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cAnalisisTramitesRequisitosLegales" runat="server" Text='<%# Eval("AnalisisTramitesRequisitosLegales") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cCompromisosInstitucionalesPrivadosPublicos" runat="server" Text='<%# Eval("CompromisosInstitucionalesPrivadosPublicos") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cOrganizacionEmpresarialPropuesta" runat="server" Text='<%# Eval("OrganizacionEmpresarialPropuesta") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cCuantificacionInversionRequerida" runat="server" Text='<%# Eval("CuantificacionInversionRequerida") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cPerspectivasRentabilidad" runat="server" Text='<%# Eval("PerspectivasRentabilidad") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cEstadosFinancieros" runat="server" Text='<%# Eval("EstadosFinancieros") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cPresupuestosCostosProduccion" runat="server" Text='<%# Eval("PresupuestosCostosProduccion") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cPresupuestoIngresosOperacion" runat="server" Text='<%# Eval("PresupuestoIngresosOperacion") %>' />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="cContemplaManejoAmbiental" runat="server" Text='<%# Eval("ContemplaManejoAmbiental") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cModeloFinanciera" runat="server" Text='<%# Eval("ModeloFinanciera") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cIndicesRentabilidad" runat="server" Text='<%# Eval("IndicesRentabilidad") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cViabilidad" runat="server" Text='<%# Eval("Viabilidad") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cIndicadoresGestion" runat="server" Text='<%# Eval("IndicadoresGestion") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cPlanOperativo" runat="server" Text='<%# Eval("PlanOperativo") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cInformeEvaluacion" runat="server" Text='<%# Eval("InformeEvaluacion") %>' />
                                </td>
                    </ItemTemplate>
                
         </asp:DataList>
       
        <p align="center">
            <asp:Label ID="lblmensaje" runat="server" Font-Size="XX-Large" Text="No se encontraron  Datos" Visible="False" />
        </p>
    </div>
    
</asp:Content>