<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="OportunidadMercado.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.OportunidadMercado.MenuOportunidadMercado" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>

<!DOCTYPE html >
<html style="overflow-x: hidden;">
<head runat="server">
    <title>Estrategia de Mercado</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
</head>
<body>
    <% Page.DataBind(); %>
    <form id="formOportunidad" runat="server">
        <div id="divParentContainer">
            <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <% Page.DataBind(); %>
                    <div style="position: relative; left: -10px">
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                    </div>
                    <div style="position: relative; left: 740px; width: 160px;">
                        <uc1:Post_It runat="server" ID="Post_It" Visible='<%# PostitVisible %>' _txtCampo="OportunidadMercado" _mostrarPost='<%# PostitVisible %>'/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="text-align: center">
                <h1>II. ¿Existe Oportunidad en el Mercado?</h1>
            </div>
            <br />
            <uc1:Help runat="server" ID="HelpTendencia" Titulo="3. Describa la tendencia de crecimiento del mercado en el que se encuentra su negocio" Mensaje="TendenciaMercado" />
            <asp:RequiredFieldValidator ID="RequiredCKTendencia" runat="server" ControlToValidate="CKTendencia" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Tendencia") %>' Font-Bold="True" ForeColor="Red" SetFocusOnError="False" ToolTip="Requerido" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
            <CKEditor:CKEditorControl ID="CKTendencia" runat="server" ValidationGroup="Requerido" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
            <br />
            <div>
                <uc1:Help runat="server" ID="HelpCompetencia" Titulo="4. Realice un análisis de la competencia, alrededor de los criterios* más relevantes para su negocio:" Mensaje="AnalisisCompetencia" />
                <blockquote style="font-style: italic">Nota: * Seleccione de las siguientes opciones de criterios, aquellos para los cuales se identifica como alto nivel de criticidad para la validación de la competencia.</blockquote>
                <asp:UpdatePanel ID="UpdatePanelGrid" runat="server">
                    <ContentTemplate>
                        <% Page.DataBind(); %>
                        <div id="divAgregar" runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgAgregar" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAgregarCompetidor_Click" />
                            &nbsp;
                <asp:LinkButton ID="btnAgregarCompetidor" runat="server" Text="Adicionar Competidor" OnClick="btnAgregarCompetidor_Click"></asp:LinkButton>
                        </div>
                        <br />
                        <asp:GridView ID="gwCompetidores" runat="server" CssClass="Grilla" RowStyle-Height="35px" GridLines="None" CellSpacing="1" CellPadding="4" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" PageSize="5" ShowHeaderWhenEmpty="True" EmptyDataText="No existen registros." Caption="Competencia" OnPageIndexChanging="gwCompetidores_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdCompetidor") %>' OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" OnClick="btnEliminar_Click" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:LinkButton ID="BtnEditarCompetidor" runat="server" OnClick="BtnEditarCompetidor_Click" CommandArgument='<%# Bind("IdCompetidor") %>' Width="70" Enabled='<%# AllowUpdate %>'><%# Eval("Nombre") %></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Localización">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; width: 135px;"><%# Eval("Localizacion") %></div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Productos y Servicios<br />(Atributos)">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; width: 135px;"><%# Eval("ProductosServicios") %></div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Precios">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; width: 135px;"><%# Eval("Precios") %></div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Logistica de Distribución">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; width: 135px;"><%# Eval("LogisticaDistribucion") %></div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Otro, ¿Cuál?">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word; width: 135px;"><%# Eval("OtroCual") %></div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle Height="35px"></RowStyle>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <blockquote>
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Italic="True" ForeColor="Red" HeaderText="Advertencia:" ValidationGroup="Requerido" />
                </blockquote>
                <div id="divBotones" runat="server" visible='<%# AllowUpdate %>' style="text-align: center">
                    <asp:Button ID="BtnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="BtnLimpiarCampos_Click" />&nbsp;<asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="Requerido" OnClick="BtnGuardar_Click" />
                </div>
            </div>
        </div>
        <asp:HiddenField ID="HiddenWidth" runat="server" />
        <script>
            $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
        </script>
    </form>
</body>
</html>
