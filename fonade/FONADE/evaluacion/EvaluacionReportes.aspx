<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="EvaluacionReportes.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReporteTareas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <style type="text/css">
        .lnk_informe
        {
            text-decoration: none;
            color: Black;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div class="help_container" style="background-color: #FFFFFF; height: 530px;">
        <br />
        <br />
        <div id="imagen1" runat="server" style="margin-left: 30px;">
            <img src="../../Images/imgAyuda.gif" onclick="textoAyuda({titulo: 'Informe de Evaluación', texto: 'InformeEvaluacion'});"
                border="0" alt="help_Objetivos" />
            <asp:LinkButton ID="lnkinforme" runat="server" Text="Informe de Evaluación" CssClass="lnk_informe"
                OnClick="lnkinforme_Click" ForeColor="Black" Font-Bold="true" />
        </div>
        <div id="imagen2" runat="server" style="display: none">
            <a onclick="textoAyuda({titulo: 'Aspectos Evaluados', texto: 'AspectoMAmbiente'});">
                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" /></a>
            <strong>Aspectos Evaluados </strong>
        </div>
    </div>
    <br />
    <br />
</asp:Content>
