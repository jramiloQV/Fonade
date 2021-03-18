<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdministrarActasSeguimiento.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.AdministrarActasSeguimiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="lblTitulo" runat="server"
        Text="Administrar Actas de Seguimiento" Font-Size="Large"></asp:Label>
    <hr />
    <div>
        <asp:GridView ID="gvMain" runat="server" AllowPaging="false" AutoGenerateColumns="False"
            EmptyDataText="No existen actas de seguimiento." Width="98%" BorderWidth="0"
            CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
            DataKeyNames="codProyecto" OnRowCommand="gvMain_RowCommand"
            ShowHeaderWhenEmpty="true">
            <Columns>                
                <asp:BoundField HeaderText="Interventor" DataField="nombreInterventor" HtmlEncode="false" />
                <asp:BoundField HeaderText="Proyecto" DataField="codProyecto" HtmlEncode="false" />                           
                <asp:TemplateField HeaderText="Accion">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("codProyecto") %>'
                        CommandName="Administrar" CausesValidation="False" Text='Administrar' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    </div>
</asp:Content>
