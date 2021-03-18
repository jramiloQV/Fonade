<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Solucion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Solucion.Solucion" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
</head>
<body>
    <form id="formSolucion" runat="server">
        <div id="divParentContainer" runat="server">
            <div class="childContainer">
                <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                    <ContentTemplate>
                        <% Page.DataBind(); %>
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <div style="position: relative; left: 720px; width: 160px;">
                            <uc1:Post_It runat="server" ID="Post_It" Visible='<%# PostitVisible %>' _txtCampo="Solucion"
                                _mostrarPost='<%# PostitVisible %>'/>
                        </div>
                        <div style="text-align: center">
                            <h1>III. ¿Cuál es mi Solución?</h1>
                        </div>
                        <uc1:Help runat="server" ID="Help" Titulo="5. Describa la alternativa o solución que usted propone para satisfacer las necesidades señaladas en la pregunta 2:" Mensaje="AlternativaOSolucion" />
                        <blockquote style="font-style: italic">Nota: La alternativa o solución debe ser descrita dando respuesta a los siguientes interrogantes: ¿qué se ofrece (concepto del negocio) y qué lo hace diferente (componente innovador)?</blockquote>
                        <uc1:Help runat="server" ID="Help7" Titulo="Concepto del negocio:" Mensaje="ConceptoNegocioV2" />
                        <asp:RequiredFieldValidator ID="RequiredCKConceptoNegocio" runat="server" ControlToValidate="CKConceptoNegocio" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Concepto del Negocio") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <CKEditor:CKEditorControl ID="CKConceptoNegocio" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <fieldset>
                            <legend>Componente Innovador o Factor Diferencial:</legend>
                            <uc1:Help runat="server" ID="Help1" Titulo="Concepto del negocio:" mensaje="ConceptoNegocioInnovador"/>
                            <asp:RequiredFieldValidator ID="RequiredCKConceptoNegocio2" runat="server" ControlToValidate="CKConceptoNegocio2" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Componente innovador/Concepto del negocio") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <CKEditor:CKEditorControl ID="CKConceptoNegocio2" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                            <br />
                            <uc1:Help runat="server" ID="Help2" Titulo="Producto o servicio:" mensaje="ProductoOServicio" />
                            <asp:RequiredFieldValidator ID="RequiredCKProductoServicio" runat="server" ControlToValidate="CKProductoServicio" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Producto o servicio") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <CKEditor:CKEditorControl ID="CKProductoServicio" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                            <br />
                            <uc1:Help runat="server" ID="Help3" Titulo="Proceso:" mensaje="Proceso" />
                            <asp:RequiredFieldValidator ID="RequiredCKProceso" runat="server" ControlToValidate="CKProceso" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Proceso") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <CKEditor:CKEditorControl ID="CKProceso" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        </fieldset>
                        <uc1:Help runat="server" ID="HelpComoValido" Mensaje="ComoValido" Titulo="6. ¿Cómo validó la aceptación en el mercado de su proyecto (metodología y resultados)?" />
                        <blockquote style="font-style: italic">Nota: Dentro de los resultados, destaque la identificación de las motivaciones que tienen los clientes para adquirir su producto.</blockquote>
                        <asp:RequiredFieldValidator ID="RequiredCKComoValido" runat="server" ControlToValidate="CKComoValido" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"¿Cómo validó la aceptación...") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <CKEditor:CKEditorControl ID="CKComoValido" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <br />
                        <uc1:Help runat="server" ID="Help8" Mensaje="AvanceLogrado" Titulo="7. Describa el avance logrado a la fecha para la puesta en marcha de su proyecto, en los aspectos: técnico - productivo, comercial y legal." />
                        <blockquote style="font-style: italic">Nota: En caso de haber realizado ventas, relacione las cantidades e ingresos generados. Si cuenta actualmente con un producto mínimo viable o infraestructura, realice una descripción de los mismos.</blockquote>
                        <uc1:Help runat="server" ID="Help4" Mensaje="TecnicoProductivo" Titulo="Técnico - productivo:" />
                        <asp:RequiredFieldValidator ID="RequiredCKTecnicoproductivo" runat="server" ControlToValidate="CKTecnicoproductivo" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Describa el avance...") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <CKEditor:CKEditorControl ID="CKTecnicoproductivo" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <br />
                        <uc1:Help runat="server" ID="Help5" Mensaje="Comercial" Titulo="Comercial:" />
                        <asp:RequiredFieldValidator ID="RequiredCKComercial" runat="server" ControlToValidate="CKComercial" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Comercial") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <CKEditor:CKEditorControl ID="CKComercial" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <br />
                        <uc1:Help runat="server" ID="Help6" Mensaje="Legal" Titulo="Legal:" />
                        <asp:RequiredFieldValidator ID="RequiredCKLegal" runat="server" ControlToValidate="CKLegal" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Legal") %>' SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <CKEditor:CKEditorControl ID="CKLegal" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <blockquote>
                            <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Italic="True" ForeColor="Red" HeaderText="Advertencia:" />
                        </blockquote>
                        <div id="divBotones" runat="server" visible='<%# AllowUpdate %>' style="text-align: center">
                            <asp:Button ID="BtnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="BtnLimpiarCampos_Click" CausesValidation="False" />&nbsp;<asp:Button ID="BtnGuardar" runat="server" Text="Guardar" OnClick="BtnGuardar_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                    <ProgressTemplate>
                        <div style="text-align: right"><b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <br />
            </div>
        </div>
    </form>
</body>
</html>
