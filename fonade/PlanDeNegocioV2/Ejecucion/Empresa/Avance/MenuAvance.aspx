<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuAvance.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Empresa.Avance.MenuAvance" %>

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
            //Plan operativo
            $('#tc_proyectos_tabPlanOperativo_tab').attr("onclick", "LoadIFrame('holderPlanOperativo','../Avance/PlanOperativo.aspx')");
            //Nomina
            $('#tc_proyectos_tabNomina_tab').attr("onclick", "LoadIFrame('holderNomina','../Avance/Nomina.aspx')");
            //Producción
            $('#tc_proyectos_tabProduccion_tab').attr("onclick", "LoadIFrame('holderProduccion','../Avance/Produccion.aspx')");
            //Ventas
            $('#tc_proyectos_tabVentas_tab').attr("onclick", "LoadIFrame('holderVentas','../Avance/Ventas.aspx')");
           
            LoadIFrame('holderPlanOperativo', '../Avance/PlanOperativo.aspx');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="661px">

                <ajaxToolkit:TabPanel ID="tabPlanOperativo" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span> Plan Operativo </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderPlanOperativo" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabNomina" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Nomina</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderNomina" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                 <ajaxToolkit:TabPanel ID="tabProduccion" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Producción</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderProduccion" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                 <ajaxToolkit:TabPanel ID="tabVentas" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Ventas</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderVentas" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                                                             
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
