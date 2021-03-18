<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuIndicadoresDeGestion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Empresa.IndicadoresDeGestion.MenuIndicadoresGestion" %>

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
            //Indicadores genericos
            $('#tc_proyectos_tabIndicadoresGenericos_tab').attr("onclick", "LoadIFrame('holderIndicadoresGenericos','../IndicadoresDeGestion/IndicadoresGenericos.aspx')");
            //Indicadores especificos
            $('#tc_proyectos_tabIndicadoresEspecificos_tab').attr("onclick", "LoadIFrame('holderIndicadoresEspecificos','../IndicadoresDeGestion/IndicadoresEspecificos.aspx')");

            LoadIFrame('holderIndicadoresGenericos', '../Avance/PlanOperativo.aspx');
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

                <ajaxToolkit:TabPanel ID="tabIndicadoresGenericos" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span> Indicadores genericos </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderIndicadoresGenericos" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabIndicadoresEspecificos" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Indicadores especificos</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderIndicadoresEspecificos" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                                                             
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
