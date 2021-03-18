<%@ Page EnableEventValidation="false" Language="C#" AutoEventWireup="true" CodeBehind="Protagonista.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Protagonista.Protagonista" MaintainScrollPositionOnPostback="true" %>



<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="uc1" %>
<%@ Register Src="../../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc2" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <style type="text/css">
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.8;
            z-index: 10000;
        }

        .panelPopup {
            display: block;
            background: white;
            height: auto;
        }

        .DivCaption {
            background-color: #00468F;
            color: #ffffff;
            font-weight: bold;
            height: 23px;
            text-align: center;
        }

        .DivCerrar {
            width: 15px;
            height: 15px;
            top: -12px;
            left: 480px;
            position: relative;
            cursor: pointer;
            font-weight: bold;
        }
    </style>
    <% Page.DataBind(); %>
    <script>
        function handleChange(opt) {
            if (opt.selectedIndex == 2) {
                if (!confirm('<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(18) %>')) {
                    opt.selectedIndex = opt.oldIndex;
                }
                else {
                    __doPostBack('DropDownPerfiles', '')
                }
            } else
                __doPostBack('DropDownPerfiles', '')
        }
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="formProtagonista" runat="server">
        <div id="divParentContainer" runat="server">
            <div class="childContainer">
                <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
                </ajaxToolkit:ToolkitScriptManager>
                <asp:UpdatePanel ID="UpdatePanel" runat="server">
                    <ContentTemplate>
                        <% Page.DataBind(); %>
                        <div>
                            <uc1:Encabezado ID="Encabezado" runat="server" />
                            <div style="position: relative; left: 740px; width: 160px;">
                                <uc2:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="Protagonista" />
                            </div>
                            <div style="text-align: center">
                                <h1>I. ¿Quién es el Protagonista?</h1>
                            </div>
                            <br />
                            <uc1:Help runat="server" ID="HelpPerfilCliente" Mensaje="QuienEsElProtagonista" Titulo="1. Describa el perfil de su cliente, junto a su localización. Justifique las razones de su elección:" />
                            <br />
                            <div runat="server" visible='<%# AllowUpdate %>'>
                                <asp:ImageButton ID="imgagregar" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAgregarCliente_Click" />
                                &nbsp;
                        <asp:LinkButton ID="btnAgregarCliente" runat="server" Text="Adicionar Cliente" OnClick="btnAgregarCliente_Click"></asp:LinkButton>
                                <br />
                            </div>
                            <asp:GridView ID="gwClientes" runat="server" CssClass="Grilla" RowStyle-Height="35px" GridLines="None" CellSpacing="1" CellPadding="4" Width="100%" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gwClientes_PageIndexChanging" PageSize="5">
                                <Columns>
                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("IdCliente") %>' OnClick="btnEliminar_Click" OnClientClick="return confirm('¿Está seguro de eliminar el registro, ya que este puede estar siendo usado en otra sección de su plan de negocio?')" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cliente">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:LinkButton ID="btnEditarCliente" runat="server" OnClick="btnEditarCliente_Click" CommandArgument='<%# Bind("IdCliente") %>' Enabled='<%# AllowUpdate %>' Width="150"><%# Eval("Nombre") %></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perfil">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Perfil") %>' Width="220"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Localización">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Localizacion") %>' Width="220"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Justificación">

                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("Justificacion") %>' Width="220"></asp:Label>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="35px"></RowStyle>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                ¿Su proyecto tiene perfiles diferentes de clientes y consumidores? 
            <asp:DropDownList ID="DropDownPerfiles" runat="server" OnSelectedIndexChanged="DropDownPerfiles_SelectedIndexChanged" CausesValidation="True" ClientIDMode="Static" Enabled='<%# AllowUpdate %>' AutoPostBack="True">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem Value="1">SI</asp:ListItem>
                <asp:ListItem Value="0">NO</asp:ListItem>
            </asp:DropDownList>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredDropDownPerfiles" runat="server" ControlToValidate="DropDownPerfiles" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"de chequeo Perfiles") %>' Font-Bold="True" ForeColor="Red" ToolTip="Requerido" Style="position: relative; top: 10px" InitialValue="" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <div id="divPerfilConsumidor" runat="server" visible="false">
                    <uc1:Help runat="server" ID="Help" Titulo="Perfil Consumidor:" Mensaje="PerfilConsumidor" />
                    <asp:RequiredFieldValidator ID="RequiredCKPerfilConsumidor" runat="server" ControlToValidate="CKPerfilConsumidor" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Perfil Consumidor") %>' Font-Bold="True" ForeColor="Red" ToolTip="Requerido" Style="position: relative; top: 10px" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                    <CKEditor:CKEditorControl ID="CKPerfilConsumidor" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                    <br />
                    <br />
                </div>
                2. ¿Cuáles son las necesidades que usted espera satisfacer de sus potenciales clientes / consumidores?
                <br />
                <br />
                <div>
                    <uc1:Help runat="server" ID="Help2" Titulo="Cliente:" Mensaje="Cliente" />
                    <asp:RequiredFieldValidator ID="RequiredCliente" runat="server" ControlToValidate="CKCliente" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Cliente") %>' Font-Bold="True" ForeColor="Red" ToolTip="Requerido" Style="position: relative; top: 10px" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator><br />
                    <div class="childContainer">
                        <CKEditor:CKEditorControl ID="CKCliente" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                    </div>
                    <br />
                    <div id="divConsumidores" runat="server" visible="false">
                        <uc1:Help runat="server" ID="Help1" Titulo="Consumidores:" Mensaje="Consumidores" />
                        <asp:RequiredFieldValidator ID="RequiredConsumidores" runat="server" ControlToValidate="CKConsumidores" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Consumidores") %>' Font-Bold="True" ForeColor="Red" ToolTip="Requerido" Style="position: relative; top: 10px" ValidationGroup="Requerido" Display="None" Font-Italic="False"></asp:RequiredFieldValidator><br />
                        <div class="childContainer">
                            <CKEditor:CKEditorControl ID="CKConsumidores" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        </div>
                        <br />
                    </div>
                    <blockquote id="bqValidation">
                        <asp:ValidationSummary ID="ValidationSummary" runat="server" ValidationGroup="Requerido" HeaderText="Advertencia:" ForeColor="Red" Font-Italic="True" />
                    </blockquote>
                    <br />
                    <div runat="server" style="text-align: center" visible='<%# AllowUpdate %>'>
                        <asp:Button ID="btnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="btnLimpiarCampos_Click" />&nbsp;<asp:Button ID="btnGuardarProtagonista" runat="server" Text="Guardar" OnClick="btnGuardarProtagonista_Click" ValidationGroup="Requerido" OnClientClick="SummaryFocus(1200);" />
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                        <ProgressTemplate>
                            <div style="text-align: right"><b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;</div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <br />
                    <br />
                </div>
                <asp:HiddenField ID="HiddenWidth" runat="server" />
                <script>
                    $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
                </script>
            </div>
        </div>
    </form>
</body>
</html>

