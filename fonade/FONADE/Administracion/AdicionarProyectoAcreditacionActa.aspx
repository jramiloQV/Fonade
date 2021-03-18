<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="AdicionarProyectoAcreditacionActa.aspx.cs" Inherits="Fonade.FONADE.Administracion.AdicionarProyectoAcreditacionActa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="95%" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td>
                    <h3><p>Buscar Proyectos de Acreditacion para adicionar al Acta</p></h3>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top" width="98%">
                    <table width="98%" border="0" cellspacing="1" cellpadding="4">
                        <tbody>
                            <tr>
                                <td align="left">
                                    &nbsp;&nbsp;
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td colspan="5">
                                                    <b>Buscar por:</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:HiddenField ID="ActaAcreditada" runat="server" />
                                                    Id:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBuscarId" runat="server" />
                                                </td>
                                                <td>
                                                    Nombre:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBuscar" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btn_buscar" Text="Buscar" runat="server" OnClick="btn_buscar_Click" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lnkbtn_opcion_A" Text="A" CssClass="opcionUno" runat="server"
                                        Style="font: bold; color: Blue; text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_B" Text="B" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_C" Text="C" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_D" Text="D" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_E" Text="E" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_F" Text="F" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_G" Text="G" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_H" Text="H" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_I" Text="I" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_J" Text="J" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_K" Text="K" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_L" Text="L" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_M" Text="M" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_N" Text="N" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_O" Text="O" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_P" Text="P" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_Q" Text="Q" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_R" Text="R" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_S" Text="S" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_T" Text="T" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_U" Text="U" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_V" Text="V" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_W" Text="W" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_X" Text="X" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_Y" Text="Y" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <asp:LinkButton ID="lnkbtn_opcion_Z" Text="Z" runat="server" Style="font: bold; color: Blue;
                                        text-decoration: none;" OnClick="lnkbtn_opcion_todos_Click" />
                                    <strong style="font: bold; color: Blue; text-decoration: none;">|</strong>
                                    <asp:LinkButton ID="lnkbtn_opcion_todos" Text="Todos" runat="server" OnClick="lnkbtn_opcion_todos_Click"
                                        Style="font: bold; color: Blue; text-decoration: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="chectodos" runat="server" Text="Seleccionar todos" AutoPostBack="true"
                                        OnCheckedChanged="chectodos_CheckedChanged" />
                                </td>
                            </tr>
                            <asp:GridView ID="gvPlanesNegocio" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                                AllowSorting="true" ShowHeaderWhenEmpty="True" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="#" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chckplanopera" runat="server" />
                                            <asp:HiddenField ID="hdf_proyecto" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Id" DataField="Id_Proyecto" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Nombre" DataField="NomProyecto" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="NomProyecto" />
                                </Columns>
                            </asp:GridView>
                            <tr align="left" valign="top">
                                <td>
                                    <asp:HiddenField ID="numProyectos" runat="server" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="left">
                                    <asp:HiddenField ID="CantidadCheck" runat="server" />
                                    <asp:Button ID="btn_Adicionar" Text="Adicionar" runat="server" OnClick="btn_Adicionar_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
