<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="MenuEvaluacionFinanciera.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera.MenuEvaluacionFinanciera" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Indicadores
            $('#bodyHolder_tc_proyectos_tc_observaciones_tab').attr("onclick", "LoadIFrame('frmIndicadores','EvaluacionIndicadoresFinancieros.aspx')");
            //Cargue
            $('#bodyHolder_tc_proyectos_tc_modelo_tab').attr("onclick", "LoadIFrame('frmModelo','EvaluacionModeloFinanciero.aspx')");
            //Centrales
            $('#bodyHolder_tc_proyectos_tc_centrales_tab').attr("onclick", "LoadIFrame('frmcentral','EvaluacionCentrales.aspx')");
            //evaluacion
            $('#bodyHolder_tc_proyectos_tc_evalua_tab').attr("onclick", "LoadIFrame('frmevalua','EvaluacionProyecto.aspx')");
            //Flujo
            $('#bodyHolder_tc_proyectos_tc_flujo_tab').attr("onclick", "LoadIFrame('frmflujo','EvaluacionFlujoCaja.aspx')");

            LoadIFrame('frmIndicadores', 'EvaluacionIndicadoresFinancieros.aspx');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="480px">
            <ajaxToolkit:TabPanel ID="tc_observaciones" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="indicadorfinanciero" class="tab_header">
                        <span>Indicadores Financieros</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmIndicadores" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_modelo" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Cargue Modelo Financiero</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmModelo"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_centrales" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Centrales de Riesgo</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmcentral"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_evalua" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Evaluación del Proyecto</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmevalua" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
               <ajaxToolkit:TabPanel ID="tc_flujo" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Flujo Caja</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmflujo" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="530px"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>

</asp:Content>
