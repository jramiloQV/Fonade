<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FiltroEmprendedorInactivo.aspx.cs"
    Inherits="Fonade.FONADE.AdministrarPerfiles.FiltroEmprendedorInactivo" MasterPageFile="~/Master.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:LinqDataSource ID="lds_emprendedores" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Emprend_Selecting">
    </asp:LinqDataSource>
    <table width="98%" border="0">
        <tr>
            <td class="style50">
                <h1>
                    <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700" /></h1>
            </td>
        </tr>
    </table>
    <table style="width: 63%;">
        <tr>
            <td valign="baseline">
                Filtrar por:
            </td>
            <td valign="baseline">
                <asp:DropDownList ID="FiltroBuscar" runat="server" OnSelectedIndexChanged="FiltroBuscar_SelectedIndexChanged"
                    AutoPostBack="True">
                    <asp:ListItem Value="Todo" Selected="True">Todos</asp:ListItem>
                    <asp:ListItem Value="Nombre">Nombre(s)</asp:ListItem>
                    <asp:ListItem Value="Apellido">Apellido(s)</asp:ListItem>
                    <asp:ListItem Value="Numero">Número de Identificación</asp:ListItem>
                    <asp:ListItem Value="email">Correo Electrónico</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td valign="baseline">
                <asp:TextBox ID="TextBoxfiltro" runat="server" Width="195px" Text="" />
            </td>
            <td valign="bottom">
                <asp:ImageButton ID="Image1" runat="server" Height="33px" ImageUrl="~/Images/buscarrr.png"
                    Width="36px" OnClick="Image1_Click" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                &nbsp;
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="GVEmprendedoresInac" CssClass="Grilla" runat="server" DataSourceID="lds_emprendedores"
                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="Nombres" SortExpression="Nombres">
                            <ItemTemplate>
                                <asp:HyperLink ID="hl_emprend" runat="server" Text='<%#"" + Eval("Nombres") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Apellidos" SortExpression="apellidos">
                            <ItemTemplate>
                                <asp:HyperLink ID="hl_apellidos" runat="server" Text='<%# Eval("apellidos") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Correo Electrónico" SortExpression="email">
                            <ItemTemplate>
                                <asp:HyperLink ID="hl_email" runat="server" Text='<%# Eval("email") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="N° de Identificación" SortExpression="identificacion">
                            <ItemTemplate>
                                <asp:HyperLink ID="hl_identificacion" runat="server" Text='<%# Eval("identificacion") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:HyperLink ID="hl_identificacion" runat="server" Text='Ir a Activación' NavigateUrl='<%# "ActivarEmprendedor.aspx?CodContacto=" + Eval("id_contacto") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style10
        {
            width: 273px;
        }
    </style>
</asp:Content>
