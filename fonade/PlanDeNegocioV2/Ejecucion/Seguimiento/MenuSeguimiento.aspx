<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSeguimiento.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Seguimiento.MenuSeguimiento" %>

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
            //Aportes
            $('#tc_proyectos_tabAportes_tab').attr("onclick", "LoadIFrame('holderAportes','../Seguimiento/Aportes.aspx')");
            //Presupuesto
            $('#tc_proyectos_tabPresupuesto_tab').attr("onclick", "LoadIFrame('holderPresupuesto','../Seguimiento/Presupuesto.aspx')");

            LoadIFrame('holderAportes', '../Seguimiento/Aportes.aspx');
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

                <ajaxToolkit:TabPanel ID="tabAportes" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span> Aportes </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderAportes" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabPresupuesto" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Presupuesto</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderPresupuesto" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                                                     
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
