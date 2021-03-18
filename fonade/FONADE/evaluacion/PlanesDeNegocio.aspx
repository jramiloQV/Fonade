<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="PlanesDeNegocio.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.PlanesDeNegocio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ScriptManager ID="scrp_2" runat="server" />
    <asp:UpdatePanel ID="up_2_a" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tbody>
                    <tr>
                        <td colspan="2">
                            <h1>
                                <asp:Label ID="Label2" runat="server" Text="PLANES DE NEGOCIO" /></h1>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="Label1" runat="server" Text="Planes de negocio asociados a: " />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                OnSorting="GridView1_Sorting">
                                <Columns>
                                    <asp:BoundField DataField="CODIGO" HeaderText="Código" HeaderStyle-Width="15%" SortExpression="CODIGO" />
                                    <asp:BoundField DataField="NOMBRE" HeaderText="Nombre" HeaderStyle-Width="85%" SortExpression="NOMBRE" />
                                </Columns>
                                <PagerSettings Mode="NextPreviousFirstLast" FirstPageText="Inicio" LastPageText="Última página"
                                    NextPageText="Siguiente" PreviousPageText="Anterior" />
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Button ID="B_Volver" runat="server" Text="Volver" PostBackUrl="~/FONADE/evaluacion/ListadoAcreditadores.aspx" />
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
