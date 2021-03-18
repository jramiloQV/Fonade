<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionIndicadoresFrame.aspx.cs"  Inherits="Fonade.FONADE.evaluacion.EvaluacionIndicadoresFrame" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
      <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //Indicadores
            $('#tc_proyectos_tc_plan_tab').attr("onclick", "CargarPestana('frmaportes','EvaluacionAportes.aspx')");
            //Cargue
            $('#tc_proyectos_tc_nomina_tab').attr("onclick", "CargarPestana('frmNomina','EvaluacionProductos.aspx')");
            //Centrales
            $('#tc_proyectos_tc_produccion_tab').attr("onclick", "CargarPestana('frmproduccion','EvaluacionRiesgos.aspx')");
            //evaluacion
            $('#tc_proyectos_tc_ventas_tab').attr("onclick", "CargarPestana('frmventas','EvaluacionConclusion.aspx')");
        });
    </script>
</head>
<body>
    
    <form id="form1" runat="server">
      <div>
           <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" 
               Width="100%" Height="480px">
            <ajaxToolkit:TabPanel ID="tc_plan" OnDemandMode="None" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header" id="aportes">
                        <span>Aportes</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmaportes" src="EvaluacionAportes.aspx" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_nomina" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Indicadores de Gestión y Cumplimiento</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmNomina" src="" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_produccion" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Riesgos</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmproduccion" src="" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_ventas" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                        <span>Conclusión</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmventas" src="" marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
    </form>
      <script type="text/javascript">
          $(document).ready(function () {

              //$("#aportes").click();
          });
    </script>
</body>
</html>
