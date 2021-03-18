<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/Evaluacion.Master" AutoEventWireup="true" CodeBehind="MainMenu.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.Master.MainMenu" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
        (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
        m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
        })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

        ga('create', 'UA-85402368-1', 'auto');
        ga('send', 'pageview');
     </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            //TablaDeEvaluacion
            $('#bodyHolder_tc_proyectos_tabTablaEvaluacion_tab').attr("onclick", "LoadIFrame('holderTablaEvaluacion','../TablaDeEvaluacion/MenuTablaDeEvaluacion.aspx')");
            //Financiera
            $('#bodyHolder_tc_proyectos_tc_modelofinanciero_tab').attr("onclick", "LoadIFrame('frmmodelo','../EvaluacionFinanciera/MenuEvaluacionFinanciera.aspx')");
            //Recomendaciones
            $('#bodyHolder_tc_proyectos_tc_concepto_tab').attr("onclick", "LoadIFrame('frmconcepto','../ConceptoFinal/MenuConceptoFinal.aspx')");
            //Plan operativo
            $('#bodyHolder_tc_proyectos_tc_plan_tab').attr("onclick", "LoadIFrame('frmPlanOperativo','../PlanOperativo/MenuPlanOperativo.aspx')");
            //Indicadores de gestión
            $('#bodyHolder_tc_proyectos_tc_indicador_tab').attr("onclick", "LoadIFrame('frmIndicadorDeGestion','../IndicadoresDeGestion/IndicadorGestion.aspx')");
            //Informes
            $('#bodyHolder_tc_proyectos_tc_informe_tab').attr("onclick", "LoadIFrame('frminforme','../Informes/EvaluacionReportes.aspx')");
            //Desempeno
            $('#bodyHolder_tc_proyectos_tc_DesempenoEvaluador_tab').attr("onclick", "LoadIFrame('frmDesempenoEvaluador','../DesempenoEvaluador/MenuDesempenoEvaluador.aspx')");
            //Hoja avance
            $('#bodyHolder_tc_proyectos_tc_hoja_tab').attr("onclick", "LoadIFrame('frmhoja','../TablaDeEvaluacion/EvaluacionHojaAvance.aspx')");
         
            LoadIFrame('holderTablaEvaluacion', '../TablaDeEvaluacion/MenuTablaDeEvaluacion.aspx');
        });
        function SetScroll(h, v) {
            window.scrollTo(h, v);
        }
    </script>
    <style type="text/css">
        html, body {
            background-color: #2980b9;
        }

        iframe {
            min-height: 650px;
        }

        .ContentInfo {
            /*margin-top: 6px !important;*/
        }

        .ajax__tab_xp .ajax__tab_header {
            font-size: 9px !important;
        }

        .tab_header .tab_aprobado:before {
            font-size: 12px !important;
        }

        .ContentInfo {
            margin-top: -15px !important;
        }
    </style>
</asp:Content>

<asp:Content runat="server" ID="ProyectosFrameBodyHolder" ContentPlaceHolderID="bodyHolder">
    <% Page.DataBind(); %>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    <div id="padre">
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="100%">
            <ajaxToolkit:TabPanel ID="tabTablaEvaluacion" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_TablaDeEvaluacionV2) %>"></li>
                        <span>Tabla de evaluación</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderTablaEvaluacion" src="../TablaDeEvaluacion/MenuTablaDeEvaluacion.aspx" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_modelofinanciero" OnDemandMode="None" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_EvaluacionFinancieraV2) %>"></li>
                        <span>Evaluación Financiera</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmmodelo" 
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
                    
            <ajaxToolkit:TabPanel ID="tc_concepto" OnDemandMode="None" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_ConceptoFinalYRecomendacionesV2) %>"></li>
                        <span>Concepto Final y Recomendaciones</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmconcepto" 
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_plan" OnDemandMode="None" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_PlanOperativoEvaluacionV2) %>"></li>
                        <span>Plan Operativo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmPlanOperativo"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_indicador" OnDemandMode="None" runat="server" Width="100%" scrolling="auto" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_IndicadoresDeGestionV2) %>"></li>
                        <span>Indicadores de gestión</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmIndicadorDeGestion"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="yes" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_informe" runat="server" Width="100%" Height="100%" >
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_InformesV2) %>"></li>
                        <span>Informes</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frminforme"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_DesempenoEvaluador" runat="server" Width="100%" Height="100%" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowAvanceEvaluacion")) %>' >
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.Const_DesempenoEvaluadorV2) %>"></li>
                        <span>Desempeño Evaluador</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmDesempenoEvaluador" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_hoja" runat="server" Width="100%" Height="100%" >
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Hoja De Avance</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmhoja" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>

    </div>
</asp:Content>

