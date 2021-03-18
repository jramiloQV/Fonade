<%@ Page Title="FONADE - CUMPLIMIENTO A VERIFICAR" Language="C#" MasterPageFile="~/Master.Master"
    AutoEventWireup="true" CodeBehind="CatalogoCriterioDetalle.aspx.cs" Inherits="Fonade.FONADE.interventoria.CatalogoCriterioDetalle" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function alerta() {
            return confirm('Desea borrar el tipo de ambito ?');
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table class="style10">
        <tr>
            <td>
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Text="Cumplimiento a Verificar" />
                </h1>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:ImageButton ID="ibtn_crearCriterio" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                        OnClick="ibtn_crearCriterio_Click" />&nbsp;
                    <asp:LinkButton ID="lbtn_crearCriterio" runat="server" OnClick="lbtn_crearCriterio_Click">Adicionar Cumplimiento a Verificar</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvcCriterio" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
            AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador" OnSorting="gvcCriterioSorting"
            OnRowCommand="gvcCriterioRowCommand" OnPageIndexChanging="gvcCriterioPageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Id">
                    <ItemTemplate>
                        <asp:Label ID="lblidproyecto" runat="server" Text='<%# Eval("Id_InterventorInformeFinalItem") %>'
                            CommandArgument='<%# Eval("Id_InterventorInformeFinalItem")  %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre" SortExpression="Tipo Ambito">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEmpresa" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_InterventorInformeFinalItem") %>'
                            CommandName="editacontacto" Text='<%# Eval("NomInterventorInformeFinalItem") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
        <br />
        <br />
    </asp:Panel>
    <br />
    <asp:Panel ID="PanelModificar" runat="server" Visible="false">
        <table>
            <tr valign="top">
                <td>
                    <asp:TextBox ID="txt_Nombre" runat="server" BackColor="White" Enabled="true" Text=""
                        Width="319px" Height="18px" MaxLength="255" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_Nombre"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Debe digitar el nombre del cumplimiento a verificar</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:DropDownList ID="ddlTipoVerificacion" runat="server" Width="251px" Height="16px" AutoPostBack="True" />
                </td>
            </tr>
            <tr valign="top">
                <td align="right">
                    <asp:Button ID="btn_actualizar" runat="server" Text="Adicionar" OnClick="btn_actualizar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="PnlActualizar" runat="server" Visible="false">
        <asp:Label ID="lbl_id_TipoAmbito" Text="Id: " runat="server" Font-Bold="true" />
        <table>
            <tr valign="top">
                <td>
                    <asp:TextBox ID="txtNomUpd" runat="server" BackColor="White" Enabled="true" Text=""
                        MaxLength="255" Width="317px" Height="18px" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNomUpd"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Debe digitar el nombre del cumplimiento a verificar</asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <asp:Label ID="Label1" runat="server" Text="Criterio" Visible="false"></asp:Label>
                    <br />
                    <asp:TextBox ID="txtCriterio" runat="server" BackColor="White" Enabled="False" Height="18px" MaxLength="255" Text="" ToolTip="Criterio del cumplimiento a verificar" Width="317px" Visible ="false" />
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:DropDownList ID="ddlTipoVerificacion1" runat="server" Width="251px" Height="16px" AutoPostBack="True" />
                </td>
            </tr>
            <tr valign="top">
                <td align="right">
                    <asp:Button ID="Btnupdate" runat="server" Text="Actualizar" OnClick="btn_update_Click" />
                    <asp:Button ID="Btndelete" runat="server" OnClick="btn_delete_Click" Text="Borrar" Visible="true" OnClientClick = " return confirm('Desea borrar este cumplimiento?');" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
