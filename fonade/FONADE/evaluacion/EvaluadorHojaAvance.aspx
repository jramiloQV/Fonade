<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluadorHojaAvance.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluadorHojaAvance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body {
            background-image: none !important;
        }
    </style>
</head>
<body style="width: 171%; background-color: #fff; background-image: none!important;">
    <form id="form1" runat="server">
        <div style="background-color: #FFFFFF">
            <asp:DataList ID="DtSeguimiento" runat="server" Width="88%"
                Style="font-weight: 700" OnItemDataBound="DtSeguimientoItemDataBound"
                ShowFooter="False"
                OnSelectedIndexChanged="DtSeguimiento_SelectedIndexChanged">

                <HeaderTemplate>
                    <table border="0" width="100%">
                        <tr align="center" bgcolor="#D1D8E2" class="Titulo">
                            <td style="padding: 0 15px;">No.
                            </td>
                            <td style="padding: 0 150px;">&nbsp;&nbsp;&nbsp;Plan de Negocio&nbsp;&nbsp;&nbsp;
                            </td>

                            <td style="padding: 0px 33px;">Evaluador
                            </td>
                            <td>Lectura del Plan de negocio
                            </td>
                            <td>Solicitud de información al emprendedor
                            </td>
                            <td colspan="23">
                                <div align="center">
                                    Tabla de evaluación
                                </div>
                            </td>
                            <td>Modelo financiero
                            </td>
                            <td>Indices de rentabilidad
                            </td>
                            <td colspan="2">Concepto y recomendaciones
                            </td>
                            <td>Plan operativo
                            </td>
                            <td>Informe de evaluacion
                            </td>
                        </tr>
                        <tr align="center" bgcolor="#EDEFF3" class="Titulo">
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td colspan="5">Generales
                            </td>
                            <td colspan="5">Comerciales
                            </td>
                            <td colspan="4">Técnico
                            </td>
                            <td colspan="3">Organizacionales
                            </td>
                            <td colspan="5">Financiero
                            </td>
                            <td>Medio Ambiente
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>Viabilidad
                            </td>
                            <td>Indicadores de gestión
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr align="center" bgcolor="#EDEFF3" class="Titulo">
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>Antecedentes
                            </td>
                            <td>Definición Objetivos
                            </td>
                            <td>Equipo de Trabajo
                            </td>
                            <td>Justificacion de proyecto
                            </td>
                            <td>resumen ejecutivo
                            </td>
                            <td>Caracterización del Producto
                            </td>
                            <td>Estrategias y garantias de comercialización
                            </td>
                            <td>Identificación Mercado Objetivo
                            </td>
                            <td>Identificacion y evaluacion de canales
                            </td>
                            <td>Proyeccion de ventas
                            </td>
                            <td>Caracterizacion Técnica del Producto o Servicio
                            </td>
                            <td>Definición del proceso de producción a implementar e indices técnicos
                            </td>
                            <td>Identificación y valoración de los requerimientos en equipamiento y materiales y
                            suministros
                            </td>
                            <td>Programa de producción
                            </td>
                            <td>Analisis en los tramites y requisitos legales para la puesta en marcha de la empresa
                            </td>
                            <td>Compromisos institucionales privados o publicos
                            </td>
                            <td>Organización empresarial propuesta
                            </td>
                            <td>Cunatificacion de la inversion requerida
                            </td>
                            <td>Perspectivas de rentabilidad
                            </td>
                            <td>Estados financieros
                            </td>
                            <td>Presupuestos de costos de produccion
                            </td>
                            <td>Presupuesto de ingresos de operación
                            </td>
                            <td>contempla o no el manejo ambiental
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td colspan="2">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table class="tablaavance" style="background-color: #fff; width: 100%; font-size: 10px!important; border: 1px solid #ccc" cellpadding="0"
                        cellspacing="0">



                        <!--coso-->
                        <tr>
                            <td style="width: 55px;">
                                <asp:Label ID="lproyecto" runat="server" Text='<%# Eval("CodProyecto") %>' />
                                <asp:Label ID="lcontacto" Visible="False" runat="server" Text='<%# Eval("CodContacto") %>' />

                                <td style="width: 397px;">
                                    <asp:Label ID="lplannegocio" runat="server" Text='<%# Eval("NomProyecto") %>' />
                                </td>
                                <td style="width: 140px;">
                                    <asp:Label ID="levaluador" runat="server" />
                                </td>
                                <td>&nbsp;
                                </td>
                                <td style="width: 65px;">
                                    <asp:CheckBox ID="cLecturaPlanNegocio" runat="server" Text='<%# Eval("LecturaPlanNegocio") %>' />
                                </td>
                                <td style="width: 98px;">
                                    <asp:CheckBox ID="cSolicitudInformacionEmprendedor" runat="server" Text='<%# Eval("SolicitudInformacionEmprendedor") %>' />
                                </td>
                                <td style="width: 69px;">
                                    <asp:CheckBox ID="cAntecedentes" runat="server" Text='<%# Eval("Antecedentes") %>' />
                                </td>
                                <td style="width: 63px;">
                                    <asp:CheckBox ID="cDefinicionObjetivos" runat="server" Text='<%# Eval("DefinicionObjetivos") %>' />
                                </td>
                                <td style="width: 59px;">
                                    <asp:CheckBox ID="cEquipoTrabajo" runat="server" Text='<%# Eval("EquipoTrabajo") %>' />
                                </td>
                                <td style="width: 79px;">
                                    <asp:CheckBox ID="cJustificacionProyecto" runat="server" Text='<%# Eval("JustificacionProyecto") %>' />
                                </td>
                                <td style="width: 82px;">
                                    <asp:CheckBox ID="cResumenEjecutivo" runat="server" Text='<%# Eval("ResumenEjecutivo") %>' />
                                </td>
                                <td style="width: 103px;">
                                    <asp:CheckBox ID="cCaracterizacionProducto" runat="server" Text='<%# Eval("CaracterizacionProducto") %>' />
                                </td>
                                <td style="width: 100px;">
                                    <asp:CheckBox ID="cEstrategiasGarantiasComercializacion" runat="server" Text='<%# Eval("EstrategiasGarantiasComercializacion") %>' />
                                </td>
                                <td style="width: 78px;">
                                    <asp:CheckBox ID="cIdentificacionMercadoObjetivo" runat="server" Text='<%# Eval("IdentificacionMercadoObjetivo") %>' />
                                </td>
                                <td style="width: 88px;">
                                    <asp:CheckBox ID="cIdentificacionEvaluacionCanales" runat="server" Text='<%# Eval("IdentificacionEvaluacionCanales") %>' />
                                </td>
                                <td style="width: 90px;">
                                    <asp:CheckBox ID="cProyeccionVentas" runat="server" Text='<%# Eval("ProyeccionVentas") %>' />
                                </td>
                                <td style="width: 84px;">
                                    <asp:CheckBox ID="cCaracterizacionTecnicaProductoServicio" runat="server" Text='<%# Eval("CaracterizacionTecnicaProductoServicio") %>' />
                                </td>
                                <td style="width: 90px;">
                                    <asp:CheckBox ID="cDefinicionProcesoProduccionImplementarIndicesTecnicos" runat="server"
                                        Text='<%# Eval("DefinicionProcesoProduccionImplementarIndicesTecnicos") %>' />
                                </td>
                                <td style="width: 85px;">
                                    <asp:CheckBox ID="cIdentificacionValoracionRequerimientosEquipamiento" runat="server"
                                        Text='<%# Eval("IdentificacionValoracionRequerimientosEquipamiento") %>' />
                                </td>
                                <td style="width: 72px;">
                                    <asp:CheckBox ID="cProgramaProduccion" runat="server" Text='<%# Eval("ProgramaProduccion") %>' />
                                </td>
                                <td style="width: 76px;">
                                    <asp:CheckBox ID="cAnalisisTramitesRequisitosLegales" runat="server" Text='<%# Eval("AnalisisTramitesRequisitosLegales") %>' />
                                </td>
                                <td style="width: 87px;">
                                    <asp:CheckBox ID="cCompromisosInstitucionalesPrivadosPublicos" runat="server" Text='<%# Eval("CompromisosInstitucionalesPrivadosPublicos") %>' />
                                </td>
                                <td style="width: 88px;">
                                    <asp:CheckBox ID="cOrganizacionEmpresarialPropuesta" runat="server" Text='<%# Eval("OrganizacionEmpresarialPropuesta") %>' />
                                </td>
                                <td style="width: 87px;">
                                    <asp:CheckBox ID="cCuantificacionInversionRequerida" runat="server" Text='<%# Eval("CuantificacionInversionRequerida") %>' />
                                </td>
                                <td style="width: 76px;">
                                    <asp:CheckBox ID="cPerspectivasRentabilidad" runat="server" Text='<%# Eval("PerspectivasRentabilidad") %>' />
                                </td>
                                <td style="width: 80px;">
                                    <asp:CheckBox ID="cEstadosFinancieros" runat="server" Text='<%# Eval("EstadosFinancieros") %>' />
                                </td>
                                <td style="width: 85px;">
                                    <asp:CheckBox ID="cPresupuestosCostosProduccion" runat="server" Text='<%# Eval("PresupuestosCostosProduccion") %>' />
                                </td>
                                <td style="width: 66px;">
                                    <asp:CheckBox ID="cPresupuestoIngresosOperacion" runat="server" Text='<%# Eval("PresupuestoIngresosOperacion") %>' />
                                </td>
                                <td style="width: 73px;">
                                    <asp:CheckBox ID="cContemplaManejoAmbiental" runat="server" Text='<%# Eval("ContemplaManejoAmbiental") %>' />
                                </td>
                                <td style="width: 71px;">
                                    <asp:CheckBox ID="cModeloFinanciera" runat="server" Text='<%# Eval("ModeloFinanciera") %>' />
                                </td>
                                <td style="width: 75px">
                                    <asp:CheckBox ID="cIndicesRentabilidad" runat="server" Text='<%# Eval("IndicesRentabilidad") %>' />
                                </td>
                                <td style="width: 64px;">
                                    <asp:CheckBox ID="cViabilidad" runat="server" Text='<%# Eval("Viabilidad") %>' />
                                </td>
                                <td style="width: 69px;">
                                    <asp:CheckBox ID="cIndicadoresGestion" runat="server" Text='<%# Eval("IndicadoresGestion") %>' />
                                </td>
                                <td style="width: 70px;">
                                    <asp:CheckBox ID="cPlanOperativo" runat="server" Text='<%# Eval("PlanOperativo") %>' />
                                </td>
                                <td>
                                    <asp:CheckBox ID="cInformeEvaluacion" runat="server" Text='<%# Eval("InformeEvaluacion") %>' />
                                </td>
                            </td>
                        </tr>

                    </table>
                </ItemTemplate>
            </asp:DataList>
            
                <div style="float: left; width: 65%; text-align: center;">
                <asp:Label ID="lblmensaje" runat="server" Font-Size="XX-Large" Visible="False"
                    Text="No se encontraron  Datos" />
                </div>
            
        </div>
    </form>
</body>
</html>
