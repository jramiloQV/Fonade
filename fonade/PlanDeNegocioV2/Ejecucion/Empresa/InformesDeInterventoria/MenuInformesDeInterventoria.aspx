<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuInformesDeInterventoria.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Empresa.InformesDeInterventoria.MenuInformesDeInterventoria" %>

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
            //Visita interventoria
            $('#tc_proyectos_tabVisitaInterventoria_tab').attr("onclick", "LoadIFrame('holderVisitaInterventoria','../InformesDeInterventoria/VisitaInterventoria.aspx')");
            //Avance Bimensual
            $('#tc_proyectos_tabAvanceBimensual_tab').attr("onclick", "LoadIFrame('holderAvanceBimensual','../InformesDeInterventoria/AvanceBimensual.aspx')");
            //Ejecución presupuestal
            $('#tc_proyectos_tabEjecucionPresupuestal_tab').attr("onclick", "LoadIFrame('holderEjecucionPresupuestal','../InformesDeInterventoria/EjecucionPresupuestal.aspx')");
            //Consolidado interventoria
            $('#tc_proyectos_tabConsolidadoInterventoria_tab').attr("onclick", "LoadIFrame('holderConsolidadoInterventoria','../InformesDeInterventoria/ConsolidadoInterventoria.aspx')");

            LoadIFrame('holderVisitaInterventoria', '../InformesDeInterventoria/VisitaInterventoria.aspx');
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

                <ajaxToolkit:TabPanel ID="tabVisitaInterventoria" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span> Visita interventoria </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderVisitaInterventoria" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabAvanceBimensual" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Avance Bimensual</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderAvanceBimensual" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                        
                <ajaxToolkit:TabPanel ID="tabEjecucionPresupuestal" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Ejecución Presupuestal</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderEjecucionPresupuestal" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabConsolidadoInterventoria" OnDemandMode="Once" runat="server" Width="100%" Height="800px" >
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Consolidado interventoria</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderConsolidadoInterventoria" marginwidth="0" marginheight="0" frameborder="0" scrolling="no" width="100%" height="800px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                                                     
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>