<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuEmpresa.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Empresa.MenuEmpresa" %>

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
            //Registro mercantil
            $('#tc_proyectos_tabRegistroMercantil_tab').attr("onclick", "LoadIFrame('holderRegistroMercantil','../Empresa/InformesDeInterventoria/MenuInformesDeInterventoria.aspx')");
            //Avance
            $('#tc_proyectos_tabAvance_tab').attr("onclick", "LoadIFrame('holderAvance','../Empresa/Avance/MenuAvance.aspx')");
            //Indicadores de gestion
            $('#tc_proyectos_tabIndicadorDeGestion_tab').attr("onclick", "LoadIFrame('holderIndicadorDeGestion','../Empresa/IndicadoresDeGestion/MenuIndicadoresDeGestion.aspx')");
            //Informes de interventoria
            $('#tc_proyectos_tabInformesDeInterventoria_tab').attr("onclick", "LoadIFrame('holderInformesDeInterventoria','../Empresa/InformesDeInterventoria/MenuInformesDeInterventoria.aspx')");
           
            LoadIFrame('holderRegistroMercantil', '../Empresa/InformesDeInterventoria/MenuInformesDeInterventoria.aspx');
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

                <ajaxToolkit:TabPanel ID="tabRegistroMercantil" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>Registro mercantil</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderRegistroMercantil" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabAvance" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Avance</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderAvance" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                 <ajaxToolkit:TabPanel ID="tabIndicadorDeGestion" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Indicadores de gestión</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderIndicadorDeGestion" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                 <ajaxToolkit:TabPanel ID="tabInformesDeInterventoria" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Informes de interventoria</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderInformesDeInterventoria" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                                                             
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>

