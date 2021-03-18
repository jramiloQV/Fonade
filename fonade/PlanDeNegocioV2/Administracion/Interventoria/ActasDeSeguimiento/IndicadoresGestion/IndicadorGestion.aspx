<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="IndicadorGestion.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.IndicadorGestion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;overflow: hidden;">
<head runat="server">
    <title>Indicadores Gestión</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
       <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="3. CUMPLIMIENTO DE INDICADORES DE GESTIÓN"></asp:Label>
            </h1>
        </div>
         <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>

        <ajaxToolkit:TabContainer ID="tc_ActasSeg" runat="server" ActiveTabIndex="0" Width="100%" Height="100%">
            <!--Generación Empleo-->
            <ajaxToolkit:TabPanel ID="tabGeneracionEmpleo" OnDemandMode="Once" 
                runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Generación Empleo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderGeneracionEmpleo" src="GestionEmpleo.aspx"
                        style="height: 560px;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Ejecución Presupuestal-->
            <ajaxToolkit:TabPanel ID="tabEjecucionPresupuestal" 
                OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Ejecución Presupuestal</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderEjecucionPresupuestal" src="EjecucionPresupuestal.aspx" style="height: 560px;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Gestión Mercadeo-->
            <ajaxToolkit:TabPanel ID="tabGestionMercadeo" 
                OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Gestión Mercadeo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderGestionMercadeo" src="GestionMercadeo.aspx" style="height: 560px;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Contrapartidas-->
            <ajaxToolkit:TabPanel ID="tabContrapartidas" 
                OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Contrapartidas</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderContrapartidas" src="Contrapartidas.aspx" style="height: 560px;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

             <!--Gestión Producción-->
            <ajaxToolkit:TabPanel ID="tabGestionProduccion" 
                OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Gestión Producción</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderGestionProduccion" src="GestionProduccionTotal.aspx" style="height: 560px;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <!--Gestión Ventas-->
            <ajaxToolkit:TabPanel ID="tabGestionVentas" 
                OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">                        
                        <span>Gestión Ventas</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderGestionVentas" src="GestionVentas.aspx" style="height: 560px;"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
           
        </ajaxToolkit:TabContainer>
    </form>
</body>
</html>
