<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="MenuConceptoFinal.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.MenuConceptoFinal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Aportes
            $('#bodyHolder_tc_proyectos_tc_aportes_tab').attr("onclick", "LoadIFrame('holderAportes','Aportes.aspx')");
            //Indicadores de gestion y cumplimiento
            $('#bodyHolder_tc_proyectos_tc_indicadores_tab').attr("onclick", "LoadIFrame('holderIndicadores','Indicadores.aspx')");
            //Riesgos
            $('#bodyHolder_tc_proyectos_tc_riesgos_tab').attr("onclick", "LoadIFrame('holderRiesgos','Riesgos.aspx')");
            //Conclusión
            $('#bodyHolder_tc_proyectos_tc_conclusion_tab').attr("onclick", "LoadIFrame('holderConclusion','Conclusion.aspx')");
            
            LoadIFrame('holderAportes', 'Aportes.aspx');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="480px">
            
            <ajaxToolkit:TabPanel ID="tc_aportes" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="indicadorfinanciero" class="tab_header">
                        <span>Aportes</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderAportes" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_indicadores" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Indicadores financieros</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderIndicadores"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

            <ajaxToolkit:TabPanel ID="tc_riesgos" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Riesgos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderRiesgos"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_conclusion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Conclusión</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="holderConclusion" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
                                       
        </ajaxToolkit:TabContainer>
</asp:Content>
