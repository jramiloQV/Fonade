<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionDesempenoFrame.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionDesempenoFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
       <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(function () {
            //Financiera
            $('#tc_proyectos_tc_observaciones_tab').attr("onclick", "CargarPestana('frmIdentificacion','EvaluacionFinanciera.aspx?txtTab=1')");
            //Tabla
            $('#tc_proyectos_tc_generales_tab').attr("onclick", "CargarPestana('frmGenerales','EvaluacionFinanciera.aspx?txtTab=4')");
            //Recomendaciones
            $('#tc_proyectos_tc_frmcomercial_tab').attr("onclick", "CargarPestana('frmcomercial','EvaluacionFinanciera.aspx?txtTab=15')");
        });
    </script>
</head>

<body>
    <form id="form1" runat="server">
      <div>
           <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%"
            Height="480px">
            <ajaxToolkit:TabPanel ID="tc_observaciones" OnDemandMode="Once" runat="server" Width="100%"
                Height="100%">
                <HeaderTemplate>
                    <div class="tab_header"  id="dividentificacion">
                        <span>Evaluacion Financiera</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmIdentificacion" src=""
                        marginwidth="0" marginheight="0" frameborder="0" scrolling="auto" width="100%"
                        height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_generales" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Tabla de Evaluación</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmGenerales" src="" marginwidth="0" marginheight="0" frameborder="0"
                        scrolling="auto" width="100%" height="100%"></iframe>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tc_frmcomercial" runat="server" Width="100%" Height="100%">
                <HeaderTemplate>
                    <div class="tab_header">
                      <span>Concepto Final y Recomendaciones</span>
                    </div>
                </HeaderTemplate>
                <ContentTemplate>
                    <iframe id="frmcomercial" src="" marginwidth="0" marginheight="0" frameborder="0"
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
