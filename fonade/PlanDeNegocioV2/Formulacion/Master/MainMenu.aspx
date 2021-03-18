<%@ Page Title="" Language="C#" 
    MasterPageFile="~/PlanDeNegocioV2/Formulacion/Master/Proyecto.Master" 
    AutoEventWireup="true" CodeBehind="MainMenu.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Formulacion.Master.MainMenu" %>

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
            //Protagonista directo en el TabPanel
            $('#bodyHolder_tc_proyectos_tabProtagonista_tab').attr("onclick", "LoadIFrame('holderProtagonista','../Protagonista/Protagonista.aspx')");
            //OportunidadMercado
            $('#bodyHolder_tc_proyectos_tabOportunidad_tab').attr("onclick", "LoadIFrame('holderOportunidad','../OportunidadMercado/OportunidadMercado.aspx')");
            //Solucion
            $('#bodyHolder_tc_proyectos_tabSolucion_tab').attr("onclick", "LoadIFrame('holderSolucion','../Solucion/MenuSolucion.aspx')");
            //DesarrolloSolucion
            $('#bodyHolder_tc_proyectos_tabDesarrolloSolucion_tab').attr("onclick", "LoadIFrame('holderDesarrolloSolucion','../DesarrolloSolucion/MenuDesarrolloSolucion.aspx')");
            //FuturoDelNegocio
            $('#bodyHolder_tc_proyectos_tabFuturoDelNegocio_tab').attr("onclick", "LoadIFrame('holderFuturoDelNegocio','../FuturoDelNegocio/MenuFuturoDelNegocio.aspx')");
            //Riesgos
            $('#bodyHolder_tc_proyectos_tabRiesgos_tab').attr("onclick", "LoadIFrame('holderRiesgos','../Riesgos/Riesgos.aspx')");
            //Finanzas
            $('#bodyHolder_tc_proyectos_tabFinanzas_tab').attr("onclick", "LoadIFrame('holderFinanzas','../Finanzas/MenuFinanzas.aspx')");
            //Resumen ejecutivo
            $('#bodyHolder_tc_proyectos_tabResumenEjecutivo_tab').attr("onclick", "LoadIFrame('holderResumenEjecutivo','../ResumenEjecutivo/ResumenEjecutivo.aspx')");
            //Plan operativo
            $('#bodyHolder_tc_proyectos_tabPlanOperativo_tab').attr("onclick", "LoadIFrame('holderPlanOperativo','../PlanOperativo/MenuPlanOperativo.aspx')");
            //Anexos
            $('#bodyHolder_tc_proyectos_tabAnexos_tab').attr("onclick", "LoadIFrame('holderAnexos','../Anexos/Anexos.aspx')");
            //Empresa
            $('#bodyHolder_tc_proyectos_tabEmpresa_tab').attr("onclick", "LoadIFrame('holderEmpresa','../../../FONADE/Proyecto/ProyectoEmpresaFrame.aspx')");
            //Seguimiento
            $('#bodyHolder_tc_proyectos_tabSeguimiento_tab').attr("onclick", "LoadIFrame('holderSeguimiento','../../../FONADE/interventoria/SeguimientoPPtalInterTabs.aspx')");
            //Contrato
            $('#bodyHolder_tc_proyectos_tabContrato_tab').attr("onclick", "LoadIFrame('holderContrato','../../../FONADE/interventoria/InterContratoFrame.aspx')");
            //Seguimiento Interventoria
            $('#bodyHolder_tc_proyectos_tabSeguimInterventoria_tab').attr("onclick", "LoadIFrame('holderSeguimInterventoria','../SeguimientoInterventoria/SeguimInterventoria.aspx')");

            LoadIFrame('holderProtagonista', '../Protagonista/Protagonista.aspx');
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
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="4" Width="100%" Height="100%">

            <ajaxToolkit:TabPanel ID="tabProtagonista" OnDemandMode="Once" runat="server" Width="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_Protagonista) %>"></li>
                        <span>Protagonista</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderProtagonista" src="../Protagonista/Protagonista.aspx" scrolling="yes" marginwidth="0" marginheight="0"
                        frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabOportunidad" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header" onclick="">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_OportunidadMercado) %>"></li>
                        <span>Oportunidad Mercado</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderOportunidad" marginwidth="0" marginheight="0" frameborder="0"
                        width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabSolucion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_Solucion) %>"></li>
                        <span>Solución</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderSolucion" marginwidth="0" marginheight="0" 
                        frameborder="0" scrolling="yes" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabDesarrolloSolucion" OnDemandMode="Once" runat="server" Width="100%" 
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_DesarolloSolucion) %>"></li>
                        <span>Desarollo Solución</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderDesarrolloSolucion" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabFuturoDelNegocio" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_Futurodelnegocio) %>"></li>
                        <span>Futuro del negocio</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderFuturoDelNegocio" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
           
            <ajaxToolkit:TabPanel ID="tabRiesgos" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_Riesgos) %>"></li>
                        <span>Riesgos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderRiesgos" marginwidth="0" marginheight="0" frameborder="0" scrolling="yes" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabResumenEjecutivo" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_ResumenEjecutivoV2) %>"></li>
                        <span>Resumen Ejecutivo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderResumenEjecutivo" marginwidth="0" marginheight="0" frameborder="0" scrolling="yes" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabFinanzas" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_FinanzasV2) %>"></li>
                        <span>Estructura financiera</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderFinanzas" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabPlanOperativo" OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= GetTabStatus(Datos.Constantes.CONST_PlanOperativoV2) %>"></li>
                        <span>Plan operativo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderPlanOperativo" marginwidth="0" marginheight="0" frameborder="0" scrolling="yes" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabAnexos" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li></li>
                        <span>Anexos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderAnexos" marginwidth="0" marginheight="0" frameborder="0" scrolling="yes" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabEmpresa" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible='<%# ((Boolean)DataBinder.GetPropertyValue(this, "ShowEjecucionTabs")) %>'>
                <HeaderTemplate>
                    <div class="tab_header">
                        <li></li>
                        <span>Empresa</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderEmpresa" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabSeguimiento" OnDemandMode="Once" runat="server" Width="100%" Height="800px" Visible='<%# ((Boolean)DataBinder.GetPropertyValue(this, "ShowEjecucionTabs")) %>'>
                <HeaderTemplate>
                    <div class="tab_header">
                        <li></li>
                        </li>
                        <span>Seguimiento Presupuestal</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderSeguimiento" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabContrato" OnDemandMode="Once" runat="server" Width="100%" Height="800px" Visible='<%# ((Boolean)DataBinder.GetPropertyValue(this, "ShowContratoTabs")) %>'>
                <HeaderTemplate>
                    <div class="tab_header">
                        <li></li>
                        <span>Contrato</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderContrato" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tabSeguimInterventoria" OnDemandMode="Once" runat="server" Width="100%" Height="800px" Visible='<%# ((Boolean)DataBinder.GetPropertyValue(this, "ShowEjecucionTabs")) %>'>
                <HeaderTemplate>
                    <div class="tab_header">
                        <li></li>
                        <span>Seguimiento Interventoria</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderSeguimInterventoria" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

        </ajaxToolkit:TabContainer>

    </div>
</asp:Content>

