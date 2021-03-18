<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="ProyectosActaConcejoDirectivo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ProyectosActaConcejoDirectivo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="95%" border="0" cellpadding="0" cellspacing="0">
        <tbody>
            <tr>
                <td>
                    <h3><p>Buscar Proyectos de concejo directivo para adicionar al Acta</p></h3>
                </td>
            </tr>
            <tr>
                <td align="center" valign="top" width="98%">
                    <table width="98%" border="0" cellspacing="1" cellpadding="4">
                        <tbody>                                                                             
                            <asp:GridView ID="gvMain" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                                AllowSorting="true" ShowHeaderWhenEmpty="True" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="#" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckProyecto" runat="server" />
                                            <asp:HiddenField ID="hdCodigoProyecto" runat="server" Value='<%# Eval("Id_Proyecto") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Id" DataField="Id_Proyecto" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderText="Nombre" DataField="NomProyecto" ItemStyle-HorizontalAlign="Left" SortExpression="NomProyecto" />
                                </Columns>
                            </asp:GridView>
                            
                            <tr valign="top">
                                <td align="left">
                                    <asp:Label ID="lblError" Text="Sucedio un error inesperado" Visible="False" runat="server" ForeColor="Red" />
                                    <asp:Button ID="btnAdd" Text="Adicionar" runat="server" OnClick="btn_Adicionar_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
