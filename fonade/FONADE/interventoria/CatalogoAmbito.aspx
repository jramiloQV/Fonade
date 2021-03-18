<%@ Page Title="FONDO EMPRENDER - AMBITO" Language="C#" MasterPageFile="~/Master.Master"
    AutoEventWireup="true" CodeBehind="CatalogoAmbito.aspx.cs" Inherits="Fonade.FONADE.interventoria.CatalogoAmbito1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2
        {
            height: 22px;
        }
    </style>
    <script type="text/javascript">
        function alerta() {
            return confirm('Desea borrar el ambito ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table class="style10">
        <tr>
            <td>
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Text="ÁMBITO" />
                </h1>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:ImageButton ID="ibtn_crearAmbito" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                        OnClick="ibtn_crearAmbito_Click" />
                    &nbsp;<asp:LinkButton ID="lbtn_crearAmbito" runat="server" Text="Adicionar Ámbito"
                        OnClick="lbtn_crearAmbito_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvcAmbito" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
            AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador" OnSorting="gvcAmbitoSorting"
            OnRowCommand="gvcAmbitoRowCommand" OnPageIndexChanging="gvcAmbitoPageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Id">
                    <ItemTemplate>
                        <asp:Label ID="lblidproyecto" runat="server" Text='<%# Eval("id_Ambito") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre" SortExpression="Ambito">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkproyecto" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_Ambito") %>'
                            CommandName="editacontacto" Text='<%# Eval("nomAmbito") %>' />
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
        <table border="0" cellpadding="0" cellspacing="0">
            <tr valign="top">
                <td>
                    <asp:TextBox ID="txt_Nombre" runat="server" BackColor="White" Enabled="true" Text=""
                        MaxLength="255" Width="319px" Height="18px" />
                    &nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_Nombre"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Debe digitar el nombre del ámbito</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:DropDownList ID="ddlAmbito" runat="server" Width="251px" Height="16px" />
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
        <table>
            <tr valign="top">
                <td>
                    <asp:Label ID="lbl_id_ambito" Text="Id: " runat="server" Font-Bold="true" />
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:TextBox ID="txtNomUpd" runat="server" BackColor="White" Enabled="true" Text=""
                        Width="319px" Height="18px" MaxLength="255" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNomUpd"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Debe digitar el nombre del ámbito</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:DropDownList ID="ddlTipo" runat="server" Width="251px" Height="16px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="Btnupdate" runat="server" Text="Actualizar" OnClick="btn_update_Click" />
                    <asp:Button ID="Btnupdate0" runat="server" OnClick="btn_Delete_Click" Text="Borrar"
                        OnClientClick="return alerta()" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
