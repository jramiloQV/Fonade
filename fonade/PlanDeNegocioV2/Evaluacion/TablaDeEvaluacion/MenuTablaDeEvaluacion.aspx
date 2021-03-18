<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="MenuTablaDeEvaluacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.MenuTablaDeEvaluacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {            
            //Datos generales
            $('#bodyHolder_tc_proyectos_tabDatosGenerales_tab').attr("onclick", "LoadIFrame('holderDatosGenerales','../TablaDeEvaluacion/DatosGenerales.aspx')");
            //QuienEsElProtagonista            
            $('#bodyHolder_tc_proyectos_tabProtagonista_tab').attr("onclick", "LoadIFrameAspectos('holderProtagonista','../TablaDeEvaluacion/EvaluacionAspectos.aspx',69)");
            //OportunidadMercado 
            $('#bodyHolder_tc_proyectos_tabOportunidadMercado_tab').attr("onclick", "LoadIFrameAspectos('holderOportunidadMercado','../TablaDeEvaluacion/EvaluacionAspectos.aspx',74)");
            //MiSolucion
            $('#bodyHolder_tc_proyectos_tabMiSolucion_tab').attr("onclick", "LoadIFrameAspectos('holderMiSolucion','../TablaDeEvaluacion/EvaluacionAspectos.aspx',87)");
            //DesarrolloSolucion
            $('#bodyHolder_tc_proyectos_tabDesarrolloSolucion_tab').attr("onclick", "LoadIFrameAspectos('holderDesarrolloMiSolucion','../TablaDeEvaluacion/EvaluacionAspectos.aspx',102)");
            //FuturoNegocio
            $('#bodyHolder_tc_proyectos_tabFuturoNegocio_tab').attr("onclick", "LoadIFrameAspectos('holderFuturoNegocio','../TablaDeEvaluacion/EvaluacionAspectos.aspx',123)");
            //Riesgos
            $('#bodyHolder_tc_proyectos_tabRiesgos_tab').attr("onclick", "LoadIFrameAspectos('holderRiesgos','../TablaDeEvaluacion/EvaluacionAspectos.aspx',136)");
            //ResumenEjecutivo
			//Se cambia el id de resumen ejecuto de 140 a 156
            $('#bodyHolder_tc_proyectos_tabResumenEjecutivo_tab').attr("onclick", "LoadIFrameAspectos('holderResumenEjecutivo','../TablaDeEvaluacion/EvaluacionAspectos.aspx',156)");

            LoadIFrame('holderDatosGenerales', '../TablaDeEvaluacion/DatosGenerales.aspx');
        });        
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">   
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="650px">
                
                <ajaxToolkit:TabPanel ID="tabDatosGenerales" OnDemandMode="Once" runat="server" Width="100%" Height="100%">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Datos generales</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderDatosGenerales" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabProtagonista" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Protagonista</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderProtagonista" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabOportunidadMercado" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>Oportunidad mercado</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderOportunidadMercado" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabMiSolucion" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span>¿Cuál es mi solución?</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderMiSolucion" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabDesarrolloSolucion" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span> ¿Cómo desarrollo mi solución? </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderDesarrolloMiSolucion" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabFuturoNegocio" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span> ¿Cuál es el futuro de mi negocio? </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderFuturoNegocio" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabRiesgos" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span> ¿Qué riesgos enfrento? </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderRiesgos" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabResumenEjecutivo" OnDemandMode="Once" runat="server" Width="100%" Height="100%" Visible="false">
                    <HeaderTemplate>
                        <div class="tab_header">
                            <span> Resumen ejecutivo </span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderResumenEjecutivo" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

            </ajaxToolkit:TabContainer>
        </div>
    </div>
</asp:Content>
