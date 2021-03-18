<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListadoAcreditadores.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.ListadoAcreditadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ScriptManager ID="scrp_1" runat="server" />
    <asp:UpdatePanel ID="up_1_a" runat="server">
        <ContentTemplate>
            <table width="98%" border="0" cellspacing="0" cellpadding="2">
                <tbody>
                    <tr>
                        <td colspan="2">
                            <h1>
                                <asp:Label ID="L_ReportesEvaluacion" runat="server" Text="ACREDITADORES" /></h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:GridView ID="GridView1" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                                AllowPaging="True" AllowSorting="True" DataKeyNames="ID_CONTACTO" Width="100%"
                                OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound"
                                OnSorting="GridView1_Sorting">
                                <RowStyle VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField DataField="ID_CONTACTO" HeaderText="ID" Visible="false" />
                                    <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="42%" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="NOMBRE">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("NOMBRE") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_Nombre" runat="server" Text='<%# Bind("NOMBRE") %>' OnClick="LB_Nombre_Click"
                                                ToolTip="Seleccionar este acreditador" OnClientClick="return confirm('¿Desea seleccionar este usuario como acreditador?')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="E-mail" HeaderStyle-Width="42%" ItemStyle-HorizontalAlign="Left"
                                        SortExpression="EMAIL">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("EMAIL") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_Emaoil" runat="server" Text='<%# Bind("EMAIL") %>' OnClick="LB_Emaoil_Click"
                                                OnClientClick="return confirm('¿Desea seleccionar este usuario como acreditador?')"
                                                ToolTip="Seleccionar este acreditador" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Planes de Negocio" HeaderStyle-Width="16%" ItemStyle-HorizontalAlign="Center"
                                        SortExpression="CANTIDAD">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("CANTIDAD") %>' ToolTip="Ver los planes de negocio relacionados" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LB_Cantidad" runat="server" Text='<%# Bind("CANTIDAD") %>' OnClick="LB_Cantidad_Click"
                                                ToolTip="Ver los planes de negocio relacionados" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <asp:Button ID="B_Volver" runat="server" Text="Volver" PostBackUrl="~/FONADE/evaluacion/AsignarProyectosAcreditadores.aspx" />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="B_Volver" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
