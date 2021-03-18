<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoResumenFrame.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.ProyectoResumenFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color:white;background-image:none">
    <form id="form1" runat="server" style="background-color:white;background-image:none">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <ajaxToolkit:TabContainer ID="tbc_resumen" runat="server" ActiveTabIndex="1" Width="100%"
                Height="660px">
                <ajaxToolkit:TabPanel ID="tb_resumenejecutivo" runat="server" HeaderText="Resumen Ejecutivo"
                    Height="100%">
                    <ContentTemplate>
                        <iframe src="PProyectoResumen.aspx" marginwidth="0" marginheight="0" frameborder="0"
                            scrolling="auto" width="100%" height="480px" style="overflow-x: hidden;"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tb_equipotrabajo" OnDemandMode="Once" runat="server" HeaderText="Equipo de Trabajo"
                    Height="100%">
                    <ContentTemplate>
                        <iframe src="PProyectoResumenEquipo.aspx" marginwidth="0" marginheight="0" frameborder="0"
                            scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
