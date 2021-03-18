<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NormatividadYCondicionesTecnicas.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.NormatividadYCondicionesTecnicas" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="../../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc2" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc3" TagName="Help" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <div class="parentContainer">
            <div class="childContainer">
                <asp:UpdatePanel ID="UpdatePanel" runat="server" Width="100%">
                    <ContentTemplate>
                        <% Page.DataBind();%>
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <br />
                        <div style="position: relative; left: 705px; width: 160px;">
                            <uc2:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="NormatividadYCondiciones"/>
                            <br />
                        </div>
                        <div style="text-align: center">
                            <h1>IV. ¿Cómo desarrollo mi solución?</h1>
                            <br />
                            <br />
                        </div>

                        <uc3:Help runat="server" ID="HelpPregunta12" Mensaje="DescribaNormatividad" Titulo="12. Describa la normatividad que debe cumplirse para el portafolio definido anteriormente: Identificación de la norma, procesos, costos y tiempos asociados al cumplimiento de la normatividad." />
                        <br />
                        <br />
                        <uc3:Help runat="server" ID="HelpNormEmpresa" Mensaje="NormatividadEmpresarial" Titulo="Normatividad empresarial (constitución empresa):" />
                        <br />
                        <blockquote style="font-style: italic">Nota: Si a la fecha la empresa está constituida,  por favor anexe el certificado de existencia y representación legal, y el documento privado de constitución</blockquote>
                        &nbsp;<asp:RequiredFieldValidator ID="rvNormEmpresa" runat="server" ControlToValidate="CKENormEmpresa" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(107)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKENormEmpresa" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <uc3:Help ID="HelpNormTribu" runat="server" Mensaje="NormatividadTributaria" Titulo="Normatividad tributaria:" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvNormTribu" runat="server" ControlToValidate="CKENormTribu" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(108)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKENormTribu" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <uc3:Help ID="HelpNormTecnica" runat="server" Mensaje="NormatividadTecnica" Titulo="Normatividad técnica (Permisos, licencias de funcionamiento, registros, reglamentos):" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvNormTecnica" runat="server" ControlToValidate="CKNormTecnica" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(109)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKNormTecnica" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <uc3:Help ID="HelpNormLaboral" runat="server" Mensaje="NormatividadLaboral" Titulo="Normatividad laboral:" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvNormLaboral" runat="server" ControlToValidate="CKNormLaboral" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(110)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKNormLaboral" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <uc3:Help ID="HelpNormAmbiental" runat="server" Mensaje="NormatividadAmbiental" Titulo="Normatividad ambiental:" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvNormAmbiental" runat="server" ControlToValidate="CKENormAmbiental" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(111)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKENormAmbiental" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <uc3:Help ID="HelpMarcaProp" runat="server" Mensaje="RegistroMarca" Titulo="Registro de marca – Propiedad intelectual:" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvMarcaProp" runat="server" ControlToValidate="CKEMarcaProp" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(112)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKEMarcaProp" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <uc3:Help ID="HelpPregunta13" runat="server" Mensaje="DescribaCondicionesTecnicas" Titulo="13. Describa las condiciones técnicas más importantes que se requieren para la operación del negocio." />
                        <blockquote style="font-style: italic">Nota: Para los proyectos agropecuarios, debe identificarse las condiciones ambientales como: clima, temperatura, altitud, topografía, pluviosidad, y demás requisitos de alimentación (pecuario) o fertilización (agrícola) etc.</blockquote>
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta13" runat="server" ControlToValidate="CKEPregunta13" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(113)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKEPregunta13" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />

                        <asp:ValidationSummary ID="vsErrores"
                            runat="server"
                            HeaderText="Advertencia: "
                            ForeColor="Red"
                            Font-Italic="true"
                            ValidationGroup="grupo1" />
                        <br />
                        <div style="text-align: center">
                            <asp:Button ID="btnLimpiarCampos" runat="server" OnClick="btnLimpiarCampos_Click" Text="Limpiar Campos" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                            <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" ValidationGroup="grupo1" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                        </div>
                        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                            <ProgressTemplate>
                                <div style="text-align: right">
                                    <b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
                <br />
                <br />
            </div>
        </div>
    </form>
</body>
</html>
