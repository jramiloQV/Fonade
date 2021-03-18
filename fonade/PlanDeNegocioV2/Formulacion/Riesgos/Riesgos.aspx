<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Riesgos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Riesgos.Riesgos" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>
<!DOCTYPE html >
<html style="overflow-x: hidden;">
<head runat="server">
    <title>Riesgos</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
</head>
<body>
    <form id="formRiesgos" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <% Page.DataBind(); %>
                <div id="divParentContainer" runat="server">
                    <div class="childContainer">
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <div style="position: relative; left: 720px; width: 160px;">
                            <uc1:Post_It runat="server" ID="Post_It" Visible='<%# PostitVisible %>' _txtCampo="Riesgos" _mostrarPost='<%# PostitVisible %>' />
                        </div>
                        <div style="text-align: center">
                            <h1>VI. ¿Qué Riesgos Enfrento?</h1>
                        </div>
                        <br />
                        <uc1:Help runat="server" ID="Help" Titulo="¿Qué actores externos son críticos para la ejecución del negocio? Indique el nombre y su rol en la ejecución." Mensaje="ActoresExternos" />
                        <CKEditor:CKEditorControl ID="CKActores" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <asp:RequiredFieldValidator ID="RequiredCKActores" runat="server" ControlToValidate="CKActores" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"¿Qué actores...") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                        <br />
                        <uc1:Help runat="server" ID="Help1" Titulo="¿Qué factores externos pueden afectar la operación del negocio, y cuál es el plan de acción para mitigar estos riesgos?" Mensaje="FactoresExternos" />
                        <CKEditor:CKEditorControl ID="CKFactores" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <asp:RequiredFieldValidator ID="RequiredCKFactores" runat="server" ControlToValidate="CKFactores" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"¿Qué factores...") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                        <blockquote>
                            <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Italic="True" ForeColor="Red" HeaderText="Advertencia:" ValidationGroup="Requerido" />
                        </blockquote>
                        <div id="divBotones" runat="server" visible='<%# AllowUpdate %>' style="text-align: center">
                            <asp:Button ID="BtnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="BtnLimpiarCampos_Click" />&nbsp;<asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="Requerido" OnClick="BtnGuardar_Click" />
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
