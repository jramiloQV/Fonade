<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PeriodoArranque.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.PeriodoArranque" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Periodo de Arranque</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
</head>
<body>
    <form id="FormEstrategia" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <% Page.DataBind(); %>
                <div id="divParentContainer" runat="server">
                    <div class="childContainer">
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <div style="position: relative; left: 720px; width: 160px;">
                            <uc1:Post_It runat="server" ID="Post_It" Visible='<%# PostitVisible %>' _txtCampo="PeriodoArranque" _mostrarPost='<%# PostitVisible %>' />
                        </div>
                        <div style="text-align: center">
                            <h1>V. ¿Cuál es el Futuro de mi Negocio?</h1>
                        </div>
                        <uc1:Help runat="server" ID="Help" Titulo="19. ¿Cuál es el período de arranque del proyecto (meses)?" Mensaje="PeriodoArranque" />
                        <blockquote style="font-style: italic">
                            Nota: Nota: Este tiempo corresponde al período estimado entre la fecha de firma del acta de inicio del contrato y la aprobación del lugar de operaciones.
                        </blockquote>
                        <CKEditor:CKEditorControl ID="CKArranque" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <asp:RequiredFieldValidator ID="RequiredCKArranque" runat="server" ControlToValidate="CKArranque" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"¿Cuál es el período de arranque...") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                        <br />
                        <uc1:Help runat="server" ID="Help1" Titulo="20. ¿Cuál es el período improductivo (meses) que exige el primer ciclo de producción?" Mensaje="PeriodoImproductivo" />
                        <blockquote style="font-style: italic">
                            Nota: Este tiempo corresponde al período estimado entre la fecha de firma del acta de inicio del contrato y la producción del primer lote de bienes o servicios.
                        </blockquote>
                        <CKEditor:CKEditorControl ID="CKImproductivo" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <asp:RequiredFieldValidator ID="RequiredCKImproductivo" runat="server" ControlToValidate="CKImproductivo" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"¿Cuál es el período improductivo...") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                        <blockquote>
                            <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Italic="True" ForeColor="Red" HeaderText="Advertencia:" ValidationGroup="Requerido" />
                        </blockquote>
                        <div id="divBotones" runat="server" visible='<%# AllowUpdate %>' style="text-align: center">
                            <asp:Button ID="BtnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="BtnLimpiarCampos_Click" />&nbsp;<asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="Requerido" OnClick="BtnGuardar_Click" OnClientClick="SummaryFocus(1000);" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
            <ProgressTemplate>
                <div style="text-align: right"><b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
