<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/Master/MasterActas.Master" AutoEventWireup="true" CodeBehind="MainMenuActasSeg.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Master.MainMenuActasSeg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">   
    
     <script type="text/javascript">
        $(document).ready(function () {
            //Protagonista directo en el TabPanel
            //$('#bodyHolder_tc_proyectos_tabProtagonista_tab').attr("onclick", "LoadIFrame('holderProtagonista','../Protagonista/Protagonista.aspx')");
            ////OportunidadMercado
            //$('#bodyHolder_tc_proyectos_tabOportunidad_tab').attr("onclick", "LoadIFrame('holderOportunidad','../OportunidadMercado/OportunidadMercado.aspx')");
            ////Solucion
            //$('#bodyHolder_tc_proyectos_tabSolucion_tab').attr("onclick", "LoadIFrame('holderSolucion','../Solucion/MenuSolucion.aspx')");
            ////DesarrolloSolucion
            //$('#bodyHolder_tc_proyectos_tabDesarrolloSolucion_tab').attr("onclick", "LoadIFrame('holderDesarrolloSolucion','../DesarrolloSolucion/MenuDesarrolloSolucion.aspx')");
            ////FuturoDelNegocio
            //$('#bodyHolder_tc_proyectos_tabFuturoDelNegocio_tab').attr("onclick", "LoadIFrame('holderFuturoDelNegocio','../FuturoDelNegocio/MenuFuturoDelNegocio.aspx')");
            ////Riesgos
            //$('#bodyHolder_tc_proyectos_tabRiesgos_tab').attr("onclick", "LoadIFrame('holderRiesgos','../Riesgos/Riesgos.aspx')");
            ////Finanzas
            //$('#bodyHolder_tc_proyectos_tabFinanzas_tab').attr("onclick", "LoadIFrame('holderFinanzas','../Finanzas/MenuFinanzas.aspx')");
            ////Resumen ejecutivo
            //$('#bodyHolder_tc_proyectos_tabResumenEjecutivo_tab').attr("onclick", "LoadIFrame('holderResumenEjecutivo','../ResumenEjecutivo/ResumenEjecutivo.aspx')");
            ////Plan operativo
            //$('#bodyHolder_tc_proyectos_tabPlanOperativo_tab').attr("onclick", "LoadIFrame('holderPlanOperativo','../PlanOperativo/MenuPlanOperativo.aspx')");
            ////Anexos
            //$('#bodyHolder_tc_proyectos_tabAnexos_tab').attr("onclick", "LoadIFrame('holderAnexos','../Anexos/Anexos.aspx')");
            ////Empresa
            //$('#bodyHolder_tc_proyectos_tabEmpresa_tab').attr("onclick", "LoadIFrame('holderEmpresa','../../../FONADE/Proyecto/ProyectoEmpresaFrame.aspx')");
            ////Seguimiento
            //$('#bodyHolder_tc_proyectos_tabSeguimiento_tab').attr("onclick", "LoadIFrame('holderSeguimiento','../../../FONADE/interventoria/SeguimientoPPtalInterTabs.aspx')");
            ////Contrato
            //$('#bodyHolder_tc_proyectos_tabContrato_tab').attr("onclick", "LoadIFrame('holderContrato','../../../FONADE/interventoria/InterContratoFrame.aspx')");

            //LoadIFrame('holderGeneralRiesgos', '../Riesgos/GeneralRiesgos.aspx');
        });
        //function SetScroll(h, v) {
        //    window.scrollTo(h, v);
        //}
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
 
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
    
    <div id="padre">
        <ajaxToolkit:TabContainer ID="tc_ActasSeg" runat="server" ActiveTabIndex="0" Width="100%" Height="100%">
            <!--General/Riesgos-->
            <ajaxToolkit:TabPanel ID="tabGeneralRiesgos" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>General/Riesgos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderGeneralRiesgos" src="../Riesgos/GeneralRiesgos.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Indicadores Gestion-->
            <ajaxToolkit:TabPanel ID="tabIndicadorGestion" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Indicadores Gestión</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderIndicadorGestion" src="../IndicadoresGestion/IndicadorGestion.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

             <!--Obligaciones Tipicas-->
            <ajaxToolkit:TabPanel ID="tabObligacionesTipicas" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Obligaciones Tipicas</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderObligTipicas" src="../ObligTipicas/ObligacionesTipicas.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Otros Aspectos-->
            <ajaxToolkit:TabPanel ID="tabOtrosAspectos" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Otros Aspectos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderOtrosAspectos" src="../OtrosAspectos/OtrosAspectos.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

             <!--Otras Obligaciones-->
            <ajaxToolkit:TabPanel ID="tabOtrasObligaciones" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Otras Obligaciones</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderOtrasObligaciones" src="../OtrasObligaciones/OtrasObligaciones.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Estado Empresa-->
            <ajaxToolkit:TabPanel ID="tabEstadoEmpresa" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Estado Empresa</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderEstadoEmpresa" src="../EstadoEmpresa/EstadoEmpresa.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Compromisos-->
            <ajaxToolkit:TabPanel ID="tabCompromisos" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Compromisos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderCompromisos" src="../Compromisos/Compromisos.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

             <!--Notas-->
            <ajaxToolkit:TabPanel ID="TabNotas" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Notas</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderNotas" src="../Notas/Notas.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
           
        </ajaxToolkit:TabContainer>
    </div>
</asp:Content>
