<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoMercadoFrame.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.ProyectoMercadoFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Investigacion de mercados
            $('#tbc_mercado_tb_mercadoInvestigacion_tab').attr("onclick", "CargarPestana('frmInvestigacionMercados','PProyectoMercadoInvestigacion.aspx')");
            //Estrategias de mercado
            $('#tbc_mercado_tb_mercadoEstrategia_tab').attr("onclick", "CargarPestana('frmEstrategiaMercado','PProyectoMercadoEstrategia.aspx')");
            //Proyeciones de ventas
            $('#tbc_mercado_tb_mercadoProyecciones_tab').attr("onclick", "CargarPestana('frmProyeccionVentas','PProyectoMercadoProyecciones.aspx')");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <ajaxToolkit:TabContainer ID="tbc_mercado" runat="server" ActiveTabIndex="0" Width="100%"
                Height="661px">
                <ajaxToolkit:TabPanel ID="tb_mercadoInvestigacion" runat="server" Height="100%">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Investigación de Mercados</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="frmInvestigacionMercados"
                            marginwidth="0" src="PProyectoMercadoInvestigacion.aspx" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                            height="550px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_mercadoEstrategia" OnDemandMode="Once" runat="server"
                    Height="100%">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Estrategias de Mercado</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="frmEstrategiaMercado" marginwidth="0" marginheight="0" frameborder="0"
                            scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_mercadoProyecciones" OnDemandMode="Once" runat="server"
                    HeaderText="" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <span>Proyecciones de Ventas</span>
                    </div>
                </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="frmProyeccionVentas" marginwidth="0"
                            marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%">
                        </iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
