<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionReportes.aspx.cs" ValidateRequest="false"
    Inherits="Fonade.PlanDeNegocioV2.Evaluacion.Informes.EvaluacionReportes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <!--<link href="../../../Styles/siteProyecto.css" rel="stylesheet" />-->
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="help_container" style="background-color: #FFFFFF; height: 530px;">
                <br />
                <br />
                <div id="imagen1" runat="server" style="margin-left: 30px;">
                    <img src="../../../Images/imgAyuda.gif" onclick="textoAyuda({titulo: 'Informe de Evaluación', texto: 'InformeEvaluacion'});"
                        border="0" alt="help_Objetivos" />
                    <asp:LinkButton ID="lnkinforme" runat="server" Text="Informe de Evaluación" CssClass="lnk_informe"
                        OnClick="lnkinforme_Click" ForeColor="Black" Font-Bold="true" />
                </div>
              <%--  <div id="imagen2" runat="server" style="display: none">
                    <a onclick="textoAyuda({titulo: 'Aspectos Evaluados', texto: 'AspectoMAmbiente'});">
                        <img src="./Images/imgAyuda.gif" border="0" alt="help_Objetivos" /></a>
                    <strong>Aspectos Evaluados </strong>
                </div>--%>
            </div>
            <asp:HiddenField ID="HiddenWidth" runat="server" />
            <script>
                $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
            </script>
        </div>
    </form>
</body>
</html>
