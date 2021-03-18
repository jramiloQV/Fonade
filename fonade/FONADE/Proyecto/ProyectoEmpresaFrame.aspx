<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoEmpresaFrame.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.ProyectoEmpresaFrame" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="700px">
            <ajaxToolkit:TabPanel ID="tbRegistro" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="divRegistro" class="tab_header" onclick="CargarPestana('frmRegistro','/Fonade/interventoria/ProyectoEmpresa.aspx')">
                        <span>Registro mercantil</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmRegistro" src="/Fonade/interventoria/ProyectoEmpresa.aspx" marginwidth="0"
                        marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%">
                    </iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbAvance" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="divAvance" onclick="CargarPestana('frmAvance','/Fonade/interventoria/ProyectoOperativoInterFrame.aspx')">
                        <span>Avance</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmAvance" src="/Fonade/interventoria/ProyectoOperativoInterFrame.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbIndicadores" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="divIndicadores" onclick="CargarPestana('frmIndicadores','/Fonade/interventoria/InterIndicadoresFrame.aspx')">
                        <span>Indicadores de Gestión</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmIndicadores" src="/Fonade/interventoria/InterIndicadoresFrame.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbInformesInterventoria" runat="server" Width="100%" Height="100%" Visible="false">
                <HeaderTemplate>
                    <div class="divInformesInteventoria" onclick="CargarPestana('frmInformesInterventoria','/Fonade/interventoria/InformesInterventoriaProyectoFrame.aspx')" >
                        <span>Informes de Interventoría</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmInformesInterventoria" src="/Fonade/interventoria/InformesInterventoriaProyectoFrame.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
    </form>
</body>
</html>
