<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformesInterventoriaProyectoFrame.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.InformesInterventoriaProyectoFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FFONDO EMPRENDER - </title>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="tlk_1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_informes" runat="server" ActiveTabIndex="0" Width="100%"
            Height="80%">
            <ajaxToolkit:TabPanel ID="tbVisita" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="visitadiv" class="tab_header" onclick="CargarPestana('frmInformesVisita','InformeVisitaProyecto.aspx')">
                        <span>Visita de Interventoría</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmInformesVisita" src="InformeVisitaProyecto.aspx" marginwidth="0" marginheight="0"
                        frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbAvanceBimensual" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header" onclick="CargarPestana('frmAvanceBimensual','InformeBimensualProyecto.aspx')">
                        <span>Avance Bimensual</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmAvanceBimensual" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbEjecucionPresupuestal" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header" onclick="CargarPestana('frmEjecucionPresupuestal','InformeEjecucionProyecto.aspx')">
                        <span>Ejecución Presupuestal</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmEjecucionPresupuestal" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbConsolidadoInterventoria" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header" onclick="CargarPestana('frmConsolidadoInterventoria','InformeConsolidadoProyecto.aspx')">
                        <span>Consolidado Interventoría</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmConsolidadoInterventoria" src="" marginwidth="0" marginheight="0"
                        frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
    </form>
</body>
</html>
