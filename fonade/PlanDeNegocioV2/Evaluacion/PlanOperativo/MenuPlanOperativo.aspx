<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="MenuPlanOperativo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo.MenuPlanOperativo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Plan operativo
            $('#bodyHolder_tc_proyectos_tc_planOperativo_tab').attr("onclick", "LoadIFrame('holderPlanOperativo','PlanOperativo.aspx')");
            //Nomina
            $('#bodyHolder_tc_proyectos_tc_Nomina_tab').attr("onclick", "LoadIFrame('holderNomina','Nomina.aspx')");
            //Producción
            $('#bodyHolder_tc_proyectos_tc_Produccion_tab').attr("onclick", "LoadIFrame('holderProduccion','Produccion.aspx')");
            //Ventas
            $('#bodyHolder_tc_proyectos_tc_Ventas_tab').attr("onclick", "LoadIFrame('holderVentas','Ventas.aspx')");
            
            LoadIFrame('holderPlanOperativo', 'PlanOperativo.aspx');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="480px">
            
            <ajaxToolkit:TabPanel ID="tc_planOperativo" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="indicadorfinanciero" class="tab_header">
                        <span>Plan operativo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderPlanOperativo" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_Nomina" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Nómina</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderNomina"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_Produccion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Producción</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderProduccion"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_Ventas" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Ventas</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderVentas" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
                                       
        </ajaxToolkit:TabContainer>
</asp:Content>
