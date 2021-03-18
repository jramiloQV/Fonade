<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoFrameSet.aspx.cs" 
    Inherits="Fonade.FONADE.Proyecto.ProyectoFrameSet" MasterPageFile="~/FONADE/Proyecto/Proyecto.Master" %>

<asp:Content runat="server" ID="ProyectosFrameBodyHolder" ContentPlaceHolderID="bodyHolder">
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">        
        $(document).ready(function () {
            //Mercado
            $('#bodyHolder_tc_proyectos_tc_mercado_tab').attr("onclick", "CargarPestana('frmMercado','ProyectoMercadoFrame.aspx')");
            //Operacion
            $('#bodyHolder_tc_proyectos_tc_operacion_tab').attr("onclick", "CargarPestana('frmProyectoOperacion','ProyectoOperacionFrame.aspx')");
            //Organizacion
            $('#bodyHolder_tc_proyectos_tc_organizacion_tab').attr("onclick", "CargarPestana('frmProyectoOrganizacion','ProyectoOrganizacionFrame.aspx')");
            //Finanzas
            $('#bodyHolder_tc_proyectos_tc_finanzas_tab').attr("onclick", "CargarPestana('frmProyectoFinanzas','ProyectoFinanzasFrame.aspx')");
            //Plan operativo
            $('#bodyHolder_tc_proyectos_tc_planOperativo_tab').attr("onclick", "CargarPestana('frmPlanOperativo','ProyectoOperativoFrame.aspx')");
            //Impacto
            $('#bodyHolder_tc_proyectos_tc_impacto_tab').attr("onclick", "CargarPestana('frmProyectoImpacto','ProyectoImpacto.aspx')");
            //Resumen ejecutivo
            $('#bodyHolder_tc_proyectos_tc_resumen_tab').attr("onclick", "CargarPestana('frmProyectoResumen','ProyectoResumenFrame.aspx')");
            //Anexos
            $('#bodyHolder_tc_proyectos_tc_anexos_tab').attr("onclick", "CargarPestana('frmAnexos','ProyectoAnexos.aspx')");
            //Empresa
            $('#bodyHolder_tc_proyectos_tc_empresa_tab').attr("onclick", "CargarPestana('frmProyectoEmpresa','ProyectoEmpresaFrame.aspx')");
            //Seguimiento presupuestal
            $('#bodyHolder_tc_proyectos_tc_seguimiento_tab').attr("onclick", "CargarPestana('frmContrato','/Fonade/interventoria/SeguimientoPPtalInterTabs.aspx')");
            //Contrato
            $('#bodyHolder_tc_proyectos_tc_contrato_tab').attr("onclick", "CargarPestana('frmContrato','/Fonade/interventoria/InterContratoFrame.aspx')");
        });

        function ActualizaPlanOper() {
            var iframe = document.getElementById('__tab_tc_proyectos_tc_observaciones');
            iframe.reload();
        }

    </script>
    <style type="text/css">
        html,body{
            background-color: #2980b9;
        }
        iframe
        {
            min-height: 700px;
        }
        .ContentInfo
        {
            /*margin-top: 6px !important;*/
        }
        .ajax__tab_xp .ajax__tab_header
        {
            font-size: 9px !important;
        }
        .tab_header .tab_aprobado:before
        {
            font-size: 12px !important;
        }
        .ContentInfo{
            margin-top: -15px !important;
        }
    </style>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:TextBox ID="txtIdGrupoUser" runat="server" Visible="false" ClientIDMode="Static" />
    <div id="padre">
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="100%">
            <ajaxToolkit:TabPanel ID="tc_mercado" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_Mercado, "Mercado") %>"></li>
                        <span>Mercado</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Mercado, "Mercado") %>"></img>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmMercado" src="ProyectoMercadoFrame.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" scrolling="no" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_operacion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header" onclick="">
                        <li class="<%= setTab(Datos.Constantes.CONST_Operacion, "Operacion") %>"></li>
                        <span>Operacion</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Operacion, "Operacion") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmProyectoOperacion" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_organizacion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_Organizacion, "Organizacion") %>"></li>
                        <span>Organización</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Organizacion, "Organizacion") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmProyectoOrganizacion" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_finanzas" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_Finanzas, "Finanzas") %>"></li>
                        <span>Finanzas</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Finanzas, "Finanzas") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmProyectoFinanzas" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_planOperativo" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_PlanOperativo, "Operativo") %>"></li>
                        <span>Plan Operativo</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_PlanOperativo, "Operativo") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <!-- Este contenedor de iframes de plan Operativo -->
                    <iframe id="frmPlanOperativo" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" style="height: 685px !important;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_impacto" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_Impacto, "Impacto") %>"></li>
                        <span>Impacto</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Impacto, "Impacto") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmProyectoImpacto" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_resumen" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_ResumenEjecutivo, "Resumen") %>"></li>
                        <span>Resumen Ejecutivo</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_ResumenEjecutivo, "Resumen") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmProyectoResumen" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_anexos" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_Anexos, "Anexos") %>"></li>
                        <span>Anexos</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Anexos, "Anexos") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmAnexos" src="" marginwidth="0" marginheight="0" frameborder="0" scrolling="no"
                        width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_empresa" OnDemandMode="Once" runat="server" Visible="false"
                Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_Empresa, "Empresa") %>"></li>
                        <span>Empresa</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_Empresa, "Empresa") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmProyectoEmpresa" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_seguimiento" OnDemandMode="Once" runat="server" Visible="false"
                Width="100%" Height="800px">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(10, "Seguimiento") %>">
                        </li>
                        <span>Seguimiento Presupuestal</span>
                        <%--<img alt="img" src="<%= setTab(10, "Seguimiento") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmSeguimiento" src="/Fonade/interventoria/SeguimientoPPtalInterTabs.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%"
                        height="800px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_contrato" OnDemandMode="Once" runat="server" HeaderText=""
                Visible="false" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <li class="<%= setTab(Datos.Constantes.CONST_ContratoInter, "Seguimiento") %>"></li>
                        <span>Contrato</span>
                        <%--<img alt="img" src="<%= setTab(Datos.Constantes.CONST_ContratoInter, "Contrato") %>"></img></li>--%>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmContrato" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="no" width="100%" height="550px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
</asp:Content>
