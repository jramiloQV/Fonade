<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Produccion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Produccion" %>

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
                            <uc2:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="Produccion" />
                            <br />
                        </div>
                        <div style="text-align: center">
                            <h1>IV. ¿Cómo desarrollo mi solución?</h1>
                            <br />
                            <br />
                        </div>
                        <uc3:Help runat="server" ID="HelpPregunta143" Mensaje="DetalleCondiciones" Titulo="14.3. Detalle las condiciones técnicas de infraestructura: áreas requeridas y distribución de espacios. (Anexar mapa y /o plano)" />
                        <br />
                        <blockquote style="font-style: italic">Nota: Si ha seleccionado el sitio de operación, realizar la consulta del POT / EOT para validar que este lugar se encuentra habilitado para el uso del suelo que usted requiere y demás normatividad que aplica para su negocio.</blockquote>
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta143" runat="server" ControlToValidate="CKEPregunta143" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(107)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKEPregunta143" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />

                        <uc3:Help runat="server" ID="HelpPregunta144" Mensaje="AdquisicionActivo" Titulo="14.4. ¿Para la adquisición de algún activo, se tiene contemplado realizar importación? (SI / NO, justificación)" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta144ddl" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(114)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="ddlPregunta144" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <asp:DropDownList ID="ddlPregunta144" runat="server" AutoPostBack="True" CausesValidation="True" ValidationGroup="grupo1" ClientIDMode="Static" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="1">SI</asp:ListItem>
                            <asp:ListItem Value="0">NO</asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta144cke" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(115)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_Pregunta144" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <CKEditor:CKEditorControl ID="cke_Pregunta144" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />


                        <uc3:Help ID="HelpPregunta1441" runat="server" Mensaje="DetalleActivo" Titulo="14.4.1. Detalle los activos, países proveedores y tiempos estimados:" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta1441" runat="server" ControlToValidate="CKEPregunta1441" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(116)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKEPregunta1441" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />


                        <uc3:Help ID="HelpPregunta1442" runat="server" Mensaje="IncrementoActivo" Titulo="14.4.2. En caso de presentarse incremento en el valor del activo por factores como: tasa de cambio, reformas tributarias, etc. ¿Cómo financiará éste mayor valor?" />
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CKEPregunta1442" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(117)) %>' Font-Bold="True" Font-Size="X-Large" ForeColor="Red" SetFocusOnError="True" Text="*" Display="None" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="CKEPregunta1442" runat="server" BasePath="~/ckeditor" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' EnableTabKeyTools="true" ForcePasteAsPlainText="false" ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />

                        <uc3:Help ID="HelpPregunta15" runat="server" Mensaje="ProcesoProduccion" Titulo="15. ¿Cuál es el proceso que se debe seguir para la producción del bien o prestación del servicio?. Nota: Elabore un cuadro para cada producto." />
                        <br />
                        <ajaxToolkit:Accordion ID="Accordion1" runat="server" RequireOpenedPane ="false"
                            FadeTransitions="True"
                            FramesPerSecond="50"
                            TransitionDuration="200"
                            HeaderCssClass="definedAccordianHeader" HeaderSelectedCssClass="definedAccordianSelectedHeader">
                            <Panes>
                            </Panes>
                        </ajaxToolkit:Accordion>
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
                        <br />
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
