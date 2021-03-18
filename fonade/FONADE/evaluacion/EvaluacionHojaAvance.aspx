<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionHojaAvance.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionHojaAvance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/ScriptsGenerales.js"></script>
    <style type="text/css">
        htnl, body {
            background-color: white !important;
            background-image:none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div style="width: 100%; height: auto; overflow-x: scroll;">
                <table class="Grilla" style="width: 100%; text-align: center;">
                    <thead>
                        <tr>
                            <th>Lectura del Plan de negocio</th>
                            <th>Solicitud de información al emprendedor</th>
                            <th colspan="23">Tabla de evaluación</th>
                            <th>Modelo financiero</th>
                            <th>Indices de rentabilidad </th>
                            <th colspan="2">Concepto y recomendaciones</th>
                            <th>Plan operativo</th>
                            <th>Informe de evaluacion</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td></td>
                            <td></td>
                            <td colspan="5">Generales</td>
                            <td colspan="5">Comerciales</td>
                            <td colspan="4">Técnico</td>
                            <td colspan="3">Organizacionales</td>
                            <td colspan="5">Financiero</td>
                            <td>Medio Ambiente</td>
                            <td></td>
                            <td></td>
                            <td>Viabilidad</td>
                            <td>Indicadores de gestión</td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td>Antecedentes</td>
                            <td>Definición Objetivos</td>
                            <td>Equipo de Trabajo</td>
                            <td>Justificacion de proyecto</td>
                            <td>resumen ejecutivo</td>
                            <td>Caracterización del Producto</td>
                            <td>Estrategias y garantias de comercialización</td>
                            <td>Identificación Mercado Objetivo</td>
                            <td>Identificacion y evaluacion de canales</td>
                            <td>Proyeccion de ventas</td>
                            <td>Caracterizacion Técnica del Producto o Servicio </td>
                            <td>Definición del proceso de producción a implementar e indices técnicos</td>
                            <td>Identificación y valoración de los requerimientos en equipamiento y materiales y suministros</td>
                            <td>Programa de producción </td>
                            <td>Analisis en los tramites y requisitos legales para la puesta en marcha  de la empresa</td>
                            <td>Compromisos institucionales privados o publicos</td>
                            <td>Organización empresarial propuesta</td>
                            <td>Cunatificacion de la inversion requerida </td>
                            <td>Perspectivas de rentabilidad</td>
                            <td>Estados financieros</td>
                            <td>Presupuestos de costos de produccion </td>
                            <td>Presupuesto de ingresos de operación </td>
                            <td>contempla o no el manejo ambiental</td>
                            <td></td>
                            <td></td>
                            <td colspan="2"></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbx_LecturaPlanNegocio" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_SolicitudInformacionEmprendedor" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_Antecedentes" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_DefinicionObjetivos" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_EquipoTrabajo" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_JustificacionProyecto" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_ResumenEjecutivo" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_CaracterizacionProducto" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_EstrategiasGarantiasComercializacion" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_IdentificacionMercadoObjetivo" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_IdentificacionEvaluacionCanales" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_ProyeccionVentas" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_CaracterizacionTecnicaProductoServicio" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_DefinicionProcesoProduccionImplementarIndicesTecnicos" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_IdentificacionValoracionRequerimientosEquipamiento" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_ProgramaProduccion" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_AnalisisTramitesRequisitosLegales" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_CompromisosInstitucionalesPrivadosPublicos" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_OrganizacionEmpresarialPropuesta" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_CuantificacionInversionRequerida" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_PerspectivasRentabilidad" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_EstadosFinancieros" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_PresupuestosCostosProduccion" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_PresupuestoIngresosOperacion" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_ContemplaManejoAmbiental" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_ModeloFinanciera" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_IndicesRentabilidad" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_Viabilidad" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_IndicadoresGestion" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_PlanOperativo" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="cbx_InformeEvaluacion" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <br />
            <br />
            <div id="cajabotones">
                <table>
                    <tr>
                        <td style="padding: 25px;">
                            <asp:Button ID="btn_Enviar" runat="server" Text="Enviar" OnClick="btn_Enviar_Click" />
                            <asp:Button ID="btn_Limpiar" runat="server" Text="Limpiar" OnClick="btn_Limpiar_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
