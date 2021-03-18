<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UnidadesDeEmprendimiento.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.UnidadEmprendimiento.UnidadesDeEmprendimiento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
     <h1>
                    <asp:Label ID="lblTitulo" runat="server" Text="Unidades de emprendimiento" />
                </h1>
                <asp:Label ID="lblError" runat="server" Text="" Style="color: red;" />
                <table border="0" style="width: 100%;">
                    <tr>
                        <td style="text-align: left">                            
                            <asp:ImageButton ID="imgNuevaUnidadEmprendimiento" runat="server" ImageUrl="../../../Images/icoAdicionarUsuario.gif" Style="cursor: pointer;" OnClick="lnkNuevaUnidadEmprendimiento_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lnkNuevaUnidadEmprendimiento" runat="server" Text="Adicionar Unidad de emprendimiento" OnClick="lnkNuevaUnidadEmprendimiento_Click" />
                        </td>
                    </tr>                    
                    <tr>
                        <td style="text-align: center">
                            <asp:GridView ID="gvUnidadesEmprendimiento" runat="server" Width="98%" AutoGenerateColumns="false" DataSourceID="dsUnidadEmprendimiento"
                                CssClass="Grilla" AllowSorting="false" ShowHeaderWhenEmpty="true" BorderWidth="1" CellSpacing="1" CellPadding="4" EmptyDataText="No hay unidades de emprendimiento"
                                PageSize="20" OnRowCommand="gvUnidadesEmprendimiento_RowCommand" >
                                <PagerStyle CssClass="Paginador" />
                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField HeaderText="Id" DataField="Id" Visible="false" />
                                    <asp:TemplateField HeaderStyle-Width="3%" Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDeleteInstitucion" ImageUrl="../../../Images/icoBorrar.gif" runat="server" CausesValidation="false" CommandName="inactivar" CommandArgument='<%# Eval("Id")%>' Visible='<%# !(bool)Eval("Estado")%>' AlternateText="Inactivar" />
                                            <asp:ImageButton ID="ImageButton1" ImageUrl="../../../Images/icoActivar.gif" runat="server" CausesValidation="false" CommandName="activar" CommandArgument='<%# Eval("Id")%>' Visible='<%# (bool)Eval("Estado")%>' AlternateText="Activar" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre Unidad" ControlStyle-CssClass="miClase" SortExpression="NombreUnidad" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEditarUnidadEmprendimiento" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id")%>' CommandName="updateUnidad" Text='<%# Eval("NombreUnidad")+ " "  +"("+Eval("NombreInstitucion")+")" %>' />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="miClase" />
                                        <HeaderStyle Width="40%" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Tipo" DataField="TipoInstitucion" SortExpression="TipoInstitucion"
                                        HeaderStyle-Width="30%">
                                        <HeaderStyle Width="30%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Estado" ItemStyle-ForeColor="Blue" HeaderStyle-Width="27%">
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lblEstado" Text='<%# Eval("EstadoFormated") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="27%" />
                                        <ItemStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
        <asp:ObjectDataSource ID="dsUnidadEmprendimiento" runat="server" EnablePaging="false" SelectMethod="getUnidadesEmprendimiento"
        SelectCountMethod="getUnidadesEmprendimientoCount" TypeName="Fonade.FONADE.JefeUnidad.UnidadesDeEmprendimiento" >
    </asp:ObjectDataSource>
</asp:Content>
