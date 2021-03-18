<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/FONADE/evaluacion/Evaluacion.Master" 
AutoEventWireup="true" CodeBehind="SeguimientoFrameset.aspx.cs" Inherits="Fonade.FONADE.interventoria.SeguimientoFrameset" 
    ValidateRequest="false"
    %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodyHolder" runat="server">
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        iframe {
            min-height: 750px;
        }
        .ContentInfo {
            margin-top: 6px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //alert("How we doing?")
        });
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Plan operativo
            $('#bodyHolder_tc_proyectos_tc_tabla_tab').attr("onclick", "CargarPestana('frmtabla','ProyectoOperativoInterFrame.aspx')");
            //Indicadores
            $('#bodyHolder_tc_proyectos_tc_modelofinanciero_tab').attr("onclick", "CargarPestana('frmmodelo','InterIndicadoresFrame.aspx')");
            //Riesgos
            $('#bodyHolder_tc_proyectos_tc_concepto_tab').attr("onclick", "CargarPestana('frmconcepto','riesgos/Riesgos.aspx')");
            //Concepto
            $('#bodyHolder_tc_proyectos_tc_plan_tab').attr("onclick", "CargarPestana('frmPlanOperativo','InterConceptosFrame.aspx')");
            //Contrato
            $('#bodyHolder_tc_proyectos_tc_informe_tab').attr("onclick", "CargarPestana('frminforme','InterContratoFrame.aspx')");
            //Seguimiento
            $('#bodyHolder_tc_proyectos_TabPanel1_tab').attr("onclick", "CargarPestana('frmDesempeno','SeguimientoPPtalInterTabs.aspx')");
            //Informe Evaluacion
            //$('#bodyHolder_tc_proyectos_TabInformeEvaluacion_tab').attr("onclick", "CargarPestana('frmInformeEvaluacion','../../PlanDeNegocioV2/Evaluacion/Informes/EvaluacionReportes.aspx')");
            
        });
    </script>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="100%">
            <ajaxToolkit:TabPanel ID="tc_tabla" OnDemandMode="Once" runat="server" Width="100%" 
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_PlanOperativoInter) %>"></li>
                        <span>Plan Operativo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmtabla" src="ProyectoOperativoInterFrame.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_modelofinanciero" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_IndicadoresGestionInter2) %>"></li>
                        <span>Indicadores De Gestión</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmmodelo"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
                    
            <ajaxToolkit:TabPanel ID="tc_concepto" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_RiesgosInter) %>"></li>
                        <span>Riesgos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmconcepto"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_plan" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_ConceptosInter) %>"></li>
                        <span>Concepto Final y Recomendaciones</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmPlanOperativo"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_informe" runat="server" Width="100%" Height="100%" >
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_ContratoInter) %>"></li>
                        <span>Contrato</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frminforme" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" Width="100%" Height="100%" >
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(6) %>"></li>
                        <span>Seguimiento Presupuestal</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmDesempeno" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="TabInformeEvaluacion" runat="server" Width="100%" Height="100%" >
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.Const_InformesV2) %>"></li>
                        <span>Informe Evaluacion</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmInformeEvaluacion"
                        src="../../PlanDeNegocioV2/Evaluacion/Informes/EvaluacionReportes.aspx"
                        marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>


            </ajaxToolkit:TabContainer>
    </div>
    <script type="text/javascript">
        $(document).on("ready", cargarTab);

        function cargarTab() {

            $("#tc_tabla");
        }
    
          </script>
</asp:Content>

