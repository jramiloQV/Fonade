<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoPPtalInterTabs.aspx.cs" Inherits="Fonade.FONADE.interventoria.SeguimientoPPtalInterTabs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <style type="text/css">
        #frmEvalAportes{
            height: 700px !important;
        }
        #frmEvalPresupuesto{
            height: 700px !important;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            //Aportes
            $('#tc_proyectos_tbaportes_tab').attr("onclick", "CargarPestana('frmEvalAportes','SeguimientoPptalAportes.aspx')");
            //Presupuesto
            $('#tc_proyectos_tbpresupuesto_tab').attr("onclick", "CargarPestana('frmEvalPresupuesto','SeguimientoPptal.aspx')");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
    <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" scrolling="no"
            Height="100%">
            <ajaxToolkit:TabPanel ID="tbaportes" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="aportesdiv" class="tab_header">
                        <span>Aportes</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmEvalAportes" src="SeguimientoPptalAportes.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" width="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tbpresupuesto" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Presupuesto</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmEvalPresupuesto" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="90%"></iframe>
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
