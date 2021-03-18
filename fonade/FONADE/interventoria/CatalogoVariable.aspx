<%@ Page Title="FONDO EMPRENDER - VARIABLE" Language="C#" MasterPageFile="~/Master.Master"
    AutoEventWireup="true" CodeBehind="CatalogoVariable.aspx.cs" Inherits="Fonade.FONADE.interventoria.CatalogoVariable" %>

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
            return confirm('Desea borrar la variable ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table class="style10">
        <tr>
            <td>
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Text="VARIABLE" />
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
                        OnClick="ibtn_crearAmbito_Click" />&nbsp;
                    <asp:LinkButton ID="lbtn_crearAmbito" runat="server" OnClick="lbtn_crearAmbito_Click">Adicionar Variable</asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView ID="gvcAmbito" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador"
            OnSorting="gvcAmbitoSorting" OnRowCommand="gvcAmbitoRowCommand" OnPageIndexChanging="gvcAmbitoPageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Id">
                    <ItemTemplate>
                        <asp:Label ID="lblidproyecto" runat="server" Text='<%# Eval("id_Variable") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkproyecto" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_Variable")+ ";"+Eval("CodTipoVariable")  %>'
                            CommandName="editacontacto" Text='<%# Eval("nomVariable") %>' />
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
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Debe digitar el nombre de la variable</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:DropDownList ID="ddlTipoVariable" runat="server" Width="251px" Height="16px" />
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
                    <asp:Label ID="lbl_id_variable" Text="Id: " runat="server" Font-Bold="true" />
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <asp:TextBox ID="txtNomUpd" runat="server" BackColor="White" Enabled="true" Text=""
                        Width="319px" Height="18px" MaxLength="255" />&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNomUpd"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Debe digitar el nombre de la variable</asp:RequiredFieldValidator>
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
                    <asp:Button ID="BtnBorrar" runat="server" OnClick="BtnBorrar_Click" Text="Borrar"
                        OnClientClick="return alerta()" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
