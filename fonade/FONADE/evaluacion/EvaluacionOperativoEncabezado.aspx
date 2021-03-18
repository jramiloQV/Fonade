<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionOperativoEncabezado.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionOperativoEncabezado" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
       <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>
</head>
<body>
   
    <form id="form2" runat="server">
      <div>
           <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

          <ajaxToolkit:TabContainer ID="tc_produccion" runat="server" ActiveTabIndex="2" Width="100%" Height="80%">
            <ajaxToolkit:TabPanel ID="tb_planoperativo" OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                <HeaderTemplate><div class="ajax__tab_header"><span>Plan Operativo</span> </div></HeaderTemplate>
                <ContentTemplate></ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tb_nomina" runat="server" Width="100%" Height="100%">
                <HeaderTemplate><div class="tab_header"><span>Nomina</span> </div></HeaderTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tb_produccion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate><div class="tab_header"><span>Producción</span> </div></HeaderTemplate>
                <ContentTemplate><iframe id="frmProduccion" runat="server" src="EvaluacionProduccion.aspx" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" style=" width:100%; height:383px">
                </iframe></ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tb_ventas" runat="server" Width="100%" Height="100%">
                <HeaderTemplate><div class="tab_header"><span>Ventas</span> </div></HeaderTemplate>
                <ContentTemplate><iframe id="frmVentas" runat="server" src="EvaluacionVentas.aspx" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" style=" width:100%; height:383px">
                </iframe></ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
    </form>
</body>
</html>
