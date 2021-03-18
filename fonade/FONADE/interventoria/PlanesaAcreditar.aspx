<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="PlanesaAcreditar.aspx.cs" Inherits="Fonade.FONADE.interventoria.PlanesaAcreditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="contentPrincipal" runat="server">
        <h1>
            <label>
                PLANES DE NEGOCIO A ACREDITAR ❀ </label>
        </h1>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gvplanesaacreditar" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                        OnRowCommand="gvplanesaacreditar_RowCommand" EmptyDataText="No existen planes de negocio para acreditar">
                        <Columns>
                            <asp:TemplateField HeaderText="Nombre">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnproyecto" runat="server" Text='<%# Eval("Id_proyecto") + " - " + Eval("NomProyecto") %>'
                                        CssClass="boton_Link_Grid" CommandArgument='<%# Eval("id_proyecto") %>' CommandName="proyectoFrame" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Fecha de Asignación" DataField="FechaAsignacion" />
                            <asp:BoundField HeaderText="Días transcurridos asignación" DataField="Dias" />
                            <asp:BoundField HeaderText="Estado" DataField="Estado" />
                            <asp:TemplateField HeaderText="Acreditar">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Acreditar") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnacreditacion" runat="server" Text="Acreditar" CssClass="boton_Link_Grid"
                                        CommandArgument='<%# Eval("id_proyecto") + ";" + Eval("CodConvocatoria") %>'
                                        CommandName="acreditacion" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
