<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoOperativoInterFrame.aspx.cs" Inherits="Fonade.FONADE.interventoria.ProyectoOperativoInterFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <style>
    #tc_proyectos{
        overflow-x: hidden;
    }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //Plan operativo
            $('#tc_proyectos_tc_observaciones_tab').attr("onclick", "CargarPestana('frmPlanoperativo','FramePlanOperativoInterventoria.aspx?codAspecto=1')");
            //Nomina
            $('#tc_proyectos_tc_generales_tab').attr("onclick", "CargarPestana('frmnomina','FrameNominaInter.aspx?codAspecto=2')");
            //Produccion
            $('#tc_proyectos_tc_frmcomercial_tab').attr("onclick", "CargarPestana('frmproduccion','FrameProduccionInter.aspx?codAspecto=3')");
            //Ventas
            $('#tc_proyectos_tc_tecnico_tab').attr("onclick", "CargarPestana('frmventas','FrameVentasInter.aspx?codAspecto=4')");
        });
    </script>
</head>
<body style="overflow-x: hidden;">
    <form id="form2" runat="server">
        <div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
            <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
                Height="600px">
                <ajaxToolkit:TabPanel ID="tc_observaciones" OnDemandMode="Once" runat="server" Width="100%"
                    Height="100%">
                    <HeaderTemplate>
                        <div class="tab_header" id="dividentificacion">
                            <span>Plan Operativo</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="frmPlanoperativo" src="FramePlanOperativoInterventoria.aspx?codAspecto=1"
                            marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                            height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tc_generales" runat="server" Width="100%" Height="100%">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Nomina</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="frmnomina" src="" marginwidth="0" marginheight="0" frameborder="0"
                            scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                <ajaxToolkit:TabPanel ID="tc_frmcomercial" runat="server" Width="100%" Height="100%">
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
                <ajaxToolkit:TabPanel ID="tc_tecnico" runat="server" Width="100%" Height="100%">
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

            $("#tc_observaciones").load();
        }

    </script>
</body>
</html>
