<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuFinanzas.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.MenuFinanzas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>    
    <script src="../../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Plan de compras
            $('#tc_proyectos_tabPlanDeCompras_tab').attr("onclick", "LoadIFrame('holderPlanDeCompras','../Finanzas/PlanDeCompras.aspx')");
            //Costos de producción
            $('#tc_proyectos_tabCostosDeProduccion_tab').attr("onclick", "LoadIFrame('holderCostosDeProduccion','../Finanzas/CostosDeProduccion.aspx')");
            //Costos administrativos
            $('#tc_proyectos_tabCostosAdministrativos_tab').attr("onclick", "LoadIFrame('holderCostosAdministrativos','../Finanzas/CostosAdministrativos.aspx')");
            //Ingresos                
            $('#tc_proyectos_tabIngresos_tab').attr("onclick", "LoadIFrame('holderIngresos','../Finanzas/Ingresos.aspx')");
            //Egresos
            $('#tc_proyectos_tabEgresos_tab').attr("onclick", "LoadIFrame('holderEgresos','../Finanzas/Egresos.aspx')");
            //Capital de trabajo
            $('#tc_proyectos_tabCapitalDeTrabajo_tab').attr("onclick", "LoadIFrame('holderCapitalDeTrabajo','../Finanzas/CapitalDeTrabajo.aspx')");
           
            LoadIFrame('holderPlanDeCompras', '../Finanzas/PlanDeCompras.aspx');
        });
    </script>

    <!--<style type="text/css">
        #tc_proyectos_body {
            height : 800px !important;
        } 
    </style>-->
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="1200px">

                <ajaxToolkit:TabPanel ID="tabPlanDeCompras" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>Plan de compras</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderPlanDeCompras" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabCostosDeProduccion" OnDemandMode="Once" runat="server" 
                    Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>Costos de producción</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderCostosDeProduccion" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabCostosAdministrativos" OnDemandMode="Once" runat="server" 
                    Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>Costos administrativos</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderCostosAdministrativos" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabIngresos" OnDemandMode="Once" runat="server" 
                    Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>Ingresos</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderIngresos" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabEgresos" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Egresos</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        
                            <iframe id="holderEgresos" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="no" width="100%" height="1200px"></iframe>
                        
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                 <ajaxToolkit:TabPanel ID="tabCapitalDeTrabajo" OnDemandMode="Once" runat="server" 
                     Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Capital de trabajo</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderCapitalDeTrabajo" marginwidth="0" marginheight="0" frameborder="0"
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                                                             
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
