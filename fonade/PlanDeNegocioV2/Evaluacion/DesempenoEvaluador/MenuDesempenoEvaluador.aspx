<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="MenuDesempenoEvaluador.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.DesempenoEvaluador.MenuDesempenoEvaluador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Financiera
            $('#bodyHolder_tc_proyectos_tc_EvaluacionFinanciera_tab').attr("onclick", "LoadIFrameAspectos('frmEvaluacionFinanciera','EvaluacionFinanciera.aspx',1)");
            //Tabla
            $('#bodyHolder_tc_proyectos_tc_TablaEvaluacion_tab').attr("onclick", "LoadIFrameAspectos('frmTablaEvaluacion','EvaluacionFinanciera.aspx',4)");
            //Recomendaciones
            $('#bodyHolder_tc_proyectos_tc_ConceptoFinal_tab').attr("onclick", "LoadIFrameAspectos('frmConceptoFinal','EvaluacionFinanciera.aspx',15)");
            
            LoadIFrameAspectos('frmEvaluacionFinanciera', 'EvaluacionFinanciera.aspx',1);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">

    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="480px">
                        
            <ajaxToolkit:TabPanel ID="tc_EvaluacionFinanciera" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div id="indicadorfinanciero" class="tab_header">
                        <span>Evaluacion Financiera</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmEvaluacionFinanciera" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_TablaEvaluacion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Tabla de Evaluación</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmTablaEvaluacion"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_ConceptoFinal" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Concepto Final y Recomendaciones</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmConceptoFinal"  marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
                             
        </ajaxToolkit:TabContainer>
</asp:Content>
