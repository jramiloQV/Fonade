<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionOperativoFrame.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionOperativoFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //Plan operativo
            $('#tc_proyectos_tc_plan_tab').attr("onclick", "CargarPestana('frmEvalPlanOperativo','EvaluacionPlanOperativo.aspx')");
            //Nómina
            $('#tc_proyectos_tc_nomina_tab').attr("onclick","CargarPestana('frmEvalNomina','EvaluacionNomina.aspx')");
            //Producción
            $('#tc_proyectos_tc_produccion_tab').attr("onclick", "CargarPestana('frmproduccion','EvaluacionProduccion.aspx')");
            //Ventas
            $('#tc_proyectos_tc_ventas_tab').attr("onclick", "CargarPestana('frmventas','EvaluacionVentas.aspx')");
        });
    </script>
</head>
<body>
    
    <form id="form1" runat="server">
      <div>
           <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="480px">
            <ajaxToolkit:TabPanel ID="tc_plan" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="planoperativo" class="tab_header">
                        <span>Plan Operativo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmEvalPlanOperativo" src="EvaluacionPlanOperativo.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_nomina" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Nómina</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmEvalNomina" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_produccion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Producción</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmproduccion" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_ventas" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                   <span>Ventas</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmventas" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
          
        </ajaxToolkit:TabContainer>
    </div>
    </form>
    
    <script type="text/javascript">
        $(document).on("ready", cargarTab);

        function cargarTab() {

            $("#planoperativo").click();
        }
    </script>
</body>
</html>
