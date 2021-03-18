<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuPlanOperativo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.PlanOperativo.MenuPlanOperativo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Plan operativo</title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>    
    <script src="../../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Plan operativo
            $('#tc_proyectos_tabPlanOperativo_tab').attr("onclick", "LoadIFrame('holderPlanOperativo','../PlanOperativo/PlanOperativo.aspx')");
            //Metas sociales
            $('#tc_proyectos_tabMetasSociales_tab').attr("onclick", "LoadIFrame('holderMetasSociales','../PlanOperativo/MetasSociales.aspx')");
            
            LoadIFrame('holderPlanOperativo', '../PlanOperativo/PlanOperativo.aspx');
        });
    </script> 
     <style type="text/css">
        #tc_proyectos_body {
            height : 800px !important;
        } 
    </style>
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
                            <span>Plan operativo</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderPlanOperativo" marginwidth="0" marginheight="0" frameborder="0" scrolling="yes" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabMetasSociales" OnDemandMode="Once" runat="server" Width="100%" Height="100%" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>Metas sociales</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderMetasSociales" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>                                                                                             
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
