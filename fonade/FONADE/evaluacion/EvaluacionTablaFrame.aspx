<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionTablaFrame.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionTablaFrame" ValidateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Identificacion
            $('#tc_proyectos_tc_observaciones_tab').attr("onclick", "CargarPestana('frmIdentificacion','EvaluacionObservaciones.aspx')");
            //Generales
            $('#tc_proyectos_tc_generales_tab').attr("onclick", "CargarPestana('frmGenerales','EvaluacionAspectos.aspx?codAspecto=1')");
            //Comerciales
            $('#tc_proyectos_tc_frmcomercial_tab').attr("onclick", "CargarPestana('frmcomercial','EvaluacionAspectos.aspx?codAspecto=2')");
            //Tecnicos
            $('#tc_proyectos_tc_tecnico_tab').attr("onclick", "CargarPestana('frmtecnico','EvaluacionAspectos.aspx?codAspecto=3')");
            //Organizacionales
            $('#tc_proyectos_tc_organizacional_tab').attr("onclick", "CargarPestana('frmorgani','EvaluacionAspectos.aspx?codAspecto=4')");
            //Financiero
            $('#tc_proyectos_tc_financiero_tab').attr("onclick", "CargarPestana('frmfinanciero','EvaluacionAspectos.aspx?codAspecto=5')");
            //Medio ambiente
            $('#tc_proyectos_tc_medio_tab').attr("onclick", "CargarPestana('frmmedio','EvaluacionAspectos.aspx?codAspecto=6')");
        });       
    </script>
</head>
<body>
   
    <form id="form2" runat="server">
      <div>
           <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="480px">
            <ajaxToolkit:TabPanel ID="tc_observaciones" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header"  id="dividentificacion">
                        <span>Identificación del Proyecto</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmIdentificacion" src="EvaluacionObservaciones.aspx"
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_generales" runat="server" Width="100%" Height="100%" Visible ="false">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Generales</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmGenerales" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_frmcomercial" runat="server" Width="100%" Height="100%" Visible ="false">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Comerciales</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmcomercial" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_tecnico" runat="server" Width="100%" Height="100%" Visible ="false">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Técnicos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmtecnico" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
               <ajaxToolkit:TabPanel ID="tc_organizacional" runat="server" Width="100%" Height="100%" Visible ="false">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Organizacionales</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmorgani" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            
            <ajaxToolkit:TabPanel ID="tc_financiero" runat="server" Width="100%" Height="100%" Visible ="false">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Financiero</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmfinanciero" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_medio" runat="server" Width="100%" Height="100%" Visible ="false">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Medio Ambiente</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmmedio" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>

          
        </ajaxToolkit:TabContainer>
    </div>
    </form>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#dividentificacion").click();
        });
    </script>
</body>
</html>
